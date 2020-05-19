using DevExpress.XtraTreeList.Nodes;
using GISData.Common;
using GISData.Parameter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckBegin
{
    public partial class FormBegin : Form
    {
        public FormBegin()
        {
            InitializeComponent();
            bindZqTree();
        }

        private void FormBegin_Load(object sender, EventArgs e)
        {
            
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
            else 
            {
                Boolean dhhm = System.Text.RegularExpressions.Regex.IsMatch(this.textBoxlxdh.Text, @"^(\d{3,4}-)?\d{6,8}$");
                Boolean sjhm = System.Text.RegularExpressions.Regex.IsMatch(this.textBoxlxdh.Text, @"^[1]+[3,5]+\d{9}");
                if (dhhm || sjhm)
                {
                    string lxr = this.textBoxlxr.Text;
                    string lxdh = this.textBoxlxdh.Text;

                    CommonClass common = new CommonClass();
                    common.SetConfigValue("GLDW", gldwstr);
                    string datanow = DateTime.Now.ToLocalTime().ToString();
                    ConnectDB cdb = new ConnectDB();
                    DataTable dt = cdb.GetDataBySql("select STARTTIME from GISDATA_GLDW WHERE GLDW = '" + gldwstr + "'");
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
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("电话号码填写有误！");
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFun()) 
            {
                FormMain main = new FormMain();
                main.ShowSetpara();
            }
        }
    }
}
