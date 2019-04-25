using GISData.Common;
using GISData.DataCheck.CheckDialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataCheck
{
    public partial class FormCheckMain : Form
    {
        public FormCheckMain()
        {
            InitializeComponent();
        }

        private FormStructureDia StructureDia;
        private FormAttrDia AttrDia;
        private FormTopoDia TopoDia;
        private List<CheckBox> CheckBoxArr = new List<CheckBox>();
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void FormCheckMain_Load(object sender, EventArgs e)
        {
            loadStep();
        }
        /// <summary>
        /// 加载检查项
        /// </summary>
        public void loadStep()
        {
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select * from GISDATA_CONFIGSTEP where IS_CONFIG = '1' order by STEP_NO");
            DataRow[] dr = result.Select("1=1");
            for (int i = 0; i < dr.Length; i++)
            {
                string stepNo = dr[i]["STEP_NO"].ToString();
                string stepName = dr[i]["STEP_NAME"].ToString();
                string stepType = dr[i]["STEP_TYPE"].ToString();
                string isConfig = dr[i]["IS_CONFIG"].ToString();
                CheckBox cb = new CheckBox();
                cb.Text = "";
                TabPage tp = new TabPage();

                tp.Name = stepType;
                cb.Name = stepType;
                cb.Location = new Point(7, (i+1)*60-39);
                cb.Size = new Size(18, 18);
                tp.AccessibleDescription = stepType;//AccessibleDescription属性暂赋值为质检类型
                tp.SendToBack();
                tp.Text = "第" + stepNo + "步(" + stepName + ")";
                tp.Tag = stepNo;
                cb.Tag = stepNo;
                cb.Click += (se, a) => checkState(se,a,stepType);
                this.splitContainer1.Panel2.Controls.Add(cb);
                this.tabControl1.Controls.Add(tp);
                cb.BringToFront();
                this.CheckBoxArr.Add(cb);
                if (stepType == "结构检查")
                {
                    FormStructureDia fsa = new FormStructureDia(stepNo, cb);
                    StructureDia = fsa;
                    ShowForm(tp, fsa);
                }
                else if (stepType == "属性检查")
                {
                    FormAttrDia attr = new FormAttrDia(stepNo, cb);
                    AttrDia = attr;
                    ShowForm(tp, attr);
                }
                else if (stepType == "图形检查")
                {
                    FormTopoDia topo = new FormTopoDia(stepNo, cb);
                    TopoDia = topo;
                    ShowForm(tp, topo);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void checkState(object sender, EventArgs e,string type) 
        {
            CheckBox CheckBoxSender = (CheckBox)sender;
            if (type == "结构检查")
            {
                if (CheckBoxSender.Checked)
                {
                    StructureDia.SelectAll();
                }
                else {
                    StructureDia.UnSelectAll();
                }
               
            }
            else if (type == "属性检查")
            {
                if (CheckBoxSender.Checked)
                {
                    AttrDia.SelectAll();
                }
                else
                {
                    AttrDia.UnSelectAll();
                }
            }
            else if (type == "图形检查")
            {
                if (CheckBoxSender.Checked)
                {
                    TopoDia.SelectAll();
                }
                else
                {
                    TopoDia.UnSelectAll();
                }
            }
        }

        /// <summary>
        /// 重绘tabControl，使其竖向展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle tabArea = tabControl1.GetTabRect(e.Index);//主要是做个转换来获得TAB项的RECTANGELF
            RectangleF tabTextArea = (RectangleF)(tabControl1.GetTabRect(e.Index));
            tabTextArea.Location = new Point(30, int.Parse(tabTextArea.Top.ToString()));
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();//封装文本布局信息
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            Font font = this.tabControl1.Font;
            SolidBrush brush = new SolidBrush(Color.Black);//绘制边框的画笔
            g.DrawString(((TabControl)(sender)).TabPages[e.Index].Text, font, brush, tabTextArea, sf);
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
        public void ShowForm(TabPage page, Form frm)
        {
            lock (this)
            {
                try
                {
                    page.Controls.Clear();
                    frm.TopLevel = true;
                    frm.MdiParent = this;
                    frm.Parent = page;
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

        private void buttonCheckStar_Click(object sender, EventArgs e)
        {
            foreach (CheckBox item in CheckBoxArr)
            {
                if(item.CheckState == CheckState.Checked)
                {
                    if (item.Name == "结构检查")
                    {

                        StructureDia.doCheckStructure();
                    }
                    else if (item.Name == "属性检查")
                    {
                        AttrDia.doCheckAttr();
                    }
                    else if (item.Name == "图形检查")
                    {
                        TopoDia.doCheckTopo();
                    }
                }
            }
        }
    }
}
