namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Carto;
    using System;
    using System.Collections.Generic;
    using TopologyCheck.Error;
    using ESRI.ArcGIS.Geodatabase;

    internal class OverLapChecker : TopoClassChecker
    {
        public OverLapChecker(IList<IFeatureClass> pList) : base(pList)
        {
            base.ErrType = ErrType.MultiOverlap;
        }

        public OverLapChecker(IFeatureLayer pLayer, int iCheckType) : base(pLayer, iCheckType)
        {
            base.ErrType = ErrType.OverLap;
        }
    }
}

