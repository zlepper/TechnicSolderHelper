using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Mods
{
    /// <summary>
    /// The version 2 of mcmod
    /// </summary>
    public class Mcmod2
    {
        //public int Modinfoversion { get; set; }

        public int ModListVersion { get; set; }

        public List<Mcmod> Modlist { get; set; }

        /// <summary>
        /// Converts the mcmod2 into a normal mcmod
        /// </summary>
        /// <returns></returns>
        public Mcmod ToMcmod()
        {
            if (Modlist.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Modlist[0];
        }

        /// <summary>
        /// Get a mod of the mcmod2 format
        /// </summary>
        /// <param name="json">The json to parse</param>
        /// <returns></returns>
        public static Mcmod2 GetMcmod2(string json)
        {
            try
            {
                Mcmod2 m = JsonConvert.DeserializeObject<Mcmod2>(json);
                return m.ModListVersion == 2 ? m : null;
            }
            catch (JsonReaderException)
            {
                return null;
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }
    }
}
