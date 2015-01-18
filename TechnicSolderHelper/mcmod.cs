using System;
using System.Collections.Generic;

namespace TechnicSolderHelper
{
    public class Mcmod
    {
        public string Modid { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Mcversion { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public Boolean HasBeenWritenToModlist { get; set; }

        public Boolean IsSkipping { get; set; }

        public List<string> AuthorList { get; set; }

        public List<string> Authors { get; set; }

        public PermissionLevel PublicPerms { get; set; }

        public PermissionLevel PrivatePerms { get; set; }

        public Boolean IsIgnore { get; set; }

        public Boolean UseShortName { get; set; }

        public Boolean FromSuggestion { get; set; }

    }

    public class OwnPermissions
    {
        public Boolean HasPermission { get; set; }

        public String PermissionLink { get; set; }

        public String ModLink { get; set; }
    }

    public class ModHelper
    {

        public static Mcmod GoodVersioning(String fileName)
        {
            fileName = fileName.Remove(fileName.LastIndexOf("."));
            Mcmod mod = new Mcmod();

            //Figure out modname
            String modname = "";
            foreach (char c in fileName)
            {
                if (!(c.Equals('-')))
                {
                    modname = modname + c;
                }
                else
                {
                    break;
                }
            }
            mod.Name = modname;
            fileName = fileName.Replace(modname + "-", "");

            //Figure out minecraft version
            String mcversion = "";
            foreach (char c in fileName)
            {
                if (!(c.Equals('-')))
                {
                    mcversion = mcversion + c;
                }
                else
                {
                    break;
                }
            }
            mod.Mcversion = mcversion;

            //Figure out modversion
            fileName = fileName.Replace(mcversion + "-", "");
            mod.Version = fileName;


            return mod;
        }

        public static Mcmod WailaPattern(String fileName) // waila-1.5.5_1.7.10.jar
        {
            fileName = fileName.Remove(fileName.LastIndexOf(".", StringComparison.Ordinal));
            Mcmod mod = new Mcmod();

            String name = "";
            foreach (char c in fileName)
            {
                if (!(c.Equals('-')))
                {
                    name = name + c;
                }
                else
                {
                    break;
                }
            }
            mod.Name = name;

            fileName = fileName.Replace(name, "");

            String version = "";
            foreach (char c in fileName)
            {
                if (!(c.Equals('_')))
                {
                    version = version + c;
                }
                else
                {
                    break;
                }
            }
            mod.Version = version;

            fileName = fileName.Replace("_", "").Replace(version, "");
            mod.Mcversion = fileName;

            return mod;
        }

        public static Mcmod ReikasMods(String fileName)
        {
            Mcmod mod = new Mcmod();

            fileName = fileName.Remove(fileName.LastIndexOf(".", StringComparison.Ordinal));

            //Figure out mod name
            String[] reikas = fileName.Split(' ');

            mod.Name = reikas[0].Replace(" ", String.Empty);
            mod.Mcversion = reikas[1].Replace(" ", String.Empty);
            mod.Version = reikas[2].Replace(" ", String.Empty);

            return mod;
        }
    }

    public class Mcmod2
    {
        public int Modinfoversion { get; set; }

        public int ModListVersion { get; set; }

        public List<Modlist> Modlist { get; set; }
    }
}
