using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
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

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormReportDia : Form
    {
        private CheckBox checkBox;
        private string stepNo;
        public FormReportDia()
        {
            InitializeComponent();
        }

        public FormReportDia(string stepNo, CheckBox cb)
        {
            InitializeComponent();
            this.stepNo = stepNo;
            this.checkBox = cb;
        }
        private void bindDataSource()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_REPORT");
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
        }

        private void FormReportDia_Load(object sender, EventArgs e)
        {
            bindDataSource();
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

        public void DoReport() 
        {
            CommonClass common = new CommonClass();
            ConnectDB db = new ConnectDB();
            int[] selectRows = this.gridView1.GetSelectedRows();
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string gldw = common.GetConfigValue("GLDW");
                DataTable dtTask = db.GetDataBySql("select YZLGLDW,YZLFS,ZCSBND,XMMC,RWMJ from GISDATA_TASK where YZLGLDW = '" + gldw + "'");
                DataRow[] drTask = dtTask.Select(null);
                DataTable dtDs = new DataTable();
                dtTask.Columns.Add("SBMJ");
                dtDs.Columns.Add("YZLGLDW");
                dtDs.Columns.Add("YZLFS");
                dtDs.Columns.Add("ZCSBND");
                dtDs.Columns.Add("XMMC");
                dtDs.Columns.Add("RWMJ");
                dtDs.Columns.Add("SBMJ");
                
                ITable table = common.GetLayerByName(row["DATASOURCE"].ToString()).FeatureClass as ITable;
                DataTable dt = ToDataTable(table);

                for (int i = 0; i < drTask.Length; i++)
                {
                    DataRow rowItem = drTask[i];
                    string zcsbnd = rowItem["ZCSBND"].ToString();
                    string yzlfs = rowItem["YZLFS"].ToString();
                    string xmmc = rowItem["XMMC"].ToString();
                    string sbmj = "";

                    var query = from t in dt.AsEnumerable()
                                where (t.Field<string>("YZLGLDW") == gldw && t.Field<string>("ZCSBND") == zcsbnd && t.Field<string>("YZLFS") == yzlfs &&t.Field<string>("XMMC") == xmmc)
                                group t by new { t1 = t.Field<string>("YZLGLDW"), t2 = t.Field<string>("ZCSBND"), t3 = t.Field<string>("YZLFS"), t4 = t.Field<string>("XMMC") } into m
                                select new
                                {
                                    gldwItem = m.Key.t1,
                                    zcsbndItem = m.Key.t2,
                                    yzlfsItem = m.Key.t3,
                                    xmmcItem = m.Key.t4,
                                    sbmjItem = m.Sum(n => Convert.ToDouble(n["SBMJ"]))
                                };
                    if (query.ToList().Count > 0)
                    {
                        query.ToList().ForEach(q =>
                        {
                            sbmj = q.sbmjItem.ToString();
                        });
                    } 

                    //DataRow[] drItem = dt.Select("YZLGLDW = '" + gldw + "' and ZCSBND= '" + zcsbnd + "' AND YZLFS = '" + yzlfs + "' AND XMMC ='" + xmmc + "'");
                    //if (drItem.Length > 0) 
                    //{
                    //    sbmj = drItem[0]["SBMJ"].ToString();
                    //}
                    rowItem["SBMJ"] = sbmj;
                    dtDs.ImportRow(rowItem);
                }

                XtraReport report = new XtraReport();
                report.LoadLayout(Application.StartupPath +"\\Report\\"+ row["REPORTMOULD"].ToString());
                report.DataSource = dtDs;
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                report.ExportToXlsx("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
            }
        }
        private void BindingFields(DataTable ds, XRTableCellCollection cc)
        {
            for (int i = 0; i < ds.Columns.Count - 1; i++)
            {
                cc[i].DataBindings.Add("Text", ds, ds.Columns[i].Caption);
             }
        }

        /// <summary>  
        /// 将ITable转换为DataTable  
        /// </summary>  
        /// <param name="mTable"></param>  
        /// <returns></returns>  
        public static DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }

                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];
                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }

                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
