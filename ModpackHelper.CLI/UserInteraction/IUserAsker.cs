using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.CLI.UserInteraction
{
    public interface IUserAsker
    {
        string GetData();
    }

    public class UserAsker : IUserAsker
    {
        public string GetData()
        {
            return Console.ReadLine();
        }
    }
}
