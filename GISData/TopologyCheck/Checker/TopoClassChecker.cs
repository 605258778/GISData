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
    //using TopologyCheck.Base;
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
            return listErrorEntity;
        }
        /// <summary>
        /// 缝隙检查。融合后去内环。
        /// </summary>
        /// <param name="pFeature"></param>
        /// <returns></returns>
        public List<Dictionary<string, IGeometry>> CheckFeatureGap(IFeature pFeature, IFeatureClass pFClass, string inputtext)
        {
            List<Dictionary<string, IGeometry>> listGeo = new List<Dictionary<string, IGeometry>>();
            IPolygon4 pMergerPolygon = pFeature.Shape as IPolygon4;
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
                    ISegmentCollection SegCol =  pInteriorGeometryCollection.get_Geometry(j) as ISegmentCollection;

                    IPolygon PPolygon = new PolygonClass();
                    ISegmentCollection newSegCol = PPolygon as ISegmentCollection;
                    newSegCol.AddSegmentCollection(SegCol);
                    //pInteriorGeometry即为多边形的内部环
                    IGeometry inRing = PPolygon as IGeometry;
                    inRing.SpatialReference = pFeature.ShapeCopy.SpatialReference;
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
        ///
        ///  面积计算
        ///
        /// 1:平方公里 保留6位小数 ；2:平方米 保留4位小数
        ///
        ///
        public static string CalculateAreaPrecision(string unitType, IGeometry geometry)
        {
            (geometry as ITopologicalOperator).Simplify();
            //如果面积为负数的话，则进行反转。因为geometry面积跟点的顺序有关，若是顺时针的话是面，为正；若是逆时针的话是洞，为负。
            if ((geometry as IArea).Area <= 0)
            {
                (geometry as IPolygon).ReverseOrientation();
            }
            IArea wktArea = geometry as IArea;
            string areaPrecision = "";
            double area = Math.Abs(wktArea.Area);
            switch (unitType)
            {
                case "1":
                    areaPrecision = (area / 1000000).ToString("0.000000");//平方公里时，保留六位小数
                    break;
                case "2":
                    areaPrecision = (area).ToString("0.0000");//平方米时，保留四位小数
                    break;
                default:
                    areaPrecision = (area).ToString("0.0000");
                    break;
            }
            return areaPrecision;
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

