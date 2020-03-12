namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geometry;
    using System;
    using TopologyCheck.Error;

    internal class GapsChecker : TopoClassChecker
    {
        public GapsChecker(IFeatureLayer pLayer, int iCheckType) : base(pLayer, iCheckType)
        {
            base.ErrType = ErrType.Gap;
        }

        public GapsChecker(IFeatureLayer pLayer, IGeometry pGeo, int iCheckType) : base(pLayer, iCheckType)
        {
            base.BoundaryGeo = pGeo;
            base.ErrType = ErrType.Gap;
        }
    }
}

