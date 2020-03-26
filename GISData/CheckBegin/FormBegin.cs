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
            DataTable dt = cdb.GetDataBySql("select ID,L_PARID,C_NAME from GISDATA_ZQSJZD");
            this.treeList1.DataSource = dt;
            treeList1.OptionsView.ShowCheckBoxes = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFun();
            this.Close();
        }

        private void saveFun() 
        {
            TreeListNodes selectNode = this.treeList1.Nodes;
            string gldwstr = this.GetGldw(selectNode);
            string lxr = this.textBoxlxr.Text;
            string lxdh = this.textBoxlxdh.Text;

            string datanow = DateTime.Now.ToLocalTime().ToString();
            ConnectDB cdb = new ConnectDB();
            DataTable dt = cdb.GetDataBySql("select count(*) as isexists from GISDATA_CHECKTASK WHERE GLDW = '"+gldwstr+"'");
            DataRow[] dr = dt.Select(null);
            string exists = dr[0]["isexists"].ToString();
            if (exists == "0")
            {
                cdb.Insert("insert into GISDATA_CHECKTASK (GLDW,LXR,LXDH,INSERTDATA) VALUES ('" + gldwstr + "','" + lxr + "','" + lxdh + "','"+datanow+"')");
            }
            else 
            {
                cdb.Update("Update GISDATA_CHECKTASK set LXR = '" + lxr + "',UPDATEDATA='" + datanow + "',LXDH='" + lxdh + "' where GLDW = '" + gldwstr + "'");
            }
        }

        private string GetGldw(TreeListNodes selectNode) 
        {
            foreach (TreeListNode node in selectNode) 
            {
                if (node.Checked) 
                {
                    DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                    this.gldw = nodeData["C_NAME"].ToString();
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
            this.Close();
            FormMain main = new FormMain();
            main.ShowSetpara();
        }
    }
}
