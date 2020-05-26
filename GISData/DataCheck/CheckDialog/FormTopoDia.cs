using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ESRI.ArcGIS.Controls;
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
using TopologyCheck.Error;

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormTopoDia : Form
    {
        private string stepNo;
        private CheckBox checkBox;
        private CheckBox checkBoxCheckMain;
        private GridControl gridControlError = null;
        private IHookHelper m_hookHelper = null;
        public DataTable topoErrorTable = null;
        private string scheme;
        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo, CheckBox cb, string scheme,CheckBox cbCheckMain, GridControl gridControlError)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.scheme = scheme;
            this.checkBox = cb;
            this.checkBoxCheckMain = cbCheckMain;
            this.gridControlError = gridControlError;
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
            DataTable dt = db.GetDataBySql("select NAME,STATE,ERROR,CHECKTYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT,ID from GISDATA_TBTOPO where SCHEME ='" + this.scheme + "' and STEP_NO = '" + this.stepNo + "'");
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            topoErrorTable = new DataTable();
            foreach (DataColumn column in dt.Columns)
            {
                topoErrorTable.Columns.Add(column.ColumnName);
            }
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

        //public void doCheckTopo()
        //{
        //    string GdbPath = Application.StartupPath + "\\GISData.gdb";
        //    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
        //    IWorkspace workspace = fac.OpenFromFile(GdbPath, 0);
        //    CommonClass common = new CommonClass();
        //    IFeatureDataset mainlogyDataSet = common.getDataset(workspace);
        //    int[] selectRows = this.gridView1.GetSelectedRows();
        //    //主要有添加构建拓扑，拓扑中添加要素，添加规则，输出拓扑错误的功能。  
        //    TopologyChecker topocheck = new TopologyChecker(mainlogyDataSet);//传入要处理的要素数据集  
        //    topocheck.PUB_TopoBuild("dataset_Topology");//构建拓扑的名字  
        //    topocheck.PUB_AddFeatureClass(null);//将该要素中全部要素都加入拓扑  
        //    Dictionary<string, string> DicTopoData = new Dictionary<string, string>();
        //    foreach (int itemRow in selectRows)
        //    {
        //        DataRow row = this.gridView1.GetDataRow(itemRow);
        //        string ID = row["ID"].ToString();
        //        string NAME = row["NAME"].ToString();
        //        string STATE = row["STATE"].ToString();
        //        string ERROR = row["ERROR"].ToString();
        //        string TYPE = row["CHECKTYPE"].ToString();
        //        string TABLENAME = row["TABLENAME"].ToString();
        //        string WHERESTRING = row["WHERESTRING"].ToString();
        //        string SUPTABLE = row["SUPTABLE"].ToString();
        //        string INPUTTEXT = row["INPUTTEXT"].ToString();
        //        if (INPUTTEXT!= null && INPUTTEXT!="")
        //        {
        //            DicTopoData.Add(ID, INPUTTEXT);
        //        }
        //        row["STATE"] = "检查中";
        //        GISData.Common.TopologyChecker.TopoErroType aa = GetTypeByString(TYPE);
        //        if (TYPE == "面多部件检查" || TYPE == "面自相交检查")
        //        {
        //            topocheck.OtherRule(ID, TYPE, topocheck.PUB_GetAllFeatureClassByName(TABLENAME));
        //        }else if (SUPTABLE == null || SUPTABLE == "")
        //        {
        //            topocheck.PUB_AddRuleToTopology(ID,GetTypeByString(TYPE), (topocheck.PUB_GetAllFeatureClassByName(TABLENAME)));
        //        }
        //        else 
        //        {
        //            topocheck.PUB_AddRuleToTopology(ID,GetTypeByString(TYPE), (topocheck.PUB_GetAllFeatureClassByName(TABLENAME)), (topocheck.PUB_GetAllFeatureClassByName(SUPTABLE)));
        //        }
        //    }
        //    topocheck.doValidateTopology(selectRows, DicTopoData);
        //    Dictionary<string,int> DicTopoError = topocheck.DicTopoError;
        //    foreach (int itemRow in selectRows)
        //    {
        //        DataRow row = this.gridView1.GetDataRow(itemRow);
        //        string ID = row["ID"].ToString();
        //        row["ERROR"] = DicTopoError[ID];
        //        row["STATE"] = "检查完成";
        //        if (DicTopoError[ID].ToString() != "0") 
        //        {
        //            topoErrorTable.ImportRow(row);
        //        }
        //    }
        //}

        public void doCheckTopo(IHookHelper m_hookHelper)
        {
            this.m_hookHelper = m_hookHelper;
            CommonClass common = new CommonClass();
            int[] selectRows = this.gridView1.GetSelectedRows();
            //主要有添加构建拓扑，拓扑中添加要素，添加规则，输出拓扑错误的功能。  
            TopoChecker topocheck = new TopoChecker();//传入要处理的要素数据集  
            Dictionary<string, string> DicTopoData = new Dictionary<string, string>();
            foreach (int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string ID = row["ID"].ToString();
                string NAME = row["NAME"].ToString();
                string STATE = row["STATE"].ToString();
                string ERROR = row["ERROR"].ToString();
                string TYPE = row["CHECKTYPE"].ToString();
                string TABLENAME = row["TABLENAME"].ToString();
                string WHERESTRING = row["WHERESTRING"].ToString();
                string SUPTABLE = row["SUPTABLE"].ToString();
                string INPUTTEXT = row["INPUTTEXT"].ToString();
                if (INPUTTEXT != null && INPUTTEXT != "")
                {
                    DicTopoData.Add(ID, INPUTTEXT);
                }
                row["STATE"] = "检查中";
                if (SUPTABLE != null && SUPTABLE != "")
                {
                    topocheck.OtherRule(ID, TYPE, common.GetLayerByName(TABLENAME).FeatureClass, common.GetLayerByName(SUPTABLE).FeatureClass, INPUTTEXT, m_hookHelper);
                }
                else 
                {
                    topocheck.OtherRule(ID, TYPE, common.GetLayerByName(TABLENAME).FeatureClass, null, INPUTTEXT, m_hookHelper);
                }
                Dictionary<string, int> DicTopoError = topocheck.DicTopoError;
                row["ERROR"] = DicTopoError[row["ID"].ToString()];
                row["STATE"] = "检查完成";
                if (DicTopoError[ID].ToString() != "0")
                {
                    topoErrorTable.ImportRow(row);
                }
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

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
                if (e.Button == MouseButtons.Left && e.Clicks == 2)//判断是否左键双击
                {
                    //判断光标是否在行范围内 
                    if (hInfo.InRow)
                    {
                        if (this.checkBoxCheckMain.Checked)
                        {
                            var index = this.gridView1.GetFocusedDataSourceRowIndex();//获取数据行的索引值，从0开始
                            DataRowView row = (DataRowView)this.gridView1.GetRow(index);
                            string errorType = row["CHECKTYPE"].ToString();
                            string idname = row["ID"].ToString();
                            string TABLENAME = row["TABLENAME"].ToString();
                            
                            GridView gridView = this.gridControlError.DefaultView as GridView;
                            ErrorTable table = new ErrorTable();
                            DataTable table2 = table.GetTable(idname);
                            DataRow[] dr = table2.Select("1=1");
                            //ESRI.ArcGIS.Geometry.IGeometry igo = (ESRI.ArcGIS.Geometry.IGeometry) dr[0]["Geometry"];
                            gridView.Columns.Clear();
                            for (int i = 0; i < table2.Columns.Count; i++) 
                            {
                                GridColumn gridCol_FeatureID = new GridColumn();
                                gridCol_FeatureID.Caption = table2.Columns[i].Caption;
                                gridCol_FeatureID.FieldName = table2.Columns[i].ColumnName;
                                gridCol_FeatureID.Name = table2.Columns[i].ColumnName;
                                gridCol_FeatureID.Visible = true;
                                gridView.Columns.Add(gridCol_FeatureID);
                            }
                            this.gridControlError.DataSource = table2;
                            gridView.OptionsBehavior.Editable = false;
                            gridView.OptionsSelection.MultiSelect = true;
                            gridView.ViewCaption = "拓扑检查";
                            gridView.NewItemRowText = TABLENAME;
                            this.gridControlError.RefreshDataSource();
                        }
                        else {
                            Console.WriteLine("未选中");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MyException Throw:" + ex.Message);
            }
        }
    }
}
