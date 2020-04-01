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
        private string gldw = "";
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
            DataTable dt = cdb.GetDataBySql("select ID,L_PARID,C_NAME,C_CODE from GISDATA_ZQSJZD");
            this.treeList1.DataSource = dt;
            treeList1.OptionsView.ShowCheckBoxes = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFun();
        }

        private void saveFun() 
        {
            TreeListNodes selectNode = this.treeList1.Nodes;
            string gldwstr = this.GetGldw(selectNode);
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
                    DataTable dt = cdb.GetDataBySql("select count(*) as isexists from GISDATA_CHECKTASK WHERE GLDW = '" + gldwstr + "'");
                    DataRow[] dr = dt.Select(null);
                    string exists = dr[0]["isexists"].ToString();
                    if (exists == "0")
                    {
                        cdb.Insert("insert into GISDATA_CHECKTASK (GLDW,LXR,LXDH,INSERTDATA) VALUES ('" + gldwstr + "','" + lxr + "','" + lxdh + "','" + datanow + "')");
                    }
                    else
                    {
                        cdb.Update("Update GISDATA_CHECKTASK set LXR = '" + lxr + "',UPDATEDATA='" + datanow + "',LXDH='" + lxdh + "' where GLDW = '" + gldwstr + "'");
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("电话号码填写有误！");
                }
            }
        }

        private string GetGldw(TreeListNodes selectNode) 
        {
            foreach (TreeListNode node in selectNode) 
            {
                if (node.Checked) 
                {
                    DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                    this.gldw = nodeData["C_CODE"].ToString();
                    break;
                }else if (node.Nodes.Count != 0)
                {
                    GetGldw(node.Nodes);
                }
            }
            return this.gldw;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFun();
            FormMain main = new FormMain();
            main.ShowSetpara();
        }
    }
}
