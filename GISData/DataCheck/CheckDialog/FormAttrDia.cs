using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Xls;
using System.Runtime.InteropServices;
using System.Xml;
//using Excel = Microsoft.Office.Interop.Excel;

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormAttrDia : Form
    {
        private string stepNo;
        private CheckBox checkBox;
        Boolean flag = true;
        private CheckBox checkBoxCheckMain;
        private GridControl gridControlError = null;
        public DataTable attrErrorTable = null;
        private ProgressBar progressBar = null;
        private string scheme;
        private string attrDir;
        private List<List<string>> listError;
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
        public FormAttrDia(string stepNo, CheckBox cb, string scheme, CheckBox cbCheckMain, GridControl gridControlError, ProgressBar progressbar)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
            this.scheme = scheme;
            this.checkBoxCheckMain = cbCheckMain;
            this.gridControlError = gridControlError;
            this.progressBar = progressbar;
            bindTreeView();
            CommonClass common = new CommonClass();
            ConnectDB db = new ConnectDB();
            DataTable DT = db.GetDataBySql("select GLDWNAME from GISDATA_GLDW where GLDW = '" + common.GetConfigValue("GLDW") + "'");
            DataRow dr = DT.Select(null)[0];
            this.attrDir = common.GetConfigValue("SAVEDIR") + "\\" + dr["GLDWNAME"].ToString() + "\\错误参考\\属性错误";
            this.listError = new List<List<string>>();
            CreateExcelFile();
        }

        private void FormAttrDia_Load(object sender, EventArgs e)
        {
            bindTreeView();
        }

        private void bindTreeView() 
        {
            ConnectDB cdb = new ConnectDB();
            DataTable dt = cdb.GetDataBySql("select * from GISDATA_TBATTR where SCHEME ='" + this.scheme + "' and STEP_NO = '" + this.stepNo + "'");

            foreach (DataRow dr in dt.Rows)  //对特定的行添加限制条件
            {
                if (dr["CHECKTYPE"].ToString() != "") 
                {
                    string ID = dr["ID"].ToString();
                    string value = getXMLInnerText(ID);
                    dr["ERROR"] = value;
                    dr["ISCHECK"] = "已检查";
                }
            }
            
            this.treeList1.DataSource = dt;
            treeList1.OptionsView.ShowCheckBoxes = true;
            attrErrorTable = new DataTable();
            foreach ( DataColumn column in dt.Columns) 
            {
                attrErrorTable.Columns.Add(column.ColumnName);
            }
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

        }

        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {

        }

        private void findOrigin(DevExpress.XtraTreeList.TreeList tree,TreeListNodes nodes = null)
        {
            //this.checkBox.CheckState = CheckState.Unchecked;
            nodes = nodes ?? tree.Nodes;
            if (flag) 
            {
                foreach (TreeListNode item in nodes)
                {
                    if (item.CheckState == System.Windows.Forms.CheckState.Checked)
                    {
                        this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
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
                item.CheckState = System.Windows.Forms.CheckState.Checked;
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
                item.CheckState = System.Windows.Forms.CheckState.Unchecked;
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
            XmlDocument doc = new XmlDocument();
            doc.Load(@"checkEroor.xml");
            var root = doc.DocumentElement;//取到根结点
            XmlElement Attrs = doc.CreateElement("Attrs");
            root.AppendChild(Attrs);

            this.listError.Clear();
            CheckForIllegalCrossThreadCalls = false;
            ExpandAppointTreeNode(this.treeList1);
            TreeListNodes selectNode = this.treeList1.Nodes;
            loopCheck(selectNode,Attrs,doc);
            WriteToExcel(this.listError);

            doc.Save(@"checkEroor.xml");
            //Thread tr = new Thread(doCheck);
            //tr.Start();
        }
        private void doCheck() 
        {
            this.backgroundWorker1.RunWorkerAsync();
        }

        public void loopCheck(TreeListNodes selectNode, XmlElement Attrs,XmlDocument doc)
        {
           
            foreach (TreeListNode node in selectNode)
            {
                //this.progressBar.Value = Convert.ToInt32((node.Id+1)/this.treeList1.AllNodesCount*100);
                DataRowView nodeData = this.treeList1.GetDataRecordByNode(node) as DataRowView;
                node.Expanded = true;
                nodeData["ERROR"] = "";
                nodeData["ISCHECK"] = "";
                if (node.Checked|| node.CheckState == System.Windows.Forms.CheckState.Indeterminate)
                {
                    if(!node.HasChildren)
                    {
                        this.treeList1.FocusedNode = node;
                        string NAME = nodeData["NAME"].ToString();
                        Console.WriteLine(NAME);
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
                            try{
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
                                if (int.Parse(count) > 0 )
                                {
                                    List<string> listItem = new List<string>();
                                    listItem.Add(CHECKTYPE);
                                    listItem.Add(NAME);
                                    listItem.Add(count);
                                    listItem.Add(FIELD + " IS NULL OR  format(" + FIELD + ") = '' ");
                                    this.listError.Add(listItem);
                                }
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                                Marshal.ReleaseComObject(pCur);
                            }
                            catch (Exception e)
                            {
                                LogHelper.WriteLog(typeof(FormAttrDia), e);
                                LogHelper.WriteLog(typeof(FormAttrDia), FIELD + " IS NULL OR  format(" + FIELD + ") = '' ");
                            }
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
                                try
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
                                    if (int.Parse(count) > 0)
                                    {
                                        List<string> listItem = new List<string>();
                                        listItem.Add(CHECKTYPE);
                                        listItem.Add(NAME);
                                        listItem.Add(count);
                                        listItem.Add(" (" + WHERESTRING + ") and (" + RESULT + ")");
                                        this.listError.Add(listItem);
                                    }
                                    nodeData["ERROR"] = count;
                                    nodeData["ISCHECK"] = "已检查";
                                    Marshal.ReleaseComObject(pCur);
                                }catch(Exception e)
                                {
                                    LogHelper.WriteLog(typeof(FormAttrDia), e);
                                    LogHelper.WriteLog(typeof(FormAttrDia), " (" + WHERESTRING + ") and (" + RESULT + ")");
                                }
                            }
                            
                        }
                        else if (CHECKTYPE == "唯一值检查")
                        {
                            try{
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
                                if (int.Parse(count) > 0)
                                {
                                    List<string> listItem = new List<string>();
                                    listItem.Add(CHECKTYPE);
                                    listItem.Add(NAME);
                                    listItem.Add(count);
                                    listItem.Add(FIELD + " in ( select " + FIELD + " as wyz from  " + TABLENAME + " group by " + FIELD + " having count(" + FIELD + ")>1)");
                                    this.listError.Add(listItem);
                                }
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                                Marshal.ReleaseComObject(pCur);
                            }
                            catch (Exception e)
                            {
                                LogHelper.WriteLog(typeof(FormAttrDia), e);
                                LogHelper.WriteLog(typeof(FormAttrDia), FIELD + " in ( select " + FIELD + " as wyz from  " + TABLENAME + " group by " + FIELD + " having count(" + FIELD + ")>1)");
                            }
                        }
                        else if (CHECKTYPE == "值域检查")
                        {
                            try
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
                                        pQueryDef.WhereClause = FIELD + " IS NOT NULL AND " + FIELD + " <> '' AND "+ FIELD + " not in ('" + codesString + "')";
                                        ICursor pCur = pQueryDef.Evaluate();
                                        IRow pRow = pCur.NextRow();
                                        int iInx = pCur.Fields.FindField("errorCount");
                                        string count = pRow.get_Value(iInx).ToString();
                                        if (int.Parse(count) > 0)
                                        {
                                            List<string> listItem = new List<string>();
                                            listItem.Add(CHECKTYPE);
                                            listItem.Add(NAME);
                                            listItem.Add(count);
                                            listItem.Add(FIELD + " IS NOT NULL AND " + FIELD + " <> '' AND " + FIELD + " not in ('" + codesString + "')");
                                            this.listError.Add(listItem);
                                        }
                                        nodeData["ERROR"] = count;
                                        nodeData["ISCHECK"] = "已检查";
                                        Marshal.ReleaseComObject(pCur);
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
                                    if (int.Parse(count) > 0)
                                    {
                                        List<string> listItem = new List<string>();
                                        listItem.Add(CHECKTYPE);
                                        listItem.Add(NAME);
                                        listItem.Add(count);
                                        listItem.Add(FIELD + " not in (" + CUSTOMVALUE + ")");
                                        this.listError.Add(listItem);
                                    }
                                    nodeData["ERROR"] = count;
                                    nodeData["ISCHECK"] = "已检查";
                                    Marshal.ReleaseComObject(pCur);
                                }
                            }catch(Exception e)
                            {
                                LogHelper.WriteLog(typeof(FormAttrDia), e);
                            }
                        }
                        else if (CHECKTYPE == "项目名称检查")
                        {
                            try{
                                string[] arrStr = FIELD.Split('#');
                                string fieldStr = arrStr[0];
                                string taskfieldStr = "CONCAT(" + arrStr[1].Replace("&",",")+")";
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
                                if (int.Parse(count) > 0)
                                {
                                    List<string> listItem = new List<string>();
                                    listItem.Add(CHECKTYPE);
                                    listItem.Add(NAME);
                                    listItem.Add(count);
                                    listItem.Add(fieldStr + " not in ('" + codesString + "')");
                                    this.listError.Add(listItem);
                                }
                                nodeData["ERROR"] = count;
                                nodeData["ISCHECK"] = "已检查";
                                Marshal.ReleaseComObject(pCur);
                            }
                            catch (Exception e)
                            {
                                LogHelper.WriteLog(typeof(FormAttrDia), e);
                                LogHelper.WriteLog(typeof(FormAttrDia), "项目名称检查错误");
                            }
                        }
                        if (nodeData["ERROR"].ToString()!="0") 
                        {
                            attrErrorTable.ImportRow(nodeData.Row);
                        }



                        XmlElement Xmlnode = doc.CreateElement("attr");
                        ////创建bai用户du名节点
                        //XmlElement element1 = doc.CreateElement("ID");
                        //element1.InnerText = nodeData["ID"].ToString();
                        //Xmlnode.AppendChild(element1);
                        ////创建密码节点
                        //XmlElement element2 = doc.CreateElement("ERROR");
                        //element2.InnerText = nodeData["ERROR"].ToString();
                        //Xmlnode.AppendChild(element2);
                        Attrs.AppendChild(Xmlnode);
                        XmlAttribute xa = doc.CreateAttribute("key");
                        xa.Value = nodeData["ID"].ToString();
                        XmlAttribute xa2 = doc.CreateAttribute("value");
                        xa2.Value = nodeData["ERROR"].ToString();
                        Xmlnode.SetAttributeNode(xa);
                        Xmlnode.SetAttributeNode(xa2); 
                    }
                    if (node.Nodes.Count != 0)
                    {
                        loopCheck(node.Nodes, Attrs,doc);
                    }
                }

            }
            
            
        }

        private void treeList1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private string GenerateLogicRelationSQL(string whereString ,string resultString )
        {
            try{
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
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(FormAttrDia), e);
                return "1=1";
            }
        }

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            if (checkBoxCheckMain.Checked) 
            {
                
            }
        }

        /// <summary>
        /// 展开指定节点以及其父节点
        /// </summary>
        /// <param name="tree">树</param>
        private void ExpandAppointTreeNode(TreeList tree)
        {
            foreach (TreeListNode node in tree.Nodes)
            {
                if (node.Level == 0)
                {
                    node.ExpandAll();
                }
            }
        }

        private void treeList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.checkBoxCheckMain.Checked)
                {
                    TreeListNode node = this.treeList1.FocusedNode;
                    if (!node.HasChildren && node["ERROR"].ToString() != "0" && node["ISCHECK"].ToString() == "已检查")
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
                            pQuery.WhereClause = FIELD + " IS NULL OR  format(" + FIELD + ") = '' ";
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
                        else if (CHECKTYPE == "项目名称检查")
                        {
                            string[] arrStr = FIELD.Split('#');
                            string fieldStr = arrStr[0];
                            string taskfieldStr = "CONCAT(" + arrStr[1].Replace("&", ",") + ")";
                            CommonClass common = new CommonClass();
                            string gldwstr = common.GetConfigValue("GLDW");
                            DataTable xmmcArr = db.GetDataBySql("SELECT " + taskfieldStr + " AS XMMCARR FROM GISDATA_TASK WHERE YZLGLDW ='" + gldwstr + "'");
                            string[] idInts = xmmcArr.AsEnumerable().Select(d => d.Field<string>("XMMCARR")).ToArray();
                            string codesString = String.Join("','", idInts);

                            IQueryFilter pQuery = new QueryFilterClass();
                            pQuery.WhereClause = fieldStr + " not in ('" + codesString + "')";
                            pFeatureCuror = pTable.Search(pQuery, false);
                        }
                        else if (CHECKTYPE == "值域检查")
                        {
                            if (DOMAINTYPE == "own")
                            {
                                DataTable dt1 = db.GetDataBySql("SELECT CODE_PK,CODE_WHERE FROM GISDATA_MATEDATA WHERE REG_NAME = '" + TABLENAME + "' AND FIELD_NAME = '" + FIELD + "'");
                                string CODETABLENAME = dt1.Rows[0]["CODE_PK"].ToString();
                                string CODEWHERESTRING = dt1.Rows[0]["CODE_WHERE"].ToString();
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

                                IQueryFilter pQuery = new QueryFilterClass();
                                //pQuery.WhereClause = FIELD + " not in (SELECT C_CODE FROM " + CODETABLENAME + " WHERE " + CODEWHERESTRING + ")";
                                pQuery.WhereClause = FIELD + " IS NOT NULL AND " + FIELD + " <> '' AND " + FIELD + " not in ('" + codesString + "')";
                                pFeatureCuror = pTable.Search(pQuery, false);
                            }
                            else if (DOMAINTYPE == "custom")
                            {
                                string CUSTOMVALUE = node["CUSTOMVALUE"].ToString();
                                IQueryFilter pQuery = new QueryFilterClass();
                                pQuery.WhereClause = FIELD + " not in (" + CUSTOMVALUE + ") AND " + FIELD + " IS NOT NULL AND " + FIELD + " <> ''";
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
                        gridView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
                        gridView.OptionsView.ColumnAutoWidth = false;

                        gridView.ViewCaption = "属性检查";
                        gridView.NewItemRowText = TABLENAME;
                        this.gridControlError.RefreshDataSource();
                        Marshal.ReleaseComObject(pFeatureCuror);
                        Marshal.ReleaseComObject(pFLayer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormAttrDia), ex);
            }
        }
        /// <summary>
        /// If the supplied excel File does not exist then Create it
        /// </summary>
        /// <param name="FileName"></param>
        private void CreateExcelFile()
        {
            if (System.IO.File.Exists(this.attrDir + "\\属性错误参考.xls")) 
            {
                System.IO.File.Delete(this.attrDir + "\\属性错误参考.xls");
            }
            //create
            object Nothing = System.Reflection.Missing.Value;
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            CellRange rng = sheet.Range["A1:D1"];
            rng.ColumnWidth = 30;
            rng.BorderAround();
            rng.Style.Font.Size = 14;
            rng.Style.HorizontalAlignment = HorizontalAlignType.Center;
            rng.Style.HorizontalAlignment = HorizontalAlignType.Center;
            //rng.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThick, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
            sheet.Range["A1"].Text = "检查类型";
            sheet.Range["B1"].Text = "检查项";
            sheet.Range["C1"].Text = "错误数量";
            sheet.Range["D1"].Text = "参考sql";
            workbook.SaveToFile(this.attrDir + "\\属性错误参考.xls");
        }

        /// <summary>
        /// open an excel file,then write the content to file
        /// </summary>
        /// <param name="FileName">file name</param>
        /// <param name="findString">first cloumn</param>
        /// <param name="replaceString">second cloumn</param>
        private void WriteToExcel(List<List<string>> listError)
        {

            Workbook workbook = new Workbook();

            //Load an excel file including pivot table
            workbook.LoadFromFile(this.attrDir + "\\属性错误参考.xls");

            //Modify data of data source
            Worksheet datasheet = workbook.Worksheets[0];
            int maxrow = datasheet.Rows.Count()+1;
            int startIndex = maxrow;
            foreach (List<string> item in listError)
            {
                datasheet.Range[maxrow, 1].Text = item[0];
                datasheet.Range[maxrow, 2].Text = item[1];
                datasheet.Range[maxrow, 3].Text = item[2];
                datasheet.Range[maxrow, 4].Text = item[3];
                maxrow++;
            }
            CellRange rng = datasheet.Range["A" + startIndex + ":D" + (maxrow - 1)];
            rng.ColumnWidth = 30;
            rng.BorderAround(LineStyleType.Double,Color.Black);
            rng.BorderInside(LineStyleType.Thin, Color.Black); 
            rng.Style.Font.Size = 11;
            rng.Style.HorizontalAlignment = HorizontalAlignType.Center;
            rng.Style.HorizontalAlignment = HorizontalAlignType.Center;
            workbook.Save();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            TreeListNodes selectNode = this.treeList1.Nodes;
            //loopCheck(selectNode);
        }

        private void treeList1_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
        }

        public string getXMLInnerText(string ID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"checkEroor.xml");
                string strPath = string.Format("/Error/Attrs/attr[@key=\"{0}\"]", ID);
                if (strPath != null)
                {
                    XmlElement selectXe = (XmlElement)doc.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    return selectXe.GetAttribute("value");
                }
                else 
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
                return null;
            }

        }

        private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
        }
    }
}
