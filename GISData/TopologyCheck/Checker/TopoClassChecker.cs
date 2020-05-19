namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.Geoprocessing;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    //using TaskManage;
    using TopologyCheck.Base;
    using TopologyCheck.Error;
    //using Utilities;

    internal class TopoClassChecker : ITopologyChecker
    {
        private static int _checkType = 0;
        private TopologyCheck.Error.ErrType _errType;
        private IGeometry _geo;
        private static IFeatureLayer _layer;
        private IList<IFeatureClass> m_FCList;
        private string m_TempPath;
        private List<string> GapsList;

        public TopoClassChecker(IList<IFeatureClass> pList)
        {
            this.m_TempPath = "";
            if ((pList == null) || (pList.Count < 1))
            {
                throw new Exception("图层数据为空！");
            }
            if (this.m_TempPath == "")
            {
                this.m_TempPath = "D://temp";
            }
            this.m_FCList = pList;
            this.GapsList = new List<string>();
        }

        public TopoClassChecker(IFeatureLayer pLayer, int iCheckType)
        {
            this.m_TempPath = "";
            _layer = pLayer;
            _checkType = iCheckType;
            IFeatureClass featureClass = pLayer.FeatureClass;
            if (featureClass == null)
            {
                throw new Exception("图层数据为空！");
            }
            if (this.m_TempPath == "")
            {
                this.m_TempPath = "D://temp";
            }
            if (iCheckType == 1)
            {
                string name = ((IDataset) featureClass).Name;
                string sTargetName = name.Substring(name.LastIndexOf('.') + 1) + "_task";
                IFeatureLayerDefinition definition = (IFeatureLayerDefinition) pLayer;
                string definitionExpression = definition.DefinitionExpression;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = featureClass.OIDFieldName + "," + featureClass.ShapeFieldName;
                pQueryFilter.WhereClause = definitionExpression;
                //featureClass = SpatialAnalysis.ExportToShp((IWorkspace) EditTask.EditWorkspace, featureClass, this.m_TempPath, sTargetName, pQueryFilter);
            }
            if (featureClass == null)
            {
                throw new Exception("图层数据为空！");
            }
            this.m_FCList = new List<IFeatureClass>();
            this.m_FCList.Add(featureClass);
            this.GapsList = new List<string>();
        }

        public virtual bool Check(string idname)
        {
            IFeatureClass pFClass = null;
            if ((this._errType == TopologyCheck.Error.ErrType.OverLap) || (this._errType == TopologyCheck.Error.ErrType.MultiOverlap))
            {
                pFClass = SpatialAnalysis.Intersect(this.m_FCList, this.m_TempPath + @"\intersect.shp");
                if (pFClass == null)
                {
                    return true;
                }
                this.WriteError(pFClass, idname);
            }
            else if (this._errType == TopologyCheck.Error.ErrType.Gap)
            {
                IFeatureClass class3 = this.m_FCList[0];
                if (this._geo == null)
                {
                    this.CheckGap(idname,class3);
                }
                else
                {
                    this.CheckGap1(idname,class3);
                }
            }
            return false;
        }

        public virtual bool CheckFeature(IFeature pFeature, ref object pErrFeatureInf)
        {
            pErrFeatureInf = null;
            try
            {
                IGeometry shapeCopy = pFeature.ShapeCopy;
                ITopologicalOperator2 @operator = null;
                if (shapeCopy.GeometryType != esriGeometryType.esriGeometryPoint)
                {
                    @operator = (ITopologicalOperator2) shapeCopy;
                    @operator.IsKnownSimple_2 = false;
                    @operator.Simplify();
                }
                IFeatureClass featureClass = _layer.FeatureClass;
                if (this._errType == TopologyCheck.Error.ErrType.OverLap)
                {
                    IFeature feature;
                    IList<IGeometry> list = new List<IGeometry>();
                    ISpatialFilter filter = new SpatialFilterClass();
                    filter.Geometry = shapeCopy;
                    filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps;
                    IFeatureCursor o = featureClass.Search(filter, false);
                    for (feature = o.NextFeature(); feature != null; feature = o.NextFeature())
                    {
                        IGeometry item = feature.ShapeCopy;
                        if (@operator == null)
                        {
                            list.Add(item);
                        }
                        else
                        {
                            IGeometry geometry3 = @operator.Intersect(item, esriGeometryDimension.esriGeometry2Dimension);
                            list.Add(geometry3);
                        }
                    }
                    Marshal.ReleaseComObject(o);
                    filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    o = featureClass.Search(filter, false);
                    for (feature = o.NextFeature(); feature != null; feature = o.NextFeature())
                    {
                        if (feature.OID != pFeature.OID)
                        {
                            IGeometry geometry4 = feature.ShapeCopy;
                            if (@operator == null)
                            {
                                list.Add(geometry4);
                            }
                            else
                            {
                                IGeometry geometry5 = @operator.Intersect(geometry4, esriGeometryDimension.esriGeometry2Dimension);
                                list.Add(geometry5);
                            }
                        }
                    }
                    Marshal.ReleaseComObject(o);
                    filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                    o = featureClass.Search(filter, false);
                    for (feature = o.NextFeature(); feature != null; feature = o.NextFeature())
                    {
                        if (feature.OID != pFeature.OID)
                        {
                            IGeometry geometry6 = feature.ShapeCopy;
                            if (@operator == null)
                            {
                                list.Add(geometry6);
                            }
                            else
                            {
                                IGeometry geometry7 = @operator.Intersect(geometry6, esriGeometryDimension.esriGeometry2Dimension);
                                list.Add(geometry7);
                            }
                        }
                    }
                    Marshal.ReleaseComObject(o);
                    if (list.Count < 1)
                    {
                        return true;
                    }
                    pErrFeatureInf = list;
                    return false;
                }
                if (this._errType == TopologyCheck.Error.ErrType.Gap)
                {
                    IGeometry boundary = @operator.Boundary;
                    IPolyline polyline = boundary as IPolyline;
                    ITopologicalOperator2 operator2 = (ITopologicalOperator2) boundary;
                    operator2.IsKnownSimple_2 = false;
                    operator2.Simplify();
                    ISpatialFilter filter2 = new SpatialFilterClass();
                    filter2.Geometry = polyline;
                    filter2.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor cursor2 = featureClass.Search(filter2, false);
                    IFeature feature2 = cursor2.NextFeature();
                    IList<IGeometry> list2 = new List<IGeometry>();
                    while (feature2 != null)
                    {
                        if (feature2.OID != pFeature.OID)
                        {
                            IGeometry geometry9 = operator2.Intersect(feature2.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                            list2.Add(geometry9);
                        }
                        feature2 = cursor2.NextFeature();
                    }
                    Marshal.ReleaseComObject(cursor2);
                    IList<IGeometry> list3 = new List<IGeometry>();
                    if (list2.Count == 0)
                    {
                        list3.Add(boundary);
                    }
                    else
                    {
                        IGeometry other = null;
                        for (int i = 0; i < list2.Count; i++)
                        {
                            IGeometry geometry11 = list2[i];
                            if (other == null)
                            {
                                other = geometry11;
                            }
                            else
                            {
                                ITopologicalOperator2 operator3 = (ITopologicalOperator2) other;
                                operator3.IsKnownSimple_2 = false;
                                operator3.Simplify();
                                other = operator3.Union(geometry11);
                            }
                        }
                        IGeometry geometry12 = null;
                        geometry12 = operator2.Difference(other);
                        if (geometry12.IsEmpty)
                        {
                            return true;
                        }
                        list3.Add(geometry12);
                    }
                    pErrFeatureInf = list3;
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ErrorEntity> CheckOverLap(string idname,IFeatureLayer pLayer, int iCheckType)
        {
            

            IFeatureCursor cursor = pLayer.Search(null, true);
            List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
            IFeature pFeature = null;

            //IGeometryCollection pGeometryCollection = new GeometryBagClass();
            

            while ((pFeature = cursor.NextFeature()) != null)
            {
                //pGeometryCollection.AddGeometry(pFeature.ShapeCopy);
                ITopologicalOperator2 @operator = (ITopologicalOperator2)pFeature.ShapeCopy;
                @operator.IsKnownSimple_2 = false;
                @operator.Simplify();

                IFeature feature;

                IIdentify pIdentify = pLayer as IIdentify;
                IArray pIDs = pIdentify.Identify((IGeometry)pFeature.ShapeCopy);
                if (pIDs != null && pIDs.Count > 1)
                {
                    //取第一个实体
                    for (int i = 0; i < pIDs.Count; i++)
                    {
                        feature = (pIDs.get_Element(i) as IRowIdentifyObject).Row as IFeature;
                        if (feature != null && pFeature.OID != feature.OID)
                        {
                            
                            IGeometry item = feature.ShapeCopy;
                            IGeometry geometry3 = @operator.Intersect(item, esriGeometryDimension.esriGeometry2Dimension);
                            if (!geometry3.IsEmpty) 
                            {
                                Console.WriteLine(pFeature.OID + "-" + feature.OID);
                                listErrorEntity.Add(new ErrorEntity(idname,pFeature.OID.ToString(), "自重叠", feature.OID.ToString(), ErrType.OverLap, geometry3));
                            }
                        }
                    }

                }
                
            }
            //ISpatialIndex pSpatialIndex = pGeometryCollection as ISpatialIndex;
            //pSpatialIndex.AllowIndexing = true;
            //pSpatialIndex.Invalidate();
            return listErrorEntity;
        }

        public object CheckFeatureGapcopy(IFeature pFeature, IFeatureClass pFClass)
        {
            
            IList<IGeometry> list = new List<IGeometry>();
            IGeometry shapeCopy = pFeature.ShapeCopy;
            if (shapeCopy.IsEmpty)
            {
                return null;
            }
            ITopologicalOperator2 @operator = null;
            if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                return null;
            }
            @operator = (ITopologicalOperator2) shapeCopy;
            @operator.IsKnownSimple_2 = false;
            @operator.Simplify();
            IGeometry boundary = @operator.Boundary;
            ITopologicalOperator2 operator2 = (ITopologicalOperator2) boundary;
            operator2.IsKnownSimple_2 = false;
            operator2.Simplify();
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = pFeature.ShapeCopy;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor o = pFClass.Search(filter, false);
            IFeature feature = o.NextFeature();
            while (feature != null && !GapsList.Contains(feature.OID.ToString() + pFeature.OID.ToString()) && !GapsList.Contains(pFeature.OID.ToString() + feature.OID.ToString()))
            {
                GapsList.Add(feature.OID.ToString() + pFeature.OID.ToString());
                if (feature.OID != pFeature.OID)
                {
                    Console.WriteLine(pFeature.OID + "---" + feature.OID);
                    //flag = true;
                    IGeometry geometry3 = operator2.Intersect(feature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                    IGeometry itemaa = operator2.Difference(geometry3);
                    list.Add(itemaa);
                    //Console.WriteLine("相交：" + pFeature.OID +"-"+ feature.OID);
                }
                feature = o.NextFeature();
            }
            Marshal.ReleaseComObject(o);
            IList<IGeometry> list2 = new List<IGeometry>();
            if (list.Count == 0)
            {
                //list2.Add(boundary);
                return list2;
            }
            IGeometry other = null;
            for (int i = 0; i < list.Count; i++)
            {
                IGeometry geometry5 = list[i];
                if (other == null)
                {
                    other = geometry5;
                }
                else
                {
                    ITopologicalOperator2 operator3 = (ITopologicalOperator2) other;
                    operator3.IsKnownSimple_2 = false;
                    operator3.Simplify();
                    other = operator3.Union(geometry5);
                }
            }
            IGeometry item = null;
            item = operator2.Difference(other);
            if (item.IsEmpty)
            {
                //Console.WriteLine("完全包含："+pFeature.OID);
            }
            else 
            {
                IPolyline IntersectsPolyline = other as IPolyline;
                if (!IntersectsPolyline.IsClosed) 
                {
                    list2.Add(item);
                }
            }
            if (list2.Count > 0)
            {
                return list2[0] as IGeometry;
            }
            else 
            {
                return null;
            }
        }


        public List<IGeometry> CheckFeatureGap(IFeature pFeature)
        {
            List<IGeometry> list = new List<IGeometry>();
            IPolygon4 pMergerPolygon = pFeature.Shape as IPolygon4;
            IGeometryBag pOutGeometryBag = pMergerPolygon.ExteriorRingBag;  //获取外部环
            IGeometryCollection pOutGmtyCollection = pOutGeometryBag as IGeometryCollection;

            IGeometry other = null;
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
                    ISegmentCollection SegCol =  pInteriorGeometryCollection.get_Geometry(j) as ISegmentCollection;

                    IPolygon PPolygon = new PolygonClass();
                    ISegmentCollection newSegCol = PPolygon as ISegmentCollection;
                    newSegCol.AddSegmentCollection(SegCol);
                    //pInteriorGeometry即为多边形的内部环
                    list.Add(PPolygon as IGeometry);
                }
            }
            if (list.Count > 0)
            {
                return list;
            }
            else 
            {
                return null;
            }
        }

        

        public object CheckFeatureGap(IFeature pFeature, IFeatureClass pFClass)
        {
            IGeometry shapeCopy = pFeature.ShapeCopy;
            if (shapeCopy.IsEmpty)
            {
                return null;
            }
            ITopologicalOperator2 @operator = null;
            if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                return null;
            }
            @operator = (ITopologicalOperator2)shapeCopy;
            @operator.IsKnownSimple_2 = false;
            @operator.Simplify();
            IGeometry boundary = @operator.Boundary;
            IPolyline polyline = boundary as IPolyline;
            ITopologicalOperator2 operator2 = (ITopologicalOperator2)boundary;

            operator2.IsKnownSimple_2 = false;
            operator2.Simplify();
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = polyline;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor o = pFClass.Search(filter, false);
            IFeature feature = o.NextFeature();

            IGeometry other = null;
            while (feature != null)
            {
                IGeometry geometry11 = feature.ShapeCopy;
                if (other == null)
                {
                    other = geometry11;
                }
                else
                {
                    ITopologicalOperator2 operator3 = (ITopologicalOperator2)other;
                    operator3.IsKnownSimple_2 = false;
                    operator3.Simplify();
                    other = operator3.Union(geometry11);
                }
                feature = o.NextFeature();
            }

            IPolygon4 pMergerPolygon = other as IPolygon4;
            //IGeometryBag pInteriotGeometryBag = pMergerPolygon.get_InteriorRingBag(pOutRing as IRing);

            IGeometry geometry12 = null;
            geometry12 = operator2.Difference(other);
            if (geometry12.IsEmpty)
            {
                return true;
            }


            IGeometryCollection polygon = new PolygonClass();
            IPolygon polyGonGeo = null;
            while (feature != null && !GapsList.Contains(feature.OID.ToString() + pFeature.OID.ToString()) && !GapsList.Contains(pFeature.OID.ToString() + feature.OID.ToString()))
            {
                GapsList.Add(feature.OID.ToString() + pFeature.OID.ToString());
                if (feature.OID != pFeature.OID)
                {
                    
                    IGeometry geometry3 = operator2.Intersect(feature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                    
                    IGeometryCollection geometryCollection = geometry3 as IGeometryCollection;
                    List<IPoint> listPoint = new List<IPoint>();
                    if (geometryCollection.GeometryCount > 1) 
                    {
                        ISegmentCollection segmentCollection = new RingClass();
                        for (int i = 0; i < geometryCollection.GeometryCount - 1; i++)
                        {
                            Console.WriteLine(feature.OID.ToString() + "--" + pFeature.OID.ToString());
                            IPath ItemPath = geometryCollection.get_Geometry(i) as IPath;
                            IPath NextPath = geometryCollection.get_Geometry(i+1) as IPath;
                            IPoint StarPoint = ItemPath.ToPoint;
                            IPoint EndPoint = NextPath.FromPoint;
                            IPolyline pline = BuildLine(polyline, StarPoint, EndPoint,false);
                            IPolyline pline1 = BuildLine(PolygonToPolyline(feature.ShapeCopy), EndPoint, StarPoint,true);
                            segmentCollection.AddSegmentCollection(pline as ISegmentCollection);
                            segmentCollection.AddSegmentCollection(pline1 as ISegmentCollection);
                            polygon.AddGeometry(segmentCollection as IGeometry, Type.Missing, Type.Missing);
                            polyGonGeo = polygon as IPolygon;
                            polyGonGeo.SimplifyPreserveFromTo();
                            if (polyGonGeo.IsEmpty) 
                            {
                                polyGonGeo = null;
                            }
                        }
                        
                    }
                }
                feature = o.NextFeature();
            }
            Marshal.ReleaseComObject(o);
            return polyGonGeo;
        }

        public object CheckFeatureGap1111(IFeature pFeature, IFeatureClass pFClass)
        {
            IGeometry shapeCopy = pFeature.ShapeCopy;
            if (shapeCopy.IsEmpty)
            {
                return null;
            }
            ITopologicalOperator2 @operator = null;
            if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                return null;
            }
            @operator = (ITopologicalOperator2)shapeCopy;
            @operator.IsKnownSimple_2 = false;
            @operator.Simplify();
            IGeometry boundary = @operator.Boundary;
            IPolyline polyline = boundary as IPolyline;
            ITopologicalOperator2 operator2 = (ITopologicalOperator2)boundary;

            operator2.IsKnownSimple_2 = false;
            operator2.Simplify();
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = polyline;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor o = pFClass.Search(filter, false);
            IFeature feature = o.NextFeature();

            IGeometryCollection polygon = new PolygonClass();
            IPolygon polyGonGeo = null;
            while (feature != null && !GapsList.Contains(feature.OID.ToString() + pFeature.OID.ToString()) && !GapsList.Contains(pFeature.OID.ToString() + feature.OID.ToString()))
            {
                GapsList.Add(feature.OID.ToString() + pFeature.OID.ToString());
                if (feature.OID != pFeature.OID)
                {

                    IGeometry geometry3 = operator2.Intersect(feature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);

                    IGeometryCollection geometryCollection = geometry3 as IGeometryCollection;
                    List<IPoint> listPoint = new List<IPoint>();
                    if (geometryCollection.GeometryCount > 1)
                    {
                        ISegmentCollection segmentCollection = new RingClass();
                        for (int i = 0; i < geometryCollection.GeometryCount - 1; i++)
                        {
                            Console.WriteLine(feature.OID.ToString() + "--" + pFeature.OID.ToString());
                            IPath ItemPath = geometryCollection.get_Geometry(i) as IPath;
                            IPath NextPath = geometryCollection.get_Geometry(i + 1) as IPath;
                            IPoint StarPoint = ItemPath.ToPoint;
                            IPoint EndPoint = NextPath.FromPoint;
                            IPolyline pline = BuildLine(polyline, StarPoint, EndPoint, false);
                            IPolyline pline1 = BuildLine(PolygonToPolyline(feature.ShapeCopy), EndPoint, StarPoint, true);
                            segmentCollection.AddSegmentCollection(pline as ISegmentCollection);
                            segmentCollection.AddSegmentCollection(pline1 as ISegmentCollection);
                            polygon.AddGeometry(segmentCollection as IGeometry, Type.Missing, Type.Missing);
                            polyGonGeo = polygon as IPolygon;
                            polyGonGeo.SimplifyPreserveFromTo();
                            if (polyGonGeo.IsEmpty)
                            {
                                polyGonGeo = null;
                            }
                        }

                    }
                }
                feature = o.NextFeature();
            }
            Marshal.ReleaseComObject(o);
            return polyGonGeo;
        }

        /// <summary>
        /// Geometry（Polygon）转Polyline
        /// </summary>
        /// <param name="pGeometry">传入的Polygon多边形</param>
        /// <returns>转换后的多段线</returns>
        public static IPolyline PolygonToPolyline(IGeometry pGeometry)
        {
            if (null == pGeometry)
            {
                return null;
            }
            IPolyline aTempPolyline = new PolylineClass();
            ISegmentCollection aTempGeometryCollection = aTempPolyline as ISegmentCollection;
            var pSegmentCollection = pGeometry as ISegmentCollection;
            for (int i = 0; i < pSegmentCollection.SegmentCount; i++)
            {
                aTempGeometryCollection.AddSegment(pSegmentCollection.Segment[i]);
            }
            return aTempGeometryCollection as IPolyline;
        }

        /// <summary>
        /// 创建区间线段
        /// </summary>
        /// <param name="pLine">输入的线图形</param>
        /// <param name="p1">插入的其中一个点</param>
        /// <param name="p2">插入的一种一个点</param>
        /// <returns>这两点间的线段</returns>
        /// 创建人 ： zw
        private IPolyline BuildLine(IPolyline pLine, IPoint p1, IPoint p2, Boolean Reverse)
        {
            if (Reverse)
            {
                pLine.ReverseOrientation();
            }
            bool isSplit;
            int splitIndex, segIndex;
            //插入第一点，segIndex记录插入点的相对线的节点位置
            pLine.SplitAtPoint(p1, true, false, out isSplit, out splitIndex, out segIndex);
            int fIndex = segIndex;
            //插入第二点
            pLine.SplitAtPoint(p2, true, false, out isSplit, out splitIndex, out segIndex);
            int sIndex = segIndex;
            //比较一下插入第一点和第二点的节点次序
            if (fIndex > sIndex)
            {
                int temp = fIndex;
                fIndex = sIndex;
                sIndex = temp;
            }
            IPointCollection pPointCol = new PolylineClass();
            object o = Type.Missing;
            //利用两点区间，获取线上区间所在的点，并将其转换为线
            IPointCollection LineCol = pLine as IPointCollection;
            for (int i = fIndex; i <= sIndex; i++)
            {
                pPointCol.AddPoint(LineCol.get_Point(i), ref o, ref o);
            }
            return pPointCol as IPolyline;
        }

        private bool CheckGap(string idname,IFeatureClass pFClass)
        {
            if (pFClass == null)
            {
                return false;
            }
            try
            {
                List<GapErrorEntity> pErrEntity = new List<GapErrorEntity>();
                IFeatureCursor o = pFClass.Search(null, false);
                for (IFeature feature = o.NextFeature(); feature != null; feature = o.NextFeature())
                {
                    object pErrGeo = this.CheckFeatureGap(feature, pFClass);
                    if (pErrGeo != null)
                    {
                        GapErrorEntity item = new GapErrorEntity(idname,feature.OID.ToString(), pErrGeo);
                        pErrEntity.Add(item);
                    }
                }
                Marshal.ReleaseComObject(o);
                new ErrorTable().AddGapErr(pErrEntity, idname);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool CheckGap1(string idname,IFeatureClass pFClass)
        {
            if (pFClass == null)
            {
                return false;
            }
            try
            {
                IGeometry other = SpatialAnalysis.DissolveGeo(pFClass, this.m_TempPath + @"\dissolve.shp");
                if ((this._geo != null) && (other != null))
                {
                    ITopologicalOperator2 @operator = (ITopologicalOperator2) this._geo;
                    @operator.Simplify();
                    IGeometry geometry2 = @operator.Intersect(other, esriGeometryDimension.esriGeometry2Dimension);
                    IGeometry pGeo = @operator.SymmetricDifference(geometry2);
                    if (pGeo.IsEmpty)
                    {
                        return true;
                    }
                    IList<IGeometry> geometrys = SpatialAnalysis.GetGeometrys(pGeo);
                    List<GapErrorEntity> pErrEntity = new List<GapErrorEntity>();
                    for (int i = 0; i < geometrys.Count; i++)
                    {
                        GapErrorEntity item = new GapErrorEntity(idname,"", geometrys[i]);
                        pErrEntity.Add(item);
                    }
                    new ErrorTable().AddGapErr(pErrEntity, idname);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual void WriteError(IFeatureClass pFClass,string idname)
        {
            ErrorTable table = new ErrorTable();
            if (this._errType == TopologyCheck.Error.ErrType.MultiOverlap)
            {
                table.AddMultiOverlapErr(pFClass);
            }
            else
            {
                table.AddTopoErr(pFClass, this._errType, idname);
            }
        }

        protected IGeometry BoundaryGeo
        {
            get
            {
                return this._geo;
            }
            set
            {
                this._geo = value;
            }
        }

        protected TopologyCheck.Error.ErrType ErrType
        {
            get
            {
                return this._errType;
            }
            set
            {
                this._errType = value;
            }
        }
    }
}

