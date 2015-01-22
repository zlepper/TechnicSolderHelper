namespace TechnicSolderHelper
{
    partial class Modinfo
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
            this.modlist = new System.Windows.Forms.ListBox();
            this.dataBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.textBoxModVersion = new System.Windows.Forms.TextBox();
            this.textBoxModID = new System.Windows.Forms.TextBox();
            this.textBoxModName = new System.Windows.Forms.TextBox();
            this.permissionBox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.permissionUnknown = new TechnicSolderHelper.ReadOnlyRadioButton();
            this.permissionRequest = new TechnicSolderHelper.ReadOnlyRadioButton();
            this.permissionNotify = new TechnicSolderHelper.ReadOnlyRadioButton();
            this.permissionClosed = new TechnicSolderHelper.ReadOnlyRadioButton();
            this.permissionFTBExclusive = new TechnicSolderHelper.ReadOnlyRadioButton();
            this.permissionOpen = new TechnicSolderHelper.ReadOnlyRadioButton();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dataBox.SuspendLayout();
            this.permissionBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // modlist
            // 
            this.modlist.FormattingEnabled = true;
            this.modlist.Location = new System.Drawing.Point(12, 30);
            this.modlist.Name = "modlist";
            this.modlist.Size = new System.Drawing.Size(163, 485);
            this.modlist.Sorted = true;
            this.modlist.TabIndex = 0;
            this.modlist.SelectedIndexChanged += new System.EventHandler(this.modlist_SelectedIndexChanged);
            // 
            // dataBox
            // 
            this.dataBox.Controls.Add(this.label8);
            this.dataBox.Controls.Add(this.label4);
            this.dataBox.Controls.Add(this.label3);
            this.dataBox.Controls.Add(this.label2);
            this.dataBox.Controls.Add(this.label1);
            this.dataBox.Controls.Add(this.textBox4);
            this.dataBox.Controls.Add(this.textBoxAuthor);
            this.dataBox.Controls.Add(this.textBoxModVersion);
            this.dataBox.Controls.Add(this.textBoxModID);
            this.dataBox.Controls.Add(this.textBoxModName);
            this.dataBox.Location = new System.Drawing.Point(181, 13);
            this.dataBox.Name = "dataBox";
            this.dataBox.Size = new System.Drawing.Size(253, 251);
            this.dataBox.TabIndex = 1;
            this.dataBox.TabStop = false;
            this.dataBox.Text = "Data";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Author";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Mod Version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ModID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mod Name";
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Location = new System.Drawing.Point(24, 167);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(205, 20);
            this.textBoxAuthor.TabIndex = 0;
            // 
            // textBoxModVersion
            // 
            this.textBoxModVersion.Location = new System.Drawing.Point(24, 124);
            this.textBoxModVersion.Name = "textBoxModVersion";
            this.textBoxModVersion.Size = new System.Drawing.Size(205, 20);
            this.textBoxModVersion.TabIndex = 0;
            // 
            // textBoxModID
            // 
            this.textBoxModID.Location = new System.Drawing.Point(24, 80);
            this.textBoxModID.Name = "textBoxModID";
            this.textBoxModID.Size = new System.Drawing.Size(205, 20);
            this.textBoxModID.TabIndex = 0;
            // 
            // textBoxModName
            // 
            this.textBoxModName.Location = new System.Drawing.Point(24, 36);
            this.textBoxModName.Name = "textBoxModName";
            this.textBoxModName.Size = new System.Drawing.Size(205, 20);
            this.textBoxModName.TabIndex = 0;
            // 
            // permissionBox
            // 
            this.permissionBox.Controls.Add(this.label7);
            this.permissionBox.Controls.Add(this.label6);
            this.permissionBox.Controls.Add(this.label5);
            this.permissionBox.Controls.Add(this.permissionUnknown);
            this.permissionBox.Controls.Add(this.permissionRequest);
            this.permissionBox.Controls.Add(this.permissionNotify);
            this.permissionBox.Controls.Add(this.permissionClosed);
            this.permissionBox.Controls.Add(this.textBox3);
            this.permissionBox.Controls.Add(this.textBox2);
            this.permissionBox.Controls.Add(this.textBox1);
            this.permissionBox.Controls.Add(this.permissionFTBExclusive);
            this.permissionBox.Controls.Add(this.permissionOpen);
            this.permissionBox.Location = new System.Drawing.Point(182, 270);
            this.permissionBox.Name = "permissionBox";
            this.permissionBox.Size = new System.Drawing.Size(252, 245);
            this.permissionBox.TabIndex = 2;
            this.permissionBox.TabStop = false;
            this.permissionBox.Text = "Permissions";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "License Link";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Mod Link";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Permission Link";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(23, 201);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(205, 20);
            this.textBox3.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(23, 154);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(205, 20);
            this.textBox2.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(23, 112);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(205, 20);
            this.textBox1.TabIndex = 0;
            // 
            // permissionUnknown
            // 
            this.permissionUnknown.AutoSize = true;
            this.permissionUnknown.Location = new System.Drawing.Point(161, 66);
            this.permissionUnknown.Name = "permissionUnknown";
            this.permissionUnknown.ReadOnly = true;
            this.permissionUnknown.Size = new System.Drawing.Size(71, 17);
            this.permissionUnknown.TabIndex = 0;
            this.permissionUnknown.TabStop = true;
            this.permissionUnknown.Text = "Unknown";
            this.permissionUnknown.UseVisualStyleBackColor = true;
            // 
            // permissionRequest
            // 
            this.permissionRequest.AutoSize = true;
            this.permissionRequest.Location = new System.Drawing.Point(161, 43);
            this.permissionRequest.Name = "permissionRequest";
            this.permissionRequest.ReadOnly = true;
            this.permissionRequest.Size = new System.Drawing.Size(65, 17);
            this.permissionRequest.TabIndex = 0;
            this.permissionRequest.TabStop = true;
            this.permissionRequest.Text = "Request";
            this.permissionRequest.UseVisualStyleBackColor = true;
            // 
            // permissionNotify
            // 
            this.permissionNotify.AutoSize = true;
            this.permissionNotify.Location = new System.Drawing.Point(23, 66);
            this.permissionNotify.Name = "permissionNotify";
            this.permissionNotify.ReadOnly = true;
            this.permissionNotify.Size = new System.Drawing.Size(52, 17);
            this.permissionNotify.TabIndex = 0;
            this.permissionNotify.TabStop = true;
            this.permissionNotify.Text = "Notify";
            this.permissionNotify.UseVisualStyleBackColor = true;
            // 
            // permissionClosed
            // 
            this.permissionClosed.AutoSize = true;
            this.permissionClosed.Location = new System.Drawing.Point(161, 20);
            this.permissionClosed.Name = "permissionClosed";
            this.permissionClosed.ReadOnly = true;
            this.permissionClosed.Size = new System.Drawing.Size(57, 17);
            this.permissionClosed.TabIndex = 0;
            this.permissionClosed.TabStop = true;
            this.permissionClosed.Text = "Closed";
            this.permissionClosed.UseVisualStyleBackColor = true;
            // 
            // permissionFTBExclusive
            // 
            this.permissionFTBExclusive.AutoSize = true;
            this.permissionFTBExclusive.Location = new System.Drawing.Point(23, 43);
            this.permissionFTBExclusive.Name = "permissionFTBExclusive";
            this.permissionFTBExclusive.ReadOnly = true;
            this.permissionFTBExclusive.Size = new System.Drawing.Size(93, 17);
            this.permissionFTBExclusive.TabIndex = 0;
            this.permissionFTBExclusive.TabStop = true;
            this.permissionFTBExclusive.Text = "FTB Exclusive";
            this.permissionFTBExclusive.UseVisualStyleBackColor = true;
            // 
            // permissionOpen
            // 
            this.permissionOpen.AutoSize = true;
            this.permissionOpen.Location = new System.Drawing.Point(23, 20);
            this.permissionOpen.Name = "permissionOpen";
            this.permissionOpen.ReadOnly = true;
            this.permissionOpen.Size = new System.Drawing.Size(51, 17);
            this.permissionOpen.TabIndex = 0;
            this.permissionOpen.TabStop = true;
            this.permissionOpen.Text = "Open";
            this.permissionOpen.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(24, 211);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(205, 20);
            this.textBox4.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "File Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Select Mod";
            // 
            // Modinfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 527);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.permissionBox);
            this.Controls.Add(this.dataBox);
            this.Controls.Add(this.modlist);
            this.Name = "Modinfo";
            this.Text = "modinfo";
            this.dataBox.ResumeLayout(false);
            this.dataBox.PerformLayout();
            this.permissionBox.ResumeLayout(false);
            this.permissionBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox modlist;
        private System.Windows.Forms.GroupBox dataBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxModVersion;
        private System.Windows.Forms.TextBox textBoxModID;
        private System.Windows.Forms.TextBox textBoxModName;
        private System.Windows.Forms.GroupBox permissionBox;
        private ReadOnlyRadioButton permissionUnknown;
        private ReadOnlyRadioButton permissionRequest;
        private ReadOnlyRadioButton permissionNotify;
        private ReadOnlyRadioButton permissionClosed;
        private ReadOnlyRadioButton permissionFTBExclusive;
        private ReadOnlyRadioButton permissionOpen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAuthor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label9;
    }
}