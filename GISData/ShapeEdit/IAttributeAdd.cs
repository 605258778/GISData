namespace ShapeEdit
{
    using ESRI.ArcGIS.Geodatabase;
    using System.Windows.Forms;

    public interface IAttributeAdd
    {
        DialogResult AttributeAdd(ref IFeature editFeature);
    }
}

