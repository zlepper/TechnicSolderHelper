using System.Collections.Generic;

namespace ModpackHelper.Shared.Web.Api
{
    /// <summary>
    /// A user of the webapplication
    /// </summary>
    public class User
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username the user uses to login
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The hashed password the user uses to login
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// An enum descripting how deep the user can access the data
        /// </summary>
        public AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// A collection of all the mods accepted by the user
        /// </summary>
        public virtual ICollection<Mod> AcceptedMods { get; set; }

        /// <summary>
        /// A collection of all the connections the server currently
        /// has to the server
        /// </summary>
        public virtual ICollection<Connection> Connections { get; set; }
    }

    public enum AccessLevel
    {
        Administrator,
        Contributor
    }
}
