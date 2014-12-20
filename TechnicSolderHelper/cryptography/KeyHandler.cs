using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TechnicSolderHelper.cryptography
{
    public class KeyHandler
    {
        String keysPath;
        String VectorPath;

        public KeyHandler()
        {
            keysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "keys.dat");
            VectorPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "vector.dat");

            if (!File.Exists(keysPath))
            {
                Random r = new Random();
                for (int i = 0; i < 32; i++)
                {
                    File.AppendAllText(keysPath, r.Next(0, 255).ToString()+Environment.NewLine);
                }
            }
            if (!File.Exists(VectorPath))
            {
                Random r = new Random();
                for (int i = 0; i < 16; i++)
                {
                    File.AppendAllText(VectorPath, r.Next(0, 255).ToString() + Environment.NewLine);
                }
            }
        }

        public byte[] getKeys()
        {
            byte[] b = new byte[32];
            using (StreamReader reader = new StreamReader(keysPath))
            {
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = Convert.ToByte(reader.ReadLine());
                }
            }
            return b;
        }

        public byte[] getVector()
        {
            byte[] b = new byte[16];
            using (StreamReader reader = new StreamReader(VectorPath))
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
