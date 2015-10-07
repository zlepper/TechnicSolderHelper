using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using ModpackHelper.Shared.Web.Api;
using ModpackHelper.webmods.db;
using ModpackHelper.webmods.Helpers;

namespace ModpackHelper.webmods.Hubs
{
    /// <summary>
    /// A SignalR hub that provides the same options as the api itself, expect it's much faster due to less shaking hands and stuff like that. 
    /// </summary>
    public class ApiHub : Hub<IApiClient>
    {
        /// <summary>
        /// The database with all the info
        /// </summary>
        private ModpackHelperContext db = new ModpackHelperContext();

        /// <summary>
        /// Get a list of all the mods in the database
        /// </summary>
        public IEnumerable<Mod> GetMods()
        {
            return db.Mods;
        }

        /// <summary>
        /// Get the specific mod from the db, provided it has been accepted
        /// </summary>
        /// <param name="md5">The md5 value of the mod</param>
        public Mod GetMod(string md5)
        {
            return db.Mods.FirstOrDefault(m => m.JarMd5.Equals(md5) && m.Status == Status.Accepted);
        }

        /// <summary>
        /// Add a mod to the database
        /// </summary>
        /// <param name="mod">The mod to add to the database</param>
        public void PostMod(Mod mod)
        {
            // Make sure the mod actually is valid before we add it to the database
            if (mod != null && mod.IsValid())
            {
                ApiHelpers.AddModToDB(mod);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// An interface descripting what methods are available on the client. 
    /// </summary>
    public interface IApiClient
    {
    }
}