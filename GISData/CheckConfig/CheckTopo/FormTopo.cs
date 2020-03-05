﻿using GISData.CheckConfig.CheckTopo.CheckDialog;
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

namespace GISData.ChekConfig.CheckTopo
{
    public partial class FormTopo : Form
    {
        private FormContainPoint ContainPoint;
        private FormNoInterLine NoInterLine;
        private FormNoOverlapArea NoOverlapArea;
        private Formxbm xbmDig;
        private FormNoGaps NoGaps;
        private string SavaType;
        private string EditId;
        public FormTopo()
        {
            InitializeComponent();
        }
        /// <summary>
        ///    面内包含点个数
        ///    面多部件检查
        ///    面和线不相交
        ///    跨边界面不相交
        ///    跨图层面重叠
        ///    面图层自相交
        ///    面缝隙
        ///    面重叠
        ///    细碎面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCheckType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.splitContainer2.Panel2.Controls.Clear();
            if (comboBoxCheckType.SelectedItem == null) 
            {
                return;
            }
            string checkType = comboBoxCheckType.SelectedItem.ToString();
            if (checkType == "面内包含点个数")
            {
                FormContainPoint formContainPoint = new FormContainPoint();
                ContainPoint = formContainPoint;
                ShowForm(this.splitContainer2.Panel2, formContainPoint);
            }
            else if (checkType == "面和线不相交")
            {
                FormNoInterLine formNoInterLine = new FormNoInterLine();
                NoInterLine = formNoInterLine;
                ShowForm(this.splitContainer2.Panel2, formNoInterLine);
            }
            else if (checkType == "两图层面要素必须互相覆盖")
            {
                FormNoOverlapArea formNoOverlapArea = new FormNoOverlapArea();
                NoOverlapArea = formNoOverlapArea;
                ShowForm(this.splitContainer2.Panel2, formNoOverlapArea);
            }
            else if (checkType == "细碎面")
            {
                Formxbm formxbm = new Formxbm();
                xbmDig = formxbm;
                ShowForm(this.splitContainer2.Panel2, formxbm);
            }
            else if (checkType == "面要素之间无空隙")
            {
                FormNoGaps formgaps = new FormNoGaps();
                NoGaps = formgaps;
                ShowForm(this.splitContainer2.Panel2, formgaps);
            }
        }

        public void ShowForm(Panel panel, Form frm)
         {
            lock (this)
            {
                try
                {
                    frm.TopLevel = false;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    panel.Controls.Clear();
                    panel.Controls.Add(frm);
                    frm.Show();
                }
                catch (System.Exception ex)
                {
                    //
                }
            }
        }

        private void FormTopo_Load(object sender, EventArgs e)
        {
            refushTable();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO");
            comboBoxDataSource.DataSource = dt;
            comboBoxDataSource.DisplayMember = "REG_ALIASNAME";
            comboBoxDataSource.ValueMember = "REG_NAME";
            //this.comboBoxCheckType.Items.Add(new DictionaryEntry("面内包含点个数", "面内包含点个数"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("面要素无多部件", "多部件"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("面和线不相交", "面和线不相交"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("跨边界面不相交", "面不相交"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("两图层面要素必须互相覆盖", "两图层面要素必须互相覆盖"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("面图层自相交", "面图层自相交"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("面要素之间无空隙", "面要素之间无空隙"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("面要素间无重叠", "面要素间无重叠"));
            this.comboBoxCheckType.Items.Add(new DictionaryEntry("细碎面", "细碎面"));
            this.comboBoxCheckType.DisplayMember = "Value";
            this.comboBoxCheckType.ValueMember = "Key";
            
        }

