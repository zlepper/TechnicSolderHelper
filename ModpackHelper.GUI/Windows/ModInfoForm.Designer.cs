namespace ModpackHelper.GUI.Windows
{
    partial class ModInfoForm
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
            System.Windows.Forms.Label technicLinkToPermissionListingLabel;
            System.Windows.Forms.Label technicLinkToProofOfPermissionsLabel;
            System.Windows.Forms.Label ftbLinkToProofOfPermissionsLabel;
            System.Windows.Forms.Label ftbLinkToPermissionListingLabel;
            this.SelectModLabel = new System.Windows.Forms.Label();
            this.ModSelectionList = new System.Windows.Forms.ListBox();
            this.ShowDoneCheckBox = new System.Windows.Forms.CheckBox();
            this.DoneButton = new System.Windows.Forms.Button();
            this.GetPermissionButton = new System.Windows.Forms.Button();
            this.SkipModCheckBox = new System.Windows.Forms.CheckBox();
            this.ModDataGroupBox = new System.Windows.Forms.GroupBox();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.ModAuthorLabel = new System.Windows.Forms.Label();
            this.ModVersionLabel = new System.Windows.Forms.Label();
            this.ModIDLabel = new System.Windows.Forms.Label();
            this.ModNameLabel = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.ModAuthorTextBox = new System.Windows.Forms.TextBox();
            this.ModVersionTextBox = new System.Windows.Forms.TextBox();
            this.ModIDTextBox = new System.Windows.Forms.TextBox();
            this.ModNameTextBox = new System.Windows.Forms.TextBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SkipAllButton = new System.Windows.Forms.Button();
            this.CheckOnlineButton = new System.Windows.Forms.Button();
            this.technicPermissionsGroupBox = new System.Windows.Forms.GroupBox();
            this.technicPermissionTextLabel = new System.Windows.Forms.Label();
            this.technicLinkToProofOfPermissionsTextBox = new System.Windows.Forms.TextBox();
            this.technicLinkToPermissionListingTextBox = new System.Windows.Forms.TextBox();
            this.ftbPermissionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ftbPermissionTextLabel = new System.Windows.Forms.Label();
            this.ftbLinkToProofOfPermissionTextBox = new System.Windows.Forms.TextBox();
            this.ftbLinkToPermissionListingTextBox = new System.Windows.Forms.TextBox();
            technicLinkToPermissionListingLabel = new System.Windows.Forms.Label();
            technicLinkToProofOfPermissionsLabel = new System.Windows.Forms.Label();
            ftbLinkToProofOfPermissionsLabel = new System.Windows.Forms.Label();
            ftbLinkToPermissionListingLabel = new System.Windows.Forms.Label();
            this.ModDataGroupBox.SuspendLayout();
            this.technicPermissionsGroupBox.SuspendLayout();
            this.ftbPermissionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // technicLinkToPermissionListingLabel
            // 
            technicLinkToPermissionListingLabel.AutoSize = true;
            technicLinkToPermissionListingLabel.Location = new System.Drawing.Point(6, 99);
            technicLinkToPermissionListingLabel.Name = "technicLinkToPermissionListingLabel";
            technicLinkToPermissionListingLabel.Size = new System.Drawing.Size(180, 20);
            technicLinkToPermissionListingLabel.TabIndex = 1;
            technicLinkToPermissionListingLabel.Text = "Link to permission listing";
            // 
            // technicLinkToProofOfPermissionsLabel
            // 
            technicLinkToProofOfPermissionsLabel.AutoSize = true;
            technicLinkToProofOfPermissionsLabel.Location = new System.Drawing.Point(6, 162);
            technicLinkToProofOfPermissionsLabel.Name = "technicLinkToProofOfPermissionsLabel";
            technicLinkToProofOfPermissionsLabel.Size = new System.Drawing.Size(203, 20);
            technicLinkToProofOfPermissionsLabel.TabIndex = 1;
            technicLinkToProofOfPermissionsLabel.Text = "Link to proof of permissions";
            // 
            // ftbLinkToProofOfPermissionsLabel
            // 
            ftbLinkToProofOfPermissionsLabel.AutoSize = true;
            ftbLinkToProofOfPermissionsLabel.Location = new System.Drawing.Point(6, 162);
            ftbLinkToProofOfPermissionsLabel.Name = "ftbLinkToProofOfPermissionsLabel";
            ftbLinkToProofOfPermissionsLabel.Size = new System.Drawing.Size(203, 20);
            ftbLinkToProofOfPermissionsLabel.TabIndex = 1;
            ftbLinkToProofOfPermissionsLabel.Text = "Link to proof of permissions";
            // 
            // ftbLinkToPermissionListingLabel
            // 
            ftbLinkToPermissionListingLabel.AutoSize = true;
            ftbLinkToPermissionListingLabel.Location = new System.Drawing.Point(6, 99);
            ftbLinkToPermissionListingLabel.Name = "ftbLinkToPermissionListingLabel";
            ftbLinkToPermissionListingLabel.Size = new System.Drawing.Size(180, 20);
            ftbLinkToPermissionListingLabel.TabIndex = 1;
            ftbLinkToPermissionListingLabel.Text = "Link to permission listing";
            // 
            // SelectModLabel
            // 
            this.SelectModLabel.AutoSize = true;
            this.SelectModLabel.Location = new System.Drawing.Point(20, 20);
            this.SelectModLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SelectModLabel.Name = "SelectModLabel";
            this.SelectModLabel.Size = new System.Drawing.Size(89, 20);
            this.SelectModLabel.TabIndex = 0;
            this.SelectModLabel.Text = "Select Mod";
            // 
            // ModSelectionList
            // 
            this.ModSelectionList.FormattingEnabled = true;
            this.ModSelectionList.ItemHeight = 20;
            this.ModSelectionList.Location = new System.Drawing.Point(18, 58);
            this.ModSelectionList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModSelectionList.Name = "ModSelectionList";
            this.ModSelectionList.Size = new System.Drawing.Size(319, 664);
            this.ModSelectionList.TabIndex = 0;
            this.ModSelectionList.SelectedIndexChanged += new System.EventHandler(this.ModSelectionList_SelectedIndexChanged);
            // 
            // ShowDoneCheckBox
            // 
            this.ShowDoneCheckBox.AutoSize = true;
            this.ShowDoneCheckBox.Location = new System.Drawing.Point(20, 735);
            this.ShowDoneCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ShowDoneCheckBox.Name = "ShowDoneCheckBox";
            this.ShowDoneCheckBox.Size = new System.Drawing.Size(118, 24);
            this.ShowDoneCheckBox.TabIndex = 8;
            this.ShowDoneCheckBox.Text = "Show Done";
            this.ShowDoneCheckBox.UseVisualStyleBackColor = true;
            this.ShowDoneCheckBox.CheckedChanged += new System.EventHandler(this.ShowDoneCheckBox_CheckedChanged);
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(20, 772);
            this.DoneButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(112, 35);
            this.DoneButton.TabIndex = 7;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // GetPermissionButton
            // 
            this.GetPermissionButton.Location = new System.Drawing.Point(141, 772);
            this.GetPermissionButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GetPermissionButton.Name = "GetPermissionButton";
            this.GetPermissionButton.Size = new System.Drawing.Size(186, 35);
            this.GetPermissionButton.TabIndex = 6;
            this.GetPermissionButton.Text = "Get Permission";
            this.GetPermissionButton.UseVisualStyleBackColor = true;
            this.GetPermissionButton.Click += new System.EventHandler(this.GetPermissionButton_Click);
            // 
            // SkipModCheckBox
            // 
            this.SkipModCheckBox.AutoSize = true;
            this.SkipModCheckBox.Location = new System.Drawing.Point(152, 737);
            this.SkipModCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SkipModCheckBox.Name = "SkipModCheckBox";
            this.SkipModCheckBox.Size = new System.Drawing.Size(101, 24);
            this.SkipModCheckBox.TabIndex = 5;
            this.SkipModCheckBox.Text = "Skip Mod";
            this.SkipModCheckBox.UseVisualStyleBackColor = true;
            this.SkipModCheckBox.CheckedChanged += new System.EventHandler(this.SkipModCheckBox_CheckedChanged);
            // 
            // ModDataGroupBox
            // 
            this.ModDataGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModDataGroupBox.Controls.Add(this.FileNameLabel);
            this.ModDataGroupBox.Controls.Add(this.ModAuthorLabel);
            this.ModDataGroupBox.Controls.Add(this.ModVersionLabel);
            this.ModDataGroupBox.Controls.Add(this.ModIDLabel);
            this.ModDataGroupBox.Controls.Add(this.ModNameLabel);
            this.ModDataGroupBox.Controls.Add(this.FileNameTextBox);
            this.ModDataGroupBox.Controls.Add(this.ModAuthorTextBox);
            this.ModDataGroupBox.Controls.Add(this.ModVersionTextBox);
            this.ModDataGroupBox.Controls.Add(this.ModIDTextBox);
            this.ModDataGroupBox.Controls.Add(this.ModNameTextBox);
            this.ModDataGroupBox.Location = new System.Drawing.Point(369, 20);
            this.ModDataGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModDataGroupBox.Name = "ModDataGroupBox";
            this.ModDataGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModDataGroupBox.Size = new System.Drawing.Size(544, 358);
            this.ModDataGroupBox.TabIndex = 6;
            this.ModDataGroupBox.TabStop = false;
            this.ModDataGroupBox.Text = "Mod Data";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(8, 275);
            this.FileNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(80, 20);
            this.FileNameLabel.TabIndex = 1;
            this.FileNameLabel.Text = "File Name";
            // 
            // ModAuthorLabel
            // 
            this.ModAuthorLabel.AutoSize = true;
            this.ModAuthorLabel.Location = new System.Drawing.Point(8, 215);
            this.ModAuthorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ModAuthorLabel.Name = "ModAuthorLabel";
            this.ModAuthorLabel.Size = new System.Drawing.Size(57, 20);
            this.ModAuthorLabel.TabIndex = 1;
            this.ModAuthorLabel.Text = "Author";
            // 
            // ModVersionLabel
            // 
            this.ModVersionLabel.AutoSize = true;
            this.ModVersionLabel.Location = new System.Drawing.Point(8, 155);
            this.ModVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ModVersionLabel.Name = "ModVersionLabel";
            this.ModVersionLabel.Size = new System.Drawing.Size(98, 20);
            this.ModVersionLabel.TabIndex = 1;
            this.ModVersionLabel.Text = "Mod Version";
            // 
            // ModIDLabel
            // 
            this.ModIDLabel.AutoSize = true;
            this.ModIDLabel.Location = new System.Drawing.Point(8, 95);
            this.ModIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ModIDLabel.Name = "ModIDLabel";
            this.ModIDLabel.Size = new System.Drawing.Size(61, 20);
            this.ModIDLabel.TabIndex = 1;
            this.ModIDLabel.Text = "Mod ID";
            // 
            // ModNameLabel
            // 
            this.ModNameLabel.AutoSize = true;
            this.ModNameLabel.Location = new System.Drawing.Point(8, 35);
            this.ModNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ModNameLabel.Name = "ModNameLabel";
            this.ModNameLabel.Size = new System.Drawing.Size(86, 20);
            this.ModNameLabel.TabIndex = 1;
            this.ModNameLabel.Text = "Mod Name";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Location = new System.Drawing.Point(7, 300);
            this.FileNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.ReadOnly = true;
            this.FileNameTextBox.Size = new System.Drawing.Size(531, 26);
            this.FileNameTextBox.TabIndex = 0;
            // 
            // ModAuthorTextBox
            // 
            this.ModAuthorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModAuthorTextBox.Location = new System.Drawing.Point(7, 240);
            this.ModAuthorTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModAuthorTextBox.Name = "ModAuthorTextBox";
            this.ModAuthorTextBox.Size = new System.Drawing.Size(531, 26);
            this.ModAuthorTextBox.TabIndex = 4;
            this.ModAuthorTextBox.TextChanged += new System.EventHandler(this.ModAuthorTextBox_TextChanged);
            // 
            // ModVersionTextBox
            // 
            this.ModVersionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModVersionTextBox.Location = new System.Drawing.Point(7, 180);
            this.ModVersionTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModVersionTextBox.Name = "ModVersionTextBox";
            this.ModVersionTextBox.Size = new System.Drawing.Size(531, 26);
            this.ModVersionTextBox.TabIndex = 3;
            this.ModVersionTextBox.TextChanged += new System.EventHandler(this.ModVersionTextBox_TextChanged);
            // 
            // ModIDTextBox
            // 
            this.ModIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModIDTextBox.Location = new System.Drawing.Point(7, 120);
            this.ModIDTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModIDTextBox.Name = "ModIDTextBox";
            this.ModIDTextBox.Size = new System.Drawing.Size(531, 26);
            this.ModIDTextBox.TabIndex = 2;
            this.ModIDTextBox.TextChanged += new System.EventHandler(this.ModIDTextBox_TextChanged);
            // 
            // ModNameTextBox
            // 
            this.ModNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModNameTextBox.Location = new System.Drawing.Point(7, 60);
            this.ModNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModNameTextBox.Name = "ModNameTextBox";
            this.ModNameTextBox.Size = new System.Drawing.Size(531, 26);
            this.ModNameTextBox.TabIndex = 1;
            this.ModNameTextBox.TextChanged += new System.EventHandler(this.ModNameTextBox_TextChanged);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(20, 817);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(112, 35);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SkipAllButton
            // 
            this.SkipAllButton.Location = new System.Drawing.Point(141, 818);
            this.SkipAllButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SkipAllButton.Name = "SkipAllButton";
            this.SkipAllButton.Size = new System.Drawing.Size(184, 35);
            this.SkipAllButton.TabIndex = 10;
            this.SkipAllButton.Text = "Skip all";
            this.SkipAllButton.UseVisualStyleBackColor = true;
            this.SkipAllButton.Click += new System.EventHandler(this.SkipAllButton_Click);
            // 
            // CheckOnlineButton
            // 
            this.CheckOnlineButton.Location = new System.Drawing.Point(141, 863);
            this.CheckOnlineButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CheckOnlineButton.Name = "CheckOnlineButton";
            this.CheckOnlineButton.Size = new System.Drawing.Size(184, 35);
            this.CheckOnlineButton.TabIndex = 11;
            this.CheckOnlineButton.Text = "Check online";
            this.CheckOnlineButton.UseVisualStyleBackColor = true;
            this.CheckOnlineButton.Click += new System.EventHandler(this.CheckOnlineButton_Click);
            // 
            // technicPermissionsGroupBox
            // 
            this.technicPermissionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.technicPermissionsGroupBox.Controls.Add(this.technicPermissionTextLabel);
            this.technicPermissionsGroupBox.Controls.Add(technicLinkToProofOfPermissionsLabel);
            this.technicPermissionsGroupBox.Controls.Add(technicLinkToPermissionListingLabel);
            this.technicPermissionsGroupBox.Controls.Add(this.technicLinkToProofOfPermissionsTextBox);
            this.technicPermissionsGroupBox.Controls.Add(this.technicLinkToPermissionListingTextBox);
            this.technicPermissionsGroupBox.Location = new System.Drawing.Point(370, 386);
            this.technicPermissionsGroupBox.Name = "technicPermissionsGroupBox";
            this.technicPermissionsGroupBox.Size = new System.Drawing.Size(543, 220);
            this.technicPermissionsGroupBox.TabIndex = 12;
            this.technicPermissionsGroupBox.TabStop = false;
            this.technicPermissionsGroupBox.Text = "Technic Permissions";
            // 
            // technicPermissionTextLabel
            // 
            this.technicPermissionTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.technicPermissionTextLabel.Location = new System.Drawing.Point(6, 26);
            this.technicPermissionTextLabel.Name = "technicPermissionTextLabel";
            this.technicPermissionTextLabel.Size = new System.Drawing.Size(531, 73);
            this.technicPermissionTextLabel.TabIndex = 2;
            this.technicPermissionTextLabel.Text = "Text about permissions will appear here.";
            // 
            // technicLinkToProofOfPermissionsTextBox
            // 
            this.technicLinkToProofOfPermissionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.technicLinkToProofOfPermissionsTextBox.Location = new System.Drawing.Point(6, 188);
            this.technicLinkToProofOfPermissionsTextBox.Name = "technicLinkToProofOfPermissionsTextBox";
            this.technicLinkToProofOfPermissionsTextBox.Size = new System.Drawing.Size(531, 26);
            this.technicLinkToProofOfPermissionsTextBox.TabIndex = 0;
            this.technicLinkToProofOfPermissionsTextBox.TextChanged += new System.EventHandler(this.technicLinkToProofOfPermissionsTextBox_TextChanged);
            // 
            // technicLinkToPermissionListingTextBox
            // 
            this.technicLinkToPermissionListingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.technicLinkToPermissionListingTextBox.Location = new System.Drawing.Point(6, 125);
            this.technicLinkToPermissionListingTextBox.Name = "technicLinkToPermissionListingTextBox";
            this.technicLinkToPermissionListingTextBox.Size = new System.Drawing.Size(531, 26);
            this.technicLinkToPermissionListingTextBox.TabIndex = 0;
            this.technicLinkToPermissionListingTextBox.TextChanged += new System.EventHandler(this.technicLinkToPermissionListingTextBox_TextChanged);
            // 
            // ftbPermissionsGroupBox
            // 
            this.ftbPermissionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ftbPermissionsGroupBox.Controls.Add(this.ftbPermissionTextLabel);
            this.ftbPermissionsGroupBox.Controls.Add(ftbLinkToProofOfPermissionsLabel);
            this.ftbPermissionsGroupBox.Controls.Add(ftbLinkToPermissionListingLabel);
            this.ftbPermissionsGroupBox.Controls.Add(this.ftbLinkToProofOfPermissionTextBox);
            this.ftbPermissionsGroupBox.Controls.Add(this.ftbLinkToPermissionListingTextBox);
            this.ftbPermissionsGroupBox.Location = new System.Drawing.Point(370, 612);
            this.ftbPermissionsGroupBox.Name = "ftbPermissionsGroupBox";
            this.ftbPermissionsGroupBox.Size = new System.Drawing.Size(543, 220);
            this.ftbPermissionsGroupBox.TabIndex = 12;
            this.ftbPermissionsGroupBox.TabStop = false;
            this.ftbPermissionsGroupBox.Text = "Feed the Beast Permissions";
            // 
            // ftbPermissionTextLabel
            // 
            this.ftbPermissionTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ftbPermissionTextLabel.Location = new System.Drawing.Point(6, 22);
            this.ftbPermissionTextLabel.Name = "ftbPermissionTextLabel";
            this.ftbPermissionTextLabel.Size = new System.Drawing.Size(531, 73);
            this.ftbPermissionTextLabel.TabIndex = 3;
            this.ftbPermissionTextLabel.Text = "Text about permissions will appear here.";
            // 
            // ftbLinkToProofOfPermissionTextBox
            // 
            this.ftbLinkToProofOfPermissionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ftbLinkToProofOfPermissionTextBox.Location = new System.Drawing.Point(6, 188);
            this.ftbLinkToProofOfPermissionTextBox.Name = "ftbLinkToProofOfPermissionTextBox";
            this.ftbLinkToProofOfPermissionTextBox.Size = new System.Drawing.Size(531, 26);
            this.ftbLinkToProofOfPermissionTextBox.TabIndex = 0;
            this.ftbLinkToProofOfPermissionTextBox.TextChanged += new System.EventHandler(this.ftbLinkToProofOfPermissionTextBox_TextChanged);
            // 
            // ftbLinkToPermissionListingTextBox
            // 
            this.ftbLinkToPermissionListingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ftbLinkToPermissionListingTextBox.Location = new System.Drawing.Point(6, 125);
            this.ftbLinkToPermissionListingTextBox.Name = "ftbLinkToPermissionListingTextBox";
            this.ftbLinkToPermissionListingTextBox.Size = new System.Drawing.Size(531, 26);
            this.ftbLinkToPermissionListingTextBox.TabIndex = 0;
            this.ftbLinkToPermissionListingTextBox.TextChanged += new System.EventHandler(this.ftbLinkToPermissionListingTextBox_TextChanged);
            // 
            // ModInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 946);
            this.Controls.Add(this.ftbPermissionsGroupBox);
            this.Controls.Add(this.technicPermissionsGroupBox);
            this.Controls.Add(this.CheckOnlineButton);
            this.Controls.Add(this.SkipAllButton);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ModDataGroupBox);
            this.Controls.Add(this.SkipModCheckBox);
            this.Controls.Add(this.GetPermissionButton);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.ShowDoneCheckBox);
            this.Controls.Add(this.ModSelectionList);
            this.Controls.Add(this.SelectModLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModInfoForm";
            this.Text = "ModInfoForm";
            this.ModDataGroupBox.ResumeLayout(false);
            this.ModDataGroupBox.PerformLayout();
            this.technicPermissionsGroupBox.ResumeLayout(false);
            this.technicPermissionsGroupBox.PerformLayout();
            this.ftbPermissionsGroupBox.ResumeLayout(false);
            this.ftbPermissionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SelectModLabel;
        private System.Windows.Forms.ListBox ModSelectionList;
        private System.Windows.Forms.CheckBox ShowDoneCheckBox;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button GetPermissionButton;
        private System.Windows.Forms.CheckBox SkipModCheckBox;
        private System.Windows.Forms.GroupBox ModDataGroupBox;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.Label ModAuthorLabel;
        private System.Windows.Forms.Label ModVersionLabel;
        private System.Windows.Forms.Label ModIDLabel;
        private System.Windows.Forms.Label ModNameLabel;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.TextBox ModAuthorTextBox;
        private System.Windows.Forms.TextBox ModVersionTextBox;
        private System.Windows.Forms.TextBox ModIDTextBox;
        private System.Windows.Forms.TextBox ModNameTextBox;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SkipAllButton;
        private System.Windows.Forms.Button CheckOnlineButton;
        private System.Windows.Forms.GroupBox technicPermissionsGroupBox;
        private System.Windows.Forms.TextBox technicLinkToPermissionListingTextBox;
        private System.Windows.Forms.TextBox technicLinkToProofOfPermissionsTextBox;
        private System.Windows.Forms.GroupBox ftbPermissionsGroupBox;
        private System.Windows.Forms.TextBox ftbLinkToProofOfPermissionTextBox;
        private System.Windows.Forms.TextBox ftbLinkToPermissionListingTextBox;
        private System.Windows.Forms.Label technicPermissionTextLabel;
        private System.Windows.Forms.Label ftbPermissionTextLabel;
    }
}