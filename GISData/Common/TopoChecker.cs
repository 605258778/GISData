using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataManagementTools;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataSourcesFile;
using TopologyCheck.Error;
using TopologyCheck.Checker;
using ESRI.ArcGIS.Controls;
using System.Data;
using System.IO;
using ESRI.ArcGIS.Geoprocessing;

namespace GISData.Common
{
    class TopoChecker//功能：构建拓扑，拓扑检测
    {
        private SelfIntersectChecker _sc;
        private GapsChecker _gcChecker;
        private IHookHelper m_hookHelper;
        private ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = null;
        List<IFeatureClass> LI_FeatureClass = new List<IFeatureClass>();//要素数据集所包含的所有要素类
        public Dictionary<string, int> DicTopoError = new Dictionary<string, int>();
        /// <summary>
        /// 构造拓扑检验类
        /// </summary>
        public TopoChecker()
        {
        }

        public void OtherRule(string idname, string IN_RuleType, IFeatureClass IN_FeatureClass, IFeatureClass IN_Sup_FeatureClass, string inputtext, IHookHelper m_hookHelper)
        {
            if (IN_RuleType == "面多部件检查")
            {
                List<ErrorEntity> list = new List<ErrorEntity>();
                IFeatureCursor cursor = IN_FeatureClass.Search(null, false);
                int tempCount = 0;
                IFeature pFeature = cursor.NextFeature();
                while (pFeature != null)
                {
                    IGeometry pGeo = pFeature.ShapeCopy;
                    ITopologicalOperator pTopoOperator = pGeo as ITopologicalOperator;
                    int iCount = 0;
                    if (IN_FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        iCount = (pGeo as IPolygon).ExteriorRingCount;
                    }
                    else if (IN_FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        iCount = ((pGeo as IPolyline) as IGeometryCollection).GeometryCount;
                    }
                    else if (IN_FeatureClass.ShapeType == esriGeometryType.esriGeometryMultipoint)
                    {
                        iCount = ((pGeo as IMultipoint) as IPointCollection).PointCount;
                    }
                    if (iCount > 1)
                    {
                        tempCount++;
                        list.Add(new ErrorEntity(idname, pFeature.OID.ToString(), "多部件", "", ErrType.MultiPart, pFeature.ShapeCopy));
                        Console.WriteLine(pFeature.OID);
                    }
                    pFeature = cursor.NextFeature();
                }
                Console.WriteLine(tempCount);
                new ErrorTable().AddErr(list, ErrType.MultiPart, idname);
                DicTopoError[idname] = tempCount;
            }
            else if (IN_RuleType == "面自相交检查")
            {
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                this._sc = new SelfIntersectChecker(flay);
                List<ErrorEntity> pErrEntity = this._sc.AreaSelfIntersect(idname, flay, 2);
                DicTopoError[idname] = pErrEntity.Count;
                new ErrorTable().AddErr(pErrEntity, ErrType.SelfIntersect, idname);
            }
            else if (IN_RuleType == "缝隙检查")
            {
                this.m_hookHelper = m_hookHelper;
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                this._gcChecker = new GapsChecker(flay, 0);
                List<Dictionary<string, IGeometry>> pErrGeo =  this._gcChecker.CheckFeatureGap(IN_FeatureClass, inputtext);
                List<GapErrorEntity> pErrEntity = new List<GapErrorEntity>();
                int errorCount = 0;
                if (pErrGeo != null)
                {
                    for (int i = 0; i < pErrGeo.Count; i++)
                    {
                        errorCount++;
                        Dictionary<string, IGeometry> itemGeo = pErrGeo[i];
                        foreach (string key in itemGeo.Keys)
                        {
                            GapErrorEntity item = new GapErrorEntity(idname, key, itemGeo[key]);
                            pErrEntity.Add(item);
                        }
                    }
                }
                new ErrorTable().AddGapErr(pErrEntity, idname);
                this.DicTopoError[idname] = errorCount;
            }
            else if (IN_RuleType == "面重叠检查")
            {
                this.m_hookHelper = m_hookHelper;
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                OverLapChecker olChecker = new OverLapChecker(flay, 0);
                List<ErrorEntity> pErrEntity = olChecker.CheckOverLap(idname, flay, 3);
                DicTopoError[idname] = pErrEntity.Count;
                new ErrorTable().AddErr(pErrEntity, ErrType.OverLap, idname);
            }
            else if (IN_RuleType == "面重叠检查（与其他图层）")
            {
                List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
                IFeatureCursor pFeatureCursor = IN_FeatureClass.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {//记录集循环
                    Console.WriteLine("2cd" + pFeature.OID.ToString());
                    IList<IGeometry> list = new List<IGeometry>();
                    ISpatialFilter spatialFilter = new SpatialFilterClass();
                    spatialFilter.Geometry = pFeature.ShapeCopy;
                    spatialFilter.GeometryField = IN_Sup_FeatureClass.ShapeFieldName;
                    spatialFilter.SubFields = IN_Sup_FeatureClass.OIDFieldName + "," + IN_Sup_FeatureClass.ShapeFieldName;
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor featureCursor = IN_Sup_FeatureClass.Search(spatialFilter, false);
                    IFeature tFeature = featureCursor.NextFeature();//遍历查询结果
                    if (tFeature != null)
                    {
                        IGeometry pGeometry = new PolygonClass();
                        pGeometry = tFeature.ShapeCopy;
                        while (tFeature != null)
                        {
                            ITopologicalOperator2 iUnionTopo = pGeometry as ITopologicalOperator2;
                            pGeometry = iUnionTopo.Union(tFeature.Shape);
                            tFeature = featureCursor.NextFeature();//移动到下一条记录
                        }
                        ITopologicalOperator2 pRo = (ITopologicalOperator2)pFeature.ShapeCopy;
                        IGeometry pIntersectGeo = new PolygonClass();
                        pIntersectGeo = pRo.Intersect(pGeometry, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;
                        if (pIntersectGeo != null && !pIntersectGeo.IsEmpty)
                        {
                            list.Add(pIntersectGeo);
                        }
                    }
                    if (list.Count > 0)
                    {
                        listErrorEntity.Add(new ErrorEntity(idname, pFeature.OID.ToString(), "面重叠检查（与其他图层）", "", ErrType.MultiOverlap, list[0]));
                    }
                    Marshal.ReleaseComObject(pFeature);
                    Marshal.ReleaseComObject(spatialFilter);
                    Marshal.ReleaseComObject(featureCursor);
                    pFeature = pFeatureCursor.NextFeature();//移动到下一条记录
                }
                Marshal.ReleaseComObject(pFeatureCursor);
                new ErrorTable().AddErr(listErrorEntity, ErrType.MultiOverlap, idname);
                this.DicTopoError[idname] = listErrorEntity.Count;
            }
        }

        /// <param name="pFeatureClass">融合要素</param>
        /// <param name="dissField">融合字段</param>
        private IFeatureClass Dissolve(IFeatureClass pFeatureClass, string dissField)
        {
            IFeatureClass pOutFeatureClass = null;
            try
            {
                if (this.gp == null)
                {
                    this.gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                }

                ESRI.ArcGIS.DataManagementTools.Dissolve pDissolve = new ESRI.ArcGIS.DataManagementTools.Dissolve();
                this.gp.OverwriteOutput = true;
                pDissolve.in_features = pFeatureClass;
                pDissolve.dissolve_field = dissField;
                pDissolve.out_feature_class = System.Environment.CurrentDirectory + @"\temp\Dissolve.shp";
                pDissolve.multi_part = "true";     //跨区域融合;
                IGeoProcessorResult result = (IGeoProcessorResult)gp.Execute(pDissolve, null);

                if (result.Status != ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobSucceeded)
                {
                    return null;
                }
                else
                {
                    pOutFeatureClass = this.gp.Open(result.ReturnValue) as IFeatureClass;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
            return pOutFeatureClass;
        }

        /// <param name="pFeatureClass">融合要素</param>
        /// <param name="dissField">融合字段</param>
        /// <param name="outPath">输出数据库路径</param>
        /// <param name="name">输出要素名称</param>
        private IFeatureClass MultipartToSinglepart(IFeatureClass pFeatureClass)
        {
            IFeatureClass pOutFeatureClass = null;

            try
            {
                if (this.gp == null)
                {
                    this.gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                }
                ESRI.ArcGIS.DataManagementTools.MultipartToSinglepart pM2S = new ESRI.ArcGIS.DataManagementTools.MultipartToSinglepart();
                this.gp.OverwriteOutput = true;
                pM2S.in_features = pFeatureClass;
                pM2S.out_feature_class = System.Environment.CurrentDirectory + @"\temp\pM2S.shp";

                IGeoProcessorResult result = (IGeoProcessorResult)gp.Execute(pM2S, null);

                if (result.Status != ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobSucceeded)
                {
                    return null;
                }
                else
                {
                    pOutFeatureClass = this.gp.Open(result.ReturnValue) as IFeatureClass;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
            //IWorkspaceFactory pwf = new ShapefileWorkspaceFactory();
            ////关闭资源锁定   
            //IWorkspaceFactoryLockControl ipWsFactoryLock = (IWorkspaceFactoryLockControl)pwf;
            //if (ipWsFactoryLock.SchemaLockingEnabled)
            //{
            //    ipWsFactoryLock.DisableSchemaLocking();
            //}
            return pOutFeatureClass;
        }

        private void ClearTemp()
        {
            if (System.IO.File.Exists(System.Environment.CurrentDirectory + @"\temp\" + "Dissolve.shp"))
            {
                System.IO.File.Delete(System.Environment.CurrentDirectory + @"\temp\" + "Dissolve.shp");
            }
            if (System.IO.File.Exists(System.Environment.CurrentDirectory + @"\temp\" + "pM2S.shp"))
            {
                System.IO.File.Delete(System.Environment.CurrentDirectory + @"\temp\" + "pM2S.shp");
            }
        }

        public static void Clear_Directors(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        System.IO.File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
