using DevExpress.XtraTreeList.Nodes;
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
        private CheckBox checkBox;
        Boolean flag = true;
        public FormAttrDia()
        {
            InitializeComponent();
        }

        public void SelectAll() 
        {
            this.SelectTreeListAll(this.treeList1);
        }
        public void UnSelectAll()
        {
            this.UnSelectTreeListAll(this.treeList1);
        }
        public FormAttrDia(string stepNo,CheckBox cb)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
            bindTreeView();
        }

        private void FormAttrDia_Load(object sender, EventArgs e)
        {
            bindTreeView();
        }

        private void bindTreeView() 
        {
            ConnectDB cdb = new ConnectDB();
            DataTable dt = cdb.GetDataBySql("select ID,PARENTID,NAME,ISCHECK,ERROR from GISDATA_TBATTR");
            this.treeList1.DataSource = dt;
            treeList1.OptionsView.ShowCheckBoxes = true;
            //treeList1.OptionsBehavior.AllowIndeterminateCheckState = true;
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

        private void treeList1_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            if (e.PrevState == CheckState.Checked)
            {
                e.State = CheckState.Unchecked;
            }
            else
            {
                e.State = CheckState.Checked;
            }
        }

        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            CommonClass common = new CommonClass();
            common.SetCheckedChildNodes(e.Node, e.Node.CheckState);
            common.SetCheckedParentNodes(e.Node, e.Node.CheckState);
            flag = true;
            this.findOrigin(this.treeList1);
            if (flag) 
            {
                this.checkBox.CheckState = CheckState.Unchecked;
            }
        }

        private void findOrigin(DevExpress.XtraTreeList.TreeList tree,TreeListNodes nodes = null)
        {
            //this.checkBox.CheckState = CheckState.Unchecked;
            nodes = nodes ?? tree.Nodes;
            if (flag) 
            {
                foreach (TreeListNode item in nodes)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        this.checkBox.CheckState = CheckState.Checked;
                        flag = false;
                        break;
                    }
                    if (tree.HasChildren)
                    {
                        findOrigin(tree, item.Nodes);
                    }
                }
            }
        }

        /// <summary>
        /// 全选树
        /// </summary>
        /// <param name="tree">树控件</param>
        /// <param name="nodes">节点集合</param>
        public virtual void SelectTreeListAll(DevExpress.XtraTreeList.TreeList tree, TreeListNodes nodes = null)
        {
            nodes = nodes ?? tree.Nodes;
            foreach (TreeListNode item in nodes)
            {
                item.CheckState = CheckState.Checked;
                if (tree.HasChildren)
                {
                    SelectTreeListAll(tree, item.Nodes);
                }
            }
        }
        /// <summary>
        /// 全取消选树
        /// </summary>
        /// <param name="tree">树控件</param>
        /// <param name="nodes">节点集合</param>
        public virtual void UnSelectTreeListAll(DevExpress.XtraTreeList.TreeList tree, TreeListNodes nodes = null)
        {
            nodes = nodes ?? tree.Nodes;
            foreach (TreeListNode item in nodes)
            {
                item.CheckState = CheckState.Unchecked;
                if (tree.HasChildren)
                {
                    UnSelectTreeListAll(tree, item.Nodes);
                }
            }
        }

        private void treeList1_SelectionChanged(object sender, EventArgs e)
        {
        }

        public void doCheckAttr() 
        {
            
        }
    }
}
