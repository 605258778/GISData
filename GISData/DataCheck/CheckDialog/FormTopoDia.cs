using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopologyCheck.Error;

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormTopoDia : Form
    {
        private string stepNo;
        private CheckBox checkBox;
        private CheckBox checkBoxCheckMain;
        private GridControl gridControlError = null;
        private IHookHelper m_hookHelper = null;
        public DataTable topoErrorTable = null;
        private string scheme;
        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo, CheckBox cb, string scheme,CheckBox cbCheckMain, GridControl gridControlError)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.scheme = scheme;
            this.checkBox = cb;
            this.checkBoxCheckMain = cbCheckMain;
            this.gridControlError = gridControlError;
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

        private void FormTopoDia_Load(object sender, EventArgs e)
        {
            bindTreeView();
        }

        private void bindTreeView()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select NAME,STATE,ERROR,CHECKTYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT,ID from GISDATA_TBTOPO where SCHEME ='" + this.scheme + "' and STEP_NO = '" + this.stepNo + "'");
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            topoErrorTable = new DataTable();
            foreach (DataColumn column in dt.Columns)
            {
                topoErrorTable.Columns.Add(column.ColumnName);
            }
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int[] selectRows = this.gridView1.GetSelectedRows();
            if (selectRows.Length > 0)
            {
                this.checkBox.CheckState = CheckState.Checked;
            }
            else {
                this.checkBox.CheckState = CheckState.Unchecked;
            }
        }

        public void doCheckTopo(IHookHelper m_hookHelper)
        {
            this.m_hookHelper = m_hookHelper;
            CommonClass common = new CommonClass();
            int[] selectRows = this.gridView1.GetSelectedRows();
            TopoChecker topocheck = new TopoChecker();
            Dictionary<string, string> DicTopoData = new Dictionary<string, string>();
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string ID = row["ID"].ToString();
                string NAME = row["NAME"].ToString();
                string STATE = row["STATE"].ToString();
                string ERROR = row["ERROR"].ToString();
                string TYPE = row["CHECKTYPE"].ToString();
                string TABLENAME = row["TABLENAME"].ToString();
                string WHERESTRING = row["WHERESTRING"].ToString();
                string SUPTABLE = row["SUPTABLE"].ToString();
                string INPUTTEXT = row["INPUTTEXT"].ToString();
                if (INPUTTEXT != null && INPUTTEXT != "")
                {
                    DicTopoData.Add(ID, INPUTTEXT);
                }
                row["STATE"] = "检查中";
                if (SUPTABLE != null && SUPTABLE != "")
                {
                    topocheck.OtherRule(ID, TYPE, common.GetLayerByName(TABLENAME).FeatureClass, common.GetLayerByName(SUPTABLE).FeatureClass, INPUTTEXT, m_hookHelper);
                }
                else 
                {
                    topocheck.OtherRule(ID, TYPE, common.GetLayerByName(TABLENAME).FeatureClass, null, INPUTTEXT, m_hookHelper);
                }
                Dictionary<string, int> DicTopoError = topocheck.DicTopoError;
                row["ERROR"] = DicTopoError[row["ID"].ToString()];
                row["STATE"] = "检查完成";
                if (DicTopoError[ID].ToString() != "0")
                {
                    topoErrorTable.ImportRow(row);
                }
            }
        }

        public Boolean UpdateGrid() 
        {
            return true;
        }

        /// <summary>
        /// 双击要素，查看错误详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
                if (e.Button == MouseButtons.Left && e.Clicks == 2)//判断是否左键双击
                {
                    //判断光标是否在行范围内 
                    if (hInfo.InRow)
                    {
                        if (this.checkBoxCheckMain.Checked)
                        {
                            string errorType = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CHECKTYPE").ToString();
                            string idname = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ID").ToString();
                            string TABLENAME = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TABLENAME").ToString();
                           
                            
                            GridView gridView = this.gridControlError.DefaultView as GridView;
                            ErrorTable table = new ErrorTable();
                            DataTable table2 = table.GetTable(idname);
                            DataRow[] dr = table2.Select("1=1");
                            //ESRI.ArcGIS.Geometry.IGeometry igo = (ESRI.ArcGIS.Geometry.IGeometry) dr[0]["Geometry"];
                            gridView.Columns.Clear();
                            for (int i = 0; i < table2.Columns.Count; i++) 
                            {
                                GridColumn gridCol_FeatureID = new GridColumn();
                                gridCol_FeatureID.Caption = table2.Columns[i].Caption;
                                gridCol_FeatureID.FieldName = table2.Columns[i].ColumnName;
                                gridCol_FeatureID.Name = table2.Columns[i].ColumnName;
                                gridCol_FeatureID.Visible = true;
                                gridView.Columns.Add(gridCol_FeatureID);
                            }
                            this.gridControlError.DataSource = table2;
                            gridView.OptionsBehavior.Editable = false;
                            gridView.OptionsSelection.MultiSelect = true;
                            gridView.ViewCaption = "拓扑检查";
                            gridView.NewItemRowText = TABLENAME;
                            this.gridControlError.RefreshDataSource();
                        }
                        else {
                            Console.WriteLine("未选中");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MyException Throw:" + ex.Message);
            }
        }
    }
}
