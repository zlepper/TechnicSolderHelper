using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.Web;

// ReSharper disable InconsistentNaming

namespace ModpackHelper.Shared.Mods
{
    public class Solder
    {
        public class ApiMod
        {
            public string name { get; set; }
            public string pretty_name { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string link { get; set; }
            public List<string> versions { get; set; }
        }

        public class ApiModVersion
        {
            public string md5 { get; set; }
            public string url { get; set; }
        }

        public class ModpackWithoutSlut
        {
            public Dictionary<string, string> modpacks { get; set; }
            public string mirror_url { get; set; }
        }

        public class ModpackWithoutBuild
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public string url { get; set; }
            public string icon { get; set; }
            public string icon_md5 { get; set; }
            public string logo { get; set; }
            public string logo_md5 { get; set; }
            public string background { get; set; }
            public string background_md5 { get; set; }
            public string recommended { get; set; }
            public string latest { get; set; }
            public List<string> builds { get; set; }
        }

        public class ModpackWithBuild
        {
            public class Mod
            {
                public string name { get; set; }
                public string version { get; set; }
                public string md5 { get; set; }
                public string url { get; set; }
            }

            public string minecraft { get; set; }
            public string minecraft_md5 { get; set; }
            public object forge { get; set; }
            public List<Mod> mods { get; set; }
        }

        public class Error
        {
            public string error { get; set; }
        }

        private ISolderWebClient SolderClient;

        public Solder(ISolderWebClient solderWebClient)
        {
            SolderClient = solderWebClient;
        }

        public void Login(string email, string password)
        {
            
        }

        public void CreateModpack(string modpackname, string slug)
        {
            throw new NotImplementedException();
        }

        public void CreateBuild()
        {
            throw new NotImplementedException();
        }

        public void CreateMod(string modname, string modslug, string authors, string description, string modurl)
        {
            throw new NotImplementedException();
        }

        public void CreateModVersion(string modId, string version)
        {
            throw new NotImplementedException();
        }

        public void PutModversionInBuild()
        {
            throw new NotImplementedException();
        }
    }
}
