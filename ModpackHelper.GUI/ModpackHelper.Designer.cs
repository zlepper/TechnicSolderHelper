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
            System.Windows.Forms.Label label3;
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
            this.technicPermissionsLevelGroupBox = new System.Windows.Forms.GroupBox();
            this.technicPermissionsPublicPack = new System.Windows.Forms.RadioButton();
            this.technicPermissionsPrivatePack = new System.Windows.Forms.RadioButton();
            this.CheckTechnicPermissionsCheckBox = new System.Windows.Forms.CheckBox();
            this.createForgeZipCheckBox = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.AdditinalFoldersCheckedList = new System.Windows.Forms.CheckedListBox();
            inputFolderLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ModpackNameLabel = new System.Windows.Forms.Label();
            ModpackVersionLabel = new System.Windows.Forms.Label();
            MinecraftVersionLabel = new System.Windows.Forms.Label();
            minimumMemoryLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            currentlyDoingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            label3 = new System.Windows.Forms.Label();
            this.globalConfigurationsGroupBox.SuspendLayout();
            this.ModpackSettingsGroupBox.SuspendLayout();
            this.technicOptionsGroupBox.SuspendLayout();
            this.SolderConfigurePanel.SuspendLayout();
            this.technicPermissionsLevelGroupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputFolderLabel
            // 
            inputFolderLabel.AutoSize = true;
            inputFolderLabel.Location = new System.Drawing.Point(309, 25);
            inputFolderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            inputFolderLabel.Name = "inputFolderLabel";
            inputFolderLabel.Size = new System.Drawing.Size(95, 20);
            inputFolderLabel.TabIndex = 0;
            inputFolderLabel.Text = "Input Folder";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(309, 88);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(107, 20);
            label1.TabIndex = 0;
            label1.Text = "Output Folder";
            // 
            // ModpackNameLabel
            // 
            ModpackNameLabel.AutoSize = true;
            ModpackNameLabel.Location = new System.Drawing.Point(9, 25);
            ModpackNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ModpackNameLabel.Name = "ModpackNameLabel";
            ModpackNameLabel.Size = new System.Drawing.Size(120, 20);
            ModpackNameLabel.TabIndex = 0;
            ModpackNameLabel.Text = "Modpack Name";
            // 
            // ModpackVersionLabel
            // 
            ModpackVersionLabel.AutoSize = true;
            ModpackVersionLabel.Location = new System.Drawing.Point(9, 85);
            ModpackVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ModpackVersionLabel.Name = "ModpackVersionLabel";
            ModpackVersionLabel.Size = new System.Drawing.Size(132, 20);
            ModpackVersionLabel.TabIndex = 0;
            ModpackVersionLabel.Text = "Modpack Version";
            // 
            // MinecraftVersionLabel
            // 
            MinecraftVersionLabel.AutoSize = true;
            MinecraftVersionLabel.Location = new System.Drawing.Point(9, 145);
            MinecraftVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MinecraftVersionLabel.Name = "MinecraftVersionLabel";
            MinecraftVersionLabel.Size = new System.Drawing.Size(133, 20);
            MinecraftVersionLabel.TabIndex = 0;
            MinecraftVersionLabel.Text = "Minecraft Version";
            // 
            // minimumMemoryLabel
            // 
            minimumMemoryLabel.AutoSize = true;
            minimumMemoryLabel.Location = new System.Drawing.Point(10, 55);
            minimumMemoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            minimumMemoryLabel.Name = "minimumMemoryLabel";
            minimumMemoryLabel.Size = new System.Drawing.Size(186, 20);
            minimumMemoryLabel.TabIndex = 15;
            minimumMemoryLabel.Text = "Minimum Memory (in MB)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(9, 115);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(159, 20);
            label2.TabIndex = 15;
            label2.Text = "MinimumJavaVersion";
            // 
            // currentlyDoingLabel
            // 
            currentlyDoingLabel.Name = "currentlyDoingLabel";
            currentlyDoingLabel.Size = new System.Drawing.Size(144, 25);
            currentlyDoingLabel.Text = "Currently doing: ";
            // 
            // InputDirectoryTextBox
            // 
            this.InputDirectoryTextBox.Location = new System.Drawing.Point(309, 49);
            this.InputDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InputDirectoryTextBox.Name = "InputDirectoryTextBox";
            this.InputDirectoryTextBox.Size = new System.Drawing.Size(715, 26);
            this.InputDirectoryTextBox.TabIndex = 1;
            this.InputDirectoryTextBox.TextChanged += new System.EventHandler(this.InputDirectoryTextBox_TextChanged);
            // 
            // browseForInputDirectoryButton
            // 
            this.browseForInputDirectoryButton.Location = new System.Drawing.Point(1035, 48);
            this.browseForInputDirectoryButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.browseForInputDirectoryButton.Name = "browseForInputDirectoryButton";
            this.browseForInputDirectoryButton.Size = new System.Drawing.Size(112, 35);
            this.browseForInputDirectoryButton.TabIndex = 1;
            this.browseForInputDirectoryButton.Text = "Browse";
            this.browseForInputDirectoryButton.UseVisualStyleBackColor = true;
            this.browseForInputDirectoryButton.Click += new System.EventHandler(this.browseForInputDirectoryButton_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(309, 112);
            this.OutputDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(715, 26);
            this.OutputDirectoryTextBox.TabIndex = 2;
            // 
            // browseForOutputDirectoryButton
            // 
            this.browseForOutputDirectoryButton.Location = new System.Drawing.Point(1035, 111);
            this.browseForOutputDirectoryButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.browseForOutputDirectoryButton.Name = "browseForOutputDirectoryButton";
            this.browseForOutputDirectoryButton.Size = new System.Drawing.Size(112, 35);
            this.browseForOutputDirectoryButton.TabIndex = 1;
            this.browseForOutputDirectoryButton.Text = "Browse";
            this.browseForOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.browseForOutputDirectoryButton.Click += new System.EventHandler(this.browseForOutputDirectoryButton_Click);
            // 
            // startPackingButton
            // 
            this.startPackingButton.Location = new System.Drawing.Point(885, 331);
            this.startPackingButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startPackingButton.Name = "startPackingButton";
            this.startPackingButton.Size = new System.Drawing.Size(117, 98);
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
            this.globalConfigurationsGroupBox.Location = new System.Drawing.Point(18, 18);
            this.globalConfigurationsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.globalConfigurationsGroupBox.Name = "globalConfigurationsGroupBox";
            this.globalConfigurationsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.globalConfigurationsGroupBox.Size = new System.Drawing.Size(1161, 294);
            this.globalConfigurationsGroupBox.TabIndex = 3;
            this.globalConfigurationsGroupBox.TabStop = false;
            this.globalConfigurationsGroupBox.Text = "Global Configurations";
            // 
            // EnableDebugCheckbox
            // 
            this.EnableDebugCheckbox.AutoSize = true;
            this.EnableDebugCheckbox.Location = new System.Drawing.Point(878, 197);
            this.EnableDebugCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EnableDebugCheckbox.Name = "EnableDebugCheckbox";
            this.EnableDebugCheckbox.Size = new System.Drawing.Size(137, 24);
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
            this.ModpackSettingsGroupBox.Location = new System.Drawing.Point(9, 29);
            this.ModpackSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModpackSettingsGroupBox.Name = "ModpackSettingsGroupBox";
            this.ModpackSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModpackSettingsGroupBox.Size = new System.Drawing.Size(291, 217);
            this.ModpackSettingsGroupBox.TabIndex = 2;
            this.ModpackSettingsGroupBox.TabStop = false;
            this.ModpackSettingsGroupBox.Text = "Modpack Settings";
            // 
            // MinecraftVersionDropdown
            // 
            this.MinecraftVersionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MinecraftVersionDropdown.FormattingEnabled = true;
            this.MinecraftVersionDropdown.Location = new System.Drawing.Point(9, 169);
            this.MinecraftVersionDropdown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinecraftVersionDropdown.Name = "MinecraftVersionDropdown";
            this.MinecraftVersionDropdown.Size = new System.Drawing.Size(271, 28);
            this.MinecraftVersionDropdown.TabIndex = 5;
            this.MinecraftVersionDropdown.SelectedIndexChanged += new System.EventHandler(this.MinecraftVersionDropdown_SelectedIndexChanged);
            // 
            // ModpackVersionTextbox
            // 
            this.ModpackVersionTextbox.Location = new System.Drawing.Point(9, 109);
            this.ModpackVersionTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModpackVersionTextbox.Name = "ModpackVersionTextbox";
            this.ModpackVersionTextbox.Size = new System.Drawing.Size(271, 26);
            this.ModpackVersionTextbox.TabIndex = 4;
            // 
            // ModpackNameTextBox
            // 
            this.ModpackNameTextBox.Location = new System.Drawing.Point(9, 49);
            this.ModpackNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModpackNameTextBox.Name = "ModpackNameTextBox";
            this.ModpackNameTextBox.Size = new System.Drawing.Size(271, 28);
            this.ModpackNameTextBox.TabIndex = 3;
            this.ModpackNameTextBox.SelectedIndexChanged += new System.EventHandler(this.ModpackNameTextBox_SelectedIndexChanged);
            // 
            // ClearOutpuDirectoryCheckBox
            // 
            this.ClearOutpuDirectoryCheckBox.AutoSize = true;
            this.ClearOutpuDirectoryCheckBox.Checked = true;
            this.ClearOutpuDirectoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClearOutpuDirectoryCheckBox.Location = new System.Drawing.Point(878, 158);
            this.ClearOutpuDirectoryCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ClearOutpuDirectoryCheckBox.Name = "ClearOutpuDirectoryCheckBox";
            this.ClearOutpuDirectoryCheckBox.Size = new System.Drawing.Size(235, 24);
            this.ClearOutpuDirectoryCheckBox.TabIndex = 11;
            this.ClearOutpuDirectoryCheckBox.Text = "Clear output directory on run";
            this.ClearOutpuDirectoryCheckBox.UseVisualStyleBackColor = true;
            // 
            // getForgeVersionsButton
            // 
            this.getForgeVersionsButton.Location = new System.Drawing.Point(309, 152);
            this.getForgeVersionsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getForgeVersionsButton.Name = "getForgeVersionsButton";
            this.getForgeVersionsButton.Size = new System.Drawing.Size(240, 35);
            this.getForgeVersionsButton.TabIndex = 3;
            this.getForgeVersionsButton.Text = "Get Forge/Minecraft versions";
            this.getForgeVersionsButton.UseVisualStyleBackColor = true;
            // 
            // CreateFTBPackCheckBox
            // 
            this.CreateFTBPackCheckBox.AutoSize = true;
            this.CreateFTBPackCheckBox.Location = new System.Drawing.Point(513, 243);
            this.CreateFTBPackCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CreateFTBPackCheckBox.Name = "CreateFTBPackCheckBox";
            this.CreateFTBPackCheckBox.Size = new System.Drawing.Size(156, 24);
            this.CreateFTBPackCheckBox.TabIndex = 7;
            this.CreateFTBPackCheckBox.Text = "Create FTB Pack";
            this.CreateFTBPackCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(309, 197);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(240, 35);
            this.button1.TabIndex = 4;
            this.button1.Text = "Update stored permissions";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CreateTechnicPackCheckBox
            // 
            this.CreateTechnicPackCheckBox.AutoSize = true;
            this.CreateTechnicPackCheckBox.Checked = true;
            this.CreateTechnicPackCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateTechnicPackCheckBox.Location = new System.Drawing.Point(314, 243);
            this.CreateTechnicPackCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CreateTechnicPackCheckBox.Name = "CreateTechnicPackCheckBox";
            this.CreateTechnicPackCheckBox.Size = new System.Drawing.Size(181, 24);
            this.CreateTechnicPackCheckBox.TabIndex = 6;
            this.CreateTechnicPackCheckBox.Text = "Create Technic Pack";
            this.CreateTechnicPackCheckBox.UseVisualStyleBackColor = true;
            this.CreateTechnicPackCheckBox.CheckedChanged += new System.EventHandler(this.CreateTechnicPackCheckBox_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(681, 152);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(188, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "Repack everything";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(560, 152);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 35);
            this.button4.TabIndex = 7;
            this.button4.Text = "Edit data";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(560, 197);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(188, 35);
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
            this.technicOptionsGroupBox.Controls.Add(this.technicPermissionsLevelGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.CheckTechnicPermissionsCheckBox);
            this.technicOptionsGroupBox.Controls.Add(this.createForgeZipCheckBox);
            this.technicOptionsGroupBox.Location = new System.Drawing.Point(18, 322);
            this.technicOptionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.technicOptionsGroupBox.Name = "technicOptionsGroupBox";
            this.technicOptionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.technicOptionsGroupBox.Size = new System.Drawing.Size(856, 338);
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
            this.SolderConfigurePanel.Location = new System.Drawing.Point(438, 62);
            this.SolderConfigurePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SolderConfigurePanel.Name = "SolderConfigurePanel";
            this.SolderConfigurePanel.Size = new System.Drawing.Size(216, 220);
            this.SolderConfigurePanel.TabIndex = 20;
            this.SolderConfigurePanel.Visible = false;
            // 
            // ConfigureSolderButton
            // 
            this.ConfigureSolderButton.Location = new System.Drawing.Point(4, 11);
            this.ConfigureSolderButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConfigureSolderButton.Name = "ConfigureSolderButton";
            this.ConfigureSolderButton.Size = new System.Drawing.Size(207, 35);
            this.ConfigureSolderButton.TabIndex = 13;
            this.ConfigureSolderButton.Text = "Configure Solder";
            this.ConfigureSolderButton.UseVisualStyleBackColor = true;
            this.ConfigureSolderButton.Click += new System.EventHandler(this.ConfigureSolderButton_Click);
            // 
            // ForceSolderUpdateCheckBox
            // 
            this.ForceSolderUpdateCheckBox.AutoSize = true;
            this.ForceSolderUpdateCheckBox.Location = new System.Drawing.Point(2, 183);
            this.ForceSolderUpdateCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ForceSolderUpdateCheckBox.Name = "ForceSolderUpdateCheckBox";
            this.ForceSolderUpdateCheckBox.Size = new System.Drawing.Size(180, 24);
            this.ForceSolderUpdateCheckBox.TabIndex = 17;
            this.ForceSolderUpdateCheckBox.Text = "Force Solder update";
            this.ForceSolderUpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimumMemoryTextBox
            // 
            this.minimumMemoryTextBox.Location = new System.Drawing.Point(4, 80);
            this.minimumMemoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.minimumMemoryTextBox.Name = "minimumMemoryTextBox";
            this.minimumMemoryTextBox.Size = new System.Drawing.Size(205, 26);
            this.minimumMemoryTextBox.TabIndex = 14;
            // 
            // MinimumJavaVersionCombobox
            // 
            this.MinimumJavaVersionCombobox.FormattingEnabled = true;
            this.MinimumJavaVersionCombobox.Items.AddRange(new object[] {
            "Java 6",
            "Java 7",
            "Java 8"});
            this.MinimumJavaVersionCombobox.Location = new System.Drawing.Point(4, 142);
            this.MinimumJavaVersionCombobox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumJavaVersionCombobox.Name = "MinimumJavaVersionCombobox";
            this.MinimumJavaVersionCombobox.Size = new System.Drawing.Size(205, 28);
            this.MinimumJavaVersionCombobox.TabIndex = 16;
            // 
            // UploadToFTPCheckbox
            // 
            this.UploadToFTPCheckbox.AutoSize = true;
            this.UploadToFTPCheckbox.Location = new System.Drawing.Point(243, 143);
            this.UploadToFTPCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UploadToFTPCheckbox.Name = "UploadToFTPCheckbox";
            this.UploadToFTPCheckbox.Size = new System.Drawing.Size(137, 24);
            this.UploadToFTPCheckbox.TabIndex = 19;
            this.UploadToFTPCheckbox.Text = "Upload to FTP";
            this.UploadToFTPCheckbox.UseVisualStyleBackColor = true;
            this.UploadToFTPCheckbox.CheckedChanged += new System.EventHandler(this.UploadToFTPCheckbox_CheckedChanged);
            // 
            // ConfigureFTPButton
            // 
            this.ConfigureFTPButton.Location = new System.Drawing.Point(243, 178);
            this.ConfigureFTPButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConfigureFTPButton.Name = "ConfigureFTPButton";
            this.ConfigureFTPButton.Size = new System.Drawing.Size(154, 35);
            this.ConfigureFTPButton.TabIndex = 18;
            this.ConfigureFTPButton.Text = "Configure FTP";
            this.ConfigureFTPButton.UseVisualStyleBackColor = true;
            this.ConfigureFTPButton.Visible = false;
            this.ConfigureFTPButton.Click += new System.EventHandler(this.ConfigureFTPButton_Click);
            // 
            // UseSolderCheckbox
            // 
            this.UseSolderCheckbox.AutoSize = true;
            this.UseSolderCheckbox.Location = new System.Drawing.Point(438, 31);
            this.UseSolderCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UseSolderCheckbox.Name = "UseSolderCheckbox";
            this.UseSolderCheckbox.Size = new System.Drawing.Size(114, 24);
            this.UseSolderCheckbox.TabIndex = 12;
            this.UseSolderCheckbox.Text = "Use Solder";
            this.UseSolderCheckbox.UseVisualStyleBackColor = true;
            this.UseSolderCheckbox.CheckedChanged += new System.EventHandler(this.UseSolderCheckbox_CheckedChanged);
            // 
            // forgeVersionLabel
            // 
            this.forgeVersionLabel.AutoSize = true;
            this.forgeVersionLabel.Location = new System.Drawing.Point(243, 68);
            this.forgeVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.forgeVersionLabel.Name = "forgeVersionLabel";
            this.forgeVersionLabel.Size = new System.Drawing.Size(109, 20);
            this.forgeVersionLabel.TabIndex = 0;
            this.forgeVersionLabel.Text = "Forge Version";
            this.forgeVersionLabel.Visible = false;
            // 
            // forgeVersionDropdown
            // 
            this.forgeVersionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forgeVersionDropdown.FormattingEnabled = true;
            this.forgeVersionDropdown.Location = new System.Drawing.Point(243, 92);
            this.forgeVersionDropdown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.forgeVersionDropdown.Name = "forgeVersionDropdown";
            this.forgeVersionDropdown.Size = new System.Drawing.Size(180, 28);
            this.forgeVersionDropdown.TabIndex = 11;
            this.forgeVersionDropdown.Visible = false;
            // 
            // technicPermissionsLevelGroupBox
            // 
            this.technicPermissionsLevelGroupBox.Controls.Add(this.technicPermissionsPublicPack);
            this.technicPermissionsLevelGroupBox.Controls.Add(this.technicPermissionsPrivatePack);
            this.technicPermissionsLevelGroupBox.Location = new System.Drawing.Point(18, 67);
            this.technicPermissionsLevelGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.technicPermissionsLevelGroupBox.Name = "technicPermissionsLevelGroupBox";
            this.technicPermissionsLevelGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.technicPermissionsLevelGroupBox.Size = new System.Drawing.Size(164, 98);
            this.technicPermissionsLevelGroupBox.TabIndex = 5;
            this.technicPermissionsLevelGroupBox.TabStop = false;
            this.technicPermissionsLevelGroupBox.Text = "Permissions Level";
            this.technicPermissionsLevelGroupBox.Visible = false;
            // 
            // technicPermissionsPublicPack
            // 
            this.technicPermissionsPublicPack.AutoSize = true;
            this.technicPermissionsPublicPack.Location = new System.Drawing.Point(10, 63);
            this.technicPermissionsPublicPack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.technicPermissionsPublicPack.Name = "technicPermissionsPublicPack";
            this.technicPermissionsPublicPack.Size = new System.Drawing.Size(115, 24);
            this.technicPermissionsPublicPack.TabIndex = 1;
            this.technicPermissionsPublicPack.Text = "Public Pack";
            this.technicPermissionsPublicPack.UseVisualStyleBackColor = true;
            // 
            // technicPermissionsPrivatePack
            // 
            this.technicPermissionsPrivatePack.AutoSize = true;
            this.technicPermissionsPrivatePack.Checked = true;
            this.technicPermissionsPrivatePack.Location = new System.Drawing.Point(10, 31);
            this.technicPermissionsPrivatePack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.technicPermissionsPrivatePack.Name = "technicPermissionsPrivatePack";
            this.technicPermissionsPrivatePack.Size = new System.Drawing.Size(121, 24);
            this.technicPermissionsPrivatePack.TabIndex = 0;
            this.technicPermissionsPrivatePack.TabStop = true;
            this.technicPermissionsPrivatePack.Text = "Private Pack";
            this.technicPermissionsPrivatePack.UseVisualStyleBackColor = true;
            // 
            // CheckTechnicPermissionsCheckBox
            // 
            this.CheckTechnicPermissionsCheckBox.AutoSize = true;
            this.CheckTechnicPermissionsCheckBox.Location = new System.Drawing.Point(18, 31);
            this.CheckTechnicPermissionsCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CheckTechnicPermissionsCheckBox.Name = "CheckTechnicPermissionsCheckBox";
            this.CheckTechnicPermissionsCheckBox.Size = new System.Drawing.Size(169, 24);
            this.CheckTechnicPermissionsCheckBox.TabIndex = 9;
            this.CheckTechnicPermissionsCheckBox.Text = "Check Permissions";
            this.CheckTechnicPermissionsCheckBox.UseVisualStyleBackColor = true;
            this.CheckTechnicPermissionsCheckBox.CheckedChanged += new System.EventHandler(this.CheckTechnicPermissionsCheckBox_CheckedChanged);
            // 
            // createForgeZipCheckBox
            // 
            this.createForgeZipCheckBox.AutoSize = true;
            this.createForgeZipCheckBox.Location = new System.Drawing.Point(243, 31);
            this.createForgeZipCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.createForgeZipCheckBox.Name = "createForgeZipCheckBox";
            this.createForgeZipCheckBox.Size = new System.Drawing.Size(153, 24);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 733);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1595, 30);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(163, 25);
            this.StatusStripLabel.Text = "Absolutely nothing";
            // 
            // AdditinalFoldersCheckedList
            // 
            this.AdditinalFoldersCheckedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdditinalFoldersCheckedList.CheckOnClick = true;
            this.AdditinalFoldersCheckedList.FormattingEnabled = true;
            this.AdditinalFoldersCheckedList.Location = new System.Drawing.Point(1186, 55);
            this.AdditinalFoldersCheckedList.Name = "AdditinalFoldersCheckedList";
            this.AdditinalFoldersCheckedList.Size = new System.Drawing.Size(397, 655);
            this.AdditinalFoldersCheckedList.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(1186, 32);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(292, 20);
            label3.TabIndex = 7;
            label3.Text = "Additional folders that should be packed";
            // 
            // ModpackHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1595, 763);
            this.Controls.Add(label3);
            this.Controls.Add(this.AdditinalFoldersCheckedList);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.technicOptionsGroupBox);
            this.Controls.Add(this.globalConfigurationsGroupBox);
            this.Controls.Add(this.startPackingButton);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModpackHelper";
            this.Text = "Modpack Helper";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.formClosingHandler);
            this.globalConfigurationsGroupBox.ResumeLayout(false);
            this.globalConfigurationsGroupBox.PerformLayout();
            this.ModpackSettingsGroupBox.ResumeLayout(false);
            this.ModpackSettingsGroupBox.PerformLayout();
            this.technicOptionsGroupBox.ResumeLayout(false);
            this.technicOptionsGroupBox.PerformLayout();
            this.SolderConfigurePanel.ResumeLayout(false);
            this.SolderConfigurePanel.PerformLayout();
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
        public GroupBox technicPermissionsLevelGroupBox;
        public RadioButton technicPermissionsPublicPack;
        public RadioButton technicPermissionsPrivatePack;
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
        private CheckedListBox AdditinalFoldersCheckedList;
    }
}

