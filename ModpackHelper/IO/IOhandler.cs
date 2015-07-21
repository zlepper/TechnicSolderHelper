using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModpackHelper.IO
{
    public class IOHandler
    {
        private readonly IFileSystem fileSystem;

        // Method required for testing
        public IOHandler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem ?? new FileSystem();
        }

        public IOHandler() : this(fileSystem: new FileSystem())
        {
            
        }

        public string ReadJson(string path)
        {
            if (fileSystem.File.Exists(path))
            {
                return fileSystem.File.ReadAllText(path);
            }
            throw new FileNotFoundException();
        }

        public string ReadJson(FileInfoBase path)
        {
            return ReadJson(path.ToString());
        }

        /// <summary>
        /// Calculates the md5 value of a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string CalculateMd5(FileInfoBase file)
        {
            using (MD5 md5 = MD5.Create())
            {
                while (true)
                {
                    try
                    {
                        using (Stream stream = file.OpenRead())
                        {
                            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                        }
                    }
                    catch
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
