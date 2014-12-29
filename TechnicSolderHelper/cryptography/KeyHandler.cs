using System;
using System.IO;

namespace TechnicSolderHelper.cryptography
{
    public class KeyHandler
    {
        readonly String _keysPath;
        readonly String _vectorPath;

        public KeyHandler()
        {
            _keysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "keys.dat");
            _vectorPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "vector.dat");

            Random r;
            if (!File.Exists(_keysPath))
            {
                r = new Random();
                for (int i = 0; i < 32; i++)
                {
                    File.AppendAllText(_keysPath, r.Next(0, 255)+Environment.NewLine);
                }
            }
            if (File.Exists(_vectorPath)) return;
            r = new Random();
            for (int i = 0; i < 16; i++)
            {
                File.AppendAllText(_vectorPath, r.Next(0, 255) + Environment.NewLine);
            }
        }

        public byte[] GetKeys()
        {
            byte[] b = new byte[32];
            using (StreamReader reader = new StreamReader(_keysPath))
            {
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = Convert.ToByte(reader.ReadLine());
                }
            }
            return b;
        }

        public byte[] GetVector()
        {
            byte[] b = new byte[16];
            using (StreamReader reader = new StreamReader(_vectorPath))
            {
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = Convert.ToByte(reader.ReadLine());
                }
            }
            return b;
        }
    }
}
