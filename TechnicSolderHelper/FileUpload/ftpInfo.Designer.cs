using System.ComponentModel;
using System.Windows.Forms;

namespace TechnicSolderHelper.FileUpload
{
    partial class FtpInfo
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
            this.Username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Host = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Acceptbutton = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(12, 27);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(179, 20);
            this.Username.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username";
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(12, 74);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(179, 20);
            this.Password.TabIndex = 1;
            this.Password.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // Host
            // 
            this.Host.Location = new System.Drawing.Point(12, 120);
            this.Host.Name = "Host";
            this.Host.Size = new System.Drawing.Size(179, 20);
            this.Host.TabIndex = 2;
            this.Host.Text = "ftp://";
            this.Host.TextChanged += new System.EventHandler(this.Host_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Host";
            // 
            // Acceptbutton
            // 
            this.Acceptbutton.Location = new System.Drawing.Point(12, 146);
            this.Acceptbutton.Name = "Acceptbutton";
            this.Acceptbutton.Size = new System.Drawing.Size(52, 26);
            this.Acceptbutton.TabIndex = 3;
            this.Acceptbutton.Text = "Accept";
            this.Acceptbutton.UseVisualStyleBackColor = true;
            this.Acceptbutton.Click += new System.EventHandler(this.Acceptbutton_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(70, 146);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(55, 26);
            this.test.TabIndex = 4;
            this.test.Text = "test";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 26);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FtpInfo
            // 
            this.AcceptButton = this.Acceptbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 184);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.test);
            this.Controls.Add(this.Acceptbutton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Host);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Username);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FtpInfo";
            this.Text = "ftpInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox Username;
        private Label label1;
        private TextBox Password;
        private Label label2;
        private TextBox Host;
        private Label label3;
        private Button Acceptbutton;
        private Button test;
        private Button button1;
    }
}