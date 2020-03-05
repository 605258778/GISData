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
            DataTable dt = db.GetDataBySql("select NAME,STATE,ERROR,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT,ID from GISDATA_TBTOPO");
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
            //主要有添加构建拓扑，拓扑中添加要素，添加规则，输出拓扑错误的功能。  
            TopologyChecker topocheck = new TopologyChecker(mainlogyDataSet);//传入要处理的要素数据集  
            topocheck.PUB_TopoBuild("dataset_Topology");//构建拓扑的名字  
            topocheck.PUB_AddFeatureClass(null);//将该要素中全部要素都加入拓扑  
            Dictionary<string, string> DicTopoData = new Dictionary<string, string>();
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string ID = row["ID"].ToString();
                string NAME = row["NAME"].ToString();
                string STATE = row["STATE"].ToString();
                string ERROR = row["ERROR"].ToString();
                string TYPE = row["TYPE"].ToString();
                string TABLENAME = row["TABLENAME"].ToString();
                string WHERESTRING = row["WHERESTRING"].ToString();
                string SUPTABLE = row["SUPTABLE"].ToString();
                string INPUTTEXT = row["INPUTTEXT"].ToString();
                if (INPUTTEXT!= null && INPUTTEXT!="")
                {
                    DicTopoData.Add(ID, INPUTTEXT);
                }
                row["STATE"] = "检查中";
                GISData.Common.TopologyChecker.TopoErroType aa = GetTypeByString(TYPE);
                if (TYPE == "面多部件检查" || TYPE == "面自相交检查")
                {
                    topocheck.OtherRule(ID, TYPE, topocheck.PUB_GetAllFeatureClassByName(TABLENAME));
                }else if (SUPTABLE == null || SUPTABLE == "")
                {
                    topocheck.PUB_AddRuleToTopology(ID,GetTypeByString(TYPE), (topocheck.PUB_GetAllFeatureClassByName(TABLENAME)));
                }
                else 
                {
                    topocheck.PUB_AddRuleToTopology(ID,GetTypeByString(TYPE), (topocheck.PUB_GetAllFeatureClassByName(TABLENAME)), (topocheck.PUB_GetAllFeatureClassByName(SUPTABLE)));
                }
            }
            topocheck.doValidateTopology(selectRows, DicTopoData);
            Dictionary<string,int> DicTopoError = topocheck.DicTopoError;
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string ID = row["ID"].ToString();
                row["ERROR"] = DicTopoError[ID];
                row["STATE"] = "检查完成";
            }
        }

        public Boolean UpdateGrid() 
        {
            return true;
        }

        private TopologyChecker.TopoErroType GetTypeByString(string type)
        {
            switch (type)
            {
                case "第二个图层面要素必须被第一个图层任一面要素覆盖":
                    return TopologyChecker.TopoErroType.第二个图层面要素必须被第一个图层任一面要素覆盖;
                case "第一个图层面要素必须被第一个图层任一面要素包含":
                    return TopologyChecker.TopoErroType.第一个图层面要素必须被第一个图层任一面要素包含;
                case "第一个图层线要素不被第二个线图层线要素覆盖":
                    return TopologyChecker.TopoErroType.第一个图层线要素不被第二个线图层线要素覆盖;
                case "第一个图层线要素应被第二个线图层线要素覆盖":
                    return TopologyChecker.TopoErroType.第一个图层线要素应被第二个线图层线要素覆盖;
                case "点要素必须落在面要素边界上":
                    return TopologyChecker.TopoErroType.点要素必须落在面要素边界上;
                case "点要素必须落在面要素内":
                    return TopologyChecker.TopoErroType.点要素必须落在面要素内;
                case "点要素应被线要素覆盖":
                    return TopologyChecker.TopoErroType.点要素应被线要素覆盖;
                case "点要素应在线要素的端点上":
                    return TopologyChecker.TopoErroType.点要素应在线要素的端点上;
                case "点要素之间不相交":
                    return TopologyChecker.TopoErroType.点要素之间不相交;
                case "点要素重合点要素":
                    return TopologyChecker.TopoErroType.点要素重合点要素;
                case "两图层面要素必须互相覆盖":
                    return TopologyChecker.TopoErroType.两图层面要素必须互相覆盖;
                case "面要素必须只包含一个点要素":
                    return TopologyChecker.TopoErroType.面要素必须只包含一个点要素;
                case "面要素边界必须被线要素覆盖":
                    return TopologyChecker.TopoErroType.面要素边界必须被线要素覆盖;
                case "面要素的边界必须被另一面要素边界覆盖":
                    return TopologyChecker.TopoErroType.面要素的边界必须被另一面要素边界覆盖;
                case "面要素间无重叠":
                    return TopologyChecker.TopoErroType.面要素间无重叠;
                case "面要素内必须包含至少一个点要素":
                    return TopologyChecker.TopoErroType.面要素内必须包含至少一个点要素;
                case "面要素之间无空隙":
                    return TopologyChecker.TopoErroType.面要素之间无空隙;
                case "图层间面要素不能相互覆盖":
                    return TopologyChecker.TopoErroType.图层间面要素不能相互覆盖;
                case "线必须不相交或内部接触":
                    return TopologyChecker.TopoErroType.线必须不相交或内部接触;
                case "线不能是多段":
                    return TopologyChecker.TopoErroType.线不能是多段;
                case "线要素必须不相交":
                    return TopologyChecker.TopoErroType.线要素必须不相交;
                case "线要素必须跟面图层边界的一部分或全部重叠":
                    return TopologyChecker.TopoErroType.线要素必须跟面图层边界的一部分或全部重叠;
                case "线要素必须在面内":
                    return TopologyChecker.TopoErroType.线要素必须在面内;
                case "线要素不能自相交":
                    return TopologyChecker.TopoErroType.线要素不能自相交;
                case "线要素不能自重叠":
                    return TopologyChecker.TopoErroType.线要素不能自重叠;
                case "线要素不允许有假节点":
                    return TopologyChecker.TopoErroType.线要素不允许有假节点;
                case "线要素不允许有悬挂点":
                    return TopologyChecker.TopoErroType.线要素不允许有悬挂点;
                case "线要素端点必须被点要素覆盖":
                    return TopologyChecker.TopoErroType.线要素端点必须被点要素覆盖;
                case "线要素间不能有相互重叠部分":
                    return TopologyChecker.TopoErroType.线要素间不能有相互重叠部分;
                case "线要素间不能重叠和相交":
                    return TopologyChecker.TopoErroType.线要素间不能重叠和相交;
                case "线要素之间不能相交":
                    return TopologyChecker.TopoErroType.线要素之间不能相交;
                case "要素大于最小容差":
                    return TopologyChecker.TopoErroType.要素大于最小容差;
                case "面要素无多部件":
                    return TopologyChecker.TopoErroType.面要素无多部件;
                default:
                    return TopologyChecker.TopoErroType.任何规则;
            }
        }
    }
}
