namespace ShapeEdit
{
    using ESRI.ArcGIS.Geodatabase;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public interface IAttributeCombine
    {
        DialogResult AttributeCombine(List<IFeature> pFeatureList, ref IFeature resultFeature);
    }
}

