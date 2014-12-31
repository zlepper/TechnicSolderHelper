using System;
using System.Windows.Forms;
using TechnicSolderHelper.Properties;

namespace TechnicSolderHelper
{
    public class MessageToUser
    {
        public void FirstTimeRun()
        {
            MessageBox.Show("This is the first time you are running SolderHelper so it might take a while to start  since it needs to build some databases.");
        }

        public void UploadingToFtp()
        {
            MessageBox.Show("Uploading stuff to FTP");
        }

        public void UploadToS3()
        {
            MessageBox.Show("Uploading files to s3");
        }
    }
}

