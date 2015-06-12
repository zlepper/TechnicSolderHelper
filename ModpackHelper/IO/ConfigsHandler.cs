using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModpackHelper.IO
{
    public class ConfigsHandler
    {
        private readonly IFileSystem fileSystem;
        private static Dictionary<string, object> configsDictionary; 
        public ConfigsHandler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ConfigsHandler() : this(fileSystem: new FileSystem())
        {
            
        }

        private void Save(Dictionary<string, object> info)
        {
            if (info == null) return;
            string json = JsonConvert.SerializeObject(configsDictionary);
            fileSystem.File.WriteAllText(
                fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "config.json"), json);
        }

        private Dictionary<string, object> Load()
        {
            string json = "{}";
            try
            {
                json =
                    fileSystem.File.ReadAllText(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "config.json"));
            }
            catch (FileNotFoundException)
            {
            }
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public object SetProperty(string key, object value)
        {
            if (configsDictionary == null)
            {
                configsDictionary = Load();
            }
            if (configsDictionary.ContainsKey(key))
            {
                configsDictionary[key] = value;
            }
            else
            {
                configsDictionary.Add(key, value);
            }
            Save(configsDictionary);
            return value;
        }

        public object GetProperty(string key)
        {
            if (configsDictionary == null)
            {
                configsDictionary = Load();
            }
            if (!configsDictionary.ContainsKey(key))
            {
                throw new IndexOutOfRangeException("Key not found in dictionary");
            }
            return configsDictionary[key];
        }

        /// <summary>
        /// Should never ever fucking ever be called in productions code inless abosolute necesarry. 
        /// Will cause the confighandler to drop any cached data. 
        /// </summary>
        public static void Teardown()
        {
            configsDictionary = null;
        }
    }
}
