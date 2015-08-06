using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Cryptography
{
    public class KeyHandler
    {
        readonly string keysPath;
        readonly string vectorPath;
        private readonly IFileSystem fileSystem;

        public KeyHandler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;

            keysPath = fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "keys.dat");
            vectorPath = fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "vector.dat");

            Random r;
            if (!fileSystem.File.Exists(keysPath))
            {
                r = new Random();
                for (int i = 0; i < 32; i++)
                {
                    fileSystem.File.AppendAllText(keysPath, r.Next(0, 255) + Environment.NewLine);
                }
            }
            if (fileSystem.File.Exists(vectorPath)) return;
            r = new Random();
            for (int i = 0; i < 16; i++)
            {
                fileSystem.File.AppendAllText(vectorPath, r.Next(0, 255) + Environment.NewLine);
            }
        }

        public KeyHandler():this(new FileSystem())
        {
            
        }

        public byte[] GetKeys()
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

        public byte[] GetVector()
        {
            byte[] b = new byte[16];
            using (StreamReader reader = new StreamReader(vectorPath))
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
