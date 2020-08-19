using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using GISData.Common;
using System.Runtime.InteropServices;  

namespace GISData.MainMap
{
    public partial class FormAttribute : Form
    {
        AxMapControl _MapControl;
        IMapControl3 m_mapControl;
        public DataTable dt2;
        bool up = true;
        public string strAddField = "";
        int row_index = 0;
        int col_index = 0; 
        RowAndCol[] pRowAndCol = new RowAndCol[10000];
        int count = 0;
        //TOCControl中图层菜单

        public FormAttribute(AxMapControl pMapControl, IMapControl3 pMapCtrl)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _MapControl = pMapControl;
            m_mapControl = pMapCtrl;
        }

        private void addfield_Click(object sender, EventArgs e)
        {
            //ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            //FormAddField formaddfield = new FormAddField(pFLayer, dataGridViewAttr);
            //formaddfield.ShowDialog();
        }

        private void FormAttribute_Load(object sender, EventArgs e)
        {
            TableShow();
        }

        public struct RowAndCol
        {
            //字段  
            private int row;
            private int column;
            private string _value;

            //行属性  
            public int Row
            {
                get
                {
                    return row;
                }
                set
                {
                    row = value;
                }
            }
            //列属性  
            public int Column
            {
                get
                {
                    return column;
                }
                set
                {
                    column = value;
                }
            }
            //值属性  
            public string Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }
        }

        //public void TableShow()
        //{
        //    ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
        //    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
        //    IFeatureClass pFeatureClass = pFLayer.FeatureClass;

        //    if (pFeatureClass == null) return;

        //    DataTable dt = new DataTable();
        //    DataColumn dc = null;
        //    for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
        //    {
        //        dc = new DataColumn(pFeatureClass.Fields.get_Field(i).AliasName);
        //        dt.Columns.Add(dc);
        //    }
        //    IFeatureCursor pFeatureCuror = pFeatureClass.Search(null, false);
        //    IFeature pFeature = pFeatureCuror.NextFeature();

        //    DataRow dr = null;
        //    while (pFeature != null)
        //    {
        //        dr = dt.NewRow();
        //        for (int j = 0; j < pFeatureClass.Fields.FieldCount; j++)
        //        {
        //            if (pFeatureClass.FindField(pFeatureClass.ShapeFieldName) == j)
        //            {
        //                dr[j] = pFeatureClass.ShapeType.ToString();
        //            }
        //            else
        //            {
        //                dr[j] = pFeature.get_Value(j).ToString();

        //            }
        //        }

        //        dt.Rows.Add(dr);
        //        pFeature = pFeatureCuror.NextFeature();
        //    }
        //    dataGridViewAttr.DataSource = dt;
        //    dt2 = dt;
        //    dataGridViewAttr.ReadOnly = true;
        //}

        /// <summary>
        /// 显示表格
        /// </summary>
        /// <param name="pFeatureClass">显示图层</param>
        /// <param name="grid">装载grid</param>
        /// <param name="ViewCaption">检查类型</param>
        /// <param name="NewItemRowText">检查表格</param>
        /// <param name="GroupPanelText">错误结果名</param>
        public void TableShow()
        {
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFeatureClass = pFLayer.FeatureClass;

            if (pFeatureClass == null) return;
            this.gridView1.Columns.Clear();

            DataTable dt = new DataTable();
            DataColumn dc = null;
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                dc = new DataColumn(pFeatureClass.Fields.get_Field(i).AliasName);
                dt.Columns.Add(dc);
            }
            IFeatureCursor pFeatureCuror = pFeatureClass.Search(null, true);
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
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            Marshal.ReleaseComObject(pFeatureCuror);
        }

        private void toolEditor_Click(object sender, EventArgs e)
        {
            //dataGridViewAttr.ReadOnly = false;
            //this.dataGridViewAttr.CurrentCell = this.dataGridViewAttr.Rows[this.dataGridViewAttr.Rows.Count - 2].Cells[0]; 
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            //dataGridViewAttr.ReadOnly = true;
            //ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            //IFeatureClass pFeatureClass = pFLayer.FeatureClass;
            //ITable pTable;
            ////pTable = pFeatureClass.CreateFeature().Table;//很重要的一种获取shp表格的一种方式           
            //pTable = pFLayer as ITable;
            ////将改变的记录值传给shp中的表  
            //int i = 0;
            //while (pRowAndCol[i].Column != 0 || pRowAndCol[i].Row != 0)
            //{
            //    IRow pRow;
            //    pRow = pTable.GetRow(pRowAndCol[i].Row);
            //    pRow.set_Value(pRowAndCol[i].Column, pRowAndCol[i].Value);
            //    pRow.Store();
            //    i++;
            //}
            //for (int j = 0; j < i; j++)
            //{
            //    pRowAndCol[j].Row = 0;
            //    pRowAndCol[j].Column = 0;
            //    pRowAndCol[j].Value = null;
            //}
            //MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK);  
        }

        private void toolDelSelect_Click(object sender, EventArgs e)
        {
            if (((MessageBox.Show("确定要删除吗", "警告", MessageBoxButtons.YesNo)) == DialogResult.Yes))
            {
                ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ITable pTable = pFLayer as ITable;
                int val = this.gridView1.GetFocusedDataSourceRowIndex();
                DataRowView row = (DataRowView)this.gridView1.GetRow(val);
                int oid = int.Parse(row[pFLayer.FeatureClass.OIDFieldName].ToString());
                IRow pRow = pTable.GetRow(oid);
                pRow.Delete();
                TableShow();
                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK);
                _MapControl.ActiveView.Refresh();
            }
        }

        private void toolExpXLS_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Title = "另存为";
                saveFileDialog1.FileName = "newExcelName.xls"; //设置默认另存为的名字，可选
                saveFileDialog1.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
                saveFileDialog1.AddExtension = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.gridView1.ExportToXlsx(saveFileDialog1.FileName);
                }
            }
        }

        private void dataGridViewAttr_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ////记录值一旦改变触发此事件  
            ////在dataGridView中获取改变记录的行数，列数和记录值  
            //pRowAndCol[count].Row = dataGridViewAttr.CurrentCell.RowIndex;
            //pRowAndCol[count].Column = dataGridViewAttr.CurrentCell.ColumnIndex;
            //pRowAndCol[count].Value = dataGridViewAttr.Rows[dataGridViewAttr.CurrentCell.RowIndex].Cells[dataGridViewAttr.CurrentCell.ColumnIndex].Value.ToString();
            //count++; 
        }


        private void dataGridViewAttr_MouseMove(object sender, MouseEventArgs e)
        {
            //row_index = this.dataGridViewAttr.HitTest(e.X, e.Y).RowIndex; //行
            //col_index = this.dataGridViewAttr.HitTest(e.X, e.Y).ColumnIndex; //列
        }

        private void dataGridViewAttr_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IQueryFilter pQuery = new QueryFilterClass();

            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;


            int val = this.gridView1.GetFocusedDataSourceRowIndex();
            DataRowView row = (DataRowView)this.gridView1.GetRow(val);
            int oid = int.Parse(row[pFLayer.FeatureClass.OIDFieldName].ToString());
            pQuery.WhereClause = pFLayer.FeatureClass.OIDFieldName + "=" + oid;

            IFeatureSelection pFeatSelection;
            pFeatSelection = pFLayer as IFeatureSelection;
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            _MapControl.ActiveView.Refresh();  
        }

        private void dataGridViewAttr_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IQueryFilter pQuery = new QueryFilterClass();

            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;


            int val = this.gridView1.GetFocusedDataSourceRowIndex();
            DataRowView row = (DataRowView)this.gridView1.GetRow(val);
            int oid = int.Parse(row[pFLayer.FeatureClass.OIDFieldName].ToString());
            pQuery.WhereClause = pFLayer.FeatureClass.OIDFieldName + "=" + oid;

            IFeatureSelection pFeatSelection;
            pFeatSelection = pFLayer as IFeatureSelection;
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            _MapControl.ActiveView.Refresh(); 
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            IQueryFilter pQuery = new QueryFilterClass();

            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;


            int val = this.gridView1.GetFocusedDataSourceRowIndex();
            DataRowView row = (DataRowView)this.gridView1.GetRow(val);
            int oid = int.Parse(row[pFLayer.FeatureClass.OIDFieldName].ToString());
            pQuery.WhereClause = pFLayer.FeatureClass.OIDFieldName + "=" + oid;
            IFeature feature = pFLayer.FeatureClass.GetFeature(oid);
            IFeatureSelection pFeatSelection;
            pFeatSelection = pFLayer as IFeatureSelection;
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, true);
            _MapControl.Extent = feature.ShapeCopy.Envelope;
            _MapControl.ActiveView.Refresh();
        }
    }
}
