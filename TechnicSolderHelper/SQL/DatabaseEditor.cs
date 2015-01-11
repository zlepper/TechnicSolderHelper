using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnicSolderHelper.SQL
{
    public partial class DatabaseEditor : Form
    {
        public DatabaseEditor()
        {
            InitializeComponent();
            ModListSqlHelper modListSqlHelper = new ModListSqlHelper();
            data.DataSource = modListSqlHelper.GetTableInfoForEditing();
            if (data.Columns["ID"] != null)
            {
                data.Columns["ID"].Visible = false;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            ModListSqlHelper modListSqlHelper = new ModListSqlHelper();
            modListSqlHelper.SetTableInfoAfterEditing(data.DataSource as DataTable);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            
            Close();
        }

        private void SaveAndExit_Click(object sender, EventArgs e)
        {
            Save_Click(null, null);
            Close();
        }
    }
}
