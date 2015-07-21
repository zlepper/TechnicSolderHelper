using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.mods;

namespace ModpackHelper.Shared.IO
{
    public class ModPacker
    {
        private readonly IFileSystem fileSystem;

        public ModPacker(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Pack(List<Mcmod> mods, DirectoryInfoBase outputDirectory)
        {
            outputDirectory.Create();
            List<BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>(mods.Count);
            ZipUtils zipUtils = new ZipUtils(fileSystem);

            foreach (Mcmod mod in mods)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (sender, args) =>
                {
                    string modID = mod.Modid.Replace('|', '+').ToLower();
                    string modversion = (mod.Mcversion + "-" + mod.Version).ToLower();
                    // TODO Check if mod is online

                    // Create the output directory 
                    string zipFileDirectory = fileSystem.Path.Combine(outputDirectory.FullName, "mods", modID);
                    fileSystem.Directory.CreateDirectory(zipFileDirectory);

                    string zipFileName = fileSystem.Path.Combine(outputDirectory.FullName, "mods", modID,
                        modID + "-" + modversion + ".zip");
                    FileInfoBase zipFile = fileSystem.FileInfo.FromFileName(zipFileName);

                    Debug.WriteLine("Packing " + modID);
                    zipUtils.SpecialPackSolderMod(mod.GetPath(), zipFile);
                    Debug.WriteLine("Done packing " + modID);
                };
                backgroundWorkers.Add(bw);
                bw.RunWorkerAsync();
            }
            // Make sure all backgroundworkers are finished running before returning to the caller
            while (backgroundWorkers.Count > 0)
            {
                // Remove background workers that are done
                foreach (BackgroundWorker bw in backgroundWorkers.Where(b => !b.IsBusy))
                {
                    bw.Dispose();
                }
                backgroundWorkers.RemoveAll(b => !b.IsBusy);
                Debug.WriteLine(backgroundWorkers.Count + " backgroundworkers remaining.");
            }
        }
    }
}
