namespace TaskManage
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 编辑任务结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct EditTask
    {
        public static string KindCode;
        public static string TaskName;
        public static string DistCode;
        public static TaskState2 TaskState;
        /// <summary>
        /// 编辑任务年度
        /// </summary>
        public static string TaskYear;
        public static string CreateTime;
        public static string EditTime;
        public static string DatasetName;
        public static string LayerName;
        public static string TableName;
        /// <summary>
        /// 编辑任务ID
        /// </summary>
        public static long TaskID;
        public static IFeatureLayer EditLayer;
        public static IFeatureLayer UnderLayer;
        public static ArrayList UnderLayers;
        public static ITable UnderTable;
        public static LogicCheckState LogicChkState;
        public static ToplogicCheckState ToplogicChkState;
        public static IFeatureWorkspace EditWorkspace;
        public static IList<ILayer> LayerList;
        public static IFeatureLayer TFHLayer;
        public static string PZWH;
        static EditTask()
        {
            KindCode = "";
            TaskName = "";
            DistCode = "";
            TaskState = TaskState2.Create;
            TaskYear = "";
            CreateTime = "";
            EditTime = "";
            DatasetName = "";
            LayerName = "";
            TableName = "";
            EditLayer = null;
            UnderLayer = null;
            UnderLayers = null;
            UnderTable = null;
            LogicChkState = LogicCheckState.Failure;
            ToplogicChkState = ToplogicCheckState.Failure;
            EditWorkspace = null;
            LayerList = new List<ILayer>(30);
            TFHLayer = null;
            PZWH = "";
        }
    }
}

