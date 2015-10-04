using System.Collections.Generic;
using ModpackHelper.Shared.Web.Api;

namespace ModpackHelper.webmods.Hubs
{
    /// <summary>
    /// An interface specifying all the methods available on the client
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Sends a collection of mods to the client
        /// </summary>
        /// <param name="mods">All of the mods to display on the client</param>
        void SendMods(List<Mod> mods);

        /// <summary>
        /// Tells the client to remove a mod
        /// </summary>
        /// <param name="id">The id of the mod to remove</param>
        void RemoveMod(int id);

        /// <summary>
        /// Tells the client if it's logged in or out
        /// </summary>
        /// <param name="state"></param>
        void SetLoggedIn(bool state);

        /// <summary>
        /// Tells the client that the login attempt failed
        /// </summary>
        void LoginFailed();
    }
}