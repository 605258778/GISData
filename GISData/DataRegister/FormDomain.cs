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

namespace GISData.DataRegister
{
    public partial class FormDomain : Form
    {
        private DataGridViewRow dgvr;
        private string editId;
        private string regName;
        public FormDomain()
        {
            InitializeComponent();
        }

        public FormDomain(DataGridViewRow dgvr,string regName)
        {
            // TODO: Complete member initialization
            this.dgvr = dgvr;
            this.regName = regName;
            InitializeComponent();
            this.editId = this.dgvr.Cells["ID"].Value.ToString();
            this.textBoxFieldName.Text = this.dgvr.Cells["字段名"].Value.ToString();
            this.textBoxFieldAliName.Text = this.dgvr.Cells["别名"].Value.ToString();
            this.comboBoxIsShow.Text = this.dgvr.Cells["是否显示"].Value.ToString();
            this.comboBoxIsEdit.Text = this.dgvr.Cells["是否只读"].Value.ToString();
            this.comboBoxDomain.Text = this.dgvr.Cells["字典域"].Value.ToString();
            this.textBoxDomainWhere.Text = this.dgvr.Cells["字典域条件"].Value.ToString();
        }

        private void FormDomain_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectDB cd = new ConnectDB();
            Boolean result = cd.Update("update GISDATA_MATEDATA SET CAN_SHOW = '" + comboBoxIsShow.Text + "',IS_READONLY='" + comboBoxIsEdit.Text + "',CODE_PK='" + comboBoxDomain.Text + "',CODE_WHERE='" + textBoxDomainWhere.Text + "' where ID = " + editId);
            if (result) 
            {
                MessageBox.Show("保存成功！", "提示");
                this.Close();
                FormRegister fr = new FormRegister();
                fr.refreshDataGridField(regName);
            }
        }
    }
}
