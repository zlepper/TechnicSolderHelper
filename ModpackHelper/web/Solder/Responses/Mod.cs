using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Web.Solder.Responses
{
    public class Mod
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PrettyName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Donate { get; set; }
        public List<string> Versions { get; set; }
    }
}
