using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Caching;
using Newtonsoft.Json;

namespace ModpackHelper.IO
{
    public class ConfigHandler : IDisposable
    {
        private readonly IFileSystem _fileSystem;
        private readonly Dictionary<string, object> _configsDictionary;
        private readonly string _configFilePath;

        /// <summary>
        /// Initialize the confighandler with a special filesystem
        /// </summary>
        /// <param name="fileSystem">The filesystem to initialize with</param>
        public ConfigHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _configFilePath =
                _fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "SolderHelper", "settings.json");
            string fileContens;
            try
            {
                fileContens = _fileSystem.File.ReadAllText(_configFilePath);
            }
            catch (FileNotFoundException)
            {
                fileContens = "";
            }
            catch (DirectoryNotFoundException)
            {
                fileContens = "";
            }
            _configsDictionary = Load(fileContens);
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
        /// <param name="info"></param>
        private void Save(Dictionary<string, object> info)
        {
            string json = JsonConvert.SerializeObject(info);
            _fileSystem.FileInfo.FromFileName(_configFilePath).Directory.Create();
            _fileSystem.File.WriteAllText(_configFilePath, json);
        }

        
        private Dictionary<string, object> Load(string fileContent)
        {
            string json = "{}";
            if (!String.IsNullOrWhiteSpace(fileContent))
            {
                json = fileContent;
            }
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        /// <summary>
        /// Set a specific property to the specified value
        /// </summary>
        /// <param name="key">The key to save the property under</param>
        /// <param name="value">The value to save</param>
        /// <returns>The save value</returns>
        public object SetProperty(string key, object value)
        {
            if (_configsDictionary.ContainsKey(key))
            {
                _configsDictionary[key] = value;
            }
            else
            {
                _configsDictionary.Add(key, value);
            }
            Save(_configsDictionary);
            return value;
        }

        /// <summary>
        /// Gets a specific key from the saved configs.
        /// </summary>
        /// <param name="key">The key to find</param>
        /// <returns>The object that was found</returns>
        /// <exception cref="IndexOutOfRangeException">Throws this if the key is not found in the dictionary</exception>
        public object GetProperty(string key)
        {
            if (!_configsDictionary.ContainsKey(key))
            {
                throw new IndexOutOfRangeException("Key not found in dictionary");
            }
            return _configsDictionary[key];
        }

        public void Dispose()
        {
            Save(_configsDictionary);
        }
    }
}
