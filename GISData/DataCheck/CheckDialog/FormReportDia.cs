using DevExpress.Spreadsheet;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraSpreadsheet;
using ESRI.ArcGIS.Geodatabase;
using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
                string gldw = common.GetConfigValue("GLDW") == "" ? "520121" : common.GetConfigValue("GLDW");
                DataTable dtTaskDb = db.GetDataBySql("select YZLGLDW,YZLFS,ZCSBND,XMMC,RWMJ from GISDATA_TASK where YZLGLDW = '" + gldw + "'");
                dtTaskDb.TableName = "GISDATA_TASK";
                DataTable dtTask = common.TranslateDataTable(dtTaskDb);
                DataRow[] drTask = dtTask.Select(null);
                DataTable dtDs = new DataTable();
                dtTask.Columns.Add("SBMJ");
                dtDs.Columns.Add("YZLGLDW");
                dtDs.Columns.Add("YZLFS");
                dtDs.Columns.Add("ZCSBND");
                dtDs.Columns.Add("XMMC");
                dtDs.Columns.Add("RWMJ");
                dtDs.Columns.Add("SBMJ");

                string[] dataSourceArr = row["DATASOURCE"].ToString().Split(',');
                DataTable dt = new DataTable();
                for(int i = 0 ;i < dataSourceArr.Length;i++)
                {
                    DataTable itemDt = common.GetTableByName(dataSourceArr[i].Trim());
                    //DataTable itemDt = ToDataTable(table);
                    dt.Merge(itemDt);
                }
                
                for (int i = 0; i < drTask.Length; i++)
                {
                    DataRow rowItem = drTask[i];
                    string zcsbnd = rowItem["ZCSBND"].ToString();
                    gldw = rowItem["YZLGLDW"].ToString();
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
                    rowItem["SBMJ"] = sbmj;
                    dtDs.ImportRow(rowItem);
                }
                SpreadsheetControl sheet = new SpreadsheetControl();
                //sheet.LoadDocument(Application.StartupPath + "\\Report\\" + row["REPORTMOULD"].ToString());
                IWorkbook book = sheet.Document;
                book.LoadDocument(Application.StartupPath + "\\Report\\" + row["REPORTMOULD"].ToString());
                book.MailMergeDataSource = dtDs;
                IWorkbook resultBook = book.GenerateMailMergeDocuments()[0];
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (resultBook != null)
                {
                    using (MemoryStream result = new MemoryStream())
                    {
                        resultBook.SaveDocument("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
                        result.Seek(0, SeekOrigin.Begin);
                    }
                }
                //book.SaveDocument("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
                //XtraReport report = new XtraReport();
                //report.LoadLayout(Application.StartupPath +"\\Report\\"+ row["REPORTMOULD"].ToString());
                //report.DataSource = dtDs;
                //string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                //report.ExportToXlsx("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
            }
        }

         /// <summary>
        /// DataTable转成List
        /// </summary>
        public static List<T> ToDataList<T>(DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
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
