using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Windows.Forms;
using ModpackHelper.GUI.Helpers;
using ModpackHelper.GUI.Properties;
using ModpackHelper.GUI.UserInteraction;
using ModpackHelper.GUI.Windows;
using ModpackHelper.IO;
using ModpackHelper.MinecraftForge;
using ModpackHelper.Shared.IO;
using ModpackHelper.Shared.MinecraftForge;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.UserInteraction;

namespace ModpackHelper.GUI
{
    public partial class ModpackHelper : Form
    {
        private readonly IFileSystem fileSystem;
        private readonly IDirectoryFinder directoryFinder;
        private readonly IMessageShower messageShower;
        // Normal constructor
        public ModpackHelper() : this(new FileSystem(), new DirectoryFinder(), new MessageShower())
        {

        }

        // Unit testing constructor
        public ModpackHelper(IFileSystem fileSystem, IDirectoryFinder finder, IMessageShower messageShower)
        {
            this.fileSystem = fileSystem;
            directoryFinder = finder;
            this.messageShower = messageShower;
            InitializeComponent();

            // Reload the interface
            ConfigLoader cl = new ConfigLoader(this, fileSystem);
            cl.ReloadConfigs();

            // Get minecraft versions
            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);
            if (forgeHandler.GetMinecraftVersions().Count < 5)
            {
                forgeHandler.DownloadForgeVersions();
            }
            // ReSharper disable once CoVariantArrayConversion
            MinecraftVersionDropdown.Items.AddRange(forgeHandler.GetMinecraftVersions().ToArray());
            MinecraftVersionDropdown.SelectedIndex = MinecraftVersionDropdown.Items.Count-1;
        }

        // Called when the user clicks the browse button for the input directory
        public void browseForInputDirectoryButton_Click(object sender, EventArgs e)
        {
            string i = directoryFinder.GetDirectory(Resources.SelectTheInputDirectory, InputDirectoryTextBox.Text);
            // The input directory should always be a /mods/ directory
            if (i.EndsWith(Path.DirectorySeparatorChar + "mods"))
            {
                InputDirectoryTextBox.Text = i;
                using (ConfigHandler ch = new ConfigHandler(fileSystem))
                    if (string.IsNullOrWhiteSpace(ModpackNameTextBox.Text))
                        ch.Configs.InputDirectory = i;
                    else if (!ch.Configs.Modpacks.ContainsKey(ModpackNameTextBox.Text))
                        ch.Configs.Modpacks.Add(ModpackNameTextBox.Text, new Modpack { InputDirectory = i, Name = ModpackNameTextBox.Text });
                    else
                        ch.Configs.Modpacks[ModpackNameTextBox.Text].InputDirectory = i;
            }
            else
                messageShower.ShowMessage(Resources.InputHasToBeModDirectory);
        }

