using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace TechnicSolderHelper
{
    public class mod
    {
        public int ID { get; set; }
        public String ModName { get; set; }
        public String ModID { get; set; }
        public String ModVersion { get; set; }
        public String MinecraftVersion { get; set; }
        public String FileName { get; set; }
        public String FileVersion { get; set; }
        public String MD5 { get; set; }
    }
}
