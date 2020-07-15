using DevExpress.XtraTreeList.Nodes;
using GISData.Common;
using GISData.Parameter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckBegin
{
    public partial class FormBegin : Form
    {
        public FormBegin()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            bindZqTree();
        }

        private void FormBegin_Load(object sender, EventArgs e)
        {
            CommonClass common = new CommonClass();
            this.textBox1.Text = common.GetConfigValue("SAVEDIR");
        }

        private void bindZqTree() 
        {
            ConnectDB cdb = new ConnectDB();
            DataTable dt = cdb.GetDataBySql("select * from GISDATA_GLDW");
            this.comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "GLDWNAME";
            comboBox1.ValueMember = "GLDW";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFun();
        }

        private Boolean saveFun() 
        {
            Boolean result = false;
            string gldwstr = this.comboBox1.SelectedValue.ToString();
            if (gldwstr == "" || gldwstr == null)
            {
                MessageBox.Show("请选择管理单位！");
            }
            else if (this.textBoxlxr.Text == "" || this.textBoxlxr.Text == null)
            {
                MessageBox.Show("请填写联系人！");
            }
            else if (this.textBox1.Text == "" || this.textBox1.Text == null)
            {
                MessageBox.Show("请选择成果目录！");
            }
            else 
            {
                Regex rx = new Regex(@"^[1]+[3,5,8]+\d{9}$");
                //Boolean dhhm = System.Text.RegularExpressions.Regex.IsMatch(this.textBoxlxdh.Text, @"^(\d{3,4}-)?\d{6,8}$");
                //Boolean sjhm = System.Text.RegularExpressions.Regex.IsMatch(this.textBoxlxdh.Text, @"^[1]+[3,5]+\d{9}");
                if (rx.IsMatch(this.textBoxlxdh.Text))
                {
                    string lxr = this.textBoxlxr.Text;
                    string lxdh = this.textBoxlxdh.Text;

                    CommonClass common = new CommonClass();
                    common.SetConfigValue("GLDW", gldwstr);
                    common.SetConfigValue("SAVEDIR", this.textBox1.Text);
                    string datanow = DateTime.Now.ToLocalTime().ToString();
                    ConnectDB cdb = new ConnectDB();
                    DataTable dt = cdb.GetDataBySql("select STARTTIME,GLDWNAME from GISDATA_GLDW WHERE GLDW = '" + gldwstr + "'");
                    DataRow[] dr = dt.Select(null);
                    string exists = dr[0]["STARTTIME"].ToString();
                    if (exists == "")
                    {
                        result = cdb.Update("Update GISDATA_GLDW set CONTACTS = '" + lxr + "',STARTTIME='" + datanow + "',TEL='" + lxdh + "' where GLDW = '" + gldwstr + "'");
                    }
                    else
                    {
                        result = cdb.Update("Update GISDATA_GLDW set CONTACTS = '" + lxr + "',TEL='" + lxdh + "',CHECKLOG = concat(CHECKLOG,'" + datanow + "',CHAR(10)) where GLDW = '" + gldwstr + "'");
                    }
                    CreateSaveForder(this.textBox1.Text, dr[0]["GLDWNAME"].ToString());
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("电话号码填写有误！");
                }
            }
            return result;
        }

        private void CreateSaveForder(string path,string gldw) 
        {
            if (!Directory.Exists(path))//  
                Directory.CreateDirectory(path);
            if (!Directory.Exists(path + "\\" + gldw))//  
                Directory.CreateDirectory(path + "\\" + gldw);
            if (!Directory.Exists(path + "\\" + gldw + "\\表格"))//  
                Directory.CreateDirectory(path + "\\" + gldw + "\\表格");
            if (!Directory.Exists(path + "\\" + gldw + "\\数据库"))//  
                Directory.CreateDirectory(path + "\\" + gldw + "\\数据库");
            if (!Directory.Exists(path + "\\" + gldw + "\\错误参考"))//  
                Directory.CreateDirectory(path + "\\" + gldw + "\\错误参考");
            if (!Directory.Exists(path + "\\" + gldw + "\\错误参考\\拓扑错误"))  
                Directory.CreateDirectory(path + "\\" + gldw + "\\错误参考\\拓扑错误");
            if (!Directory.Exists(path + "\\" + gldw + "\\错误参考\\属性错误"))
                Directory.CreateDirectory(path + "\\" + gldw + "\\错误参考\\属性错误");
            if (!Directory.Exists(path + "\\" + gldw + "\\文档"))//  
                Directory.CreateDirectory(path + "\\" + gldw + "\\文档");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFun()) 
            {
                FormMain main = new FormMain();
                main.ShowSetpara();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择成果保存文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                this.textBox1.Text = dialog.SelectedPath;
            }
        }
    }
}
