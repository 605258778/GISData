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

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormAttrDia : Form
    {
        private string stepNo;

        public FormAttrDia()
        {
            InitializeComponent();
        }

        public FormAttrDia(string stepNo)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            bindTreeView();
        }

        private void FormAttrDia_Load(object sender, EventArgs e)
        {
            bindTreeView();
        }

        private void bindTreeView() 
        {
            this.treeView1.Nodes.Clear();
            ConnectDB cdb = new ConnectDB();
            CommonClass common = new CommonClass();
            try
            {
                DataTable dt = cdb.GetDataBySql("select * from GISDATA_TBATTR");
                DataRow[] dr = dt.Select("PARENTID = 0");

                TreeNode rootNode = new TreeNode();
                rootNode.Text = "属性检查";
                rootNode.Tag = "0";
                treeView1.Nodes.Add(rootNode);
                for (int i = 0; i < dr.Length; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr[i]["NAME"].ToString();
                    tn.Tag = dr[i]["ID"].ToString();
                    common.FillTree(tn, dt);
                    rootNode.Nodes.Add(tn);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("数据库出错:" + e.Message));
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CommonClass common = new CommonClass();
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    common.setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    common.setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        common.setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
        }
    }
}
