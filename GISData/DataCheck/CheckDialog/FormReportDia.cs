using ADODB;
using Spread = DevExpress.Spreadsheet;
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
using DevExpress.Office;
using DevExpress.Utils;
//using DevExpress.XtraSpreadsheet.Utils;
using Model = DevExpress.XtraSpreadsheet.Model;
using Excel = Microsoft.Office.Interop.Excel;
using DevExpress.XtraRichEdit;
using System.Collections;
using DevExpress.XtraSpreadsheet.Commands;
using DevExpress.XtraSpreadsheet.Services;
using DevExpress.XtraSpreadsheet.Services.Implementation;
using DevExpress.XtraSpreadsheet.Internal;
//using DevExpress.XtraSpreadsheet.API.Native.Implementation;
//using DevExpress.Xpf.Spreadsheet;

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
            DataTable dtDs = new DataTable();
            foreach(int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                String reportType = row["REPORTTYPE"].ToString();
                String sheetname = row["SHEETNAME"].ToString();
                String sqlstr = row["SQLSTR"].ToString();
                String rowname = row["ROWNAME"].ToString().Trim();
                String columnsname = row["COLUMNSNAME"].ToString().Trim();
                String valuestring = row["VALUESTRING"].ToString().Trim();
                string[] dataSourceArr = row["DATASOURCE"].ToString().Split(',');

                SpreadsheetControl sheet = new SpreadsheetControl();
                Spread.IWorkbook book = sheet.Document;
                book.LoadDocument(Application.StartupPath + "\\Report\\" + row["REPORTMOULD"].ToString(), Spread.DocumentFormat.OpenXml);
                if (reportType == "任务完成统计表")
                {
                    string gldw = common.GetConfigValue("GLDW") == "" ? "520121" : common.GetConfigValue("GLDW");
                    DataTable dtTaskDb = db.GetDataBySql("select YZLGLDW,YZLFS,ZCSBND,XMMC,RWMJ from GISDATA_TASK where YZLGLDW = '" + gldw + "'");
                    dtTaskDb.TableName = "GISDATA_TASK";
                    DataTable dtTask = common.TranslateDataTable(dtTaskDb);
                    DataRow[] drTask = dtTask.Select(null);
                    dtTask.Columns.Add("SBMJ");
                    dtDs.Columns.Add("YZLGLDW");
                    dtDs.Columns.Add("YZLFS");
                    dtDs.Columns.Add("ZCSBND");
                    dtDs.Columns.Add("XMMC");
                    dtDs.Columns.Add("RWMJ");
                    dtDs.Columns.Add("SBMJ");

                    DataTable dt = new DataTable();
                    for (int i = 0; i < dataSourceArr.Length; i++)
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
                                    where (t.Field<string>("YZLGLDW") == gldw && t.Field<string>("ZCSBND") == zcsbnd && t.Field<string>("YZLFS") == yzlfs && t.Field<string>("XMMC") == xmmc)
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
                        rowItem["SBMJ"] = sbmj == "" ? "0" : sbmj;
                        dtDs.ImportRow(rowItem);
                    }
                    book.MailMergeDataSource = dtDs;
                    Spread.IWorkbook resultBook = book.GenerateMailMergeDocuments()[0];
                    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (resultBook != null)
                    {
                        using (MemoryStream result = new MemoryStream())
                        {
                            resultBook.SaveDocument("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
                            result.Seek(0, SeekOrigin.Begin);
                        }
                    }
                }else if(reportType == "透视表")
                {
                    DataTable dt = new DataTable();
                    for (int i = 0; i < dataSourceArr.Length; i++)
                    {
                        DataTable itemDt = common.GetTableByName(dataSourceArr[i].Trim());
                        //DataTable itemDt = ToDataTable(table);
                        dt.Merge(itemDt);
                    }

                    ConvertPivotTable conver = new ConvertPivotTable();
                    DataTable dtPivot = conver.CreatePivotTable(dt, columnsname.Trim(), "SBMJ", "", rowname.Trim());

                    Spread.Worksheet Spreadsheet = book.Worksheets[sheetname];

                    Model.DocumentModel documentModel = new Model.DocumentModel();
                    FileInfo xlxsFile =new FileInfo(Application.StartupPath + "\\Report\\" + row["REPORTMOULD"].ToString());
                    System.IO.Stream stream = xlxsFile.OpenRead();
                    Spread.DocumentFormat format = documentModel.AutodetectDocumentFormat(xlxsFile.Name);
                    documentModel.LoadDocument(stream, format,string.Empty);

                    MailMergeOptions option = new MailMergeOptions(documentModel);

                    Model.CellRangeBase detail = option.DetailRange;
                    Model.CellRangeBase header = option.HeaderRange;

                    IEnumerable<Spread.Cell> dynamiccolArr = Spreadsheet.Search("=DYNAMICCOL(\"" + columnsname + "\")");
                    IEnumerable<Spread.Cell> dynamicfieldArr = Spreadsheet.Search("=DYNAMICFIELD(\"" + columnsname + "\")");
                    Spread.Cell dynamiccol = null;
                    foreach (Spread.Cell str in dynamiccolArr) 
                    {
                        dynamiccol = str;
                        break;
                    }
                    Spread.Cell dynamicFiled = null;

                    foreach (Spread.Cell str in dynamicfieldArr)
                    {
                        dynamicFiled = str;
                        break;
                    }
                    int ColumnIndexItem = 0;
                    for (int i = 0; i < dtPivot.Columns.Count; i++)
                    {
                        DataColumn itemColumn = dtPivot.Columns[i];
                        if (itemColumn.Namespace == columnsname)
                        {
                            Spread.Cell rangeHeader = Spreadsheet.Cells[dynamiccol.RowIndex,dynamiccol.ColumnIndex+ColumnIndexItem];
                            Spread.Cell rangeDetail = Spreadsheet.Cells[dynamicFiled.RowIndex, dynamiccol.ColumnIndex + ColumnIndexItem];
                            rangeHeader.CopyFrom(dynamiccol);
                            rangeDetail.CopyFrom(dynamicFiled);
                            rangeHeader.Value = itemColumn.Caption.ToString();
                            rangeDetail.Calculate();
                            rangeHeader.Calculate();
                            rangeDetail.Value = "=FIELD(\""+itemColumn.ColumnName.ToString()+"\")";
                            rangeDetail.Formula = "=FIELD(\"" + itemColumn.ColumnName.ToString() + "\")";
                            ColumnIndexItem++;
                        }
                    }

                    //标题range
                    if (Spreadsheet.DefinedNames.GetDefinedName("TITLESTRING") != null) 
                    {
                        Spread.Range titleRange = Spreadsheet.DefinedNames.GetDefinedName("TITLESTRING").Range;
                        Model.CellPosition TitleStarPosition = new Model.CellPosition(titleRange.LeftColumnIndex, titleRange.TopRowIndex);
                        Model.CellPosition TitleEndPosition = new Model.CellPosition(titleRange.RightColumnIndex + ColumnIndexItem - 1, titleRange.BottomRowIndex);
                        Model.CellRange newTitle = new Model.CellRange(header.Worksheet, TitleStarPosition, TitleEndPosition);
                        titleRange = Spreadsheet.Range[newTitle.ToString()];
                        Spreadsheet.MergeCells(titleRange);
                    }
                    
                    //单位Range
                    if (Spreadsheet.DefinedNames.GetDefinedName("UNITSTRING") != null) 
                    {
                        Spread.Range unitRange = Spreadsheet.DefinedNames.GetDefinedName("UNITSTRING").Range;
                        Model.CellPosition UnitStarPosition = new Model.CellPosition(unitRange.LeftColumnIndex, unitRange.TopRowIndex);
                        Model.CellPosition UnitEndPosition = new Model.CellPosition(unitRange.RightColumnIndex + ColumnIndexItem - 1, unitRange.BottomRowIndex);
                        Model.CellRange newUnit = new Model.CellRange(header.Worksheet, UnitStarPosition, UnitEndPosition);
                        unitRange = Spreadsheet.Range[newUnit.ToString()];
                        Spreadsheet.MergeCells(unitRange);
                        Spread.Style unitStyle = unitRange.Style;
                        unitStyle.Alignment.Horizontal = Spread.SpreadsheetHorizontalAlignment.Right;
                    }
                    
                    //Detail Range
                    Model.CellRange newDetail = new Model.CellRange(header.Worksheet, detail.TopLeft.Column, detail.TopLeft.Row, detail.TopRight.Column + ColumnIndexItem, detail.BottomRight.Row);
                    Spread.Range detailRange = Spreadsheet.Range[newDetail.ToString()];
                    Spreadsheet.DefinedNames.GetDefinedName("DETAILRANGE").Range = detailRange;
                    //Header Range
                    Model.CellRange newHeader = new Model.CellRange(header.Worksheet, header.TopLeft.Column, header.TopLeft.Row, header.TopRight.Column + ColumnIndexItem, header.BottomRight.Row);
                    Spread.Range headerRange = Spreadsheet.Range[newHeader.ToString()];
                    Spreadsheet.DefinedNames.GetDefinedName("HEADERRANGE").Range = headerRange;
                    
                    


                    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    book.MailMergeDataSource = dtPivot;
                    Spread.IWorkbook resultBook = book.GenerateMailMergeDocuments()[0];
                    if (resultBook != null)
                    {
                        using (MemoryStream result = new MemoryStream())
                        {
                            resultBook.SaveDocument("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
                            result.Seek(0, SeekOrigin.Begin);
                        }
                    }
                }else 
                {
                    DataTable dt = new DataTable();
                    for (int i = 0; i < dataSourceArr.Length; i++)
                    {
                        DataTable itemDt = common.GetTableByName(dataSourceArr[i].Trim());
                        //DataTable itemDt = ToDataTable(table);
                        dt.Merge(itemDt);
                    }
                    //dtDs = common.TranslateDataTable(dt);
                    book.MailMergeDataSource = dt;
                    Spread.IWorkbook resultBook = book.GenerateMailMergeDocuments()[0];
                    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (resultBook != null)
                    {
                        using (MemoryStream result = new MemoryStream())
                        {
                            resultBook.SaveDocument("D:\\report\\" + row["REPORTNAME"].ToString() + time + ".xlsx");
                            result.Seek(0, SeekOrigin.Begin);
                        }
                    }
                }
            }
        }


        private void GetFileInfo(string strFilePath)
        {
            StringBuilder sb = new StringBuilder();
            if (System.IO.File.Exists(strFilePath))
            {
                FileInfo fif = new FileInfo(strFilePath);
                sb.AppendLine(string.Format("文件创建时间：{0}", fif.CreationTime.ToString()));
                sb.AppendLine(string.Format("文件最后一次读取时间：{0}", fif.LastAccessTime.ToString()));
                sb.AppendLine(string.Format("文件最后一次修改时间：{0}", fif.LastWriteTime.ToString()));
                sb.AppendLine(string.Format("文件创建时间(UTC)：{0}", fif.CreationTimeUtc.ToString()));
                sb.AppendLine(string.Format("文件最后一次读取时间(UTC)：{0}", fif.LastAccessTimeUtc.ToString()));
                sb.AppendLine(string.Format("文件最后一次修改时间(UTC)：{0}", fif.LastWriteTimeUtc.ToString()));
                sb.AppendLine(string.Format("文件目录：{0}", fif.Directory));
                sb.AppendLine(string.Format("文件目录名称：{0}", fif.DirectoryName));
                sb.AppendLine(string.Format("文件扩展名：{0}", fif.Extension));
                sb.AppendLine(string.Format("文件完整名称：{0}", fif.FullName));
                sb.AppendLine(string.Format("文件名：{0}", fif.Name));
                sb.AppendLine(string.Format("文件字节长度：{0}", fif.Length));
                Console.WriteLine(sb.ToString());
            }
        }
        //Missing的命名空间using System.Reflection;
        public Recordset ConvertDataTableToRecordset(DataTable table)
        {
            Recordset rs = new RecordsetClass();
            foreach (DataColumn dc in table.Columns)
            {
                rs.Fields._Append(dc.ColumnName, GetDataType(dc.DataType), -1, FieldAttributeEnum.adFldIsNullable);
            }
            rs.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);
            foreach (DataRow dr in table.Rows)
            {
                rs.AddNew(Missing.Value, Missing.Value); object o;
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    rs.Fields[i].Value = dr[i];
                    o = rs.Fields[i].Value;
                }
            }
            return rs;
        }
        public static DataTypeEnum GetDataType(Type dataType)
        {
            switch (dataType.ToString())
            {
                case "System.Boolean": return DataTypeEnum.adBoolean;
                case "System.Byte": return DataTypeEnum.adUnsignedTinyInt;
                case "System.Char": return DataTypeEnum.adChar;
                case "System.DateTime": return DataTypeEnum.adDate;
                case "System.Decimal": return DataTypeEnum.adDecimal;
                case "System.Double": return DataTypeEnum.adDouble;
                case "System.Int16": return DataTypeEnum.adSmallInt;
                case "System.Int32": return DataTypeEnum.adInteger;
                case "System.Int64": return DataTypeEnum.adBigInt;
                case "System.SByte": return DataTypeEnum.adTinyInt;
                case "System.Single": return DataTypeEnum.adSingle;
                case "System.String": return DataTypeEnum.adVarChar;
                case "System.UInt16": return DataTypeEnum.adUnsignedSmallInt;
                case "System.UInt32": return DataTypeEnum.adUnsignedInt;
                case "System.UInt64": return DataTypeEnum.adUnsignedBigInt;
                default: throw new Exception("没有对应的数据类型");
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
