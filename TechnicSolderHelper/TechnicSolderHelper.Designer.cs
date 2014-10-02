namespace TechnicSolderHelper
{
    partial class SolderHelper
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InputDirectoryBrowse = new System.Windows.Forms.Button();
            this.OutputDirectoryBrowse = new System.Windows.Forms.Button();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.CreateFTBPack = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.CreateTechnicPack = new System.Windows.Forms.CheckBox();
            this.OutputFolder = new System.Windows.Forms.TextBox();
            this.InputFolder = new System.Windows.Forms.TextBox();
            this.SolderPackType = new System.Windows.Forms.GroupBox();
            this.ZipPack = new System.Windows.Forms.RadioButton();
            this.SolderPack = new System.Windows.Forms.RadioButton();
            this.IncludeConfigZip = new System.Windows.Forms.CheckBox();
            this.DistributionLevel = new System.Windows.Forms.GroupBox();
            this.PublicFTBPack = new System.Windows.Forms.RadioButton();
            this.PrivateFTBPack = new System.Windows.Forms.RadioButton();
            this.SolderPackType.SuspendLayout();
            this.DistributionLevel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Directory with mods";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Output Directory";
            // 
            // InputDirectoryBrowse
            // 
            this.InputDirectoryBrowse.Location = new System.Drawing.Point(451, 25);
            this.InputDirectoryBrowse.Name = "InputDirectoryBrowse";
            this.InputDirectoryBrowse.Size = new System.Drawing.Size(85, 23);
            this.InputDirectoryBrowse.TabIndex = 1;
            this.InputDirectoryBrowse.Text = "Browse...";
            this.InputDirectoryBrowse.UseVisualStyleBackColor = true;
            this.InputDirectoryBrowse.Click += new System.EventHandler(this.InputDirectoryBrowse_Click);
            // 
            // OutputDirectoryBrowse
            // 
            this.OutputDirectoryBrowse.Location = new System.Drawing.Point(449, 61);
            this.OutputDirectoryBrowse.Name = "OutputDirectoryBrowse";
            this.OutputDirectoryBrowse.Size = new System.Drawing.Size(85, 23);
            this.OutputDirectoryBrowse.TabIndex = 1;
            this.OutputDirectoryBrowse.Text = "Browse...";
            this.OutputDirectoryBrowse.UseVisualStyleBackColor = true;
            this.OutputDirectoryBrowse.Click += new System.EventHandler(this.OutputDirectoryBrowse_Click);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.Description = "Select the Directory which contains the modpacks mods.";
            this.FolderBrowser.ShowNewFolderButton = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(545, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 59);
            this.button1.TabIndex = 2;
            this.button1.Text = "GO";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(510, 429);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Reset database";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(470, 104);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(159, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Clear output directory on run";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(510, 376);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 47);
            this.button3.TabIndex = 6;
            this.button3.Text = "Update Stored FTB permissions";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // CreateFTBPack
            // 
            this.CreateFTBPack.AutoSize = true;
            this.CreateFTBPack.Location = new System.Drawing.Point(11, 243);
            this.CreateFTBPack.Name = "CreateFTBPack";
            this.CreateFTBPack.Size = new System.Drawing.Size(108, 17);
            this.CreateFTBPack.TabIndex = 7;
            this.CreateFTBPack.Text = "Create FTB Pack";
            this.CreateFTBPack.UseVisualStyleBackColor = true;
            this.CreateFTBPack.CheckedChanged += new System.EventHandler(this.CreateFTBPack_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(467, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Currently doing:";
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(467, 142);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(52, 13);
            this.ProgressLabel.TabIndex = 8;
            this.ProgressLabel.Text = "Waiting...";
            // 
            // CreateTechnicPack
            // 
            this.CreateTechnicPack.AutoSize = true;
            this.CreateTechnicPack.Checked = true;
            this.CreateTechnicPack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateTechnicPack.Location = new System.Drawing.Point(11, 104);
            this.CreateTechnicPack.Name = "CreateTechnicPack";
            this.CreateTechnicPack.Size = new System.Drawing.Size(127, 17);
            this.CreateTechnicPack.TabIndex = 9;
            this.CreateTechnicPack.Text = "Create Technic Pack";
            this.CreateTechnicPack.UseVisualStyleBackColor = true;
            this.CreateTechnicPack.CheckedChanged += new System.EventHandler(this.CreateTechnicPack_CheckedChanged);
            // 
            // OutputFolder
            // 
            this.OutputFolder.Location = new System.Drawing.Point(11, 64);
            this.OutputFolder.MaxLength = 2000;
            this.OutputFolder.Name = "OutputFolder";
            this.OutputFolder.Size = new System.Drawing.Size(432, 20);
            this.OutputFolder.TabIndex = 0;
            this.OutputFolder.Text = "C:\\SolderHelper";
            this.OutputFolder.TextChanged += new System.EventHandler(this.OutputFolder_TextChanged);
            // 
            // InputFolder
            // 
            this.InputFolder.Location = new System.Drawing.Point(12, 25);
            this.InputFolder.Name = "InputFolder";
            this.InputFolder.Size = new System.Drawing.Size(432, 20);
            this.InputFolder.TabIndex = 0;
            this.InputFolder.Text = "C:\\Users\\User\\AppData\\Roaming\\.minecraft\\mods";
            this.InputFolder.TextChanged += new System.EventHandler(this.InputFolder_TextChanged);
            // 
            // SolderPackType
            // 
            this.SolderPackType.Controls.Add(this.ZipPack);
            this.SolderPackType.Controls.Add(this.SolderPack);
            this.SolderPackType.Controls.Add(this.IncludeConfigZip);
            this.SolderPackType.Location = new System.Drawing.Point(14, 127);
            this.SolderPackType.Name = "SolderPackType";
            this.SolderPackType.Size = new System.Drawing.Size(148, 100);
            this.SolderPackType.TabIndex = 10;
            this.SolderPackType.TabStop = false;
            this.SolderPackType.Text = "Pack Type";
            this.SolderPackType.Visible = global::TechnicSolderHelper.Properties.Settings.Default.CreateTechnicSolderFiles;
            // 
            // ZipPack
            // 
            this.ZipPack.AutoSize = true;
            this.ZipPack.Location = new System.Drawing.Point(20, 42);
            this.ZipPack.Name = "ZipPack";
            this.ZipPack.Size = new System.Drawing.Size(68, 17);
            this.ZipPack.TabIndex = 0;
            this.ZipPack.Text = "Zip Pack";
            this.ZipPack.UseVisualStyleBackColor = true;
            // 
            // SolderPack
            // 
            this.SolderPack.AutoSize = true;
            this.SolderPack.Checked = true;
            this.SolderPack.Location = new System.Drawing.Point(20, 19);
            this.SolderPack.Name = "SolderPack";
            this.SolderPack.Size = new System.Drawing.Size(83, 17);
            this.SolderPack.TabIndex = 0;
            this.SolderPack.TabStop = true;
            this.SolderPack.Text = "Solder Pack";
            this.SolderPack.UseVisualStyleBackColor = true;
            this.SolderPack.CheckedChanged += new System.EventHandler(this.SolderPack_CheckedChanged);
            // 
            // IncludeConfigZip
            // 
            this.IncludeConfigZip.AutoSize = true;
            this.IncludeConfigZip.Checked = global::TechnicSolderHelper.Properties.Settings.Default.CreateTechnicConfigZip;
            this.IncludeConfigZip.Location = new System.Drawing.Point(20, 65);
            this.IncludeConfigZip.Name = "IncludeConfigZip";
            this.IncludeConfigZip.Size = new System.Drawing.Size(108, 17);
            this.IncludeConfigZip.TabIndex = 5;
            this.IncludeConfigZip.Text = "Create Config Zip";
            this.IncludeConfigZip.UseVisualStyleBackColor = true;
            this.IncludeConfigZip.CheckedChanged += new System.EventHandler(this.IncludeConfigZip_CheckedChanged);
            // 
            // DistributionLevel
            // 
            this.DistributionLevel.Controls.Add(this.PublicFTBPack);
            this.DistributionLevel.Controls.Add(this.PrivateFTBPack);
            this.DistributionLevel.Location = new System.Drawing.Point(16, 266);
            this.DistributionLevel.Name = "DistributionLevel";
            this.DistributionLevel.Size = new System.Drawing.Size(146, 70);
            this.DistributionLevel.TabIndex = 11;
            this.DistributionLevel.TabStop = false;
            this.DistributionLevel.Text = "Distribution Level";
            this.DistributionLevel.Visible = global::TechnicSolderHelper.Properties.Settings.Default.CreateFTBPack;
            // 
            // PublicFTBPack
            // 
            this.PublicFTBPack.AutoSize = true;
            this.PublicFTBPack.Location = new System.Drawing.Point(18, 44);
            this.PublicFTBPack.Name = "PublicFTBPack";
            this.PublicFTBPack.Size = new System.Drawing.Size(105, 17);
            this.PublicFTBPack.TabIndex = 1;
            this.PublicFTBPack.TabStop = true;
            this.PublicFTBPack.Text = "Public FTB Pack";
            this.PublicFTBPack.UseVisualStyleBackColor = true;
            // 
            // PrivateFTBPack
            // 
            this.PrivateFTBPack.AutoSize = true;
            this.PrivateFTBPack.Location = new System.Drawing.Point(18, 20);
            this.PrivateFTBPack.Name = "PrivateFTBPack";
            this.PrivateFTBPack.Size = new System.Drawing.Size(109, 17);
            this.PrivateFTBPack.TabIndex = 0;
            this.PrivateFTBPack.TabStop = true;
            this.PrivateFTBPack.Text = "Private FTB Pack";
            this.PrivateFTBPack.UseVisualStyleBackColor = true;
            this.PrivateFTBPack.CheckedChanged += new System.EventHandler(this.PrivateFTBPack_CheckedChanged);
            // 
            // SolderHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 464);
            this.Controls.Add(this.DistributionLevel);
            this.Controls.Add(this.SolderPackType);
            this.Controls.Add(this.CreateTechnicPack);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CreateFTBPack);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OutputDirectoryBrowse);
            this.Controls.Add(this.InputDirectoryBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutputFolder);
            this.Controls.Add(this.InputFolder);
            this.Name = "SolderHelper";
            this.Text = "SolderHelper";
            this.SolderPackType.ResumeLayout(false);
            this.SolderPackType.PerformLayout();
            this.DistributionLevel.ResumeLayout(false);
            this.DistributionLevel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InputFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OutputFolder;
        private System.Windows.Forms.Button InputDirectoryBrowse;
        private System.Windows.Forms.Button OutputDirectoryBrowse;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox IncludeConfigZip;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox CreateFTBPack;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.CheckBox CreateTechnicPack;
        private System.Windows.Forms.GroupBox SolderPackType;
        private System.Windows.Forms.RadioButton ZipPack;
        private System.Windows.Forms.RadioButton SolderPack;
        private System.Windows.Forms.GroupBox DistributionLevel;
        private System.Windows.Forms.RadioButton PublicFTBPack;
        private System.Windows.Forms.RadioButton PrivateFTBPack;
    }
}