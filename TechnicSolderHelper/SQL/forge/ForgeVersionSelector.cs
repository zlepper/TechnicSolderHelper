using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnicSolderHelper.SQL.forge
{
    public partial class ForgeVersionSelector : Form
    {
        private readonly SolderHelper _solderHelper;
        public ForgeVersionSelector(SolderHelper solderHelper)
        {
            _solderHelper = solderHelper;
            InitializeComponent();
            ForgeSqlHelper helper = new ForgeSqlHelper();
            List<string> forgeVersions = helper.GetForgeVersions(solderHelper._currentMcVersion);
            comboBox1.Items.AddRange(forgeVersions.ToArray());
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string version = comboBox1.SelectedItem.ToString();
            _solderHelper.PackForge(version);
            Close();
        }
    }
}
