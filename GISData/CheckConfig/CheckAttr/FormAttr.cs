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
namespace GISData.ChekConfig
{
    public partial class FormAttr : Form
    {
        public delegate void DelegateRefreshTree();
        public delegate void DelegateRefreshTopo();
        private string selectedId;
        public int checkNo;
        public FormAttr()
        {
            InitializeComponent();
        }
        public FormAttr(int No)
        {
            InitializeComponent();
            checkNo = No;
        }

        private void 添加分组_Click(object sender, EventArgs e)
        {
            selectedId = treeList1.FocusedNode.GetValue("ID").ToString();
            TreeListNode selectNode = treeList1.FocusedNode;
            FormGroup formGroup = new FormGroup(selectNode);
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
            treeList1.Columns["NAME"].Caption = "质检名称";
            treeList1.Columns["JB"].Visible = false;
            treeList1.Columns["STEP"].Visible = false;
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
            treeList1.OptionsView.ShowCheckBoxes = false;
            treeList1.OptionsBehavior.AllowIndeterminateCheckState = false;
            treeList1.Columns["NAME"].OptionsColumn.AllowEdit = false;
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
            selectedId = treeList1.FocusedNode.GetValue("ID").ToString();
            TreeListNode selectNode = treeList1.FocusedNode;
            DelegateRefreshTree RefreshTree = new DelegateRefreshTree(bindtreeViewAttr);
            FormAttrAdd formAttr = new FormAttrAdd(checkNo, selectNode,"add");
            formAttr.ShowDialog();
            if (formAttr.DialogResult == DialogResult.OK)
            {
                this.bindtreeViewAttr();//重新绑定
            }
        }

        private void 属性编辑_Click(object sender, EventArgs e)
        {
             TreeListNode selectNode =  treeList1.FocusedNode;
             FormAttrAdd formAttr = new FormAttrAdd(checkNo, selectNode, "edit");
             formAttr.ShowDialog();
        }

        private void 属性删除_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗?", "删除数据");
            if (dr == DialogResult.OK)
            {
                string selectedId = treeList1.FocusedNode.GetValue("ID").ToString();
                ConnectDB db = new ConnectDB();
                Boolean result = db.Delete("delete from GISDATA_TBATTR where id = " + selectedId + " or PARENTID = '" + selectedId+"'");
                if (result)
                {
                    this.bindtreeViewAttr();//重新绑定
                    MessageBox.Show("删除成功！");
                }
            }
            
        }
    }
}
