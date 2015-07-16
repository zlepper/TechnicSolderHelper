using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.UserInteraction
{
    public interface IMessageShower
    {
        void ShowMessage(string message);
        void ShowMessageAsync(string message);
    }
}
