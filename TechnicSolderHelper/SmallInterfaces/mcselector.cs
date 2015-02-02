using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechnicSolderHelper.SQL.forge;

namespace TechnicSolderHelper.SmallInterfaces
{
    public partial class Mcselector : Form
    {
        private readonly SolderHelper _solderHelper;
        public Mcselector(SolderHelper sh)
        {
            _solderHelper = sh;
            InitializeComponent();
            ForgeSqlHelper f = new ForgeSqlHelper();
            mcversions.Items.AddRange(f.GetMcVersions().ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = mcversions.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("You need to select a minecraft version to continue.");
                return;
            }
            String s = mcversions.SelectedItem.ToString();
            _solderHelper._currentMcVersion = s;
            Close();
        }
    }
}
