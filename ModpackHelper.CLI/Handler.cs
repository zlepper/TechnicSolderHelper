using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using ModpackHelper.Shared.IO;
using ModpackHelper.Shared.MinecraftForge;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Permissions.FTB;
using ModpackHelper.Shared.UserInteraction;

namespace ModpackHelper.CLI
{
    /// <summary>
    /// Takes care of userinput from the console
    /// </summary>
    public class Handler
    {
        /// <summary>
        /// A forge handler to check against minecraft and forge versions
        /// </summary>
        private ForgeHandler Fh { get; set; }

        /// <summary>
        ///  The filesystem to work against
        /// </summary>
        private readonly IFileSystem fileSystem;
        /// <summary>
        /// Creates a new handler to take care of userinput
        /// </summary>
        public Handler() : this(new FileSystem()) { }

        /// <summary>
        /// Creates a new handler to take care of userinput
        /// </summary>
        /// <param name="fileSystem">The custom filesystem to use</param>
        public Handler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            // Create the forge handler and grab the latest data of the Minecraft Forge servers
            Fh = new ForgeHandler(fileSystem);
            Fh.DownloadForgeVersions();
        }

        /// <summary>
        /// Indicates if the output directory should be cleared before run
        /// </summary>
        public bool ClearOutputDirectoryOnRun { get; set; }
        /// <summary>
        /// Indicates what the input directory is
        /// </summary>
        public string InputDirectory { get; set; }
        /// <summary>
        /// Indicates what the output directory is
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// Indicates if Modpack Helper should be verbose
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Indicates if all the mods found should be repacked
        /// </summary>
        public bool RepackEverything { get; set; }

        /// <summary>
        /// Indicates the name of the modpack to create
        /// </summary>
        public string ModpackName { get; set; }

        /// <summary>
        /// Indicates the version of the modpack to make
        /// </summary>
        public string ModpackVersion { get; set; }

        /// <summary>
        /// Indicates the minecraft version of the modpack
        /// </summary>
        public string MinecraftVersion { get; set; }

        /// <summary>
        /// Indicates if we should pack config files for the modpack
        /// </summary>
        public bool PackConfigFiles { get; set; }

        /// <summary>
        /// If set: Indicates what version of forge to pack
        /// If not set: Do not pack a forge version
        /// </summary>
        public int ForgeVersionToPack { get; set; }

