using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace TechnicSolderHelper.SQL
{
    public class ftbPermissions
    {
        public string shortName { get; set; }
        public string modName { get; set; }
        public string modAuthor { get; set; }
        public string modLink { get; set; }
        public string licenseLink { get; set; }
        public string licenseImage { get; set; }
        public string privateLicenseLink { get; set; }
        public string privateLicenseImage { get; set; }
        public int publicPolicy { get; set; }
        public int privatePolicy { get; set; }
        public List<string> modIDs { get; set; }
    }
}
