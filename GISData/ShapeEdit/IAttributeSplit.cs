namespace ShapeEdit
{
    using ESRI.ArcGIS.Geodatabase;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public interface IAttributeSplit
    {
        DialogResult AttributeSplit(IFeature srcFeature, ref List<IFeature> pFeatureList);
    }
}

