using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormAcceptance : Form
    {

        private FormAttrDia AttrDia;
        private FormTopoDia TopoDia;
        private FormReportDia ReportDia;
        public string ModelName;
        public FormAcceptance(FormReportDia ReportDia, FormAttrDia AttrDia, FormTopoDia TopoDia)
        {
            InitializeComponent();
            this.ReportDia = ReportDia;
            this.AttrDia = AttrDia;
            this.TopoDia = TopoDia;
        }

        private void FormAcceptance_Load(object sender, EventArgs e)
        {
            getFile();
        }

        /// <summary>
        /// 获取所有模板文件
        /// </summary>
        private void getFile()
        {
            DirectoryInfo folder = new DirectoryInfo(Application.StartupPath + "\\Report");
            foreach (FileInfo file in folder.GetFiles("*.repx"))
            {
                this.comboBox1.Items.Add(file.Name);
            }
            this.comboBox1.SelectedIndex = 0;
        }

        private void doAcceptance()
        {
            XtraReport mReport = new XtraReport();
            mReport.LoadLayout(Application.StartupPath + "\\Report\\" + this.comboBox1.Text); //报表模板文件

            DataTable taskError = new DataTable();
            if (ReportDia.wwcxmTable != null) 
            {
                taskError = ReportDia.wwcxmTable;
            }
            taskError.TableName = "taskError";
            DataTable checkError = new DataTable();
            DataTable attrError = AttrDia.attrErrorTable;
            if (attrError != null) 
            {
                checkError = attrError;
            }
            DataTable topotable = TopoDia.topoErrorTable;
            if (topotable != null) 
            {
                checkError.Merge(topotable);
                checkError.TableName = "checkError";
            }
            //查找组件
            DetailReportBand DetailReport = mReport.FindControl("DetailReport", true) as DetailReportBand;
            DetailReportBand DetailReport1 = mReport.FindControl("DetailReport1", true) as DetailReportBand;
            DetailBand Detail1 = mReport.FindControl("Detail1", true) as DetailBand;
            DetailBand Detail2 = mReport.FindControl("Detail2", true) as DetailBand;
            XRLabel xrLabel4 = mReport.FindControl("XRLabel4", true) as XRLabel;//联系人
            XRLabel xrLabel5 = mReport.FindControl("XRLabel5", true) as XRLabel;//联系方式
            XRLabel xrLabel11 = mReport.FindControl("XRLabel11", true) as XRLabel;//检查人
            DetailReport.DataSource = taskError;
            DetailReport1.DataSource = checkError;
            mReport.ShowPreviewDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ModelName = this.comboBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
