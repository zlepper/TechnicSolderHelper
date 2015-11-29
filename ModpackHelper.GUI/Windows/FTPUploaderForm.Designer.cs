namespace ModpackHelper.GUI.Windows
{
    partial class FtpUploaderForm
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
            System.Windows.Forms.Label CurrentlyUploadingLabel;
            this.UploadProgressBar = new System.Windows.Forms.ProgressBar();
            this.UploadingFileNameLabel = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.FinishedButton = new System.Windows.Forms.Button();
            this.UploadingFileLabel = new System.Windows.Forms.Label();
            CurrentlyUploadingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CurrentlyUploadingLabel
            // 
            CurrentlyUploadingLabel.AutoSize = true;
            CurrentlyUploadingLabel.Location = new System.Drawing.Point(35, 31);
            CurrentlyUploadingLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            CurrentlyUploadingLabel.Name = "CurrentlyUploadingLabel";
            CurrentlyUploadingLabel.Size = new System.Drawing.Size(275, 32);
            CurrentlyUploadingLabel.TabIndex = 1;
            CurrentlyUploadingLabel.Text = "Currently Uploading:";
            // 
            // UploadProgressBar
            // 
            this.UploadProgressBar.Location = new System.Drawing.Point(32, 69);
            this.UploadProgressBar.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.UploadProgressBar.Name = "UploadProgressBar";
            this.UploadProgressBar.Size = new System.Drawing.Size(1021, 55);
            this.UploadProgressBar.TabIndex = 0;
            // 
            // UploadingFileNameLabel
            // 
            this.UploadingFileNameLabel.Location = new System.Drawing.Point(0, 0);
            this.UploadingFileNameLabel.Name = "UploadingFileNameLabel";
            this.UploadingFileNameLabel.Size = new System.Drawing.Size(100, 23);
            this.UploadingFileNameLabel.TabIndex = 6;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(32, 138);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(200, 55);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FinishedButton
            // 
            this.FinishedButton.Location = new System.Drawing.Point(853, 138);
            this.FinishedButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.FinishedButton.Name = "FinishedButton";
            this.FinishedButton.Size = new System.Drawing.Size(200, 55);
            this.FinishedButton.TabIndex = 4;
            this.FinishedButton.Text = "Finish";
            this.FinishedButton.UseVisualStyleBackColor = true;
            this.FinishedButton.Click += new System.EventHandler(this.FinishedButton_Click);
            // 
            // UploadingFileLabel
            // 
            this.UploadingFileLabel.AutoSize = true;
            this.UploadingFileLabel.Location = new System.Drawing.Point(366, 31);
            this.UploadingFileLabel.Name = "UploadingFileLabel";
            this.UploadingFileLabel.Size = new System.Drawing.Size(54, 32);
            this.UploadingFileLabel.TabIndex = 5;
            this.UploadingFileLabel.Text = "bla";
            // 
            // FtpUploaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 215);
            this.Controls.Add(this.UploadingFileLabel);
            this.Controls.Add(this.FinishedButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.UploadingFileNameLabel);
            this.Controls.Add(CurrentlyUploadingLabel);
            this.Controls.Add(this.UploadProgressBar);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "FtpUploaderForm";
            this.Text = "FTPUploader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar UploadProgressBar;
        private System.Windows.Forms.Label UploadingFileNameLabel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button FinishedButton;
        private System.Windows.Forms.Label UploadingFileLabel;
    }
}