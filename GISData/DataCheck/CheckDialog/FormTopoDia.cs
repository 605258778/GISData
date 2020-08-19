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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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
        private ProgressBar progressBar = null;
        private string scheme;
        private string topoDir;
        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo, CheckBox cb, string scheme, CheckBox cbCheckMain, GridControl gridControlError, ProgressBar progressbar)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.scheme = scheme;
            this.checkBox = cb;
            this.checkBoxCheckMain = cbCheckMain;
            this.gridControlError = gridControlError;
            this.progressBar = progressbar;

            CommonClass common = new CommonClass();
            ConnectDB db = new ConnectDB();
            DataTable DT = db.GetDataBySql("select GLDWNAME from GISDATA_GLDW where GLDW = '" + common.GetConfigValue("GLDW") + "'");
            DataRow dr = DT.Select(null)[0];
            this.topoDir = common.GetConfigValue("SAVEDIR") + "\\" + dr["GLDWNAME"].ToString() + "\\错误参考\\拓扑错误";
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
            DataTable dt = db.GetDataBySql("select * from GISDATA_TBTOPO where SCHEME ='" + this.scheme + "' and STEP_NO = '" + this.stepNo + "'");


            foreach (DataRow dr in dt.Rows)  //对特定的行添加限制条件
            {
                if (dr["CHECKTYPE"].ToString() != "")
                {
                    string ID = dr["ID"].ToString();
                    string value = getXMLInnerText(ID);
                    dr["ERROR"] = value;
                    dr["STATE"] = "已检查";
                }
            }
            
            this.gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            topoErrorTable = new DataTable();
            foreach (DataColumn column in dt.Columns)
            {
                topoErrorTable.Columns.Add(column.ColumnName);
            }
        }


        public string getXMLInnerText(string ID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"checkEroor.xml");
                string strPath = string.Format("/Error/Topos/topo[@key=\"{0}\"]", ID);
                if (strPath != null)
                {
                    XmlElement selectXe = (XmlElement)doc.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    return selectXe.GetAttribute("value");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(CommonClass), e);
                return null;
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

        public void doCheckTopo(IHookHelper m_hookHelper)
        {
            try{
                XmlDocument doc = new XmlDocument();
                doc.Load(@"checkEroor.xml");
                var root = doc.DocumentElement;//取到根结点
                XmlElement Topos = doc.CreateElement("Topos");
                root.AppendChild(Topos);

                this.m_hookHelper = m_hookHelper;
                CommonClass common = new CommonClass();
                int[] selectRows = this.gridView1.GetSelectedRows();
                TopoChecker topocheck = new TopoChecker();
                Dictionary<string, string> DicTopoData = new Dictionary<string, string>();
                foreach (int itemRow in selectRows)
                {
                    DataRow row = this.gridView1.GetDataRow(itemRow);
                    this.gridView1.FocusedRowHandle = itemRow;
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
                        topocheck.OtherRule(ID, NAME, TYPE, TABLENAME, SUPTABLE, INPUTTEXT, m_hookHelper);
                    }
                    else 
                    {
                        //topocheck.OtherRule(ID, TYPE, common.GetPathByName(TABLENAME), null, INPUTTEXT, m_hookHelper);
                        topocheck.OtherRule(ID,NAME, TYPE, TABLENAME, null, INPUTTEXT, m_hookHelper);
                    }
                    Dictionary<string, int> DicTopoError = topocheck.DicTopoError;
                    row["ERROR"] = DicTopoError[row["ID"].ToString()];
                    row["STATE"] = "已检查";

                    XmlElement Xmlnode = doc.CreateElement("topo");
                    Topos.AppendChild(Xmlnode);
                    XmlAttribute xa = doc.CreateAttribute("key");
                    xa.Value = row["ID"].ToString();
                    XmlAttribute xa2 = doc.CreateAttribute("value");
                    xa2.Value = DicTopoError[ID].ToString();
                    Xmlnode.SetAttributeNode(xa);
                    Xmlnode.SetAttributeNode(xa2); 

                    if (DicTopoError[ID].ToString() != "0")
                    {
                        topoErrorTable.ImportRow(row);
                    }
                }
                doc.Save(@"checkEroor.xml");
                //doc.Save(Application.StartupPath+"/checkEroor.xml");
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(FormTopoDia), e);
            }
        }

        public Boolean UpdateGrid() 
        {
            return true;
        }

        /// <summary>
        /// 双击要素，查看错误详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                            string errorType = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CHECKTYPE").ToString();
                            string idname = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ID").ToString();
                            string name = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "NAME").ToString();
                            string TABLENAME = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TABLENAME").ToString();
                            CommonClass common = new CommonClass();
                            string outString = this.topoDir + "\\" + name + idname + ".shp";
                            IFeatureClass IFC = common.GetFeatureClassByShpPath(outString);
                            TableShow(IFC, this.gridControlError, "拓扑检查ByGP", TABLENAME, name + idname + ".shp");
                            Marshal.ReleaseComObject(IFC);
                        }
                        else 
                        {
                            Console.WriteLine("未选中");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormTopoDia), ex);
                Console.WriteLine("MyException Throw:" + ex.Message);
            }
        }

        private void gridControl13333_MouseDown(object sender, MouseEventArgs e)
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
                            string errorType = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CHECKTYPE").ToString();
                            string idname = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ID").ToString();
                            string TABLENAME = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TABLENAME").ToString();


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
                        else
                        {
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
        /// <summary>
        /// 显示表格
        /// </summary>
        /// <param name="pFeatureClass">显示图层</param>
        /// <param name="grid">装载grid</param>
        /// <param name="ViewCaption">检查类型</param>
        /// <param name="NewItemRowText">检查表格</param>
        /// <param name="GroupPanelText">错误结果名</param>
        public void TableShow(IFeatureClass pFeatureClass, GridControl grid, string ViewCaption, string NewItemRowText, string GroupPanelText)
        {
            if (pFeatureClass == null) return;
            GridView gridView = grid.DefaultView as GridView;
            gridView.Columns.Clear();

            DataTable dt = new DataTable();
            DataColumn dc = null;
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                dc = new DataColumn(pFeatureClass.Fields.get_Field(i).AliasName);
                dt.Columns.Add(dc);
            }
            IFeatureCursor pFeatureCuror = pFeatureClass.Search(null, true);
            IFeature pFeature = pFeatureCuror.NextFeature();

            DataRow dr = null;
            while (pFeature != null)
            {
                dr = dt.NewRow();
                for (int j = 0; j < pFeatureClass.Fields.FieldCount; j++)
                {
                    if (pFeatureClass.FindField(pFeatureClass.ShapeFieldName) == j)
                    {
                        dr[j] = pFeatureClass.ShapeType.ToString();
                    }
                    else
                    {
                        dr[j] = pFeature.get_Value(j).ToString();

                    }
                }

                dt.Rows.Add(dr);
                pFeature = pFeatureCuror.NextFeature();
            }
            grid.DataSource = dt;
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsSelection.MultiSelect = true;
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.ViewCaption = ViewCaption;
            gridView.NewItemRowText = NewItemRowText;
            gridView.GroupPanelText = GroupPanelText;
            Marshal.ReleaseComObject(pFeatureCuror);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
