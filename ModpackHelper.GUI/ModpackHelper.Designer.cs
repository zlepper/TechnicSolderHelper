namespace ModpackHelper.GUI
{
    partial class ModpackHelper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.inputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.browseForInputDirectoryButton = new System.Windows.Forms.Button();
            this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.browseForOutputDirectoryButton = new System.Windows.Forms.Button();
            this.startPackingButton = new System.Windows.Forms.Button();
            this.globalConfigurationsGroupBox = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.ClearOutpuDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.getForgeVersionsButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.minecraftVersionDropdown = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.technicOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.forgeVersionLabel = new System.Windows.Forms.Label();
            this.forgeVersionDropdown = new System.Windows.Forms.ComboBox();
            this.PackTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.ZipPackRadioButton = new System.Windows.Forms.RadioButton();
            this.SolderPackRadioButton = new System.Windows.Forms.RadioButton();
            this.technicPermissionsLevelGroupBox = new System.Windows.Forms.GroupBox();
            this.technicPermissionsPublicPack = new System.Windows.Forms.RadioButton();
            this.technicPermissionsPrivatePack = new System.Windows.Forms.RadioButton();
            this.checkPermissionsCheckBox = new System.Windows.Forms.CheckBox();
            this.createConfigZipCheckBox = new System.Windows.Forms.CheckBox();
            this.createForgeZipCheckBox = new System.Windows.Forms.CheckBox();
            inputFolderLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.globalConfigurationsGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 16);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 13);
            label2.TabIndex = 0;
            label2.Text = "Modpack Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 55);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(90, 13);
            label3.TabIndex = 0;
            label3.Text = "Modpack Version";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 94);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(89, 13);
            label4.TabIndex = 0;
            label4.Text = "Minecraft Version";
            // 
            // inputDirectoryTextBox
            // 
            this.inputDirectoryTextBox.Location = new System.Drawing.Point(6, 33);
            this.inputDirectoryTextBox.Name = "inputDirectoryTextBox";
            this.inputDirectoryTextBox.Size = new System.Drawing.Size(478, 20);
            this.inputDirectoryTextBox.TabIndex = 0;
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
            // outputDirectoryTextBox
            // 
            this.outputDirectoryTextBox.Location = new System.Drawing.Point(6, 74);
            this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
            this.outputDirectoryTextBox.Size = new System.Drawing.Size(478, 20);
            this.outputDirectoryTextBox.TabIndex = 0;
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
            this.startPackingButton.Location = new System.Drawing.Point(707, 367);
            this.startPackingButton.Name = "startPackingButton";
            this.startPackingButton.Size = new System.Drawing.Size(78, 64);
            this.startPackingButton.TabIndex = 2;
            this.startPackingButton.Text = "Start";
            this.startPackingButton.UseVisualStyleBackColor = true;
            // 
            // globalConfigurationsGroupBox
            // 
            this.globalConfigurationsGroupBox.Controls.Add(this.checkBox4);
            this.globalConfigurationsGroupBox.Controls.Add(this.ClearOutpuDirectoryCheckBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.checkBox2);
            this.globalConfigurationsGroupBox.Controls.Add(this.checkBox1);
            this.globalConfigurationsGroupBox.Controls.Add(this.groupBox2);
            this.globalConfigurationsGroupBox.Controls.Add(this.button4);
            this.globalConfigurationsGroupBox.Controls.Add(this.button3);
            this.globalConfigurationsGroupBox.Controls.Add(this.button2);
            this.globalConfigurationsGroupBox.Controls.Add(this.button1);
            this.globalConfigurationsGroupBox.Controls.Add(this.getForgeVersionsButton);
            this.globalConfigurationsGroupBox.Controls.Add(this.groupBox1);
            this.globalConfigurationsGroupBox.Controls.Add(this.inputDirectoryTextBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.outputDirectoryTextBox);
            this.globalConfigurationsGroupBox.Controls.Add(this.browseForOutputDirectoryButton);
            this.globalConfigurationsGroupBox.Controls.Add(inputFolderLabel);
            this.globalConfigurationsGroupBox.Controls.Add(this.browseForInputDirectoryButton);
            this.globalConfigurationsGroupBox.Controls.Add(label1);
            this.globalConfigurationsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.globalConfigurationsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.globalConfigurationsGroupBox.Name = "globalConfigurationsGroupBox";
            this.globalConfigurationsGroupBox.Size = new System.Drawing.Size(883, 191);
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
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(142, 159);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 17);
            this.checkBox2.TabIndex = 10;
            this.checkBox2.Text = "Create FTB Pack";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 159);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(127, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Create Technic Pack";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new System.Drawing.Point(773, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(104, 68);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Missing Info";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 45);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(96, 17);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Ask as needed";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 22);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Create list";
            this.radioButton1.UseVisualStyleBackColor = true;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.minecraftVersionDropdown);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Location = new System.Drawing.Point(573, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 141);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modpack Settings";
            // 
            // minecraftVersionDropdown
            // 
            this.minecraftVersionDropdown.FormattingEnabled = true;
            this.minecraftVersionDropdown.Location = new System.Drawing.Point(6, 110);
            this.minecraftVersionDropdown.Name = "minecraftVersionDropdown";
            this.minecraftVersionDropdown.Size = new System.Drawing.Size(182, 21);
            this.minecraftVersionDropdown.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 71);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(182, 20);
            this.textBox2.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(182, 20);
            this.textBox1.TabIndex = 0;
            // 
            // technicOptionsGroupBox
            // 
            this.technicOptionsGroupBox.Controls.Add(this.forgeVersionLabel);
            this.technicOptionsGroupBox.Controls.Add(this.forgeVersionDropdown);
            this.technicOptionsGroupBox.Controls.Add(this.PackTypeGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.technicPermissionsLevelGroupBox);
            this.technicOptionsGroupBox.Controls.Add(this.checkPermissionsCheckBox);
            this.technicOptionsGroupBox.Controls.Add(this.createConfigZipCheckBox);
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
            this.forgeVersionDropdown.FormattingEnabled = true;
            this.forgeVersionDropdown.Location = new System.Drawing.Point(162, 60);
            this.forgeVersionDropdown.Name = "forgeVersionDropdown";
            this.forgeVersionDropdown.Size = new System.Drawing.Size(121, 21);
            this.forgeVersionDropdown.TabIndex = 7;
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
            // 
            // checkPermissionsCheckBox
            // 
            this.checkPermissionsCheckBox.AutoSize = true;
            this.checkPermissionsCheckBox.Location = new System.Drawing.Point(20, 116);
            this.checkPermissionsCheckBox.Name = "checkPermissionsCheckBox";
            this.checkPermissionsCheckBox.Size = new System.Drawing.Size(115, 17);
            this.checkPermissionsCheckBox.TabIndex = 1;
            this.checkPermissionsCheckBox.Text = "Check Permissions";
            this.checkPermissionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // createConfigZipCheckBox
            // 
            this.createConfigZipCheckBox.AutoSize = true;
            this.createConfigZipCheckBox.Location = new System.Drawing.Point(20, 93);
            this.createConfigZipCheckBox.Name = "createConfigZipCheckBox";
            this.createConfigZipCheckBox.Size = new System.Drawing.Size(106, 17);
            this.createConfigZipCheckBox.TabIndex = 1;
            this.createConfigZipCheckBox.Text = "Create Config zip";
            this.createConfigZipCheckBox.UseVisualStyleBackColor = true;
            // 
            // createForgeZipCheckBox
            // 
            this.createForgeZipCheckBox.AutoSize = true;
            this.createForgeZipCheckBox.Location = new System.Drawing.Point(162, 20);
            this.createForgeZipCheckBox.Name = "createForgeZipCheckBox";
            this.createForgeZipCheckBox.Size = new System.Drawing.Size(103, 17);
            this.createForgeZipCheckBox.TabIndex = 1;
            this.createForgeZipCheckBox.Text = "Create Forge zip";
            this.createForgeZipCheckBox.UseVisualStyleBackColor = true;
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.technicOptionsGroupBox.ResumeLayout(false);
            this.technicOptionsGroupBox.PerformLayout();
            this.PackTypeGroupBox.ResumeLayout(false);
            this.PackTypeGroupBox.PerformLayout();
            this.technicPermissionsLevelGroupBox.ResumeLayout(false);
            this.technicPermissionsLevelGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox inputDirectoryTextBox;
        public System.Windows.Forms.TextBox outputDirectoryTextBox;
        private System.Windows.Forms.Button startPackingButton;
        private System.Windows.Forms.GroupBox globalConfigurationsGroupBox;
        private System.Windows.Forms.GroupBox technicOptionsGroupBox;
        private System.Windows.Forms.CheckBox createForgeZipCheckBox;
        private System.Windows.Forms.CheckBox checkPermissionsCheckBox;
        private System.Windows.Forms.CheckBox createConfigZipCheckBox;
        private System.Windows.Forms.GroupBox technicPermissionsLevelGroupBox;
        private System.Windows.Forms.RadioButton technicPermissionsPublicPack;
        private System.Windows.Forms.RadioButton technicPermissionsPrivatePack;
        private System.Windows.Forms.GroupBox PackTypeGroupBox;
        private System.Windows.Forms.RadioButton ZipPackRadioButton;
        private System.Windows.Forms.RadioButton SolderPackRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button getForgeVersionsButton;
        private System.Windows.Forms.ComboBox minecraftVersionDropdown;
        private System.Windows.Forms.Label forgeVersionLabel;
        private System.Windows.Forms.ComboBox forgeVersionDropdown;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.CheckBox ClearOutpuDirectoryCheckBox;
        public System.Windows.Forms.Button browseForInputDirectoryButton;
        public System.Windows.Forms.Button browseForOutputDirectoryButton;

    }
}

