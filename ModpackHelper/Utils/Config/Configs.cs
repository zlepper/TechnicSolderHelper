using System.Collections.Generic;

namespace ModpackHelper.Shared.Utils.Config
{
    public class Configs
    {
        public string LastSelectedModpack { get; set; }
        public Dictionary<string, Modpack> Modpacks = new Dictionary<string, Modpack>();
        public SolderLoginInfo SolderLoginInfo { get; set; }
    }
}
