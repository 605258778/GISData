using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office;

namespace GISData.Report
{
    public partial class Formdes : Form
    {
        public Formdes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataColumn dcName = new DataColumn("dcName", typeof(string));
            DataColumn dcGroup = new DataColumn("dcGroup", typeof(string));
            DataColumn dcAge = new DataColumn("dcAge", typeof(string));
            DataColumn dcTel = new DataColumn("dcTel", typeof(string));
            DataColumn dcMoney = new DataColumn("dcMoney", typeof(double));

            DataTable dt = new DataTable();
            dt.Columns.Add(dcName);
            dt.Columns.Add(dcGroup);
            dt.Columns.Add(dcAge);
            dt.Columns.Add(dcTel);
            dt.Columns.Add(dcMoney);

            DataRow dr = dt.NewRow();
            dr["dcName"] = "aaa";
            dr["dcGroup"] = "a";
            dr["dcAge"] = "14";
            dr["dcTel"] = "1";
            dr["dcMoney"] = "13223";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1["dcName"] = "aaa";
            dr1["dcGroup"] = "b";
            dr1["dcAge"] = "14";
            dr1["dcTel"] = "2";
            dr1["dcMoney"] = "13223";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2["dcName"] = "aaa";
            dr2["dcGroup"] = "b";
            dr2["dcAge"] = "14";
            dr2["dcTel"] = "3";
            dr2["dcMoney"] = "13223";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["dcName"] = "aaa";
            dr3["dcGroup"] = "c";
            dr3["dcAge"] = "14";
            dr3["dcTel"] = "4";
            dr3["dcMoney"] = "13223";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4["dcName"] = "aaa";
            dr4["dcGroup"] = "c";
            dr4["dcAge"] = "14";
            dr4["dcTel"] = "5";
            dr4["dcMoney"] = "13223";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5["dcName"] = "aaa";
            dr5["dcGroup"] = "c";
            dr5["dcAge"] = "15";
            dr5["dcTel"] = "6";
            dr5["dcMoney"] = "13223";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6["dcName"] = "aaa";
            dr6["dcGroup"] = "a";
            dr6["dcAge"] = "15";
            dr6["dcTel"] = "7";
            dr6["dcMoney"] = "13223";
            dt.Rows.Add(dr6);
            dt.AcceptChanges();

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            ds.WriteXml(Application.StartupPath + "\\ExcelBindingXml.xml");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Application.StartupPath + "\\ExcelBindingXml.xml");

            Microsoft.Office.Interop.Excel.Application m_objExcel = null;

            Microsoft.Office.Interop.Excel._Workbook m_objBook = null;

            Microsoft.Office.Interop.Excel.Sheets m_objSheets = null;

            Microsoft.Office.Interop.Excel._Worksheet m_objSheet = null;

            Microsoft.Office.Interop.Excel.Range m_objRange = null;

            object m_objOpt = System.Reflection.Missing.Value;

            try
            {

                m_objExcel = new Microsoft.Office.Interop.Excel.Application();
                m_objBook = m_objExcel.Workbooks.Open(Application.StartupPath + "\\ExcelTemplate.xlsx", m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt);
                m_objSheets = (Microsoft.Office.Interop.Excel.Sheets)m_objBook.Worksheets;
                m_objSheet = (Microsoft.Office.Interop.Excel._Worksheet)(m_objSheets.get_Item(1));
                int maxRow = m_objSheet.UsedRange.Rows.Count;
                int maxCol = m_objSheet.UsedRange.Columns.Count;
                int TitleRow = 0;
                for (int excelrow = 1; excelrow <= maxRow; excelrow++)
                {
                    for (int col = 0; col < maxCol; col++)
                    {
                        string excelColName = ExcelColNumberToColText(col);
                        m_objRange = m_objSheet.get_Range(excelColName + excelrow.ToString(), m_objOpt);
                        m_objRange.Text.ToString().Contains("$");
                        TitleRow = excelrow;
                        break;
                    }
                }
                for (int excelrow = 1; excelrow <= ds.Tables[0].Rows.Count; excelrow++) 
                {
                    DataRow dr = ds.Tables[0].Rows[excelrow-1];
                    for (int col = 0; col < maxCol; col++) 
                    {
                        string excelColName = ExcelColNumberToColText(col);

                        m_objRange = m_objSheet.get_Range(excelColName + TitleRow.ToString(), m_objOpt);
                        //Microsoft.Office.Interop.Excel.Range item_objRange = m_objSheet.get_Range(excelColName + (maxRow + excelrow).ToString(), m_objOpt);
                                
                        if (m_objRange.Text.ToString().Replace("$", "") == ds.Tables[0].Columns[col].ColumnName)
                        {
                            m_objSheet.Cells[maxRow + excelrow, col+1].value = dr[col].ToString();
                            m_objSheet.Cells[maxRow + excelrow, col+1].Style = m_objRange.Style;
                            Console.WriteLine(m_objSheet.Cells[maxRow + excelrow, col + 1].value + ":" + dr[col].ToString());
                            //item_objRange.Value2 = dr[col].ToString();
                        }

                    }
                }
                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[TitleRow, m_objOpt];
                range.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);

                m_objExcel.DisplayAlerts = false;
                m_objBook.SaveAs(Application.StartupPath + "\\ExcelBindingXml.xlsx", m_objOpt, m_objOpt,
                m_objOpt, m_objOpt, m_objOpt, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,

                                                m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                m_objBook.Close(m_objOpt, m_objOpt, m_objOpt);
                m_objExcel.Workbooks.Close();
                m_objExcel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objExcel);
                m_objBook = null;
                m_objExcel = null;
                GC.Collect();
            }
        }

        private string ExcelColNumberToColText(int colNumber)
        {
            string colText = "";
            switch (colNumber)
            {
                case 0: colText = colText + "A"; break;
                case 1: colText = colText + "B"; break;
                case 2: colText = colText + "C"; break;
                case 3: colText = colText + "D"; break;
                case 4: colText = colText + "E"; break;
                case 5: colText = colText + "F"; break;
                case 6: colText = colText + "G"; break;
                case 7: colText = colText + "H"; break;
                case 8: colText = colText + "I"; break;
                case 9: colText = colText + "J"; break;
                case 10: colText = colText + "K"; break;
                case 11: colText = colText + "L"; break;
                case 12: colText = colText + "M"; break;
                case 13: colText = colText + "N"; break;
                case 14: colText = colText + "O"; break;
                case 15: colText = colText + "P"; break;
                case 16: colText = colText + "Q"; break;
                case 17: colText = colText + "R"; break;
                case 18: colText = colText + "S"; break;
                case 19: colText = colText + "T"; break;
                case 20: colText = colText + "U"; break;
                case 21: colText = colText + "V"; break;
                case 22: colText = colText + "W"; break;
                case 23: colText = colText + "X"; break;
                case 24: colText = colText + "Y"; break;
                case 25: colText = colText + "Z"; break;
                default: break;
            }
            return colText;
        }
    }
}
