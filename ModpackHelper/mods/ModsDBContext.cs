using System;
using System.Collections.Concurrent;
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
        public ConcurrentBag<Mcmod> Mods { get; set; }
        private readonly IFileSystem fileSystem;

        public readonly string JsonDataFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper",
                "ModsDB.json"); 

        public ModsDBContext() : this(new FileSystem())
        {}

        public ModsDBContext(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            Mods = new ConcurrentBag<Mcmod>();
            if (this.fileSystem.File.Exists(JsonDataFile))
            {
                using (Stream s = this.fileSystem.File.OpenRead(JsonDataFile))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    List<Mcmod> m = serializer.Deserialize<List<Mcmod>>(reader);
                    for (int i = 0, count = m.Count; i < count; i++)
                    {
                        Mods.Add(m[i]);
                    }
                }
            }
        }

        public void Save()
        {
            if(!fileSystem.FileInfo.FromFileName(JsonDataFile).Directory.Exists) fileSystem.FileInfo.FromFileName(JsonDataFile).Directory.Create();
            using (Stream s = fileSystem.File.OpenWrite(JsonDataFile))
            using (StreamWriter sw = new StreamWriter(s))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, Mods.ToList());
            }
        }


        public void Dispose()
        {
            Save();
        }
    }
}
