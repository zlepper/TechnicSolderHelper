using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModpackHelper.mods
{
    /// <summary>
    /// Used to descripe a liteloader mod
    /// </summary>
    public class Litemod
    {
        /// <summary>
        /// The name of the liteloader mod
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The minecraft version of the mod
        /// </summary>
        public string Mcversion { get; set; }

        /// <summary>
        /// The version of the mod
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The small revision number
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// The author of the mod
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The description of the mod
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Turns the liteloader mod into an Mcmod
        /// </summary>
        /// <returns></returns>
        public Mcmod ToMcmod()
        {
            return new Mcmod()
            {
                Name = Name,
                Modid = Name.Replace(" ", ""),
                Description = Description,
                Authors = Author.Split(",".ToCharArray()).ToList(),
                Version = Version + "-" + Revision,
                Mcversion = Mcversion
            };
        }

        public static Litemod GetLitemod(string json)
        {
            try
            {
                Litemod m = JsonConvert.DeserializeObject<Litemod>(json);
                return isNull(m) ? null : m;
            }
            catch (JsonSerializationException)
            {
                return null;
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

        public static bool isNull(Litemod m)
        {
            return m.Author == null && m.Description == null && m.Mcversion == null && m.Name == null &&
                   m.Revision == null && m.Version == null;
        }
    }

}
