namespace ShapeEdit
{
    using ESRI.ArcGIS.Geodatabase;
    using System;

    public interface IFeatureTool
    {
        IFeature Feature { set; }
    }
}

