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

namespace TechnicSolderHelper
{
    public partial class Modinfo : Form
    {
        private SolderHelper solderHelper;
        private String filename;
        public Modinfo(SolderHelper solderHelper)
        {
            this.solderHelper = solderHelper;
            InitializeComponent();
        }

        public Modinfo(List<Mcmod> modsList, SolderHelper solderHelper)
        {
            this.solderHelper = solderHelper;
            InitializeComponent();
            foreach (Mcmod mcmod in modsList)
            {
                if (String.IsNullOrWhiteSpace(mcmod.Name))
                {
                    modlist.Items.Add(mcmod.Filename);
                }
                else
                {
                    modlist.Items.Add(mcmod.Name);
                }
            }
        }

        private void modlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            filename = modlist.SelectedItem.ToString();
            Debug.WriteLine(filename);
        }
    }
}
