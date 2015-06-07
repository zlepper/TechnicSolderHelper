using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.mods
{
    /// <summary>
    /// The permissions needed to distribute a mod
    /// </summary>
    public enum PermissionLevel
    {
        Unknown,
        Open,
        Notify,
        Ftb,
        Request,
        Closed
        
    }
}