        // Called when the user clicks the browser button for the output directory
        public void browseForOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            OutputDirectoryTextBox.Text = directoryFinder.GetDirectory(Resources.SelectOutputDirectory, OutputDirectoryTextBox.Text);
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                ch.Configs.OutputDirectory = OutputDirectoryTextBox.Text;
        }

        // Called when the user checks or unchecks the ClearOutDirectory checkbox
        public void ClearOutpuDirectoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                ch.Configs.ClearOutputDirectory = ClearOutpuDirectoryCheckBox.Checked;
        }

        private void CreateTechnicPackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                ch.Configs.CreateTechnicPack = CreateTechnicPackCheckBox.Checked;

        }

        private void CreateConfigZipCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                ch.Configs.CreateConfigZip = CreateConfigZipCheckBox.Checked;
        }

        private void createForgeZipCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                ch.Configs.CreateForgeZip = createForgeZipCheckBox.Checked;
        }

        private void technicPermissionsPrivatePack_CheckedChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                ch.Configs.TechnicPermissionsPrivate = technicPermissionsPrivatePack.Checked;
        }

        private void ModpackNameTextBox_LostFocus(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                string text = ModpackNameTextBox.Text;
                if (!string.IsNullOrWhiteSpace(text))
                {
                    if (ch.Configs.Modpacks.ContainsKey(text))
                    {
                        ch.Configs.Modpacks[text].Name = text;
                    }
                    else
                    {
                        ch.Configs.Modpacks.Add(text, new Modpack { Name = text });
                    }
                }
                ch.Configs.ModpackName = text;
            }
        }

        private void startPackingButton_Click(object sender, EventArgs e)
        {
            // Indicates that everything is valid and the packing can continue
            bool valid = true;
            // Indicates if a technic pack should be created
            bool createTechnicPack = CreateTechnicPackCheckBox.Checked;
            // Indicates if it should be a solderpack or a zip pack
            bool createSolderPack = createTechnicPack && SolderPackRadioButton.Checked;
            // Indicates if a config zip should be included in the modpack
            bool createConfigZip = createTechnicPack && CreateConfigZipCheckBox.Checked;
            // Indicates if permissions should be check
            bool checkPermissions = createTechnicPack && CheckPermissionsCheckBox.Checked;
            // Indicates if this is a public pack
            bool publicPack = checkPermissions && technicPermissionsPublicPack.Checked;
            // Indicates if a forge zip should be created
            bool createForgeZip = createTechnicPack && createForgeZipCheckBox.Checked;
            // Indicates what the selected forge version is
            string selectedForgeVersion = createTechnicPack && createForgeZip ? forgeVersionDropdown.SelectedItem.ToString() : null;
            if (createForgeZip && string.IsNullOrWhiteSpace(selectedForgeVersion))
            {
                messageShower.ShowMessageAsync("You have chosen to include forge, but hasn't specified a version. ");
                valid = false;
            }

            // Get the inputdirectory and validate it
            string inputDirectory = InputDirectoryTextBox.Text;
            if (!fileSystem.Directory.Exists(inputDirectory))
            {
                messageShower.ShowMessageAsync("The specified inputdirectory does not exist.");
                valid = false;
            }

            // Get the output directory
            string outputdirectory = OutputDirectoryTextBox.Text;

            // Get the modpackname and validate it
            string modpackName = ModpackNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(modpackName))
            {
                messageShower.ShowMessageAsync("You need to specify a modpack name.");
                valid = false;
            }

            // Get the minecraft version and validate it
            string minecraftVersion = MinecraftVersionDropdown.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(minecraftVersion))
            {
                messageShower.ShowMessageAsync("You need to specify a minecraft version");
                valid = false;
            }

            // Get the modpack version and validate it
            string modpackVersion = ModpackVersionTextbox.Text;
            if (string.IsNullOrWhiteSpace(modpackVersion))
            {
                messageShower.ShowMessageAsync("You need to specify a modpack version");
                valid = false;
            }

            if (!valid) return;

            // We are free to continue
            
            ModExtractor modExtractor = new ModExtractor(fileSystem);
            
            // Make sure we don't lock the main thread
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                // Measure the time it takes to get all the modinfo
                Stopwatch sw = new Stopwatch();
                sw.Start();
                // Get mod info of the mods in the input directory
                List<Mcmod> mods = modExtractor.FindAllMods(inputDirectory);
                sw.Stop();
                Debug.WriteLine(sw.Elapsed);
                OpenModsInfoForm(mods, minecraftVersion, outputdirectory);
                
            };
            bw.RunWorkerAsync();
        }

        
        private void OpenModsInfoForm(List<Mcmod> mods, string minecraftVersion, string outputDirectory)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OpenModsInfoForm(mods, minecraftVersion, outputDirectory)));
            }
            else
            {
                ModInfoForm form = new ModInfoForm(fileSystem, messageShower);
                form.InitializeContent(mods, minecraftVersion);
                form.Show();
                form.DoneFillingInInfo += delegate(List<Mcmod> modslist)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (sender, args) =>
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        ModPacker packer = new ModPacker(fileSystem);
                        packer.Pack(modslist, fileSystem.DirectoryInfo.FromDirectoryName(outputDirectory));
                        sw.Stop();
                        Debug.WriteLine(sw.Elapsed);
                        string html = packer.GetFinishedHTML();
                        fileSystem.File.WriteAllText(fileSystem.Path.Combine(outputDirectory, "mods.html"), html);
                        messageShower.ShowMessageAsync("Done packing mods");
                    };
                    bw.RunWorkerAsync();
                };
            }
        }
        
 
        private void MinecraftVersionDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ModpackNameTextBox.Text)) return;
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
                if (ch.Configs.Modpacks.ContainsKey(ModpackNameTextBox.Text))
                    ch.Configs.Modpacks[ModpackNameTextBox.Text].MinecraftVersion =
                        MinecraftVersionDropdown.SelectedItem.ToString();
                else
                    ch.Configs.Modpacks.Add(ModpackNameTextBox.Text,
                        new Modpack
                        {
                            MinecraftVersion = MinecraftVersionDropdown.SelectedItem.ToString(),
                            Name = ModpackNameTextBox.Text
                        });
        }
    }
}
