namespace TechnicSolderHelper.FileUpload
{
    partial class UploadProgression
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
            this.FileProgress = new System.Windows.Forms.ProgressBar();
            this.TotalProgress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CurrentFileName = new System.Windows.Forms.Label();
            this.ProgressPercentage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FileProgress
            // 
            this.FileProgress.Location = new System.Drawing.Point(12, 72);
            this.FileProgress.Name = "FileProgress";
            this.FileProgress.Size = new System.Drawing.Size(260, 23);
            this.FileProgress.TabIndex = 0;
            // 
            // TotalProgress
            // 
            this.TotalProgress.Location = new System.Drawing.Point(12, 25);
            this.TotalProgress.Name = "TotalProgress";
            this.TotalProgress.Size = new System.Drawing.Size(260, 23);
            this.TotalProgress.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File progress";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total Progress";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current File:";
            // 
            // CurrentFileName
            // 
            this.CurrentFileName.AutoSize = true;
            this.CurrentFileName.Location = new System.Drawing.Point(82, 102);
            this.CurrentFileName.Name = "CurrentFileName";
            this.CurrentFileName.Size = new System.Drawing.Size(79, 13);
            this.CurrentFileName.TabIndex = 5;
            this.CurrentFileName.Text = "Some file name";
            // 
            // ProgressPercentage
            // 
            this.ProgressPercentage.AutoSize = true;
            this.ProgressPercentage.Location = new System.Drawing.Point(85, 55);
            this.ProgressPercentage.Name = "ProgressPercentage";
            this.ProgressPercentage.Size = new System.Drawing.Size(35, 13);
            this.ProgressPercentage.TabIndex = 6;
            this.ProgressPercentage.Text = "label4";
            // 
            // UploadProgression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 125);
            this.Controls.Add(this.ProgressPercentage);
            this.Controls.Add(this.CurrentFileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TotalProgress);
            this.Controls.Add(this.FileProgress);
            this.MaximizeBox = false;
            this.Name = "UploadProgression";
            this.Text = "UploadProgression";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar FileProgress;
        private System.Windows.Forms.ProgressBar TotalProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label CurrentFileName;
        private System.Windows.Forms.Label ProgressPercentage;
        private System.ComponentModel.BackgroundWorker uploadWorker;
    }
}