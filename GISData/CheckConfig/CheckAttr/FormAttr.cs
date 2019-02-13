using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.ChekConfig
{
    public partial class FormAttr : Form
    {
        public delegate void DelegateRefreshTree();
        public delegate void DelegateRefreshTopo();
        public FormAttr()
        {
            InitializeComponent();
        }

        private void 添加分组_Click(object sender, EventArgs e)
        {
            DelegateRefreshTree RefreshTree = new DelegateRefreshTree(bindtreeViewAttr);
            //FormGroup FormGroupDig = new FormGroup(this.treeViewAttr, RefreshTree);
            //FormGroupDig.Show();
            this.treeViewAttr.Refresh();
        }
        private void bindtreeViewAttr()
        {
            this.treeViewAttr.Nodes.Clear();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_TBATTR");
            try
            {
                DataRow[] dr = dt.Select("PARENTID = 0");
                for (int i = 0; i < dr.Length; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr[i]["NAME"].ToString();
                    tn.Tag = dr[i]["id"].ToString();
                    FillTree(tn, dt);
                    treeViewAttr.Nodes.Add(tn);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("数据库出错:" + e.Message));
            }
        }
        private void FillTree(TreeNode node, DataTable dt)
        {
            DataRow[] drr = dt.Select("PARENTID='" + node.Tag.ToString() + "'");
            if (drr.Length > 0)
            {
                for (int i = 0; i < drr.Length; i++)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = drr[i]["NAME"].ToString();
                    tnn.Tag = drr[i]["id"].ToString();
                    if (drr[i]["PARENTID"].ToString() == node.Tag.ToString())
                    {
                        FillTree(tnn, dt);
                    }
                    node.Nodes.Add(tnn);
                }
            }
        }

        private void FormAttr_Load(object sender, EventArgs e)
        {
            bindtreeViewAttr();
        }

        private void 添加项_Click(object sender, EventArgs e)
        {
            DelegateRefreshTree RefreshTree = new DelegateRefreshTree(bindtreeViewAttr);
            FormAttrAdd formAttr = new FormAttrAdd();
            formAttr.Show();
        }
    }
}
