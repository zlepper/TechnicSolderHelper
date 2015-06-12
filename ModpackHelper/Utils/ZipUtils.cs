using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.IO.Abstractions;

namespace ModpackHelper.Utils
{
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

        public void GetInfoFilesFromArchive(string pathToArchive, string directoryToExtractInto)
        {
            FileInfoBase zipFile = fileSystem.FileInfo.FromFileName(pathToArchive);
            if (!zipFile.Exists)
            {
                throw new FileNotFoundException();
            }
            using (Stream filestream = zipFile.OpenRead())
            {
                using (ZipArchive archive = new ZipArchive(filestream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries.Where(entry => entry.FullName.EndsWith(".info", StringComparison.OrdinalIgnoreCase)))
                    {
                        using (Stream f = entry.Open())
                        {
                            string p = Path.Combine(directoryToExtractInto, entry.FullName);
                            FileInfoBase outFile = fileSystem.FileInfo.FromFileName(p);
                            using (Stream o = outFile.Create())
                            {
                                f.CopyTo(o);
                            }
                        }
                    }
                }
            }
        }

        public void ZipDirectory(string folder, string output)
        {
            if (!output.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Output should point to a zip folder");
            }
            FileInfoBase ziparchive = fileSystem.FileInfo.FromFileName(output);
            using (Stream zipFileStream = ziparchive.Create())
            {
                using (ZipArchive zip = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                {
                    foreach (string file in fileSystem.Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                    {
                        FileInfoBase f = fileSystem.FileInfo.FromFileName(file);
                        string entryname = f.FullName.Replace(folder, "");
                        //zip.CreateEntryFromFile(f.FullName, entryname);

                        ZipArchiveEntry entry = zip.CreateEntry(entryname);
                        using (StreamWriter writer = new StreamWriter(entry.Open()) )
                        {
                            using (Stream reader = f.OpenRead())
                            {
                                reader.CopyTo(writer.BaseStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
