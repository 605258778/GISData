using GISData.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataRegister
{
    public partial class FormDomain : Form
    {
        private DataRowView dgvr;
        private string editId;
        private string regName;
        public FormDomain()
        {
            InitializeComponent();
        }

        public FormDomain(DataRowView dgvr, string regName)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.dgvr = dgvr;
            this.regName = regName;
            this.editId = this.dgvr["ID"].ToString();
            this.textBoxFieldName.Text = this.dgvr["字段名"].ToString();
            this.textBoxFieldAliName.Text = this.dgvr["别名"].ToString();
            this.comboBoxDomain.Text = this.dgvr["字典域"].ToString();
            this.textBoxDomainWhere.Text = this.dgvr["字典域条件"].ToString().Replace("'","\'");
        }

        private void FormDomain_Load(object sender, EventArgs e)
        {
            this.comboBoxDomain.Items.Add(new DictionaryEntry("资源数据字典", "GISDATA_ZYSJZD"));
            this.comboBoxDomain.Items.Add(new DictionaryEntry("政区数据字典", "GISDATA_ZQSJZD"));
            this.comboBoxDomain.DisplayMember = "Key";
            this.comboBoxDomain.ValueMember = "Value";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectDB cd = new ConnectDB();
            string domainValue = "";
            if (comboBoxDomain.SelectedItem != null)
            {
                DictionaryEntry selectValue = (DictionaryEntry)comboBoxDomain.SelectedItem;
                domainValue = selectValue.Value.ToString();
            }
            else 
            {
                domainValue = comboBoxDomain.Text;
            }
            string textWhere = textBoxDomainWhere.Text.Replace("'","\"");
            Boolean result = cd.Update("update GISDATA_MATEDATA SET  CODE_PK='" + domainValue + "',CODE_WHERE='" + textWhere + "' where ID = " + editId);
            if (result) 
            {
                MessageBox.Show("保存成功！", "提示");
                this.dgvr["字典域"] = domainValue;
                this.dgvr["字典域条件"] = textWhere;
                this.Close();
            }
        }
    }
}
