﻿using GISData.ChekConfig.CheckDialog;
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
        public FormAttrAdd()
        {
            InitializeComponent();
        }

        private void FormAttrDig_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO");
            comboBoxDataSour.DataSource = dt;
            comboBoxDataSour.DisplayMember = "REG_ALIASNAME";
            comboBoxDataSour.ValueMember = "REG_NAME";
        }

        private void comboBoxDataSour_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxDataSour_SelectedValueChanged(object sender, EventArgs e)
        {
            string text = comboBoxDataSour.SelectedText.ToString();
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
                ShowForm(this.splitContainer3.Panel1, nullValue);
            }
            else if (checkType == "值域检查")
            {
                FormDomain formDomain = new FormDomain();
                ShowForm(this.splitContainer3.Panel1, formDomain);
            }
            else if (checkType == "唯一值检查")
            {
                FormUnique formUnique = new FormUnique();
                ShowForm(this.splitContainer3.Panel1, formUnique);
            }
            else if (checkType == "逻辑关系检查")
            {
                FormLogic formLogic = new FormLogic();
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
            if (checkType == "空值检查")
            {

            }
            else if (checkType == "值域检查")
            {

            }
            else if (checkType == "唯一值检查")
            {

            }
            else if (checkType == "逻辑关系检查")
            {

            }
        }
    }
}
