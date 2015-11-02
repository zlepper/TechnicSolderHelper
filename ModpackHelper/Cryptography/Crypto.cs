using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Cryptography
{
    public class Crypto
    {
        private readonly ICryptoTransform encryptorTransform;
        private readonly ICryptoTransform decryptorTransform;
        private readonly UTF8Encoding utfEncoder;

        public Crypto()
        {
            KeyHandler keyhandler = new KeyHandler();
            byte[] key = keyhandler.GetKeys();
            byte[] vector = keyhandler.GetVector();

            //This is our encryption method
            RijndaelManaged rm = new RijndaelManaged();

            //Create an encryptor and a decryptor using our encryption method, key, and vector.
            encryptorTransform = rm.CreateEncryptor(key, vector);
            decryptorTransform = rm.CreateDecryptor(key, vector);

            //Used to translate bytes to text and vice versa
            utfEncoder = new UTF8Encoding();
        }


        /// ----------- The commonly used methods ------------------------------    
        /// Encrypt some text and return a string suitable for passing in a URL.
        public string EncryptToString(string textValue)
        {
            return ByteArrToString(Encrypt(textValue));
        }

        /// Encrypt some text and return an encrypted byte array.
        private byte[] Encrypt(string textValue)
        {
            //Translates our text value into a byte array.
            byte[] bytes = utfEncoder.GetBytes(textValue);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            /*
             * We will have to write the unencrypted bytes to the stream,
             * then read the encrypted result back from the stream.
             */
            #region Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, encryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            #endregion

            #region Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        /// The other side: Decryption methods
        public string DecryptString(string encryptedString)
        {
            return Decrypt(StrToByteArray(encryptedString));
        }

        /// Decryption when working with byte arrays.    
        private string Decrypt(byte[] encryptedValue)
        {
            #region Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, decryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(encryptedValue, 0, encryptedValue.Length);
            decryptStream.FlushFinalBlock();
            #endregion

            #region Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            byte[] decryptedBytes = new byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();
            #endregion
            return utfEncoder.GetString(decryptedBytes);
        }

        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //      return encoding.GetBytes(str);
        // However, this results in character values that cannot be passed in a URL.  So, instead, I just
        // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        private byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                byte val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }

        // Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
        //      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //      return enc.GetString(byteArr);    
        private string ByteArrToString(byte[] byteArr)
        {
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                byte val = byteArr[i];
                if (val < 10)
                    tempStr += "00" + val;
                else if (val < 100)
                    tempStr += "0" + val;
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
    }
}
