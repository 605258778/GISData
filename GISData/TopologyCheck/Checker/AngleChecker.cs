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

    internal class AngleChecker : ITopologyChecker
    {
        private double _cosAngle;
        private IFeatureLayer _layer;
        private int m_CheckType;

        public AngleChecker()
        {
        }

        public AngleChecker(double pAngle)
        {
            this._cosAngle = pAngle;
        }

        public AngleChecker(double pAngle, IFeatureLayer pLayer)
        {
            this._cosAngle = pAngle;
            this._layer = pLayer;
        }

        public AngleChecker(double pAngle, IFeatureLayer pLayer, int iCheckType)
        {
            this._cosAngle = pAngle;
            this._layer = pLayer;
            this.m_CheckType = iCheckType;
        }

        protected virtual List<ErrorEntity> AngleCheck(IFeatureLayer pLayer, int iCheckType)
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
            List<ErrorEntity> list = new List<ErrorEntity>();
            IFeature feature = null;
            this._cosAngle = (3.1415926535897931 * this._cosAngle) / 180.0;
            double num = Math.Cos(this._cosAngle);
            while ((feature = cursor.NextFeature()) != null)
            {
                IGeometryCollection shapeCopy = feature.ShapeCopy as IGeometryCollection;
                int geometryCount = shapeCopy.GeometryCount;
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < geometryCount; i++)
                {
                    ICurve curve = shapeCopy.get_Geometry(i) as ICurve;
                    ISegmentCollection segments = curve as ISegmentCollection;
                    int segmentCount = segments.SegmentCount;
                    if (segmentCount > 1)
                    {
                        ISegment segment = null;
                        ISegment segment2 = null;
                        if (curve.IsClosed)
                        {
                            segment = segments.get_Segment(segmentCount - 1);
                            segment2 = segments.get_Segment(0);
                            double num5 = segment.FromPoint.X - segment2.ToPoint.X;
                            double num6 = segment.FromPoint.Y - segment2.ToPoint.Y;
                            double num7 = (num5 * num5) + (num6 * num6);
                            double num8 = segment.Length * segment.Length;
                            double num9 = segment2.Length * segment2.Length;
                            double num10 = ((num8 + num9) - num7) / ((2.0 * segment.Length) * segment2.Length);
                            if (num < num10)
                            {
                                builder.Append(";");
                                builder.Append(segment.ToPoint.X.ToString());
                                builder.Append(",");
                                builder.Append(segment.ToPoint.Y.ToString());
                            }
                        }
                        else
                        {
                            segment2 = segments.get_Segment(0);
                        }
                        for (int j = 1; j < segmentCount; j++)
                        {
                            segment = segment2;
                            segment2 = segments.get_Segment(j);
                            double num12 = segment.FromPoint.X - segment2.ToPoint.X;
                            double num13 = segment.FromPoint.Y - segment2.ToPoint.Y;
                            double num14 = (num12 * num12) + (num13 * num13);
                            double num15 = segment.Length * segment.Length;
                            double num16 = segment2.Length * segment2.Length;
                            double num17 = ((num15 + num16) - num14) / ((2.0 * segment.Length) * segment2.Length);
                            if (num < num17)
                            {
                                builder.Append(";");
                                builder.Append(segment.ToPoint.X.ToString());
                                builder.Append(",");
                                builder.Append(segment.ToPoint.Y.ToString());
                            }
                        }
                    }
                }
                if (builder.Length > 0)
                {
                    list.Add(new ErrorEntity(feature.OID.ToString(), "角度过小", builder.ToString().Substring(1), ErrType.Angle));
                }
            }
            return list;
        }

        public bool Check()
        {
            List<ErrorEntity> pErrEntity = this.AngleCheck(this._layer, this.m_CheckType);
            //new ErrorTable().AddErr(pErrEntity, ErrType.Angle);
            return (pErrEntity.Count == 0);
        }

        public bool CheckFeature(IFeature pFeature, ref object pErrFeatureInf)
        {
            IGeometryCollection shapeCopy = pFeature.ShapeCopy as IGeometryCollection;
            int geometryCount = shapeCopy.GeometryCount;
            List<double[]> list = (List<double[]>) pErrFeatureInf;
            this._cosAngle = (3.1415926535897931 * this._cosAngle) / 180.0;
            double num2 = Math.Cos(this._cosAngle);
            for (int i = 0; i < geometryCount; i++)
            {
                ICurve curve = shapeCopy.get_Geometry(i) as ICurve;
                ISegmentCollection segments = curve as ISegmentCollection;
                int segmentCount = segments.SegmentCount;
                ISegment segment = null;
                ISegment segment2 = null;
                if (curve.IsClosed)
                {
                    segment = segments.get_Segment(segmentCount - 1);
                    segment2 = segments.get_Segment(0);
                    double num5 = segment.FromPoint.X - segment2.ToPoint.X;
                    double num6 = segment.FromPoint.Y - segment2.ToPoint.Y;
                    double num7 = (num5 * num5) + (num6 * num6);
                    double num8 = segment.Length * segment.Length;
                    double num9 = segment2.Length * segment2.Length;
                    double num10 = ((num8 + num9) - num7) / ((2.0 * segment.Length) * segment2.Length);
                    if (num2 < num10)
                    {
                        double[] item = new double[] { segment.ToPoint.X, segment.ToPoint.Y };
                        list.Add(item);
                    }
                }
                else
                {
                    segment2 = segments.get_Segment(0);
                }
                for (int j = 1; j < segmentCount; j++)
                {
                    segment = segment2;
                    segment2 = segments.get_Segment(j);
                    double num12 = segment.FromPoint.X - segment2.ToPoint.X;
                    double num13 = segment.FromPoint.Y - segment2.ToPoint.Y;
                    double num14 = (num12 * num12) + (num13 * num13);
                    double num15 = segment.Length * segment.Length;
                    double num16 = segment2.Length * segment2.Length;
                    double num17 = ((num15 + num16) - num14) / ((2.0 * segment.Length) * segment2.Length);
                    if (num2 < num17)
                    {
                        double[] numArray2 = new double[] { segment.ToPoint.X, segment.ToPoint.Y };
                        list.Add(numArray2);
                    }
                }
            }
            if (list.Count > 0)
            {
                return false;
            }
            return true;
        }

        private void GetIndexByPoint(IGeometry pGeo, IPoint pPoint, out int hitSegmentIndex, out int hitPartIndex)
        {
            double hitDistance = -1.0;
            hitSegmentIndex = -1;
            hitPartIndex = -1;
            bool bRightSide = false;
            double searchRadius = 2.0;
            (pGeo as IHitTest).HitTest(pPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartVertex, null, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);
        }

        public double CosAngle
        {
            set
            {
                this._cosAngle = value;
            }
        }
    }
}

