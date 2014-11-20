using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnicSolderHelper.ftp
{
    public partial class ftpInfo : Form
    {
        public ftpInfo()
        {
            InitializeComponent();
        }

        private void Acceptbutton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Username.Text) || String.IsNullOrWhiteSpace(Password.Text) || String.IsNullOrWhiteSpace(Host.Text))
            {
                MessageBox.Show("Please fill out all values");
                return;
            }
            else
            {
                if (globalfunctions.isUnix())
                {
                    ConfigHandler ch = new ConfigHandler();




                    ch.setConfig("ftpPassword", null);
                }
            }
        }
    }
}
