using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using TechnicSolderHelper.SQL;
using TechnicSolderHelper.Properties;

namespace TechnicSolderHelper
{
    partial class SolderHelper
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
            this.InputFolder = new System.Windows.Forms.ComboBox();
            this.UploadToFTPServer = new System.Windows.Forms.CheckBox();
            this.GetForgeVersions = new System.Windows.Forms.Button();
            this.missingInfoAction = new System.Windows.Forms.GroupBox();
            this.missingInfoActionCreateList = new System.Windows.Forms.RadioButton();
            this.missingInfoActionOnTheRun = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.ModpackNameInput = new System.Windows.Forms.ComboBox();
            this.ModpackVersionInput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.getliteloaderversions = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.configureFTP = new System.Windows.Forms.Button();
            this.labelforgeversion = new System.Windows.Forms.Label();
            this.labelmcversion = new System.Windows.Forms.Label();
            this.ForgeBuild = new System.Windows.Forms.ListBox();
            this.MCversion = new System.Windows.Forms.ListBox();
            this.TechnicDistributionLevel = new System.Windows.Forms.GroupBox();
            this.TechnicPublicPermissions = new System.Windows.Forms.RadioButton();
            this.TechnicPrivatePermissions = new System.Windows.Forms.RadioButton();
            this.DistributionLevel = new System.Windows.Forms.GroupBox();
            this.PublicFTBPack = new System.Windows.Forms.RadioButton();
            this.PrivateFTBPack = new System.Windows.Forms.RadioButton();
            this.SolderPackType = new System.Windows.Forms.GroupBox();
            this.IncludeForgeVersion = new System.Windows.Forms.CheckBox();
            this.CheckPermissions = new System.Windows.Forms.CheckBox();
            this.ZipPack = new System.Windows.Forms.RadioButton();
            this.SolderPack = new System.Windows.Forms.RadioButton();
            this.IncludeConfigZip = new System.Windows.Forms.CheckBox();
            this.testmysql = new System.Windows.Forms.Button();
            this.useSolder = new System.Windows.Forms.CheckBox();
            this.configureSolder = new System.Windows.Forms.Button();
            this.savesqlcommands = new System.Windows.Forms.CheckBox();
            this.UseS3 = new System.Windows.Forms.CheckBox();
            this.ConfigureS3 = new System.Windows.Forms.Button();
            this.Resize += new EventHandler(this.form_resize);
            this.missingInfoAction.SuspendLayout();
            this.TechnicDistributionLevel.SuspendLayout();
            this.DistributionLevel.SuspendLayout();
            this.SolderPackType.SuspendLayout();
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
            this.CreateFTBPack.Location = new System.Drawing.Point(11, 298);
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
            this.ProgressLabel.TabIndex = 0;
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
            this.OutputFolder.TextChanged += new System.EventHandler(this.OutputFolder_TextChanged);
            // 
            // InputFolder
            // 
            this.InputFolder.Location = new System.Drawing.Point(12, 25);
            this.InputFolder.Name = "InputFolder";
            this.InputFolder.Size = new System.Drawing.Size(432, 21);
            this.InputFolder.TabIndex = 0;
            this.InputFolder.Text = "C:\\Users\\User\\AppData\\Roaming\\.minecraft\\mods";
            this.InputFolder.TextChanged += new System.EventHandler(this.InputFolder_TextChanged);
            // 
            // UploadToFTPServer
            // 
            this.UploadToFTPServer.AutoSize = true;
            this.UploadToFTPServer.Location = new System.Drawing.Point(11, 399);
            this.UploadToFTPServer.Name = "UploadToFTPServer";
            this.UploadToFTPServer.Size = new System.Drawing.Size(135, 17);
            this.UploadToFTPServer.TabIndex = 12;
            this.UploadToFTPServer.Text = "Upload to FTP address";
            this.UploadToFTPServer.UseVisualStyleBackColor = true;
            this.UploadToFTPServer.CheckedChanged += new System.EventHandler(this.UploadToFTPServer_CheckedChanged);
            // 
            // GetForgeVersions
            // 
            this.GetForgeVersions.Location = new System.Drawing.Point(510, 343);
            this.GetForgeVersions.Name = "GetForgeVersions";
            this.GetForgeVersions.Size = new System.Drawing.Size(119, 27);
            this.GetForgeVersions.TabIndex = 14;
            this.GetForgeVersions.Text = "Get Forge versions";
            this.GetForgeVersions.UseVisualStyleBackColor = true;
            this.GetForgeVersions.Click += new System.EventHandler(this.GetForgeVersions_Click);
            // 
            // missingInfoAction
            // 
            this.missingInfoAction.Controls.Add(this.missingInfoActionCreateList);
            this.missingInfoAction.Controls.Add(this.missingInfoActionOnTheRun);
            this.missingInfoAction.Location = new System.Drawing.Point(510, 186);
            this.missingInfoAction.Name = "missingInfoAction";
            this.missingInfoAction.Size = new System.Drawing.Size(110, 69);
            this.missingInfoAction.TabIndex = 0;
            this.missingInfoAction.TabStop = false;
            this.missingInfoAction.Text = "Missing Info";
            this.missingInfoAction.Visible = false;
            // 
            // missingInfoActionCreateList
            // 
            this.missingInfoActionCreateList.AutoSize = true;
            this.missingInfoActionCreateList.Location = new System.Drawing.Point(7, 44);
            this.missingInfoActionCreateList.Name = "missingInfoActionCreateList";
            this.missingInfoActionCreateList.Size = new System.Drawing.Size(75, 17);
            this.missingInfoActionCreateList.TabIndex = 1;
            this.missingInfoActionCreateList.Text = "Create List";
            this.missingInfoActionCreateList.UseVisualStyleBackColor = true;
            // 
            // missingInfoActionOnTheRun
            // 
            this.missingInfoActionOnTheRun.AutoSize = true;
            this.missingInfoActionOnTheRun.Checked = true;
            this.missingInfoActionOnTheRun.Location = new System.Drawing.Point(7, 20);
            this.missingInfoActionOnTheRun.Name = "missingInfoActionOnTheRun";
            this.missingInfoActionOnTheRun.Size = new System.Drawing.Size(96, 17);
            this.missingInfoActionOnTheRun.TabIndex = 0;
            this.missingInfoActionOnTheRun.TabStop = true;
            this.missingInfoActionOnTheRun.Text = "Ask as needed";
            this.missingInfoActionOnTheRun.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(507, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Modpack Name";
            // 
            // ModpackNameInput
            // 
            this.ModpackNameInput.FormattingEnabled = true;
            this.ModpackNameInput.Location = new System.Drawing.Point(507, 274);
            this.ModpackNameInput.Name = "ModpackNameInput";
            this.ModpackNameInput.Size = new System.Drawing.Size(121, 21);
            this.ModpackNameInput.TabIndex = 18;
            // 
            // ModpackVersionInput
            // 
            this.ModpackVersionInput.Location = new System.Drawing.Point(507, 317);
            this.ModpackVersionInput.Name = "ModpackVersionInput";
            this.ModpackVersionInput.Size = new System.Drawing.Size(122, 20);
            this.ModpackVersionInput.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(507, 302);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Modpack version";
            // 
            // getliteloaderversions
            // 
            this.getliteloaderversions.Location = new System.Drawing.Point(429, 400);
            this.getliteloaderversions.Name = "getliteloaderversions";
            this.getliteloaderversions.Size = new System.Drawing.Size(75, 52);
            this.getliteloaderversions.TabIndex = 21;
            this.getliteloaderversions.Text = "Get liteloader versions";
            this.getliteloaderversions.UseVisualStyleBackColor = true;
            this.getliteloaderversions.Click += new System.EventHandler(this.getliteloaderversions_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(636, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 427);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Additional folders";
            // 
            // configureFTP
            // 
            this.configureFTP.Location = new System.Drawing.Point(12, 424);
            this.configureFTP.Name = "configureFTP";
            this.configureFTP.Size = new System.Drawing.Size(107, 23);
            this.configureFTP.TabIndex = 24;
            this.configureFTP.Text = "Configure FTP";
            this.configureFTP.UseVisualStyleBackColor = true;
            this.configureFTP.Click += new System.EventHandler(this.configureFTP_Click);
            // 
            // labelforgeversion
            // 
            this.labelforgeversion.AutoSize = true;
            this.labelforgeversion.Location = new System.Drawing.Point(172, 169);
            this.labelforgeversion.Name = "labelforgeversion";
            this.labelforgeversion.Size = new System.Drawing.Size(72, 13);
            this.labelforgeversion.TabIndex = 16;
            this.labelforgeversion.Text = "Forge Version";
            // 
            // labelmcversion
            // 
            this.labelmcversion.AutoSize = true;
            this.labelmcversion.Location = new System.Drawing.Point(172, 127);
            this.labelmcversion.Name = "labelmcversion";
            this.labelmcversion.Size = new System.Drawing.Size(57, 13);
            this.labelmcversion.TabIndex = 15;
            this.labelmcversion.Text = "MCversion";
            // 
            // ForgeBuild
            // 
            this.ForgeBuild.FormattingEnabled = true;
            this.ForgeBuild.Location = new System.Drawing.Point(250, 169);
            this.ForgeBuild.Name = "ForgeBuild";
            this.ForgeBuild.Size = new System.Drawing.Size(123, 30);
            this.ForgeBuild.TabIndex = 0;
            // 
            // MCversion
            // 
            this.MCversion.FormattingEnabled = true;
            this.MCversion.Location = new System.Drawing.Point(250, 127);
            this.MCversion.Name = "MCversion";
            this.MCversion.Size = new System.Drawing.Size(123, 30);
            this.MCversion.TabIndex = 1;
            this.MCversion.SelectedIndexChanged += new System.EventHandler(this.MCversion_SelectedIndexChanged);
            // 
            // TechnicDistributionLevel
            // 
            this.TechnicDistributionLevel.Controls.Add(this.TechnicPublicPermissions);
            this.TechnicDistributionLevel.Controls.Add(this.TechnicPrivatePermissions);
            this.TechnicDistributionLevel.Location = new System.Drawing.Point(168, 207);
            this.TechnicDistributionLevel.Name = "TechnicDistributionLevel";
            this.TechnicDistributionLevel.Size = new System.Drawing.Size(136, 73);
            this.TechnicDistributionLevel.TabIndex = 7;
            this.TechnicDistributionLevel.TabStop = false;
            this.TechnicDistributionLevel.Text = "Permissions Level";
            // 
            // TechnicPublicPermissions
            // 
            this.TechnicPublicPermissions.AutoSize = true;
            this.TechnicPublicPermissions.Location = new System.Drawing.Point(7, 43);
            this.TechnicPublicPermissions.Name = "TechnicPublicPermissions";
            this.TechnicPublicPermissions.Size = new System.Drawing.Size(82, 17);
            this.TechnicPublicPermissions.TabIndex = 1;
            this.TechnicPublicPermissions.TabStop = true;
            this.TechnicPublicPermissions.Text = "Public Pack";
            this.TechnicPublicPermissions.UseVisualStyleBackColor = true;
            // 
            // TechnicPrivatePermissions
            // 
            this.TechnicPrivatePermissions.AutoSize = true;
            this.TechnicPrivatePermissions.Location = new System.Drawing.Point(7, 20);
            this.TechnicPrivatePermissions.Name = "TechnicPrivatePermissions";
            this.TechnicPrivatePermissions.Size = new System.Drawing.Size(86, 17);
            this.TechnicPrivatePermissions.TabIndex = 0;
            this.TechnicPrivatePermissions.TabStop = true;
            this.TechnicPrivatePermissions.Text = "Private Pack";
            this.TechnicPrivatePermissions.UseVisualStyleBackColor = true;
            this.TechnicPrivatePermissions.CheckedChanged += new System.EventHandler(this.TechnicPrivatePermissions_CheckedChanged);
            // 
            // DistributionLevel
            // 
            this.DistributionLevel.Controls.Add(this.PublicFTBPack);
            this.DistributionLevel.Controls.Add(this.PrivateFTBPack);
            this.DistributionLevel.Location = new System.Drawing.Point(12, 323);
            this.DistributionLevel.Name = "DistributionLevel";
            this.DistributionLevel.Size = new System.Drawing.Size(146, 70);
            this.DistributionLevel.TabIndex = 11;
            this.DistributionLevel.TabStop = false;
            this.DistributionLevel.Text = "Distribution Level";
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
            // SolderPackType
            // 
            this.SolderPackType.Controls.Add(this.IncludeForgeVersion);
            this.SolderPackType.Controls.Add(this.CheckPermissions);
            this.SolderPackType.Controls.Add(this.ZipPack);
            this.SolderPackType.Controls.Add(this.SolderPack);
            this.SolderPackType.Controls.Add(this.IncludeConfigZip);
            this.SolderPackType.Location = new System.Drawing.Point(11, 127);
            this.SolderPackType.Name = "SolderPackType";
            this.SolderPackType.Size = new System.Drawing.Size(148, 153);
            this.SolderPackType.TabIndex = 10;
            this.SolderPackType.TabStop = false;
            this.SolderPackType.Text = "Pack Type";
            // 
            // IncludeForgeVersion
            // 
            this.IncludeForgeVersion.AutoSize = true;
            this.IncludeForgeVersion.Location = new System.Drawing.Point(20, 65);
            this.IncludeForgeVersion.Name = "IncludeForgeVersion";
            this.IncludeForgeVersion.Size = new System.Drawing.Size(109, 17);
            this.IncludeForgeVersion.TabIndex = 13;
            this.IncludeForgeVersion.Text = "Include Forge Zip";
            this.IncludeForgeVersion.UseVisualStyleBackColor = true;
            this.IncludeForgeVersion.CheckedChanged += new System.EventHandler(this.IncludeForgeVersion_CheckedChanged);
            // 
            // CheckPermissions
            // 
            this.CheckPermissions.AutoSize = true;
            this.CheckPermissions.Location = new System.Drawing.Point(20, 111);
            this.CheckPermissions.Name = "CheckPermissions";
            this.CheckPermissions.Size = new System.Drawing.Size(114, 17);
            this.CheckPermissions.TabIndex = 6;
            this.CheckPermissions.Text = "Check permissions";
            this.CheckPermissions.UseVisualStyleBackColor = true;
            this.CheckPermissions.CheckedChanged += new System.EventHandler(this.CheckPermissions_CheckedChanged);
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
            this.IncludeConfigZip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncludeConfigZip.Location = new System.Drawing.Point(20, 88);
            this.IncludeConfigZip.Name = "IncludeConfigZip";
            this.IncludeConfigZip.Size = new System.Drawing.Size(108, 17);
            this.IncludeConfigZip.TabIndex = 5;
            this.IncludeConfigZip.Text = "Create Config Zip";
            this.IncludeConfigZip.UseVisualStyleBackColor = true;
            this.IncludeConfigZip.CheckedChanged += new System.EventHandler(this.IncludeConfigZip_CheckedChanged);
            // 
            // testmysql
            // 
            this.testmysql.Location = new System.Drawing.Point(429, 371);
            this.testmysql.Name = "testmysql";
            this.testmysql.Size = new System.Drawing.Size(75, 23);
            this.testmysql.TabIndex = 25;
            this.testmysql.Text = "Test mysql";
            this.testmysql.UseVisualStyleBackColor = true;
            this.testmysql.Visible = false;
            this.testmysql.Click += new System.EventHandler(this.testmysql_Click);
            // 
            // useSolder
            // 
            this.useSolder.AutoSize = true;
            this.useSolder.Location = new System.Drawing.Point(168, 298);
            this.useSolder.Name = "useSolder";
            this.useSolder.Size = new System.Drawing.Size(128, 17);
            this.useSolder.TabIndex = 26;
            this.useSolder.Text = "Use solder installation";
            this.useSolder.UseVisualStyleBackColor = true;
            this.useSolder.CheckedChanged += new System.EventHandler(this.useSolder_CheckedChanged);
            // 
            // configureSolder
            // 
            this.configureSolder.Location = new System.Drawing.Point(168, 321);
            this.configureSolder.Name = "configureSolder";
            this.configureSolder.Size = new System.Drawing.Size(103, 23);
            this.configureSolder.TabIndex = 27;
            this.configureSolder.Text = "Configure solder";
            this.configureSolder.UseVisualStyleBackColor = true;
            this.configureSolder.Click += new System.EventHandler(this.configureSolder_Click);
            // 
            // savesqlcommands
            // 
            this.savesqlcommands.AutoSize = true;
            this.savesqlcommands.Location = new System.Drawing.Point(168, 351);
            this.savesqlcommands.Name = "savesqlcommands";
            this.savesqlcommands.Size = new System.Drawing.Size(132, 17);
            this.savesqlcommands.TabIndex = 28;
            this.savesqlcommands.Text = "Save SQL commands ";
            this.savesqlcommands.UseVisualStyleBackColor = true;
            this.savesqlcommands.Visible = false;
            this.savesqlcommands.CheckedChanged += new System.EventHandler(this.savesqlcommands_CheckedChanged);
            // 
            // UseS3
            // 
            this.UseS3.AutoSize = true;
            this.UseS3.Location = new System.Drawing.Point(168, 375);
            this.UseS3.Name = "UseS3";
            this.UseS3.Size = new System.Drawing.Size(61, 17);
            this.UseS3.TabIndex = 29;
            this.UseS3.Text = "Use S3";
            this.UseS3.UseVisualStyleBackColor = true;
            this.UseS3.CheckedChanged += new System.EventHandler(this.UseS3_CheckedChanged);
            // 
            // ConfigureS3
            // 
            this.ConfigureS3.Location = new System.Drawing.Point(168, 399);
            this.ConfigureS3.Name = "ConfigureS3";
            this.ConfigureS3.Size = new System.Drawing.Size(103, 23);
            this.ConfigureS3.TabIndex = 30;
            this.ConfigureS3.Text = "Configure S3";
            this.ConfigureS3.UseVisualStyleBackColor = true;
            this.ConfigureS3.Click += new System.EventHandler(this.ConfigureS3_Click);
            // 
            // SolderHelper
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 464);
            this.Controls.Add(this.ConfigureS3);
            this.Controls.Add(this.UseS3);
            this.Controls.Add(this.savesqlcommands);
            this.Controls.Add(this.configureSolder);
            this.Controls.Add(this.useSolder);
            this.Controls.Add(this.testmysql);
            this.Controls.Add(this.configureFTP);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.getliteloaderversions);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ModpackVersionInput);
            this.Controls.Add(this.ModpackNameInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.missingInfoAction);
            this.Controls.Add(this.labelforgeversion);
            this.Controls.Add(this.labelmcversion);
            this.Controls.Add(this.ForgeBuild);
            this.Controls.Add(this.MCversion);
            this.Controls.Add(this.GetForgeVersions);
            this.Controls.Add(this.UploadToFTPServer);
            this.Controls.Add(this.TechnicDistributionLevel);
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
            this.Text = "Modpack Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnApplicationClosing);
            this.missingInfoAction.ResumeLayout(false);
            this.missingInfoAction.PerformLayout();
            this.TechnicDistributionLevel.ResumeLayout(false);
            this.TechnicDistributionLevel.PerformLayout();
            this.DistributionLevel.ResumeLayout(false);
            this.DistributionLevel.PerformLayout();
            this.SolderPackType.ResumeLayout(false);
            this.SolderPackType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox InputFolder;
        private Label label1;
        private Label label2;
        private TextBox OutputFolder;
        private Button InputDirectoryBrowse;
        private Button OutputDirectoryBrowse;
        private FolderBrowserDialog FolderBrowser;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
        private CheckBox IncludeConfigZip;
        private Button button3;
        private CheckBox CreateFTBPack;
        private Label label3;
        public Label ProgressLabel;
        private CheckBox CreateTechnicPack;
        private GroupBox SolderPackType;
        private RadioButton ZipPack;
        private RadioButton SolderPack;
        private GroupBox DistributionLevel;
        private RadioButton PublicFTBPack;
        private RadioButton PrivateFTBPack;
        private CheckBox CheckPermissions;
        private GroupBox TechnicDistributionLevel;
        private RadioButton TechnicPublicPermissions;
        private RadioButton TechnicPrivatePermissions;
        private CheckBox UploadToFTPServer;
        private CheckBox IncludeForgeVersion;
        private ListBox MCversion;
        private ListBox ForgeBuild;
        private Button GetForgeVersions;
        private Label labelmcversion;
        private Label labelforgeversion;
        private GroupBox missingInfoAction;
        private RadioButton missingInfoActionCreateList;
        private RadioButton missingInfoActionOnTheRun;
        private Label label4;
        private ComboBox ModpackNameInput;
        private TextBox ModpackVersionInput;
        private Label label5;
        private Button getliteloaderversions;
        private GroupBox groupBox1;
        private Button configureFTP;
        private Button testmysql;
        private CheckBox useSolder;
        private Button configureSolder;
        private CheckBox savesqlcommands;

        public SolderHelper()
        {
            //if (Debugger.IsAttached) { Properties.Settings.Default.Reset(); }

            Globalfunctions.PathSeperator = Globalfunctions.IsUnix() ? '/' : '\\';
            InitializeComponent();
            bool firstRun = true;
            try
            {
                firstRun = Convert.ToBoolean(_confighandler.GetConfig("FirstRun"));
            }
            catch (Exception)
            {
                firstRun = true;
            }
            if (firstRun)
            {
                MessageToUser m = new MessageToUser();
                Thread startingThread = new Thread(m.FirstTimeRun);
                startingThread.Start();
                getliteloaderversions_Click(null, null);
                _confighandler.SetConfig("InputDirectory", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods"));
                _confighandler.SetConfig("OutputDirectory", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "SolderHelper"));
                _confighandler.SetConfig("FirstRun", "false");

                #region Find MC versions

                MCversion.Items.Clear();

                _forgeSqlHelper.FindAllForgeVersion();
                List<String> mcversions = _forgeSqlHelper.GetMcVersions();
                foreach (String mcversion in mcversions)
                {
                    MCversion.Items.Add(mcversion);
                }

                #endregion
                ExcelReader.AddFtbPermissions();

            }
            #region Reload Interface
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "inputDirectories.json")))
            {
                _inputDirectories = JsonConvert.DeserializeObject<List<String>>(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "inputDirectories.json")));
                InputFolder.Items.Clear();
                InputFolder.Items.AddRange(_inputDirectories.ToArray());
            }
            try
            {
                InputFolder.Text = _confighandler.GetConfig("InputDirectory");
            }
            catch (Exception)
            {
                InputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/mods";
            }

            try
            {
                OutputFolder.Text = _confighandler.GetConfig("OutputDirectory");
            }
            catch (Exception)
            {
                OutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/SolderHelper";
            }

            try
            {
                CreateTechnicPack.Checked = Convert.ToBoolean(_confighandler.GetConfig("CreateTechnicSolderFiles"));
                SolderPackType.Visible = CreateTechnicPack.Checked;
            }
            catch (Exception)
            {
                CreateTechnicPack.Checked = false;
            }

            try
            {
                CreateFTBPack.Checked = Convert.ToBoolean(_confighandler.GetConfig("CreateFTBPack"));
            }
            catch (Exception)
            {
                CreateFTBPack.Checked = false;
            }

            Boolean createSolderHelper = true, createPrivateFtbPack = true, technicPrivatePermissionsLevel = true, includeForgeVersion = false, createTechnicConfigZip = true, checkTecnicPermissions = false, uploadToFTPServer = false, uses3 = false, createFtbPack = false;
            try
            {
                useSolder.Checked = Convert.ToBoolean(_confighandler.GetConfig("useSolder"));
            }
            catch (Exception)
            {
                useSolder.Checked = false;
            }
            if (useSolder.Checked)
            {
                configureSolder.Show();
            }
            else
            {
                configureSolder.Hide();
            }
            try
            {
                createSolderHelper = Convert.ToBoolean(_confighandler.GetConfig("CreateSolderPack"));
            }
            catch (Exception)
            {
            }
            try
            {
                createFtbPack = Convert.ToBoolean(_confighandler.GetConfig("CreateFTBPack"));
            }
            catch (Exception)
            {
                
            }
            try
            {
                createPrivateFtbPack = Convert.ToBoolean(_confighandler.GetConfig("CreatePrivateFTBPack"));
            }
            catch (Exception)
            {
            }
            try
            {
                technicPrivatePermissionsLevel = Convert.ToBoolean(_confighandler.GetConfig("TechnicPrivatePermissionsLevel"));
            }
            catch (Exception)
            {
            }
            try
            {
                includeForgeVersion = Convert.ToBoolean(_confighandler.GetConfig("IncludeForgeVersion"));
            }
            catch (Exception)
            {

            }
            try
            {
                createTechnicConfigZip = Convert.ToBoolean(_confighandler.GetConfig("CreateTechnicConfigZip"));
            }
            catch (Exception)
            {
            }
            try
            {
                checkTecnicPermissions = Convert.ToBoolean(_confighandler.GetConfig("CheckTecnicPermissions"));
            }
            catch (Exception)
            {
            }
            try
            {
                uploadToFTPServer = Convert.ToBoolean(_confighandler.GetConfig("UploadToFTPServer"));
            }
            catch
            {
            }
            try
            {
                uses3 = Convert.ToBoolean(_confighandler.GetConfig("useS3"));
            }
            catch
            {
            }

            if (uses3)
            {
                UseS3.Checked = true;
                ConfigureS3.Show();
            }
            else
            {
                UseS3.Checked = false;
                ConfigureS3.Hide();
            }
            if (createSolderHelper)
            {
                ZipPack.Checked = false;
                SolderPack.Checked = true;
            }
            else
            {
                SolderPack.Checked = false;
                ZipPack.Checked = true;
            }
            if (createFtbPack)
            {
                DistributionLevel.Show();
            }
            else
            {
                DistributionLevel.Hide();
            }
            if (createPrivateFtbPack)
            {
                PublicFTBPack.Checked = false;
                PrivateFTBPack.Checked = true;
            }
            else
            {
                PrivateFTBPack.Checked = false;
                PublicFTBPack.Checked = true;
            }
            if (technicPrivatePermissionsLevel)
            {
                TechnicPublicPermissions.Checked = false;
                TechnicPrivatePermissions.Checked = true;
            }
            else
            {
                TechnicPrivatePermissions.Checked = false;
                TechnicPublicPermissions.Checked = true;
            }
            UploadToFTPServer.Checked = uploadToFTPServer;
            if (UploadToFTPServer.Checked)
            {
                configureFTP.Show();
            }
            else
            {
                configureFTP.Hide();
            }
            IncludeForgeVersion.Checked = includeForgeVersion;
            if (includeForgeVersion)
            {
                MCversion.Show();
                labelmcversion.Show();
                labelforgeversion.Show();
                ForgeBuild.Show();
            }
            else
            {
                MCversion.Hide();
                labelmcversion.Hide();
                labelforgeversion.Hide();
                ForgeBuild.Hide();
            }
            IncludeConfigZip.Checked = createTechnicConfigZip;
            CheckPermissions.Checked = checkTecnicPermissions;
            if (checkTecnicPermissions && CreateTechnicPack.Checked)
            {
                TechnicDistributionLevel.Visible = true;
            }
            else
            {
                TechnicDistributionLevel.Visible = false;
            }

            if (SolderPack.Checked)
            {
                IncludeForgeVersion.Text = "Create Forge zip";
                IncludeConfigZip.Text = "Create Config zip";
            }
            else
            {
                IncludeForgeVersion.Text = "Include Forge in zip";
                IncludeConfigZip.Text = "Include Configs in zip";
            }
            List<String> minecraftversions = _forgeSqlHelper.GetMcVersions();
            foreach (String mcversion in minecraftversions)
                MCversion.Items.Add(mcversion);

            if (!CreateTechnicPack.Checked)
            {
                MCversion.Hide();
                ForgeBuild.Hide();
                labelmcversion.Hide();
                labelforgeversion.Hide();
            }

            #endregion

            _startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _startInfo.FileName = Globalfunctions.IsUnix() ? "unzip" : _sevenZipLocation;

            if (File.Exists(_modpacksJsonFile))
            {
                String modpackJson = "";
                using (StreamReader reader = new StreamReader(_modpacksJsonFile))
                {
                    modpackJson = reader.ReadToEnd();
                }
                _modpacks = JsonConvert.DeserializeObject<Modpacks>(modpackJson);
                foreach (String item in _modpacks.Modpack.Keys)
                {
                    ModpackNameInput.Items.Add(item);
                }
            }
            this.MinimumSize = new System.Drawing.Size(923, 527);
            Debug.WriteLine(this.Width);
            Debug.WriteLine(this.Height);
            Debug.WriteLine(groupBox1.Width);
        }

        private CheckBox UseS3;
        private Button ConfigureS3;
    }
}