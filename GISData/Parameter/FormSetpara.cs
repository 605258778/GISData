using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.CatalogUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using GISData.Common;

namespace GISData.Parameter
{
    public partial class FormSetpara : Form
    {
        public FormSetpara()
        {
            InitializeComponent();
        }

        private void SpatialRSelect_Click(object sender, EventArgs e)
        {
            string GdbPath = Application.StartupPath + "\\GISData.gdb";
            ISpatialReferenceDialog2 pSRDialog = new SpatialReferenceDialogClass();
            ISpatialReference iSpatialReference = pSRDialog.DoModalCreate(true, false, false, 0);
            this.textBox1.Text = iSpatialReference.Name;
            CommonClass common = new CommonClass();
            common.SetConfigValue("SpatialReferenceName", iSpatialReference.Name);
            FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
            IWorkspace workspace = fac.OpenFromFile(GdbPath, 0);
            IFeatureWorkspace pFeatureWorkspace = workspace as IFeatureWorkspace;
            IFeatureWorkspaceManage featureWorkspaceMange = (IFeatureWorkspaceManage)pFeatureWorkspace;
            IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (datasetName.Name.Equals("dataset"))
                {
                    featureWorkspaceMange.DeleteByName(datasetName);//删除指定要素类
                    pFeatureWorkspace.CreateFeatureDataset("dataset", iSpatialReference);
                    break;
                }
            }
        }

        private void FormSetpara_Load(object sender, EventArgs e)
        {

        }
    }
}
