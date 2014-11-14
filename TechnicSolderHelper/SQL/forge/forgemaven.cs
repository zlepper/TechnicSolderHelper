using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicSolderHelper.forge
{
    public class forgemaven
    {
        public string homepage { get; set; }

        public string name { get; set; }

        public Dictionary<int, Number> number { get; set; }

        public string webpath { get; set; }
    }

    public class Number
    {
        public string branch { get; set; }

        public int build { get; set; }

        public List<List<String>> files { get; set; }

        public string jobver { get; set; }

        public string mcversion { get; set; }

        public string modified { get; set; }

        public string version { get; set; }

        public string downloadurl { get; set; }
    }

}
