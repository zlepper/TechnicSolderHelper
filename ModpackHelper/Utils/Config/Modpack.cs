using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Utils.Config
{
    public class Modpack
    {
        /// <summary>
        /// Name of the modpack
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Minecraft version of the modpack
        /// </summary>
        public string MinecraftVersion { get; set; }

        /// <summary>
        /// The local developer directory of the modpack
        /// </summary>
        public string InputDirectory { get; set; }

        /// <summary>
        /// The output directory of the modpack
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Indicates if we should clear the output directory before running
        /// Defaults to true
        /// </summary>
        public bool ClearOutputDirectory { get; set; } = true;

        /// <summary>
        /// Indicates if we should create a technic pack in the first place
        /// </summary>
        public bool CreateTechnicPack { get; set; }
        /// <summary>
        /// Indicates if we should pack the configs folder
        /// </summary>
        public bool CreateConfigZip { get; set; }
        /// <summary>
        /// Indicates if we should pack forge with the modpack
        /// </summary>
        public bool CreateForgeZip { get; set; }

        /// <summary>
        /// Indicates if we should check for permissions for the technic pack
        /// </summary>
        public bool CheckTechnicPermissions { get; set; }

        /// <summary>
        /// Indicates if we should check for private permissions
        /// </summary>
        public bool TechnicPermissionsPrivate { get; set; }

        /// <summary>
        /// Indicates if we should create a solderpack or a zip pack
        /// </summary>
        public bool CreateSolderPack { get; set; }

        /// <summary>
        /// Indicates what version of forge the pack should be packed with
        /// </summary>
        public string ForgeVersion { get; set; }

        /// <summary>
        /// Indicates if this modpack should be uploaded to solder
        /// </summary>
        public bool UseSolder { get; set; }

        /// <summary>
        /// Indicates if this modpack should be uploaded to FTP
        /// </summary>
        public bool UploadToFTP { get; set; }

        /// <summary>
        /// Indicates what version of the modpack we are packing
        /// This will not be saved to the json file
        /// </summary>
        [JsonIgnore]
        public string Version { get; set; }

        /// <summary>
        /// Indicates if the entire pack should be repacked
        /// </summary>
        [JsonIgnore]
        public bool RepackEverything { get; set; }

        /// <summary>
        /// The minimum java version required to play the pack
        /// </summary>
        public string MinJava { get; set; }

        /// <summary>
        /// The minimum required amount of memory to play the pack
        /// </summary>
        public string MinMemory { get; set; }

        /// <summary>
        /// Indicates if the mods should be uploaded and reshashed no matter what
        /// </summary>
        public bool ForceSolder { get; set; }

        /// <summary>
        /// Generates a safe modpack slug
        /// </summary>
        /// <returns></returns>
        public string GetSlug()
        {
            return Regex.Replace(Name.ToLower().Replace(" ", "-"), "\\|/|\\||:|\\*|\"|<|>|\\?|'", string.Empty);
        }
    }
}
