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
            Shown += DatabaseEditor_Shown;
        }

        private void DatabaseEditor_Shown(object sender, EventArgs e)
        {
            int col1 = data.Columns["ModName"].Width;
            int col2 = data.Columns["ModID"].Width; 
            int col3 = data.Columns["ModVersion"].Width;
            int col4 = data.Columns["MinecraftVersion"].Width;
            int col5 = data.Columns["FileName"].Width;
            int allCol = col1 + col2 + col3 + col4 + col5;
            this.Size = new System.Drawing.Size(allCol + 95, this.Height);
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

        private void data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void highLightVerBtn_Click(object sender, EventArgs e)
        {
            int ModVersionIndex = data.Columns["ModVersion"].Index;
            int MinecraftVersionIndex = data.Columns["MinecraftVersion"].Index;
            foreach (DataGridViewRow Myrow in data.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                string wtf = Myrow.Cells[ModVersionIndex].Value.ToString();
                if (Myrow.Cells[ModVersionIndex].Value.ToString().Contains(Myrow.Cells[MinecraftVersionIndex].Value.ToString()))
                {
                    data.Rows[Myrow.Index].Cells[ModVersionIndex].Style.BackColor = Color.Red;

                }
                else
                {
                    data.Rows[Myrow.Index].Cells[ModVersionIndex].Style.BackColor = Color.White;
                }
            }
        }
    }
}
