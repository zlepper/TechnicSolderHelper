using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.UserInteraction;

namespace ModpackHelper.CLI.UserInteraction
{
    class MessageShower : IMessageShower
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public void ShowMessageAsync(string message)
        {
            Console.WriteLine(message);
        }
    }
}
