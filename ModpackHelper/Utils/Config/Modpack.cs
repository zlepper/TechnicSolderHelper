using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
