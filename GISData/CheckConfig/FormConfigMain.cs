using GISData.CheckConfig;
using GISData.CheckConfig.CheckStructure;
using GISData.ChekConfig.CheckTopo;
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

namespace GISData.ChekConfig
{
    public partial class FormConfigMain : Form
    {
        public int dbSTEP_NO;
        public int click_NO;
        public FormConfigMain()
        {
            InitializeComponent();
            BindScheme();
        }

        private void FormConfigMain_Load(object sender, EventArgs e)
        {
            setMaxNO();
            loadStep();
        }

        private void setMaxNO() 
        {
            string scheme = this.comboBoxScheme.Text.ToString();
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select max(STEP_NO) as STEPNO from GISDATA_CONFIGSTEP where SCHEME = '"+scheme+"'");
            DataRow[] dr = result.Select("1=1");
            string a = dr[0]["STEPNO"].ToString();
            if (a == "")
            {
                dbSTEP_NO = 0;
            }
            else
            {
                dbSTEP_NO = int.Parse(a);
            }
        }

        /// <summary>
        /// 加载质检方案数据源
        /// </summary>
        private void BindScheme() 
        {
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select * from GISDATA_SCHEME order by IS_DEFAULT desc");
            comboBoxScheme.DataSource = result;
            comboBoxScheme.DisplayMember = "SCHEME_NAME";
            comboBoxScheme.ValueMember = "SCHEME_NAME";
        }

        //新增步骤
        private void buttonAddStep_Click(object sender, EventArgs e)
        {
            if (this.comboBoxScheme.Text.ToString() != "") 
            {
                ButtonEx btn = new ButtonEx();
                dbSTEP_NO += 1;
                btn.Name = dbSTEP_NO.ToString();
                btn.Text = "第" + dbSTEP_NO.ToString() + "步";
                ConnectDB db = new ConnectDB();
                Boolean result = db.Insert("insert into GISDATA_CONFIGSTEP (STEP_NO,SCHEME) values (" + dbSTEP_NO + ",'" + this.comboBoxScheme.Text.ToString() + "')");
                if (result)
                {
                    btn.Size = new Size(this.splitContainer2.Panel1.Width - 5, 40);
                    btn.Location = new Point(2, 20 + (dbSTEP_NO - 1) * 40);
                    btn.DoubleClick += new EventHandler(aBtn_DbClick);
                    btn.Tag = 0;
                    this.splitContainer3.Panel2.Controls.Add(btn);
                }
            }
        }

        public void aBtn_DbClick(object sender, EventArgs e)
        {
            //ShowForm();
            ButtonEx sendButton = (ButtonEx)sender;
            this.click_NO = int.Parse(sendButton.Name);
            if (sendButton.Tag.ToString() != "1")
            {
                FormStep fs = new FormStep(this);
                fs.Show();
            }
            else if (sendButton.AccessibleDescription.ToString() == "结构检查")
            {
                FormStructure fs = new FormStructure(this.click_NO);
                Panel panel = this.splitContainer2.Panel2;
                ShowForm(panel, fs);
            }
            else if (sendButton.AccessibleDescription.ToString() == "数据排查")
            {

            }
            else if (sendButton.AccessibleDescription.ToString() == "变化提取")
            {

            }
            else if (sendButton.AccessibleDescription.ToString() == "自动计算")
            {

            }
            else if (sendButton.AccessibleDescription.ToString() == "属性检查")
            {
                FormAttr fad = new FormAttr(this.click_NO);
                Panel panel = this.splitContainer2.Panel2;
                ShowForm(panel, fad);
            }
            else if (sendButton.AccessibleDescription.ToString() == "图形检查")
            {
                FormTopo fad = new FormTopo();
                Panel panel = this.splitContainer2.Panel2;
                ShowForm(panel, fad);
            }
            else if (sendButton.AccessibleDescription.ToString() == "统计报表")
            {

            }
        }

        
        /// <summary>
        /// 加载质检步骤
        /// </summary>
        public void loadStep() 
        {
            this.splitContainer3.Panel2.Controls.Clear();
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select * from GISDATA_CONFIGSTEP where SCHEME ='"+this.comboBoxScheme.Text.ToString()+"' order by STEP_NO");
            DataRow[] dr = result.Select("1=1");
            for (int i = 0; i < dr.Length; i++)
            {
                string stepNo = dr[i]["STEP_NO"].ToString();
                string stepName = dr[i]["STEP_NAME"].ToString();
                string stepType = dr[i]["STEP_TYPE"].ToString();
                string isConfig = dr[i]["IS_CONFIG"].ToString();
                ButtonEx btn = new ButtonEx();
                btn.Name = stepNo;
                btn.AccessibleDescription = stepType;//AccessibleDescription属性暂赋值为质检类型
                if (isConfig == "1")
                {
                    btn.Text = "第" + stepNo + "步(" + stepName + ")";
                    btn.Tag = 1;
                }
                else 
                {
                    btn.Text = "第" + stepNo + "步";
                    btn.Tag = 0;
                }
                btn.Size = new Size(this.splitContainer2.Panel1.Width - 5, 40);
                btn.Location = new Point(2, 20 + (int.Parse(stepNo) - 1) * 40);
                btn.DoubleClick += new EventHandler(aBtn_DbClick);
                this.splitContainer3.Panel2.Controls.Add(btn);
            }
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
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

        private void AddScheme_Click(object sender, EventArgs e)
        {
            FormAddScheme schem = new FormAddScheme();
            schem.ShowDialog();
            if (schem.DialogResult == DialogResult.OK)
            {
                BindScheme();
                //this.bindtreeViewAttr();//重新绑定
            }
        }

        private void comboBoxScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMaxNO();
            loadStep();
        }
    }
}
