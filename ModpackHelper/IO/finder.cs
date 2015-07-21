using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace ModpackHelper.Shared.IO
{
    /// <summary>
    /// Used to find special files
    /// </summary>
    public class Finder
    {
        private readonly IFileSystem fileSystem;

        public Finder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public Finder(): this(fileSystem: new FileSystem()){}

        /// <summary>
        /// Finds all the mods in the specified directory
        /// </summary>
        /// <param name="dir">The directory to search</param>
        /// <returns></returns>
        public List<FileInfoBase> GetModFiles(DirectoryInfoBase dir)
        {
            List<FileInfoBase> files = dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).Where(file => !Regex.IsMatch(file.Name, @"-?[0-9]+,-?[0-9]+.zip")).ToList();
            files.AddRange(dir.EnumerateFiles("*.jar", SearchOption.AllDirectories));
            files.AddRange(dir.EnumerateFiles("*.litemod", SearchOption.AllDirectories));
            files.AddRange(dir.EnumerateFiles("*.disabled", SearchOption.AllDirectories));
            return files;
        }

        /// <summary>
        /// Finds all the mods in the specified directory
        /// </summary>
        /// <param name="dir">The directory to search</param>
        /// <returns></returns>
        public List<FileInfoBase> GetModFiles(string dir)
        {
            return GetModFiles(fileSystem.DirectoryInfo.FromDirectoryName(dir));
        } 
    }
}
