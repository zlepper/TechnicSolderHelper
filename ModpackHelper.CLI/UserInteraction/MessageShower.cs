using System;
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
