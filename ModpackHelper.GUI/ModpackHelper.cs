using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModpackHelper.GUI.Helpers;
using ModpackHelper.GUI.Properties;
using ModpackHelper.GUI.UserInteraction;
using ModpackHelper.IO;
using ModpackHelper.Shared.resx;
using ModpackHelper.UserInteraction;

namespace ModpackHelper.GUI
{
    public partial class ModpackHelper : Form
    {
        private readonly IFileSystem _fileSystem;
        private readonly IDirectoryFinder _directoryFinder;
        private readonly IMessageShower _messageShower;
        // Normal constructor
        public ModpackHelper() : this(new FileSystem(), new DirectoryFinder(), new MessageShower())
        {
            
        }

        // Unit testing constructor
        public ModpackHelper(IFileSystem fileSystem, IDirectoryFinder finder, IMessageShower messageShower)
        {
            _fileSystem = fileSystem;
            _directoryFinder = finder;
            _messageShower = messageShower;
            InitializeComponent();

            // Reload the interface
            ConfigLoader cl = new ConfigLoader(this, fileSystem);
            cl.ReloadConfigs();
        }

        // Called when the user clicks the browse button for the input directory
        public void browseForInputDirectoryButton_Click(object sender, EventArgs e)
        {
            string i = _directoryFinder.GetDirectory(Resources.SelectTheInputDirectory, inputDirectoryTextBox.Text);
            // The input directory should always be a /mods/ directory
            if (i.EndsWith(Path.DirectorySeparatorChar + "mods"))
            {
                inputDirectoryTextBox.Text = i;
                using (ConfigHandler ch = new ConfigHandler(_fileSystem))
                    ch.SetProperty(ConfigHandlerKeys.inputDirectory, i);
            }
            else
                _messageShower.ShowMessage(Resources.InputHasToBeModDirectory);
        }

        // Called when the user clicks the browser button for the output directory
        public void browseForOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            outputDirectoryTextBox.Text = _directoryFinder.GetDirectory(Resources.SelectOutputDirectory, outputDirectoryTextBox.Text);
            using (ConfigHandler ch = new ConfigHandler(_fileSystem))
                ch.SetProperty(ConfigHandlerKeys.outputDirectory, outputDirectoryTextBox.Text);
        }

        // Called when the user checks or unchecks the ClearOutDirectory checkbox
        public void ClearOutpuDirectoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(_fileSystem))
                ch.SetProperty(ConfigHandlerKeys.clearOutputDirectory, ClearOutpuDirectoryCheckBox.Checked);
        }
    }
}
