using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckConfig.CheckReport
{
    public partial class FormReportConfig : Form
    {
        public FormReportConfig()
        {
            InitializeComponent();
        }

        private void FormReportDia_Load(object sender, EventArgs e)
        {
            getFile();
            bindData();
            bindDataSource();
        }
        /// <summary>
        /// 获取所有模板文件
        /// </summary>
        private void getFile() 
        {
            DirectoryInfo folder = new DirectoryInfo(Application.StartupPath+"\\Report");
            foreach (FileInfo file in folder.GetFiles("*.xlsx"))
            {
                this.comboBox1.Items.Add(file.Name);
            }
        }
        /// <summary>
        /// 添加报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string reportname = this.textBox1.Text;
            string reportmould = this.comboBox1.Text;
            string datasource = this.checkedComboBoxEdit1.EditValue.ToString().Trim();

            ConnectDB db = new ConnectDB();
            Boolean result = db.Insert("insert into GISDATA_REPORT (REPORTNAME,REPORTMOULD,DATASOURCE) VALUES ('" + reportname + "','" + reportmould + "','" + datasource + "')");
            if (result) 
            {
                bindData();
            }
        }

        /// <summary>
        /// 绑定所有的报表
        /// </summary>
        private void bindData()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_REPORT");
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void bindDataSource()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO");
            checkedComboBoxEdit1.Properties.DataSource = dt;
            checkedComboBoxEdit1.Properties.DisplayMember = "REG_ALIASNAME";
            checkedComboBoxEdit1.Properties.ValueMember = "REG_NAME";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            int[] selectRows = this.gridView1.GetSelectedRows();
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                int ID = int.Parse(row["ID"].ToString());
                db.Delete("delete from GISDATA_REPORT WHERE ID = " + ID);
            }
            bindData();
        }
    }
}
