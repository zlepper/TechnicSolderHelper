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
            this.InputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.browseForInputDirectoryButton = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.browseForOutputDirectoryButton = new System.Windows.Forms.Button();
            this.startPackingButton = new System.Windows.Forms.Button();
            this.globalConfigurationsGroupBox = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.ClearOutpuDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateFTBPackCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateTechnicPackCheckBox = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.getForgeVersionsButton = new System.Windows.Forms.Button();
            this.ModpackSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.MinecraftVersionDropdown = new System.Windows.Forms.ComboBox();
            this.ModpackVersionTextbox = new System.Windows.Forms.TextBox();
            this.ModpackNameTextBox = new System.Windows.Forms.ComboBox();
            this.technicOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.forgeVersionLabel = new System.Windows.Forms.Label();
            this.forgeVersionDropdown = new System.Windows.Forms.ComboBox();
            this.PackTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.ZipPackRadioButton = new System.Windows.Forms.RadioButton();
            this.SolderPackRadioButton = new System.Windows.Forms.RadioButton();
            this.technicPermissionsLevelGroupBox = new System.Windows.Forms.GroupBox();
            this.technicPermissionsPublicPack = new System.Windows.Forms.RadioButton();
            this.technicPermissionsPrivatePack = new System.Windows.Forms.RadioButton();
            this.CheckPermissionsCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateConfigZipCheckBox = new System.Windows.Forms.CheckBox();
            this.createForgeZipCheckBox = new System.Windows.Forms.CheckBox();
            inputFolderLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ModpackNameLabel = new System.Windows.Forms.Label();
            ModpackVersionLabel = new System.Windows.Forms.Label();
            MinecraftVersionLabel = new System.Windows.Forms.Label();
            this.globalConfigurationsGroupBox.SuspendLayout();
            this.ModpackSettingsGroupBox.SuspendLayout();
            this.technicOptionsGroupBox.SuspendLayout();
            this.PackTypeGroupBox.SuspendLayout();
            this.technicPermissionsLevelGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputFolderLabel
            // 
            inputFolderLabel.AutoSize = true;
            inputFolderLabel.Location = new System.Drawing.Point(6, 17);
            inputFolderLabel.Name = "inputFolderLabel";
            inputFolderLabel.Size = new System.Drawing.Size(63, 13);
            inputFolderLabel.TabIndex = 0;
            inputFolderLabel.Text = "Input Folder";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 58);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(71, 13);
            label1.TabIndex = 0;
            label1.Text = "Output Folder";
            // 
            // ModpackNameLabel
            // 
            ModpackNameLabel.AutoSize = true;
            ModpackNameLabel.Location = new System.Drawing.Point(6, 16);
            ModpackNameLabel.Name = "ModpackNameLabel";
            ModpackNameLabel.Size = new System.Drawing.Size(83, 13);
            ModpackNameLabel.TabIndex = 0;
            ModpackNameLabel.Text = "Modpack Name";
            // 
            // ModpackVersionLabel
            // 
            ModpackVersionLabel.AutoSize = true;
            ModpackVersionLabel.Location = new System.Drawing.Point(6, 55);
            ModpackVersionLabel.Name = "ModpackVersionLabel";
            ModpackVersionLabel.Size = new System.Drawing.Size(90, 13);
            ModpackVersionLabel.TabIndex = 0;
            ModpackVersionLabel.Text = "Modpack Version";
            // 
            // MinecraftVersionLabel
            // 
            MinecraftVersionLabel.AutoSize = true;
            MinecraftVersionLabel.Location = new System.Drawing.Point(6, 94);
            MinecraftVersionLabel.Name = "MinecraftVersionLabel";
            MinecraftVersionLabel.Size = new System.Drawing.Size(89, 13);
            MinecraftVersionLabel.TabIndex = 0;
            MinecraftVersionLabel.Text = "Minecraft Version";
            // 
            // InputDirectoryTextBox
            // 
            this.InputDirectoryTextBox.Location = new System.Drawing.Point(6, 33);
            this.InputDirectoryTextBox.Name = "InputDirectoryTextBox";
            this.InputDirectoryTextBox.Size = new System.Drawing.Size(478, 20);
            this.InputDirectoryTextBox.TabIndex = 1;
            // 
            // browseForInputDirectoryButton
            // 
            this.browseForInputDirectoryButton.Location = new System.Drawing.Point(490, 32);
            this.browseForInputDirectoryButton.Name = "browseForInputDirectoryButton";
            this.browseForInputDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.browseForInputDirectoryButton.TabIndex = 1;
            this.browseForInputDirectoryButton.Text = "Browse";
            this.browseForInputDirectoryButton.UseVisualStyleBackColor = true;
            this.browseForInputDirectoryButton.Click += new System.EventHandler(this.browseForInputDirectoryButton_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(6, 74);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(478, 20);
            this.OutputDirectoryTextBox.TabIndex = 2;
            // 
            // browseForOutputDirectoryButton
            // 
            this.browseForOutputDirectoryButton.Location = new System.Drawing.Point(490, 73);
            this.browseForOutputDirectoryButton.Name = "browseForOutputDirectoryButton";
            this.browseForOutputDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.browseForOutputDirectoryButton.TabIndex = 1;
            this.browseForOutputDirectoryButton.Text = "Browse";
            this.browseForOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.browseForOutputDirectoryButton.Click += new System.EventHandler(this.browseForOutputDirectoryButton_Click);
            // 
            // startPackingButton
            // 
            this.startPackingButton.Location = new System.Drawing.Point(590, 215);
            this.startPackingButton.Name = "startPackingButton";
            this.startPackingButton.Size = new System.Drawing.Size(78, 64);
            this.startPackingButton.TabIndex = 2;
            this.startPackingButton.Text = "Start";
            this.startPackingButton.UseVisualStyleBackColor = true;
            this.startPackingButton.Click += new System.EventHandler(this.startPackingButton_Click);
            // 
            // globalConfigurationsGroupBox
            // 
            this.globalConfigurationsGroupBox.Controls.Add(this.checkBox4);
            this.globalConfigurationsGroupBox.Controls.Add(this.ClearOutpuDirectoryCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.CreateFTBPackCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.CreateTechnicPackCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.button4);
            this.globalConfigurationsGroupBox.Controls.Add(this.button3);
            this.globalConfigurationsGroupBox.Controls.Add(this.button2);
            this.globalConfigurationsGroupBox.Controls.Add(this.button1);
            this.globalConfigurationsGroupBox.Controls.Add(this.getForgeVersionsButton);
            this.globalConfigurationsGroupBox.Controls.Add(this.ModpackSettingsGroupBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.InputDirectoryTextBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.OutputDirectoryTextBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.browseForOutputDirectoryButton);
            this.globalConfigurationsGroupBox.Controls.Add(inputFolderLabel);
            this.globalConfigurationsGroupBox.Controls.Add(this.browseForInputDirectoryButton);
            this.globalConfigurationsGroupBox.Controls.Add(label1);
            this.globalConfigurationsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.globalConfigurationsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.globalConfigurationsGroupBox.Name = "globalConfigurationsGroupBox";
            this.globalConfigurationsGroupBox.Size = new System.Drawing.Size(774, 191);
            this.globalConfigurationsGroupBox.TabIndex = 3;
            this.globalConfigurationsGroupBox.TabStop = false;
            this.globalConfigurationsGroupBox.Text = "Global Configurations";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(385, 129);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(94, 17);
            this.checkBox4.TabIndex = 12;
            this.checkBox4.Text = "Enable Debug";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // ClearOutpuDirectoryCheckBox
            // 
            this.ClearOutpuDirectoryCheckBox.AutoSize = true;
            this.ClearOutpuDirectoryCheckBox.Checked = true;
            this.ClearOutpuDirectoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClearOutpuDirectoryCheckBox.Location = new System.Drawing.Point(385, 104);
            this.ClearOutpuDirectoryCheckBox.Name = "ClearOutpuDirectoryCheckBox";
            this.ClearOutpuDirectoryCheckBox.Size = new System.Drawing.Size(159, 17);
            this.ClearOutpuDirectoryCheckBox.TabIndex = 11;
            this.ClearOutpuDirectoryCheckBox.Text = "Clear output directory on run";
            this.ClearOutpuDirectoryCheckBox.UseVisualStyleBackColor = true;
            this.ClearOutpuDirectoryCheckBox.CheckedChanged += new System.EventHandler(this.ClearOutpuDirectoryCheckBox_CheckedChanged);
            // 
            // CreateFTBPackCheckBox
            // 
            this.CreateFTBPackCheckBox.AutoSize = true;
            this.CreateFTBPackCheckBox.Location = new System.Drawing.Point(142, 159);
            this.CreateFTBPackCheckBox.Name = "CreateFTBPackCheckBox";
            this.CreateFTBPackCheckBox.Size = new System.Drawing.Size(108, 17);
            this.CreateFTBPackCheckBox.TabIndex = 7;
            this.CreateFTBPackCheckBox.Text = "Create FTB Pack";
            this.CreateFTBPackCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreateTechnicPackCheckBox
            // 
            this.CreateTechnicPackCheckBox.AutoSize = true;
            this.CreateTechnicPackCheckBox.Location = new System.Drawing.Point(9, 159);
            this.CreateTechnicPackCheckBox.Name = "CreateTechnicPackCheckBox";
            this.CreateTechnicPackCheckBox.Size = new System.Drawing.Size(127, 17);
            this.CreateTechnicPackCheckBox.TabIndex = 6;
            this.CreateTechnicPackCheckBox.Text = "Create Technic Pack";
            this.CreateTechnicPackCheckBox.UseVisualStyleBackColor = true;
            this.CreateTechnicPackCheckBox.CheckedChanged += new System.EventHandler(this.CreateTechnicPackCheckBox_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(173, 100);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Edit data";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(173, 129);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(125, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Generate Permissions";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(254, 100);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Repack everything";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Update stored permissions";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // getForgeVersionsButton
            // 
            this.getForgeVersionsButton.Location = new System.Drawing.Point(6, 100);
            this.getForgeVersionsButton.Name = "getForgeVersionsButton";
            this.getForgeVersionsButton.Size = new System.Drawing.Size(160, 23);
            this.getForgeVersionsButton.TabIndex = 3;
            this.getForgeVersionsButton.Text = "Get Forge/Minecraft versions";
            this.getForgeVersionsButton.UseVisualStyleBackColor = true;
            // 
            // ModpackSettingsGroupBox
            // 
            this.ModpackSettingsGroupBox.Controls.Add(this.MinecraftVersionDropdown);
            this.ModpackSettingsGroupBox.Controls.Add(MinecraftVersionLabel);
            this.ModpackSettingsGroupBox.Controls.Add(this.ModpackVersionTextbox);
            this.ModpackSettingsGroupBox.Controls.Add(ModpackVersionLabel);
            this.ModpackSettingsGroupBox.Controls.Add(this.ModpackNameTextBox);
            this.ModpackSettingsGroupBox.Controls.Add(ModpackNameLabel);
            this.ModpackSettingsGroupBox.Location = new System.Drawing.Point(573, 11);
            this.ModpackSettingsGroupBox.Name = "ModpackSettingsGroupBox";
            this.ModpackSettingsGroupBox.Size = new System.Drawing.Size(194, 141);
            this.ModpackSettingsGroupBox.TabIndex = 2;
            this.ModpackSettingsGroupBox.TabStop = false;
            this.ModpackSettingsGroupBox.Text = "Modpack Settings";
            // 
            // MinecraftVersionDropdown
            // 
            this.MinecraftVersionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MinecraftVersionDropdown.FormattingEnabled = true;
            this.MinecraftVersionDropdown.Location = new System.Drawing.Point(6, 110);
            this.MinecraftVersionDropdown.Name = "MinecraftVersionDropdown";
            this.MinecraftVersionDropdown.Size = new System.Drawing.Size(182, 21);
            this.MinecraftVersionDropdown.TabIndex = 5;
            this.MinecraftVersionDropdown.SelectedIndexChanged += new System.EventHandler(this.MinecraftVersionDropdown_SelectedIndexChanged);
            // 
            // ModpackVersionTextbox
            // 
            this.ModpackVersionTextbox.Location = new System.Drawing.Point(6, 71);
            this.ModpackVersionTextbox.Name = "ModpackVersionTextbox";
            this.ModpackVersionTextbox.Size = new System.Drawing.Size(182, 20);
            this.ModpackVersionTextbox.TabIndex = 4;
            // 
            // ModpackNameTextBox
            // 
            this.ModpackNameTextBox.Location = new System.Drawing.Point(6, 32);
            this.ModpackNameTextBox.Name = "ModpackNameTextBox";
            this.ModpackNameTextBox.Size = new System.Drawing.Size(182, 21);
            this.ModpackNameTextBox.TabIndex = 3;
            this.ModpackNameTextBox.LostFocus += new System.EventHandler(this.ModpackNameTextBox_LostFocus);
            // 
            // technicOptionsGroupBox
            // 
            this.technicOptionsGroupBox.Controls.Add(this.forgeVersionLabel);
            this.technicOptionsGroupBox.Controls.Add(this.forgeVersionDropdown);
            this.technicOptionsGroupBox.Controls.Add(this.PackTypeGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.technicPermissionsLevelGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.CheckPermissionsCheckBox);
            this.technicOptionsGroupBox.Controls.Add(this.CreateConfigZipCheckBox);
            this.technicOptionsGroupBox.Controls.Add(this.createForgeZipCheckBox);
            this.technicOptionsGroupBox.Location = new System.Drawing.Point(12, 209);
            this.technicOptionsGroupBox.Name = "technicOptionsGroupBox";
            this.technicOptionsGroupBox.Size = new System.Drawing.Size(571, 166);
            this.technicOptionsGroupBox.TabIndex = 4;
            this.technicOptionsGroupBox.TabStop = false;
            this.technicOptionsGroupBox.Text = "Technic Options";
            // 
            // forgeVersionLabel
            // 
            this.forgeVersionLabel.AutoSize = true;
            this.forgeVersionLabel.Location = new System.Drawing.Point(162, 44);
            this.forgeVersionLabel.Name = "forgeVersionLabel";
            this.forgeVersionLabel.Size = new System.Drawing.Size(72, 13);
            this.forgeVersionLabel.TabIndex = 0;
            this.forgeVersionLabel.Text = "Forge Version";
            // 
            // forgeVersionDropdown
            // 
            this.forgeVersionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forgeVersionDropdown.FormattingEnabled = true;
            this.forgeVersionDropdown.Location = new System.Drawing.Point(162, 60);
            this.forgeVersionDropdown.Name = "forgeVersionDropdown";
            this.forgeVersionDropdown.Size = new System.Drawing.Size(121, 21);
            this.forgeVersionDropdown.TabIndex = 11;
            // 
            // PackTypeGroupBox
            // 
            this.PackTypeGroupBox.Controls.Add(this.ZipPackRadioButton);
            this.PackTypeGroupBox.Controls.Add(this.SolderPackRadioButton);
            this.PackTypeGroupBox.Location = new System.Drawing.Point(13, 20);
            this.PackTypeGroupBox.Name = "PackTypeGroupBox";
            this.PackTypeGroupBox.Size = new System.Drawing.Size(115, 67);
            this.PackTypeGroupBox.TabIndex = 6;
            this.PackTypeGroupBox.TabStop = false;
            this.PackTypeGroupBox.Text = "PackType";
            // 
            // ZipPackRadioButton
            // 
            this.ZipPackRadioButton.AutoSize = true;
            this.ZipPackRadioButton.Location = new System.Drawing.Point(7, 44);
            this.ZipPackRadioButton.Name = "ZipPackRadioButton";
            this.ZipPackRadioButton.Size = new System.Drawing.Size(68, 17);
            this.ZipPackRadioButton.TabIndex = 1;
            this.ZipPackRadioButton.Text = "Zip Pack";
            this.ZipPackRadioButton.UseVisualStyleBackColor = true;
            // 
            // SolderPackRadioButton
            // 
            this.SolderPackRadioButton.AutoSize = true;
            this.SolderPackRadioButton.Checked = true;
            this.SolderPackRadioButton.Location = new System.Drawing.Point(7, 20);
            this.SolderPackRadioButton.Name = "SolderPackRadioButton";
            this.SolderPackRadioButton.Size = new System.Drawing.Size(83, 17);
            this.SolderPackRadioButton.TabIndex = 0;
            this.SolderPackRadioButton.TabStop = true;
            this.SolderPackRadioButton.Text = "Solder Pack";
            this.SolderPackRadioButton.UseVisualStyleBackColor = true;
            // 
            // technicPermissionsLevelGroupBox
            // 
            this.technicPermissionsLevelGroupBox.Controls.Add(this.technicPermissionsPublicPack);
            this.technicPermissionsLevelGroupBox.Controls.Add(this.technicPermissionsPrivatePack);
            this.technicPermissionsLevelGroupBox.Location = new System.Drawing.Point(162, 93);
            this.technicPermissionsLevelGroupBox.Name = "technicPermissionsLevelGroupBox";
            this.technicPermissionsLevelGroupBox.Size = new System.Drawing.Size(136, 64);
            this.technicPermissionsLevelGroupBox.TabIndex = 5;
            this.technicPermissionsLevelGroupBox.TabStop = false;
            this.technicPermissionsLevelGroupBox.Text = "Permissions Level";
            // 
            // technicPermissionsPublicPack
            // 
            this.technicPermissionsPublicPack.AutoSize = true;
            this.technicPermissionsPublicPack.Location = new System.Drawing.Point(7, 41);
            this.technicPermissionsPublicPack.Name = "technicPermissionsPublicPack";
            this.technicPermissionsPublicPack.Size = new System.Drawing.Size(82, 17);
            this.technicPermissionsPublicPack.TabIndex = 1;
            this.technicPermissionsPublicPack.Text = "Public Pack";
            this.technicPermissionsPublicPack.UseVisualStyleBackColor = true;
            // 
            // technicPermissionsPrivatePack
            // 
            this.technicPermissionsPrivatePack.AutoSize = true;
            this.technicPermissionsPrivatePack.Checked = true;
            this.technicPermissionsPrivatePack.Location = new System.Drawing.Point(7, 20);
            this.technicPermissionsPrivatePack.Name = "technicPermissionsPrivatePack";
            this.technicPermissionsPrivatePack.Size = new System.Drawing.Size(86, 17);
            this.technicPermissionsPrivatePack.TabIndex = 0;
            this.technicPermissionsPrivatePack.TabStop = true;
            this.technicPermissionsPrivatePack.Text = "Private Pack";
            this.technicPermissionsPrivatePack.UseVisualStyleBackColor = true;
            this.technicPermissionsPrivatePack.CheckedChanged += new System.EventHandler(this.technicPermissionsPrivatePack_CheckedChanged);
            // 
            // CheckPermissionsCheckBox
            // 
            this.CheckPermissionsCheckBox.AutoSize = true;
            this.CheckPermissionsCheckBox.Location = new System.Drawing.Point(20, 116);
            this.CheckPermissionsCheckBox.Name = "CheckPermissionsCheckBox";
            this.CheckPermissionsCheckBox.Size = new System.Drawing.Size(115, 17);
            this.CheckPermissionsCheckBox.TabIndex = 9;
            this.CheckPermissionsCheckBox.Text = "Check Permissions";
            this.CheckPermissionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreateConfigZipCheckBox
            // 
            this.CreateConfigZipCheckBox.AutoSize = true;
            this.CreateConfigZipCheckBox.Location = new System.Drawing.Point(20, 93);
            this.CreateConfigZipCheckBox.Name = "CreateConfigZipCheckBox";
            this.CreateConfigZipCheckBox.Size = new System.Drawing.Size(106, 17);
            this.CreateConfigZipCheckBox.TabIndex = 8;
            this.CreateConfigZipCheckBox.Text = "Create Config zip";
            this.CreateConfigZipCheckBox.UseVisualStyleBackColor = true;
            this.CreateConfigZipCheckBox.CheckedChanged += new System.EventHandler(this.CreateConfigZipCheckBox_CheckedChanged);
            // 
            // createForgeZipCheckBox
            // 
            this.createForgeZipCheckBox.AutoSize = true;
            this.createForgeZipCheckBox.Location = new System.Drawing.Point(162, 20);
            this.createForgeZipCheckBox.Name = "createForgeZipCheckBox";
            this.createForgeZipCheckBox.Size = new System.Drawing.Size(103, 17);
            this.createForgeZipCheckBox.TabIndex = 10;
            this.createForgeZipCheckBox.Text = "Create Forge zip";
            this.createForgeZipCheckBox.UseVisualStyleBackColor = true;
            this.createForgeZipCheckBox.CheckedChanged += new System.EventHandler(this.createForgeZipCheckBox_CheckedChanged);
            // 
            // ModpackHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 469);
            this.Controls.Add(this.technicOptionsGroupBox);
            this.Controls.Add(this.globalConfigurationsGroupBox);
            this.Controls.Add(this.startPackingButton);
            this.Name = "ModpackHelper";
            this.Text = "Modpack Helper";
            this.globalConfigurationsGroupBox.ResumeLayout(false);
            this.globalConfigurationsGroupBox.PerformLayout();
            this.ModpackSettingsGroupBox.ResumeLayout(false);
            this.ModpackSettingsGroupBox.PerformLayout();
            this.technicOptionsGroupBox.ResumeLayout(false);
            this.technicOptionsGroupBox.PerformLayout();
            this.PackTypeGroupBox.ResumeLayout(false);
            this.PackTypeGroupBox.PerformLayout();
            this.technicPermissionsLevelGroupBox.ResumeLayout(false);
            this.technicPermissionsLevelGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public TextBox InputDirectoryTextBox;
        public TextBox OutputDirectoryTextBox;
        public Button startPackingButton;
        public GroupBox globalConfigurationsGroupBox;
        public GroupBox technicOptionsGroupBox;
        public CheckBox createForgeZipCheckBox;
        public CheckBox CheckPermissionsCheckBox;
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
        public CheckBox checkBox4;
        public CheckBox CreateFTBPackCheckBox;
        public CheckBox CreateTechnicPackCheckBox;
        public CheckBox ClearOutpuDirectoryCheckBox;
        public Button browseForInputDirectoryButton;
        public Button browseForOutputDirectoryButton;

    }
}

