using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TechnicSolderHelper
{
    public class mcmod
    {
        public string modid { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string mcversion { get; set; }
        public string url { get; set; }
        public string credits { get; set; }
        public string description { get; set; }

    }

    public class litemod
    {
        public string name { get; set; }
        public string mcversion { get; set; }
        public string version { get; set; }
        public string revision { get; set; }
        public string author { get; set; }
        public string description { get; set; }
    }

    public class modlist
    {
        
        public int modListVersion { get; set; }
        public List<String> modList { get; set; }
    }

    public class ownPermissions
    {
        public Boolean hasPermission { get; set; }
        public String Link { get; set; }
    }

    public class ModHelper
    {
        public static mcmod NotEnoughItems(String fileName)
        {
            fileName = fileName.Replace("-universal.jar", "");
            mcmod mod = new mcmod();
            mod.name = "Not Enough Items";

            //Figure out MCVersion
            String tempString = fileName.Replace("NotEnoughItems-", "");
            int indexOfVersion = tempString.IndexOf('-');
            String version = "";
            for (int i = 0; i < indexOfVersion; i++)
            {
                version = version + tempString[i];
            }
            mod.mcversion = version;

            //Figure out mod version
            tempString = tempString.Replace("-", "").Replace(version, "");
            mod.version = tempString;


            return mod;
        }

        public static mcmod Liteloader(String fileName)
        {
            mcmod mod = new mcmod();
            mod.name = "Liteloader";
            return mod;
        }

        public static mcmod CoFHLib(String fileName)
        {
            fileName = fileName.Replace(".jar", "").Replace("\\","").ToLower();
            mcmod mod = new mcmod();
            mod.name = "CoFHLib";

            //Figure out mcversion
            fileName = fileName.Replace(mod.name.ToLower() + "-", "");
            for (int i = 0; i < fileName.Length; i++)
            {
                if (fileName[i].Equals(']'))
                {
                    break;
                }
                else
                {
                    mod.mcversion = mod.mcversion + fileName[i];
                }
            }
            mod.mcversion = mod.mcversion.Replace("[", "");
            fileName = fileName.Replace("[" + mod.mcversion + "]", "");
            //Figure out mod version

            mod.version = fileName;

            return mod;
        }

        public static mcmod CodeChickenCore(String fileName)
        {

            fileName = fileName.Replace("-universal.jar", "");
            mcmod mod = new mcmod();
            mod.name = "CodeChicken Core";

            //Figure out MCVersion
            String tempString = fileName.Replace("CodeChickenCore-", "");
            int indexOfVersion = tempString.IndexOf('-');
            String version = "";
            for (int i = 0; i < indexOfVersion; i++)
            {
                version = version + tempString[i];
            }
            mod.mcversion = version;

            //Figure out mod version
            tempString = tempString.Replace("-", "").Replace(version, "");
            mod.version = tempString;


            return mod;
        }

        public static mcmod GoodVersioning(String fileName)
        {
            fileName = fileName.Replace(".jar", "").Replace(".zip", "").Replace("\\", "");
            mcmod mod = new mcmod();

            //Figure out modname
            String modname = "";
            for (int i = 0; i < fileName.Length; i++)
            {
                if (!(fileName[i].Equals('-')))
                {
                    modname = modname + fileName[i];
                }
                else
                {
                    break;
                }
            }
            mod.name = modname;
            fileName = fileName.Replace(modname + "-", "");

            //Figure out minecraft version
            String mcversion = "";
            for (int i = 0; i < fileName.Length; i++)
            {
                if (!(fileName[i].Equals('-')))
                {
                    mcversion = mcversion + fileName[i];
                }
                else
                {
                    break;
                }
            }
            mod.mcversion = mcversion;

            //Figure out modversion
            fileName = fileName.Replace(mcversion + "-", "");
            mod.version = fileName;

            
            return mod;
        }

        public static mcmod iChunMod(String FileName)
        {
            mcmod mod = new mcmod();

            String name = "";
            for (int i = 0; i < FileName.Length; i++)
            {
                if (!(FileName[i].Equals('-')))
                {
                    name = name + FileName[i];
                }else{
                    break;
                }
            }
            mod.name = name;

            FileName = FileName.Replace(name, "").Replace("Beta", "").Replace("-", "").Replace(".jar", "");
            mod.version = FileName;

            return mod;
        }

        public static mcmod waila(String FileName)
        {
            mcmod mod = new mcmod();

            String name = "";
            for (int i = 0; i < FileName.Length; i++)
            {
                if (!(FileName[i].Equals('-')))
                {
                    name = name + FileName[i];
                }
                else
                {
                    break;
                }
            }
            mod.name = name;

            FileName = FileName.Replace(".jar", "").Replace(name, "");

            String version = "";
            for (int i = 0; i < FileName.Length; i++)
            {
                if (!(FileName[i].Equals('_')))
                {
                    version = version + FileName[i];
                }
                else
                {
                    break;
                }
            }
            mod.version = version;

            FileName = FileName.Replace("_", "").Replace(version, "");
            mod.mcversion = FileName;

            return mod;
        }

        public static mcmod ReikasMods(String FileName)
        {
            mcmod mod = new mcmod();

            FileName = FileName.Replace(".jar", String.Empty);
            FileName = FileName.Replace(".zip", String.Empty);
            Debug.WriteLine(FileName);
            Debug.WriteLine("THis");
            //Figure out mod name
            String[] reikas = FileName.Split(' ');

            mod.name = reikas[0].Replace(" ", String.Empty);
            mod.mcversion = reikas[1].Replace(" ", String.Empty);
            mod.version = reikas[2].Replace(" ", String.Empty);

            return mod;
        }
    }

    public class mcmod2
    {
        public int modListVersion { get; set; }
        public List<ModList> modList { get; set; }
    }

    public class ModList
    {
        public string modid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string version { get; set; }
        public string mcversion { get; set; }
        public string url { get; set; }
        public string updateUrl { get; set; }
        public List<string> authorList { get; set; }
        public string credits { get; set; }
        public string logoFile { get; set; }
        public List<object> screenshots { get; set; }
        public string parent { get; set; }
        public List<string> requiredMods { get; set; }
        public List<string> dependencies { get; set; }
        public List<object> dependants { get; set; }
        public string useDependencyInformation { get; set; }
    }
}