        private void refushTable() 
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select NAME AS 质检项,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT,ID from GISDATA_TBTOPO ");
            this.dataGridViewCheck.DataSource = dt;
            if (this.dataGridViewCheck.Columns.Count > 0)
            {
                this.dataGridViewCheck.Columns[1].Visible = false;
                this.dataGridViewCheck.Columns[2].Visible = false;
                this.dataGridViewCheck.Columns[3].Visible = false;
                this.dataGridViewCheck.Columns[4].Visible = false;
                this.dataGridViewCheck.Columns[5].Visible = false;
                this.dataGridViewCheck.Columns[6].Visible = false;
            }
        }

        private void buttonTopoSave_Click(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            string checkType = comboBoxCheckType.SelectedItem.ToString();
            string name = textBoxName.Text;
            string table = comboBoxDataSource.SelectedValue.ToString();
            string type = comboBoxCheckType.Text;
            
            Boolean result;
            if (checkType == "面内包含点个数")
            {
                FormContainPoint formContainPoint = new FormContainPoint();
                string where = ContainPoint.textBoxWhereValue;
                string supTable = ContainPoint.comboBoxPointValue;
                string inputText = ContainPoint.textBoxNumPointValue;
                if (this.SavaType == "edit")
                {
                    result = db.Update("update GISDATA_TBTOPO set NAME= '" + name + "',TYPE = '" + type + "',TABLENAME = '" + table + "',WHERESTRING = '" + where + "',SUPTABLE='" + supTable + "',INPUTTEXT = '" + inputText + "' where ID = "+ this.EditId);
                }
                else 
                {
                    result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT) VALUES('" + name + "','" + type + "','" + table + "','" + where + "','" + supTable + "','" + inputText + "')");
                }
            }
            else if (checkType == "面和线不相交")
            {
                FormNoInterLine formNoInterLine = new FormNoInterLine();
                string supTable = NoInterLine.comboBoxLineValue;
                if (this.SavaType == "edit")
                {
                    result = db.Update("update GISDATA_TBTOPO set NAME= '" + name + "',TYPE = '" + type + "',TABLENAME = '" + table + "',SUPTABLE='" + supTable + "' where ID = " + this.EditId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,SUPTABLE) VALUES('" + name + "','" + type + "','" + table + "','" + supTable + "')");
                }
                
            }
            else if (checkType == "跨图层面重叠")
            {
                FormNoOverlapArea formNoOverlapArea = new FormNoOverlapArea();
                string supTable = NoOverlapArea.comboBoxOverLayerValue;
                if (this.SavaType == "edit")
                {
                    result = db.Update("update GISDATA_TBTOPO set NAME= '" + name + "',TYPE = '" + type + "',TABLENAME = '" + table + "',SUPTABLE='" + supTable + "' where ID = " + this.EditId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,SUPTABLE) VALUES('" + name + "','" + type + "','" + table + "','" + supTable + "')");
                }
                
            }
            else if (checkType == "细碎面")
            {
                Formxbm formxbm = new Formxbm();
                string where = xbmDig.textBoxwhereValue;
                string input = xbmDig.textBoxinputValue;
                if (this.SavaType == "edit")
                {
                    result = db.Update("update GISDATA_TBTOPO set NAME= '" + name + "',TYPE = '" + type + "',TABLENAME = '" + table + "',WHERESTRING='" + where + "',INPUTTEXT='" + input + "' where ID = " + this.EditId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,WHERESTRING,INPUTTEXT) VALUES('" + name + "','" + type + "','" + table + "','" + where + "','" + input + "')");
                }
            }
            else if (checkType == "面缝隙")
            {
                FormNoGaps formgaps = new FormNoGaps();
                string input = NoGaps.textBoxareaValue;
                if (this.SavaType == "edit")
                {
                    result = db.Update("update GISDATA_TBTOPO set NAME= '" + name + "',TYPE = '" + type + "',TABLENAME = '" + table + "',INPUTTEXT='" + input + "' where ID = " + this.EditId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,INPUTTEXT) VALUES('" + name + "','" + type + "','" + table + "','" + input + "')");
                }
            }
            else 
            {
                if (this.SavaType == "edit")
                {
                    result = db.Update("update GISDATA_TBTOPO set NAME= '" + name + "',TYPE = '" + type + "',TABLENAME = '" + table  + "' where ID = " + this.EditId);
                }
                else
                {
                    result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME) VALUES('" + name + "','" + type + "','" + table + "')");
                }
            }
            if (result)
            {
                refushTable();
            }
        }

        private void dataGridViewCheck_DoubleClick(object sender, EventArgs e)
        {
            this.SavaType = "edit";
            DataGridViewRow row = this.dataGridViewCheck.CurrentRow;
            this.EditId = row.Cells["ID"].Value.ToString();
            string NAME = row.Cells["质检项"].Value.ToString();
            string TYPE = row.Cells["TYPE"].Value.ToString();
            string TABLENAME = row.Cells["TABLENAME"].Value.ToString();
            string WHERESTRING = row.Cells["WHERESTRING"].Value.ToString();
            string SUPTABLE = row.Cells["SUPTABLE"].Value.ToString();
            string INPUTTEXT = row.Cells["INPUTTEXT"].Value.ToString();
            //string checkType = this.comboBoxCheckType.SelectedItem.ToString();
            textBoxName.Text = NAME;
            this.comboBoxCheckType.SelectedIndex = this.comboBoxCheckType.FindString(TYPE);
            foreach (DataRowView iTable in this.comboBoxDataSource.Items)
            {
                if (TABLENAME == iTable.Row[0].ToString()) 
                {
                    this.comboBoxDataSource.SelectedIndex = this.comboBoxDataSource.FindString((string)iTable.Row[1]);
                }
            }
            if (TYPE == "面内包含点个数")
            {
                FormContainPoint formContainPoint = new FormContainPoint();
                ContainPoint.textBoxWhereValue = WHERESTRING;
                ContainPoint.comboBoxPointValue = SUPTABLE;
                ContainPoint.textBoxNumPointValue = INPUTTEXT;
            }
            else if (TYPE == "面和线不相交")
            {
                FormNoInterLine formNoInterLine = new FormNoInterLine();
                NoInterLine.comboBoxLineValue = SUPTABLE;
            }
            else if (TYPE == "跨图层面重叠")
            {
                FormNoOverlapArea formNoOverlapArea = new FormNoOverlapArea();
                NoOverlapArea.comboBoxOverLayerValue = SUPTABLE;
            }
            else if (TYPE == "细碎面")
            {
                Formxbm formxbm = new Formxbm();
                xbmDig.textBoxwhereValue = WHERESTRING;
                xbmDig.textBoxinputValue = INPUTTEXT;
            }
            else if (TYPE == "面缝隙")
            {
                FormNoGaps formgaps = new FormNoGaps();
                NoGaps.textBoxareaValue = INPUTTEXT;
            }
            else
            {
            }
        }

        private void newStep_Click(object sender, EventArgs e)
        {
            this.SavaType = "insert";
            this.splitContainer2.Panel2.Controls.Clear();
            comboBoxCheckType.SelectedIndex = -1;
            textBoxName.Text = "";
            this.comboBoxDataSource.SelectedIndex = -1;
        }

        private void DeleteStep_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗?", "删除数据");
             if (dr == DialogResult.OK)
             {
                 DataGridViewRow row = this.dataGridViewCheck.CurrentRow;
                 string id = row.Cells["ID"].Value.ToString();
                 ConnectDB db = new ConnectDB();
                 db.Delete("delete from GISDATA_TBTOPO where id = " + id);
                 refushTable();
             }
            
        }
    }
}
