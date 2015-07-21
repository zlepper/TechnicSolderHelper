using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.IO;
using ModpackHelper.mods;

namespace ModpackHelper.Shared.Mods
{
    public class ModExtractor
    {
        private readonly IFileSystem _fileSystem;

        public ModExtractor(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ModExtractor() : this(fileSystem: new FileSystem())
        {
            
        }

        public Mcmod GetMcmodDataFromFile(string pathToJson)
        {
            string json = new IOHandler(_fileSystem).ReadJson(pathToJson);

            return GetMcmodDataFromJson(json);
        }

        public Mcmod GetMcmodDataFromFile(FileInfoBase pathToJson)
        {
            return GetMcmodDataFromFile(pathToJson.FullName));
        }

        public static Mcmod GetMcmodDataFromJson(string json)
        {
            // Try to convert the json into an mcmod
            Mcmod mod = Mcmod.GetMcmod(json);

            // Try other formats
            if (mod == null)
            {
                Mcmod2 mcmod2 = Mcmod2.GetMcmod2(json);
                if (mcmod2 == null)
                {
                    Litemod litemod = Litemod.GetLitemod(json);
                    if (litemod != null)
                    {
                        mod = litemod.ToMcmod();
                    }
                }
                else
                {
                    mod = mcmod2.ToMcmod();
                }
            }

            // No matching format was found for the mod.
            if (mod == null)
            {
                throw new SerializationException("I can't turn that into anything useful");
            }

            return mod;
        }
    }
}
