using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechnicSolderHelper.confighandler;
using TechnicSolderHelper.cryptography;

namespace TechnicSolderHelper.SQL
{
    public partial class sqlInfo : Form
    {
        public sqlInfo()
        {
            InitializeComponent();
            Crypto crypto = new Crypto();
            ConfigHandler ch = new ConfigHandler();
            this.database.Text = ch.getConfig("mysqlDatabase");
            this.serveraddress.Text = ch.getConfig("mysqlAddress");
            this.password.Text = crypto.DecryptString(ch.getConfig("mysqlPassword"));
            this.username.Text = ch.getConfig("mysqlUsername");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isEverythingFilledIn())
            {
                SolderSQLHandler sqh = new SolderSQLHandler(serveraddress.Text, username.Text, password.Text, database.Text);
                sqh.testConnection();
            }
            else
            {
                MessageBox.Show("Please fill out all the data");
            }
        }

        private bool isEverythingFilledIn()
        {
            if (String.IsNullOrWhiteSpace(database.Text) || String.IsNullOrWhiteSpace(serveraddress.Text) || String.IsNullOrWhiteSpace(username.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isEverythingFilledIn())
            {
                ConfigHandler ch = new ConfigHandler();
                Crypto crypto = new Crypto();
                ch.setConfig("mysqlUsername", username.Text);
                ch.setConfig("mysqlPassword", crypto.EncryptToString(password.Text));
                ch.setConfig("mysqlAddress", serveraddress.Text);
                ch.setConfig("mysqlDatabase", database.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill out all the data");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
