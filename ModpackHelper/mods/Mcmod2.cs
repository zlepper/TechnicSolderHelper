using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.mods
{
    /// <summary>
    /// The version 2 of mcmod
    /// </summary>
    public class Mcmod2
    {
        public int Modinfoversion { get; set; }

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
                throw new NoModsStoredException();
            }
            return this.Modlist[0];
        }
    }

    public class NoModsStoredException : Exception
    {

    }
}
