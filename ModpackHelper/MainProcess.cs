using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModpackHelper.IO;
using ModpackHelper.mods;
using ModpackHelper.Mods;
using ModpackHelper.Shared.IO;
using ModpackHelper.Shared.Mods;

namespace ModpackHelper.Shared
{
    public delegate void WorkModChangedEventHandler(string filename);

    public delegate void SerializationExceptionOccuredEventHandler(string filename);

    public class MainProcess
    {
        private readonly IFileSystem fileSystem;
        public event WorkModChangedEventHandler WorkingModChanged;
        public event SerializationExceptionOccuredEventHandler SerializationExceptionOccured;

        protected virtual void OnWorkingModChanged(string filename)
        {
            WorkingModChanged?.Invoke(filename);
        }

        protected virtual void OnSerializationExceptionOccured(string filename)
        {
            SerializationExceptionOccured?.Invoke(filename);
        }

        public MainProcess(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Starts the process of packing all the mods
        /// All data should be validated before this method is called since it contains no validation by itself.
        /// Also: You really shouldn't call this method on the main thread, use a background worker or a seperate thread. 
        /// </summary>
        /// <param name="inputDirectory"></param>
        /// <param name="outputDirectory"></param>
        /// <returns></returns>
        public List<Mcmod> FindAllMods(string inputDirectory, string outputDirectory)
        {
            return FindAllMods(fileSystem.DirectoryInfo.FromDirectoryName(inputDirectory),
                fileSystem.DirectoryInfo.FromDirectoryName(outputDirectory));
        }

        /// <summary>
        /// Starts the process of packing all the mods
        /// All data should be validated before this method is called since it contains no validation by itself.
        /// Also: You really shouldn't call this method on the main thread, use a background worker or a seperate thread. 
        /// </summary> 
        public List<Mcmod> FindAllMods(DirectoryInfoBase inputDirectory, DirectoryInfoBase outputDirectory)
        {
            // Find the mod files in the input directory
            Finder finder = new Finder(fileSystem);
            List<FileInfoBase> modFiles = finder.GetModFiles(inputDirectory);

            // Used to take care of all the io we will be doing
            IOHandler ioHandler = new IOHandler(fileSystem);
            ZipUtils zipUtils = new ZipUtils(fileSystem);
            ModExtractor modExtractor = new ModExtractor(fileSystem);

            // Create the list we will return
            List<Mcmod> mods = new List<Mcmod>(modFiles.Count);

            List<BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>();

            // Context for the local mods database
            using (ModsDBContext modsDB = new ModsDBContext(fileSystem))
            {
                // Walk through all the found modfiles
                foreach (FileInfoBase modFile in modFiles)
                {
                    // Create a background worker, so we can work in multiple threads and speedup what we are doing
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (sender, args) =>
                    {
                        string filename = modFile.Name;

                        // Update any event listernes, such as the interface
                        OnWorkingModChanged(filename);

                        // Check is the mod should be handled in a special way, 
                        //fx liteloader requires a special way of finding the mod details
                        int modAction = Mcmod.IsSpecialHandledMod(filename);
                        if (modAction == 0)
                        {
                            return;
                        }

                        // Check if we already have this mod stored in the Database
                        string md5Value = ioHandler.CalculateMd5(modFile);
                        // ReSharper disable once AccessToDisposedClosure
                        Mcmod mod = modsDB.Mods.SingleOrDefault(m => m.JarMd5.Equals(md5Value));
                        if (mod == null)
                        {
                            // We don't know about that specific mod file

                            // Create a temporary folder where we can do stuff without messing up other things 
                            // In theory anyway
                            DirectoryInfoBase tempOutputFolder = fileSystem.DirectoryInfo.FromDirectoryName(fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), "ModpackHelper",
                                Guid.NewGuid().ToString()));

                            // Extract all the .info files
                            List<FileInfoBase> infoFiles = zipUtils.GetInfoFilesFromArchive(modFile, tempOutputFolder);

                            // Check if we actually extracted any .info files
                            if (infoFiles.Count == 0)
                            {
                                // Since there wasn't extracted any files, then we don't know 
                                // anything about the file, and should just return and emptry file
                                mod = new Mcmod();
                            }

                            // Remove dependencies.info files, since we can't use them for anything at all. 
                            foreach (FileInfoBase infoFile in infoFiles)
                            {
                                // Attempt to turn the .info files into info Mcmod objects
                                try
                                {
                                    mod = modExtractor.GetMcmodDataFromFile(infoFile);
                                }
                                catch (SerializationException)
                                {
                                    // Inform the caller that we couldn't parse this file info an Mcmod for some reason
                                    OnSerializationExceptionOccured(modFile.FullName);
                                }
                            }
                            // Clear the temporary setup
                            tempOutputFolder.Delete(true);
                        }
                        mod?.SetPath(modFile); // Done to ensure we can actually find the file again
                        mods.Add(mod); // Save back to our return values
                    };
                    // Save the background worker so we can find it later
                    backgroundWorkers.Add(bw);
                    // Start the background worker
                    bw.RunWorkerAsync();
                }
                // Make sure all backgroundworkers are finished running before returning to the caller
                while (backgroundWorkers.Count > 0)
                {
                    // Remove background workers that are done
                    backgroundWorkers.RemoveAll(b => !b.IsBusy);
                    Thread.Sleep(30);
                }

            }
            return mods;
        }
    }
}
