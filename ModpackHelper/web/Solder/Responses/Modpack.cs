using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Web.Solder.Responses
{
    public class Modpack
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string IconMd5 { get; set; }
        public string Logo { get; set; }
        public string LogoMd5 { get; set; }
        public string Background { get; set; }
        public string BackgroundMd5 { get; set; }
        public string Recommended { get; set; }
        public string Latest { get; set; }
        public List<string> Builds { get; set; }
    }
}
