using ESRI.ArcGIS.Geodatabase;
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
        private Dictionary<string, string> DicPivot = new Dictionary<string, string>();
        private DataTable dtSource = new DataTable();
        private int checkNo;
        private string scheme;

        public FormReportConfig()
        {
            InitializeComponent();;
        }
        public FormReportConfig(int No, string scheme)
        {
            InitializeComponent();
            this.checkNo = No;
            this.scheme = scheme;
        }

        private void FormReportDia_Load(object sender, EventArgs e)
        {
            getFile();
            bindData();
            bindDataSource();
            this.labelpivot.Visible = false;
            this.textBoxpivot.Visible = false;
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
            string sqlstr = this.textBoxSql.Text;
            string reportmould = this.comboBox1.Text;
            string reporttype = this.comboBox2.Text;
            string datasource = this.checkedComboBoxEdit1.EditValue.ToString().Trim();
            string sheetname = this.checkedComboBoxEdit3.EditValue.ToString().Trim();
            string valuestring = "";
            DataRow[] drs = this.dtSource.Select(null);
            foreach (DataRow dr in drs)
            {
                valuestring += dr[0].ToString() + "@" + dr[1].ToString() + "#";
            }
            ConnectDB db = new ConnectDB();
            Boolean result = true;
            if (reporttype == "透视表")
            {
                result = db.Insert("insert into GISDATA_REPORT (REPORTNAME,REPORTMOULD,DATASOURCE,REPORTTYPE,SHEETNAME,SQLSTR,ROWNAME,COLUMNSNAME,VALUESTRING,SCHEME,STEP_NO) VALUES ('" + reportname + "','" + reportmould + "','" + datasource + "','" + reporttype + "','" + sheetname + "','" + sqlstr + "','" + this.DicPivot["ROW"].Trim() + "','" + this.DicPivot["COLUMNS"].Trim() + "','" + valuestring.Trim() + "','" + this.scheme + "'," + this.checkNo + ")");
            }
            else 
            {
                result = db.Insert("insert into GISDATA_REPORT (REPORTNAME,REPORTMOULD,DATASOURCE,REPORTTYPE,SHEETNAME,SQLSTR,SCHEME,STEP_NO) VALUES ('" + reportname + "','" + reportmould + "','" + datasource + "','" + reporttype + "','" + sheetname + "','" + sqlstr + "','" + this.scheme + "'," + this.checkNo + ")");
            }
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
            DataTable dt = db.GetDataBySql("select * from GISDATA_REPORT where SCHEME ='" + this.scheme + "' and STEP_NO = '" + this.checkNo + "'");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stringPath = Application.StartupPath + "\\Report";
            CommonClass common = new CommonClass();
            List<string> listSource = common.GetExcelSheetNames(stringPath + "\\" + this.comboBox1.Text);
            this.checkedComboBoxEdit3.Properties.DataSource = listSource;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] dataSourceArr = this.checkedComboBoxEdit1.EditValue.ToString().Split(',');
            string sql = "("+this.textBoxSql.Text.ToString()+")";
            CommonClass conClass = new CommonClass();
            IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(dataSourceArr[0]);
            IWorkspace iw = conClass.GetWorkspaceByName(dataSourceArr[0]);
            IQueryDef pQueryDef = ifw.CreateQueryDef();
            pQueryDef.Tables = sql;
            pQueryDef.SubFields = "*";
            try 
            {
                ICursor pCur = pQueryDef.Evaluate();
                MessageBox.Show("SQL通过检查");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("SQL没有通过检查");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.Text == "透视表")
            {
                this.labelpivot.Visible = true;
                this.textBoxpivot.Visible = true;
                FormPivotConfig pivot = new FormPivotConfig(DicPivot,this.checkedComboBoxEdit1.EditValue.ToString().Split(','));
                pivot.ShowDialog();
                if (pivot.DialogResult == DialogResult.OK) 
                {
                    this.DicPivot = pivot.DicPivotValue;
                    this.dtSource = pivot.dtSourceValue;
                    textBoxpivot.Text = "行：" + this.DicPivot["ROW"] + "\r\n";
                    textBoxpivot.Text += "列：" + this.DicPivot["COLUMNS"] + "\r\n";
                    textBoxpivot.Text += "值："+ "\r\n";
                    DataRow[] drs = this.dtSource.Select(null);
                    foreach (DataRow dr in drs) 
                    {
                        textBoxpivot.Text += dr[0].ToString()+"--"+dr[1].ToString() + "\r\n";
                    }
                }
            }
            else
            {
                this.labelpivot.Visible = false;
                this.textBoxpivot.Visible = false;
            }
        }


    }
}
