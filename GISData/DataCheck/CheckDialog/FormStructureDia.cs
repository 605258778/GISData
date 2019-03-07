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

        public FormStructureDia()
        {
            InitializeComponent();
        }

        public FormStructureDia(string stepNo)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            bindData();
        }

        private void FormStructureDia_Load(object sender, EventArgs e)
        {
            
        }

        private void bindData() 
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME as 图层,REG_ALIASNAME as 别名 from GISDATA_REGINFO where IS_CHECK='1' and STEP_NO = '" + stepNo + "'");
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
    }
}
