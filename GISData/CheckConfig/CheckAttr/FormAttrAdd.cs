﻿using DevExpress.XtraTreeList.Nodes;
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
        private TreeListNode selectNode;
        private string type;
        public FormAttrAdd()
        {
            InitializeComponent();
        }

        public FormAttrAdd(int No, TreeListNode node,string type)
        {
            InitializeComponent();
            selectNode = node;
            this.checkNo = No;
            this.selectedId = node.GetValue("ID").ToString();
            this.type = type;
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
            if (type == "edit")
            {
                LoadData(dt);
            }
        }

        private void LoadData(DataTable dt) 
        {
            textBoxName.Text = this.selectNode.GetValue("NAME").ToString();
            string tablename = this.selectNode.GetValue("TABLENAME").ToString();
            DataRow[] drs = dt.Select("1=1");
            foreach(DataRow dr in drs)
            {
                if (tablename == dr["REG_NAME"].ToString()) 
                {
                    int index = this.comboBoxDataSour.FindString(dr["REG_ALIASNAME"].ToString());
                    this.comboBoxDataSour.SelectedIndex = index;
                    DispalyField(tablename, true);
                }
            }
            string CHECKTYPE = this.selectNode.GetValue("CHECKTYPE").ToString();
            int TYPEindex = this.comboBoxCheckType.FindString(CHECKTYPE);
            this.comboBoxCheckType.SelectedIndex = TYPEindex;
            displayChildWindow(CHECKTYPE);
            string showField = this.selectNode.GetValue("SHOWFIELD").ToString();
            string[] FieldArray = showField.Split(',');
            foreach (string iitem in FieldArray)
            {
                for (int i = 0; i < FIeldList.Items.Count; i++)
                {
                    DataRowView item = (DataRowView)FIeldList.Items[i];
                    if (item.Row[0].ToString() == iitem)
                    {
                        FIeldList.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void comboBoxDataSour_SelectedIndexChanged(object sender, EventArgs e)
        {
            DispalyField("",false);
        }

        private string getCheckedList() 
        {
            string strCollected = string.Empty;

            for (int i = 0; i < FIeldList.Items.Count; i++)
            {

                if (FIeldList.GetItemChecked(i))
                {
                    if (strCollected == string.Empty)
                    {
                        strCollected = FIeldList.GetItemText(FIeldList.Items[i]);
                    }
                    else
                    {
                        strCollected = strCollected + "/" + FIeldList.GetItemText(FIeldList.Items[i]);

                    }
                }
            }
            return strCollected;
        }

        private void comboBoxDataSour_SelectedValueChanged(object sender, EventArgs e)
        {
            DispalyField("", false);
        }

        private void DispalyField(string tablename,Boolean iskey) 
        {
            string table = tablename==""?comboBoxDataSour.SelectedValue.ToString():tablename;
            if (comboBoxDataSour.Focused || iskey)
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
            displayChildWindow("");
        }
        /// <summary>
        /// 显示下级窗口
        /// </summary>
        /// <param name="inputType"></param>
        private void displayChildWindow(string inputType) 
        {
            this.splitContainer3.Panel1.Controls.Clear();
            string checkType = inputType == "" ? comboBoxCheckType.SelectedItem.ToString() : inputType;
            if (checkType == "空值检查")
            {
                FormNullValue nullValue = new FormNullValue(comboBoxDataSour, this.type, selectedId);
                formNullValueDig = nullValue;
                ShowForm(this.splitContainer3.Panel1, nullValue);
            }
            else if (checkType == "值域检查")
            {
                FormDomain formDomain = new FormDomain(comboBoxDataSour,this.type,selectedId);
                formDomainDig = formDomain;
                ShowForm(this.splitContainer3.Panel1, formDomain);

            }
            else if (checkType == "唯一值检查")
            {
                FormUnique formUnique = new FormUnique(comboBoxDataSour, this.type, this.selectedId);
                formUniqueDig = formUnique;
                ShowForm(this.splitContainer3.Panel1, formUnique);
                //formUnique.textCheckedValue = this.selectNode.GetValue("FIELD").ToString();
            }
            else if (checkType == "逻辑关系检查")
            {
                FormLogic formLogic = new FormLogic();
                formLogicDig = formLogic;
                ShowForm(this.splitContainer3.Panel1, formLogic);
                formLogic.textBoxWhereValue = this.selectNode.GetValue("WHERESTRING").ToString();
                formLogic.textBoxResultValue = this.selectNode.GetValue("RESULT").ToString();
            }
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
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
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            string checkType = comboBoxCheckType.SelectedItem.ToString();
            string name = textBoxName.Text;
            string table = comboBoxDataSour.SelectedValue.ToString();
            string showfield = "";
            foreach (DataRowView itemChecked in this.FIeldList.CheckedItems)
            {
                showfield += itemChecked.Row[0] + ",";
            }
            showfield = showfield.Substring(0, showfield.Length - 1);
            //foreach (object itemChecked in FIeldList.CheckedIndices) 
            //{
            //    showfield += itemChecked.ToString()+",";
            //}
            ConnectDB db = new ConnectDB();
            if (checkType == "空值检查")
            {
                string field = formNullValueDig.SelectedValue.ToString();
                Boolean result;
                if (type == "edit")
                {
                    result = db.Update("update GISDATA_TBATTR set NAME = '" + name + "',CHECKTYPE = '空值检查',TABLENAME = '" + table + "',SHOWFIELD = '" + showfield + "',FIELD = '" + field + "' where id = " + selectedId);
                }
                else 
                {
                    result = db.Insert("insert into GISDATA_TBATTR (PARENTID,NAME,STEP,CHECKTYPE,TABLENAME,SHOWFIELD,FIELD) VALUES('" + selectedId + "','" + name + "','" + checkNo + "','空值检查','" + table + "','" + showfield + "','" + field + "')");
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (checkType == "值域检查")
            {
                string domainTable = formDomainDig.domainTable;
                string wheresSring = formDomainDig.wheresSring;
                string domainType = formDomainDig.domainType;
                string DOMAINVALUE = formDomainDig.DOMAINVALUE;
                string selectField = formDomainDig.listBoxFieldValue.SelectedValue.ToString();
                Boolean result;
                if (type == "edit")
                {
                    result = db.Update("update GISDATA_TBATTR set NAME = '" + name + "',CHECKTYPE = '值域检查',TABLENAME = '" + table + "',SHOWFIELD = '" + showfield + "',FIELD = '" + selectField + "',SUPTABLE = '" + domainTable + "',WHERESTRING = '" + wheresSring + "',DOMAINTYPE = '" + domainType + "',DOMAINVALUE = '" + DOMAINVALUE + "' where id = " + selectedId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBATTR (PARENTID,NAME,STEP,CHECKTYPE,TABLENAME,SHOWFIELD,FIELD,SUPTABLE,WHERESTRING,DOMAINTYPE,DOMAINVALUE) VALUES('" + selectedId + "','" + name + "','" + checkNo + "','值域检查','" + table + "','" + showfield + "','" + selectField + "','" + domainTable + "','" + wheresSring + "','" + domainType + "','" + DOMAINVALUE + "')");
                }
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            else if (checkType == "唯一值检查")
            {
                string uniqueField = formUniqueDig.textCheckedValue;
                Boolean result;
                if (type == "edit")
                {
                    result = db.Update("update GISDATA_TBATTR set NAME ='" + name + "',CHECKTYPE = '唯一值检查',TABLENAME = '" + table + "',SHOWFIELD = '" + showfield + "',FIELD = '" + uniqueField + "' where id = " + selectedId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBATTR (PARENTID,NAME,STEP,CHECKTYPE,TABLENAME,SHOWFIELD,FIELD) VALUES('" + selectedId + "','" + name + "','" + checkNo + "','唯一值检查','" + table + "','" + showfield + "','" + uniqueField + "')");
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else if (checkType == "逻辑关系检查")
            {
                FormLogic formLogic = new FormLogic();
                string whereString = formLogicDig.textBoxWhereValue;
                string resultString = formLogicDig.textBoxResultValue;
                string fieldList = this.FIeldList.SelectedItems.ToString();
                Boolean result;
                if (type == "edit")
                {
                    result = db.Update("update GISDATA_TBATTR set NAME ='" + name + "',CHECKTYPE = '逻辑关系检查',TABLENAME = '" + table + "',SHOWFIELD = '" + showfield + "',WHERESTRING = '" + whereString + "',RESULT = '" + resultString + "' where id = " + selectedId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBATTR (PARENTID,NAME,STEP,CHECKTYPE,TABLENAME,SHOWFIELD,WHERESTRING,RESULT) VALUES('" + selectedId + "','" + name + "','" + checkNo + "','逻辑关系检查','" + table + "','" + showfield + "','" + whereString + "','" + resultString + "')");
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
