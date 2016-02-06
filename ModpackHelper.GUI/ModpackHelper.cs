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
        private Timer animationTimer;
        private Dictionary<string, int> animationGoals = new Dictionary<string, int>();
        private bool isRunningBackgroundTask = false;
        // Normal constructor
        public ModpackHelper() : this(new FileSystem(), new DirectoryFinder(), new MessageShower())
        {

        }

        // Unit testing constructor
        public ModpackHelper(IFileSystem fileSystem, IDirectoryFinder finder, IMessageShower messageShower)
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.animationTimer = new Timer();
            this.animationTimer.Tick += AnimationTimerOnTick;
            this.animationTimer.Interval = 1000 / 144;

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

            // Ensure that the window is big enough to contain all the current controls
            EnsureWindowSize();

            // Make sure there is something in the forge version list
            
        }

        /// <summary>
        /// Handles any animations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            // Ensure right of window is correctly sized
            int rightGoal;
            bool couldGet = animationGoals.TryGetValue("right", out rightGoal);
            if (couldGet)
                if (rightGoal < Width)
                    Width--;
                else if (rightGoal > Width)
                    Width++;
                else
                    animationGoals.Remove("right");

            // Ensure bottom of window is correctly sized
            int bottom;
            couldGet = animationGoals.TryGetValue("bottom", out bottom);
            if (couldGet)
                if (bottom < Height)
                    Height--;
                else if (bottom > Height)
                    Height++;
                else
                    animationGoals.Remove("bottom");

            // Make sure we only animate when there is anything to animate
            animationTimer.Enabled = animationGoals.Count > 0;
        }

        /// <summary>
        /// Ensures the window is at least big enough to contain all the currently visible controls
        /// </summary>
        private void EnsureWindowSize()
        {
            // Check the global configuration group box size
            int right = globalConfigurationsGroupBox.Location.X + globalConfigurationsGroupBox.Size.Width;
            if (right + 25 > Width)
            {
                animationTimer.Enabled = true;
                if (!animationGoals.ContainsKey("right"))
                    animationGoals.Add("right", right + 25);
            }

            // Check the technic options box
            int bottom = technicOptionsGroupBox.Location.Y + technicOptionsGroupBox.Size.Height + statusStrip1.Size.Height;
            if (bottom + 50 > Height)
            {
                animationTimer.Enabled = true;
                if (!animationGoals.ContainsKey("bottom"))
                    animationGoals.Add("bottom", bottom + 50);
            }
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
            // Indicates if we should force a solder update no matter what
            bool forceSolder = ForceSolderUpdateCheckBox.Checked;

            // Indicates if we should upload to an FTP server
            bool uploadToFtp = UploadToFTPCheckbox.Checked;
            if (uploadToFtp)
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

            string minJava = "";
            if (useSolder)
            {
                minJava = MinimumJavaVersionCombobox.SelectedText;
            }

            string minMemory = "0";
            if (useSolder)
            {
                int m;
                minMemory = minimumMemoryTextBox.Text;
                bool parsed = int.TryParse(minMemory, out m);
                if (!parsed || m < 0)
                {
                    messageShower.ShowMessageAsync("The entered minimum memory is not a valid value.");
                    valid = false;
                }
                if (parsed && m == 0)
                {
                    minMemory = "";
                }
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
                modpack.UploadToFTP = uploadToFtp;
                modpack.Version = modpackVersion;
                modpack.MinJava = minJava;
                modpack.MinMemory = minMemory;
                modpack.ForceSolder = forceSolder;

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

            isRunningBackgroundTask = true;
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
                isRunningBackgroundTask = false;
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
                    isRunningBackgroundTask = true;
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
                        isRunningBackgroundTask = false;
                        if (modpack.UploadToFTP)
                        {
                            UploadToFtp(mods, modpack);
                        }
                        if (modpack.CreateSolderPack && modpack.UseSolder)
                        {
                            UpdateSolder(mods, modpack);
                        }
                    };
                    bw.RunWorkerAsync();
                };
            }
        }

        private void UpdateSolder(List<Mcmod> mods, Modpack modpack)
        {
            isRunningBackgroundTask = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                var ch = new ConfigHandler(fileSystem);
                var sli = ch.Configs.SolderLoginInfo;
                var s = new Solder(fileSystem);
                s.DoneUpdatingMods += () =>
                {
                    isRunningBackgroundTask = false;
                    ShowMessage("Done updating solder");
                };
                s.Initialize(sli.Username, sli.Password, mods, modpack);
                s.Update(sli.Address);
            };
            bw.RunWorkerAsync();
        }

        private void ShowMessage(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowMessage(message)));
            }
            else
            {
                messageShower.ShowMessageAsync(message);
            }
        }

        private void UploadToFtp(List<Mcmod> mods, Modpack modpack)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UploadToFtp(mods, modpack)));
            }
            else
            {
                isRunningBackgroundTask = true;
                FtpUploaderForm f = new FtpUploaderForm(fileSystem.DirectoryInfo.FromDirectoryName(Path.Combine(modpack.OutputDirectory, "mods")));
                f.ShowDialog(this);
                isRunningBackgroundTask = false;
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
                messageShower.ShowMessageAsync("The Solder connection has not been configured, please do so. ");
            }

            SolderConfigurePanel.Visible = UseSolderCheckbox.Checked;
        }

        private void ConfigureSolderButton_Click(object sender, EventArgs e)
        {
            SolderConnectForm f = new SolderConnectForm(fileSystem, messageShower);
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

        private void CreateTechnicPackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            technicOptionsGroupBox.Visible = CreateTechnicPackCheckBox.Checked;
        }
        
        private void formClosingHandler(object sender, CancelEventArgs e)
        {
            if (!isRunningBackgroundTask) return;
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to close the application. Is it currently running a background task, which can cause issues if canceled.",
                    "Confirm exit", MessageBoxButtons.OKCancel);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void FillForgeDropdown()
        {
            using (var forgeHandler = new ForgeHandler(fileSystem))
            {
                List<int> versions = forgeHandler.GetForgeBuilds(MinecraftVersionDropdown.SelectedText);
                forgeVersionDropdown.Items.Clear();
                foreach (int version in versions)
                {
                    forgeVersionDropdown.Items.Add(version);
                }
                forgeVersionDropdown.SelectedIndex = forgeVersionDropdown.Items.Count - 1;
            }
        }
    }
}
