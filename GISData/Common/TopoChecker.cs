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

namespace GISData.Common
{
    class TopoChecker//功能：构建拓扑，拓扑检测
    {
        private SelfIntersectChecker _sc;
        private GapsChecker _gcChecker;
        private IHookHelper m_hookHelper;
        List<IFeatureClass> LI_FeatureClass = new List<IFeatureClass>();//要素数据集所包含的所有要素类
        IFeatureDataset FeatureDataset_Main;//拓扑所属的要素数据集
        public Dictionary<string, int> DicTopoError = new Dictionary<string, int>();
        /// <summary>
        /// 构造拓扑检验类
        /// </summary>
        public TopoChecker()
        {
        }
        /// <summary>
        /// 构造拓扑检验类
        /// </summary>
        /// <param name="IN_MainlogyDataSet">输入的要素数据集</param>
        public TopoChecker(IFeatureDataset IN_MainlogyDataSet)
        {
            FeatureDataset_Main = IN_MainlogyDataSet;
            if (LI_FeatureClass.Count != 0)
                LI_FeatureClass.Clear();
            PUB_GetAllFeatureClass();//获取数据集中所有包含的要素类
        }
        /// <summary>
        /// 获取数据集中所有包含的要素类
        /// </summary>
        /// <returns>返回数据集中所有包含的要素类 List<IFeatureClass></returns>
        public List<IFeatureClass> PUB_GetAllFeatureClass()
        {
            if (LI_FeatureClass.Count == 0)
            {
                IFeatureClassContainer Temp_FeatureClassContainer = (IFeatureClassContainer)FeatureDataset_Main;
                IEnumFeatureClass Temp_EnumFeatureClass = Temp_FeatureClassContainer.Classes;
                IFeatureClass Temp_FeatureClass = Temp_EnumFeatureClass.Next();

                while (Temp_FeatureClass != null)
                {
                    LI_FeatureClass.Add(Temp_FeatureClass);
                    Temp_FeatureClass = Temp_EnumFeatureClass.Next();
                }
                if (LI_FeatureClass.Count == 0)
                {
                    MessageBox.Show("空数据集！");
                }
            }
            return LI_FeatureClass;
        }

