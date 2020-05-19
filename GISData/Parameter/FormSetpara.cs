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
            ISpatialReferenceDialog2 pSRDialog = new SpatialReferenceDialogClass();
            ISpatialReference iSpatialReference = pSRDialog.DoModalCreate(true, false, false, 0);
            this.textBox1.Text = iSpatialReference.Name;
            CommonClass common = new CommonClass();
            common.SetConfigValue("SpatialReferenceName", iSpatialReference.Name);
        }

        private void FormSetpara_Load(object sender, EventArgs e)
        {
            CommonClass common = new CommonClass();
            string gldw = common.GetConfigValue("GLDW");
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select SpatialR from GISDATA_GLDW WHERE GLDW ='" + gldw + "'");
            string spatial = dt.Select(null)[0][0].ToString();
            this.textBox1.Text = spatial;
            common.SetConfigValue("SpatialReferenceName", spatial);
        }
    }
}