        /// <summary>
        /// Starts the entire modpacking process
        /// </summary>
        /// <param name="args"></param>
        /// <param name="messageShower"></param>
        /// <returns>True if the process successed, otherwise false</returns>
        public bool Start(List<string> args, IMessageShower messageShower)
        {
            // TODO Write some unit tests for all this... FML
            // Check if the user specified any arguments
            if (args.Count == 0)
            {
                messageShower.ShowMessageAsync(Messages.Usage);
                return false;
            }


            // Check if Modpack Helper should be verbose in it's output
            if (args.Contains("-v"))
            {
                Verbose = true;
                args.Remove("-v");
            }

            // Get the input directory
            if (args.Contains("-i"))
            {
                // Check if the user specified the output directory, 
                // which is needed with the input directory
                if (!args.Contains("-o"))
                {
                    messageShower.ShowMessageAsync(Messages.MissingOutputDirectory);
                    return false;
                }

                int index = args.IndexOf("-i");

                // Check if the the index is the last argument
                // Also done to avoid an out of range exception in the next piece of code
                if (index + 1 == args.Count)
                {
                    messageShower.ShowMessageAsync(Messages.MissingInputDirectory);
                    return false;
                }
                string supposedInputDirectory = args[index + 1];

                // Check if the user didn't specify an input directory
                if (supposedInputDirectory.StartsWith("-"))
                {
                    messageShower.ShowMessageAsync(Messages.MissingInputDirectory);
                    return false;
                }

                // Check to make sure the user specified the 
                // /mods/ directory as input directory
                if (!supposedInputDirectory.EndsWith(fileSystem.Path.DirectorySeparatorChar + "mods"))
                {
                    messageShower.ShowMessageAsync(Messages.NotAModsDirectory);
                    return false;
                }

                // Check to make sure the inputdirectory actually exists
                if (!fileSystem.Directory.Exists(supposedInputDirectory))
                {
                    messageShower.ShowMessageAsync(Messages.InputDirectoryNotFound);
                    return false;
                }

                InputDirectory = supposedInputDirectory;
                args.Remove("-i");
                args.Remove(supposedInputDirectory);
            }

            // Get the output directory
            if (args.Contains("-o"))
            {
                // Check if the input directory was specified
                if (string.IsNullOrWhiteSpace(InputDirectory))
                {
                    messageShower.ShowMessageAsync(Messages.MissingInputDirectory);
                    return false;
                }

                int index = args.IndexOf("-o");

                // Check if the the index is the last argument
                // Also done to avoid an out of range exception in the next piece of code
                if (index + 1 == args.Count)
                {
                    messageShower.ShowMessageAsync(Messages.MissingOutputDirectory);
                    return false;
                }

                string supposedOutputDirectory = args[index + 1];

                // Check if the user didn't specify an input directory
                if (supposedOutputDirectory.StartsWith("-"))
                {
                    messageShower.ShowMessageAsync(Messages.MissingOutputDirectory);
                    return false;
                }

                OutputDirectory = supposedOutputDirectory;
                args.Remove("-o");
                args.Remove(supposedOutputDirectory);
            }

            // Check if we should clear the output directory before run
            if (args.Contains("-c"))
            {
                ClearOutputDirectoryOnRun = true;
                args.Remove("-c");
            }

            // Check if everything should be repacked
            if (args.Contains("-r"))
            {
                RepackEverything = true;
                args.Remove("-r");
            }


            // Gets the modpack name
            if (args.Contains("-Mn"))
            {
                // Make sure there is an argument after this one
                int index = args.IndexOf("-Mn");
                if (index + 1 == args.Count)
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the modpack name.");
                    return false;
                }

                // Get the modpack name
                ModpackName = args[index + 1];

                if (string.IsNullOrWhiteSpace(ModpackName))
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the modpack name.");
                    return false;
                }

