using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.CLI.UserInteraction;

namespace ModpackHelper.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Handler h = new Handler();
            h.Start(args.ToList(), new MessageShower());
        }
    }
}
