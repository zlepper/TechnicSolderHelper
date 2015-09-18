using System;
using System.Linq;
using ModpackHelper.CLI.UserInteraction;

namespace ModpackHelper.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Can't use the program without args
            if (!args.Any())
            {
                Console.WriteLine(Messages.Usage);
                return;
            }
            // All the important stuffs
            Handler h = new Handler();
            if (h.Start(args.ToList(), new MessageShower()))
            {
                h.Pack(new MessageShower());
            }
        }
    }
}
