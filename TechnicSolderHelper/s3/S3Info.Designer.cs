namespace TechnicSolderHelper.s3
{
    partial class S3Info
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
            this.serviceURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.accessKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.secretKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.buckets = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.newBucketName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serviceURL
            // 
            this.serviceURL.Location = new System.Drawing.Point(16, 29);
            this.serviceURL.Name = "serviceURL";
            this.serviceURL.Size = new System.Drawing.Size(156, 20);
            this.serviceURL.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Service URL";
            // 
            // accessKey
            // 
            this.accessKey.Location = new System.Drawing.Point(16, 68);
            this.accessKey.Name = "accessKey";
            this.accessKey.Size = new System.Drawing.Size(156, 20);
            this.accessKey.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Access Key";
            // 
            // secretKey
            // 
            this.secretKey.Location = new System.Drawing.Point(16, 107);
            this.secretKey.Name = "secretKey";
            this.secretKey.Size = new System.Drawing.Size(156, 20);
            this.secretKey.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Secret Key";
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(13, 134);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(45, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(64, 134);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(51, 23);
            this.test.TabIndex = 4;
            this.test.Text = "Test";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(122, 134);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(50, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // buckets
            // 
            this.buckets.FormattingEnabled = true;
            this.buckets.Location = new System.Drawing.Point(178, 29);
            this.buckets.Name = "buckets";
            this.buckets.Size = new System.Drawing.Size(159, 95);
            this.buckets.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Create new bucket";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // newBucketName
            // 
            this.newBucketName.Location = new System.Drawing.Point(178, 147);
            this.newBucketName.Name = "newBucketName";
            this.newBucketName.Size = new System.Drawing.Size(159, 20);
            this.newBucketName.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "New bucket name";
            // 
            // S3Info
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(349, 207);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.newBucketName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buckets);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.test);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.secretKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.accessKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serviceURL);
            this.Name = "S3Info";
            this.Text = "S3 Info";
            this.Load += new System.EventHandler(this.S3Info_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox serviceURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox accessKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox secretKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button test;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ListBox buckets;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox newBucketName;
        private System.Windows.Forms.Label label4;
    }
}