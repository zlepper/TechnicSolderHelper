using System;
using System.IO.Abstractions;
using ModpackHelper.IO;
using ModpackHelper.Shared.Utils.Config;

namespace ModpackHelper.GUI.Helpers
{
    public class ConfigLoader
    {
        private readonly ModpackHelper modpackHelper;
        private readonly IFileSystem fileSystem;

        // Normal constructor
        public ConfigLoader(ModpackHelper modpackHelper) : this(modpackHelper, new FileSystem())
        {
        }

        // Allows for unit testing
        public ConfigLoader(ModpackHelper modpackHelper, IFileSystem fileSystem)
        {
            this.modpackHelper = modpackHelper;
            this.fileSystem = fileSystem;
        }

        public void ReloadConfigs()
        {
            // Create a confighandler to load all saved data 
            // This enables us to retain the state of the application between launches
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                Configs c = ch.Configs;

                // Check if we have ever created a pack before. If we haven't, then
                // we can't load anything
                if (string.IsNullOrWhiteSpace(c.LastSelectedModpack)) return;
                // Failsafe to make sure the pack we are trying to load data for actually exists
                if (!c.Modpacks.ContainsKey(c.LastSelectedModpack)) return;

                // Load the modpack data
                Modpack modpack = c.Modpacks[c.LastSelectedModpack];

                // Load all the data back into the form
                modpackHelper.createForgeZipCheckBox.Checked = modpack.CreateForgeZip;
                modpackHelper.ClearOutpuDirectoryCheckBox.Checked = modpack.ClearOutputDirectory;
                modpackHelper.CheckTechnicPermissionsCheckBox.Checked = modpack.CheckTechnicPermissions;
                modpackHelper.CreateConfigZipCheckBox.Checked = modpack.CreateConfigZip;
                modpackHelper.ModpackNameTextBox.Text = modpack.Name;
                modpackHelper.MinecraftVersionDropdown.SelectedText = modpack.MinecraftVersion;
            }
        }
    }
}
