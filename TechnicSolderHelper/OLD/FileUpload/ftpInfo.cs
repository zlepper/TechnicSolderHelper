using System;
using System.Net;
using System.Windows.Forms;
using TechnicSolderHelper.OLD.confighandler;
using TechnicSolderHelper.OLD.cryptography;

namespace TechnicSolderHelper.OLD.FileUpload
{
    public partial class FtpInfo : Form
    {
        public FtpInfo()
        {
            InitializeComponent();
            String url = "";
            String username = "";
            String pass = "";
            Crypto crypto = new Crypto();
            try
            {
                ConfigHandler ch = new ConfigHandler();
                url = ch.GetConfig("ftpUrl");
                username = ch.GetConfig("ftpUserName");
                pass = crypto.DecryptString(ch.GetConfig("ftpPassword"));
            }
            catch (Exception)
            {
                // ignored
            }
            Username.Text = username;
            Password.Text = pass;
            Host.Text = url;
        }

        private void Acceptbutton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Username.Text) || String.IsNullOrWhiteSpace(Password.Text) || String.IsNullOrWhiteSpace(Host.Text))
            {
                MessageBox.Show("Please fill out all values");
            }
            else
            {
                String url = Host.Text;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    Crypto crypto = new Crypto();
                    ConfigHandler ch = new ConfigHandler();
                    ch.SetConfig("ftpUserName", Username.Text);
                    ch.SetConfig("ftpUrl", url);
                    ch.SetConfig("ftpPassword", crypto.EncryptToString(Password.Text));
                    Close();
                }
                else
                {
                    MessageBox.Show("Hostname is not valid");
                }

            }
        }

        private void test_Click(object sender, EventArgs e)
        {
            String url = Host.Text;
            String name = Username.Text;
            String pass = Password.Text;

            MessageBox.Show(IsValidConnection(url, name, pass));
        }

        private String IsValidConnection(string url, string user, string password)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential(user, password);
                    request.GetResponse();
                }
                catch (WebException ex)
                {
                    return ex.Message;
                }
                return "All is working fine!!";
            }
            return "Invalid hostname";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Host_TextChanged(object sender, EventArgs e)
        {
            if (!Host.Text.StartsWith("ftp://"))
            {
                Host.Text = "ftp://" + Host.Text;
            }
        }
    }
}
