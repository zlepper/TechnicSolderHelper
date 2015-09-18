using System.Linq;
using ModpackHelper.CLI.UserInteraction;

namespace ModpackHelper.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Handler h = new Handler();
            if (h.Start(args.ToList(), new MessageShower()))
            {

            }
        }
    }
}
