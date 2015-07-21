using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.IO;
using ModpackHelper.Shared.resx;

namespace ModpackHelper.GUI.Helpers
{
    public class ConfigLoader
    {
        private readonly ModpackHelper _modpackHelper;
        private readonly IFileSystem _fileSystem;

        // Normal constructor
        public ConfigLoader(ModpackHelper modpackHelper) : this(modpackHelper, new FileSystem())
        {
        }

        // Allows for unit testing
        public ConfigLoader(ModpackHelper modpackHelper, IFileSystem fileSystem)
        {
            _modpackHelper = modpackHelper;
            _fileSystem = fileSystem;
        }

        public void ReloadConfigs()
        {
            // Create a confighandler to load all saved data 
            // This enables us to retain the state of the application between launches
            using (ConfigHandler ch = new ConfigHandler(_fileSystem))
            {
                // Attempt to find the inputdirectory
                try
                {
                    _modpackHelper.inputDirectoryTextBox.Text = ch.GetProperty(ConfigHandlerKeys.inputDirectory).ToString();
                }
                catch (IndexOutOfRangeException)
                {
                    // No default directory
                    _modpackHelper.inputDirectoryTextBox.Text = "";
                }

                // Attempt to set the output directory
                try
                {
                    _modpackHelper.outputDirectoryTextBox.Text = ch.GetProperty(ConfigHandlerKeys.outputDirectory).ToString();
                }
                catch (IndexOutOfRangeException)
                {
                    _modpackHelper.outputDirectoryTextBox.Text = _fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ModpackHelper");
                }

                // ClearOutputdirectory checkbox
                try
                {
                    bool result;
                    bool converted = bool.TryParse(ch.GetProperty(ConfigHandlerKeys.clearOutputDirectory).ToString(),
                        out result);
                    if (converted)
                        _modpackHelper.ClearOutpuDirectoryCheckBox.Checked = result;
                }
                catch (IndexOutOfRangeException)
                { }


            }
        }
    }
}
