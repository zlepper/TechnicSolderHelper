using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.mods;
using ModpackHelper.MinecraftForge;
using Newtonsoft.Json;

namespace ModpackHelper.Mods
{
    public class ModsDBContext : IDisposable
    {
        public List<Mcmod> Mods { get; set; }
        private readonly IFileSystem _fileSystem;

        public readonly string JsonDataFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper",
                "ModsDB.json"); 

        public ModsDBContext() : this(new FileSystem())
        {}

        public ModsDBContext(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;

            if (_fileSystem.File.Exists(JsonDataFile))
            {
                using (Stream s = _fileSystem.File.OpenRead(JsonDataFile))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    Mods = serializer.Deserialize<List<Mcmod>>(reader);
                }
            }
            else
            {
                Mods = new List<Mcmod>();
            }
        }

        public void Save()
        {
            if(!_fileSystem.FileInfo.FromFileName(JsonDataFile).Directory.Exists) _fileSystem.FileInfo.FromFileName(JsonDataFile).Directory.Create();
            using (Stream s = _fileSystem.File.OpenWrite(JsonDataFile))
            using (StreamWriter sw = new StreamWriter(s))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, Mods);
            }
        }


        public void Dispose()
        {
            Save();
        }
    }
}
