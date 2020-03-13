namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
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
        }

        public virtual bool Check()
        {
            IFeatureClass pFClass = null;
            if ((this._errType == TopologyCheck.Error.ErrType.OverLap) || (this._errType == TopologyCheck.Error.ErrType.MultiOverlap))
            {
                pFClass = SpatialAnalysis.Intersect(this.m_FCList, this.m_TempPath + @"\intersect.shp");
                if (pFClass == null)
                {
                    return true;
                }
                this.WriteError(pFClass);
            }
            else if (this._errType == TopologyCheck.Error.ErrType.Gap)
            {
                IFeatureClass class3 = this.m_FCList[0];
                if (this._geo == null)
                {
                    this.CheckGap(class3);
                }
                else
                {
                    this.CheckGap1(class3);
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

        public List<ErrorEntity> CheckOverLap(IFeatureLayer pLayer, int iCheckType)
        {
            IFeatureCursor cursor = pLayer.Search(null, true);
            IFieldEdit edit = pLayer.FeatureClass.Fields.get_Field(pLayer.FeatureClass.Fields.FindField(pLayer.FeatureClass.ShapeFieldName)) as IFieldEdit;
            ISpatialReference spatialReference = edit.GeometryDef.SpatialReference;
            List<ErrorEntity> listErrorEntity = new List<ErrorEntity>();
            IFeature pFeature = null;
            object missing = Type.Missing;
            while ((pFeature = cursor.NextFeature()) != null)
            {
                IGeometry shapeCopy = pFeature.ShapeCopy;
                ITopologicalOperator2 @operator = null;
                if (shapeCopy.GeometryType != esriGeometryType.esriGeometryPoint)
                {
                    @operator = (ITopologicalOperator2) shapeCopy;
                    @operator.IsKnownSimple_2 = false;
                    @operator.Simplify();
                }
                IFeatureClass featureClass = pLayer.FeatureClass;
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
                    if (list.Count > 0)
                    {
                        listErrorEntity.Add(new ErrorEntity(pFeature.OID.ToString(), "自重叠", "", ErrType.OverLap, list[0]));
                    }
                }
            }
            return listErrorEntity;
        }

        public object CheckFeatureGap(IFeature pFeature, IFeatureClass pFClass)
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
            IPolyline polyline = boundary as IPolyline;
            ITopologicalOperator2 operator2 = (ITopologicalOperator2) boundary;
            //IRelationalOperator operatorWithin = (IRelationalOperator)boundary;
            operator2.IsKnownSimple_2 = false;
            operator2.Simplify();
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = polyline;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor o = pFClass.Search(filter, false);
            IFeature feature = o.NextFeature();
            //IGeometry IntersectsGeometry = null;
            //Boolean flag = false;
            while (feature != null)
            {
                if (feature.OID != pFeature.OID)
                {
                    //flag = true;
                    IGeometry geometry3 = operator2.Intersect(feature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                    list.Add(geometry3);
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
            return list2[0] as IGeometry;
        }

        private bool CheckGap(IFeatureClass pFClass)
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
                        GapErrorEntity item = new GapErrorEntity(feature.OID.ToString(), pErrGeo);
                        pErrEntity.Add(item);
                    }
                }
                Marshal.ReleaseComObject(o);
                new ErrorTable().AddGapErr(pErrEntity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool CheckGap1(IFeatureClass pFClass)
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
                        GapErrorEntity item = new GapErrorEntity("0", geometrys[i]);
                        pErrEntity.Add(item);
                    }
                    new ErrorTable().AddGapErr(pErrEntity);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual void WriteError(IFeatureClass pFClass)
        {
            ErrorTable table = new ErrorTable();
            if (this._errType == TopologyCheck.Error.ErrType.MultiOverlap)
            {
                table.AddMultiOverlapErr(pFClass);
            }
            else
            {
                table.AddTopoErr(pFClass, this._errType);
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

