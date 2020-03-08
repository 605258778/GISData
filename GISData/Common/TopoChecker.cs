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

namespace GISData.Common
{
    class TopoChecker : FormMain//功能：构建拓扑，拓扑检测
    {
        private SelfIntersectChecker _sc;
        
        private Dictionary<int, List<IElement>> elements = new Dictionary<int, List<IElement>>();
        private Dictionary<string, dynamic> errorDic = new Dictionary<string, dynamic>();
        List<IFeatureClass> LI_FeatureClass = new List<IFeatureClass>();//要素数据集所包含的所有要素类
        IFeatureDataset FeatureDataset_Main;//拓扑所属的要素数据集
        private IHookHelper m_hookHelper = null;
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
                        Console.WriteLine(pFeature.OID);
                    }
                    pFeature = cursor.NextFeature();
                }
                Console.WriteLine(tempCount);
                //DicTopoError[idname] = tempCount;
            }else if (IN_RuleType == "面自相交检查")
            {
                this.m_hookHelper = m_hookHelper;
                IFeatureLayer flay = new FeatureLayer();
                flay.FeatureClass = IN_FeatureClass;
                this._sc = new SelfIntersectChecker(flay);
                IFeatureCursor cursor = flay.Search(null, false);
                IFeature pFeature = cursor.NextFeature();
                while (pFeature != null)
                {
                    List<double[]> list = new List<double[]>();
                    object pErrFeatureInf = list;
                    AxMapControl axMapControl = Control.FromHandle(new IntPtr(this.m_hookHelper.ActiveView.ScreenDisplay.hWnd)) as AxMapControl;
                    IGraphicsContainer focusMap = this.m_hookHelper.ActiveView.FocusMap as IGraphicsContainer;
                    if (!this._sc.CheckFeature(pFeature, ref pErrFeatureInf))
                    {
                        Console.WriteLine("拓扑错误！");
                        if (this.elements.ContainsKey(pFeature.OID))
                        {
                            List<IElement> list2 = this.elements[pFeature.OID];
                            foreach (IElement element in list2)
                            {
                                Console.WriteLine("error！");
                            }
                            this.elements[pFeature.OID].Clear();
                        }
                        else
                        {
                            List<IElement> list3 = new List<IElement>();
                            this.elements.Add(pFeature.OID, list3);
                        }
                        foreach (double[] numArray in list)
                        {
                            double pX = numArray[0];
                            double pY = numArray[1];
                            Console.WriteLine(pX.ToString()+","+pY.ToString());
                            IElement item = ErrManager.CreateMarkerElement(this.m_hookHelper.ActiveView, pX, pY, (flay.FeatureClass as IGeoDataset).SpatialReference);
                            this.elements[pFeature.OID].Add(item);
                            focusMap.AddElement(item, 0);
                        }
                        this.errorDic.Add("面自相交检查", this.elements);
                        this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, this.m_hookHelper.ActiveView.Extent);
                    }
                    else
                    {
                        if (this.elements.ContainsKey(pFeature.OID))
                        {
                            List<IElement> list4 = this.elements[pFeature.OID];
                            foreach (IElement element3 in list4)
                            {
                                focusMap.DeleteElement(element3);
                            }
                            this.elements.Remove(pFeature.OID);
                            this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, this.m_hookHelper.ActiveView.Extent);
                        }
                        Console.WriteLine("拓扑正确！");
                    }
                    pFeature = cursor.NextFeature();
                }
            }
            else if (IN_RuleType == "面自相交检查11")
            {
                IFeatureCursor cursor = IN_FeatureClass.Search(null, false);
                IFeature pFeature = cursor.NextFeature();
                int tempCount = 0;
                while (pFeature != null)
                {
                    IPolygon4 polygon = pFeature.ShapeCopy as IPolygon4;
                    IGeometryBag bag = polygon.ExteriorRingBag;
                    IEnumGeometry geo = bag as IEnumGeometry;
                    geo.Reset();
                    int iCount = 0;
                    IRing exRing = geo.Next() as IRing;
                    while (exRing != null)
                    {
                        if (exRing.IsExterior)
                        {
                            iCount++;
                        }
                        exRing = geo.Next() as IRing;
                    }
                    if (iCount > 1)
                    {
                        tempCount++;
                    }
                    pFeature = cursor.NextFeature();
                }
                //DicTopoError[idname] = tempCount;
            }
            else if (IN_RuleType == "面重叠检查")
            {
                IFeatureCursor cursor = IN_Sup_FeatureClass.Search(null, false);
                IFeature pFeature = cursor.NextFeature();
                IGeometry pUnionGeo = new PolygonClass();
                pUnionGeo = pFeature.Shape;
                while (pFeature != null)
                {
                    ITopologicalOperator pUnionTopo = pUnionGeo as ITopologicalOperator;
                    pUnionGeo = pUnionTopo.Union(pFeature.Shape);
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
    class DictionaryDic
    {
        public Dictionary<int, List<IElement>> Code 
        { 
            get 
            { 
                return _Code;
            } 
            set 
            { 
                _Code = value;
            }
        }
        private Dictionary<int, List<IElement>> _Code;
        public string Page 
        { 
            get 
            { 
                return _Page;
            } 
            set 
            { 
                _Page = value;
            } 
        } 
        private string _Page;
    } 
}
