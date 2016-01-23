using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Web.Solder.Responses
{
    public class Build
    {
        public string Id { get; set; }
        public string Minecraft { get; set; }
        public string Java { get; set; }
        public string Memory { get; set; }
        public object Forge { get; set; }
        public List<Mod> Mods { get; set; }
    }
}
