using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace TechnicSolderHelper
{
    /// <summary>
    /// Used to handle special zipping actions
    /// </summary>
    public class ZipUtils
    {
        public ZipUtils()
        {

        }

        /// <summary>
        /// Grabs all the .info files int the specified zip folder an extracts them to the specified destination
        /// </summary>
        /// <param name="pathToArchive"></param>
        /// <param name="directoryToExtractInto"></param>
        /// <returns>A list of all the extracted files</returns>
        public List<FileInfo> GetInfoFilesFromArchive(string pathToArchive, string directoryToExtractInto)
        {
            FileInfo zipFile = new FileInfo(pathToArchive);
            DirectoryInfo dir = new DirectoryInfo(directoryToExtractInto);
            return GetInfoFilesFromArchive(zipFile, dir);
        }

        /// <summary>
        /// Grabs all the .info files int the specified zip folder an extracts them to the specified destination
        /// </summary>
        /// <param name="pathToArchive"></param>
        /// <param name="directoryToExtractTo"></param>
        /// <returns>A list of all the extracted files</returns>
        public List<FileInfo> GetInfoFilesFromArchive(FileInfo pathToArchive, DirectoryInfo directoryToExtractTo)
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
            List<FileInfo> outputFiles = new List<FileInfo>();
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
                            FileInfo outFile = new FileInfo(p);
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
        /// Reads the text in the first valid file found in the zip file
        /// Should be less IO heavy than the above method, though in testing they seem 
        /// to be the same speed
        /// </summary>
        /// <param name="pathToArchive"></param>
        /// <returns></returns>
        public string ReadFromInfoFileInArchive(FileInfo pathToArchive)
        {
            // Validate the zip files existence
            if (!pathToArchive.Exists)
            {
                throw new FileNotFoundException();
            }
            List<FileInfo> outputFiles = new List<FileInfo>();
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
                            // Read directly from the file stream, this is much faster than actually writing it to disk first
                            using (StreamReader reader = new StreamReader(f))
                            {
                                // Read the text from the file and send it back
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            // If this happens, then we couldn't find a file
            return null;
        }

        /// <summary>
        /// Puts all the files in the specified input folder into the output file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="output"></param>
        /// <param name="blacklistedFileNames">A list of gile names to avoid packing. Acacepts wildcards at start and end</param>
        public void ZipDirectory(string folder, string output, List<string> blacklistedFileNames = null)
        {
            DirectoryInfo f = new DirectoryInfo(folder);
            FileInfo o = new FileInfo(output);
            ZipDirectory(f, o, blacklistedFileNames);
        }

        /// <summary>
        /// Puts all the files in the specified input folder into the output file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="zipArchive"></param>
        /// <param name="blacklistedFileNames">A list of file names to avoid packing. Accepts wildcards at start and end</param>
        public void ZipDirectory(DirectoryInfo folder, FileInfo zipArchive, List<string> blacklistedFileNames = null)
        {
            // Make sure we can iterate over the list, even if the user didn't specify it
            if (blacklistedFileNames == null)
            {
                blacklistedFileNames = new List<string>();
            }

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
                    foreach (FileInfo file in folder.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        // Check if the file is in the blackList, and skip over it
                        if (IsFileInBlacklist(blacklistedFileNames, file))
                        {
                            continue;
                        }


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

        /// <summary>
        /// Checks if the file should be skipped 
        /// </summary>
        /// <param name="blacklistedFileNames"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFileInBlacklist(List<string> blacklistedFileNames, FileInfo file)
        {
            var name = file.Name;
            for (int i = 0; i < blacklistedFileNames.Count; i++)
            {
                string blacklistedFileName = blacklistedFileNames[i];
                if (blacklistedFileName.StartsWith("*"))
                {
                    blacklistedFileName = blacklistedFileName.Replace("*", "");
                    if (name.EndsWith(blacklistedFileName)) return true;
                }
                if (blacklistedFileName.EndsWith("*"))
                {
                    blacklistedFileName = blacklistedFileName.Replace("*", "");
                    if (name.StartsWith(blacklistedFileName)) return true;
                }
                if (name.Equals(blacklistedFileName)) return true;
            }
            return false;
        }

        public void SpecialPackSolderMod(FileInfo modfile, FileInfo zipFile)
        {
            // Check is the zip file is actually a zip file
            if (!zipFile.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Zip file should be a zip file.");
            }
            // Create a stream for the zip file
            using (Stream zipFileStream = File.Create(zipFile.FullName))
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

        public void SpecialPackForgeZip(Stream forgeStream, FileInfo zipFile)
        {
            // Check is the zip file is actually a zip file
            if (!zipFile.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Zip file should be a zip file.");
            }

            // Create the directory the zip file should be put in
            zipFile.Directory.Create();

            // Create a stream for the zip file
            using (Stream zipFileStream = File.Create(zipFile.FullName))
            {
                using (ZipArchive zip = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                {
                    string entryName = "bin/modpack.jar";
                    ZipArchiveEntry entry = zip.CreateEntry(entryName);

                    using (StreamWriter writer = new StreamWriter(entry.Open()))
                    {
                        forgeStream.CopyTo(writer.BaseStream);
                    }
                }
            }
        }
    }
}