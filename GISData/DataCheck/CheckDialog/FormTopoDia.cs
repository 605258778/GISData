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

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormTopoDia : Form
    {
        private string stepNo;
        private CheckBox checkBox;

        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo, CheckBox cb)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
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
            DataTable dt = db.GetDataBySql("select NAME,STATE,ERROR,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT from GISDATA_TBTOPO");
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
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

        public void doCheckTopo()
        {
            string GdbPath = Application.StartupPath + "\\GISData.gdb";
            FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
            IWorkspace workspace = fac.OpenFromFile(GdbPath, 0);
            CommonClass common = new CommonClass();
            IFeatureDataset mainlogyDataSet = common.getDataset(workspace);
            int[] selectRows = this.gridView1.GetSelectedRows();
            string nametopo = "";
            foreach(int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string  NAME = row["NAME"].ToString();
                nametopo = NAME;
                string STATE = row["STATE"].ToString();
                string ERROR = row["ERROR"].ToString();
                string TYPE = row["TYPE"].ToString();
                string TABLENAME = row["TABLENAME"].ToString();
                string WHERESTRING = row["WHERESTRING"].ToString();
                string SUPTABLE = row["SUPTABLE"].ToString();
                string INPUTTEXT = row["INPUTTEXT"].ToString();
            }
            //主要有添加构建拓扑，拓扑中添加要素，添加规则，输出拓扑错误的功能。  
            TopologyChecker topocheck = new TopologyChecker(mainlogyDataSet);//传入要处理的要素数据集  
            topocheck.PUB_TopoBuild(nametopo);//构建拓扑的名字  
            topocheck.PUB_AddFeatureClass(null);//将该要素中全部要素都加入拓扑  
            //添加规则  
            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.两图层面要素必须互相覆盖, (topocheck.PUB_GetAllFeatureClass())[0], (topocheck.PUB_GetAllFeatureClass())[1]);
            //获取生成的拓扑图层并添加  
        }
    }
}
