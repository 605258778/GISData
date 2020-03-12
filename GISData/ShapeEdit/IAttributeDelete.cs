namespace ShapeEdit
{
    using ESRI.ArcGIS.Geodatabase;
    using System.Windows.Forms;

    public interface IAttributeDelete
    {
        DialogResult AttributeDelete(IFeature editFeature);
    }
}

