using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormStructureDia : Form
    {
        private string stepNo;
        private CheckBox checkBox;
        public FormStructureDia()
        {
            InitializeComponent();
        }
        public void SelectAll() 
        {
            this.gridView1.SelectAll();
        }

        public void UnSelectAll()
        {
            
            int[] rows = this.gridView1.GetSelectedRows();
            for (int i = 0; i < rows.Count(); i++) 
            {
                this.gridView1.UnselectRow(rows[i]);
            }
        }

        public FormStructureDia(string stepNo,CheckBox cb)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
            bindData();
        }

        private void FormStructureDia_Load(object sender, EventArgs e)
        {
            
        }

        private void bindData() 
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME,STATE,ERROR from GISDATA_REGINFO where IS_CHECK='1' and STEP_NO = '" + stepNo + "'");
            this.gridControl1.DataSource = dt;
            CommonClass common = new CommonClass();
            //common.AddCheckBox(this.gridControl1);
        }

        //private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if ((bool)dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
        //    {
        //        //dataGridView1.Rows[e.RowIndex].Cells[0].Value = false;
        //    }
        //    else
        //    {
        //        //dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
        //    }
        //}

        private void FormStructureDia_Load_1(object sender, EventArgs e)
        {

        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int[] selectRows = this.gridView1.GetSelectedRows();
            if (selectRows.Length > 0)
            {
                this.checkBox.CheckState = CheckState.Checked;
            }
            else
            {
                this.checkBox.CheckState = CheckState.Unchecked;
            }
        }

        public void doCheckStructure() 
        {
        
        }
    }
}
