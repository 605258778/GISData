namespace TopologyCheck.Checker
{
    using ESRI.ArcGIS.Geodatabase;
    using System;

    public interface ITopologyChecker
    {
        bool Check();
        bool CheckFeature(IFeature pFeature, ref object pErrFeatureInf);
    }
}

