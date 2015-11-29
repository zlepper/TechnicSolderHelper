using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Web.Solder.Responses
{
    public class AllModpacks
    {
        public Dictionary<string, string> Modpacks { get; set; }
        public string MirrorUrl { get; set; } 
    }
}
