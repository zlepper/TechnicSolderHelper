using System.Collections.Generic;

namespace ModpackHelper.mods
{
    /// <summary>
    /// Used to descripe a minecraft mod
    /// Also the pattern in normal mcmod.info files
    /// </summary>
    public class Mcmod
    {
        /// <summary>
        /// The modid of the mod
        /// </summary>
        public string Modid { get; set; }

        /// <summary>
        /// The name of the mod
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version of the mod
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The Minecraft version of the mod
        /// </summary>
        public string Mcversion { get; set; }

        /// <summary>
        /// The info url of the mod
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A short description of the mod
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates if this mod has been written to the output modlist
        /// </summary>
        public bool HasBeenWritenToModlist { get; set; }

        /// <summary>
        /// Indicates if this mod should be skipped when packing
        /// </summary>
        public bool IsSkipping { get; set; }

        /// <summary>
        /// A list of the authors of the mod
        /// </summary>
        public List<string> AuthorList { get; set; }

        /// <summary>
        /// A list of the authors of the mod
        /// </summary>
        public List<string> Authors { get; set; }

        /// <summary>
        /// Indicates the permissions needed to distribute the mod
        /// in a public modpack
        /// </summary>
        public PermissionLevel PublicPerms { get; set; }

        /// <summary>
        /// Indicates the permissions needed to distribute the mod
        /// in a private modpack
        /// </summary>
        public PermissionLevel PrivatePerms { get; set; }

        /// <summary>
        /// Indicates if the mod should be ignore
        /// TODO figure out exactly what i'm using this for
        /// </summary>
        public bool IsIgnore { get; set; }

        /// <summary>
        /// TODO figure out exactly what i'm using this for
        /// </summary>
        public bool UseShortName { get; set; }

        /// <summary>
        /// Indicates if this info was fetched from my remote database
        /// and therefor should not be put back into the databasee
        /// </summary>
        public bool FromSuggestion { get; set; }

        /// <summary>
        /// Indicates that the mods information was from a users input
        /// </summary>
        public bool FromUserInput { get; set; }

        /// <summary>
        /// The name of the file itself
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The path of the mod
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Indicates that all informatio has been entered for the mod
        /// </summary>
        public bool Aredone { get; set; }
    }
}
