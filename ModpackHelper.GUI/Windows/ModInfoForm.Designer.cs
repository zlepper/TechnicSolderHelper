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
            this.ModDataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectModLabel
            // 
            this.SelectModLabel.AutoSize = true;
            this.SelectModLabel.Location = new System.Drawing.Point(13, 13);
            this.SelectModLabel.Name = "SelectModLabel";
            this.SelectModLabel.Size = new System.Drawing.Size(61, 13);
            this.SelectModLabel.TabIndex = 0;
            this.SelectModLabel.Text = "Select Mod";
            // 
            // ModSelectionList
            // 
            this.ModSelectionList.FormattingEnabled = true;
            this.ModSelectionList.Location = new System.Drawing.Point(12, 38);
            this.ModSelectionList.Name = "ModSelectionList";
            this.ModSelectionList.Size = new System.Drawing.Size(214, 433);
            this.ModSelectionList.TabIndex = 0;
            this.ModSelectionList.SelectedIndexChanged += new System.EventHandler(this.ModSelectionList_SelectedIndexChanged);
            // 
            // ShowDoneCheckBox
            // 
            this.ShowDoneCheckBox.AutoSize = true;
            this.ShowDoneCheckBox.Location = new System.Drawing.Point(13, 478);
            this.ShowDoneCheckBox.Name = "ShowDoneCheckBox";
            this.ShowDoneCheckBox.Size = new System.Drawing.Size(82, 17);
            this.ShowDoneCheckBox.TabIndex = 8;
            this.ShowDoneCheckBox.Text = "Show Done";
            this.ShowDoneCheckBox.UseVisualStyleBackColor = true;
            this.ShowDoneCheckBox.CheckedChanged += new System.EventHandler(this.ShowDoneCheckBox_CheckedChanged);
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(13, 502);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 7;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // GetPermissionButton
            // 
            this.GetPermissionButton.Location = new System.Drawing.Point(94, 502);
            this.GetPermissionButton.Name = "GetPermissionButton";
            this.GetPermissionButton.Size = new System.Drawing.Size(124, 23);
            this.GetPermissionButton.TabIndex = 6;
            this.GetPermissionButton.Text = "Get Permission";
            this.GetPermissionButton.UseVisualStyleBackColor = true;
            // 
            // SkipModCheckBox
            // 
            this.SkipModCheckBox.AutoSize = true;
            this.SkipModCheckBox.Location = new System.Drawing.Point(101, 479);
            this.SkipModCheckBox.Name = "SkipModCheckBox";
            this.SkipModCheckBox.Size = new System.Drawing.Size(71, 17);
            this.SkipModCheckBox.TabIndex = 5;
            this.SkipModCheckBox.Text = "Skip Mod";
            this.SkipModCheckBox.UseVisualStyleBackColor = true;
            this.SkipModCheckBox.CheckedChanged += new System.EventHandler(this.SkipModCheckBox_CheckedChanged);
            // 
            // ModDataGroupBox
            // 
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
            this.ModDataGroupBox.Location = new System.Drawing.Point(246, 13);
            this.ModDataGroupBox.Name = "ModDataGroupBox";
            this.ModDataGroupBox.Size = new System.Drawing.Size(317, 233);
            this.ModDataGroupBox.TabIndex = 6;
            this.ModDataGroupBox.TabStop = false;
            this.ModDataGroupBox.Text = "Mod Data";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(22, 179);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(54, 13);
            this.FileNameLabel.TabIndex = 1;
            this.FileNameLabel.Text = "File Name";
            // 
            // ModAuthorLabel
            // 
            this.ModAuthorLabel.AutoSize = true;
            this.ModAuthorLabel.Location = new System.Drawing.Point(22, 140);
            this.ModAuthorLabel.Name = "ModAuthorLabel";
            this.ModAuthorLabel.Size = new System.Drawing.Size(38, 13);
            this.ModAuthorLabel.TabIndex = 1;
            this.ModAuthorLabel.Text = "Author";
            // 
            // ModVersionLabel
            // 
            this.ModVersionLabel.AutoSize = true;
            this.ModVersionLabel.Location = new System.Drawing.Point(22, 101);
            this.ModVersionLabel.Name = "ModVersionLabel";
            this.ModVersionLabel.Size = new System.Drawing.Size(66, 13);
            this.ModVersionLabel.TabIndex = 1;
            this.ModVersionLabel.Text = "Mod Version";
            // 
            // ModIDLabel
            // 
            this.ModIDLabel.AutoSize = true;
            this.ModIDLabel.Location = new System.Drawing.Point(22, 62);
            this.ModIDLabel.Name = "ModIDLabel";
            this.ModIDLabel.Size = new System.Drawing.Size(42, 13);
            this.ModIDLabel.TabIndex = 1;
            this.ModIDLabel.Text = "Mod ID";
            // 
            // ModNameLabel
            // 
            this.ModNameLabel.AutoSize = true;
            this.ModNameLabel.Location = new System.Drawing.Point(22, 23);
            this.ModNameLabel.Name = "ModNameLabel";
            this.ModNameLabel.Size = new System.Drawing.Size(59, 13);
            this.ModNameLabel.TabIndex = 1;
            this.ModNameLabel.Text = "Mod Name";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(21, 195);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.ReadOnly = true;
            this.FileNameTextBox.Size = new System.Drawing.Size(274, 20);
            this.FileNameTextBox.TabIndex = 0;
            // 
            // ModAuthorTextBox
            // 
            this.ModAuthorTextBox.Location = new System.Drawing.Point(21, 156);
            this.ModAuthorTextBox.Name = "ModAuthorTextBox";
            this.ModAuthorTextBox.Size = new System.Drawing.Size(274, 20);
            this.ModAuthorTextBox.TabIndex = 4;
            this.ModAuthorTextBox.TextChanged += new System.EventHandler(this.ModAuthorTextBox_TextChanged);
            // 
            // ModVersionTextBox
            // 
            this.ModVersionTextBox.Location = new System.Drawing.Point(21, 117);
            this.ModVersionTextBox.Name = "ModVersionTextBox";
            this.ModVersionTextBox.Size = new System.Drawing.Size(274, 20);
            this.ModVersionTextBox.TabIndex = 3;
            this.ModVersionTextBox.TextChanged += new System.EventHandler(this.ModVersionTextBox_TextChanged);
            // 
            // ModIDTextBox
            // 
            this.ModIDTextBox.Location = new System.Drawing.Point(21, 78);
            this.ModIDTextBox.Name = "ModIDTextBox";
            this.ModIDTextBox.Size = new System.Drawing.Size(274, 20);
            this.ModIDTextBox.TabIndex = 2;
            this.ModIDTextBox.TextChanged += new System.EventHandler(this.ModIDTextBox_TextChanged);
            // 
            // ModNameTextBox
            // 
            this.ModNameTextBox.Location = new System.Drawing.Point(21, 39);
            this.ModNameTextBox.Name = "ModNameTextBox";
            this.ModNameTextBox.Size = new System.Drawing.Size(274, 20);
            this.ModNameTextBox.TabIndex = 1;
            this.ModNameTextBox.TextChanged += new System.EventHandler(this.ModNameTextBox_TextChanged);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(13, 531);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SkipAllButton
            // 
            this.SkipAllButton.Location = new System.Drawing.Point(94, 532);
            this.SkipAllButton.Name = "SkipAllButton";
            this.SkipAllButton.Size = new System.Drawing.Size(123, 23);
            this.SkipAllButton.TabIndex = 10;
            this.SkipAllButton.Text = "Skip all";
            this.SkipAllButton.UseVisualStyleBackColor = true;
            this.SkipAllButton.Click += new System.EventHandler(this.SkipAllButton_Click);
            // 
            // CheckOnlineButton
            // 
            this.CheckOnlineButton.Location = new System.Drawing.Point(94, 561);
            this.CheckOnlineButton.Name = "CheckOnlineButton";
            this.CheckOnlineButton.Size = new System.Drawing.Size(123, 23);
            this.CheckOnlineButton.TabIndex = 11;
            this.CheckOnlineButton.Text = "Check online";
            this.CheckOnlineButton.UseVisualStyleBackColor = true;
            this.CheckOnlineButton.Click += new System.EventHandler(this.CheckOnlineButton_Click);
            // 
            // ModInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 615);
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
            this.Name = "ModInfoForm";
            this.Text = "ModInfoForm";
            this.ModDataGroupBox.ResumeLayout(false);
            this.ModDataGroupBox.PerformLayout();
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
    }
}