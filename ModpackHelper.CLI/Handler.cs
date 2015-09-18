using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using ModpackHelper.Shared.UserInteraction;

namespace ModpackHelper.CLI
{
    /// <summary>
    /// Takes care of userinput from the console
    /// </summary>
    public class Handler
    {
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

                // Get the modpack version
                MinecraftVersion = args[index + 1];

                if (string.IsNullOrWhiteSpace(ModpackVersion))
                {
                    messageShower.ShowMessageAsync("You have not specified a value for the Minecraft version");
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

                // Get the modpack version
                //ForgeVersionToPack = args[index + 1];
                int t = -1;
                bool converted = Int32.TryParse(args[index + 1], out t);
                if (converted)
                {
                    ForgeVersionToPack = t;
                }
                else
                {
                    messageShower.ShowMessageAsync("You have not specified a valid value for the Forge version. It has to be a number");
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
    }
}
