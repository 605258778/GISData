using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GISData.Common
{
    public class CommonClass
    {
        private static Dictionary<string, IFeatureLayer> DicLayer = new Dictionary<string, IFeatureLayer>();
        private static Dictionary<string, IFeatureWorkspace> DicWF = new Dictionary<string, IFeatureWorkspace>();
        private static Dictionary<string, IWorkspace> DicW = new Dictionary<string, IWorkspace>();

        /// <summary>
        /// 获取要素类
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IFeatureClass GetFeatureClassByShpPath(string filePath)
        {
            IFeatureClass pFeatureClass = null;
            try{
                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                IWorkspaceFactoryLockControl pWorkspaceFactoryLockControl = pWorkspaceFactory as IWorkspaceFactoryLockControl;
                if (pWorkspaceFactoryLockControl.SchemaLockingEnabled)
                {
                    pWorkspaceFactoryLockControl.DisableSchemaLocking();
                }
                IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileName(filePath));
                }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
            }
            return pFeatureClass;
        }

        /// <summary>
        /// 获取要素属性表
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public DataTable GetAttributesTable(IFeatureClass pFeatureClass)
        {
            string geometryType = string.Empty;
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                geometryType = "点";
            }
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryMultipoint)
            {
                geometryType = "点集";
            }
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                geometryType = "折线";
            }
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                geometryType = "面";
            }

            // 字段集合
            IFields pFields = pFeatureClass.Fields;
            int fieldsCount = pFields.FieldCount;

            // 写入字段名
            DataTable dataTable = new DataTable();
            for (int i = 0; i < fieldsCount; i++)
            {
                if (pFields.get_Field(i).Name.Contains("FID_"))
                {
                    ISchemaLock pSchemaLock = null;
                    try
                    {
                        pSchemaLock = pFeatureClass as ISchemaLock;
                        pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
                        IClassSchemaEdit4 pClassSchemaEdit = pFeatureClass as IClassSchemaEdit4;
                        pClassSchemaEdit.AlterFieldName(pFields.get_Field(i).Name, "FeatureID");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(typeof(CommonClass), ex);
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        //释放编辑锁
                        pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                        dataTable.Columns.Add("FeatureID");
                    }
                }
                else 
                {
                    dataTable.Columns.Add(pFields.get_Field(i).Name);
                }
            }

            // 要素游标
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, true);
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (pFeature == null)
            {
                return dataTable;
            }

            // 获取MZ值
            IMAware pMAware = pFeature.Shape as IMAware;
            IZAware pZAware = pFeature.Shape as IZAware;
            if (pMAware.MAware)
            {
                geometryType += " M";
            }
            if (pZAware.ZAware)
            {
                geometryType += "Z";
            }

            // 写入字段值
            while (pFeature != null)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < fieldsCount; i++)
                {
                    if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        dataRow[i] = pFeature.ShapeCopy;
                    }
                    else
                    {
                        dataRow[i] = pFeature.get_Value(i).ToString();
                    }
                }
                dataTable.Rows.Add(dataRow);
                pFeature = pFeatureCursor.NextFeature();
            }

            // 释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            return dataTable;
        }

        /// <summary>
        /// 根据注册名获取路径
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetPathByName(string tablename)
        {
            string path = "";
            string table = "";
            try
            {
                RegInfo reginfo = this.getRegInfo(tablename);
                path = reginfo.RegPath;
                table = reginfo.RegTable;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
            }
            return path + "\\" + table;
        }
        /// <summary>
        /// 获取IFeatureLayer
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public IFeatureLayer GetLayerByName(string tablename) 
        {
            try
            {
                RegInfo reginfo = this.getRegInfo(tablename);
                string path = reginfo.RegPath;
                string dbtype = reginfo.RegType;
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
                _Layer.FeatureClass = space.OpenFeatureClass(reginfo.RegTable);
                _Layer.Name = reginfo.RegTable;
                //DicLayer.Add(tablename, _Layer);
                return _Layer;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }
        /// <summary> 
        /// 获取Excel工作薄中Sheet页(工作表)名集合
        /// </summary> 
        /// <param name="excelFile">Excel文件名及路径,EG:C:\Users\JK\Desktop\导入测试.xls</param> 
        /// <returns>Sheet页名称集合</returns> 
        public List<string> GetExcelSheetNames(string fileName)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;
            try
            {
                string connString=string.Empty;
                string FileType =fileName.Substring(fileName.LastIndexOf("."));
                if (FileType == ".xls")  
                 connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + fileName + ";Extended Properties=Excel 8.0;";
                else//.xlsx
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + fileName + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";  
                // 创建连接对象 
                objConn = new OleDbConnection(connString);
                // 打开数据库连接 
                objConn.Open();
                // 得到包含数据架构的数据表 
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                List<string> excelSheets = new List<string>();
                int i = 0;
                // 添加工作表名称到字符串数组 
                foreach (DataRow row in dt.Rows)
                {
                    string strSheetTableName = row["TABLE_NAME"].ToString();
                    //过滤无效SheetName
                    if (strSheetTableName.Contains("$")&&strSheetTableName.Replace("'", "").EndsWith("$"))
                    {
                        excelSheets.Add(strSheetTableName.Substring(0, strSheetTableName.Length - 1));
                    }                   
                    i++;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
            finally
            {
                // 清理 
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataTable GetTableByName(string tablename)
        {
            try
            {
                RegInfo reginfo = this.getRegInfo(tablename);
                string path = reginfo.RegPath;
                string dbtype = reginfo.RegType;
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

                IFeatureClass featureClass = space.OpenFeatureClass(reginfo.RegTable);
                setIDomain(workspaceDomains, featureClass, tablename);
                ITable table = space.OpenTable(reginfo.RegTable.Trim());
                DataTable DT = ToDataTable(table);
                DT.TableName = tablename.Trim();
                return DT;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }

        /// <summary>
        /// 将DataTable的字段名全部翻译为中文
        /// </summary>
        /// <param name="table">待翻译的DataTable</param>
        /// <returns></returns>
        public DataTable TranslateDataTable(DataTable table)
        {
            try
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
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }

        /// <summary>
        /// 得到列名称的别名
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string GetColumnName(string tablename,string columnName ,string value)
        {
            try{
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
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }

        /// <summary>  
        /// 将ITable转换为DataTable  
        /// </summary>  
        /// <param name="mTable"></param>  
        /// <returns></returns>  
        public DataTable ToDataTable(ITable mTable)
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
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }/// <summary>  
        /// 将ITable转换为DataTable  
        /// </summary>  
        /// <param name="mTable"></param>  
        /// <returns></returns>  
        public DataTable ToDataTable(ICursor cursor)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < cursor.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(cursor.Fields.get_Field(i).Name);
                }
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
                LogHelper.WriteLog(typeof(CommonClass), ex);
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
            try{
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
                        CommonClass common = new CommonClass();
                        string gldw = common.GetConfigValue("GLDW") == "" ? "520121" : common.GetConfigValue("GLDW");
                        ItemDomain = db.GetDataBySql("select C_CODE,C_NAME from " + codepk + " where " + codewhere + " AND LEFT(C_CODE,6)='" + gldw + "'");
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
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
            }
        }

        public string ValueFromCode(IDomain domain, string convertedValue)
        {
            try
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
                return value;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }

        public bool AreStringsEqual(string s1, string s2)
        {
            if (string.Compare(s1, s2, true) == 0)
                return true;
            else
                return false;
        }
        public string ToText(object value)
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
            try
            {
                if (DicWF.ContainsKey(tablename))
                {
                    return DicWF[tablename];
                }
                else
                {
                    RegInfo reginfo = this.getRegInfo(tablename);
                    string path = reginfo.RegPath;
                    string dbtype = reginfo.RegType;
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
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }

        /// <summary>
        /// 获取IFeatureWorkspace
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public IWorkspace GetWorkspaceByName(string tablename)
        {
            try
            {
                if (DicW.ContainsKey(tablename))
                {
                    return DicW[tablename];
                }
                else
                {
                    RegInfo reginfo = this.getRegInfo(tablename);
                    string path = reginfo.RegPath;
                    string dbtype = reginfo.RegType;
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
                    DicW.Add(tablename, space as IWorkspace);
                    return space as IWorkspace ;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
                return null;
            }
        }
        /// <summary>
        /// 填充树形结构
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dt"></param>
        public void FillTree(TreeNode node, DataTable dt)
        {
            try
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
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CommonClass), ex);
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
        public void DrawHeaderCheckBox(GridView view, RepositoryItemCheckEdit checkItem, string fieldName, ColumnHeaderCustomDrawEventArgs e)
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
        private void DrawCheckBox(RepositoryItemCheckEdit checkItem, Graphics g, Rectangle r, bool Checked)
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
        private int getCheckedCount(GridView view, string filedName)
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

        public Boolean insertXmlNode(string regname, string regpath, string regtype, string regtable)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"RegInfoFile.xml");
                XmlNode root = doc.SelectSingleNode("regstore");

                XmlElement xelKey = doc.CreateElement("regitem");
                XmlAttribute regName = doc.CreateAttribute("regName");
                regName.InnerText = regname;
                XmlAttribute regPath = doc.CreateAttribute("regPath");
                regPath.InnerText = regpath;
                XmlAttribute regType = doc.CreateAttribute("regType");
                regType.InnerText = regtype;
                XmlAttribute regTable = doc.CreateAttribute("regTable");
                regTable.InnerText = regtable;
                xelKey.SetAttributeNode(regName);
                xelKey.SetAttributeNode(regPath);
                xelKey.SetAttributeNode(regType);
                xelKey.SetAttributeNode(regTable);
                root.AppendChild(xelKey);
                doc.Save(@"RegInfoFile.xml");
                return true;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
                return false;
            }
        }

        public Boolean updateXmlNode(string regname, string regpath, string regtype, string regtable)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"RegInfoFile.xml");
                XmlNode root = doc.SelectSingleNode("regstore");
                XmlElement xe = doc.DocumentElement;
                string strPath = string.Format("/regstore/regitem[@regName=\"{0}\"]", regname);
                if (strPath != null)
                {
                    XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    if (regname != null && regname != "")
                        selectXe.SetAttribute("Type", regname);//也可以通过SetAttribute来增加一个属性
                    if (regpath != null && regpath != "")
                        selectXe.SetAttribute("regPath", regpath);
                    if (regtype != null && regtype != "")
                        selectXe.SetAttribute("regType", regtype);
                    if (regtable != null && regtable != "")
                        selectXe.SetAttribute("regTable", regtable);
                    doc.Save(@"RegInfoFile.xml");
                    return true;
                }
                else 
                {
                    return this.insertXmlNode(regname, regpath, regtype, regtable);
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
                return false;
            }
        }

        public Boolean deleteXmlNode(string regname)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"RegInfoFile.xml");
                XmlNode root = doc.SelectSingleNode("regstore");
                XmlElement xe = doc.DocumentElement;
                string strPath = string.Format("/regstore/regitem[@regName=\"{0}\"]", regname);
                if (strPath != null)
                {
                    XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    selectXe.ParentNode.RemoveChild(selectXe);
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
                return false;
            }
        }

        public RegInfo getRegInfo(string regname) 
        {
            try
            {
                RegInfo reginfo = new RegInfo();
                XmlDocument doc = new XmlDocument();
                doc.Load(@"RegInfoFile.xml");
                XmlNode root = doc.SelectSingleNode("regstore");
                XmlElement xe = doc.DocumentElement;
                string strPath = string.Format("/regstore/regitem[@regName=\"{0}\"]", regname);
                if (strPath != null)
                {
                    XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    reginfo.RegName = regname;
                    reginfo.RegPath = selectXe.GetAttribute("regPath");
                    reginfo.RegType = selectXe.GetAttribute("regType");
                    reginfo.RegTable = selectXe.GetAttribute("regTable");
                    return reginfo;
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

        /// <summary>
        /// 创建shp文件
        /// </summary>
        /// <param name="shpFullFilePath"></param>
        /// <param name="spatialReference"></param>
        /// <param name="pGeometryType"></param>
        /// <param name="shpFileName"></param>
        public void CreatShpFile(string shpFullFilePath, ISpatialReference spatialReference, esriGeometryType pGeometryType, string shpFileName)
        {
            string pFileName = shpFullFilePath +"\\"+ shpFileName + ".shp";
            try
            {
                string shpFolder = System.IO.Path.GetDirectoryName(shpFullFilePath);
                IWorkspaceFactory pWorkspaceFac = new ShapefileWorkspaceFactoryClass();
                IWorkspace pWorkSpace = pWorkspaceFac.OpenFromFile(shpFullFilePath, 0);
                IFeatureWorkspace pFeatureWorkSpace = pWorkSpace as IFeatureWorkspace;
                //如果文件已存在               
                if (System.IO.File.Exists(pFileName))
                {
                    IFeatureClass pFCChecker = pFeatureWorkSpace.OpenFeatureClass(shpFileName);
                    if (pFCChecker != null)
                    {
                        IDataset pds = pFCChecker as IDataset;
                        pds.Delete();
                    }
                }
                IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass();
                IObjectClassDescription pObjectDescription = (IObjectClassDescription)fcDescription;
                IFields fields = pObjectDescription.RequiredFields;
                int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);
                IField field = fields.get_Field(shapeFieldIndex);
                IGeometryDef geometryDef = field.GeometryDef;
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                //线
                geometryDefEdit.GeometryType_2 = pGeometryType; //geometry类型
                geometryDefEdit.HasZ_2 = true;                  //使图层具有Z值(无，则创建一个二维文件，不具备Z值)

                geometryDefEdit.SpatialReference_2 = spatialReference;

                IFieldChecker fieldChecker = new FieldCheckerClass();
                IEnumFieldError enumFieldError = null;
                IFields validatedFields = null; //将传入字段 转成 validatedFields
                fieldChecker.ValidateWorkspace = pWorkSpace;
                fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                pFeatureWorkSpace.CreateFeatureClass(shpFileName, validatedFields, pObjectDescription.InstanceCLSID, pObjectDescription.ClassExtensionCLSID, esriFeatureType.esriFTSimple, fcDescription.ShapeFieldName, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 讲Geometry 存入shp
        /// </summary>
        /// <param name="pResultGeometry"></param>
        /// <param name="filename"></param>
        public void GenerateSHPFile(IGeometry pResultGeometry, string filename)
        {
            IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
            IFeatureWorkspace fwp;
            //IFeatureLayer flay = new FeatureLayer();
            
            fwp = (IFeatureWorkspace)wsf.OpenFromFile(System.IO.Path.GetDirectoryName(filename), 0);
            IFeatureClass featureClass = fwp.OpenFeatureClass(System.IO.Path.GetFileName(filename));

            IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();
            IFeatureCursor featureCursor = featureClass.Insert(true);

            featureBuffer.Shape = this.ModifyGeomtryZMValue(featureClass, pResultGeometry);
            object featureOID = featureCursor.InsertFeature(featureBuffer);
            featureCursor.Flush();
        }

        private IGeometry ModifyGeomtryZMValue(IObjectClass featureClass, IGeometry modifiedGeo)
        {
            try
            {
                IFeatureClass trgFtCls = featureClass as IFeatureClass;
                if (trgFtCls == null) return null;
                string shapeFieldName = trgFtCls.ShapeFieldName;
                IFields fields = trgFtCls.Fields;
                int geometryIndex = fields.FindField(shapeFieldName);
                IField field = fields.get_Field(geometryIndex);
                IGeometryDef pGeometryDef = field.GeometryDef;
                if (pGeometryDef.HasZ)
                {
                    IZAware pZAware = modifiedGeo as IZAware;
                    pZAware.ZAware = true;
                    IZ iz1 = modifiedGeo as IZ; //若报iz1为空的错误，则将设置Z值的这两句改成IPoint point = (IPoint)pGeo;  point.Z = 0;
                    if (iz1 == null)
                    {
                        try
                        {
                            IPoint point = (IPoint)modifiedGeo;
                            point.Z = 0;
                        }
                        catch(Exception ex)
                        {
                        
                        }
                        
                    }
                    else 
                    {
                        try
                        {
                            if ((modifiedGeo as IPoint) != null)
                            {
                                iz1.SetConstantZ((modifiedGeo as IPoint).Z);//如果此处报错，说明该几何体的点本身都没有Z值，在此处可以自己手动设置Z值,比如0，也就算将此句改成iz1.SetConstantZ(0);
                            }
                            else 
                            {
                                iz1.SetConstantZ(0);
                            }
                        }
                        catch (Exception ex) 
                        {
                            iz1.SetConstantZ(0);//如果此处报错，说明该几何体的点本身都没有Z值，在此处可以自己手动设置Z值,比如0，也就算将此句改成iz1.SetConstantZ(0);
                        }
                    }
                }
                else
                {
                    IZAware pZAware = modifiedGeo as IZAware;
                    pZAware.ZAware = false;
                }
                if (pGeometryDef.HasM)
                {
                    IMAware pMAware = modifiedGeo as IMAware;
                    pMAware.MAware = true;
                }
                else
                {
                    IMAware pMAware = modifiedGeo as IMAware;
                    pMAware.MAware = false;
                }
                return modifiedGeo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "ModifyGeomtryZMValue error");
                return modifiedGeo;
            }
        }
    }
}
