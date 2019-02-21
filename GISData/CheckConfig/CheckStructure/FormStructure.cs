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

namespace GISData.CheckConfig.CheckStructure
{
    public partial class FormStructure : Form
    {
        private int Step_no;

        public FormStructure()
        {
            InitializeComponent();
        }

        public FormStructure(int p)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.Step_no = p;
        }

        private void FormStructure_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO");
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "REG_ALIASNAME";
            comboBox1.ValueMember = "REG_NAME";
            refreshDatagrid();
        }
        /// <summary>
        /// 刷新datagridview
        /// </summary>
        private void refreshDatagrid() 
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME as 图层,REG_ALIASNAME as 别名 from GISDATA_REGINFO where IS_CHECK='1' and STEP_NO = '"+Step_no+"'");
            this.dataGridView1.DataSource = dt;
        }

        private void StrutureAdd_Click(object sender, EventArgs e)
        {
            string select = this.comboBox1.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            Boolean result = db.Update("update GISDATA_REGINFO set IS_CHECK='1',STEP_NO='" + Step_no + "' where REG_NAME = '" + select + "'");
            if (result) 
            {
                refreshDatagrid();
            }
        }

        private void StrutureRemove_Click(object sender, EventArgs e)
        {
            int a = this.dataGridView1.CurrentRow.Index;
            string select = this.dataGridView1.Rows[a].Cells[0].Value.ToString();
            ConnectDB db = new ConnectDB();
            Boolean result = db.Update("update GISDATA_REGINFO set IS_CHECK='0',STEP_NO='' where REG_NAME = '" + select + "'");
            if (result)
            {
                refreshDatagrid();
            }
        }
    }
}
