using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.IO;
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



        public Litemod GetLitemod(string path)
        {
            string json = new IOHandler().ReadJson(path);
            try
            {
                return JsonConvert.DeserializeObject<Litemod>(json);
            }
            catch (JsonReaderException)
            {
                throw new NotLitemodException();
            }
        }
    }

    [Serializable]
    public class NotLitemodException : Exception
    {
        public NotLitemodException()
        {
        }

        public NotLitemodException(string message) : base(message)
        {
        }

        public NotLitemodException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NotLitemodException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
