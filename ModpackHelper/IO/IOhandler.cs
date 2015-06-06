using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.IO
{
    public class IOHandler
    {
        private readonly IFileSystem fileSystem;

        // Method required for testing
        public IOHandler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
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
    }
}