        public void OtherRule(string idname, string IN_RuleType, IFeatureClass IN_FeatureClass, IFeatureClass IN_Sup_FeatureClass, IHookHelper m_hookHelper)
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
                        list.Add(new ErrorEntity(pFeature.OID.ToString(), "多部件", "", ErrType.MultiPart, pFeature.ShapeCopy));
                        Console.WriteLine(pFeature.OID);
                    }
                    pFeature = cursor.NextFeature();
                }
                Console.WriteLine(tempCount);
                new ErrorTable().AddErr(list, ErrType.MultiPart);
                DicTopoError[idname] = tempCount;
            }
            else if (IN_RuleType == "面自相交检查")
            {
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                this._sc = new SelfIntersectChecker(flay);
                List<ErrorEntity> pErrEntity = this._sc.AreaSelfIntersect(flay,2);
                DicTopoError[idname] = pErrEntity.Count;
                new ErrorTable().AddErr(pErrEntity, ErrType.SelfIntersect);
            }
            else if (IN_RuleType == "缝隙检查")
            {
                this.m_hookHelper = m_hookHelper;
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                IFeatureCursor cursor = flay.Search(null, false);
                IFeature pFeature = cursor.NextFeature();
                this._gcChecker = new GapsChecker(flay, 0);
                List<GapErrorEntity> pErrEntity = new List<GapErrorEntity>();
                int errorCount = 0;
                while (pFeature != null)
                {
                    this._gcChecker = new GapsChecker(flay, 0);
                    object pErrGeo = this._gcChecker.CheckFeatureGap(pFeature, IN_FeatureClass);
                    if (pErrGeo as IGeometry != null)
                    {
                        errorCount++;
                        GapErrorEntity item = new GapErrorEntity(pFeature.OID.ToString(), pErrGeo);
                        pErrEntity.Add(item);
                    }
                    pFeature = cursor.NextFeature();
                }
                Marshal.ReleaseComObject(cursor);
                new ErrorTable().AddGapErr(pErrEntity);
                
                this.DicTopoError[idname] = errorCount;
            }
            else if (IN_RuleType == "面重叠检查")
            {
                this.m_hookHelper = m_hookHelper;
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                OverLapChecker olChecker = new OverLapChecker(flay, 0);
                List<ErrorEntity> pErrEntity = olChecker.CheckOverLap(flay, 3);
                DicTopoError[idname] = pErrEntity.Count;
                new ErrorTable().AddErr(pErrEntity, ErrType.OverLap);
            }
            else if (IN_RuleType == "面重叠检查（与其他图层）")
            {
                List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
                IFeatureLayer pFeatureLayer = new FeatureLayer();
                pFeatureLayer.FeatureClass = IN_Sup_FeatureClass;
                IFeatureCursor pFeatureCursor = IN_FeatureClass.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {//记录集循环
                    IList<IGeometry> list = new List<IGeometry>();
                    ISpatialFilter spatialFilter = new SpatialFilterClass();
                    spatialFilter.Geometry = pFeature.Extent;
                    spatialFilter.GeometryField = IN_FeatureClass.ShapeFieldName;
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor featureCursor = pFeatureLayer.Search(spatialFilter, true);
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
                        ITopologicalOperator2 pRo = (ITopologicalOperator2) pFeature.ShapeCopy;
                        IGeometry pIntersectGeo = new PolygonClass();
                        pIntersectGeo = pRo.Intersect(pGeometry, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;
                        if (pIntersectGeo != null)
                        {
                            list.Add(pIntersectGeo);
                        }
                    }
                    if (list.Count > 0) 
                    {
                        listErrorEntity.Add(new ErrorEntity(pFeature.OID.ToString(), "面重叠检查（与其他图层）", "", ErrType.MultiOverlap, list[0]));
                        Marshal.ReleaseComObject(featureCursor);
                    }
                    pFeature = pFeatureCursor.NextFeature();//移动到下一条记录
                }
                Marshal.ReleaseComObject(pFeatureCursor);
                new ErrorTable().AddErr(listErrorEntity, ErrType.MultiOverlap);
                this.DicTopoError[idname] = listErrorEntity.Count;
            }
            else if (IN_RuleType == "面重叠检查11")
            {
                IFeatureCursor cursor = IN_Sup_FeatureClass.Search(null, false);
                IFeature pFeature = cursor.NextFeature();
                IGeometry pUnionGeo = new PolygonClass();
                pUnionGeo = pFeature.Shape;
                while (pFeature != null)
                {
                    ITopologicalOperator pUnionTopo = pUnionGeo as ITopologicalOperator;



                    pFeature = cursor.NextFeature();
                }
                IGeometry pGeometry = new PolygonClass();
                IFeatureCursor pCursor = IN_FeatureClass.Search(null, false);
                IFeature iFeature = pCursor.NextFeature();
                pGeometry = iFeature.Shape;
                while (iFeature != null)
                {
                    ITopologicalOperator iUnionTopo = pGeometry as ITopologicalOperator;
                    pGeometry = iUnionTopo.Union(iFeature.Shape);
                    iFeature = pCursor.NextFeature();
                }
                ITopologicalOperator  pRo = pGeometry as ITopologicalOperator;
                IGeometry pIntersectGeo = new PolygonClass();
                pIntersectGeo = pRo.Intersect(pUnionGeo, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;
                GenerateSHPFile(pIntersectGeo);
                this.DicTopoError[idname] = 11;
            }
        }


        private void GenerateSHPFile(IGeometry pResultGeometry)
        {
            string filename = "D:/test.shp";
            IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
            IFeatureWorkspace fwp;
            IFeatureLayer flay = new FeatureLayer();
            
            fwp = (IFeatureWorkspace)wsf.OpenFromFile(System.IO.Path.GetDirectoryName(filename), 0);

            IFeatureClass fc = fwp.OpenFeatureClass(System.IO.Path.GetFileName(filename));
            IWorkspaceEdit Temp_WorkspaceEdit = (IWorkspaceEdit)FeatureDataset_Main.Workspace;
            Temp_WorkspaceEdit.StartEditing(true);
            Temp_WorkspaceEdit.StartEditOperation();
            IFeatureBuffer Temp_FeatureBuffer = fc.CreateFeatureBuffer();
            Temp_FeatureBuffer.Shape = pResultGeometry;

            IFeatureCursor featureCursor = fc.Insert(false);
            object featureOID = featureCursor.InsertFeature(Temp_FeatureBuffer);
            featureCursor.Flush();//保存要素  
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
                IPointCollection pPointCollection = modifiedGeo as IPointCollection;
                if (pGeometryDef.HasZ)
                {
                    IZAware pZAware = modifiedGeo as IZAware;
                    pZAware.ZAware = true;
                    IZ iz1 = modifiedGeo as IZ; //若报iz1为空的错误，则将设置Z值的这两句改成IPoint point = (IPoint)pGeo;  point.Z = 0;
                    iz1.SetConstantZ((modifiedGeo as IPoint).Z);//如果此处报错，说明该几何体的点本身都没有Z值，在此处可以自己手动设置Z值,比如0，也就算将此句改成iz1.SetConstantZ(0);
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

        /// <summary>
        /// 获取数据集中的要素类
        /// </summary>
        /// <returns>返回数据集中所有包含的要素类 List<IFeatureClass></returns>
        public IFeatureClass PUB_GetAllFeatureClassByName(string FeatureName)
        {
            IFeatureClass returnFeatureClass = null;
            if (LI_FeatureClass.Count != 0)
            {
                foreach (IFeatureClass itemFeatureClass in LI_FeatureClass)
                {
                    if (itemFeatureClass.AliasName == FeatureName)
                    {
                        returnFeatureClass = itemFeatureClass;
                    }
                }

            }
            return returnFeatureClass;
        }
    }
}
