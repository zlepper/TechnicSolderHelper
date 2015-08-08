using System;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Windows.Forms;
using ModpackHelper.Shared.Web.FTP;

namespace ModpackHelper.GUI.Windows
{
    public partial class FTPUploaderForm : Form
    {
        private DirectoryInfoBase uploadPath;
        private BackgroundWorker uploader;
        public FTPUploaderForm(DirectoryInfoBase path):this(path, new FileSystem())
        {
            
        }

        public FTPUploaderForm(DirectoryInfoBase path, IFileSystem fileSystem)
        {
            InitializeComponent();
            uploadPath = path;
            uploader = new BackgroundWorker();
            uploader.DoWork += UploaderOnDoWork;
        }

        private void UploaderOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            
        }
    }
}
