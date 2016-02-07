using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Permissions
{
    public class UserPermission
    {
        /// <summary>
        /// The modid of the mod the permissions are for
        /// </summary>
        public string ModId { get; set; }
        /// <summary>
        /// The link to where the permissions for modpack destribution can be found
        /// </summary>
        public string LinkToPermissionListing { get; set; }
        /// <summary>
        /// A link to the proof of permissions that the user can destribute this mod
        /// </summary>
        public string LinkToProofOfPermissions { get; set; }

        /// <summary>
        /// The launcher these permissions are for
        /// </summary>
        public Launcher Launcher { get; set; }

        public bool IsDone()
        {
            return LinkToProofOfPermissions.IsFullyQualifiedUrl() && LinkToPermissionListing.IsFullyQualifiedUrl();
        }
    }

    public enum Launcher
    {
        Technic,
        FTB
    }
}