                // Remove the used values from the list
                args.Remove(ModpackName);
                args.Remove("-Mn");
            }
            else
            {
                messageShower.ShowMessageAsync("You have to enter a modpack name with the flag \"-Mn\".");
                return false;
            }

            // Get the modpack version
            if (args.Contains("-Mv"))
            {
                // Make sure there is an argument after this one
                int index = args.IndexOf("-Mv");
                if (index + 1 == args.Count)
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the modpack version");
                    return false;
                }

                // Get the modpack version
                ModpackVersion = args[index + 1];

                if (string.IsNullOrWhiteSpace(ModpackVersion))
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the modpack version");
                    return false;
                }

                // Remove the used values from the list
                args.Remove("-Mv");
                args.Remove(ModpackVersion);
            }
            else
            {
                messageShower.ShowMessageAsync("You have to enter a modpack version with the flag \"-Mv\".");
                return false;
            }

            // Get the Minecraft version
            if (args.Contains("-MCv"))
            {
                // Make sure there is an argument after this one
                int index = args.IndexOf("-MCv");
                if (index + 1 == args.Count)
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the Minecraft version");
                    return false;
                }

                // Get the minecraft version
                MinecraftVersion = args[index + 1];

                if (string.IsNullOrWhiteSpace(ModpackVersion))
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the Minecraft version");
                    return false;
                }

                // Make sure that the minecraft version actually exists
                List<string> validMinecraftVersions = Fh.GetMinecraftVersions();
                if (!validMinecraftVersions.Contains(MinecraftVersion))
                {
                    messageShower.ShowMessageAsync("Unknown Minecraft version, valid versions are:");
                    foreach (string validMinecraftVersion in validMinecraftVersions)
                        messageShower.ShowMessageAsync(validMinecraftVersion);
                    return false;
                }

                // Remove the used values from the list
                args.Remove("-MCv");
                args.Remove(MinecraftVersion);
            }
            else
            {
                messageShower.ShowMessageAsync("You have to enter a Minecraft version with the flag \"-MCv\".");
                return false;
            }

            // Check if we should pack config files
            if (args.Contains("-Cfg"))
            {
                PackConfigFiles = true;
                args.Remove("-Cfg");
            }

            // Check if we should pack a forge file (modpack.jar)
            if (args.Contains("-f"))
            {
                // Make sure there is an argument after this one
                int index = args.IndexOf("-f");
                if (index + 1 == args.Count)
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the Forge version");
                    return false;
                }

                // Get the forge version
                int t = -1;
                bool converted = int.TryParse(args[index + 1], out t);
                if (converted)
                {
                    ForgeVersionToPack = t;
                }
                else
                {
                    messageShower.ShowMessageAsync("You have not specified a valid value for the Forge version. It has to be a number");
                    return false;
                }

                // Check that the selected forgeversion actually can be used
                List<int> validForgeVersions = Fh.GetForgeBuilds(MinecraftVersion);
                if (!validForgeVersions.Contains(ForgeVersionToPack))
                {
                    messageShower.ShowMessageAsync("Unknown or invalid forge version for the selected version of Minecraft. Valid versions for Minecraft " + MinecraftVersion + " is:");
                    StringBuilder sb = new StringBuilder();
                    foreach (int validForgeVersion in validForgeVersions)
                        sb.Append(validForgeVersion + "\t");
                    messageShower.ShowMessageAsync(sb.ToString());
                    return false;
                }

                // Remove the used values from the list
                args.Remove("-f");
                args.Remove(ForgeVersionToPack.ToString());
            }

            // Make sure we actually used everything the user entered
            if (args.Any())
            {
                messageShower.ShowMessageAsync("Unknown arguments:");
                // Output anything we couldn't use
                foreach (string arg in args)
                    messageShower.ShowMessageAsync(arg);
                return false;
            }

            // Everything working fine, so we are free to continue
            return true;
        }

        /// <summary>
        /// Actually starts the pack creation progress
        /// </summary>
        /// <param name="messageShower"></param>
        public void Pack(IMessageShower messageShower)
        {
            ModExtractor modExtractor = new ModExtractor(fileSystem);

            // Measure the time it takes to get all the modinfo
            // It's fast by the way ;)
            messageShower.ShowMessageAsync("Extracting mod data");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // Get mod info of the mods in the input directory
            List<Mcmod> mods = modExtractor.FindAllMods(InputDirectory);
            sw.Stop();
            messageShower.ShowMessageAsync("Extracting " + mods.Count + " mods data took: " + sw.Elapsed);
            // Load some FTB data in we can query against for smartness
            PermissionGetter pGetter = new PermissionGetter(fileSystem);
            pGetter.LoadOnlinePermissions();
            // Get the missing mods' info
            GetInfoOnUnknownMods(mods, messageShower);

            // Save the mod data back into the base
            using (ModsDBContext db = new ModsDBContext(fileSystem))
            {
                messageShower.ShowMessageAsync("Saving mod data.");

                // save the mods to the database 
                db.Mods.AddRange(mods);
                db.Save();
            }

            if (ClearOutputDirectoryOnRun)
            {
                int attempts = 0;
                while (attempts < 20)
                {
                    try
                    {
                        Directory.Delete(OutputDirectory, true);
                        break;
                    }
                    catch (Exception)
                    {
                        if (20 - attempts == 0)
                        {
                            messageShower.ShowMessageAsync("Unable to clear output directory, giving up. ");
                            return;
                        }
                        messageShower.ShowMessageAsync("Unable to clear output directory. Trying again in 1 second. " + (20 - attempts) + " attempts left.");
                        Thread.Sleep(1000);
                        attempts++;
                    }
                }
            }

            // Actually pack the mods
            messageShower.ShowMessageAsync("Packing Mods, please stand by, this can take a moment!");
            sw.Restart();
            ModPacker packer = new ModPacker(fileSystem);
            packer.Pack(mods, fileSystem.DirectoryInfo.FromDirectoryName(OutputDirectory));
            sw.Stop();
            messageShower.ShowMessageAsync("packing " + mods.Count + " mods took: " + sw.Elapsed);

            // Output list of all the mods packed
            string html = packer.GetFinishedHTML();
            var p = fileSystem.Path.Combine(OutputDirectory, "mods.html");
            fileSystem.File.WriteAllText(p, html);
            messageShower.ShowMessage("Finished doing everything. There is now a list of the packed mods at: " + p + Environment.NewLine + "Press enter to continue.");
        }

        /// <summary>
        /// Gets all the unknown mods info
        /// </summary>
        /// <param name="mods"></param>
        /// <param name="messageShower"></param>
        private void GetInfoOnUnknownMods(List<Mcmod> mods, IMessageShower messageShower)
        {
            // Find all the mods still missing info
            Mcmod[] missingMods = mods.Where(m => !m.IsValid()).ToArray();
            // Make sure there actually are mods missing info
            if (missingMods.Any())
            {
                messageShower.ShowMessageAsync("Unable to find info for all the mods. Please provide info as requested:");
                // Request data for all the mods missing info
                foreach (Mcmod missingMod in missingMods)
                {
                    if (missingMod.AuthorList == null || !missingMod.AuthorList.Any())
                    {
                        // Try to figure out the authors from some stored data
                        missingMod.AuthorList = missingMod.GetAuthors(fileSystem);
                    }

                    // Get the minecraft version
                    if (string.IsNullOrWhiteSpace(missingMod.Mcversion) || missingMod.Mcversion.ToLower().Contains("example") || missingMod.Mcversion.Contains("${"))
                    {
                        missingMod.Mcversion = MinecraftVersion;
                    }

                    if(missingMod.IsValid()) continue;

                    // Make sure the user knows what file they are dealing with
                    messageShower.ShowMessageAsync("Filename: " + missingMod.GetPath().Name);

                    // Get the mod name
                    // The loop ensures that the user can't continue until they enter something
                    while (string.IsNullOrWhiteSpace(missingMod.Name) || missingMod.Name.ToLower().Contains("example") ||
                        missingMod.Name.Contains("${"))
                    {
                        messageShower.ShowMessageAsync("What is the name of the mod?");
                        missingMod.Name = Console.ReadLine();
                    }

                    // Get the mod version
                    // The loop ensured that the user can't continue until they enter something
                    while (string.IsNullOrWhiteSpace(missingMod.Version) ||
                           missingMod.Version.ToLower().Contains("example") ||
                           missingMod.Version.Contains("${"))
                    {
                        messageShower.ShowMessageAsync("What is the version of the mod?");
                        missingMod.Version = Console.ReadLine();
                    }

                    

                    // Get the modid
                    if (string.IsNullOrWhiteSpace(missingMod.Modid) || missingMod.Modid.ToLower().Contains("example") ||
                        missingMod.Modid.Contains("${"))
                    {
                        missingMod.Modid = missingMod.Name.Replace(' ', '-').Replace('|', '-');
                    }

                    // Get the mods author(s)
                    if (!missingMod.AuthorList.Any())
                    {
                        // Check if we actually found anything
                        while (!missingMod.AuthorList.Any())
                        {
                            messageShower.ShowMessageAsync("Who is the author of \"" + missingMod.Name + "\"?");
                            string authors = Console.ReadLine();

                            // Make sure the user entered something
                            if (string.IsNullOrWhiteSpace(authors)) continue;
                            missingMod.AuthorList.AddRange(authors.Replace(", ", ",").Split(','));
                        }
                    }
                }
            }
        }

    }
}
