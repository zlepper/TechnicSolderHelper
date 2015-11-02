using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModpackHelper.GUI.UserInteraction;
using ModpackHelper.IO;
using ModpackHelper.Shared.UserInteraction;
using ModpackHelper.Shared.Utils.Config;

namespace ModpackHelper.GUI.Windows
{
    public partial class FTPConnectForm : Form
    {
        private IFileSystem fileSystem;
        private IMessageShower messageShower;

        public FTPConnectForm(IFileSystem fileSystem, IMessageShower messageShower)
        {
            this.fileSystem = fileSystem;
            this.messageShower = messageShower;
            InitializeComponent();
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                if (ch.Configs?.FTPLoginInfo == null || !ch.Configs.FTPLoginInfo.IsValid()) return;
                FTPLoginInfo ftpLoginInfo = ch.Configs.FTPLoginInfo;
                ServerAddressTextBox.Text = ftpLoginInfo.Address;
                UsernameTextBox.Text = ftpLoginInfo.Username;
                PasswordTextBox.Text = ftpLoginInfo.Password;
            }
        }

        public FTPConnectForm() : this(new FileSystem(), new MessageShower())
        {
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            string url = ServerAddressTextBox.Text;
            string name = UsernameTextBox.Text;
            string pass = PasswordTextBox.Text;

            MessageBox.Show(IsValidConnection(url, name, pass));
        }

        private string IsValidConnection(string url, string user, string password)
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

        private void ServerAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!ServerAddressTextBox.Text.StartsWith("ftp://"))
            {
                ServerAddressTextBox.Text = $"ftp://{ServerAddressTextBox.Text}";
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text) || string.IsNullOrWhiteSpace(ServerAddressTextBox.Text))
            {
                MessageBox.Show("Please fill out all values");
            }
            else
            {
                string url = ServerAddressTextBox.Text;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    using (ConfigHandler ch = new ConfigHandler(fileSystem))
                    {
                        if (ch.Configs != null)
                        {
                            ch.Configs.FTPLoginInfo = new FTPLoginInfo()
                            {
                                Address = ServerAddressTextBox.Text,
                                Password = PasswordTextBox.Text,
                                Username = UsernameTextBox.Text
                            };
                        }
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Hostname is not valid");
                }

            }
        }
    }
}
