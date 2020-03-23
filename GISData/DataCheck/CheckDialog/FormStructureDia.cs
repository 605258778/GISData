using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using GISData.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormStructureDia : Form
    {
        private string stepNo;
        private CheckBox checkBox;
        public FormStructureDia()
        {
            InitializeComponent();
        }
        public void SelectAll() 
        {
            this.gridView1.SelectAll();
        }

        public void UnSelectAll()
        {
            
            int[] rows = this.gridView1.GetSelectedRows();
            for (int i = 0; i < rows.Count(); i++) 
            {
                this.gridView1.UnselectRow(rows[i]);
            }
        }

        public FormStructureDia(string stepNo,CheckBox cb)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
            bindData();
        }

        private void FormStructureDia_Load(object sender, EventArgs e)
        {
            
        }

        private void bindData() 
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME,STATE,ERROR from GISDATA_REGINFO where IS_CHECK='1' and STEP_NO = '" + stepNo + "'");
            this.gridControl1.DataSource = dt;
            CommonClass common = new CommonClass();
            //common.AddCheckBox(this.gridControl1);
        }

        //private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if ((bool)dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
        //    {
        //        //dataGridView1.Rows[e.RowIndex].Cells[0].Value = false;
        //    }
        //    else
        //    {
        //        //dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
        //    }
        //}

        private void FormStructureDia_Load_1(object sender, EventArgs e)
        {

        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int[] selectRows = this.gridView1.GetSelectedRows();
            if (selectRows.Length > 0)
            {
                this.checkBox.CheckState = CheckState.Checked;
            }
            else
            {
                this.checkBox.CheckState = CheckState.Unchecked;
            }
        }

        public void doCheckStructure() 
        {
            CommonClass common = new CommonClass();
            int[] selectRows = this.gridView1.GetSelectedRows();
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string tablename = row["REG_NAME"].ToString();
                IFeatureLayer _layer = common.GetLayerByName(tablename);
                IFields fields = _layer.FeatureClass.Fields;
                Dictionary<string, List<string>> dicCustom = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> dicSys = new Dictionary<string, List<string>>();

                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select FIELD_NAME,DATA_TYPE,MAXLEN from GISDATA_MATEDATA where REG_NAME  = '" + tablename + "'");
                DataRow[] dr = dt.Select(null);

                string errorString = "";

                for (int i = 0; i < dr.Length; i++)
                {
                    string FIELD_NAME = dr[i]["FIELD_NAME"].ToString();
                    string DATA_TYPE = dr[i]["DATA_TYPE"].ToString();
                    string MAXLEN = dr[i]["MAXLEN"].ToString();
                    List<string> list1 = new List<string>();
                    list1.Add(DATA_TYPE);
                    list1.Add(MAXLEN);
                    dicSys.Add(FIELD_NAME, list1);
                }

                for(int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    if (field.Name != _layer.FeatureClass.ShapeFieldName && field.Name != _layer.FeatureClass.OIDFieldName)
                    {
                        List<string> list1 = new List<string>();
                        list1.Add(field.Type.ToString());
                        list1.Add(field.Length.ToString());
                        dicCustom.Add(field.Name.ToString(), list1);
                        if (dicSys.ContainsKey(field.Name))
                        {
                            if (dicSys[field.Name][0] != field.Type.ToString()) 
                            {
                                errorString += "字段类型错误：" + field.Name + "(" + dicSys[field.Name][0] + ")；";
                            }
                            else if (dicSys[field.Name][1] != field.Length.ToString())
                            {
                                errorString += "字段长度错误：" + field.Name + "(" + dicSys[field.Name][0] + ")；";
                            }
                        }
                        else 
                        {
                            errorString += "多余字段：" + field.Name + "；";
                        }
                    }
                }

                foreach (KeyValuePair<string,List<string>> itemList in dicSys) 
                {
                    if (!dicCustom.ContainsKey(itemList.Key))
                    {
                        errorString += "缺少字段：" + itemList.Key + "；";
                    }
                }

            }
        }
    }
}
