using System.Collections.Generic;

namespace ModpackHelper.Shared.Web.Api
{
    /// <summary>
    /// A user who uses the helper application and provide
    /// all sorts of tasty data
    /// </summary>
    public class HelperUser
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The IP of the user
        /// </summary>
        public string Ip { get; set; }
        

        /// <summary>
        /// A list of all the mods the user upload
        /// </summary>
        public virtual ICollection<Mod> Mods { get; set; }

        /// <summary>
        /// Create a new helperuser
        /// </summary>
        public HelperUser()
        {
        }

        /// <summary>
        /// Create a new user with a specific ip
        /// </summary>
        /// <param name="ip">The ip of the user</param>
        public HelperUser(string ip)
        {
            Ip = ip;
        }
    }
}
