using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Caching;
using Newtonsoft.Json;

namespace ModpackHelper.IO
{
    public class ConfigsHandler : IDisposable
    {
        private readonly IFileSystem _fileSystem;
        private readonly Dictionary<string, object> _configsDictionary;
        private readonly string _configFilePath;
        public ConfigsHandler(IFileSystem fileSystem)
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

        // ReSharper disable once UnusedMember.Global
        public ConfigsHandler()
            : this(fileSystem: new FileSystem())
        {

        }

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
