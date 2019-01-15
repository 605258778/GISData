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
        private Form currentForm;
        public FormConfigMain()
        {
            InitializeComponent();
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select max(STEP_NO) as STEPNO from GISDATA_CONFIGSTEP");
            DataRow[] dr = result.Select("1=1");
            string a = dr[0]["STEPNO"].ToString();
            if (a == "")
            {
                dbSTEP_NO = 1;
            }
            else
            {
                dbSTEP_NO = int.Parse(a);
            }
        }
        //新增步骤
        private void buttonAddStep_Click(object sender, EventArgs e)
        {
            ButtonEx btn = new ButtonEx();
            btn.Name = "第" + dbSTEP_NO.ToString()+"步";
            btn.Text = "第" + dbSTEP_NO.ToString() + "步";
            btn.Size = new Size(this.splitContainer2.Panel1.Width-5, 40);
            btn.Location = new Point(2, this.buttonAddStep.Bottom+20+(dbSTEP_NO-1)*40);
            btn.DoubleClick += new EventHandler(aBtn_DbClick);
            this.splitContainer2.Panel1.Controls.Add(btn);
            dbSTEP_NO += 1;
        }

        public void aBtn_DbClick(object sender, EventArgs e)
        {
            ShowForm();
            //FormStep fs = new FormStep();
            //fs.Show(); 
        }

        private void FormConfigMain_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
        public void ShowForm()
        {
            lock (this)
            {
                try
                {
                    FormAttr fad = new FormAttr();
                    Panel panel = this.splitContainer2.Panel2;
                    Form frm = fad;
                    if (this.currentForm != null && this.currentForm == frm)
                    {
                        return;
                    }
                    else if (this.currentForm != null)
                    {
                        if (this.ActiveMdiChild != null)
                        {
                            this.ActiveMdiChild.Hide();
                        }
                    }
                    this.currentForm = frm;
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
    }
}
