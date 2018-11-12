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

namespace GISData.Dictionary
{
    public partial class FormDictionary : Form
    {
        public FormDictionary()
        {
            InitializeComponent();
        }

        private void comboBoxDic_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeViewDic.Nodes.Clear();
            ConnectDB connectDB = new ConnectDB();
            DataTable dt;
            DataRow[] dr;
            if (this.comboBoxDic.SelectedItem == "政区数据字典") 
            {
                dt = connectDB.GetDataBySql("select * from GISDATA_ZQSJZD");
                dr = dt.Select("ID=1");
                for (int i = 0; i < dr.Length; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr[i]["C_ZQNAME"].ToString();
                    tn.Tag = dr[i]["ID"].ToString();
                    FillTree(tn, dt,"ID","C_ZQNAME");
                    treeViewDic.Nodes.Add(tn);
                }
            }else
            {
                dt = connectDB.GetDataBySql("select * from GISDATA_ZYSJZD");
                dr = dt.Select("L_PARID=0");
                for (int i = 0; i < dr.Length; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr[i]["C_NAME"].ToString();
                    tn.Tag = dr[i]["L_ID"].ToString();
                    FillTree(tn, dt,"L_ID","C_NAME");
                    treeViewDic.Nodes.Add(tn);
                }
            }
            
            
            
        }

        private void FormDictionary_Load(object sender, EventArgs e)
        {

        }

        private void FillTree(TreeNode node, DataTable dt,string code,string name)
        {
            DataRow[] drr = dt.Select("L_PARID='" + node.Tag.ToString() + "'");
            if (drr.Length > 0)
            {
                for (int i = 0; i < drr.Length; i++)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = drr[i][name].ToString();
                    tnn.Tag = drr[i][code].ToString();
                    if (drr[i]["L_PARID"].ToString() == node.Tag.ToString())
                    {
                        FillTree(tnn, dt,code,name);
                    }
                    node.Nodes.Add(tnn);
                }
            }
        }
    }
}
