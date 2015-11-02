using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.Web.Solder;

namespace ModpackHelper.Shared.Utils.Solder
{
    /// <summary>
    /// A helper to lift all the db interacting
    /// </summary>
    public class SolderMySQLHelper
    {
        /// <summary>
        /// The name of the modpack
        /// </summary>
        public string ModpackName { get; set; }

        /// <summary>
        /// The modpack version to update
        /// </summary>
        public string ModpackVersion { get; set; }

        /// <summary>
        /// The modpack id
        /// </summary>
        private int ModpackId = -1;

        /// <summary>
        /// The build to update
        /// </summary>
        private int BuildId = -1;

        /// <summary>
        /// The MySQL client to interact with the DB
        /// </summary>
        public SolderMySQLClient SolderClient { get; set; }

        /// <summary>
        /// Create a new helper
        /// </summary>
        /// <param name="modpackName">The name of the modpack</param>
        /// <param name="modpackversion">The version of the modpack</param>
        public SolderMySQLHelper(string modpackName, string modpackversion)
        {
            ModpackName = modpackName;
            ModpackVersion = modpackversion;

            try
            {
                SolderClient = new SolderMySQLClient();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Create a new modpack in solder, if required
        /// </summary>
        /// <param name="minecraftVersion">The minecraft version</param>
        /// <param name="javaVersion">The minimum required javaversion</param>
        /// <param name="minimumMemory">The minimum required amount of memory</param>
        public void CreateModpackBuild(string minecraftVersion, string javaVersion = "1.6", int minimumMemory = 0)
        {
            // Create the modpack if it doesn't exist
            ModpackId = SolderClient.CreateNewModpack(ModpackName);

            // Check if a build has already been created
            BuildId = SolderClient.GetBuildId(ModpackId, ModpackVersion);
            if (BuildId != -1) return;

            // Create the modpack and save the buildid
            SolderClient.CreateModpackBuild(ModpackId, ModpackVersion, minecraftVersion, javaVersion, minimumMemory);
            BuildId = SolderClient.GetBuildId(ModpackId, ModpackVersion);
        }

        /// <summary>
        /// Checks if the mod has already been uploaded to solder
        /// </summary>
        /// <param name="mod">The mod to check for</param>
        /// <returns>True if the mod is online, otherwise false</returns>
        public bool IsModVersionOnline(Mcmod mod)
        {
            return SolderClient.IsModversionOnline(mod);
        }

        /// <summary>
        /// Adds a mod to a build
        /// </summary>
        /// <param name="id">The id the of the mod in the database</param>
        /// <param name="mod">The mod to add to the build</param>
        public void AddModToBuild(int id, Mcmod mod)
        {
            // Get the modversion id in the database
            int modversionId = SolderClient.GetModversionId(id, mod);

            // Upload the new data
            SolderClient.AddModversionToBuild(BuildId, modversionId);
        }

        /// <summary>
        /// Adds a mod to solder
        /// </summary>
        /// <param name="mod">The mod to add</param>
        public void AddModToSolder(Mcmod mod)
        {
            // If the mod version is already in the database, then we only need to update the md5 value
            if (IsModVersionOnline(mod))
            {
                SolderClient.UpdateModversionMd5(mod);
            }

            // Check if the mod has already been added to solder.
            int id = SolderClient.GetModId(mod);
            if (id == -1)
            {
                // The mod is not in solder, so add it there
                SolderClient.AddModToSolder(mod);
                id = SolderClient.GetModId(mod);
            }
            // Add the mod to the build
            AddModToBuild(id, mod);
        }
    }
}
