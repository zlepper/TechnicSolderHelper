using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;

namespace ModpackHelper.Shared.IO
{
    /// <summary>
    /// Used to handle special zipping actions
    /// </summary>
    public class ZipUtils
    {
        private readonly IFileSystem fileSystem;

        public ZipUtils(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem ?? new FileSystem();
        }

        public ZipUtils()
            : this(fileSystem: new FileSystem())
        {

        }

        /// <summary>
        /// Grabs all the .info files int the specified zip folder an extracts them to the specified destination
        /// </summary>
        /// <param name="pathToArchive"></param>
        /// <param name="directoryToExtractInto"></param>
        /// <returns>A list of all the extracted files</returns>
        public List<FileInfoBase> GetInfoFilesFromArchive(string pathToArchive, string directoryToExtractInto)
        {
            FileInfoBase zipFile = fileSystem.FileInfo.FromFileName(pathToArchive);
            DirectoryInfoBase dir = fileSystem.DirectoryInfo.FromDirectoryName(directoryToExtractInto);
            return GetInfoFilesFromArchive(zipFile, dir);
        }
        /// <summary>
        /// Grabs all the .info files int the specified zip folder an extracts them to the specified destination
        /// </summary>
        /// <param name="pathToArchive"></param>
        /// <param name="directoryToExtractTo"></param>
        /// <returns>A list of all the extracted files</returns>
        public List<FileInfoBase> GetInfoFilesFromArchive(FileInfoBase pathToArchive, DirectoryInfoBase directoryToExtractTo)
        {
            // Validate the zip files existence
            if (!pathToArchive.Exists)
            {
                throw new FileNotFoundException();
            }
            // Create the output directory is it doesn't exist
            if (!directoryToExtractTo.Exists)
            {
                directoryToExtractTo.Create();
            }
            List<FileInfoBase> outputFiles = new List<FileInfoBase>();
            // Read from the zip file
            using (Stream filestream = pathToArchive.OpenRead())
            {
                // Create a zip archive to work through
                using (ZipArchive archive = new ZipArchive(filestream))
                {
                    // Iterate over all the entries in the zip file that's end in .info or .json
                    // And skip and dependencies files, since we can't use them anyway
                    foreach (ZipArchiveEntry entry in archive.Entries.Where(entry => entry.Name.EndsWith(".info", StringComparison.OrdinalIgnoreCase) || entry.Name.Equals("litemod.json")).Where(entry => !entry.Name.Contains("dependancies") && !entry.Name.Contains("dependencies")))
                    {
                        // Begin reading the file
                        using (Stream f = entry.Open())
                        {
                            // Create the output file
                            string p = Path.Combine(directoryToExtractTo.FullName, entry.Name);
                            FileInfoBase outFile = fileSystem.FileInfo.FromFileName(p);
                            using (Stream o = outFile.Create())
                            {
                                // Write to the output file
                                f.CopyTo(o);
                            }
                            outputFiles.Add(outFile);
                        }
                    }
                }
            }
            return outputFiles;
        }

        /// <summary>
        /// Puts all the files in the specified input folder into the output file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="output"></param>
        public void ZipDirectory(string folder, string output)
        {
            DirectoryInfoBase f = fileSystem.DirectoryInfo.FromDirectoryName(folder);
            FileInfoBase o = fileSystem.FileInfo.FromFileName(output);
            ZipDirectory(f, o);
        }

        /// <summary>
        /// Puts all the files in the specified input folder into the output file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="zipArchive"></param>
        public void ZipDirectory(DirectoryInfoBase folder, FileInfoBase zipArchive)
        {
            // Check if the output file actually is a zipfile
            if (!zipArchive.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Output should point to a zip folder");
            }
            // Create a stream for the zipfile
            using (Stream zipFileStream = zipArchive.Create())
            {
                using (ZipArchive zip = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                {
                    // Find all the files in the inputfolder
                    foreach (FileInfoBase file in folder.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        string entryname = file.FullName.Replace(folder.FullName, "");

                        ZipArchiveEntry entry = zip.CreateEntry(entryname);
                        // Write that specific entry to the zipfile
                        using (StreamWriter writer = new StreamWriter(entry.Open()))
                        {
                            using (Stream reader = file.OpenRead())
                            {
                                reader.CopyTo(writer.BaseStream);
                            }
                        }
                    }
                }
            }
        }

        public void SpecialPackSolderMod(FileInfoBase modfile, FileInfoBase zipFile)
        {
            // Check is the zip file is actually a zip file
            if (!zipFile.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Zip file should be a zip file.");
            }
            // Create a stream for the zip file
            using (Stream zipFileStream = zipFile.Create())
            {
                using (ZipArchive zip = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                {
                    string entryName = "mods/" + modfile.Name;
                    ZipArchiveEntry entry = zip.CreateEntry(entryName);

                    using (StreamWriter writer = new StreamWriter(entry.Open()))
                    {
                        using (Stream reader = modfile.OpenRead())
                        {
                            reader.CopyTo(writer.BaseStream);
                        }
                    }
                }
            }

        }
    }
}
