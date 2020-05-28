using DevExpress.XtraReports.Design;
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

namespace GISData.Report
{
    public partial class FormReportWord : Form
    {
        protected int XRTableCellHeight = 38;
        public FormReportWord()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void FormReportDesign_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void fieldListDockPanel1_Click(object sender, EventArgs e)
        {
        }

        private void bbiSaveFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraReport report = this.reportDesigner1.ActiveDesignPanel.Report;
            ReportDesignTool designTool = new ReportDesignTool(report);
            IDesignForm designForm = designTool.DesignRibbonForm;
            PropertyGridDockPanel propertyGrid = (PropertyGridDockPanel)designForm.DesignDockManager[DesignDockPanelType.PropertyGrid];
            NameExtender name = new NameExtender();
            Console.WriteLine(name.GetName(this.propertyGridDockPanel1));
        }
    }
}
