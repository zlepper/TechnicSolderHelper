using System;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Windows.Forms;
using ModpackHelper.GUI.UserInteraction;
using ModpackHelper.IO;
using ModpackHelper.Shared.Utils;
using ModpackHelper.Shared.Web.FTP;

namespace ModpackHelper.GUI.Windows
{
    public partial class FtpUploaderForm : Form
    {
        private DirectoryInfoBase uploadPath;
        private BackgroundWorker uploader;
        private IFileSystem fileSystem;

        public FtpUploaderForm(DirectoryInfoBase path):this(path, new FileSystem())
        {
            
        }

        public FtpUploaderForm(DirectoryInfoBase path, IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            InitializeComponent();
            uploadPath = path;
            uploader = new BackgroundWorker();
            uploader.DoWork += UploaderOnDoWork;
            uploader.RunWorkerAsync();
            uploader.RunWorkerCompleted += UploaderOnRunWorkerCompleted;
        }

        private void UploaderOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UploaderOnRunWorkerCompleted(sender, runWorkerCompletedEventArgs)));
            }
            else
            {
                new MessageShower().ShowMessageAsync("Done uploading");
                Close();
            }
        }

        private void UploaderOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            ConfigHandler ch = new ConfigHandler(fileSystem);
            var loginInfo = ch.Configs.FTPLoginInfo;
            FTPUploader ftp = new FTPUploader(loginInfo, fileSystem);
            ftp.WorkingFileChanged += FtpOnWorkingFileChanged;
            ftp.FileProgressChanged += FtpOnFileProgressChanged;
            ftp.UploadFolder(uploadPath);
        }

        private void FtpOnFileProgressChanged(double progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => FtpOnFileProgressChanged(progress)));
            }
            else
            {
                UploadProgressBar.Value = (int)progress;
            }
        }

        private void FtpOnWorkingFileChanged(string filename)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => FtpOnWorkingFileChanged(filename)));
            }
            else
            {
                Debug.WriteLine(filename);
                UploadingFileLabel.Text = filename;
            }
        }

        private void FinishedButton_Click(object sender, EventArgs e)
        {
            new MessageShower().ShowMessageAsync("Yeah no. This button doesn't actually work either.");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            new MessageShower().ShowMessageAsync("Yeah no. This button doesn't actually work.");
        }
    }
}
