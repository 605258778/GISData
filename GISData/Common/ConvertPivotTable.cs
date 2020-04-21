using Spire.Xls;
using Spire.Xls.Core.Spreadsheet.PivotTables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GISData.Common
{
    class ConvertPivotTable
    {
        public Excel.Application m_xlApp = null; 

        public void setpivottable(string xlsxPath,string dataSource , string pivotSheetName)
        {
            //Create a workbook
            Workbook workbook = new Workbook();

            //Load an excel file including pivot table
            workbook.LoadFromFile(@xlsxPath);

            //Modify data of data source
            Worksheet data = workbook.Worksheets[dataSource];

            //Get the sheet in which the pivot table is located
            Worksheet sheet = workbook.Worksheets[pivotSheetName];

            XlsPivotTable pt = sheet.PivotTables[0] as XlsPivotTable;
            //Refresh and calculate
            pt.Cache.IsRefreshOnLoad = true;
            pt.CalculateData();
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            sheet.SaveToHtml("D:\\report\\" + time + ".html");
            Workbook wk = new Workbook();
            wk.LoadFromFile("D:\\report\\" + time + ".html");
            wk.SaveToFile("D:\\report\\" + time + ".xlsx");
            //Workbook workbookexport = new Workbook();
            //workbookexport.Version = workbook.Version;
            //Worksheet sheetexport = workbookexport.Worksheets.Add(pivotSheetName);
            //sheetexport.Name = pivotSheetName;
            
            //workbookexport.SaveToFile("D:\\report\\" + time + ".xlsx");
            //Save to file
            //workbook.SaveToFile("D:\\report\\" + time + ".xlsx", ExcelVersion.Version2013);
            //FileViewer("D:\\report\\" + time + ".xlsx");
        }
        private void FileViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }
        }

        /// <summary>
        /// 依据原始数据创建透视表
        /// 透视表:CreatePivotTable(scoreTable, "Course", "Score","No=学号;Name=姓名;Sex=性别", "No,Name,Sex");
        /// </summary>
        /// <paramname="dataSource">原始数据</param>
        /// <paramname="totalColumn">distinct统计字段</param>
        /// <paramname="valueColumn">数据字段</param>
        /// <paramname="nullValue">不存在时的填充值</param>
        /// <paramname="aliasColumns">生成的透视表列别名 eg: f1=栏位1;f2=栏位2</param>
        /// <paramname="groupFields">分组字段 eg:f1,f2</param>
        /// <returns></returns>
        public DataTable CreatePivotTable(DataTable dataSource, string totalColumn, string valueColumn, string aliasColumns, string groupFields, object nullValue = null)
        {
            if (dataSource ==null || string.IsNullOrEmpty(totalColumn) ||string.IsNullOrEmpty(valueColumn) ||string.IsNullOrEmpty(groupFields))
                return null;
            int i;
            string[] arry, arry2;
            DataTable result, table;
            DataView dataView;
            DataRow newRow;
            DataColumn col;
            List<DataColumn> columns;
            Type valueType;
            StringBuilder wheresb;

            if (nullValue ==null) nullValue =DBNull.Value;
            string[] groupColumns = groupFields.Split(',');
            result = new DataTable();
            //构建数据列
            columns = new List<DataColumn>();
            for (i =0; i < groupColumns.Length; i++)
            {
                col = new DataColumn(groupColumns[i],dataSource.Columns[groupColumns[i].Trim()].DataType);
                col.Namespace = groupColumns[i];
                result.Columns.Add(col);
                columns.Add(col);
            }
            if (columns.Count >0) result.PrimaryKey =columns.ToArray();
            dataView = dataSource.DefaultView;
            dataView.RowFilter ="";
            table = dataView.ToTable(true,totalColumn);
            valueType = dataSource.Columns[valueColumn].DataType;
            columns.Clear();
            foreach (DataRow row in table.Rows)
            {
                col = new DataColumn(row[0].ToString(),valueType);
                col.Namespace = totalColumn;
                result.Columns.Add(col);
                columns.Add(col);
            }
            //填充分组栏位数据
            table = dataView.ToTable(true,groupColumns);
            wheresb = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                newRow = result.NewRow();
                wheresb.Clear();
                for (i =0; i < groupColumns.Length; i++)
                {
                    newRow[groupColumns[i]] =row[groupColumns[i]];
                    wheresb.AppendFormat(" and {0}='{1}'",groupColumns[i],row[groupColumns[i]].ToString());
                }
                //查找数据填充剩余栏位的数据
                for (i =0; i < columns.Count; i++)
                {
                    dataView.RowFilter =string.Format("{0}='{1}' {2}",totalColumn, columns[i].ColumnName,wheresb.ToString());
                    newRow[columns[i]] = dataView.Count > 0 ? dataView.ToTable().AsEnumerable().Sum(p => Convert.ToDouble(p[valueColumn])) : nullValue;
                    
                }
                result.Rows.Add(newRow);
            }
            dataView.RowFilter ="";
            //设置透视表列别名
            if (!string.IsNullOrEmpty(aliasColumns))
            {
                arry = aliasColumns.Split(';');
                for (i =0; i < arry.Length; i++)
                {
                    arry[i] =arry[i].Trim();
                    if (arry[i].Length <1) continue;
                    arry2 = arry[i].Split('=');
                    if (arry2.Length <2) continue;
                    arry2[0] =arry2[0].Trim();
                    arry2[1] =arry2[1].Trim();
                    if (arry2[0].Length <1 || arry2[1].Length <1) continue;
                    if (!result.Columns.Contains(arry2[0]))continue;
                    result.Columns[arry2[0]].ColumnName =arry2[1];
                }
            }
            return result;
        }

        /// <summary>   
        /// 将DataTable数据导出到Excel表   
        /// </summary>   
        /// <param name="tmpDataTable">要导出的DataTable</param>   
        /// <param name="strFileName">Excel的保存路径及名称</param>   
        public void DataTabletoExcel(System.Data.DataTable tmpDataTable, string strFileName)
        {
            if (tmpDataTable == null)
            {
                return;
            }
            long rowNum = tmpDataTable.Rows.Count;//行数   
            int columnNum = tmpDataTable.Columns.Count;//列数   
            Excel.Application m_xlApp = new Excel.Application();
            m_xlApp.DisplayAlerts = false;//不显示更改提示   
            m_xlApp.Visible = false;

            Excel.Workbooks workbooks = m_xlApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];//取得sheet1   

            try
            {
                if (rowNum > 65536)//单张Excel表格最大行数   
                {
                    long pageRows = 65535;//定义每页显示的行数,行数必须小于65536   
                    int scount = (int)(rowNum / pageRows);//导出数据生成的表单数   
                    if (scount * pageRows < rowNum)//当总行数不被pageRows整除时，经过四舍五入可能页数不准   
                    {
                        scount = scount + 1;
                    }
                    for (int sc = 1; sc <= scount; sc++)
                    {
                        if (sc > 1)
                        {
                            object missing = System.Reflection.Missing.Value;
                            worksheet = (Excel.Worksheet)workbook.Worksheets.Add(
                                        missing, missing, missing, missing);//添加一个sheet   
                        }
                        else
                        {
                            worksheet = (Excel.Worksheet)workbook.Worksheets[sc];//取得sheet1   
                        }
                        string[,] datas = new string[pageRows + 1, columnNum];

                        for (int i = 0; i < columnNum; i++) //写入字段   
                        {
                            datas[0, i] = tmpDataTable.Columns[i].Caption;//表头信息   
                        }
                        //Excel.Range range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]);
                        Excel.Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]];
                        range.Interior.ColorIndex = 15;//15代表灰色   
                        range.Font.Bold = true;
                        range.Font.Size = 9;

                        int init = int.Parse(((sc - 1) * pageRows).ToString());
                        int r = 0;
                        int index = 0;
                        int result;
                        if (pageRows * sc >= rowNum)
                        {
                            result = (int)rowNum;
                        }
                        else
                        {
                            result = int.Parse((pageRows * sc).ToString());
                        }

                        for (r = init; r < result; r++)
                        {
                            index = index + 1;
                            for (int i = 0; i < columnNum; i++)
                            {
                                object obj = tmpDataTable.Rows[r][tmpDataTable.Columns[i].ToString()];
                                datas[index, i] = obj == null ? "" : "'" + obj.ToString().Trim();//在obj.ToString()前加单引号是为了防止自动转化格式   
                            }
                            System.Windows.Forms.Application.DoEvents();
                            //添加进度条   
                        }

                        Excel.Range fchR = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[index + 1, columnNum]];
                        fchR.Value2 = datas;
                        worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。   
                        m_xlApp.WindowState = Excel.XlWindowState.xlMaximized;//Sheet表最大化   
                        //range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[index + 1, columnNum]);
                        range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[index + 1, columnNum]];
                        
                        //range.Interior.ColorIndex = 15;//15代表灰色   
                        range.Font.Size = 9;
                        range.RowHeight = 14.25;
                        range.Borders.LineStyle = 1;
                        range.HorizontalAlignment = 1;
                    }
                }
                else
                {
                    string[,] datas = new string[rowNum + 1, columnNum];
                    for (int i = 0; i < columnNum; i++) //写入字段   
                    {
                        datas[0, i] = tmpDataTable.Columns[i].Caption;
                    }
                    //Excel.Range range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]);
                    Excel.Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]];
                    range.Interior.ColorIndex = 15;//15代表灰色   
                    range.Font.Bold = true;
                    range.Font.Size = 9;

                    int r = 0;
                    for (r = 0; r < rowNum; r++)
                    {
                        for (int i = 0; i < columnNum; i++)
                        {
                            object obj = tmpDataTable.Rows[r][tmpDataTable.Columns[i].ToString()];
                            datas[r + 1, i] = obj == null ? "" : "'" + obj.ToString().Trim();//在obj.ToString()前加单引号是为了防止自动转化格式   
                        }
                        System.Windows.Forms.Application.DoEvents();
                        //添加进度条   
                    }
                    //Excel.Range fchR1 = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[rowNum + 1, columnNum]);
                    Excel.Range fchR = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowNum + 1, columnNum]];
                    fchR.Value2 = datas;
                    worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。   
                    m_xlApp.WindowState = Excel.XlWindowState.xlMaximized;

                    //range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[rowNum + 1, columnNum]);
                    range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowNum + 1, columnNum]];
                    //range.Interior.ColorIndex = 15;//15代表灰色   
                    range.Font.Size = 9;
                    range.RowHeight = 14.25;
                    range.Borders.LineStyle = 1;
                    range.HorizontalAlignment = 1;
                }
                workbook.Saved = true;
                //Create a workbook
                Excel.Workbook workbookCopy = new Excel.Workbook();

                //Load an excel file including pivot table
                Excel.Application xlApp = new Excel.Application();
                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                xlApp.ScreenUpdating = false;
                Excel.Workbook xlsWorkBook = xlApp.Workbooks.Open(strFileName, System.Type.Missing, System.Type.Missing, System.Type.Missing,
                    System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing,
                    System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);
                Excel.Worksheet sheet = xlsWorkBook.Worksheets["数据表"];
                sheet.Copy(worksheet);
                //workbook.SaveCopyAs(strFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出异常：" + ex.Message, "导出异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                EndReport();
                //setpivottable(@strFileName, "数据表", "透视表");
            }
        }

        /// <summary>   
        /// 退出报表时关闭Excel和清理垃圾Excel进程   
        /// </summary>   
        private void EndReport()
        {
            object missing = System.Reflection.Missing.Value;
            try
            {
                m_xlApp.Workbooks.Close();
                m_xlApp.Workbooks.Application.Quit();
                m_xlApp.Application.Quit();
                m_xlApp.Quit();
            }
            catch { }
            finally
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_xlApp.Workbooks);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_xlApp.Application);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_xlApp);
                    m_xlApp = null;
                }
                catch { }
                try
                {
                    //清理垃圾进程   
                    this.killProcessThread();
                }
                catch { }
                GC.Collect();
            }
        }
        /// <summary>   
        /// 杀掉不死进程   
        /// </summary>   
        private void killProcessThread()
        {
            ArrayList myProcess = new ArrayList();
            for (int i = 0; i < myProcess.Count; i++)
            {
                try
                {
                    System.Diagnostics.Process.GetProcessById(int.Parse((string)myProcess[i])).Kill();
                }
                catch { }
            }
        }

        public void datatble2excel(DataTable DT,string xlsxPath) 
        {
            //Create a workbook
            Workbook workbook = new Workbook();

            //Load an excel file including pivot table
            workbook.LoadFromFile(@xlsxPath);

            //Modify data of data source
            Worksheet sheet = workbook.Worksheets["数据表"];
            //获取第一张工作表
            //从工作表第一行第一列开始插入
            sheet.InsertDataTable(DT, true, 1, 1);
            workbook.Save();
            //保存文档
            setpivottable(xlsxPath,"数据表","透视表");
        }
    }
}
