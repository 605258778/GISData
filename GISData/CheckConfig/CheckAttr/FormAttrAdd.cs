using GISData.ChekConfig.CheckDialog;
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

namespace GISData.ChekConfig
{
    public partial class FormAttrAdd : Form
    {
        private FormLogic formLogicDig;
        private FormDomain formDomainDig;
        private FormNullValue formNullValueDig;
        private FormUnique formUniqueDig;
        private int checkNo;
        private string selectedId;
        public FormAttrAdd()
        {
            InitializeComponent();
        }

        public FormAttrAdd(int No,string id )
        {
            InitializeComponent();
            this.checkNo = No;
            this.selectedId = id;
        }

        private void FormAttrDig_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO");
            comboBoxDataSour.DataSource = dt;
            comboBoxDataSour.DisplayMember = "REG_ALIASNAME";
            comboBoxDataSour.ValueMember = "REG_NAME";
            comboBoxDataSour.SelectedIndex = 0;
            DataRowView dr = (DataRowView)comboBoxDataSour.Items[0];
            string table = dr["REG_NAME"].ToString();
            DataTable dtfield = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            FIeldList.DataSource = dtfield;
            FIeldList.DisplayMember = "FIELD_ALSNAME";
            FIeldList.ValueMember = "FIELD_NAME";
        }

        private void comboBoxDataSour_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxDataSour_SelectedValueChanged(object sender, EventArgs e)
        {
            DispalyField();
        }

        private void DispalyField() 
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            if (comboBoxDataSour.Focused)
            {
                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
                FIeldList.DataSource = dt;
                FIeldList.DisplayMember = "FIELD_ALSNAME";
                FIeldList.ValueMember = "FIELD_NAME";
            }
        }

        private void comboBoxCheckType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.splitContainer3.Panel1.Controls.Clear();
            string checkType = comboBoxCheckType.SelectedItem.ToString();
            if (checkType=="空值检查")
            {
                FormNullValue nullValue = new FormNullValue(comboBoxDataSour);
                formNullValueDig = nullValue;
                ShowForm(this.splitContainer3.Panel1, nullValue);
            }
            else if (checkType == "值域检查")
            {
                FormDomain formDomain = new FormDomain(comboBoxDataSour);
                formDomainDig = formDomain;
                ShowForm(this.splitContainer3.Panel1, formDomain);
                
            }
            else if (checkType == "唯一值检查")
            {
                FormUnique formUnique = new FormUnique();
                formUniqueDig = formUnique;
                ShowForm(this.splitContainer3.Panel1, formUnique);
            }
            else if (checkType == "逻辑关系检查")
            {
                FormLogic formLogic = new FormLogic();
                formLogicDig = formLogic;
                ShowForm(this.splitContainer3.Panel1, formLogic);
            }
        }

        public void ShowForm(Panel panel,Form frm)
        {
            lock (this)
            {
                try
                {
                    frm.TopLevel = false;
                    frm.MdiParent = this;
                    frm.Parent = panel;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Show();
                }
                catch (System.Exception ex)
                {
                    //
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string checkType = comboBoxCheckType.SelectedItem.ToString();
            string name = textBoxName.Text;
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            if (checkType == "空值检查")
            {
                string showfield = FIeldList.SelectedValue.ToString();
                string field = formNullValueDig.SelectedValue.ToString();
                Boolean result = db.Insert("insert into GISDATA_TBATTR (PARENTID,NAME,STEP,CHECKTYPE,TABLENAME,SHOWFIELD,FIELD) VALUES('" + selectedId + "','" + name + "','" + checkNo + "','NullValue','" + table + "','" + showfield + "','" + field  + "')");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (checkType == "值域检查")
            {

            }
            else if (checkType == "唯一值检查")
            {

            }
            else if (checkType == "逻辑关系检查")
            {
                FormLogic formLogic = new FormLogic();
                string where = formLogicDig.textBoxWhereValue;
                string result = formLogicDig.textBoxResultValue;
                string fieldList = this.FIeldList.SelectedItems.ToString();
            }
            
        }
    }
}
