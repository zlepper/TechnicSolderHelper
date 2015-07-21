using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Threading;

namespace ModpackHelper.Shared.IO
{
    /// <summary>
    /// Used to handle IO
    /// </summary>
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

        /// <summary>
        /// Reads the text of the specified file
        /// </summary>
        /// <param name="path">The file to read</param>
        /// <returns>The text of the file</returns>
        public string ReadText(string path)
        {
            if (fileSystem.File.Exists(path))
            {
                return fileSystem.File.ReadAllText(path);
            }
            throw new FileNotFoundException();
        }

        /// <summary>
        /// Reads the text of the specified file
        /// </summary>
        /// <param name="path">The file to read</param>
        /// <returns>The text of the file</returns>
        public string ReadText(FileInfoBase path)
        {
            return ReadText(path.FullName);
        }

        /// <summary>
        /// Calculates the md5 value of a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string CalculateMd5(FileInfoBase file)
        {
            using (MD5 md5 = MD5.Create())
                while (true)
                    try
                    {
                        using (Stream stream = file.OpenRead())
                            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    }
                    catch
                    {
                        Thread.Sleep(100);
                    }
        }
    }
}
