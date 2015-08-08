namespace ModpackHelper.GUI.Windows
{
    partial class FTPUploaderForm
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.UploadingFileNameLabel = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.FinishedButton = new System.Windows.Forms.Button();
            CurrentlyUploadingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(383, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // CurrentlyUploadingLabel
            // 
            CurrentlyUploadingLabel.AutoSize = true;
            CurrentlyUploadingLabel.Location = new System.Drawing.Point(13, 13);
            CurrentlyUploadingLabel.Name = "CurrentlyUploadingLabel";
            CurrentlyUploadingLabel.Size = new System.Drawing.Size(102, 13);
            CurrentlyUploadingLabel.TabIndex = 1;
            CurrentlyUploadingLabel.Text = "Currently Uploading:";
            // 
            // UploadingFileNameLabel
            // 
            this.UploadingFileNameLabel.AutoSize = true;
            this.UploadingFileNameLabel.Location = new System.Drawing.Point(122, 13);
            this.UploadingFileNameLabel.Name = "UploadingFileNameLabel";
            this.UploadingFileNameLabel.Size = new System.Drawing.Size(0, 13);
            this.UploadingFileNameLabel.TabIndex = 2;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(12, 58);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // FinishedButton
            // 
            this.FinishedButton.Location = new System.Drawing.Point(320, 58);
            this.FinishedButton.Name = "FinishedButton";
            this.FinishedButton.Size = new System.Drawing.Size(75, 23);
            this.FinishedButton.TabIndex = 4;
            this.FinishedButton.Text = "Finish";
            this.FinishedButton.UseVisualStyleBackColor = true;
            // 
            // FTPUploaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 90);
            this.Controls.Add(this.FinishedButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.UploadingFileNameLabel);
            this.Controls.Add(CurrentlyUploadingLabel);
            this.Controls.Add(this.progressBar1);
            this.Name = "FTPUploaderForm";
            this.Text = "FTPUploader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label UploadingFileNameLabel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button FinishedButton;
    }
}