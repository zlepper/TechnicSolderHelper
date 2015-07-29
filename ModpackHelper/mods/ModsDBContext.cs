using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using ModpackHelper.mods;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Mods
{
    /// <summary>
    /// Used to manage all the saved mod data
    /// </summary>
    public class ModsDBContext : IDisposable
    {
        /// <summary>
        /// The saved mods
        /// </summary>
        public List<Mcmod> Mods { get; set; }
        private readonly IFileSystem fileSystem;
        /// <summary>
        /// Location of the data file
        /// </summary>
        public readonly string JsonDataFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper",
                "ModsDB.json"); 

        public ModsDBContext() : this(new FileSystem())
        {}

        public ModsDBContext(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            Mods = new List<Mcmod>();
            if (this.fileSystem.File.Exists(JsonDataFile))
            {
                using (Stream s = this.fileSystem.File.OpenRead(JsonDataFile))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    Mods = serializer.Deserialize<List<Mcmod>>(reader);
                }
            }
        }

        /// <summary>
        /// Saves the mod data to disk
        /// </summary>
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

        public static void SaveNewData(IFileSystem fileSystem, List<Mcmod> mods)
        {
            using (ModsDBContext db = new ModsDBContext(fileSystem))
            {
                foreach (Mcmod mod in mods)
                {
                    db.Mods.RemoveAll(m => m.JarMd5.Equals(mod.JarMd5));
                }
                db.Mods.AddRange(mods);
            }
        }

        public List<string> GetSuggestedModAuthors(Mcmod mod)
        {
            List<Mcmod> options = Mods.Where(m => m.Modid.Equals(mod.Modid)).ToList();
            return options.Any() ? options.Last().AuthorList : new List<string>();
        }
    }
}
