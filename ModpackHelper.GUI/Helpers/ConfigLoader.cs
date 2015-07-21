using System;
using System.IO.Abstractions;
using ModpackHelper.IO;

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
                modpackHelper.InputDirectoryTextBox.Text = string.IsNullOrWhiteSpace(ch.Configs.ModpackName) ? ch.Configs.InputDirectory : ch.Configs.Modpacks[ch.Configs.ModpackName].InputDirectory;

                modpackHelper.OutputDirectoryTextBox.Text = string.IsNullOrWhiteSpace(ch.Configs.OutputDirectory) ? fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ModpackHelper") : ch.Configs.OutputDirectory;
                
                modpackHelper.ClearOutpuDirectoryCheckBox.Checked = ch.Configs.ClearOutputDirectory;

                modpackHelper.CreateConfigZipCheckBox.Checked = ch.Configs.CreateConfigZip;

                if (ch.Configs.TechnicPermissionsPrivate)
                    modpackHelper.technicPermissionsPrivatePack.Checked = true;
                else
                    modpackHelper.technicPermissionsPublicPack.Checked = true;

                modpackHelper.MinecraftVersionDropdown.SelectedItem = string.IsNullOrWhiteSpace(ch.Configs.ModpackName)
                    ? "1.7.10"
                    : ch.Configs.Modpacks[ch.Configs.ModpackName].MinecraftVersion;
            }
        }
    }
}
