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
using System.Diagnostics;

namespace GISData.Common
{
    class TopoChecker//功能：构建拓扑，拓扑检测
    {
        private SelfIntersectChecker _sc;
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
        public void OtherRule(string idname, string IN_RuleType, string TABLENAME, string SUPTABLE, string inputtext, IHookHelper m_hookHelper)
        {
            CommonClass common = new CommonClass();
            
            IFeatureClass IN_FeatureClass = common.GetLayerByName(TABLENAME).FeatureClass;
            IGeoDataset pGeoDataset = IN_FeatureClass as IGeoDataset;
            ISpatialReference spatialReference = pGeoDataset.SpatialReference;
            IFeatureClass IN_Sup_FeatureClass = null;
            if (SUPTABLE != null) 
            {
                IN_Sup_FeatureClass = common.GetLayerByName(SUPTABLE).FeatureClass;
            
            }
            string ErrorFilePath = Application.StartupPath + "\\TopoError\\" + IN_RuleType + idname + ".shp";
            if (IN_RuleType == "面多部件检查")
            {
                try
                {
                    common.CreatShpFile(Application.StartupPath + "\\TopoError", spatialReference, esriGeometryType.esriGeometryPolygon, IN_RuleType + idname);
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
                            //list.Add(new ErrorEntity(idname, pFeature.OID.ToString(), "多部件", "", ErrType.MultiPart, pFeature.ShapeCopy));
                            common.GenerateSHPFile(pFeature.ShapeCopy, ErrorFilePath);
                        }
                        pFeature = cursor.NextFeature();
                    }
                    //new ErrorTable().AddErr(list, ErrType.MultiPart, idname);
                    if (!DicTopoError.ContainsKey(idname))
                    {
                        DicTopoError.Add(idname, tempCount);
                    }
                    else
                    {
                        DicTopoError[idname] = tempCount;
                    }
                }
                catch (Exception ex) 
                {
                    LogHelper.WriteLog(typeof(TopoChecker), ex);
                }
            }
            else if (IN_RuleType == "面自相交检查")
            {
                common.CreatShpFile(Application.StartupPath + "\\TopoError", spatialReference,esriGeometryType.esriGeometryPoint, IN_RuleType + idname);
                IFeatureLayer pLayer = new FeatureLayer();
                pLayer.FeatureClass = IN_FeatureClass;
                TopoClassChecker topo = new TopoClassChecker();
                //List<ErrorEntity> pErrEntity = topo.AreaSelfIntersect(idname, flay);

                IFeatureCursor cursor;
                IQueryFilter filter = new QueryFilterClass();
                filter.SubFields = pLayer.FeatureClass.OIDFieldName + "," + pLayer.FeatureClass.ShapeFieldName;
                cursor = pLayer.FeatureClass.Search(filter, true);
                //IFieldEdit edit = pLayer.FeatureClass.Fields.get_Field(pLayer.FeatureClass.Fields.FindField(pLayer.FeatureClass.ShapeFieldName)) as IFieldEdit;
                //ISpatialReference spatialReference = edit.GeometryDef.SpatialReference;
                List<ErrorEntity> list = new List<ErrorEntity>();
                IFeature feature = null;
                object missing = Type.Missing;
                while ((feature = cursor.NextFeature()) != null)
                {
                    IPoint tempPoint = null;
                    StringBuilder builder = new StringBuilder();
                    IGeometryCollection shape = feature.Shape as IGeometryCollection;
                    for (int i = 0; i < shape.GeometryCount; i++)
                    {
                        esriNonSimpleReasonEnum enum2;
                        IPointCollection newPoints = shape.get_Geometry(i) as IPointCollection;
                        IRing ring = newPoints as IRing;
                        int num2 = 0;
                        if (ring.IsClosed)
                        {
                            num2 = 1;
                        }
                        PolylineClass o = new PolylineClass();
                        o.AddPointCollection(newPoints);
                        o.SpatialReference = spatialReference;

                        ITopologicalOperator3 @operator = o;
                        @operator.IsKnownSimple_2 = false;
                        if (!@operator.get_IsSimpleEx(out enum2) && (enum2 == esriNonSimpleReasonEnum.esriNonSimpleSelfIntersections))
                        {
                            List<string> list2 = new List<string>();
                            List<string> list3 = new List<string>();
                            for (int j = num2; j < newPoints.PointCount; j++)
                            {
                                IPoint point = newPoints.get_Point(j);
                                tempPoint = point;
                                string item = point.X.ToString() + "," + point.Y.ToString();
                                if (list2.Contains(item))
                                {
                                    if (!list3.Contains(item))
                                    {
                                        builder.Append(";");
                                        builder.Append(item);
                                        list3.Add(item);
                                    }
                                }
                                else
                                {
                                    list2.Add(item);
                                }
                            }
                        }
                        Marshal.ReleaseComObject(o);
                        o = null;
                    }
                    if (builder.Length > 0)
                    {
                        list.Add(new ErrorEntity(idname, feature.OID.ToString(), "自相交", builder.ToString().Substring(1), ErrType.SelfIntersect, tempPoint));
                        common.GenerateSHPFile(tempPoint, ErrorFilePath);
                    }
                }

                if (!DicTopoError.ContainsKey(idname))
                {
                    DicTopoError.Add(idname, list.Count);
                }
                else
                {
                    DicTopoError[idname] = list.Count;
                }
                new ErrorTable().AddErr(list, ErrType.SelfIntersect, idname);
            }
            else if (IN_RuleType == "缝隙检查")
            {
                int errorCount = 0;
                common.CreatShpFile(Application.StartupPath + "\\TopoError", spatialReference, esriGeometryType.esriGeometryPolygon, IN_RuleType + idname);
                this.m_hookHelper = m_hookHelper;
                //IFeatureLayer flay = new FeatureLayer();
                //flay.FeatureClass = IN_FeatureClass;
                //TopoClassChecker topo = new TopoClassChecker();
                //List<Dictionary<string, IGeometry>> pErrGeo = topo.CheckFeatureGap(IN_FeatureClass, inputtext);
                IFeatureClass pFClass = IN_FeatureClass;
                //获取空间参考
                IGeometry geometryBag = new GeometryBagClass();
                IGeoDataset geoDataset = pFClass as IGeoDataset;
                geometryBag.SpatialReference = geoDataset.SpatialReference;

                ////属性过滤
                IFeatureCursor featureCursor = pFClass.Search(null, false);

                // 遍历游标
                IFeature currentFeature = featureCursor.NextFeature();
                IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
                object missing = Type.Missing;
                while (currentFeature != null)
                {
                    geometryCollection.AddGeometry(currentFeature.Shape, ref missing, ref missing);
                    currentFeature = featureCursor.NextFeature();
                }

                // 合并要素
                ITopologicalOperator unionedPolygon = null;
                unionedPolygon = new Polygon() as ITopologicalOperator;
                unionedPolygon.ConstructUnion(geometryCollection as IEnumGeometry);

                Marshal.ReleaseComObject(featureCursor);
                IPolygon4 pMergerPolygon = unionedPolygon as IPolygon4;
                IGeometryBag pOutGeometryBag = pMergerPolygon.ExteriorRingBag;  //获取外部环
                IGeometryCollection pOutGmtyCollection = pOutGeometryBag as IGeometryCollection;

                for (int i = 0; i < pOutGmtyCollection.GeometryCount; i++)  //对外部环遍历
                {
                    IGeometry pOutRing = pOutGmtyCollection.get_Geometry(i); //外部环
                    //【此处可以对外部环进行操作】
                    IPointCollection pOutRingCollection = pOutRing as IPointCollection;
                    for (int j = 0; j < pOutRingCollection.PointCount; j++)
                    {
                        IPoint pOutRingPoint = pOutRingCollection.get_Point(j);//获取外环上的点
                    }

                    IGeometryBag pInteriotGeometryBag = pMergerPolygon.get_InteriorRingBag(pOutRing as IRing);  //获取内部环
                    IGeometryCollection pInteriorGeometryCollection = pInteriotGeometryBag as IGeometryCollection;

                    for (int j = 0; j < pInteriorGeometryCollection.GeometryCount; j++)
                    {
                        ISegmentCollection SegCol = pInteriorGeometryCollection.get_Geometry(j) as ISegmentCollection;

                        IPolygon PPolygon = new PolygonClass();
                        ISegmentCollection newSegCol = PPolygon as ISegmentCollection;
                        newSegCol.AddSegmentCollection(SegCol);
                        //pInteriorGeometry即为多边形的内部环
                        IGeometry inRing = PPolygon as IGeometry;
                        inRing.SpatialReference = geometryBag.SpatialReference;
                        IArea area = inRing as IArea;
                        Double getarea = System.Math.Abs(Convert.ToDouble(area.Area));
                        if (inputtext == null || inputtext == "" || getarea < Convert.ToDouble(inputtext))
                        {
                            Boolean flag = true;
                            ISpatialFilter filter = new SpatialFilterClass();
                            filter.Geometry = inRing;
                            filter.SubFields = pFClass.OIDFieldName + "," + pFClass.ShapeFieldName;
                            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            IFeatureCursor o = pFClass.Search(filter, true);
                            IFeature feature = o.NextFeature();
                            while (feature != null && flag)
                            {
                                IPolygon4 pPolygon = feature.Shape as IPolygon4;
                                IGeometryBag iOutGeometryBag = pPolygon.ExteriorRingBag;  //获取外部环
                                IGeometryCollection iOutGmtyCollection = iOutGeometryBag as IGeometryCollection;

                                for (int m = 0; m < iOutGmtyCollection.GeometryCount && flag; m++)  //对外部环遍历
                                {
                                    IGeometry outGeo = iOutGmtyCollection.get_Geometry(m);
                                    IGeometryCollection polyGonGeo = new PolygonClass();
                                    polyGonGeo.AddGeometry(outGeo);
                                    IPolygon iPolygon = polyGonGeo as IPolygon;
                                    iPolygon.SimplifyPreserveFromTo();
                                    IRelationalOperator2 pRelationalOperator2 = iPolygon as IRelationalOperator2;
                                    if (!pRelationalOperator2.Contains(inRing))
                                    {
                                        errorCount++;
                                        common.GenerateSHPFile(inRing, ErrorFilePath);
                                        flag = false;
                                    }
                                }
                                feature = o.NextFeature();
                            }
                            Marshal.ReleaseComObject(o);
                        }
                    }
                }
                if (!DicTopoError.ContainsKey(idname))
                {
                    DicTopoError.Add(idname, errorCount);
                }
                else
                {
                    DicTopoError[idname] = errorCount;
                }
            }
            else if (IN_RuleType == "面重叠检查")
            {
                IFeatureClass outIFC = null;
                this.m_hookHelper = m_hookHelper;
                ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
                string outString = Application.StartupPath + "\\TopoError\\" + IN_RuleType + idname + ".shp";
                intersect.in_features = common.GetPathByName(TABLENAME);
                intersect.out_feature_class = outString;
                Geoprocessor geoProcessor = new Geoprocessor();
                geoProcessor.OverwriteOutput = true;
                try 
                {
                    if (this.gp == null)
                    {
                        this.gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                    }
                    IGeoProcessorResult result = (IGeoProcessorResult)this.gp.Execute(intersect, null);

                    if (result.Status != ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobSucceeded)
                    {
                        Console.WriteLine("gp工具执行错误");
                    }
                    else
                    {
                        outIFC = this.gp.Open(result.ReturnValue) as IFeatureClass;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (!DicTopoError.ContainsKey(idname))
                {
                    DicTopoError.Add(idname, outIFC.FeatureCount(null));
                }
                else
                {
                    DicTopoError[idname] = outIFC.FeatureCount(null);
                }
            }
            else if (IN_RuleType == "面重叠检查111")
            {
                this.m_hookHelper = m_hookHelper;
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                TopoClassChecker topo = new TopoClassChecker();
                List<ErrorEntity> pErrEntity = topo.CheckOverLap(idname, flay);
                if (!DicTopoError.ContainsKey(idname))
                {
                    DicTopoError.Add(idname, pErrEntity.Count);
                }
                else
                {
                    DicTopoError[idname] = pErrEntity.Count;
                }
                new ErrorTable().AddErr(pErrEntity, ErrType.OverLap, idname);
            }
            else if (IN_RuleType == "面重叠检查（与其他图层）")
            {
                IFeatureClass outIFC = null;
                this.m_hookHelper = m_hookHelper;
                ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
                string outString = Application.StartupPath + "\\TopoError\\" + IN_RuleType + idname + ".shp";
                intersect.in_features = @common.GetPathByName(TABLENAME) + ";" + @common.GetPathByName(SUPTABLE);
                intersect.out_feature_class = outString;
                Geoprocessor geoProcessor = new Geoprocessor();
                geoProcessor.OverwriteOutput = true;
                try
                {
                    if (this.gp == null)
                    {
                        this.gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                    }
                    IGeoProcessorResult result = (IGeoProcessorResult)this.gp.Execute(intersect, null);

                    if (result.Status != ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobSucceeded)
                    {
                        Console.WriteLine("gp工具执行错误");
                    }
                    else
                    {
                        outIFC = this.gp.Open(result.ReturnValue) as IFeatureClass;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (!DicTopoError.ContainsKey(idname))
                {
                    DicTopoError.Add(idname, outIFC.FeatureCount(null));
                }
                else
                {
                    DicTopoError[idname] = outIFC.FeatureCount(null);
                }
            }
            else if (IN_RuleType == "面重叠检查（与其他图层）1111")
            {
                List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
                IFeatureCursor pFeatureCursor = IN_FeatureClass.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {//记录集循环
                    IList<IGeometry> list = new List<IGeometry>();
                    ISpatialFilter spatialFilter = new SpatialFilterClass();
                    spatialFilter.Geometry = pFeature.ShapeCopy;
                    spatialFilter.GeometryField = IN_Sup_FeatureClass.ShapeFieldName;
                    spatialFilter.SubFields = IN_Sup_FeatureClass.OIDFieldName + "," + IN_Sup_FeatureClass.ShapeFieldName;
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor featureCursor = IN_Sup_FeatureClass.Search(spatialFilter, true);
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
                if (!DicTopoError.ContainsKey(idname))
                {
                    DicTopoError.Add(idname, listErrorEntity.Count);
                }
                else
                {
                    DicTopoError[idname] = listErrorEntity.Count;
                }
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
