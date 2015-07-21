using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared
{
    public class MainProcess
    {
        private IFileSystem fileSystem;

        public MainProcess(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool Start(string inputDirectory, string outputDirectory)
        {
            return Start(fileSystem.DirectoryInfo.FromDirectoryName(inputDirectory),
                fileSystem.DirectoryInfo.FromDirectoryName(outputDirectory));
        }

        public bool Start(DirectoryInfoBase inputDirectory, DirectoryInfoBase outputDirectory)
        {
            
        }
    }
}
