using System;
using System.IO;
using System.IO.Abstractions;
using ModpackHelper.Shared;
using ModpackHelper.Shared.Utils.Config;
using Newtonsoft.Json;

namespace ModpackHelper.IO
{
    public class ConfigHandler : IDisposable
    {
        private readonly IFileSystem fileSystem;
        public readonly Configs Configs;
        private readonly string configFilePath;

        /// <summary>
        /// Initialize the confighandler with a special filesystem
        /// </summary>
        /// <param name="fileSystem">The filesystem to initialize with</param>
        public ConfigHandler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            configFilePath =
                this.fileSystem.Path.Combine(Constants.ApplicationDataPath, "settings.json");
            string fileContens;
            try
            {
                fileContens = this.fileSystem.File.ReadAllText(configFilePath);
            }
            catch (FileNotFoundException)
            {
                fileContens = "";
            }
            catch (DirectoryNotFoundException)
            {
                fileContens = "";
            }
            Configs = Load(fileContens);
        }

        /// <summary>
        /// Create a confighandler
        /// </summary>
        public ConfigHandler()
            : this(fileSystem: new FileSystem())
        {

        }

        /// <summary>
        /// Save the currently stored data to disk
        /// </summary>
        /// <param name="config"></param>
        private void Save(Configs config)
        {
            string json = JsonConvert.SerializeObject(config);
            fileSystem.FileInfo.FromFileName(configFilePath).Directory.Create();
            fileSystem.File.WriteAllText(configFilePath, json);
        }

        /// <summary>
        /// Loads the saved configs from the specified string
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        private Configs Load(string fileContent)
        {
            string json = "{}";
            if (!String.IsNullOrWhiteSpace(fileContent))
            {
                json = fileContent;
            }
            return JsonConvert.DeserializeObject<Configs>(json);
        }

        public void Dispose()
        {
            Save(Configs);
        }
    }
}
