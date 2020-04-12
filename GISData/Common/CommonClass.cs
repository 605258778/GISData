using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.Common
{
    public class CommonClass
    {
        private static Dictionary<string, IFeatureLayer> DicLayer = new Dictionary<string, IFeatureLayer>();
        private static Dictionary<string, IFeatureWorkspace> DicWF = new Dictionary<string, IFeatureWorkspace>();
        /// <summary>
        /// 获取IFeatureLayer
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public IFeatureLayer GetLayerByName(string tablename) 
        {
            if (DicLayer.ContainsKey(tablename))
            {
                return DicLayer[tablename];
            }
            else 
            {
                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select * from GISDATA_REGINFO where REG_NAME = '" + tablename.Trim() + "'");
                DataRow[] dr = dt.Select(null);
                string path = dr[0]["PATH"].ToString();
                string dbtype = dr[0]["DBTYPE"].ToString();
                IFeatureWorkspace space;
                if (dbtype == "Access数据库")
                {
                    AccessWorkspaceFactory fac = new AccessWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);

                }
                else if (dbtype == "文件夹数据库")
                {
                    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);
                }
                else 
                {
                    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);
                }
                IFeatureLayer _Layer = new FeatureLayer();
                _Layer.FeatureClass = space.OpenFeatureClass(tablename);
                _Layer.Name = tablename;
                DicLayer.Add(tablename, _Layer);
                return _Layer;
            }
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataTable GetTableByName(string tablename)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_REGINFO where REG_NAME = '" + tablename.Trim() + "'");
            DataRow[] dr = dt.Select(null);
            string path = dr[0]["PATH"].ToString();
            string dbtype = dr[0]["DBTYPE"].ToString();
            IFeatureWorkspace space;
            if (dbtype == "Access数据库")
            {
                AccessWorkspaceFactory fac = new AccessWorkspaceFactoryClass();
                space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);

            }
            else if (dbtype == "文件夹数据库")
            {
                FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);
            }
            else
            {
                FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);
            }

            IWorkspaceDomains workspaceDomains = (IWorkspaceDomains)space;
            
            IFeatureClass featureClass = space.OpenFeatureClass(tablename);
            setIDomain(workspaceDomains, featureClass, tablename);
            ITable table = space.OpenTable(tablename.Trim());
            DataTable DT = ToDataTable(table);
            DT.TableName = tablename.Trim();
            return DT;
        }

        /// <summary>
        /// 将DataTable的字段名全部翻译为中文
        /// </summary>
        /// <param name="table">待翻译的DataTable</param>
        /// <returns></returns>
        public DataTable TranslateDataTable(DataTable table)
        {
            DataTable dt = new DataTable();
            dt.TableName = table.TableName;

            if (table != null && table.Rows.Count > 0)
            {
                //先为dt构造列信息
                foreach (DataColumn column in table.Columns)
                {
                    dt.Columns.Add(column.ColumnName);
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow NewRow = dt.NewRow();
                    DataRow row = table.Rows[i];

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        NewRow[j] = GetColumnName(table.TableName, dt.Columns[j].ColumnName, row[j].ToString());
                    }

                    dt.Rows.Add(NewRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 得到列名称的别名
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string GetColumnName(string tablename,string columnName ,string value)
        {
            ConnectDB db = new ConnectDB();

            DataTable dt = db.GetDataBySql("select CODE_PK,CODE_WHERE from GISDATA_MATEDATA where REG_NAME = '" + tablename.Trim() + "' and FIELD_NAME ='"+columnName+"' and CODE_PK is not null and CODE_PK<>'' and CODE_WHERE is not null and CODE_WHERE<>''");
            DataRow[] drs = dt.Select();
            string returnValue="";
            if (drs.Length > 0) 
            {
                string whereString = drs[0]["CODE_WHERE"].ToString();
                string dictable = drs[0]["CODE_PK"].ToString();

                DataTable DT = db.GetDataBySql("select C_NAME FROM " + dictable + " where " + whereString + " and C_CODE ='" + value + "'");
                string name = DT.Select(null)[0]["C_NAME"].ToString();
                returnValue = name == "" ? value : name;
            }
            return returnValue == "" ? value : returnValue;
        }

        /// <summary>  
        /// 将ITable转换为DataTable  
        /// </summary>  
        /// <param name="mTable"></param>  
        /// <returns></returns>  
        public static DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }
                ICursor cursor = mTable.Search(null, false);
                IRow pFeature = cursor.NextRow();
                while (pFeature != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pFeature.Fields.FieldCount];
                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        //StrRow[i] = pFeature.get_Value(i).ToString();
                        IField field = pFeature.Fields.get_Field(i);
                        if (field.Domain != null)
                        {
                            StrRow[i] = ValueFromCode(field.Domain, pFeature.get_Value(i).ToString());
                        }
                        else 
                        {
                            StrRow[i] = pFeature.get_Value(i).ToString();
                        }
                        
                        //StrRow[i] = ValueFromCode(pFeature);
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pFeature = cursor.NextRow();
                }

                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 设置属性域
        /// </summary>
        /// <param name="workspaceDomains"></param>
        /// <param name="featureClass"></param>
        /// <param name="tablename"></param>
        private void setIDomain(IWorkspaceDomains workspaceDomains, IFeatureClass featureClass, string tablename) 
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select CODE_PK,CODE_WHERE,FIELD_NAME,FIELD_ALSNAME,DATA_TYPE from GISDATA_MATEDATA where REG_NAME = '" + tablename.Trim() + "' and CODE_PK is not null and CODE_PK<>'' and CODE_WHERE is not null and CODE_WHERE<>''");
            DataRow[] drs = dt.Select();
            for (int i = 0; i < drs.Length; i++) 
            {
                ICodedValueDomain codedValueDomain = new CodedValueDomainClass();
                string codepk = drs[i][0].ToString();
                string codewhere = drs[i][1].ToString();
                DataTable ItemDomain = new DataTable();
                if (codepk == "GISDATA_ZQSJZD") 
                { 
                    ItemDomain = db.GetDataBySql("select C_CODE,C_NAME from " + codepk + " where " + codewhere+" AND LEFT(C_CODE,6)='520121'");
                } 
                else 
                {
                    ItemDomain = db.GetDataBySql("select C_CODE,C_NAME from " + codepk + " where " + codewhere);
                }
                 
                DataRow[] ItemDomainRows = ItemDomain.Select(null);
                for (int j = 0; j < ItemDomainRows.Length; j++)
                {
                    codedValueDomain.AddCode(ItemDomainRows[j][0].ToString(), ItemDomainRows[j][1].ToString());
                }
                IDomain domain = (IDomain)codedValueDomain;
                int fieldIndex = featureClass.Fields.FindField(drs[i][2].ToString());
                IField field = featureClass.Fields.get_Field(fieldIndex);
                domain.FieldType = field.Type;
                domain.Name = drs[i][2].ToString();
                domain.Description = drs[i][3].ToString();
                domain.MergePolicy = esriMergePolicyType.esriMPTDefaultValue;
                domain.SplitPolicy = esriSplitPolicyType.esriSPTDuplicate;
                if (workspaceDomains.get_DomainByName(drs[i][2].ToString()) == null) 
                {
                    //workspaceDomains.DeleteDomain(drs[i][2].ToString());
                    workspaceDomains.AddDomain(domain);
                }
                ISchemaLock schemaLock = (ISchemaLock)featureClass;
                IClassSchemaEdit classSchemaEdit = (IClassSchemaEdit)featureClass;
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                classSchemaEdit.AlterDomain(drs[i][2].ToString(), domain);
            }
        }

        public static string ValueFromCode(IDomain domain, string convertedValue)
        {
            //string methodName = MethodInfo.GetCurrentMethod().Name;
            string value = string.Empty;

            if (domain == null)
                return value;

            if (domain is ICodedValueDomain)
            {
                ICodedValueDomain codedValueDomain = domain as ICodedValueDomain;
                for (int i = 0; i < codedValueDomain.CodeCount; i++)
                {
                    if (AreStringsEqual(convertedValue, ToText(codedValueDomain.get_Value(i))))
                        value = codedValueDomain.get_Name(i);
                }
            }
            // else
            //  _logger.Log(String.Format("{0} :Domain does not implement ICodedValueDomain.", methodName), LogLevel.enumLogLevelWarn);

            return value;
        }

        public static bool AreStringsEqual(string s1, string s2)
        {
            if (string.Compare(s1, s2, true) == 0)
                return true;
            else
                return false;
        }
        public static string ToText(object value)
        {
            // string methodName = MethodInfo.GetCurrentMethod().Name;
            string ret = string.Empty;

            if (value != DBNull.Value && value != null && value != "")
            {
                try
                {
                    ret = Convert.ToString(value);
                    ret = ret.Trim();
                }
                catch (Exception e)
                {
                    //_logger.LogFormat("{0}: [{1}] {2}", methodName, e.TargetSite, e.Message, LogLevel.enumLogLevelDebug);
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取IFeatureWorkspace
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public IFeatureWorkspace GetFeatureWorkspaceByName(string tablename)
        {
            if (DicWF.ContainsKey(tablename))
            {
                return DicWF[tablename];
            }
            else
            {
                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select * from GISDATA_REGINFO where REG_NAME = '" + tablename.Trim() + "'");
                DataRow[] dr = dt.Select("1=1");
                string path = dr[0]["PATH"].ToString();
                string dbtype = dr[0]["DBTYPE"].ToString();
                IFeatureWorkspace space;
                if (dbtype == "Access数据库")
                {
                    AccessWorkspaceFactory fac = new AccessWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);

                }
                else if (dbtype == "文件夹数据库")
                {
                    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);

                }
                else 
                {
                    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(path, 0);
                }
                DicWF.Add(tablename, space);
                return space;
            }
        }
        /// <summary>
        /// 填充树形结构
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dt"></param>
        public void FillTree(TreeNode node, DataTable dt)
        {
            DataRow[] drr = dt.Select("PARENTID='" + node.Tag.ToString() + "'");
            if (drr.Length > 0)
            {
                for (int i = 0; i < drr.Length; i++)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = drr[i]["NAME"].ToString();
                    tnn.Tag = drr[i]["ID"].ToString();
                    if (drr[i]["PARENTID"].ToString() == node.Tag.ToString())
                    {
                        FillTree(tnn, dt);
                    }
                    node.Nodes.Add(tnn);
                }
            }
        }

        //取消节点选中状态之后，取消所有父节点的选中状态
        public void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        //选中节点之后，选中节点的所有子节点
        public void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes=currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            } 
        }
        /// <summary>
        /// DataGridView添加一列checkbox
        /// </summary>
        /// <param name="dg"></param>
        public void AddCheckBox(DataGridView dg)
        {
            //repositoryItemResourcesComboBox1 combox = new repositoryItemResourcesComboBox();
            DataGridViewCheckBoxColumn columncb = new DataGridViewCheckBoxColumn();
            columncb.HeaderText = "选择";
            columncb.Name = "cb_check";
            columncb.TrueValue = true;
            columncb.FalseValue = false;
            columncb.DataPropertyName = "IsChecked";
            dg.Columns.Insert(0, columncb);    //添加的checkbox在第一列
            //dg.Columns.Add(columncb);     //添加的checkbox在最后一列

        }
        /// <summary>
        /// 在DataGridView控件的CellMouseClick属性中：点击数据勾选上checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e, DataGridView UserGridView)
        {

            //checkbox 勾上
            if ((bool)UserGridView.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
            {
                UserGridView.Rows[e.RowIndex].Cells[0].Value = false;
            }
            else
            {
                UserGridView.Rows[e.RowIndex].Cells[0].Value = true;
            }

        }
        /// <summary>
        /// 设置CheckBox子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        public void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }
        /// <summary>
        /// 设置CheckBox父节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        public void SetCheckedParentNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                if (b)
                {
                    node.ParentNode.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    node.ParentNode.CheckState = check;
                }
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        /// <summary>
        /// 为列头绘制CheckBox
        /// </summary>
        /// <param name="view">GridView</param>
        /// <param name="checkItem">RepositoryItemCheckEdit</param>
        /// <param name="fieldName">需要绘制Checkbox的列名</param>
        /// <param name="e">ColumnHeaderCustomDrawEventArgs</param>
        public static void DrawHeaderCheckBox(GridView view, RepositoryItemCheckEdit checkItem, string fieldName, ColumnHeaderCustomDrawEventArgs e)
        {
            /*说明：
             *参考：https://www.devexpress.com/Support/Center/Question/Details/Q354489
             *在CustomDrawColumnHeader中使用
             *eg：
             * private void gvCabChDetail_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
             * {
             * GridView _view = sender as GridView;
             * _view.DrawHeaderCheckBox(CheckItem, "Check", e);
             * }
             */
            if (e.Column != null && e.Column.FieldName.Equals(fieldName))
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(checkItem, e.Graphics, e.Bounds, getCheckedCount(view, fieldName) == view.DataRowCount);
                e.Handled = true;
            }
        }
        private static void DrawCheckBox(RepositoryItemCheckEdit checkItem, Graphics g, Rectangle r, bool Checked)
        {
            CheckEditViewInfo _info;
            CheckEditPainter _painter;
            ControlGraphicsInfoArgs _args;
            _info = checkItem.CreateViewInfo() as CheckEditViewInfo;
            _painter = checkItem.CreatePainter() as CheckEditPainter;
            _info.EditValue = Checked;

            _info.Bounds = r;
            _info.PaintAppearance.ForeColor = Color.Black;
            _info.CalcViewInfo(g);
            _args = new ControlGraphicsInfoArgs(_info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            _painter.Draw(_args);
            _args.Cache.Dispose();
        }
        private static int getCheckedCount(GridView view, string filedName)
        {
            int count = 0;
            for (int i = 0; i < view.DataRowCount; i++)
            {
                object _cellValue = view.GetRowCellValue(i, view.Columns[filedName]);
                if (_cellValue == null) continue;
                if (string.IsNullOrEmpty(_cellValue.ToString().Trim())) continue;
                bool _checkStatus = false;
                if (bool.TryParse(_cellValue.ToString(), out _checkStatus))
                {
                    if (_checkStatus)
                        count++;
                }
            }
            return count;
        }

        /// <summary>
         /// 修改AppSettings中配置
         /// </summary>
         /// <param name="key">key值</param>
         /// <param name="value">相应值</param>
         public bool SetConfigValue(string key, string value)
         {
             try
             {
                 Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                 if (config.AppSettings.Settings[key] != null)
                     config.AppSettings.Settings[key].Value = value;
                 else
                     config.AppSettings.Settings.Add(key, value);
                 config.Save();
                 ConfigurationManager.RefreshSection("appSettings");
                 return true;
             }
             catch
             {
                 return false;
             }
         }
        /// <summary>
        /// 获取AppSettings中某一节点值
        /// </summary>
        /// <param name="key"></param>
        public string GetConfigValue(string key)
        {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                     return  config.AppSettings.Settings[key].Value;
                else
                  
                return string.Empty;
        }
        /// <summary>
        /// 获取dataset
        /// </summary>
        /// <returns></returns>
        public IFeatureDataset getDataset(IWorkspace workspace) 
        {
            IEnumDataset datasets = workspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IFeatureDataset featureDataset = datasets.Next() as IFeatureDataset;
            while (featureDataset != null)
            {
                if (featureDataset.BrowseName == "dataset")
                {
                    break;
                }
            }
            return featureDataset;
        }

        /// <summary>
        /// //从空间数据库中删除拓扑对象
        /// </summary>
        /// <returns></returns>
        public bool DeleteTopolgyFromGISDB(ITopology top)
        {
            //delete top's ITopologyRuleContainer 
            ITopologyRuleContainer topruleList = top as ITopologyRuleContainer;
            IEnumRule ER = topruleList.Rules;
            ER.Reset();
            IRule r = ER.Next();
            while (r != null && r is ITopologyRule)
            {
                topruleList.DeleteRule(r as ITopologyRule);
                r = ER.Next();
            }
            //delete top's featureclass
            IFeatureClassContainer topFcList = top as IFeatureClassContainer;
            for (int d = topFcList.ClassCount - 1; d >= 0; d--)
            {
                top.RemoveClass(topFcList.get_Class(d) as IClass);
            }
            //delete top object
            (top as IDataset).Delete();
            return true;
        }

        /// <summary>
        /// //从空间数据库中删除所有拓扑对象
        /// </summary>
        /// <returns></returns>
        public bool DeleteALLTopolgyFromGISDB(IEnumDataset topEnumDataset)
        {
            bool rbc = true;

            if (topEnumDataset != null)
            {
                topEnumDataset.Reset();
                IDataset ds = topEnumDataset.Next();
                while (ds != null)
                {
                    switch (ds.Type)
                    {
                        case esriDatasetType.esriDTFeatureDataset:
                            if (ds is ITopologyContainer)
                            {
                                ITopologyContainer topContainer = ds as ITopologyContainer;
                                ISchemaLock schemaLock = (ISchemaLock)ds;
                                try
                                {
                                    schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                                    int tc = topContainer.TopologyCount;
                                    for (int i = tc - 1; i >= 0; i--)
                                    {
                                        ITopology top = topContainer.get_Topology(i);
                                        if (top != null && top is IDataset)
                                        {
                                            //delete top's ITopologyRuleContainer 
                                            ITopologyRuleContainer topruleList = top as ITopologyRuleContainer;
                                            IEnumRule ER = topruleList.Rules;
                                            ER.Reset();
                                            IRule r = ER.Next();
                                            while (r != null && r is ITopologyRule)
                                            {
                                                topruleList.DeleteRule(r as ITopologyRule);
                                                r = ER.Next();
                                            }
                                            //delete top's featureclass
                                            IFeatureClassContainer topFcList = top as IFeatureClassContainer;
                                            for (int d = topFcList.ClassCount - 1; d >= 0; d--)
                                            {
                                                top.RemoveClass(topFcList.get_Class(d) as IClass);
                                            }
                                            //delete top object
                                            (top as IDataset).Delete();
                                            rbc = true;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                finally
                                {
                                    schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                                }
                            }
                            break;
                        case esriDatasetType.esriDTFeatureClass:
                            break;
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ds);
                    ds = topEnumDataset.Next();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(topEnumDataset);
            }//
            return rbc;
        }

        public void setCheckByValue(ComboBox iBox) 
        {
            foreach (object iitem in iBox.Items) 
            {
                IRasterCatalogItem iiitem = (IRasterCatalogItem)iitem;
                string a = "werwer";
            }
        }
    }
}
