using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModpackHelper.GUI.Properties;
using ModpackHelper.GUI.UserInteraction;
using ModpackHelper.IO;
using ModpackHelper.Shared.UserInteraction;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.Web.Solder;

namespace ModpackHelper.GUI.Windows
{
    public partial class MySQLConnectForm : Form
    {
        private IFileSystem fileSystem;
        private readonly IMessageShower messageShower;

        public MySQLConnectForm(IFileSystem fileSystem, IMessageShower messageShower)
        {
            this.fileSystem = fileSystem;
            this.messageShower = messageShower;
            InitializeComponent();
            ConfigHandler ch = new ConfigHandler(fileSystem);
            if (ch.Configs?.SolderLoginInfo == null || !ch.Configs.SolderLoginInfo.IsValid()) return;
            SolderLoginInfo solderInfo = ch.Configs.SolderLoginInfo;
            ServerAddressTextBox.Text = solderInfo.Address;
            UsernameTextBox.Text = solderInfo.Username;
            PasswordTextBox.Text = solderInfo.Password;
            DatabaseTextBox.Text = solderInfo.DatabaseSchema;
            PrefixTextBox.Text = solderInfo.TablePrefix;
        }

        public MySQLConnectForm() : this(new FileSystem(), new MessageShower())
        {
            
        }

        private SolderLoginInfo CreateSli()
        {
            return new SolderLoginInfo()
            {
                Address = ServerAddressTextBox.Text,
                DatabaseSchema = DatabaseTextBox.Text,
                Password = PasswordTextBox.Text,
                TablePrefix = PrefixTextBox.Text,
                Username = UsernameTextBox.Text
            };

        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            SolderLoginInfo sli = CreateSli();
            if (sli.IsValid())
            {
                SolderMySQLClient ssc = new SolderMySQLClient(sli);
                messageShower.ShowMessageAsync(ssc.TestConnection()
                    ? Resources.MySQLConnectionSuccess
                    : Resources.MySQLConnectionFailed);
            }
            else
            {
                messageShower.ShowMessageAsync("Please fill out all the data!");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SolderLoginInfo sli = CreateSli();
            if (sli.IsValid())
            {
                using (ConfigHandler ch = new ConfigHandler(fileSystem))
                    ch.Configs.SolderLoginInfo = sli;
                Close();
            }
            else
            {
                messageShower.ShowMessageAsync("Please fill out all the data!");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
