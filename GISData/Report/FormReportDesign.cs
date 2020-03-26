using DevExpress.XtraReports.UI;
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

namespace GISData.Report
{
    public partial class FormReportDesign : Form
    {
        protected int XRTableCellHeight = 38;
        public FormReportDesign()
        {
            InitializeComponent();
        }

        private void FormReportDesign_Load(object sender, EventArgs e)
        {
            bindDataSoure();
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void bindDataSoure()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO");
            comboBoxDataSoure.DataSource = dt;
            comboBoxDataSoure.DisplayMember = "REG_ALIASNAME";
            comboBoxDataSoure.ValueMember = "REG_NAME";
            comboBoxDataSoure.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XtraReport activeReport = this.reportDesigner1.ActiveDesignPanel.Report;
            CommonClass common = new CommonClass();
            ITable table = common.GetLayerByName(this.comboBoxDataSoure.Text.ToString()).FeatureClass as ITable;
            DataTable dt = ToDataTable(table);
            DataSet myDataSet = new DataSet();
            myDataSet.Tables.Add(dt);
            activeReport.DataSource = myDataSet;
            InitBands(activeReport);
            InitDetailsBasedonXRTable(activeReport);
        }

        public void InitBands(XtraReport rpt)
        {
            DetailBand detail = new DetailBand();
            PageHeaderBand pageHeader = new PageHeaderBand();
            ReportFooterBand reportFooter = new ReportFooterBand();
            detail.Height = XRTableCellHeight;
            reportFooter.Height = 380;
            pageHeader.Height = XRTableCellHeight;

            rpt.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { detail, pageHeader, reportFooter });
        }

        public void InitDetailsBasedonXRTable(XtraReport rpt)
        {
            DataSet ds = ((DataSet)rpt.DataSource);
            int colCount = ds.Tables[0].Columns.Count;
            int colWidth = (rpt.PageWidth - (rpt.Margins.Left + rpt.Margins.Right)) / colCount;

            // Create a table to represent headers
            XRTable tableHeader = new XRTable();
            tableHeader.Height = XRTableCellHeight;
            tableHeader.Width = (rpt.PageWidth - (rpt.Margins.Left + rpt.Margins.Right));
            tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeader.Font = new System.Drawing.Font("宋体", 12.75F, System.Drawing.FontStyle.Bold);

            // Create a table to display data
            XRTable tableDetail = new XRTable();
            tableDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetail.Width = (rpt.PageWidth - (rpt.Margins.Left + rpt.Margins.Right));
            tableDetail.Height = XRTableCellHeight;
            tableDetail.Font = new System.Drawing.Font("宋体", 12.75F, System.Drawing.FontStyle.Regular);


            XRTableRow headerRow = new XRTableRow();
            XRTableRow detailRow = new XRTableRow();
            // Create table cells, fill the header cells with text, bind the cells to data
            for (int i = 0; i < colCount; i++)
            {
                XRTableCell headerCell = new XRTableCell();
                headerCell.Width = colWidth;
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                headerCell.Text = ds.Tables[0].Columns[i].Caption;

                XRTableCell detailCell = new XRTableCell();
                detailCell.Width = colWidth;
                detailCell.DataBindings.Add("Text", null, ds.Tables[0].Columns[i].Caption);
                detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;

                // Place the cells into the corresponding tables
                headerRow.Cells.Add(headerCell);
                detailRow.Cells.Add(detailCell);
            }

            headerRow.Width = tableHeader.Width;
            tableHeader.Rows.Add(headerRow);

            detailRow.Width = tableDetail.Width;
            tableDetail.Rows.Add(detailRow);

            // Place the table onto a report's Detail band
            rpt.Bands[BandKind.PageHeader].Controls.Add(tableHeader);
            rpt.Bands[BandKind.Detail].Controls.Add(tableDetail);

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

        private void fieldListDockPanel1_Click(object sender, EventArgs e)
        {
        }
    }
}
