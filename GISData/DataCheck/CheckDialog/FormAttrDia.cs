using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
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
        private CheckBox checkBoxCheckMain;
        private GridControl gridControlError = null;
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
        public FormAttrDia(string stepNo, CheckBox cb, CheckBox cbCheckMain, GridControl gridControlError)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
            this.checkBoxCheckMain = cbCheckMain;
            this.gridControlError = gridControlError;
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
                            pQueryDef.WhereClause = FIELD + " IS NULL OR  format(" + FIELD + ") = '' ";
                            ICursor pCur = pQueryDef.Evaluate();
                            IRow pRow = pCur.NextRow();
                            int iInx = pCur.Fields.FindField("errorCount");
                            string count = pRow.get_Value(iInx).ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
                        }
                        else if (CHECKTYPE == "逻辑关系检查")
                        {
                            if (WHERESTRING == "" || RESULT == "")
                            {
                                nodeData["ERROR"] = "";
                                nodeData["ISCHECK"] = "未检查";
                            }
                            else 
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
                                if (CODETABLENAME != "" && CODEWHERESTRING != "")
                                {
                                    DataTable codeArr = null;
                                    if (CODETABLENAME == "GISDATA_ZQSJZD")
                                    {
                                        CommonClass common = new CommonClass();
                                        string gldwstr = common.GetConfigValue("GLDW");
                                        codeArr = db.GetDataBySql("SELECT C_CODE FROM " + CODETABLENAME + " WHERE " + CODEWHERESTRING + " AND LEFT(C_CODE,6)='" + gldwstr + "'");
                                    }
                                    else 
                                    {
                                        codeArr = db.GetDataBySql("SELECT C_CODE FROM " + CODETABLENAME + " WHERE " + CODEWHERESTRING);
                                    }
                                    string[] idInts = codeArr.AsEnumerable().Select(d => d.Field<string>("C_CODE")).ToArray();
                                    string codesString = String.Join("','", idInts);
                                    CommonClass conClass = new CommonClass();
                                    IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                                    IQueryDef pQueryDef = ifw.CreateQueryDef();
                                    pQueryDef.Tables = TABLENAME;
                                    pQueryDef.SubFields = "count(*) as errorCount";
                                    pQueryDef.WhereClause = FIELD + " IS NOT NULL AND " + FIELD + " not in ('" + codesString + "')";
                                    ICursor pCur = pQueryDef.Evaluate();
                                    IRow pRow = pCur.NextRow();
                                    int iInx = pCur.Fields.FindField("errorCount");
                                    string count = pRow.get_Value(iInx).ToString();
                                    nodeData["ERROR"] = count;
                                    nodeData["ISCHECK"] = "已检查";
                                }
                                else 
                                {
                                    nodeData["ERROR"] = "";
                                    nodeData["ISCHECK"] = "未检查";
                                }
                                
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
                        else if (CHECKTYPE == "项目名称检查")
                        {
                            string[] arrStr = FIELD.Split('#');
                            string fieldStr = arrStr[0];
                            string taskfieldStr = arrStr[1];
                            CommonClass common = new CommonClass();
                            string gldwstr = common.GetConfigValue("GLDW");
                            DataTable xmmcArr = db.GetDataBySql("SELECT " + taskfieldStr + " AS XMMCARR FROM GISDATA_TASK WHERE YZLGLDW ='" + gldwstr + "'");
                            string[] idInts = xmmcArr.AsEnumerable().Select(d => d.Field<string>("XMMCARR")).ToArray();
                            string codesString = String.Join("','", idInts);
                            
                            
                            CommonClass conClass = new CommonClass();
                            IFeatureWorkspace ifw = conClass.GetFeatureWorkspaceByName(TABLENAME);
                            IQueryDef pQueryDef = ifw.CreateQueryDef();
                            pQueryDef.Tables = TABLENAME;
                            pQueryDef.SubFields = "count(*) as errorCount";
                            pQueryDef.WhereClause = fieldStr + " not in ('" + codesString + "')";
                            ICursor pCur = pQueryDef.Evaluate();
                            IRow pRow = pCur.NextRow();
                            int iInx = pCur.Fields.FindField("errorCount");
                            string count = pRow.get_Value(iInx).ToString();
                            nodeData["ERROR"] = count;
                            nodeData["ISCHECK"] = "已检查";
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

        private void treeList1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListNode node = this.treeList1.FocusedNode;
                if (!node.HasChildren && node["ERROR"].ToString() != "0")
                {
                    string NAME = node["NAME"].ToString();
                    string CHECKTYPE = node["CHECKTYPE"].ToString();
                    string FIELD = node["FIELD"].ToString();
                    string TABLENAME = node["TABLENAME"].ToString();
                    string SUPTABLE = node["SUPTABLE"].ToString();
                    string SELECT = node["SELECT"].ToString();
                    string WHERESTRING = node["WHERESTRING"].ToString();
                    string RESULT = node["RESULT"].ToString();
                    string DOMAINTYPE = node["DOMAINTYPE"].ToString();
                    string SHOWFIELD = node["SHOWFIELD"].ToString();
                    DataTable dt = new DataTable();
                    CommonClass conClass = new CommonClass();
                    ICursor pFeatureCuror = null;
                    IFeatureLayer pFLayer = conClass.GetLayerByName(TABLENAME);
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                    ITable pTable = pFeatureClass as ITable;
                    ConnectDB db = new ConnectDB();
                    if (CHECKTYPE == "空值检查")
                    {
                        IQueryFilter pQuery = new QueryFilterClass();
                        pQuery.WhereClause = FIELD + " IS NULL OR " + FIELD + " = '' ";
                        pFeatureCuror = pTable.Search(pQuery, false);
                    }
                    else if (CHECKTYPE == "逻辑关系检查")
                    {
                        IQueryFilter pQuery = new QueryFilterClass();
                        pQuery.WhereClause = " (" + WHERESTRING + ") and (" + RESULT + ")";
                        pFeatureCuror = pTable.Search(pQuery, false);
                    }
                    else if (CHECKTYPE == "唯一值检查")
                    {
                        IQueryFilter pQuery = new QueryFilterClass();
                        pQuery.WhereClause = FIELD + " in ( select " + FIELD + " as wyz from  " + TABLENAME + " group by " + FIELD + " having count(" + FIELD + ")>1)";
                        pFeatureCuror = pTable.Search(pQuery, false);
                    }
                    else if (CHECKTYPE == "值域检查")
                    {
                        if (DOMAINTYPE == "own")
                        {
                            DataTable dt1 = db.GetDataBySql("SELECT CODE_PK,CODE_WHERE FROM GISDATA_MATEDATA WHERE REG_NAME = '" + TABLENAME + "' AND FIELD_NAME = '" + FIELD + "'");
                            string CODETABLENAME = dt1.Rows[0]["CODE_PK"].ToString();
                            string CODEWHERESTRING = dt1.Rows[0]["CODE_WHERE"].ToString();
                            IQueryFilter pQuery = new QueryFilterClass();
                            pQuery.WhereClause = FIELD + " not in (SELECT C_CODE FROM " + CODETABLENAME + " WHERE " + CODEWHERESTRING + ")";
                            pFeatureCuror = pTable.Search(pQuery, false);
                        }
                        else if (DOMAINTYPE == "custom")
                        {
                            string CUSTOMVALUE = node["CUSTOMVALUE"].ToString();
                            IQueryFilter pQuery = new QueryFilterClass();
                            pQuery.WhereClause = FIELD + " not in (" + CUSTOMVALUE + ")";
                            pFeatureCuror = pTable.Search(pQuery, false);
                        }
                    }
                    if (pFeatureClass == null) return;
                    
                    DataColumn dc = null;
                    for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                    {
                        IField field = pFeatureClass.Fields.get_Field(i);
                        dc = new DataColumn(field.Name);
                        dc.Caption = field.AliasName;
                        dt.Columns.Add(dc);
                        if (SHOWFIELD.Contains(field.Name))
                        {
                            dc.SetOrdinal(0);
                        }
                    }

                    ITable iTable = pFeatureClass as ITable;
                    IRow pFeature = pFeatureCuror.NextRow();
                    DataRow dr = null;
                    while (pFeature != null)
                    {
                        dr = dt.NewRow();
                        int colIndex = 0;
                        foreach (DataColumn itemColumns in dt.Columns)
                        {
                            int index = pFeatureClass.FindField(itemColumns.ColumnName);
                            if (pFeatureClass.FindField(pFeatureClass.ShapeFieldName) == index)
                            {
                                dr[colIndex] = pFeatureClass.ShapeType.ToString();
                            }
                            else
                            {
                                dr[colIndex] = pFeature.get_Value(index).ToString();
                            }
                            colIndex++;
                        }
                        dt.Rows.Add(dr);
                        pFeature = pFeatureCuror.NextRow();
                    }
                    GridView gridView = this.gridControlError.DefaultView as GridView;
                    gridView.Columns.Clear();
                    this.gridControlError.DataSource = dt;
                    gridView.OptionsBehavior.Editable = false;
                    gridView.OptionsSelection.MultiSelect = true;
                    gridView.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
                    gridView.HorzScrollVisibility = ScrollVisibility.Always;
                    gridView.OptionsView.ColumnAutoWidth = false;
                    //for (int I = 0; I < gridView.Columns.Count; I++)
                    //{
                    //    gridView.BestFitColumns();
                    //    gridView.Columns[I].BestFit();//自动列宽
                    //}
                    gridView.ViewCaption = "属性检查";
                    gridView.NewItemRowText = TABLENAME;
                    this.gridControlError.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MyException Throw:" + ex.Message);
            }
        }

        private string GenerateLogicRelationSQL(string whereString ,string resultString )
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            stringBuilder.Append(whereString);
            if (resultString.Contains("0=0") || resultString.Contains("0 =0") || resultString.Contains("0 = 0") || resultString.Contains("0= 0"))
            {
                stringBuilder.Append(") AND (");
            }
            else
            {
                stringBuilder.Append(") AND NOT (");
            }
            stringBuilder.Append(resultString);
            stringBuilder.Append(")");
            //if (!string.IsNullOrEmpty(this.GetGlobalFilter()))
            //{
            //    stringBuilder.Append(" AND ");
            //    stringBuilder.Append(this.GetGlobalFilter());
            //}
            return stringBuilder.ToString();
        }

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            if (checkBoxCheckMain.Checked) 
            {
                
            }
        }
    }
}
