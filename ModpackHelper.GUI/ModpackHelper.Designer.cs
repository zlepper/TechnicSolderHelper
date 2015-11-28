using System.ComponentModel;
using System.Windows.Forms;

namespace ModpackHelper.GUI
{
    partial class ModpackHelper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label inputFolderLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label ModpackNameLabel;
            System.Windows.Forms.Label ModpackVersionLabel;
            System.Windows.Forms.Label MinecraftVersionLabel;
            System.Windows.Forms.Label minimumMemoryLabel;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.ToolStripStatusLabel currentlyDoingLabel;
            this.InputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.browseForInputDirectoryButton = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.browseForOutputDirectoryButton = new System.Windows.Forms.Button();
            this.startPackingButton = new System.Windows.Forms.Button();
            this.globalConfigurationsGroupBox = new System.Windows.Forms.GroupBox();
            this.EnableDebugCheckbox = new System.Windows.Forms.CheckBox();
            this.ModpackSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.MinecraftVersionDropdown = new System.Windows.Forms.ComboBox();
            this.ModpackVersionTextbox = new System.Windows.Forms.TextBox();
            this.ModpackNameTextBox = new System.Windows.Forms.ComboBox();
            this.ClearOutpuDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.getForgeVersionsButton = new System.Windows.Forms.Button();
            this.CreateFTBPackCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.CreateTechnicPackCheckBox = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.technicOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.SolderConfigurePanel = new System.Windows.Forms.Panel();
            this.ConfigureSolderButton = new System.Windows.Forms.Button();
            this.ForceSolderUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.minimumMemoryTextBox = new System.Windows.Forms.TextBox();
            this.MinimumJavaVersionCombobox = new System.Windows.Forms.ComboBox();
            this.UploadToFTPCheckbox = new System.Windows.Forms.CheckBox();
            this.ConfigureFTPButton = new System.Windows.Forms.Button();
            this.UseSolderCheckbox = new System.Windows.Forms.CheckBox();
            this.forgeVersionLabel = new System.Windows.Forms.Label();
            this.forgeVersionDropdown = new System.Windows.Forms.ComboBox();
            this.PackTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.ZipPackRadioButton = new System.Windows.Forms.RadioButton();
            this.SolderPackRadioButton = new System.Windows.Forms.RadioButton();
            this.technicPermissionsLevelGroupBox = new System.Windows.Forms.GroupBox();
            this.technicPermissionsPublicPack = new System.Windows.Forms.RadioButton();
            this.technicPermissionsPrivatePack = new System.Windows.Forms.RadioButton();
            this.CheckTechnicPermissionsCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateConfigZipCheckBox = new System.Windows.Forms.CheckBox();
            this.createForgeZipCheckBox = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            inputFolderLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ModpackNameLabel = new System.Windows.Forms.Label();
            ModpackVersionLabel = new System.Windows.Forms.Label();
            MinecraftVersionLabel = new System.Windows.Forms.Label();
            minimumMemoryLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            currentlyDoingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.globalConfigurationsGroupBox.SuspendLayout();
            this.ModpackSettingsGroupBox.SuspendLayout();
            this.technicOptionsGroupBox.SuspendLayout();
            this.SolderConfigurePanel.SuspendLayout();
            this.PackTypeGroupBox.SuspendLayout();
            this.technicPermissionsLevelGroupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputFolderLabel
            // 
            inputFolderLabel.AutoSize = true;
            inputFolderLabel.Location = new System.Drawing.Point(549, 38);
            inputFolderLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            inputFolderLabel.Name = "inputFolderLabel";
            inputFolderLabel.Size = new System.Drawing.Size(166, 32);
            inputFolderLabel.TabIndex = 0;
            inputFolderLabel.Text = "Input Folder";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(549, 136);
            label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(189, 32);
            label1.TabIndex = 0;
            label1.Text = "Output Folder";
            // 
            // ModpackNameLabel
            // 
            ModpackNameLabel.AutoSize = true;
            ModpackNameLabel.Location = new System.Drawing.Point(16, 38);
            ModpackNameLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            ModpackNameLabel.Name = "ModpackNameLabel";
            ModpackNameLabel.Size = new System.Drawing.Size(212, 32);
            ModpackNameLabel.TabIndex = 0;
            ModpackNameLabel.Text = "Modpack Name";
            // 
            // ModpackVersionLabel
            // 
            ModpackVersionLabel.AutoSize = true;
            ModpackVersionLabel.Location = new System.Drawing.Point(16, 131);
            ModpackVersionLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            ModpackVersionLabel.Name = "ModpackVersionLabel";
            ModpackVersionLabel.Size = new System.Drawing.Size(234, 32);
            ModpackVersionLabel.TabIndex = 0;
            ModpackVersionLabel.Text = "Modpack Version";
            // 
            // MinecraftVersionLabel
            // 
            MinecraftVersionLabel.AutoSize = true;
            MinecraftVersionLabel.Location = new System.Drawing.Point(16, 224);
            MinecraftVersionLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            MinecraftVersionLabel.Name = "MinecraftVersionLabel";
            MinecraftVersionLabel.Size = new System.Drawing.Size(236, 32);
            MinecraftVersionLabel.TabIndex = 0;
            MinecraftVersionLabel.Text = "Minecraft Version";
            // 
            // minimumMemoryLabel
            // 
            minimumMemoryLabel.AutoSize = true;
            minimumMemoryLabel.Location = new System.Drawing.Point(0, 86);
            minimumMemoryLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            minimumMemoryLabel.Name = "minimumMemoryLabel";
            minimumMemoryLabel.Size = new System.Drawing.Size(335, 32);
            minimumMemoryLabel.TabIndex = 15;
            minimumMemoryLabel.Text = "Minimum Memory (in MB)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(0, 179);
            label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(287, 32);
            label2.TabIndex = 15;
            label2.Text = "MinimumJavaVersion";
            // 
            // InputDirectoryTextBox
            // 
            this.InputDirectoryTextBox.Location = new System.Drawing.Point(549, 76);
            this.InputDirectoryTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.InputDirectoryTextBox.Name = "InputDirectoryTextBox";
            this.InputDirectoryTextBox.Size = new System.Drawing.Size(1268, 38);
            this.InputDirectoryTextBox.TabIndex = 1;
            // 
            // browseForInputDirectoryButton
            // 
            this.browseForInputDirectoryButton.Location = new System.Drawing.Point(1840, 74);
            this.browseForInputDirectoryButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.browseForInputDirectoryButton.Name = "browseForInputDirectoryButton";
            this.browseForInputDirectoryButton.Size = new System.Drawing.Size(200, 55);
            this.browseForInputDirectoryButton.TabIndex = 1;
            this.browseForInputDirectoryButton.Text = "Browse";
            this.browseForInputDirectoryButton.UseVisualStyleBackColor = true;
            this.browseForInputDirectoryButton.Click += new System.EventHandler(this.browseForInputDirectoryButton_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(549, 174);
            this.OutputDirectoryTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(1268, 38);
            this.OutputDirectoryTextBox.TabIndex = 2;
            // 
            // browseForOutputDirectoryButton
            // 
            this.browseForOutputDirectoryButton.Location = new System.Drawing.Point(1840, 172);
            this.browseForOutputDirectoryButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.browseForOutputDirectoryButton.Name = "browseForOutputDirectoryButton";
            this.browseForOutputDirectoryButton.Size = new System.Drawing.Size(200, 55);
            this.browseForOutputDirectoryButton.TabIndex = 1;
            this.browseForOutputDirectoryButton.Text = "Browse";
            this.browseForOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.browseForOutputDirectoryButton.Click += new System.EventHandler(this.browseForOutputDirectoryButton_Click);
            // 
            // startPackingButton
            // 
            this.startPackingButton.Location = new System.Drawing.Point(1573, 513);
            this.startPackingButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.startPackingButton.Name = "startPackingButton";
            this.startPackingButton.Size = new System.Drawing.Size(208, 153);
            this.startPackingButton.TabIndex = 2;
            this.startPackingButton.Text = "Start";
            this.startPackingButton.UseVisualStyleBackColor = true;
            this.startPackingButton.Click += new System.EventHandler(this.startPackingButton_Click);
            // 
            // globalConfigurationsGroupBox
            // 
            this.globalConfigurationsGroupBox.Controls.Add(this.EnableDebugCheckbox);
            this.globalConfigurationsGroupBox.Controls.Add(this.ModpackSettingsGroupBox);
            this.globalConfigurationsGroupBox.Controls.Add(inputFolderLabel);
            this.globalConfigurationsGroupBox.Controls.Add(this.ClearOutpuDirectoryCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.InputDirectoryTextBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.getForgeVersionsButton);
            this.globalConfigurationsGroupBox.Controls.Add(this.CreateFTBPackCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.browseForOutputDirectoryButton);
            this.globalConfigurationsGroupBox.Controls.Add(this.button1);
            this.globalConfigurationsGroupBox.Controls.Add(this.CreateTechnicPackCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.button2);
            this.globalConfigurationsGroupBox.Controls.Add(this.OutputDirectoryTextBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.browseForInputDirectoryButton);
            this.globalConfigurationsGroupBox.Controls.Add(this.button4);
            this.globalConfigurationsGroupBox.Controls.Add(this.button3);
            this.globalConfigurationsGroupBox.Controls.Add(label1);
            this.globalConfigurationsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.globalConfigurationsGroupBox.Location = new System.Drawing.Point(32, 29);
            this.globalConfigurationsGroupBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.globalConfigurationsGroupBox.Name = "globalConfigurationsGroupBox";
            this.globalConfigurationsGroupBox.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.globalConfigurationsGroupBox.Size = new System.Drawing.Size(2064, 455);
            this.globalConfigurationsGroupBox.TabIndex = 3;
            this.globalConfigurationsGroupBox.TabStop = false;
            this.globalConfigurationsGroupBox.Text = "Global Configurations";
            // 
            // EnableDebugCheckbox
            // 
            this.EnableDebugCheckbox.AutoSize = true;
            this.EnableDebugCheckbox.Location = new System.Drawing.Point(1560, 305);
            this.EnableDebugCheckbox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.EnableDebugCheckbox.Name = "EnableDebugCheckbox";
            this.EnableDebugCheckbox.Size = new System.Drawing.Size(234, 36);
            this.EnableDebugCheckbox.TabIndex = 12;
            this.EnableDebugCheckbox.Text = "Enable Debug";
            this.EnableDebugCheckbox.UseVisualStyleBackColor = true;
            this.EnableDebugCheckbox.CheckedChanged += new System.EventHandler(this.EnableDebugCheckbox_CheckedChanged);
            // 
            // ModpackSettingsGroupBox
            // 
            this.ModpackSettingsGroupBox.Controls.Add(this.MinecraftVersionDropdown);
            this.ModpackSettingsGroupBox.Controls.Add(MinecraftVersionLabel);
            this.ModpackSettingsGroupBox.Controls.Add(this.ModpackVersionTextbox);
            this.ModpackSettingsGroupBox.Controls.Add(ModpackVersionLabel);
            this.ModpackSettingsGroupBox.Controls.Add(this.ModpackNameTextBox);
            this.ModpackSettingsGroupBox.Controls.Add(ModpackNameLabel);
            this.ModpackSettingsGroupBox.Location = new System.Drawing.Point(16, 45);
            this.ModpackSettingsGroupBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ModpackSettingsGroupBox.Name = "ModpackSettingsGroupBox";
            this.ModpackSettingsGroupBox.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ModpackSettingsGroupBox.Size = new System.Drawing.Size(517, 336);
            this.ModpackSettingsGroupBox.TabIndex = 2;
            this.ModpackSettingsGroupBox.TabStop = false;
            this.ModpackSettingsGroupBox.Text = "Modpack Settings";
            // 
            // MinecraftVersionDropdown
            // 
            this.MinecraftVersionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MinecraftVersionDropdown.FormattingEnabled = true;
            this.MinecraftVersionDropdown.Location = new System.Drawing.Point(16, 262);
            this.MinecraftVersionDropdown.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MinecraftVersionDropdown.Name = "MinecraftVersionDropdown";
            this.MinecraftVersionDropdown.Size = new System.Drawing.Size(479, 39);
            this.MinecraftVersionDropdown.TabIndex = 5;
            this.MinecraftVersionDropdown.SelectedIndexChanged += new System.EventHandler(this.MinecraftVersionDropdown_SelectedIndexChanged);
            // 
            // ModpackVersionTextbox
            // 
            this.ModpackVersionTextbox.Location = new System.Drawing.Point(16, 169);
            this.ModpackVersionTextbox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ModpackVersionTextbox.Name = "ModpackVersionTextbox";
            this.ModpackVersionTextbox.Size = new System.Drawing.Size(479, 38);
            this.ModpackVersionTextbox.TabIndex = 4;
            // 
            // ModpackNameTextBox
            // 
            this.ModpackNameTextBox.Location = new System.Drawing.Point(16, 76);
            this.ModpackNameTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ModpackNameTextBox.Name = "ModpackNameTextBox";
            this.ModpackNameTextBox.Size = new System.Drawing.Size(479, 39);
            this.ModpackNameTextBox.TabIndex = 3;
            this.ModpackNameTextBox.SelectedIndexChanged += new System.EventHandler(this.ModpackNameTextBox_SelectedIndexChanged);
            // 
            // ClearOutpuDirectoryCheckBox
            // 
            this.ClearOutpuDirectoryCheckBox.AutoSize = true;
            this.ClearOutpuDirectoryCheckBox.Checked = true;
            this.ClearOutpuDirectoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClearOutpuDirectoryCheckBox.Location = new System.Drawing.Point(1560, 246);
            this.ClearOutpuDirectoryCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ClearOutpuDirectoryCheckBox.Name = "ClearOutpuDirectoryCheckBox";
            this.ClearOutpuDirectoryCheckBox.Size = new System.Drawing.Size(411, 36);
            this.ClearOutpuDirectoryCheckBox.TabIndex = 11;
            this.ClearOutpuDirectoryCheckBox.Text = "Clear output directory on run";
            this.ClearOutpuDirectoryCheckBox.UseVisualStyleBackColor = true;
            // 
            // getForgeVersionsButton
            // 
            this.getForgeVersionsButton.Location = new System.Drawing.Point(549, 236);
            this.getForgeVersionsButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.getForgeVersionsButton.Name = "getForgeVersionsButton";
            this.getForgeVersionsButton.Size = new System.Drawing.Size(427, 55);
            this.getForgeVersionsButton.TabIndex = 3;
            this.getForgeVersionsButton.Text = "Get Forge/Minecraft versions";
            this.getForgeVersionsButton.UseVisualStyleBackColor = true;
            // 
            // CreateFTBPackCheckBox
            // 
            this.CreateFTBPackCheckBox.AutoSize = true;
            this.CreateFTBPackCheckBox.Location = new System.Drawing.Point(912, 377);
            this.CreateFTBPackCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CreateFTBPackCheckBox.Name = "CreateFTBPackCheckBox";
            this.CreateFTBPackCheckBox.Size = new System.Drawing.Size(268, 36);
            this.CreateFTBPackCheckBox.TabIndex = 7;
            this.CreateFTBPackCheckBox.Text = "Create FTB Pack";
            this.CreateFTBPackCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(549, 305);
            this.button1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(427, 55);
            this.button1.TabIndex = 4;
            this.button1.Text = "Update stored permissions";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CreateTechnicPackCheckBox
            // 
            this.CreateTechnicPackCheckBox.AutoSize = true;
            this.CreateTechnicPackCheckBox.Checked = true;
            this.CreateTechnicPackCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateTechnicPackCheckBox.Location = new System.Drawing.Point(557, 377);
            this.CreateTechnicPackCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CreateTechnicPackCheckBox.Name = "CreateTechnicPackCheckBox";
            this.CreateTechnicPackCheckBox.Size = new System.Drawing.Size(315, 36);
            this.CreateTechnicPackCheckBox.TabIndex = 6;
            this.CreateTechnicPackCheckBox.Text = "Create Technic Pack";
            this.CreateTechnicPackCheckBox.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1211, 236);
            this.button2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(333, 55);
            this.button2.TabIndex = 5;
            this.button2.Text = "Repack everything";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(995, 236);
            this.button4.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(200, 55);
            this.button4.TabIndex = 7;
            this.button4.Text = "Edit data";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(995, 305);
            this.button3.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(333, 55);
            this.button3.TabIndex = 6;
            this.button3.Text = "Generate Permissions";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // technicOptionsGroupBox
            // 
            this.technicOptionsGroupBox.Controls.Add(this.SolderConfigurePanel);
            this.technicOptionsGroupBox.Controls.Add(this.UploadToFTPCheckbox);
            this.technicOptionsGroupBox.Controls.Add(this.ConfigureFTPButton);
            this.technicOptionsGroupBox.Controls.Add(this.UseSolderCheckbox);
            this.technicOptionsGroupBox.Controls.Add(this.forgeVersionLabel);
            this.technicOptionsGroupBox.Controls.Add(this.forgeVersionDropdown);
            this.technicOptionsGroupBox.Controls.Add(this.PackTypeGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.technicPermissionsLevelGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.CheckTechnicPermissionsCheckBox);
            this.technicOptionsGroupBox.Controls.Add(this.CreateConfigZipCheckBox);
            this.technicOptionsGroupBox.Controls.Add(this.createForgeZipCheckBox);
            this.technicOptionsGroupBox.Location = new System.Drawing.Point(32, 498);
            this.technicOptionsGroupBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.technicOptionsGroupBox.Name = "technicOptionsGroupBox";
            this.technicOptionsGroupBox.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.technicOptionsGroupBox.Size = new System.Drawing.Size(1523, 524);
            this.technicOptionsGroupBox.TabIndex = 4;
            this.technicOptionsGroupBox.TabStop = false;
            this.technicOptionsGroupBox.Text = "Technic Options";
            // 
            // SolderConfigurePanel
            // 
            this.SolderConfigurePanel.Controls.Add(this.ConfigureSolderButton);
            this.SolderConfigurePanel.Controls.Add(minimumMemoryLabel);
            this.SolderConfigurePanel.Controls.Add(label2);
            this.SolderConfigurePanel.Controls.Add(this.ForceSolderUpdateCheckBox);
            this.SolderConfigurePanel.Controls.Add(this.minimumMemoryTextBox);
            this.SolderConfigurePanel.Controls.Add(this.MinimumJavaVersionCombobox);
            this.SolderConfigurePanel.Location = new System.Drawing.Point(779, 95);
            this.SolderConfigurePanel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.SolderConfigurePanel.Name = "SolderConfigurePanel";
            this.SolderConfigurePanel.Size = new System.Drawing.Size(384, 341);
            this.SolderConfigurePanel.TabIndex = 20;
            this.SolderConfigurePanel.Visible = false;
            // 
            // ConfigureSolderButton
            // 
            this.ConfigureSolderButton.Location = new System.Drawing.Point(8, 17);
            this.ConfigureSolderButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ConfigureSolderButton.Name = "ConfigureSolderButton";
            this.ConfigureSolderButton.Size = new System.Drawing.Size(368, 55);
            this.ConfigureSolderButton.TabIndex = 13;
            this.ConfigureSolderButton.Text = "Configure Solder MySQL";
            this.ConfigureSolderButton.UseVisualStyleBackColor = true;
            this.ConfigureSolderButton.Click += new System.EventHandler(this.ConfigureSolderButton_Click);
            // 
            // ForceSolderUpdateCheckBox
            // 
            this.ForceSolderUpdateCheckBox.AutoSize = true;
            this.ForceSolderUpdateCheckBox.Location = new System.Drawing.Point(3, 284);
            this.ForceSolderUpdateCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ForceSolderUpdateCheckBox.Name = "ForceSolderUpdateCheckBox";
            this.ForceSolderUpdateCheckBox.Size = new System.Drawing.Size(310, 36);
            this.ForceSolderUpdateCheckBox.TabIndex = 17;
            this.ForceSolderUpdateCheckBox.Text = "Force Solder update";
            this.ForceSolderUpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimumMemoryTextBox
            // 
            this.minimumMemoryTextBox.Location = new System.Drawing.Point(8, 124);
            this.minimumMemoryTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.minimumMemoryTextBox.Name = "minimumMemoryTextBox";
            this.minimumMemoryTextBox.Size = new System.Drawing.Size(361, 38);
            this.minimumMemoryTextBox.TabIndex = 14;
            // 
            // MinimumJavaVersionCombobox
            // 
            this.MinimumJavaVersionCombobox.FormattingEnabled = true;
            this.MinimumJavaVersionCombobox.Items.AddRange(new object[] {
            "Java 6",
            "Java 7",
            "Java 8"});
            this.MinimumJavaVersionCombobox.Location = new System.Drawing.Point(8, 219);
            this.MinimumJavaVersionCombobox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MinimumJavaVersionCombobox.Name = "MinimumJavaVersionCombobox";
            this.MinimumJavaVersionCombobox.Size = new System.Drawing.Size(361, 39);
            this.MinimumJavaVersionCombobox.TabIndex = 16;
            // 
            // UploadToFTPCheckbox
            // 
            this.UploadToFTPCheckbox.AutoSize = true;
            this.UploadToFTPCheckbox.Location = new System.Drawing.Point(432, 222);
            this.UploadToFTPCheckbox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.UploadToFTPCheckbox.Name = "UploadToFTPCheckbox";
            this.UploadToFTPCheckbox.Size = new System.Drawing.Size(235, 36);
            this.UploadToFTPCheckbox.TabIndex = 19;
            this.UploadToFTPCheckbox.Text = "Upload to FTP";
            this.UploadToFTPCheckbox.UseVisualStyleBackColor = true;
            this.UploadToFTPCheckbox.CheckedChanged += new System.EventHandler(this.UploadToFTPCheckbox_CheckedChanged);
            // 
            // ConfigureFTPButton
            // 
            this.ConfigureFTPButton.Location = new System.Drawing.Point(432, 277);
            this.ConfigureFTPButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ConfigureFTPButton.Name = "ConfigureFTPButton";
            this.ConfigureFTPButton.Size = new System.Drawing.Size(275, 55);
            this.ConfigureFTPButton.TabIndex = 18;
            this.ConfigureFTPButton.Text = "Configure FTP";
            this.ConfigureFTPButton.UseVisualStyleBackColor = true;
            this.ConfigureFTPButton.Visible = false;
            this.ConfigureFTPButton.Click += new System.EventHandler(this.ConfigureFTPButton_Click);
            // 
            // UseSolderCheckbox
            // 
            this.UseSolderCheckbox.AutoSize = true;
            this.UseSolderCheckbox.Location = new System.Drawing.Point(779, 48);
            this.UseSolderCheckbox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.UseSolderCheckbox.Name = "UseSolderCheckbox";
            this.UseSolderCheckbox.Size = new System.Drawing.Size(193, 36);
            this.UseSolderCheckbox.TabIndex = 12;
            this.UseSolderCheckbox.Text = "Use Solder";
            this.UseSolderCheckbox.UseVisualStyleBackColor = true;
            this.UseSolderCheckbox.CheckedChanged += new System.EventHandler(this.UseSolderCheckbox_CheckedChanged);
            // 
            // forgeVersionLabel
            // 
            this.forgeVersionLabel.AutoSize = true;
            this.forgeVersionLabel.Location = new System.Drawing.Point(432, 105);
            this.forgeVersionLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.forgeVersionLabel.Name = "forgeVersionLabel";
            this.forgeVersionLabel.Size = new System.Drawing.Size(193, 32);
            this.forgeVersionLabel.TabIndex = 0;
            this.forgeVersionLabel.Text = "Forge Version";
            this.forgeVersionLabel.Visible = false;
            // 
            // forgeVersionDropdown
            // 
            this.forgeVersionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forgeVersionDropdown.FormattingEnabled = true;
            this.forgeVersionDropdown.Location = new System.Drawing.Point(432, 143);
            this.forgeVersionDropdown.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.forgeVersionDropdown.Name = "forgeVersionDropdown";
            this.forgeVersionDropdown.Size = new System.Drawing.Size(316, 39);
            this.forgeVersionDropdown.TabIndex = 11;
            this.forgeVersionDropdown.Visible = false;
            // 
            // PackTypeGroupBox
            // 
            this.PackTypeGroupBox.Controls.Add(this.ZipPackRadioButton);
            this.PackTypeGroupBox.Controls.Add(this.SolderPackRadioButton);
            this.PackTypeGroupBox.Location = new System.Drawing.Point(35, 48);
            this.PackTypeGroupBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.PackTypeGroupBox.Name = "PackTypeGroupBox";
            this.PackTypeGroupBox.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.PackTypeGroupBox.Size = new System.Drawing.Size(307, 160);
            this.PackTypeGroupBox.TabIndex = 6;
            this.PackTypeGroupBox.TabStop = false;
            this.PackTypeGroupBox.Text = "PackType";
            // 
            // ZipPackRadioButton
            // 
            this.ZipPackRadioButton.AutoSize = true;
            this.ZipPackRadioButton.Location = new System.Drawing.Point(19, 105);
            this.ZipPackRadioButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ZipPackRadioButton.Name = "ZipPackRadioButton";
            this.ZipPackRadioButton.Size = new System.Drawing.Size(162, 36);
            this.ZipPackRadioButton.TabIndex = 1;
            this.ZipPackRadioButton.Text = "Zip Pack";
            this.ZipPackRadioButton.UseVisualStyleBackColor = true;
            // 
            // SolderPackRadioButton
            // 
            this.SolderPackRadioButton.AutoSize = true;
            this.SolderPackRadioButton.Checked = true;
            this.SolderPackRadioButton.Location = new System.Drawing.Point(19, 48);
            this.SolderPackRadioButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.SolderPackRadioButton.Name = "SolderPackRadioButton";
            this.SolderPackRadioButton.Size = new System.Drawing.Size(205, 36);
            this.SolderPackRadioButton.TabIndex = 0;
            this.SolderPackRadioButton.TabStop = true;
            this.SolderPackRadioButton.Text = "Solder Pack";
            this.SolderPackRadioButton.UseVisualStyleBackColor = true;
            // 
            // technicPermissionsLevelGroupBox
            // 
            this.technicPermissionsLevelGroupBox.Controls.Add(this.technicPermissionsPublicPack);
            this.technicPermissionsLevelGroupBox.Controls.Add(this.technicPermissionsPrivatePack);
            this.technicPermissionsLevelGroupBox.Location = new System.Drawing.Point(53, 331);
            this.technicPermissionsLevelGroupBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.technicPermissionsLevelGroupBox.Name = "technicPermissionsLevelGroupBox";
            this.technicPermissionsLevelGroupBox.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.technicPermissionsLevelGroupBox.Size = new System.Drawing.Size(291, 153);
            this.technicPermissionsLevelGroupBox.TabIndex = 5;
            this.technicPermissionsLevelGroupBox.TabStop = false;
            this.technicPermissionsLevelGroupBox.Text = "Permissions Level";
            this.technicPermissionsLevelGroupBox.Visible = false;
            // 
            // technicPermissionsPublicPack
            // 
            this.technicPermissionsPublicPack.AutoSize = true;
            this.technicPermissionsPublicPack.Location = new System.Drawing.Point(19, 98);
            this.technicPermissionsPublicPack.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.technicPermissionsPublicPack.Name = "technicPermissionsPublicPack";
            this.technicPermissionsPublicPack.Size = new System.Drawing.Size(201, 36);
            this.technicPermissionsPublicPack.TabIndex = 1;
            this.technicPermissionsPublicPack.Text = "Public Pack";
            this.technicPermissionsPublicPack.UseVisualStyleBackColor = true;
            // 
            // technicPermissionsPrivatePack
            // 
            this.technicPermissionsPrivatePack.AutoSize = true;
            this.technicPermissionsPrivatePack.Checked = true;
            this.technicPermissionsPrivatePack.Location = new System.Drawing.Point(19, 48);
            this.technicPermissionsPrivatePack.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.technicPermissionsPrivatePack.Name = "technicPermissionsPrivatePack";
            this.technicPermissionsPrivatePack.Size = new System.Drawing.Size(211, 36);
            this.technicPermissionsPrivatePack.TabIndex = 0;
            this.technicPermissionsPrivatePack.TabStop = true;
            this.technicPermissionsPrivatePack.Text = "Private Pack";
            this.technicPermissionsPrivatePack.UseVisualStyleBackColor = true;
            // 
            // CheckTechnicPermissionsCheckBox
            // 
            this.CheckTechnicPermissionsCheckBox.AutoSize = true;
            this.CheckTechnicPermissionsCheckBox.Location = new System.Drawing.Point(53, 277);
            this.CheckTechnicPermissionsCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CheckTechnicPermissionsCheckBox.Name = "CheckTechnicPermissionsCheckBox";
            this.CheckTechnicPermissionsCheckBox.Size = new System.Drawing.Size(295, 36);
            this.CheckTechnicPermissionsCheckBox.TabIndex = 9;
            this.CheckTechnicPermissionsCheckBox.Text = "Check Permissions";
            this.CheckTechnicPermissionsCheckBox.UseVisualStyleBackColor = true;
            this.CheckTechnicPermissionsCheckBox.CheckedChanged += new System.EventHandler(this.CheckTechnicPermissionsCheckBox_CheckedChanged);
            // 
            // CreateConfigZipCheckBox
            // 
            this.CreateConfigZipCheckBox.AutoSize = true;
            this.CreateConfigZipCheckBox.Location = new System.Drawing.Point(53, 222);
            this.CreateConfigZipCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CreateConfigZipCheckBox.Name = "CreateConfigZipCheckBox";
            this.CreateConfigZipCheckBox.Size = new System.Drawing.Size(272, 36);
            this.CreateConfigZipCheckBox.TabIndex = 8;
            this.CreateConfigZipCheckBox.Text = "Create Config zip";
            this.CreateConfigZipCheckBox.UseVisualStyleBackColor = true;
            // 
            // createForgeZipCheckBox
            // 
            this.createForgeZipCheckBox.AutoSize = true;
            this.createForgeZipCheckBox.Location = new System.Drawing.Point(432, 48);
            this.createForgeZipCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.createForgeZipCheckBox.Name = "createForgeZipCheckBox";
            this.createForgeZipCheckBox.Size = new System.Drawing.Size(263, 36);
            this.createForgeZipCheckBox.TabIndex = 10;
            this.createForgeZipCheckBox.Text = "Create Forge zip";
            this.createForgeZipCheckBox.UseVisualStyleBackColor = true;
            this.createForgeZipCheckBox.CheckedChanged += new System.EventHandler(this.createForgeZipCheckBox_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            currentlyDoingLabel,
            this.StatusStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1235);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(2667, 46);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // currentlyDoingLabel
            // 
            currentlyDoingLabel.Name = "currentlyDoingLabel";
            currentlyDoingLabel.Size = new System.Drawing.Size(240, 43);
            currentlyDoingLabel.Text = "Currently doing: ";
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(270, 43);
            this.StatusStripLabel.Text = "Absolutely nothing";
            // 
            // ModpackHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2667, 1281);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.technicOptionsGroupBox);
            this.Controls.Add(this.globalConfigurationsGroupBox);
            this.Controls.Add(this.startPackingButton);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "ModpackHelper";
            this.Text = "Modpack Helper";
            this.globalConfigurationsGroupBox.ResumeLayout(false);
            this.globalConfigurationsGroupBox.PerformLayout();
            this.ModpackSettingsGroupBox.ResumeLayout(false);
            this.ModpackSettingsGroupBox.PerformLayout();
            this.technicOptionsGroupBox.ResumeLayout(false);
            this.technicOptionsGroupBox.PerformLayout();
            this.SolderConfigurePanel.ResumeLayout(false);
            this.SolderConfigurePanel.PerformLayout();
            this.PackTypeGroupBox.ResumeLayout(false);
            this.PackTypeGroupBox.PerformLayout();
            this.technicPermissionsLevelGroupBox.ResumeLayout(false);
            this.technicPermissionsLevelGroupBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TextBox InputDirectoryTextBox;
        public TextBox OutputDirectoryTextBox;
        public Button startPackingButton;
        public GroupBox globalConfigurationsGroupBox;
        public GroupBox technicOptionsGroupBox;
        public CheckBox createForgeZipCheckBox;
        public CheckBox CheckTechnicPermissionsCheckBox;
        public CheckBox CreateConfigZipCheckBox;
        public GroupBox technicPermissionsLevelGroupBox;
        public RadioButton technicPermissionsPublicPack;
        public RadioButton technicPermissionsPrivatePack;
        public GroupBox PackTypeGroupBox;
        public RadioButton ZipPackRadioButton;
        public RadioButton SolderPackRadioButton;
        public GroupBox ModpackSettingsGroupBox;
        public ComboBox ModpackNameTextBox;
        public TextBox ModpackVersionTextbox;
        public Button button4;
        public Button button3;
        public Button button2;
        public Button button1;
        public Button getForgeVersionsButton;
        public ComboBox MinecraftVersionDropdown;
        public Label forgeVersionLabel;
        public ComboBox forgeVersionDropdown;
        public CheckBox EnableDebugCheckbox;
        public CheckBox CreateFTBPackCheckBox;
        public CheckBox CreateTechnicPackCheckBox;
        public CheckBox ClearOutpuDirectoryCheckBox;
        public Button browseForInputDirectoryButton;
        public Button browseForOutputDirectoryButton;
        private Button ConfigureSolderButton;
        private Button ConfigureFTPButton;
        private Panel SolderConfigurePanel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusStripLabel;
        public CheckBox UseSolderCheckbox;
        public TextBox minimumMemoryTextBox;
        public ComboBox MinimumJavaVersionCombobox;
        public CheckBox ForceSolderUpdateCheckBox;
        public CheckBox UploadToFTPCheckbox;
    }
}

