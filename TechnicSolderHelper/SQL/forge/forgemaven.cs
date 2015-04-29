using System.Collections.Generic;

namespace TechnicSolderHelper.SQL.forge
{
    public class Forgemaven
    {

        public Dictionary<int, Number> Number { get; set; }

        public string Webpath { get; set; }
    }

    public class Number
    {

        public int Build { get; set; }

        public string Jobver { get; set; }

        public string Mcversion { get; set; }

        public string Version { get; set; }

        public string Downloadurl { get; set; }
    }

}
