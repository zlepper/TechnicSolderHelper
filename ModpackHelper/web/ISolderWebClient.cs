namespace ModpackHelper.Shared.Web
{
    public interface ISolderWebClient
    {
        /// <summary>
        /// Logs into the service
        /// </summary>
        /// <param name="email">The email to login with</param>
        /// <param name="password">The password to login with</param>
        void Login(string email, string password);

        /// <summary>
        /// Create a pack with the specific name and slug
        /// Should hit http://solder.zlepper.dk/modpack/create with POST
        /// Paramerters of request is:
        ///         name: Modpackname
        ///         slug: modpack slug
        /// </summary>
        /// <param name="modpackname">The name of the modpack to create</param>
        /// <param name="slug">The slug to create the modpack with</param>
        void CreatePack(string modpackname, string slug);

        /// <summary>
        /// Adds a mod to solder
        /// Should hit: http://solder.zlepper.dk/mod/create with POST
        /// Parameters of request:
        ///         pretty_name: Mod name
        ///         name: mod slug
        ///         author: Author
        ///         description: Description
        ///         link: Mod url
        /// </summary>
        /// <param name="modname">The name of the mod to create</param>
        /// <param name="modslug">The modslug of the mod (ModID)</param>
        /// <param name="authors">The authors of the mod, optional</param>
        /// <param name="description">A description of the mod, optional</param>
        /// <param name="modurl">A url to the mod, optional</param>
        void AddMod(string modname, string modslug, string authors, string description, string modurl);

        /// <summary>
        /// Adds/update a version of a mod
        /// Should hit: http://solder.zlepper.dk/mod/add-version with POST
        ///         mod-id: the numeric id of the mod
        ///         add-version: The version of the mod (minecraft version + "-" + mod version)
        /// </summary>
        /// <param name="modId"></param>
        /// <param name="version"></param>
        void AddModVersion(string modId, string version);

        /// <summary>
        /// Rehashes a specific mod version
        /// Should hit: http://solder.zlepper.dk/mod/rehash with POST
        /// Paremeters of request:
        ///         ver_id: The id of the modversion
        /// </summary>
        /// <param name="modversionId"></param>
        void RehashModVersion(string modversionId);
    }
}
