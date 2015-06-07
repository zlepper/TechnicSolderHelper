using System.Collections.Generic;

namespace TechnicSolderHelper.OLD
{
    public class Modlist
    {
        public string Modid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }

        public string Mcversion { get; set; }

        public string Url { get; set; }

        public List<string> Authors { get; set; }
    }
}