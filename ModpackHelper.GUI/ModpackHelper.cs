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
using Debug = ModpackHelper.Shared.Utils.Debug;

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
            
            // Get minecraft versions
            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);
            if (forgeHandler.GetMinecraftVersions().Count < 5)
            {
                forgeHandler.DownloadForgeVersions();
            }
            // ReSharper disable once CoVariantArrayConversion
            MinecraftVersionDropdown.Items.AddRange(forgeHandler.GetMinecraftVersions().ToArray());
            MinecraftVersionDropdown.SelectedIndex = MinecraftVersionDropdown.Items.Count - 1;

            // Reload the interface
            ConfigLoader cl = new ConfigLoader(this, fileSystem);
            cl.ReloadConfigs();
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
                {
                    if (string.IsNullOrWhiteSpace(ModpackNameTextBox.Text))
                    {
                        return;
                    }
                    if (!ch.Configs.Modpacks.ContainsKey(ModpackNameTextBox.Text))
                    {
                        ch.Configs.Modpacks.Add(ModpackNameTextBox.Text,
                            new Modpack
                            {
                                InputDirectory = i,
                                Name = ModpackNameTextBox.Text
                            });
                    }
                    else
                    {
                        ch.Configs.Modpacks[ModpackNameTextBox.Text].InputDirectory = i;
                    }
                }
            }
            else
                messageShower.ShowMessage(Resources.InputHasToBeModDirectory);
        }

        // Called when the user clicks the browser button for the output directory
        public void browseForOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            OutputDirectoryTextBox.Text = directoryFinder.GetDirectory(Resources.SelectOutputDirectory, OutputDirectoryTextBox.Text);
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
            bool checkPermissions = createTechnicPack && CheckTechnicPermissionsCheckBox.Checked;
            // Indicates if this is a public pack
            bool privatePack = checkPermissions && technicPermissionsPrivatePack.Checked;
            // Indicates if a forge zip should be created
            bool createForgeZip = createTechnicPack && createForgeZipCheckBox.Checked;
            // Indicates if we should clear the output directory before packing
            bool clearOutputDirectory = ClearOutpuDirectoryCheckBox.Checked;

            // Indicates if we should upload to an FTP server
            bool uploadToFTP = UploadToFTPCheckbox.Checked;
            if (uploadToFTP)
            {
                var ch = new ConfigHandler(fileSystem);
                if (ch.Configs.FTPLoginInfo == null || !ch.Configs.FTPLoginInfo.IsValid())
                {
                    messageShower.ShowMessageAsync("You have chosen to upload to ftp, but ftp has not yet been configured");
                    valid = false;
                }
            }

            // Indicates if we should update a solder install
            bool useSolder = UseSolderCheckbox.Checked;
            if (useSolder)
            {
                // Make sure the user has actually configured solder first
                var ch = new ConfigHandler(fileSystem);
                if (ch.Configs.SolderLoginInfo == null || !ch.Configs.SolderLoginInfo.IsValid())
                {
                    messageShower.ShowMessageAsync("You have chosen to update solder, but solder has not yet been configured");
                    valid = false;
                }
            }

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

            // Check if we had any errors along the way
            if (!valid) return;

            Modpack modpack;
            // Save the modpack data locally
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                // Load the configs
                Configs c = ch.Configs;

                // Locate data about this modpack, or create some new data
                modpack = c.Modpacks.ContainsKey(modpackName) ? c.Modpacks[modpackName] : new Modpack();

                // Save all the data
                modpack.Name = modpackName;
                modpack.CreateConfigZip = createConfigZip;
                modpack.CreateForgeZip = createForgeZip;
                modpack.CreateTechnicPack = createTechnicPack;
                modpack.InputDirectory = inputDirectory;
                modpack.MinecraftVersion = minecraftVersion;
                modpack.OutputDirectory = outputdirectory;
                modpack.TechnicPermissionsPrivate = privatePack;
                modpack.ClearOutputDirectory = clearOutputDirectory;
                modpack.CheckTechnicPermissions = checkPermissions;
                modpack.CreateSolderPack = createSolderPack;
                modpack.ForgeVersion = selectedForgeVersion;
                modpack.UseSolder = useSolder;
                modpack.UploadToFTP = uploadToFTP;
                modpack.Version = modpackVersion;

                // Change the last selected pack, so we know what pack to load on startup
                c.LastSelectedModpack = modpackName;

                // Save the modpack
                if (!c.Modpacks.ContainsKey(modpackName))
                {
                    c.Modpacks.Add(modpackName, modpack);
                }
            }

            // We are free to continue
            ModExtractor modExtractor = new ModExtractor(minecraftVersion, fileSystem);

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
                Debug.WriteLine(sw.Elapsed.ToString());
                OpenModsInfoForm(mods, modpack);

            };
            bw.RunWorkerAsync();

            
        }


        private void OpenModsInfoForm(List<Mcmod> mods, Modpack modpack)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OpenModsInfoForm(mods, modpack)));
            }
            else
            {
                ModInfoForm form = new ModInfoForm(fileSystem, messageShower);
                form.InitializeContent(mods, modpack.MinecraftVersion);
                form.Show();
                form.DoneFillingInInfo += delegate (List<Mcmod> modslist)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (sender, args) =>
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        ModPacker packer = new ModPacker(fileSystem);
                        packer.Pack(modslist, modpack);
                        sw.Stop();
                        Debug.WriteLine(sw.Elapsed.ToString());
                        string html = packer.GetFinishedHTML();
                        fileSystem.File.WriteAllText(fileSystem.Path.Combine(modpack.OutputDirectory, "mods.html"), html);
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

        private void UseSolderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ConfigHandler ch = new ConfigHandler(fileSystem);
            if (ch.Configs.SolderLoginInfo == null && UseSolderCheckbox.Checked)
            {
                messageShower.ShowMessageAsync("This feature requires that Solder is run under MySQL. " + Environment.NewLine + "The Solder connection has not been configured, please do so. ");
            }

            SolderConfigurePanel.Visible = UseSolderCheckbox.Checked;
        }

        private void ConfigureSolderButton_Click(object sender, EventArgs e)
        {
            MySQLConnectForm f = new MySQLConnectForm(fileSystem, messageShower);
            f.ShowDialog(this);
        }

        private void UploadToFTPCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ConfigureFTPButton.Visible = UploadToFTPCheckbox.Checked;
        }

        private void ConfigureFTPButton_Click(object sender, EventArgs e)
        {
            FTPConnectForm f = new FTPConnectForm(fileSystem, messageShower);
            f.ShowDialog(this);
        }

        private void ModpackNameTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                if (ch.Configs.Modpacks.ContainsKey(ModpackNameTextBox.SelectedText))
                {
                    Modpack modpack = ch.Configs.Modpacks[ModpackNameTextBox.SelectedText];
                    MinecraftVersionDropdown.SelectedIndex = MinecraftVersionDropdown.Items.IndexOf(modpack.MinecraftVersion);
                    InputDirectoryTextBox.Text = modpack.InputDirectory;
                    OutputDirectoryTextBox.Text = modpack.OutputDirectory;
                    createForgeZipCheckBox.Checked = modpack.CreateForgeZip;
                    CreateConfigZipCheckBox.Checked = modpack.CreateConfigZip;
                    CreateTechnicPackCheckBox.Checked = modpack.CreateTechnicPack;
                    ClearOutpuDirectoryCheckBox.Checked = modpack.ClearOutputDirectory;
                    CheckTechnicPermissionsCheckBox.Checked = modpack.CheckTechnicPermissions;
                    SolderPackRadioButton.Checked = modpack.CreateSolderPack;
                    technicPermissionsPrivatePack.Checked = modpack.TechnicPermissionsPrivate;
                    if (!string.IsNullOrWhiteSpace(modpack.ForgeVersion))
                        forgeVersionDropdown.SelectedIndex = forgeVersionDropdown.Items.IndexOf(modpack.ForgeVersion);
                }
            }
        }

        private void CheckTechnicPermissionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            technicPermissionsLevelGroupBox.Visible = CheckTechnicPermissionsCheckBox.Checked;
        }

        private void createForgeZipCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            forgeVersionDropdown.Visible = createForgeZipCheckBox.Checked;
            forgeVersionLabel.Visible = createForgeZipCheckBox.Checked;
        }

        private void EnableDebugCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Debug.OutputDebug = EnableDebugCheckbox.Checked;
        }
    }
}
