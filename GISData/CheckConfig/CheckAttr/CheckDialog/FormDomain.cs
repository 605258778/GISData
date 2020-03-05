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

namespace GISData.ChekConfig.CheckDialog
{
    public partial class FormDomain : Form
    {
        public ListBox listBoxFieldValue
        {
            get { return listBoxField; }
            set { this.listBoxField = value; }
        }
        public string domainTable;
        public string wheresSring;
        public string domainType;
        public string DOMAINVALUE;
        private ComboBox comboBoxDataSour;
        private string type;
        private string selectedId;
        public FormDomain()
        {
            InitializeComponent();
        }

        public FormDomain(ComboBox comboBox,string type,string selectedId)
        {
            InitializeComponent();
            comboBoxDataSour = comboBox;
            this.type = type;
            this.selectedId = selectedId;
        }
        //增加值域
        private void button1_Click(object sender, EventArgs e)
        {
            string textBoxDomain = this.textBoxDomain.Text;
            if (listBox1.FindString(textBoxDomain) != ListBox.NoMatches)
            {
                MessageBox.Show("此项已经存在");
                return;
            }
            //将文本框中文本加入到ListBox的列表项中
            listBox1.Items.Add(textBoxDomain);
            this.DOMAINVALUE = "";
            foreach (object item in listBox1.Items) 
            {
                this.DOMAINVALUE += item.ToString() + ",";
            }
            this.DOMAINVALUE = this.DOMAINVALUE.Substring(0, this.DOMAINVALUE.Length - 1);
            this.textBoxDomain.Clear();
        }
        //删除值域
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
                this.listBox1.Items.Remove(this.listBox1.SelectedItem);
            if (this.listBox1.Items.Count == 0)
                return;
        }

        private void FormDomain_Load(object sender, EventArgs e)
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            listBoxField.DataSource = dt;
            listBoxField.DisplayMember = "FIELD_ALSNAME";
            listBoxField.ValueMember = "FIELD_NAME";
            if (this.type == "edit")
            {
                DataTable editDB = db.GetDataBySql("select FIELD,DOMAINTYPE,DOMAINVALUE from GISDATA_TBATTR where id = " + selectedId);
                DataRow[] drs = editDB.Select("1=1");
                string field = drs[0]["FIELD"].ToString();
                string domaintype = drs[0]["DOMAINTYPE"].ToString();
                for (int i = 0; i < listBoxField.Items.Count; i++)
                {
                    DataRowView item = (DataRowView)listBoxField.Items[i];
                    if (item.Row[0].ToString().Equals(field))
                    {
                        this.listBoxField.SelectedIndex = i;
                    }
                }
                if (domaintype == "own") 
                {
                    this.radioButton1.Select();
                    loadDamain();
                }
            }
        }
        /// <summary>
        /// 系统域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                loadDamain();
            }
            else 
            {
                this.splitContainer3.Enabled = true;
                this.domainType = "custom";
            }
        }
        /// <summary>
        /// 加载系统字典域
        /// </summary>
        private void loadDamain() 
        {
            this.splitContainer3.Enabled = false;
            ConnectDB db = new ConnectDB();
            string table = comboBoxDataSour.SelectedValue.ToString();
            string selectField = listBoxField.SelectedValue.ToString();
            DataTable dt = db.GetDataBySql("select CODE_PK,CODE_WHERE from GISDATA_MATEDATA where REG_NAME = '" + table + "' and FIELD_NAME = '" + selectField + "'");
            this.domainTable = dt.Rows[0]["CODE_PK"].ToString();
            if (this.domainTable == "政区数据字典")
            {
                this.domainTable = "GISDATA_ZQSJZD";
                DataTable result = db.GetDataBySql("select C_CODE from GISDATA_ZQSJZD where " + dt.Rows[0]["CODE_WHERE"].ToString());
                this.listBox1.DataSource = result;
                this.listBox1.DisplayMember = "C_CODE";
            }
            else if (this.domainTable == "资源数据字典")
            {
                this.domainTable = "GISDATA_ZYSJZD";
                DataTable result = db.GetDataBySql("select C_CODE from GISDATA_ZYSJZD where " + dt.Rows[0]["CODE_WHERE"].ToString());
                this.listBox1.DataSource = result;
                this.listBox1.DisplayMember = "C_CODE";
            }
            this.DOMAINVALUE = "";
            if (listBox1.Items.Count > 0) 
            {
                foreach (DataRowView item in listBox1.Items)
                {
                    this.DOMAINVALUE += item[0].ToString() + ",";
                }
                this.DOMAINVALUE = this.DOMAINVALUE.Substring(0, this.DOMAINVALUE.Length - 1);
                this.wheresSring = dt.Rows[0]["CODE_WHERE"].ToString();
                this.domainType = "own";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.splitContainer3.Enabled = false;
                this.domainType = "own";
            }
            else
            {
                this.listBox1.DataSource = null;
                this.listBox1.Items.Clear();
                this.splitContainer3.Enabled = true;
                this.domainType = "custom";
            }
        }
    }
}
