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

    internal class BoundaryBeyondChecker : ITopologyChecker
    {
        private IGeometry _geo;
        private IFeatureLayer _layer;
        private int m_CheckType;

        public BoundaryBeyondChecker()
        {
        }

        public BoundaryBeyondChecker(IFeatureLayer pLayer, IGeometry pGeo)
        {
            this._layer = pLayer;
            this._geo = pGeo;
        }

        public BoundaryBeyondChecker(IFeatureLayer pLayer, IGeometry pGeo, int iCheckType)
        {
            this._layer = pLayer;
            this._geo = pGeo;
            this.m_CheckType = iCheckType;
        }

        protected virtual List<ErrorEntity> BoundaryBeyondCheck(IFeatureLayer pLayer, int iCheckType)
        {
            if (this._geo == null)
            {
                return null;
            }
            List<ErrorEntity> list = new List<ErrorEntity>();
            try
            {
                IFeatureCursor cursor;
                IFeatureClass featureClass = pLayer.FeatureClass;
                if (featureClass == null)
                {
                    return null;
                }
                IEnvelope extent = ((IGeoDataset) featureClass).Extent;
                IPointCollection points = new PolygonClass();
                object missing = Type.Missing;
                object after = Type.Missing;
                points.AddPoint(extent.LowerLeft, ref missing, ref after);
                points.AddPoint(extent.UpperLeft, ref missing, ref after);
                points.AddPoint(extent.UpperRight, ref missing, ref after);
                points.AddPoint(extent.LowerRight, ref missing, ref after);
                points.AddPoint(extent.LowerLeft, ref missing, ref after);
                IPolygon polygon = points as IPolygon;
                polygon.Close();
                polygon.SpatialReference = this._geo.SpatialReference;
                ITopologicalOperator2 @operator = polygon as ITopologicalOperator2;
                @operator.Simplify();
                IGeometry geometry = @operator.Difference(this._geo);
                IList<IFeature> list2 = new List<IFeature>();
                ISpatialFilter filter = new SpatialFilterClass();
                filter.Geometry = geometry;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps;
                filter.SubFields = featureClass.OIDFieldName + "," + featureClass.ShapeFieldName;
                if (iCheckType == 0)
                {
                    cursor = featureClass.Search(filter, true);
                }
                else
                {
                    cursor = pLayer.Search(filter, true);
                }
                IFeature item = null;
                while ((item = cursor.NextFeature()) != null)
                {
                    list2.Add(item);
                }
                filter = new SpatialFilterClass();
                filter.Geometry = geometry;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                filter.SubFields = featureClass.OIDFieldName + "," + featureClass.ShapeFieldName;
                if (iCheckType == 0)
                {
                    cursor = featureClass.Search(filter, true);
                }
                else
                {
                    cursor = pLayer.Search(filter, true);
                }
                while ((item = cursor.NextFeature()) != null)
                {
                    list2.Add(item);
                }
                Marshal.ReleaseComObject(cursor);
                IFeature feature2 = null;
                IRelationalOperator operator2 = this._geo as IRelationalOperator;
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < list2.Count; i++)
                {
                    feature2 = list2[i];
                    IGeometry shapeCopy = feature2.ShapeCopy;
                    if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        IPoint other = shapeCopy as IPoint;
                        if (operator2.Disjoint(other))
                        {
                            builder.Append(other.X.ToString());
                            builder.Append(",");
                            builder.Append(other.Y.ToString());
                            builder.Append(";");
                        }
                    }
                    else
                    {
                        IGeometryCollection geometrys = shapeCopy as IGeometryCollection;
                        int geometryCount = geometrys.GeometryCount;
                        for (int j = 0; j < geometryCount; j++)
                        {
                            int num3;
                            IGeometry geometry3 = geometrys.get_Geometry(j);
                            ICurve curve = geometry3 as ICurve;
                            if (curve.IsClosed)
                            {
                                num3 = 1;
                            }
                            else
                            {
                                num3 = 0;
                            }
                            IPointCollection points2 = geometry3 as IPointCollection;
                            for (int k = num3; k < points2.PointCount; k++)
                            {
                                IPoint point2 = points2.get_Point(k);
                                if (operator2.Disjoint(point2))
                                {
                                    builder.Append(point2.X.ToString());
                                    builder.Append(",");
                                    builder.Append(point2.Y.ToString());
                                    builder.Append(";");
                                }
                            }
                        }
                    }
                    if (builder.Length > 0)
                    {
                        //list.Add(new ErrorEntity(feature2.OID.ToString(), "超过边界", builder.ToString(), ErrType.BeyondBoundary));
                        builder.Remove(0, builder.Length);
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        public bool Check()
        {
            if (this._geo == null)
            {
                return true;
            }
            List<ErrorEntity> pErrEntity = this.BoundaryBeyondCheck(this._layer, this.m_CheckType);
            //new ErrorTable().AddErr(pErrEntity, ErrType.BeyondBoundary);
            return (pErrEntity.Count == 0);
        }

        public bool CheckFeature(IFeature pFeature, ref object pErrInfo)
        {
            if (this._geo == null)
            {
                return true;
            }
            IGeometry shapeCopy = pFeature.ShapeCopy;
            List<double[]> list = (List<double[]>) pErrInfo;
            IRelationalOperator @operator = this._geo as IRelationalOperator;
            if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                IPoint other = shapeCopy as IPoint;
                if (@operator.Disjoint(other))
                {
                    list.Add(new double[] { other.X, other.Y });
                }
            }
            else
            {
                IGeometryCollection geometrys = shapeCopy as IGeometryCollection;
                int geometryCount = geometrys.GeometryCount;
                for (int i = 0; i < geometryCount; i++)
                {
                    int num2;
                    IGeometry geometry2 = geometrys.get_Geometry(i);
                    ICurve curve = geometry2 as ICurve;
                    if (curve.IsClosed)
                    {
                        num2 = 1;
                    }
                    else
                    {
                        num2 = 0;
                    }
                    IPointCollection points = geometry2 as IPointCollection;
                    for (int j = num2; j < points.PointCount; j++)
                    {
                        IPoint point2 = points.get_Point(j);
                        if (@operator.Disjoint(point2))
                        {
                            list.Add(new double[] { point2.X, point2.Y });
                        }
                    }
                }
            }
            return (list.Count == 0);
        }
    }
}

