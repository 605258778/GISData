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
            _MapControl = pMapControl;
            m_mapControl = pMapCtrl;
        }

        private void addfield_Click(object sender, EventArgs e)
        {
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            FormAddField formaddfield = new FormAddField(pFLayer, dataGridViewAttr);
            formaddfield.ShowDialog();
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

        public void TableShow()
        {
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFeatureClass = pFLayer.FeatureClass;

            if (pFeatureClass == null) return;

            DataTable dt = new DataTable();
            DataColumn dc = null;
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                dc = new DataColumn(pFeatureClass.Fields.get_Field(i).AliasName);
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
            dataGridViewAttr.DataSource = dt;
            dt2 = dt;
            dataGridViewAttr.ReadOnly = true;
        }

        private void toolEditor_Click(object sender, EventArgs e)
        {
            dataGridViewAttr.ReadOnly = false;
            this.dataGridViewAttr.CurrentCell = this.dataGridViewAttr.Rows[this.dataGridViewAttr.Rows.Count - 2].Cells[0]; 
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            dataGridViewAttr.ReadOnly = true;
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
            ITable pTable;
            //pTable = pFeatureClass.CreateFeature().Table;//很重要的一种获取shp表格的一种方式           
            pTable = pFLayer as ITable;
            //将改变的记录值传给shp中的表  
            int i = 0;
            while (pRowAndCol[i].Column != 0 || pRowAndCol[i].Row != 0)
            {
                IRow pRow;
                pRow = pTable.GetRow(pRowAndCol[i].Row);
                pRow.set_Value(pRowAndCol[i].Column, pRowAndCol[i].Value);
                pRow.Store();
                i++;
            }
            for (int j = 0; j < i; j++)
            {
                pRowAndCol[j].Row = 0;
                pRowAndCol[j].Column = 0;
                pRowAndCol[j].Value = null;
            }
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK);  
        }

        private void toolDelSelect_Click(object sender, EventArgs e)
        {
            if (((MessageBox.Show("确定要删除吗", "警告", MessageBoxButtons.YesNo)) == DialogResult.Yes))
            {
                ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ITable pTable = pFLayer as ITable;
                IRow pRow = pTable.GetRow(dataGridViewAttr.CurrentCell.RowIndex);
                pRow.Delete();
                TableShow();
                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK);
                _MapControl.ActiveView.Refresh();
            }
        }

        private void toolExpXLS_Click(object sender, EventArgs e)
        {
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
            IFields pFields = pFeatureClass.Fields;
            ExportExcelClass exportExcel = new ExportExcelClass();
            exportExcel.ExportExcel(dataGridViewAttr, pFields);
        }

        private void dataGridViewAttr_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //记录值一旦改变触发此事件  
            //在dataGridView中获取改变记录的行数，列数和记录值  
            pRowAndCol[count].Row = dataGridViewAttr.CurrentCell.RowIndex;
            pRowAndCol[count].Column = dataGridViewAttr.CurrentCell.ColumnIndex;
            pRowAndCol[count].Value = dataGridViewAttr.Rows[dataGridViewAttr.CurrentCell.RowIndex].Cells[dataGridViewAttr.CurrentCell.ColumnIndex].Value.ToString();
            count++; 
        }

        private void dataGridViewAttr_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dataGridViewAttr.Rows[e.RowIndex].Selected == false)
                    {
                        dataGridViewAttr.ClearSelection();
                        dataGridViewAttr.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dataGridViewAttr.SelectedRows.Count == 1)
                    {
                        dataGridViewAttr.CurrentCell = dataGridViewAttr.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    //contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                else if (e.RowIndex == -1)
                {
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void dataGridViewAttr_MouseMove(object sender, MouseEventArgs e)
        {
            row_index = this.dataGridViewAttr.HitTest(e.X, e.Y).RowIndex; //行
            col_index = this.dataGridViewAttr.HitTest(e.X, e.Y).ColumnIndex; //列
        }

        private void dataGridViewAttr_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IQueryFilter pQuery = new QueryFilterClass();
            int count = this.dataGridViewAttr.SelectedRows.Count;
            string val;
            string col;
            col = this.dataGridViewAttr.Columns[0].Name;
            //当只选中一行时  
            if (count == 1)
            {
                val = this.dataGridViewAttr.SelectedRows[0].Cells[col].Value.ToString();
                //设置高亮要素的查询条件  
                pQuery.WhereClause = col + "=" + val;
            }
            else//当选中多行时  
            {
                int i;
                string str;
                for (i = 0; i < count - 1; i++)
                {
                    val = this.dataGridViewAttr.SelectedRows[i].Cells[col].Value.ToString();
                    str = col + "=" + val + " OR ";
                    pQuery.WhereClause += str;
                }
                //添加最后一个要素的条件  
                val = this.dataGridViewAttr.SelectedRows[i].Cells[col].Value.ToString();
                str = col + "=" + val;
                pQuery.WhereClause += str;
            }
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureSelection pFeatSelection;
            pFeatSelection = pFLayer as IFeatureSelection;
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            _MapControl.ActiveView.Refresh();  
        }

        private void dataGridViewAttr_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IQueryFilter pQuery = new QueryFilterClass();
            int count = this.dataGridViewAttr.SelectedRows.Count;
            string val;
            string col;
            col = this.dataGridViewAttr.Columns[0].Name;
            //当只选中一行时  
            if (count == 1)
            {
                val = this.dataGridViewAttr.SelectedRows[0].Cells[col].Value.ToString();
                //设置高亮要素的查询条件  
                pQuery.WhereClause = col + "=" + val;
            }
            else//当选中多行时  
            {
                int i;
                string str;
                for (i = 0; i < count - 1; i++)
                {
                    val = this.dataGridViewAttr.SelectedRows[i].Cells[col].Value.ToString();
                    str = col + "=" + val + " OR ";
                    pQuery.WhereClause += str;
                }
                //添加最后一个要素的条件  
                val = this.dataGridViewAttr.SelectedRows[i].Cells[col].Value.ToString();
                str = col + "=" + val;
                pQuery.WhereClause += str;
            }

            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeature feature = (pLayer as IFeatureLayer).FeatureClass.GetFeature(int.Parse(val));
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureSelection pFeatSelection;
            pFeatSelection = pFLayer as IFeatureSelection;
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            _MapControl.ActiveView.Extent = feature.ShapeCopy.Envelope;
            _MapControl.ActiveView.Refresh();
        }
    }
}
