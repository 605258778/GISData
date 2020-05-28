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

namespace GISData.User
{
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            bindData();
        }

        private void bindData()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_USER");
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
             DialogResult dr = MessageBox.Show("确定要删除吗?", "删除数据",MessageBoxButtons.YesNo);
             if (dr == DialogResult.Yes)
             {
                 var index = gridView1.GetFocusedDataSourceRowIndex();//获取数据行的索引值，从0开始
                 string selectID = gridView1.GetRowCellValue(index, "ID").ToString();//获取选中行的那个单元格的值
                 ConnectDB db = new ConnectDB();
                 Boolean result = db.Delete("delete from GISDATA_USER where ID =" + selectID);
                 if (result) 
                 {
                     bindData();
                 }
             }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormVerfied verfied = new FormVerfied();
            verfied.ShowDialog();
            if (verfied.DialogResult == DialogResult.OK) 
            {
                var index = gridView1.GetFocusedDataSourceRowIndex();//获取数据行的索引值，从0开始
                string selectID = gridView1.GetRowCellValue(index, "ID").ToString();//获取选中行的那个单元格的值
                ConnectDB db = new ConnectDB();
                Boolean result = db.Update("update gisdata_user set verified = '" + verfied.result + "' where ID =" + selectID + "");
                if (result) 
                {
                    bindData();
                }
            }
        }
    }
}
