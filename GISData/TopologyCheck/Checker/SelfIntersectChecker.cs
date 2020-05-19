namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using TopologyCheck.Error;

    internal class SelfIntersectChecker : ITopologyChecker
    {
        private IFeatureLayer _layer;
        private int m_CheckType;

        public SelfIntersectChecker(IFeatureLayer pLayer)
        {
            this._layer = pLayer;
        }

        public SelfIntersectChecker(IFeatureLayer pLayer, int iCheckType)
        {
            this._layer = pLayer;
            this.m_CheckType = iCheckType;
        }

        public List<ErrorEntity> AreaSelfIntersect(string idname,IFeatureLayer pLayer, int iCheckType)
        {
            IFeatureCursor cursor;
            IQueryFilter filter = new QueryFilterClass();
            filter.SubFields = pLayer.FeatureClass.OIDFieldName + "," + pLayer.FeatureClass.ShapeFieldName;
            if (iCheckType == 0)
            {
                cursor = pLayer.FeatureClass.Search(filter, true);
            }
            else
            {
                cursor = pLayer.Search(filter, true);
            }
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
                    list.Add(new ErrorEntity(idname,feature.OID.ToString(), "自相交", builder.ToString().Substring(1), ErrType.SelfIntersect, tempPoint));
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

        public bool IsSelfCross(IGeometry pGeometry)
        {
            ITopologicalOperator3 pTopologicalOperator2 = pGeometry as ITopologicalOperator3;
            pTopologicalOperator2.IsKnownSimple_2 = false;
            esriNonSimpleReasonEnum reason = esriNonSimpleReasonEnum.esriNonSimpleOK;
            if (!pTopologicalOperator2.get_IsSimpleEx(out reason))
            {
                if (reason == esriNonSimpleReasonEnum.esriNonSimpleSelfIntersections)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Check()
        {
            List<ErrorEntity> pErrEntity = this.AreaSelfIntersect("",this._layer, this.m_CheckType);
            //new ErrorTable().AddErr(pErrEntity, ErrType.SelfIntersect);
            return (pErrEntity.Count == 0);
        }

        public bool CheckFeature(IFeature pFeature, ref object pErrFeatureInf)
        {
            IFieldEdit edit = this._layer.FeatureClass.Fields.get_Field(this._layer.FeatureClass.Fields.FindField(this._layer.FeatureClass.ShapeFieldName)) as IFieldEdit;
            ISpatialReference spatialReference = edit.GeometryDef.SpatialReference;
            bool flag = true;
            List<double[]> list = (List<double[]>) pErrFeatureInf;
            IGeometryCollection shape = pFeature.Shape as IGeometryCollection;
            for (int i = 0; i < shape.GeometryCount; i++)
            {
                esriNonSimpleReasonEnum enum2 = esriNonSimpleReasonEnum.esriNonSimpleOK;
                IPointCollection newPoints = shape.get_Geometry(i) as IPointCollection;
                PolylineClass o = new PolylineClass();
                o.AddPointCollection(newPoints);
                o.SpatialReference = spatialReference;
                ITopologicalOperator3 ioperator = o as ITopologicalOperator3;
                ioperator.IsKnownSimple_2 = false;
                if (!ioperator.get_IsSimpleEx(out enum2))
                {
                    IRing ring = newPoints as IRing;
                    int num2 = 0;
                    if (ring.IsClosed)
                    {
                        num2 = 1;
                    }
                    flag = false;
                    List<string> list2 = new List<string>();
                    List<string> list3 = new List<string>();
                    for (int j = num2; j < newPoints.PointCount; j++)
                    {
                        IPoint point = newPoints.get_Point(j);
                        string item = point.X.ToString() + "," + point.Y.ToString();
                        if (list2.Contains(item))
                        {
                            if (!list3.Contains(item))
                            {
                                double[] numArray = new double[] { point.X, point.Y };
                                list.Add(numArray);
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
            return flag;
        }
    }
}

