using GISData.CheckConfig.CheckTopo.CheckDialog;
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
        private Form currentForm;
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
            this.IsMdiContainer = true;
            this.splitContainer2.Panel2.Controls.Clear();
            string checkType = comboBoxCheckType.SelectedItem.ToString();
            if (checkType == "面内包含点个数")
            {
                FormContainPoint formContainPoint = new FormContainPoint();
                ShowForm(this.splitContainer2.Panel2, formContainPoint);
            }
            else if (checkType == "面和线不相交")
            {
                FormNoInterLine formNoInterLine = new FormNoInterLine();
                ShowForm(this.splitContainer2.Panel2, formNoInterLine);
            }
            else if (checkType == "跨图层面重叠")
            {
                FormNoOverlapArea formNoOverlapArea = new FormNoOverlapArea();
                ShowForm(this.splitContainer2.Panel2, formNoOverlapArea);
            }
            else if (checkType == "细碎面")
            {
                Formxbm formxbm = new Formxbm();
                ShowForm(this.splitContainer2.Panel2, formxbm);
            }
        }

        public void ShowForm(Panel panel, Form frm)
         {
            lock (this)
            {
                try
                {
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
