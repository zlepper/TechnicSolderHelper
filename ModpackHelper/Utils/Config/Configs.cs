using System.Collections.Generic;

namespace ModpackHelper.Shared.Utils.Config
{
    public class Configs
    {
        public string InputDirectory { get; set; }
        public string OutputDirectory { get; set; }
        public bool ClearOutputDirectory = true;
        public bool CreateTechnicPack { get; set; }
        public bool CreateConfigZip { get; set; }
        public bool CreateForgeZip { get; set; }
        public bool TechnicPermissionsPrivate { get; set; }
        public Dictionary<string, Modpack> Modpacks = new Dictionary<string, Modpack>();
        public string ModpackName { get; set; }
    }
}
