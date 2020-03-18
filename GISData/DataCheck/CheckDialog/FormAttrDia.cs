using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
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
            DataTable dt = cdb.GetDataBySql("select * from GISDATA_TBATTR");
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
            TreeListNodes selectNode = this.treeList1.Nodes;
            loopCheck(selectNode);
        }

        public void loopCheck(TreeListNodes selectNode)
        {
            foreach (TreeListNode node in selectNode)
            {
                DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                nodeData["ERROR"] = "";
                nodeData["ISCHECK"] = "";
                if (node.Checked|| node.CheckState == CheckState.Indeterminate)
                {
                    if(!node.HasChildren)
                    {
                        string NAME = nodeData["NAME"].ToString();
                        string CHECKTYPE = nodeData["CHECKTYPE"].ToString();
                        string FIELD = nodeData["FIELD"].ToString();
                        string TABLENAME = nodeData["TABLENAME"].ToString();
                        string SUPTABLE = nodeData["SUPTABLE"].ToString();
                        string SELECT = nodeData["SELECT"].ToString();
                        string WHERESTRING = nodeData["WHERESTRING"].ToString();
                        string RESULT = nodeData["RESULT"].ToString();
                        string DOMAINTYPE = nodeData["DOMAINTYPE"].ToString();
                        ConnectDB db = new ConnectDB();
                        if (CHECKTYPE == "空值检查")
                        {
                            CommonClass conClass = new CommonClass();
                            IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                            IQueryDef pQueryDef = ifw.CreateQueryDef();
                            pQueryDef.Tables = TABLENAME;
                            pQueryDef.SubFields = "count(*) as errorCount";
                            pQueryDef.WhereClause = FIELD + " IS NULL OR " + FIELD + " = '' ";
                            ICursor pCur = pQueryDef.Evaluate();
                            IRow pRow = pCur.NextRow();
                            int iInx = pCur.Fields.FindField("errorCount");
                            string count = pRow.get_Value(iInx).ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "逻辑关系检查")
                        {
                            CommonClass conClass = new CommonClass();
                            IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                            IQueryDef pQueryDef = ifw.CreateQueryDef();
                            pQueryDef.Tables = TABLENAME;
                            pQueryDef.SubFields = "count(*) as errorCount";
                            pQueryDef.WhereClause = " (" + WHERESTRING + ") and (" + RESULT + ")";
                            ICursor pCur = pQueryDef.Evaluate();
                            IRow pRow = pCur.NextRow();
                            int iInx = pCur.Fields.FindField("errorCount");
                            string count = pRow.get_Value(iInx).ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "唯一值检查")
                        {
                            CommonClass conClass = new CommonClass();
                            IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                            IQueryDef pQueryDef = ifw.CreateQueryDef();
                            pQueryDef.Tables = TABLENAME;
                            pQueryDef.SubFields = "count(*) as errorCount";
                            pQueryDef.WhereClause = FIELD + " in ( select " + FIELD + " as wyz from  "+TABLENAME+" group by " + FIELD + " having count(" + FIELD + ")>1)";
                            ICursor pCur = pQueryDef.Evaluate();
                            IRow pRow = pCur.NextRow();
                            int iInx = pCur.Fields.FindField("errorCount");
                            string count = pRow.get_Value(iInx).ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "值域检查")
                        {
                            if (DOMAINTYPE == "own") 
                            {
                                DataTable dt = db.GetDataBySql("SELECT CODE_PK,CODE_WHERE FROM GISDATA_MATEDATA WHERE REG_NAME = '"+TABLENAME+"' AND FIELD_NAME = '"+FIELD+"'");
                                string CODETABLENAME = dt.Rows[0]["CODE_PK"].ToString();
                                string CODEWHERESTRING = dt.Rows[0]["CODE_WHERE"].ToString();
                                CommonClass conClass = new CommonClass();
                                IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                                IQueryDef pQueryDef = ifw.CreateQueryDef();
                                pQueryDef.Tables = TABLENAME;
                                pQueryDef.SubFields = "count(*) as errorCount";
                                pQueryDef.WhereClause = FIELD + " not in (SELECT C_CODE FROM " + CODETABLENAME + " WHERE " + CODEWHERESTRING + ")";
                                ICursor pCur = pQueryDef.Evaluate();
                                IRow pRow = pCur.NextRow();
                                int iInx = pCur.Fields.FindField("errorCount");
                                string count = pRow.get_Value(iInx).ToString();
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                            }
                            else if (DOMAINTYPE == "custom")
                            {
                                string CUSTOMVALUE = nodeData["CUSTOMVALUE"].ToString();
                                CommonClass conClass = new CommonClass();
                                IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                                IQueryDef pQueryDef = ifw.CreateQueryDef();
                                pQueryDef.Tables = TABLENAME;
                                pQueryDef.SubFields = "count(*) as errorCount";
                                pQueryDef.WhereClause = FIELD + " not in (" + CUSTOMVALUE + ")";
                                ICursor pCur = pQueryDef.Evaluate();
                                IRow pRow = pCur.NextRow();
                                int iInx = pCur.Fields.FindField("errorCount");
                                string count = pRow.get_Value(iInx).ToString();
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                            }
                        }
                    }
                    if (node.Nodes.Count != 0)
                    {
                        loopCheck(node.Nodes);
                    }
                }
            }
        }

        public void loopCheck11(TreeListNodes selectNode)
        {
            foreach (TreeListNode node in selectNode)
            {
                DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                nodeData["ERROR"] = "";
                nodeData["ISCHECK"] = "";
                if (node.Checked || node.CheckState == CheckState.Indeterminate)
                {
                    if (!node.HasChildren)
                    {
                        string NAME = nodeData["NAME"].ToString();
                        string CHECKTYPE = nodeData["CHECKTYPE"].ToString();
                        string FIELD = nodeData["FIELD"].ToString();
                        string TABLENAME = nodeData["TABLENAME"].ToString();
                        string SUPTABLE = nodeData["SUPTABLE"].ToString();
                        string SELECT = nodeData["SELECT"].ToString();
                        string WHERESTRING = nodeData["WHERESTRING"].ToString();
                        string RESULT = nodeData["RESULT"].ToString();
                        string DOMAINTYPE = nodeData["DOMAINTYPE"].ToString();
                        ConnectDB db = new ConnectDB();
                        if (CHECKTYPE == "空值检查")
                        {
                            DataTable dt = db.GetDataBySql("select count(*) as nullValue from " + TABLENAME + "_TB where " + FIELD + " IS NULL OR " + FIELD + " = '' ");
                            string count = dt.Rows[0]["nullValue"].ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "逻辑关系检查")
                        {
                            DataTable dt = db.GetDataBySql("select count(*) as logicError from " + TABLENAME + "_TB where (" + WHERESTRING + " ) and (" + RESULT + ")");
                            string count = dt.Rows[0]["logicError"].ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "唯一值检查")
                        {
                            DataTable dt = db.GetDataBySql("select count(*) as uniqueError from " + TABLENAME + "_TB WHERE " + FIELD + " in ( select " + FIELD + " as wyz from  YZL_PY_ZLYSXB_TB group by " + FIELD + " having count(" + FIELD + ")>1)");
                            string count = dt.Rows[0]["uniqueError"].ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "值域检查")
                        {
                            if (DOMAINTYPE == "own")
                            {
                                DataTable dt = db.GetDataBySql("SELECT CODE_PK,CODE_WHERE FROM GISDATA_MATEDATA WHERE REG_NAME = 'YZL_PY_ZLYSXB' AND FIELD_NAME = 'XIAN'");
                                string CODETABLENAME = dt.Rows[0]["CODE_PK"].ToString();
                                string CODEWHERESTRING = dt.Rows[0]["CODE_WHERE"].ToString();
                                DataTable dt1 = db.GetDataBySql("SELECT count(*) as uniqueError FROM " + TABLENAME + "_TB where " + FIELD + " not in (SELECT C_CODE FROM " + CODETABLENAME + " WHERE " + CODEWHERESTRING + ")");
                                string count = dt1.Rows[0]["uniqueError"].ToString();
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                            }
                            else if (DOMAINTYPE == "custom")
                            {
                                string CUSTOMVALUE = nodeData["CUSTOMVALUE"].ToString();
                                DataTable dt = db.GetDataBySql("SELECT count(*) as uniqueError FROM " + TABLENAME + "_TB where " + FIELD + " not in (" + CUSTOMVALUE + ")");
                                string count = dt.Rows[0]["uniqueError"].ToString();
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                            }
                        }
                    }
                    if (node.Nodes.Count != 0)
                    {
                        loopCheck(node.Nodes);
                    }
                }
            }
        }
    }
}
