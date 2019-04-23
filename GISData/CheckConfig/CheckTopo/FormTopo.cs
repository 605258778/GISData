using GISData.CheckConfig.CheckTopo.CheckDialog;
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

namespace GISData.ChekConfig.CheckTopo
{
    public partial class FormTopo : Form
    {
        private FormContainPoint ContainPoint;
        private FormNoInterLine NoInterLine;
        private FormNoOverlapArea NoOverlapArea;
        private Formxbm xbmDig;
        private FormNoGaps NoGaps;
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
            else if (checkType == "跨图层面重叠")
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
            else if (checkType == "面缝隙")
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
            
        }

        private void refushTable() 
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select NAME AS 质检项,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT from GISDATA_TBTOPO ");
            this.dataGridViewCheck.DataSource = dt;
            if (this.dataGridViewCheck.Columns.Count > 0)
            {
                this.dataGridViewCheck.Columns[1].Visible = false;
                this.dataGridViewCheck.Columns[2].Visible = false;
                this.dataGridViewCheck.Columns[3].Visible = false;
                this.dataGridViewCheck.Columns[4].Visible = false;
                this.dataGridViewCheck.Columns[5].Visible = false;
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
                result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT) VALUES('" + name + "','" + type + "','" + table + "','" + where + "','" + supTable + "','" + inputText + "')");
            }
            else if (checkType == "面和线不相交")
            {
                FormNoInterLine formNoInterLine = new FormNoInterLine();
                string supTable = NoInterLine.comboBoxLineValue;
                result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,SUPTABLE) VALUES('" + name + "','" + type + "','" + table + "','" + supTable + "')");
            }
            else if (checkType == "跨图层面重叠")
            {
                FormNoOverlapArea formNoOverlapArea = new FormNoOverlapArea();
                string supTable = NoOverlapArea.comboBoxOverLayerValue;
                result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,SUPTABLE) VALUES('" + name + "','" + type + "','" + table + "','" + supTable + "')");
            }
            else if (checkType == "细碎面")
            {
                Formxbm formxbm = new Formxbm();
                string where = xbmDig.textBoxwhereValue;
                string input = xbmDig.textBoxinputValue;
                result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,WHERESTRING,INPUTTEXT) VALUES('" + name + "','" + type + "','" + table + "','" + where + "','" + input + "')");
            }
            else if (checkType == "面缝隙")
            {
                FormNoGaps formgaps = new FormNoGaps();
                string input = NoGaps.textBoxareaValue;
                result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME,INPUTTEXT) VALUES('" + name + "','" + type + "','" + table + "','" + input + "')");
            }
            else 
            {
                result = db.Insert("insert into GISDATA_TBTOPO (NAME,TYPE,TABLENAME) VALUES('" + name + "','" + type + "','" + table + "')");
            }
            if (result)
            {
                refushTable();
            }
        }
    }
}
