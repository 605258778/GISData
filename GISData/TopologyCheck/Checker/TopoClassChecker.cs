namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.Geoprocessing;
    using ESRI.ArcGIS.Geoprocessor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;
    //using TaskManage;
    //using TopologyCheck.Base;
    using TopologyCheck.Error;
    //using Utilities;

    internal class TopoClassChecker : ITopologyChecker
    {
        private TopologyCheck.Error.ErrType _errType;
        private IGeometry _geo;

        //public List<ErrorEntity> CheckOverLap(string idname,IFeatureLayer pLayer)
        //{
        //    ArrayList arraylist = new ArrayList();
            
        //    IQueryFilter filter = new QueryFilterClass();
        //    filter.SubFields = pLayer.FeatureClass.OIDFieldName + "," + pLayer.FeatureClass.ShapeFieldName;
        //    IFeatureCursor cursor = pLayer.FeatureClass.Search(filter, true);
        //    List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
        //    IFeature pFeature = null;
        //    IFeature feature;
        //    while ((pFeature = cursor.NextFeature()) != null)
        //    {
        //        ITopologicalOperator2 @operator = (ITopologicalOperator2)pFeature.ShapeCopy;
        //        @operator.IsKnownSimple_2 = false;
        //        @operator.Simplify();
        //        IIdentify pIdentify = pLayer as IIdentify;
        //        IArray pIDs = pIdentify.Identify((IGeometry)pFeature.ShapeCopy);
        //        if (pIDs != null && pIDs.Count > 1)
        //        {
        //            //取第一个实体
        //            for (int i = 0; i < pIDs.Count; i++)
        //            {
        //                feature = (pIDs.get_Element(i) as IRowIdentifyObject).Row as IFeature;
        //                if (feature != null && pFeature.OID != feature.OID && (!arraylist.Contains(pFeature.OID + "" + feature.OID) && !arraylist.Contains(feature.OID + "" + pFeature.OID)))
        //                {
        //                    arraylist.Add(pFeature.OID + "" + feature.OID);
        //                    IGeometry item = feature.ShapeCopy;
        //                    IGeometry geometry3 = @operator.Intersect(item, esriGeometryDimension.esriGeometry2Dimension);
        //                    if (!geometry3.IsEmpty)
        //                    {
        //                        Console.WriteLine(pFeature.OID + "-" + feature.OID);
        //                        listErrorEntity.Add(new ErrorEntity(idname, pFeature.OID.ToString(), "自重叠", feature.OID.ToString(), ErrType.OverLap, geometry3));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return listErrorEntity;
        //}

        public List<ErrorEntity> CheckOverLap(string idname, IFeatureLayer pLayer)
        {
            List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();

            IFeatureClass IN_FeatureClass = pLayer.FeatureClass;
            
            IList<IGeometry> list = new List<IGeometry>();
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.GeometryField = IN_FeatureClass.ShapeFieldName;
            spatialFilter.SubFields = IN_FeatureClass.OIDFieldName + "," + IN_FeatureClass.ShapeFieldName;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps;

           IFeatureCursor pFeatureCursor = IN_FeatureClass.Search(null, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {//记录集循环
                spatialFilter.Geometry = pFeature.ShapeCopy;
                IFeatureCursor featureCursor = IN_FeatureClass.Search(spatialFilter, true);
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
                        listErrorEntity.Add(new ErrorEntity(idname, pFeature.OID.ToString(), "自重叠", "", ErrType.OverLap, pIntersectGeo));
                    }
                }
                Marshal.ReleaseComObject(featureCursor);
                pFeature = pFeatureCursor.NextFeature();//移动到下一条记录
            }
            Marshal.ReleaseComObject(pFeature);
            Marshal.ReleaseComObject(spatialFilter);
            
            return listErrorEntity;
        }
        /// <summary>
        /// 用gp工具查重叠
        /// </summary>
        /// <param name="idname"></param>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        public List<ErrorEntity> CheckOverLap11(string idname, IFeatureLayer pLayer)
        {
            List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
            //输出要素层设置
            

            ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
            intersect.in_features = pLayer;//"D://开阳县_520121_YZL_2017.mdb//YZL_PY_FYYSXB";
            intersect.out_feature_class = "D://report//temp.shp";
            Geoprocessor geoProcessor = new Geoprocessor();
            geoProcessor.OverwriteOutput = true;
            try
            {
                geoProcessor.Execute(intersect, null);
            }
            catch (Exception ex)
            {
                // Print a generic exception message.
                Console.WriteLine(ex.Message);
            }


            return listErrorEntity;
        }
        
        /// <summary>
        /// 缝隙检查。融合后去内环。
        /// </summary>
        /// <param name="pFeature"></param>
        /// <returns></returns>
        public List<Dictionary<string, IGeometry>> CheckFeatureGap(IFeatureClass pFClass, string inputtext)
        {
            List<Dictionary<string, IGeometry>> listGeo = new List<Dictionary<string, IGeometry>>();
            if (pFClass == null)
            { return null; }

            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            //获取空间参考
            IGeometry geometryBag = new GeometryBagClass();
            IGeoDataset geoDataset = pFClass as IGeoDataset;
            geometryBag.SpatialReference = geoDataset.SpatialReference;

            ////属性过滤
            //ISpatialFilter queryFilter = new SpatialFilterClass();
            //queryFilter.SubFields = "Shape";
            IFeatureCursor featureCursor = pFClass.Search(null,false);

            // 遍历游标
            IFeature currentFeature = featureCursor.NextFeature();
            if (currentFeature == null) 
            {
                return null;
            }
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            object missing = Type.Missing;
            while (currentFeature != null)
            {
                geometryCollection.AddGeometry(currentFeature.Shape, ref missing, ref missing);
                currentFeature = featureCursor.NextFeature();
            }

            // 合并要素
            ITopologicalOperator unionedPolygon = null;
            if (pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                unionedPolygon = new Multipoint() as ITopologicalOperator;
                unionedPolygon.ConstructUnion(geometryCollection as IEnumGeometry);
            }
            else if (pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                unionedPolygon = new Polyline() as ITopologicalOperator;
                unionedPolygon.ConstructUnion(geometryCollection as IEnumGeometry);
            }
            else
            {
                unionedPolygon = new Polygon() as ITopologicalOperator;
                unionedPolygon.ConstructUnion(geometryCollection as IEnumGeometry);
            }
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
                            Console.WriteLine(feature.OID);
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
                                    Dictionary<string, IGeometry> itemDic = new Dictionary<string, IGeometry>();
                                    itemDic.Add(feature.OID.ToString(), inRing);
                                    listGeo.Add(itemDic);
                                    flag = false;
                                }
                            }
                            feature = o.NextFeature();
                        }
                        Marshal.ReleaseComObject(o);
                    }
                }
            }
            if (listGeo.Count > 0)
            {
                return listGeo;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 自相交检查
        /// </summary>
        /// <param name="idname"></param>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        public List<ErrorEntity> AreaSelfIntersect(string idname, IFeatureLayer pLayer)
        {
            IFeatureCursor cursor;
            IQueryFilter filter = new QueryFilterClass();
            filter.SubFields = pLayer.FeatureClass.OIDFieldName + "," + pLayer.FeatureClass.ShapeFieldName;
            cursor = pLayer.FeatureClass.Search(filter, true);
            IFieldEdit edit = pLayer.FeatureClass.Fields.get_Field(pLayer.FeatureClass.Fields.FindField(pLayer.FeatureClass.ShapeFieldName)) as IFieldEdit;
            ISpatialReference spatialReference = edit.GeometryDef.SpatialReference;
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
                }
            }
            return list;
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

