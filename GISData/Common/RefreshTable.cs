using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto; 

namespace GISData.Common
{
    class RefreshTable
    {
        //刷新属性表  
        public void Refresh(DataGridView dataGridView, IFeatureLayer pFeatureLayer)
        {
            IFeatureLayer pFLayer = pFeatureLayer;
            IFeatureClass pFeatureClass = pFLayer.FeatureClass;

            if (pFeatureClass == null) return;

            DataTable dt = new DataTable();
            DataColumn dc = null;

            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {

                dc = new DataColumn(pFeatureClass.Fields.get_Field(i).Name);

                dt.Columns.Add(dc);

            }

            IFeatureCursor pFeatureCuror = pFeatureClass.Search(null, false);
            IFeature pFeature = pFeatureCuror.NextFeature();

            DataRow dr = null;
            while (pFeature != null)
            {
                dr = dt.NewRow();
                for (int j = 0; j < pFeatureClass.Fields.FieldCount; j++)
                {
                    if (pFeatureClass.FindField(pFeatureClass.ShapeFieldName) == j)
                    {

                        dr[j] = pFeatureClass.ShapeType.ToString();
                    }
                    else
                    {
                        dr[j] = pFeature.get_Value(j).ToString();

                    }
                }

                dt.Rows.Add(dr);
                pFeature = pFeatureCuror.NextFeature();
            }
            dataGridView.DataSource = dt;
        } 
    }
}
