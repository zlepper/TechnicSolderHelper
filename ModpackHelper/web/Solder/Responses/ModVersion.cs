using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Web.Solder.Responses
{
    public class ModVersion
    {
        public string Filesize { get; set; }
        public string Md5 { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
