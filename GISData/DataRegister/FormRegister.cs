using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
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

namespace GISData.DataRegister
{
    public partial class FormRegister : Form
    {
        private string regName;
        Boolean flag = true;
        public FormRegister()
        {
            InitializeComponent();
        }

        private void buttonAddConnect_Click(object sender, EventArgs e)
        {
            FormDBConnectInfo fdci = new FormDBConnectInfo(this.treeViewReg);
            fdci.ShowDialog();
            if (fdci.DialogResult == DialogResult.OK)
            {
                this.refreshTreeViewReg();//重新绑定
            }
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            refreshTreeViewReg();
            refreshDataGridReg();
        }
        public void refreshDataGridField(string regName)
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select ID,FIELD_NAME AS 字段名,FIELD_ALSNAME AS 别名,DATA_TYPE AS 数据类型,MAXLEN AS 字段长度,CODE_PK AS 字典域,CODE_WHERE AS 字典域条件 from GISDATA_MATEDATA where REG_NAME = '" + regName+"'");
            this.dataGridViewFieldView.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
            this.gridView1.HorzScrollVisibility = ScrollVisibility.Always;
            if (this.gridView1.Columns.Count > 0)
            {
                this.gridView1.Columns[0].Visible = false;
            }
        }

        public void refreshDataGridReg()
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select REG_NAME,REG_ALIASNAME AS 注册名称 from GISDATA_REGINFO");
            this.dataGridViewDataReg.DataSource = dt;
            this.gridView2.OptionsBehavior.Editable = false;
            if (this.gridView2.Columns.Count > 0)
            {
                this.gridView2.Columns[0].Visible = false;
            }
        }
        
        //刷新注册连接树
        public void refreshTreeViewReg()
        {
            this.treeList1.ClearNodes();
            //加载注册数据连接
            DataTable dtable = new DataTable();
            dtable.Columns.Add("ParentID", typeof(string));
            dtable.Columns.Add("ID", typeof(string));
            dtable.Columns.Add("NAME", typeof(string));
            dtable.Columns.Add("PATH", typeof(string));
            dtable.Columns.Add("TYPE", typeof(string));
            ConnectDB cd = new ConnectDB();
            GetAllFeatures gaf = new GetAllFeatures();
            DataTable dt = cd.GetDataBySql("select * from GISDATA_REGCONNECT");
            DataRow[] dr = dt.Select("1=1");
            for (int i = 0; i < dr.Length; i++)
            {
                DataRow row = dtable.NewRow(); ;
                string regPath = dr[i]["REG_PATH"].ToString();
                string regType = dr[i]["REG_TYPE"].ToString();
                string name = dr[i]["REG_NAME"].ToString();
                string id = dr[i]["ID"].ToString();
                row["ParentID"] = "";
                row["ID"] = id;
                row["NAME"] = name;
                row["PATH"] = regPath;
                row["TYPE"] = regType;
                dtable.Rows.Add(row);
                IFeatureWorkspace space;
                if (regType == "Access数据库")
                {
                    AccessWorkspaceFactory fac = new AccessWorkspaceFactoryClass();
                    try
                    {
                        space = (IFeatureWorkspace)fac.OpenFromFile(regPath, 0);
                    }
                    catch 
                    {
                        continue;
                    }
                }
                else
                {
                    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                    try
                    {
                        space = (IFeatureWorkspace)fac.OpenFromFile(regPath, 0);
                    }
                    catch
                    {
                        continue;
                    }
                }
                IWorkspace iwk = (IWorkspace)space;
                List<IDataset> ifcList = gaf.GetAllFeatureClass(iwk);
                for(int j = 0; j < ifcList.Count; j++)
                {
                    DataRow rowKid = dtable.NewRow();
                    rowKid["ParentID"] = id;
                    rowKid["ID"] = int.Parse(id)*100+j;
                    rowKid["NAME"] = ifcList[j].Name;
                    rowKid["PATH"] = regPath;
                    rowKid["TYPE"] = regType;
                    dtable.Rows.Add(rowKid);
                }
            }
            this.treeList1.DataSource = dtable;
            this.treeList1.OptionsView.ShowCheckBoxes = true;
        }

        private void treeViewReg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            { 
            
            }
        }
        //添加注册连接
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定注册？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                TreeListNodes selectNode = this.treeList1.Nodes;
                Boolean boolFlag = RecursionNodes(selectNode);
                if (boolFlag)
                {
                    MessageBox.Show("注册成功！", "提示");
                    refreshDataGridReg();
                }
                else
                {
                    MessageBox.Show("注册失败！", "提示");
                }
            }
        }

        /// <summary>
        /// 遍历树节点
        /// </summary>
        private Boolean RecursionNodes(TreeListNodes Nodes)
        {
            ConnectDB cd = new ConnectDB();
            Boolean boolFlag = true;
            foreach (TreeListNode node in Nodes)
            {
                //// 如果当前节点下还包括子节点，就调用递归
                if (node.Nodes.Count > 0)
                {
                    RecursionNodes(node.Nodes);
                }
                else
                {
                    if (node.Checked&&!node.HasChildren)
                    {
                        DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                        
                        IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                        IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(nodeData["PATH"].ToString(), 0) as IFeatureWorkspace;
                        //打开数据文件中航路点这个表
                        IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(nodeData["NAME"].ToString());
                        string FeatureType = pFeatureClass.ShapeType.ToString();
                        Boolean result = cd.Insert("insert into GISDATA_REGINFO (REG_NAME,REG_ALIASNAME,FEATURE_TYPE,PATH,DBTYPE) values ('" + nodeData["NAME"].ToString() + "','" + nodeData["NAME"].ToString() + "','" + FeatureType + "','" + nodeData["PATH"].ToString() + "','" + nodeData["TYPE"].ToString() + "')");
                        if (result)
                        {
                            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                            {
                                IField field = pFeatureClass.Fields.get_Field(i);
                                if (field.Name != pFeatureClass.ShapeFieldName && field.Name != pFeatureClass.OIDFieldName)
                                {
                                    string REG_NAME = nodeData["NAME"].ToString();
                                    string FIELDID = i.ToString();
                                    string FIELD_NAME = field.Name;
                                    string FIELD_ALSNAME = field.AliasName;
                                    string DATA_TYPE = field.Type.ToString();
                                    string MAXLEN = field.Length.ToString();
                                    string MINLEN = field.Length.ToString();
                                    string CODE_PK = "";
                                    string CODE_WHERE = "";
                                    string IS_NULL = field.IsNullable.ToString();
                                    string DEFAULT_VALUE = field.DefaultValue.ToString();
                                    Boolean insertResult = cd.Insert("insert into GISDATA_MATEDATA (REG_NAME,FIELDID,FIELD_NAME,FIELD_ALSNAME,DATA_TYPE,MAXLEN,MINLEN,CODE_PK,CODE_WHERE,IS_NULL,DEFAULT_VALUE) values ('" + REG_NAME + "','" + FIELDID + "','" + FIELD_NAME + "','" + FIELD_ALSNAME + "','" + DATA_TYPE + "','" + MAXLEN + "','" + MINLEN + "','" + CODE_PK + "','" + CODE_WHERE + "','" + IS_NULL + "','" + DEFAULT_VALUE + "')");
                                    if (!insertResult)
                                    {
                                        boolFlag = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return boolFlag;
        }

        //删除注册连接
        private void buttonDelConnecte_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
            Boolean result = false;
            TreeListNodes selectNode = this.treeList1.Nodes;
            foreach (TreeListNode node in selectNode)
            {
                if (node.Checked)
                {
                    if (this.treeList1.IsRootNode(node))
                    {
                        DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                        ConnectDB cd = new ConnectDB();
                        result = cd.Delete("DELETE from GISDATA_REGCONNECT WHERE ID = " + nodeData["ID"]);
                        if(!result)
                        {
                            break;
                        }
                    }
                }
            }
            if (result)
            {
                MessageBox.Show("删除成功！", "提示");
                refreshTreeViewReg();
            }
            }
            //if (this.treeViewReg.SelectedNode.Level == 0){
            //    DialogResult dr = MessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //    if (dr == DialogResult.OK)
            //    {
            //        ConnectDB cd = new ConnectDB();
            //        Boolean result = cd.Delete("DELETE from GISDATA_REGCONNECT WHERE ID = " + this.treeViewReg.SelectedNode.Tag);
            //        if (result)
            //        {
            //            MessageBox.Show("删除成功！", "提示");
            //        }
            //    }
            //}
        }

        //删除注册信息
        private void buttonDelReg_Click(object sender, EventArgs e)
        {
            var index = this.gridView2.GetFocusedDataSourceRowIndex();
            DataRowView row = (DataRowView)this.gridView2.GetRow(index);
            string val = row["REG_NAME"].ToString();
            if (val != "") 
            {
                DialogResult dr = MessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    ConnectDB cd = new ConnectDB();
                    Boolean result = cd.Delete("DELETE from GISDATA_REGINFO WHERE REG_NAME = '" + val+"'");
                    if (result)
                    {
                        Boolean result1 = cd.Delete("DELETE from GISDATA_MATEDATA WHERE REG_NAME = '" + val + "'");
                        if (result1)
                        {
                            refreshDataGridReg();
                            MessageBox.Show("删除成功！", "提示");
                        }
                    }
                }
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
                //this.checkBox.CheckState = CheckState.Unchecked;
            }
        }

        private void findOrigin(DevExpress.XtraTreeList.TreeList tree, TreeListNodes nodes = null)
        {
            //this.checkBox.CheckState = CheckState.Unchecked;
            nodes = nodes ?? tree.Nodes;
            if (flag)
            {
                foreach (TreeListNode item in nodes)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        //this.checkBox.CheckState = CheckState.Checked;
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

        private void dataGridViewFieldView_DoubleClick(object sender, EventArgs e)
        {
            var index = gridView1.GetFocusedDataSourceRowIndex();
            DataRowView row = (DataRowView)this.gridView1.GetRow(index);
            FormDomain fd = new FormDomain(row, regName);
            fd.Show();
        }

        private void dataGridViewDataReg_DoubleClick(object sender, EventArgs e)
        {
            var index = this.gridView2.GetFocusedDataSourceRowIndex();
            DataRowView row = (DataRowView)this.gridView2.GetRow(index);
            string val = row["REG_NAME"].ToString();
            this.regName = val;
            refreshDataGridField(val);
        }
    }
}
