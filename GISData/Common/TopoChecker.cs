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
        private IHookHelper m_hookHelper;
        private ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = null;
        List<IFeatureClass> LI_FeatureClass = new List<IFeatureClass>();//要素数据集所包含的所有要素类
        public Dictionary<string, int> DicTopoError = new Dictionary<string, int>();
        private string topoDir;
        /// <summary>
        /// 构造拓扑检验类
        /// </summary>
        public TopoChecker()
        {
            CommonClass common = new CommonClass();
            ConnectDB db = new ConnectDB();
            DataTable DT = db.GetDataBySql("select GLDWNAME from GISDATA_GLDW where GLDW = '" + common.GetConfigValue("GLDW") + "'");
            DataRow dr = DT.Select(null)[0];
            this.topoDir = common.GetConfigValue("SAVEDIR") + "\\" + dr["GLDWNAME"].ToString() + "\\错误参考\\拓扑错误";
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
            string ErrorFilePath = this.topoDir + "\\" + IN_RuleType + idname + ".shp";
            if (IN_RuleType == "面多部件检查")
            {
                try
                {
                    common.CreatShpFile(this.topoDir, spatialReference, esriGeometryType.esriGeometryPolygon, IN_RuleType + idname);
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
                int errorCount = 0;
                common.CreatShpFile(this.topoDir, spatialReference, esriGeometryType.esriGeometryPoint, IN_RuleType + idname);
                IFeatureLayer pLayer = new FeatureLayer();
                pLayer.FeatureClass = IN_FeatureClass;
                TopoClassChecker topo = new TopoClassChecker();

                IFeatureCursor cursor;
                IQueryFilter filter = new QueryFilterClass();
                filter.SubFields = pLayer.FeatureClass.OIDFieldName + "," + pLayer.FeatureClass.ShapeFieldName;
                cursor = pLayer.FeatureClass.Search(filter, true);
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
                        errorCount++;

                        string[] strArray = builder.ToString().Substring(1).Split(new char[] { ';' });
                        ESRI.ArcGIS.Geometry.IPointCollection pPointCollection1 = new ESRI.ArcGIS.Geometry.MultipointClass();
                        foreach (string str in strArray)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                string[] strArray2 = str.Split(new char[] { ',' });
                                double pX = double.Parse(strArray2[0]);
                                double pY = double.Parse(strArray2[1]);
                                ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.PointClass();
                                point.X = pX;
                                point.Y = pY;
                                common.GenerateSHPFile(point, ErrorFilePath);
                            }
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
            else if (IN_RuleType == "缝隙检查")
            {
                int errorCount = 0;
                common.CreatShpFile(this.topoDir, spatialReference, esriGeometryType.esriGeometryPolygon, IN_RuleType + idname);
                this.m_hookHelper = m_hookHelper;
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
                string outString = this.topoDir + "\\" + IN_RuleType + idname + ".shp";
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
            else if (IN_RuleType == "面重叠检查（与其他图层）")
            {
                IFeatureClass outIFC = null;
                this.m_hookHelper = m_hookHelper;
                ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
                string outString = this.topoDir + "\\" + IN_RuleType + idname + ".shp";
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
