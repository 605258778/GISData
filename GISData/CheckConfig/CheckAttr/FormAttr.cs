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
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
namespace GISData.ChekConfig
{
    public partial class FormAttr : Form
    {
        public delegate void DelegateRefreshTree();
        public delegate void DelegateRefreshTopo();
        private string selectedId;
        public int checkNo;
        public string scheme;
        private List<string> DeleteList = new List<string>();
        List<TreeListNode> DeleteNodes = new List<TreeListNode>();
        public FormAttr()
        {
            InitializeComponent();
        }
        public FormAttr(int No,string scheme)
        {
            InitializeComponent();
            this.checkNo = No;
            this.scheme = scheme;
        }

        private void 添加分组_Click(object sender, EventArgs e)
        {
            selectedId = (treeList1.FocusedNode != null  && treeList1.FocusedNode.Checked) ? treeList1.FocusedNode.GetValue("ID").ToString(): "0";
            TreeListNode selectNode = treeList1.FocusedNode;
            FormGroup formGroup = new FormGroup(selectedId, this.scheme, this.checkNo);
            formGroup.ShowDialog();
            if (formGroup.DialogResult == DialogResult.OK)
            {
                this.bindtreeViewAttr();//重新绑定
            }
        }
        public void bindtreeViewAttr()
        {
            ConnectDB cdb = new ConnectDB();
            DataTable dt = cdb.GetDataBySql("select * from GISDATA_TBATTR");
            this.treeList1.DataSource = dt;
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "PARENTID";
            if (treeList1.Columns.Count > 0) 
            {
                treeList1.Columns["NAME"].Caption = "质检名称";
                treeList1.Columns["JB"].Visible = false;
                treeList1.Columns["STEP_NO"].Visible = false;
                treeList1.Columns["CHECKTYPE"].Visible = false;
                treeList1.Columns["SHOWFIELD"].Visible = false;
                treeList1.Columns["FIELD"].Visible = false;
                treeList1.Columns["TABLENAME"].Visible = false;
                treeList1.Columns["SUPTABLE"].Visible = false;
                treeList1.Columns["SELECT"].Visible = false;
                treeList1.Columns["WHERESTRING"].Visible = false;
                treeList1.Columns["RESULT"].Visible = false;
                treeList1.Columns["BEIZHU"].Visible = false;
                treeList1.Columns["DOMAINTYPE"].Visible = false;
                treeList1.Columns["DOMAINVALUE"].Visible = false;
                treeList1.Columns["ISCHECK"].Visible = false;
                treeList1.Columns["ERROR"].Visible = false;
                treeList1.Columns["SCHEME"].Visible = false;
                treeList1.Columns["NAME"].OptionsColumn.AllowEdit = false;
            }
            treeList1.OptionsView.ShowCheckBoxes = true;
            treeList1.OptionsBehavior.AllowIndeterminateCheckState = false;
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
            selectedId = treeList1.FocusedNode == null ? "0" : treeList1.FocusedNode.GetValue("ID").ToString();
            TreeListNode selectNode = treeList1.FocusedNode;
            DelegateRefreshTree RefreshTree = new DelegateRefreshTree(bindtreeViewAttr);
            FormAttrAdd formAttr = new FormAttrAdd(checkNo, selectNode,"add",this.scheme,this.treeList1);
            formAttr.ShowDialog();
            if (formAttr.DialogResult == DialogResult.OK)
            {
                //this.bindtreeViewAttr();//重新绑定
            }
        }

        private void 属性编辑_Click(object sender, EventArgs e)
        {
             TreeListNode selectNode =  treeList1.FocusedNode;
             FormAttrAdd formAttr = new FormAttrAdd(checkNo, selectNode, "edit", this.scheme,this.treeList1);
             formAttr.ShowDialog();
        }

        private void 属性删除_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗?", "删除数据",MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DeleteNodes.Clear();
                DeleteList.Clear();
                ConnectDB db = new ConnectDB();
                TreeListNodes selectNode = this.treeList1.Nodes;
                deleteNode(selectNode);
                string idstr = string.Join(",", DeleteList.ToArray());
                Boolean result = db.Delete("delete from GISDATA_TBATTR where id in (" + idstr+")");
                if (result) 
                {
                    foreach (TreeListNode itemNode in DeleteNodes)
                    {
                        this.treeList1.DeleteNode(itemNode);
                    }
                }
            }
        }

        private void deleteNode(TreeListNodes selectNode) 
        {
            ConnectDB db = new ConnectDB();
            foreach (TreeListNode node in selectNode)
            {
                DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                if (node.Checked || node.CheckState == CheckState.Indeterminate)
                {
                    if (node.Checked) 
                    {
                        DataRowView itemNodeRow = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                        DeleteList.Add(itemNodeRow["ID"].ToString());
                        DeleteNodes.Add(node);
                    }
                    if (node.Nodes.Count != 0)
                    {
                        deleteNode(node.Nodes);
                    }
                }
            }
        }


        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            CommonClass common = new CommonClass();
            common.SetCheckedChildNodes(e.Node, e.Node.CheckState);
            common.SetCheckedParentNodes(e.Node, e.Node.CheckState);
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

        private void treeList1_DragDrop(object sender, DragEventArgs e)
        {
            TreeListNode targetNode = GetNodeByLocation(this.treeList1, new Point(e.X, e.Y));
            TreeListNode dragNode = this.treeList1.FocusedNode;
            DataRowView targetNodeRow = this.treeList1.GetDataRecordByNode(targetNode) as DataRowView;
            DataRowView dragNodeRow = this.treeList1.GetDataRecordByNode(dragNode) as DataRowView;
            ConnectDB db = new ConnectDB();
            db.Update("update   GISDATA_TBATTR SET PARENTID =" + targetNodeRow["id"] + " where ID =" + dragNodeRow["id"]);
        }
         /// <summary>
        ///  根据鼠标位置获取节点
        /// </summary>
        /// <param name="treeList">节点所在的treelist</param>
        /// <param name="location">节点位置</param>
        /// <returns></returns>
        private TreeListNode GetNodeByLocation(TreeList treeList, Point location)
        {
            TreeListHitInfo hitInfo = treeList.CalcHitInfo(treeList.PointToClient(location));
            return hitInfo.Node;
        }
    }
}
