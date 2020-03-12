namespace TaskManage
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.DataSourcesGDB;
    using ESRI.ArcGIS.Geodatabase;
    using FunFactory;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Management;
    using td.db.orm;
    using td.logic.sys;
    using Utilities;

    /// <summary>
    /// 任务管理器
    /// </summary>
    public class TaskManageClass
    {
        /// <summary>
        /// 日志检查状态：默认为失败
        /// </summary>
        public static TaskManage.LogicCheckState LogicCheckState = TaskManage.LogicCheckState.Failure;
        /// <summary>
        ///  mClassName = "TaskManage.TaskManageClass";
        /// </summary>
        private const string mClassName = "TaskManage.TaskManageClass";
        private static ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private static string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        /// <summary>
        /// 任务状态：默认为关闭
        /// </summary>
        public static TaskManage.TaskState TaskState = TaskManage.TaskState.Close;
        /// <summary>
        /// 顶部日志检查状态：默认为失败
        /// </summary>
        public static TaskManage.ToplogicCheckState ToplogicCheckState = TaskManage.ToplogicCheckState.Failure;

        public static string CreateMachineCode()
        {
            return (GetCpuCode() + GetDiskVolumnSerialNumber()).Substring(0, 0x18);
        }

        private static IFeatureWorkspace CreateVersion(IWorkspace _workspace, string verName)
        {
            try
            {
                IVersionedWorkspace workspace = (IVersionedWorkspace)_workspace;
                IVersion defaultVersion = workspace.DefaultVersion;
                IVersion version2 = null;
                try
                {
                    version2 = workspace.FindVersion(verName);
                    if (version2 == null)
                    {
                        version2 = defaultVersion.CreateVersion(verName);
                        string str = Environment.MachineName + "创建的版本";
                        version2.Description = str;
                    }
                }
                catch (Exception)
                {
                    if (version2 == null)
                    {
                        version2 = defaultVersion.CreateVersion(verName);
                        string str2 = Environment.MachineName + "创建的版本";
                        version2.Description = str2;
                    }
                }
                version2.Access = esriVersionAccess.esriVersionAccessPublic;
                IVersionedWorkspace workspace2 = (IVersionedWorkspace)version2;
                IVersion version3 = workspace2 as IVersion;
                return (version3 as IFeatureWorkspace);
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "CreateVersion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public static string GetCpuCode()
        {
            string str = string.Empty;
            ManagementClass class2 = new ManagementClass("win32_Processor");
            foreach (ManagementObject obj2 in class2.GetInstances())
            {
                return obj2.Properties["Processorid"].Value.ToString();
            }
            return str;
        }
        public static DataTable GetDataTable(string WhereStr)
        {
            try
            {


                DataSet dataSet = new DataSet();

                return dataSet.Tables[0];
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate(mSubSysName, "TaskManage.TaskManageClass", "GetDataTable", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }


        public static DataTable GetDataTable(string TableName, string FieldsName, string OrderByStr, string WhereStr, bool flag)
        {
            try
            {
                string sCmdText = "Select " + FieldsName + " From " + TableName;
                if (WhereStr != "")
                {
                    sCmdText = sCmdText + " Where " + WhereStr;
                }
                if (OrderByStr != "")
                {
                    sCmdText = sCmdText + " ORDER BY " + OrderByStr;
                }

                DataSet dataSet = new DataSet();

                return dataSet.Tables[0];
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate(mSubSysName, "TaskManage.TaskManageClass", "GetDataTable", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public static string GetDiskVolumnSerialNumber()
        {
            try
            {
                new ManagementClass("Win32_NetworkAdapterConfiguration");
                string str = UtilFactory.GetConfigOpt().RootPath.Split(new char[] { '\\' })[0].ToLower();
                ManagementObject obj2 = null;
                switch (str)
                {
                    case "d:":
                        obj2 = new ManagementObject("win32_logicaldisk.deviceid=\"d:\"");
                        break;

                    case "c:":
                        obj2 = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                        break;
                }
                if (obj2 != null)
                {
                    obj2.Get();
                    return obj2.GetPropertyValue("VolumeSerialNumber").ToString();
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        private static PathManager PathManager
        {
            get
            {
                return DBServiceFactory<PathManager>.Service;
            }
        }

        /// <summary>
        /// 获取数据库的工作空间
        /// </summary>
        /// <param name="bversion"></param>
        /// <returns></returns>
        private static IFeatureWorkspace GetFeatureWorkspace(bool bversion)
        {
            try
            {
                //<MapDBkey Name="空间数据库位置 Local or SqlServer">Local</MapDBkey>
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("MapDBkey");
                IFeatureWorkspace featureWorkspace = null;
                switch (configValue)
                {
                    case "Local"://此项目使用的是"Local"
                        {//<EditDataPath Name="编辑数据库路径">Template\GX.gdb</EditDataPath>
                            string sSourceFile = UtilFactory.GetConfigOpt().RootPath + @"\" + PathManager.FindValue("EditDataPath");
                            if (sSourceFile.Contains(".gdb") || sSourceFile.Contains(".GDB"))
                            {
                                featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSFileGDBWorkspaceFactory);
                            }
                            else if (sSourceFile.Contains(".mdb") || sSourceFile.Contains(".MDB"))
                            {
                                featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSAccessWorkspaceFactory);
                            }
                            break;
                        }
                    case "SqlServer":
                        featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(WorkspaceSource.esriWSSdeWorkspaceFactory);
                        
                        
                       if (bversion)
                        {
                            string version = GetVersion();
                           featureWorkspace = CreateVersion(featureWorkspace as IWorkspace, version);
                       }
                        break;
                }
                return featureWorkspace;
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "GetFeatureWorkspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        private static string GetVersion()
        {
            try
            {
                string str = vGUIDGen.GetCUPID().Trim() + vGUIDGen.GetIDEInfo().Trim();
                if (str.Length > 0x11)
                {
                    return str.Trim();
                }
                string str2 = "";
                ManagementClass class2 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                foreach (ManagementObject obj2 in class2.GetInstances())
                {
                    if ((bool)obj2["IPEnabled"])
                    {
                        str2 = str2 + obj2["MacAddress"].ToString() + " ";
                        break;
                    }
                }
                class2 = null;
                return str2.Trim();
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "GetVersion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return "";
            }
        }

        /// <summary>
        /// 在任务管理器中初始化编辑的值(重要的是要连接工作空间)
        /// </summary>
        /// <param name="mEditKind"></param>
        /// <param name="year"></param>
        /// <param name="pMap"></param>
        public static void InitialEditValues(string mEditKind, string year, IMap pMap)
        {
            try
            {
                string str = "";
                string sLayerName = "";
                string str3 = "";
                string str4 = "";
                string str5 = "";
                string str6 = "";
                bool bversion = true;
                if (mEditKind == "造林")
                {
                    str = "ZaoLin";
                    str4 = "01";
                    bversion = false;

                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind == "采伐")
                {
                    str = "CaiFa";
                    str4 = "02";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind == "林业工程")
                {
                    str = "LYGC";
                    str4 = "03";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind.Contains("征占用"))
                {
                    str = "ZZY";
                    str4 = "04";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind.Contains("火灾"))
                {
                    str = "Fire";
                    str4 = "05";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind.Contains("自然灾害"))
                {
                    str = "ZRZH";
                    str4 = "06";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind.Contains("案件"))
                {
                    str = "AnJian";
                    str4 = "07";
                    ///非版本化
                    bversion = false;
                }
                else if ((mEditKind == "小班变更") || (mEditKind == "年度小班"))
                {
                    str = "XB";
                    str4 = "08";
                    bversion = false;
                }
                else if (mEditKind == "遥感")
                {
                    str = "YG";
                    str4 = "09";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind == "二类变化")
                {
                    str = "ELBH";
                    str4 = "10";
                    ///非版本化
                    bversion = false;
                }
                else if (mEditKind == "系统管理")
                {
                    str = "XB";
                    str4 = "08";
                    bversion = false;
                    mEditKind = "年度小班";
                }
                //IFeatureWorkspace界面提供对创建和打开各种类型的数据集和其他工作空间级别对象的成员的访问。
                //IFeatureWorkspace接口用于访问和管理作为基于特征的地理数据库的关键组件的数据集;表和ObjectClasses，FeatureClasses，FeatureDatasets和RelationshipClasses。
                //所有Open方法（如OpenTable）都将数据集名称作为输入。使用企业级地理数据库时，名称可能是完全限定的（例如“database.owner.tablename”或“owner.tablename”），使用适用于底层数据库的限定字符（请参阅ISQLSyntax））。如果输入名称未完全限定，则使用当前连接的用户进行工作区限定。
                //在使用地理数据库（个人，文件或ArcSDE）时，工作区会保留实例化数据集的正在运行的对象表。打开已实例化数据集的多个调用将返回对已实例化数据集的引用。
                IFeatureWorkspace featureWorkspace = null;
                featureWorkspace = GetFeatureWorkspace(bversion);
                if (featureWorkspace != null)
                {
                    IWorkspace workspace2 = featureWorkspace as IWorkspace;
                    string str7 = workspace2.ConnectionProperties.GetProperty("database").ToString();
                    if (str7 != null)
                    {
                        string[] strArray = str7.Split(new char[] { '_' });
                        if (strArray.Length == 3)
                        {
                            if (strArray[2].Length == 4)
                            {
                                year = strArray[2];
                            }
                            if (strArray[1].Length == 6)
                            {
                                EditTask.DistCode = strArray[1];
                            }
                        }
                        else
                        {
                            string str8 = UtilFactory.GetConfigOpt().GetConfigValue(str + "Dataset");
                            ///IEnumDataset接口提供对通过Datasets枚举的成员的访问。数据集（esriTrackingAnalyst）控制与数据集相关的功能。RasterBands（esriDataSourcesRaster）一个枚举器，用于遍历一组光栅带。
                            ///IFeatureWorkspace.OpenFeatureDataset方法打开现有要素数据集。OpenFeatureDataset方法可用于打开工作区中的任何现有要素数据集，其中包含完整限定名称。使用IDatabaseConnectionInfo接口来确定用户和数据库（如果适用）。 ISQLSyntax :: QualifyTableName可用于确定要素数据集的完全限定名称。
                            IEnumDataset subsets = featureWorkspace.OpenFeatureDataset(str8).Subsets;
                            IDataset dataset3 = subsets.Next();
                            str8 = UtilFactory.GetConfigOpt().GetConfigValue(str + "Layer");
                            while (dataset3 != null)
                            {
                                string[] strArray2 = dataset3.Name.Split(new char[] { '.' });
                                string str9 = strArray2[strArray2.Length - 1];
                                if ((dataset3.Name.Contains(str8 + "_") && (str9.Length == (str8.Length + 5))) && (int.Parse(str9.Substring(str8.Length + 1, 4)) > 0x3e8))
                                {
                                    year = str9.Substring(str8.Length + 1, 4);
                                    break;
                                }
                                dataset3 = subsets.Next();
                            }
                        }
                    }
                    EditTask.EditWorkspace = featureWorkspace;
                    EditTask.TaskName = mEditKind;
                    if ((pMap != null) && (pMap.Description != ""))
                    {
                        string[] strArray3 = pMap.Description.Split(new char[] { ',' });
                        if (strArray3.Length == 2)
                        {
                            if (strArray3[1].Contains("TASK_ID="))
                            {
                                EditTask.TaskID = long.Parse(strArray3[1].Replace("TASK_ID=", ""));
                            }
                            else
                            {
                                EditTask.TaskID = long.Parse(strArray3[1]);
                            }

                            DataTable table = GetDataTable("T_EditTask_ZT", "*", "", "ID='" + EditTask.TaskID + "'", false);
                            if (table.Rows.Count > 0)
                            {
                                DataRow row = table.Rows[0];
                                TaskState = TaskManage.TaskState.Open;
                                LogicCheckState = TaskManage.LogicCheckState.Failure;
                                ToplogicCheckState = TaskManage.ToplogicCheckState.Failure;
                                EditTask.KindCode = row["taskkind"].ToString();
                                EditTask.TaskName = row["taskname"].ToString();
                                EditTask.DistCode = row["distcode"].ToString();
                                EditTask.TaskState = (TaskState2)int.Parse(row["taskstate"].ToString());
                                EditTask.TaskYear = row["taskyear"].ToString();
                                EditTask.CreateTime = row["createtime"].ToString();
                                EditTask.EditTime = row["edittime"].ToString();
                                EditTask.DatasetName = row["datasetname"].ToString();
                                EditTask.LayerName = row["layername"].ToString();
                                EditTask.TableName = row["tablename"].ToString();
                                EditTask.TaskID = long.Parse(row["ID"].ToString());
                                if (row["taskpath"] != null)
                                {
                                    EditTask.PZWH = row["taskpath"].ToString();
                                }
                                if (row["logiccheckstate"].ToString() == "1")
                                {
                                    EditTask.LogicChkState = TaskManage.LogicCheckState.Success;
                                }
                                else if (row["logiccheckstate"].ToString() == "0")
                                {
                                    EditTask.LogicChkState = TaskManage.LogicCheckState.Failure;
                                }
                                if (row["logiccheckstate"].ToString() == "1")
                                {
                                    EditTask.ToplogicChkState = TaskManage.ToplogicCheckState.Success;
                                }
                                else if (row["logiccheckstate"].ToString() == "0")
                                {
                                    EditTask.ToplogicChkState = TaskManage.ToplogicCheckState.Failure;
                                }
                            }
                        }
                    }
                    if (EditTask.DistCode.Length != 6)
                    {//<DicTableName>T_SYS_META_CODE</DicTableName>:此表格里封装有山西临县各乡镇的详细数据
                        string str10 = UtilFactory.GetConfigOpt().GetConfigValue("DicTableName");
                        ITable table2 = featureWorkspace.OpenTable(str10);
                        IQueryFilter queryFilter = new QueryFilterClass
                        {
                            WhereClause = "CINDEX='103'"//"CINDEX='103'"：临县=‘103’
                        };
                        ESRI.ArcGIS.Geodatabase.IRow row2 = table2.Search(queryFilter, false).NextRow();
                        int index = row2.Fields.FindField("CCODE");
                        if (index > -1)
                        {
                            EditTask.DistCode = row2.get_Value(index).ToString();
                        }
                        else
                        {
                            EditTask.DistCode = "142326";
                        }
                    }
                    IFeatureLayer layer = null;
                    IFeatureLayer layer2 = null;
                    EditTask.KindCode = str4;
                    EditTask.DatasetName = UtilFactory.GetConfigOpt().GetConfigValue(str + "Dataset");
                    EditTask.LayerName = UtilFactory.GetConfigOpt().GetConfigValue(str + "Layer") + "_" + year;
                    if (mEditKind == "年度小班")
                    {
                        EditTask.LayerName = UtilFactory.GetConfigOpt().GetConfigValue("XBLayer1") + "_" + year;
                    }
                    EditTask.TaskYear = year;
                    UtilFactory.GetConfigOpt().SetConfigValue("EditYear", year);
                    EditTask.TableName = UtilFactory.GetConfigOpt().GetConfigValue(str + "TableName");
                    if (featureWorkspace != null)
                    {
                        if (str == "YG")
                        {
                            sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str + "GroupName2");
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName2");
                        }
                        else
                        {
                            sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str + "GroupName");
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName");
                        }
                        if (mEditKind == "年度小班")
                        {
                            sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str + "GroupName2");
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName1");
                            str5 = UtilFactory.GetConfigOpt().GetConfigValue("XBLayer") + "_" + year;
                            str6 = UtilFactory.GetConfigOpt().GetConfigValue("XBLayerName");
                        }
                        else if (mEditKind == "征占用")
                        {
                            str5 = UtilFactory.GetConfigOpt().GetConfigValue("ZZYLayer") + "_" + year;
                            str6 = UtilFactory.GetConfigOpt().GetConfigValue("ZZYLayerName0");
                        }
                        IGroupLayer pGroupLayer = null;
                        if (pMap != null)
                        {
                            pGroupLayer = GISFunFactory.LayerFun.FindLayer(pMap as IBasicMap, sLayerName, true) as IGroupLayer;
                            if (pGroupLayer == null)
                            {
                                sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str + "GroupName3");
                                pGroupLayer = GISFunFactory.LayerFun.FindLayer(pMap as IBasicMap, sLayerName, true) as IGroupLayer;
                            }
                            if (pGroupLayer != null)
                            {
                                layer = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, str3, true) as IFeatureLayer;
                                if (layer == null)
                                {
                                    str3 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName0");
                                    layer = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, str3, true) as IFeatureLayer;
                                }
                            }
                        }
                        if (layer == null)
                        {
                            layer = new FeatureLayerClass();
                        }
                        layer.Name = str3;
                        //  IDataset pdataset = featureWorkspace.OpenFeatureDataset("FORDATA_141100_2016.DBO.FOREST");
                        IDataset pdataset = featureWorkspace.OpenFeatureDataset(EditTask.DatasetName);
                        if (!(workspace2.WorkspaceFactory is FileGDBWorkspaceFactory) && !(workspace2.WorkspaceFactory is AccessWorkspaceFactory))
                        {
                            RegeditDataset(pdataset);
                        }
                        IFeatureDataset dataset5 = pdataset as IFeatureDataset;
                        IEnumDataset dataset6 = dataset5.Subsets;
                        IDataset dataset7 = dataset6.Next();
                        IFeatureClass class2 = null;
                        IFeatureClass class3 = null;
                        while (dataset7 != null)
                        {
                            if (dataset7.Type == esriDatasetType.esriDTFeatureClass)
                            {
                                class2 = dataset7 as IFeatureClass;
                                string[] strArray4 = dataset7.Name.Split(new char[] { '.' });
                                if (dataset7.Name.Contains(EditTask.LayerName) && (strArray4[strArray4.Length - 1] == EditTask.LayerName))
                                {
                                    class3 = class2;
                                    break;
                                }
                            }
                            dataset7 = dataset6.Next();
                        }
                        try
                        {
                            layer.FeatureClass = class3;
                            if (!(workspace2.WorkspaceFactory is FileGDBWorkspaceFactory) && !(workspace2.WorkspaceFactory is AccessWorkspaceFactory))
                            {
                                RegeditFeatureClass(layer.FeatureClass);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        EditTask.EditLayer = layer;
                        EditTask.UnderLayers = new ArrayList();
                        string[] strArray5 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName2").Split(new char[] { ',' });
                        string[] strArray6 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName3").Split(new char[] { ',' });
                        string[] strArray7 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName22").Split(new char[] { ',' });
                        string[] strArray8 = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName33").Split(new char[] { ',' });
                        if ((strArray6.Length == 1) && (strArray6[0] == ""))
                        {
                            strArray6 = UtilFactory.GetConfigOpt().GetConfigValue(str + "Layer2").Split(new char[] { ',' });
                            str6 = strArray5[0];
                            str5 = strArray6[0];
                        }
                        if (((strArray6.Length == 1) && (strArray6[0] == "")) && (strArray5[0] != ""))
                        {
                            strArray6[0] = UtilFactory.GetConfigOpt().GetConfigValue(str + "Layer");
                            str6 = strArray5[0];
                            str5 = strArray6[0];
                        }
                        if (((strArray5[0] != "") && (strArray5.Length > 1)) && (strArray5.Length == strArray6.Length))
                        {
                            for (int i = 0; i < strArray5.Length; i++)
                            {
                                if (pGroupLayer != null)
                                {
                                    layer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, strArray5[i], true) as IFeatureLayer;
                                }
                                if (layer2 != null)
                                {
                                    try
                                    {
                                        class2 = featureWorkspace.OpenFeatureClass(strArray6[i] + "_" + year);
                                        layer2.FeatureClass = class2;
                                        EditTask.UnderLayers.Add(layer2);
                                        if (!(workspace2.WorkspaceFactory is FileGDBWorkspaceFactoryClass) && !(workspace2.WorkspaceFactory is AccessWorkspaceFactoryClass))
                                        {
                                            RegeditFeatureClass(layer2.FeatureClass);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                        if ((((strArray7[0] != "") && (strArray7.Length > 1)) && ((strArray7.Length == strArray8.Length) && (mEditKind != "年度小班"))) && (mEditKind != "小班变更"))
                        {
                            for (int j = 0; j < strArray7.Length; j++)
                            {
                                if (pGroupLayer != null)
                                {
                                    layer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, strArray7[j], true) as IFeatureLayer;
                                }
                                if (layer2 != null)
                                {
                                    try
                                    {
                                        class2 = featureWorkspace.OpenFeatureClass(strArray8[j] + "_" + year);
                                        layer2.FeatureClass = class2;
                                        EditTask.UnderLayers.Add(layer2);
                                        if (!(workspace2.WorkspaceFactory is FileGDBWorkspaceFactoryClass) && !(workspace2.WorkspaceFactory is AccessWorkspaceFactoryClass))
                                        {
                                            RegeditFeatureClass(layer2.FeatureClass);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                        if ((str5 != "") && (str6 != ""))
                        {
                            if (str == "ELBH")
                            {
                                str5 = str5 + "_" + ((int.Parse(EditTask.TaskYear) - 1)).ToString();
                            }
                            if (pGroupLayer != null)
                            {
                                EditTask.UnderLayer = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, str6, true) as IFeatureLayer;
                            }
                            if (EditTask.UnderLayer != null)
                            {
                                EditTask.UnderLayer.Name = str6;
                                dataset6.Reset();
                                for (dataset7 = dataset6.Next(); dataset7 != null; dataset7 = dataset6.Next())
                                {
                                    if (dataset7.Type == esriDatasetType.esriDTFeatureClass)
                                    {
                                        class2 = dataset7 as IFeatureClass;
                                        string[] strArray9 = dataset7.Name.Split(new char[] { '.' });
                                        if (strArray9[strArray9.Length - 1] == str5)
                                        {
                                            class3 = class2;
                                            break;
                                        }
                                    }
                                }
                                if (class3 != null)
                                {
                                    EditTask.UnderLayer.FeatureClass = class3;
                                    if (!(workspace2.WorkspaceFactory is FileGDBWorkspaceFactoryClass) && !(workspace2.WorkspaceFactory is AccessWorkspaceFactoryClass))
                                    {
                                        RegeditFeatureClass(EditTask.UnderLayer.FeatureClass);
                                    }
                                }
                            }
                        }
                    }
                    string configValue = UtilFactory.GetConfigOpt().GetConfigValue("TfhLayerName");
                    string name = UtilFactory.GetConfigOpt().GetConfigValue("TfhClassName");
                    EditTask.TFHLayer = GISFunFactory.LayerFun.FindFeatureLayer(pMap as IBasicMap, configValue, true);
                    IFeatureClass featureClass = null;
                    if (EditTask.TFHLayer != null)
                    {
                        featureClass = EditTask.TFHLayer.FeatureClass;
                    }
                    else
                    {
                        try
                        {
                            featureClass = featureWorkspace.OpenFeatureClass(name);
                            EditTask.TFHLayer = new FeatureLayerClass();
                            EditTask.TFHLayer.Name = configValue;
                            EditTask.TFHLayer.FeatureClass = featureClass;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "InitialEditValues", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private static bool RegeditDataset(IDataset pdataset)
        {
            try
            {
                bool flag;
                bool flag2;
                IVersionedObject3 obj2 = (IVersionedObject3)pdataset;
                obj2.GetVersionRegistrationInfo(out flag, out flag2);
                if (!flag)
                {
                    if (!flag2)
                    {
                        obj2.RegisterAsVersioned3(true);
                    }
                    obj2.GetVersionRegistrationInfo(out flag, out flag2);
                    return flag;
                }
                return false;
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "RegeditFeatureClass", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private static bool RegeditFeatureClass(IFeatureClass pfeatureclass)
        {
            try
            {
                bool flag;
                bool flag2;
                IVersionedObject3 obj2 = (IVersionedObject3)pfeatureclass;
                obj2.GetVersionRegistrationInfo(out flag, out flag2);
                if (!flag)
                {
                    if (!flag2)
                    {
                        obj2.RegisterAsVersioned3(true);
                    }
                    obj2.GetVersionRegistrationInfo(out flag, out flag2);
                    return flag;
                }
                return false;
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "RegeditFeatureClass", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public static void SetEditLayer(string mEditKind, string year, IMap pMap)
        {
            try
            {
                bool flag = false;
                string str = "";
                string sLayerName = "";
                string str3 = "";
                string str4 = "";
                IFeatureLayer editLayer = EditTask.EditLayer;
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("XBGroupName");
                if (mEditKind.Contains("年度"))
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("XBGroupName2");
                }
                IGroupLayer pGroupLayer = GISFunFactory.LayerFun.FindLayer(pMap as IBasicMap, configValue, true) as IGroupLayer;
                if ((pGroupLayer == null) && GISFunFactory.LayerFun.AddGroupLayer(pMap as IBasicMap, null, configValue))
                {
                    pGroupLayer = GISFunFactory.LayerFun.FindLayer(pMap as IBasicMap, configValue, true) as IGroupLayer;
                }
                if (mEditKind.Contains("年度"))
                {
                    str = UtilFactory.GetConfigOpt().GetConfigValue("XBLayer1") + "_" + year;
                    sLayerName = UtilFactory.GetConfigOpt().GetConfigValue("XBLayerName1");
                    if ((EditTask.EditLayer.FeatureClass as IDataset).Name != str)
                    {
                        flag = true;
                    }
                    str3 = UtilFactory.GetConfigOpt().GetConfigValue("XBLayer") + "_" + year;
                    str4 = UtilFactory.GetConfigOpt().GetConfigValue("XBLayerName");
                }
                else
                {
                    str = UtilFactory.GetConfigOpt().GetConfigValue("XBLayer") + "_" + year;
                    sLayerName = UtilFactory.GetConfigOpt().GetConfigValue("XBLayerName");
                    if ((EditTask.EditLayer.FeatureClass as IDataset).Name != str)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    if (pGroupLayer != null)
                    {
                        editLayer = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, sLayerName, true) as IFeatureLayer;
                    }
                    if (editLayer == null)
                    {
                        editLayer = new FeatureLayerClass();
                    }
                    editLayer.Name = sLayerName;
                    IEnumDataset subsets = EditTask.EditWorkspace.OpenFeatureDataset(EditTask.DatasetName).Subsets;
                    IDataset dataset3 = subsets.Next();
                    IFeatureClass class2 = null;
                    IFeatureClass class3 = null;
                    while (dataset3 != null)
                    {
                        if (dataset3.Type == esriDatasetType.esriDTFeatureClass)
                        {
                            class2 = dataset3 as IFeatureClass;
                            if (dataset3.Name.Substring(dataset3.Name.Length - str.Length, str.Length) == str)
                            {
                                class3 = class2;
                                break;
                            }
                        }
                        dataset3 = subsets.Next();
                    }
                    EditTask.LayerName = str;
                    try
                    {
                        editLayer.FeatureClass = class3;
                    }
                    catch (Exception)
                    {
                    }
                    EditTask.EditLayer = editLayer;
                    if ((str3 != "") && (str4 != ""))
                    {
                        if (pGroupLayer != null)
                        {
                            EditTask.UnderLayer = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, str4, true) as IFeatureLayer;
                        }
                        if (EditTask.UnderLayer == null)
                        {
                            EditTask.UnderLayer = new FeatureLayerClass();
                        }
                        EditTask.UnderLayer.Name = str4;
                        subsets.Reset();
                        for (dataset3 = subsets.Next(); dataset3 != null; dataset3 = subsets.Next())
                        {
                            if (dataset3.Type == esriDatasetType.esriDTFeatureClass)
                            {
                                class2 = dataset3 as IFeatureClass;
                                if (dataset3.Name.Substring(dataset3.Name.Length - str3.Length, str3.Length) == str3)
                                {
                                    class3 = class2;
                                    break;
                                }
                            }
                        }
                        if (class3 != null)
                        {
                            EditTask.UnderLayer.FeatureClass = class3;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                mErrOpt.ErrorOperate("TaskManage.TaskManageClass", "TaskManage.TaskManageClass", "SetEditLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
    }
}

