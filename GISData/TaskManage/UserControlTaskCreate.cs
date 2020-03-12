namespace TaskManage
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraNavBar;
    using DevExpress.XtraNavBar.ViewInfo;
    using DevExpress.XtraTab;
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Columns;
    using DevExpress.XtraTreeList.Nodes;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Catalog;
    using ESRI.ArcGIS.CatalogUI;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.DataSourcesGDB;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using FormBase;
    using FunFactory;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using Utilities;
    using DevExpress.XtraGrid.Views.Base;
    using td.logic.sys;
    using td.db.orm;

    public class UserControlTaskCreate : UserControlBase1
    {
        private ButtonEdit buttonEditDataPath;
        private ButtonEdit buttonEditSavePath;
        private CheckedListBoxControl cListBoxKind;
        private CheckedListBoxControl cListBoxKind2;
        private CheckedListBoxControl cListBoxKind3;
        private CheckedListBoxControl cListBoxLayer;
        private IContainer components;
        private GridColumn gridColumn1;
        private GridColumn gridColumn2;
        private GridControl gridControl1;
        private GridView gridView1;
        internal ImageList ImageList1;
        internal ImageList ImageList2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelKind;
        private Label labelprogress;
        private ListBoxControl listBoxDataList;
        private ListBoxControl listBoxDist;
        private ListBoxControl listBoxName;
        private IFeatureLayer m_CountyLayer;
        private ITable m_CountyTable;
        private IFeatureLayer m_EditLayer;
        private IFeatureLayer m_TownLayer;
        private ITable m_TownTable;
        private IFeatureLayer m_VillageLayer;
        private ITable m_VillageTable;
        private const string mClassName = "TaskManage.UserControlTaskCreate";
   
        private string mEditKind = "小班";
        private string mEditKindCode = "";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private DataTable mFieldTable;
        private HookHelper mHookHelper;
        private bool mIsBatch = true;
        private DataTable mKindTable;
        private ArrayList mRangeList;
        private bool mSelected;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private const string myClassName = "任务创建";
        private NavBarControl navBarControl1;
        private NavBarGroup navBarGroup1;
        private NavBarGroup navBarGroup2;
        private NavBarGroup navBarGroup3;
        private NavBarGroup navBarGroup4;
        private NavBarGroup navBarGroup5;
        private NavBarGroupControlContainer navBarGroupControlContainer1;
        private NavBarGroupControlContainer navBarGroupControlContainer2;
        private NavBarGroupControlContainer navBarGroupControlContainer3;
        private NavBarGroupControlContainer navBarGroupControlContainer4;
        private NavBarGroupControlContainer navBarGroupControlContainer5;
        private Panel panel1;
        private Panel panel10;
        private Panel panel11;
        private Panel panel12;
        private Panel panel13;
        private Panel panel14;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private Panel panelKind;
        private ProgressBarControl progressBar;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private SimpleButton simpleButton1;
        private SimpleButton simpleButton2;
        private SimpleButton simpleButton4;
        private SimpleButton simpleButton5;
        private SimpleButton simpleButtonAdd;
        private SimpleButton simpleButtonClear;
        private SimpleButton simpleButtonCreate;
        private SimpleButton simpleButtonFind;
        private SimpleButton simpleButtonRemove;
        private SimpleButton simpleButtonReset;
        private SimpleButton simpleButtonSelectAll;
        internal TreeListColumn tcolBase1;
        internal TreeListColumn tcolBase2;
        private TextEdit textEdit1;
        internal TreeList tList;
        private XtraTabControl xtraTabControl1;
        private XtraTabPage xtraTabPage1;
        private XtraTabPage xtraTabPage2;

        public UserControlTaskCreate()
        {
            this.InitializeComponent();
        }

        private void buttonEditDataPath_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            IGxObjectFilter filter = new GxFilterDatasetsAndLayersClass();
            GxFilterShapefiles shapefiles = new GxFilterShapefilesClass();
            filter = shapefiles;
            IGxDialog dialog = new GxDialogClass {
                AllowMultiSelect = false,
                Title = "选择" + this.mEditKind + "数据",
                ObjectFilter = filter
            };
            IEnumGxObject selection = null;
            IGxObject obj3 = null;
            if (dialog.DoModalOpen((int) base.Handle, out selection))
            {
                obj3 = selection.Next();
                this.buttonEditDataPath.Text = obj3.FullName;
                this.buttonEditDataPath.Tag = Directory.GetParent(obj3.FullName).ToString() + @"\" + obj3.BaseName;
            }
        }

        private void buttonEditSavePath_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog {
                SelectedPath = this.buttonEditSavePath.Text
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.buttonEditSavePath.Text = dialog.SelectedPath.ToString();
            }
        }

        private bool CheckSelect(TreeListNode node)
        {
            if (node == null)
            {
                return false;
            }
            if (node.Nodes.Count == 0)
            {
                return false;
            }
            bool flag = true;
            bool flag2 = false;
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].StateImageIndex == 1)
                {
                    flag2 = true;
                }
                if (node.Nodes[i].StateImageIndex == 0)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                node.StateImageIndex = 1;
                return true;
            }
            if (flag2)
            {
                node.StateImageIndex = 2;
                return true;
            }
            node.StateImageIndex = 0;
            return true;
        }

        private void cListBoxKind_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
        }

        private void cListBoxKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((!this.mSelected && (this.cListBoxKind.SelectedIndex != -1)) && (this.cListBoxKind.Tag != null))
                {
                    for (int i = 0; i < this.cListBoxKind.Items.Count; i++)
                    {
                        if (i != this.cListBoxKind.SelectedIndex)
                        {
                            this.cListBoxKind.Items[i].CheckState = CheckState.Unchecked;
                        }
                    }
                    this.cListBoxKind2.Items.Clear();
                    this.cListBoxKind3.Items.Clear();
                    string str = "";
                    DataTable tag = this.cListBoxKind.Tag as DataTable;
                    str = tag.Rows[this.cListBoxKind.SelectedIndex]["code"].ToString().Substring(0, 2);
                    this.mEditKindCode = tag.Rows[this.cListBoxKind.SelectedIndex]["code"].ToString();
                    DataTable dataTable = null;
                    //this.GetDataTable(this.mDBAccess, "select * from T_Zaolin where ( code like '" + str + "%' and right(code ,4 )<>'0000' and right(code ,2 )='00')");
                    if (dataTable != null)
                    {
                        for (int j = 0; j < dataTable.Rows.Count; j++)
                        {
                            this.mSelected = true;
                            this.cListBoxKind2.Items.Add(dataTable.Rows[j]["name"].ToString());
                            this.mSelected = false;
                        }
                        if (this.cListBoxKind2.Items.Count > 0)
                        {
                            this.mSelected = true;
                            this.cListBoxKind2.SelectedIndex = 0;
                            this.cListBoxKind2.Items[0].CheckState = CheckState.Checked;
                            this.mSelected = false;
                        }
                        if (this.cListBoxKind2.Items.Count != 0)
                        {
                            this.cListBoxKind2.Tag = dataTable;
                            str = dataTable.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString().Substring(0, 4);
                            this.mEditKindCode = dataTable.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString();
                            DataTable table3 = null;// this.GetDataTable(this.mDBAccess, "select * from T_Zaolin where (code like '" + str + "%' and right(code ,2 )<>'00' and right(code,4)<>'0000')");
                            if (table3 != null)
                            {
                                for (int k = 0; k < table3.Rows.Count; k++)
                                {
                                    this.mSelected = true;
                                    this.cListBoxKind3.Items.Add(table3.Rows[k]["name"].ToString());
                                    this.mSelected = false;
                                }
                                if (this.cListBoxKind3.Items.Count > 0)
                                {
                                    this.mSelected = true;
                                    this.cListBoxKind3.SelectedIndex = 0;
                                    this.cListBoxKind3.Items[0].CheckState = CheckState.Checked;
                                    this.mSelected = false;
                                }
                                if (this.cListBoxKind3.Items.Count != 0)
                                {
                                    this.mEditKindCode = table3.Rows[this.cListBoxKind3.SelectedIndex]["code"].ToString();
                                    this.cListBoxKind3.Tag = table3;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void cListBoxKind2_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
        }

        private void cListBoxKind2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((!this.mSelected && (this.cListBoxKind2.SelectedIndex != -1)) && (this.cListBoxKind2.Tag != null))
                {
                    for (int i = 0; i < this.cListBoxKind2.Items.Count; i++)
                    {
                        if (i != this.cListBoxKind2.SelectedIndex)
                        {
                            this.cListBoxKind2.Items[i].CheckState = CheckState.Unchecked;
                        }
                    }
                    this.cListBoxKind3.Items.Clear();
                    string str = "";
                    DataTable tag = this.cListBoxKind2.Tag as DataTable;
                    str = tag.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString().Substring(0, 4);
                    this.mEditKindCode = tag.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString();
                    DataTable dataTable = null; //this.GetDataTable(this.mDBAccess, "select * from T_Zaolin where (code like '" + str + "%' and right(code ,2 )<>'00')");
                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        this.mSelected = true;
                        this.cListBoxKind3.Items.Add(dataTable.Rows[j]["name"].ToString());
                        this.mSelected = false;
                    }
                    if (this.cListBoxKind3.Items.Count > 0)
                    {
                        this.mSelected = true;
                        this.cListBoxKind3.SelectedIndex = 0;
                        this.cListBoxKind3.Items[0].CheckState = CheckState.Checked;
                        this.mSelected = false;
                    }
                    if (this.cListBoxKind3.Items.Count != 0)
                    {
                        this.mEditKindCode = dataTable.Rows[this.cListBoxKind3.SelectedIndex]["code"].ToString();
                        this.cListBoxKind3.Tag = dataTable;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void cListBoxKind3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.mSelected)
            {
                DataTable tag = this.cListBoxKind3.Tag as DataTable;
                this.mEditKindCode = tag.Rows[this.cListBoxKind3.SelectedIndex]["code"].ToString();
            }
        }

        private IFeatureWorkspace CopyDatabase(string sName)
        {
            try
            {
                string sSourceFile = UtilFactory.GetConfigOpt().RootPath + @"\Template\编辑任务.mdb";
                IWorkspace featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSAccessWorkspaceFactory) as IWorkspace;
                IWorkspaceName workspaceName = new WorkspaceNameClass {
                    WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory",
                    PathName = featureWorkspace.PathName
                };
                IWorkspaceName workspaceNameCopy = new WorkspaceNameClass {
                    WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory",
                    PathName = this.buttonEditSavePath.Text + @"\" + sName + @"\编辑任务.mdb"
                };
                IWorkspaceFactory factory = new AccessWorkspaceFactoryClass();
                IFeatureWorkspace workspace3 = null;
                FileInfo info = new FileInfo(workspaceNameCopy.PathName);
                string directoryName = info.DirectoryName;
                if (Directory.Exists(directoryName))
                {
                    if (info.Exists)
                    {
                        info.Delete();
                    }
                }
                else
                {
                    Directory.CreateDirectory(directoryName);
                }
                if (factory.Copy(workspaceName, this.buttonEditSavePath.Text + @"\" + this.listBoxName.Items[0].ToString(), out workspaceNameCopy))
                {
                    workspace3 = factory.OpenFromFile(workspaceNameCopy.PathName, 0) as IFeatureWorkspace;
                    info = new FileInfo(featureWorkspace.PathName);
                    info = new FileInfo(info.Directory + @"\编辑任务.mxd");
                    FileInfo info2 = new FileInfo(workspaceNameCopy.PathName);
                    info.CopyTo(info2.DirectoryName + @"\编辑任务.mxd", true);
                }
                return workspace3;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }



   

        public void Hook(object hook, string sEditKind)
        {
            try
            {
                if (hook != null)
                {
                    this.mHookHelper = new HookHelperClass();
                    this.mHookHelper.Hook = hook;
                    if (this.InitialValue())
                    {
                        this.InitialList();
                    }
                    this.InitialControl();
                }
                this.mEditKind = sEditKind;
            }
            catch (Exception)
            {
            }
        }

        private void InitialControl()
        {
            try
            {
                this.listBoxName.Items.Clear();
                this.cListBoxLayer.Items.Clear();
                string configValue = "";
                if (this.mEditKind == "小班")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLayer");
                }
                else if (this.mEditKind == "造林")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditZLLayer");
                }
                else if (this.mEditKind == "采伐")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditCFLayer");
                }
                else if (this.mEditKind == "林改")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLGLayer");
                }
                this.cListBoxLayer.Items.Add(configValue, true);
                string[] strArray = UtilFactory.GetConfigOpt().GetConfigValue("ConnectLayer").Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    this.cListBoxLayer.Items.Add(strArray[i], true);
                }

                string str2 = UtilFactory.GetConfigOpt().RootPath + @"\" + PathManager.FindValue("TaskPath");
                this.buttonEditSavePath.Text = str2;
                if (this.IsBatch)
                {
                    this.buttonEditSavePath.Enabled = false;
                    this.cListBoxLayer.Enabled = false;
                }
                else
                {
                    this.buttonEditSavePath.Enabled = true;
                    this.cListBoxLayer.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
        private static PathManager PathManager
        {
            get
            {
                return DBServiceFactory<PathManager>.Service;
            }
        }
        private void InitialFieldGrid()
        {
            try
            {
                this.mFieldTable = new DataTable();
                this.mFieldTable.Clear();
                DataColumn column = new DataColumn("目标数据字段", typeof(string));
                this.mFieldTable.Columns.Add(column);
                column = new DataColumn("源数据字段", typeof(string));
                this.mFieldTable.Columns.Add(column);
                this.gridControl1.DataSource = null;
                this.gridView1.Columns.Clear();
                this.gridControl1.DataSource = this.mFieldTable;
                this.gridView1.RefreshData();
            }
            catch (Exception)
            {
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTaskCreate));
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ImageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.simpleButtonReset = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonSelectAll = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.tList = new DevExpress.XtraTreeList.TreeList();
            this.tcolBase1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tcolBase2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.listBoxDist = new DevExpress.XtraEditors.ListBoxControl();
            this.panel13 = new System.Windows.Forms.Panel();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.panel14 = new System.Windows.Forms.Panel();
            this.simpleButtonFind = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cListBoxLayer = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.listBoxName = new DevExpress.XtraEditors.ListBoxControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelprogress = new System.Windows.Forms.Label();
            this.simpleButtonCreate = new DevExpress.XtraEditors.SimpleButton();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label5 = new System.Windows.Forms.Label();
            this.panelKind = new System.Windows.Forms.Panel();
            this.cListBoxKind3 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panel9 = new System.Windows.Forms.Panel();
            this.cListBoxKind2 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panel8 = new System.Windows.Forms.Panel();
            this.cListBoxKind = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.labelKind = new System.Windows.Forms.Label();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer3 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer4 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonEditSavePath = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.navBarGroupControlContainer5 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.panel10 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.simpleButtonClear = new DevExpress.XtraEditors.SimpleButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.listBoxDataList = new DevExpress.XtraEditors.ListBoxControl();
            this.panel12 = new System.Windows.Forms.Panel();
            this.simpleButtonAdd = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonRemove = new DevExpress.XtraEditors.SimpleButton();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonEditDataPath = new DevExpress.XtraEditors.ButtonEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup4 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup5 = new DevExpress.XtraNavBar.NavBarGroup();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tList)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDist)).BeginInit();
            this.panel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxName)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            this.panelKind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.navBarControl1.SuspendLayout();
            this.navBarGroupControlContainer1.SuspendLayout();
            this.navBarGroupControlContainer2.SuspendLayout();
            this.navBarGroupControlContainer3.SuspendLayout();
            this.navBarGroupControlContainer4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSavePath.Properties)).BeginInit();
            this.navBarGroupControlContainer5.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.panel11.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDataList)).BeginInit();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditDataPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "blank16.ico");
            this.ImageList1.Images.SetKeyName(1, "tick16.ico");
            this.ImageList1.Images.SetKeyName(2, "PART16.ICO");
            this.ImageList1.Images.SetKeyName(3, "");
            this.ImageList1.Images.SetKeyName(4, "");
            this.ImageList1.Images.SetKeyName(5, "");
            this.ImageList1.Images.SetKeyName(6, "");
            this.ImageList1.Images.SetKeyName(7, "");
            this.ImageList1.Images.SetKeyName(8, "");
            this.ImageList1.Images.SetKeyName(9, "");
            this.ImageList1.Images.SetKeyName(10, "");
            this.ImageList1.Images.SetKeyName(11, "");
            this.ImageList1.Images.SetKeyName(12, "");
            this.ImageList1.Images.SetKeyName(13, "");
            this.ImageList1.Images.SetKeyName(14, "");
            this.ImageList1.Images.SetKeyName(15, "");
            this.ImageList1.Images.SetKeyName(16, "(30,24).png");
            this.ImageList1.Images.SetKeyName(17, "(00,02).png");
            this.ImageList1.Images.SetKeyName(18, "(00,17).png");
            this.ImageList1.Images.SetKeyName(19, "(00,46).png");
            this.ImageList1.Images.SetKeyName(20, "(01,10).png");
            this.ImageList1.Images.SetKeyName(21, "(01,25).png");
            this.ImageList1.Images.SetKeyName(22, "(05,32).png");
            this.ImageList1.Images.SetKeyName(23, "(06,32).png");
            this.ImageList1.Images.SetKeyName(24, "(07,32).png");
            this.ImageList1.Images.SetKeyName(25, "(08,32).png");
            this.ImageList1.Images.SetKeyName(26, "(08,36).png");
            this.ImageList1.Images.SetKeyName(27, "(09,36).png");
            this.ImageList1.Images.SetKeyName(28, "(10,26).png");
            this.ImageList1.Images.SetKeyName(29, "(11,26).png");
            this.ImageList1.Images.SetKeyName(30, "(11,29).png");
            this.ImageList1.Images.SetKeyName(31, "(12,29).png");
            this.ImageList1.Images.SetKeyName(32, "(11,32).png");
            this.ImageList1.Images.SetKeyName(33, "(11,36).png");
            this.ImageList1.Images.SetKeyName(34, "(13,32).png");
            this.ImageList1.Images.SetKeyName(35, "(19,31).png");
            this.ImageList1.Images.SetKeyName(36, "(22,18).png");
            this.ImageList1.Images.SetKeyName(37, "(25,27).png");
            this.ImageList1.Images.SetKeyName(38, "(29,43).png");
            this.ImageList1.Images.SetKeyName(39, "(30,14).png");
            this.ImageList1.Images.SetKeyName(40, "5.png");
            this.ImageList1.Images.SetKeyName(41, "10.png");
            this.ImageList1.Images.SetKeyName(42, "11.png");
            this.ImageList1.Images.SetKeyName(43, "16.png");
            this.ImageList1.Images.SetKeyName(44, "17.png");
            this.ImageList1.Images.SetKeyName(45, "18.png");
            this.ImageList1.Images.SetKeyName(46, "19.png");
            this.ImageList1.Images.SetKeyName(47, "20.png");
            this.ImageList1.Images.SetKeyName(48, "21.png");
            this.ImageList1.Images.SetKeyName(49, "22.png");
            this.ImageList1.Images.SetKeyName(50, "25.png");
            this.ImageList1.Images.SetKeyName(51, "31.png");
            this.ImageList1.Images.SetKeyName(52, "41.png");
            this.ImageList1.Images.SetKeyName(53, "add.png");
            this.ImageList1.Images.SetKeyName(54, "bullet_minus.png");
            this.ImageList1.Images.SetKeyName(55, "control_add_blue.png");
            this.ImageList1.Images.SetKeyName(56, "control_power_blue.png");
            this.ImageList1.Images.SetKeyName(57, "control_remove_blue.png");
            this.ImageList1.Images.SetKeyName(58, "cross.png");
            this.ImageList1.Images.SetKeyName(59, "down.png");
            this.ImageList1.Images.SetKeyName(60, "draw_tools.png");
            this.ImageList1.Images.SetKeyName(61, "Feedicons_v2_010.png");
            this.ImageList1.Images.SetKeyName(62, "Feedicons_v2_011.png");
            this.ImageList1.Images.SetKeyName(63, "Feedicons_v2_031.png");
            this.ImageList1.Images.SetKeyName(64, "Feedicons_v2_032.png");
            this.ImageList1.Images.SetKeyName(65, "Feedicons_v2_033.png");
            this.ImageList1.Images.SetKeyName(66, "flag blue.png");
            this.ImageList1.Images.SetKeyName(67, "flag red.png");
            this.ImageList1.Images.SetKeyName(68, "flag yellow.png");
            this.ImageList1.Images.SetKeyName(69, "(44,23).png");
            this.ImageList1.Images.SetKeyName(70, "(12,29).png");
            this.ImageList1.Images.SetKeyName(71, "(34,00).png");
            this.ImageList1.Images.SetKeyName(72, "(03,02).png");
            this.ImageList1.Images.SetKeyName(73, "(49,06).png");
            this.ImageList1.Images.SetKeyName(74, "(09,13).png");
            this.ImageList1.Images.SetKeyName(75, "(16,47).png");
            this.ImageList1.Images.SetKeyName(76, "(13,47).png");
            this.ImageList1.Images.SetKeyName(77, "(18,01).png");
            this.ImageList1.Images.SetKeyName(78, "(18,13).png");
            this.ImageList1.Images.SetKeyName(79, "(19,01).png");
            this.ImageList1.Images.SetKeyName(80, "(28,40).png");
            this.ImageList1.Images.SetKeyName(81, "(39,47).png");
            this.ImageList1.Images.SetKeyName(82, "(45,12).png");
            this.ImageList1.Images.SetKeyName(83, "(45,17).png");
            this.ImageList1.Images.SetKeyName(84, "(45,41).png");
            // 
            // ImageList2
            // 
            this.ImageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList2.ImageStream")));
            this.ImageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList2.Images.SetKeyName(0, "");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.simpleButtonReset);
            this.panel2.Controls.Add(this.simpleButtonSelectAll);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 23);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(7, 2, 5, 2);
            this.panel2.Size = new System.Drawing.Size(325, 28);
            this.panel2.TabIndex = 12;
            this.panel2.Visible = false;
            // 
            // simpleButtonReset
            // 
            this.simpleButtonReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonReset.ImageIndex = 28;
            this.simpleButtonReset.ImageList = this.ImageList1;
            this.simpleButtonReset.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonReset.Location = new System.Drawing.Point(274, 2);
            this.simpleButtonReset.Name = "simpleButtonReset";
            this.simpleButtonReset.Size = new System.Drawing.Size(23, 24);
            this.simpleButtonReset.TabIndex = 10;
            this.simpleButtonReset.ToolTip = "重选";
            this.simpleButtonReset.Visible = false;
            // 
            // simpleButtonSelectAll
            // 
            this.simpleButtonSelectAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonSelectAll.ImageIndex = 5;
            this.simpleButtonSelectAll.ImageList = this.ImageList1;
            this.simpleButtonSelectAll.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonSelectAll.Location = new System.Drawing.Point(297, 2);
            this.simpleButtonSelectAll.Name = "simpleButtonSelectAll";
            this.simpleButtonSelectAll.Size = new System.Drawing.Size(23, 24);
            this.simpleButtonSelectAll.TabIndex = 9;
            this.simpleButtonSelectAll.ToolTip = "全选";
            this.simpleButtonSelectAll.Visible = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.ImageIndex = 13;
            this.label2.ImageList = this.ImageList1;
            this.label2.Location = new System.Drawing.Point(7, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "    设定范围:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(317, 206);
            this.xtraTabControl1.TabIndex = 13;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.tList);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(311, 177);
            this.xtraTabPage1.Text = "行政范围";
            // 
            // tList
            // 
            this.tList.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.Empty.Options.UseBackColor = true;
            this.tList.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tList.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tList.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.EvenRow.Options.UseBackColor = true;
            this.tList.Appearance.EvenRow.Options.UseForeColor = true;
            this.tList.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tList.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tList.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.tList.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tList.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tList.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tList.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tList.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tList.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tList.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tList.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.GroupButton.Options.UseBackColor = true;
            this.tList.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tList.Appearance.GroupButton.Options.UseForeColor = true;
            this.tList.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tList.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tList.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tList.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tList.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tList.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tList.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tList.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tList.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tList.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tList.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tList.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tList.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.HorzLine.Options.UseBackColor = true;
            this.tList.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.OddRow.Options.UseBackColor = true;
            this.tList.Appearance.OddRow.Options.UseForeColor = true;
            this.tList.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tList.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tList.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.Preview.Options.UseBackColor = true;
            this.tList.Appearance.Preview.Options.UseForeColor = true;
            this.tList.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.Row.Options.UseBackColor = true;
            this.tList.Appearance.Row.Options.UseForeColor = true;
            this.tList.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tList.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tList.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tList.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tList.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tList.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.TreeLine.Options.UseBackColor = true;
            this.tList.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tList.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.VertLine.Options.UseBackColor = true;
            this.tList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tcolBase1,
            this.tcolBase2});
            this.tList.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tList.Location = new System.Drawing.Point(0, 0);
            this.tList.LookAndFeel.SkinName = "Blue";
            this.tList.Name = "tList";
            this.tList.BeginUnboundLoad();
            this.tList.AppendNode(new object[] {
            "乡1",
            null}, -1, 2, 2, 1);
            this.tList.AppendNode(new object[] {
            "村1",
            null}, 0, -1, -1, 1);
            this.tList.AppendNode(new object[] {
            "村2",
            null}, 0, 0, 0, 1);
            this.tList.AppendNode(new object[] {
            "乡2",
            null}, -1, 0, 0, 0);
            this.tList.AppendNode(new object[] {
            "村1",
            null}, 3, 0, 0, 0);
            this.tList.AppendNode(new object[] {
            "乡3",
            null}, -1);
            this.tList.AppendNode(new object[] {
            "村1",
            null}, 5);
            this.tList.EndUnboundLoad();
            this.tList.OptionsBehavior.Editable = false;
            this.tList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tList.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.tList.OptionsSelection.InvertSelection = true;
            this.tList.OptionsView.ShowColumns = false;
            this.tList.OptionsView.ShowHorzLines = false;
            this.tList.OptionsView.ShowIndicator = false;
            this.tList.OptionsView.ShowVertLines = false;
            this.tList.Size = new System.Drawing.Size(311, 177);
            this.tList.StateImageList = this.ImageList1;
            this.tList.TabIndex = 76;
            this.tList.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tList.StateImageClick += new DevExpress.XtraTreeList.NodeClickEventHandler(this.tList_StateImageClick);
            this.tList.StateChanged += new System.EventHandler(this.tList_StateChanged);
            this.tList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tList_MouseUp);
            // 
            // tcolBase1
            // 
            this.tcolBase1.Caption = "名称";
            this.tcolBase1.FieldName = "设备号";
            this.tcolBase1.MinWidth = 100;
            this.tcolBase1.Name = "tcolBase1";
            this.tcolBase1.Visible = true;
            this.tcolBase1.VisibleIndex = 0;
            this.tcolBase1.Width = 100;
            // 
            // tcolBase2
            // 
            this.tcolBase2.Caption = "定位";
            this.tcolBase2.FieldName = "定位";
            this.tcolBase2.Name = "tcolBase2";
            this.tcolBase2.Visible = true;
            this.tcolBase2.VisibleIndex = 1;
            this.tcolBase2.Width = 20;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.listBoxDist);
            this.xtraTabPage2.Controls.Add(this.panel13);
            this.xtraTabPage2.Controls.Add(this.label8);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(311, 177);
            this.xtraTabPage2.Text = "指定范围";
            // 
            // listBoxDist
            // 
            this.listBoxDist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDist.Location = new System.Drawing.Point(0, 61);
            this.listBoxDist.MultiColumn = true;
            this.listBoxDist.Name = "listBoxDist";
            this.listBoxDist.Size = new System.Drawing.Size(311, 116);
            this.listBoxDist.TabIndex = 80;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.textEdit1);
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Controls.Add(this.simpleButtonFind);
            this.panel13.Controls.Add(this.simpleButton5);
            this.panel13.Controls.Add(this.simpleButton4);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 26);
            this.panel13.Name = "panel13";
            this.panel13.Padding = new System.Windows.Forms.Padding(5);
            this.panel13.Size = new System.Drawing.Size(311, 35);
            this.panel13.TabIndex = 79;
            // 
            // textEdit1
            // 
            this.textEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEdit1.Location = new System.Drawing.Point(5, 5);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(216, 20);
            this.textEdit1.TabIndex = 78;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel14.Location = new System.Drawing.Point(221, 5);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(7, 25);
            this.panel14.TabIndex = 83;
            // 
            // simpleButtonFind
            // 
            this.simpleButtonFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonFind.ImageIndex = 9;
            this.simpleButtonFind.ImageList = this.ImageList1;
            this.simpleButtonFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonFind.Location = new System.Drawing.Point(228, 5);
            this.simpleButtonFind.Name = "simpleButtonFind";
            this.simpleButtonFind.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonFind.TabIndex = 79;
            this.simpleButtonFind.ToolTip = "查找";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton5.ImageIndex = 72;
            this.simpleButton5.ImageList = this.ImageList1;
            this.simpleButton5.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButton5.Location = new System.Drawing.Point(254, 5);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(26, 25);
            this.simpleButton5.TabIndex = 82;
            this.simpleButton5.ToolTip = "图上选择";
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton4.ImageIndex = 74;
            this.simpleButton4.ImageList = this.ImageList1;
            this.simpleButton4.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButton4.Location = new System.Drawing.Point(280, 5);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(26, 25);
            this.simpleButton4.TabIndex = 81;
            this.simpleButton4.ToolTip = "图上定位";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.ImageList = this.ImageList1;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(311, 26);
            this.label8.TabIndex = 81;
            this.label8.Text = "区划名称:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 593);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(7, 2, 7, 7);
            this.panel3.Size = new System.Drawing.Size(325, 35);
            this.panel3.TabIndex = 14;
            this.panel3.Visible = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(7, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(311, 28);
            this.label4.TabIndex = 8;
            this.label4.Text = "设定图层:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cListBoxLayer
            // 
            this.cListBoxLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cListBoxLayer.Location = new System.Drawing.Point(5, 5);
            this.cListBoxLayer.MultiColumn = true;
            this.cListBoxLayer.Name = "cListBoxLayer";
            this.cListBoxLayer.Size = new System.Drawing.Size(313, 82);
            this.cListBoxLayer.TabIndex = 0;
            // 
            // listBoxName
            // 
            this.listBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxName.Location = new System.Drawing.Point(7, 25);
            this.listBoxName.MultiColumn = true;
            this.listBoxName.Name = "listBoxName";
            this.listBoxName.Size = new System.Drawing.Size(309, 121);
            this.listBoxName.TabIndex = 15;
            this.listBoxName.SelectedIndexChanged += new System.EventHandler(this.listBoxControl1_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.listBoxName);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(7, 2, 7, 7);
            this.panel4.Size = new System.Drawing.Size(323, 153);
            this.panel4.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(7, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(309, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "任务名称:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.progressBar);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 628);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(7, 7, 7, 9);
            this.panel5.Size = new System.Drawing.Size(325, 72);
            this.panel5.TabIndex = 17;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.labelprogress);
            this.panel6.Controls.Add(this.simpleButtonCreate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(7, 7);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(311, 28);
            this.panel6.TabIndex = 11;
            // 
            // labelprogress
            // 
            this.labelprogress.BackColor = System.Drawing.Color.Transparent;
            this.labelprogress.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelprogress.Location = new System.Drawing.Point(0, 0);
            this.labelprogress.Name = "labelprogress";
            this.labelprogress.Size = new System.Drawing.Size(167, 28);
            this.labelprogress.TabIndex = 8;
            this.labelprogress.Text = "生成进度:";
            this.labelprogress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelprogress.Visible = false;
            // 
            // simpleButtonCreate
            // 
            this.simpleButtonCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonCreate.Location = new System.Drawing.Point(240, 0);
            this.simpleButtonCreate.Name = "simpleButtonCreate";
            this.simpleButtonCreate.Size = new System.Drawing.Size(71, 28);
            this.simpleButtonCreate.TabIndex = 9;
            this.simpleButtonCreate.Text = "生成";
            this.simpleButtonCreate.Click += new System.EventHandler(this.simpleButtonCreate_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(7, 42);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(311, 21);
            this.progressBar.TabIndex = 10;
            this.progressBar.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.ImageIndex = 28;
            this.simpleButton1.ImageList = this.ImageList1;
            this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButton1.Location = new System.Drawing.Point(235, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(20, 20);
            this.simpleButton1.TabIndex = 10;
            this.simpleButton1.ToolTip = "重选";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton2.ImageIndex = 5;
            this.simpleButton2.ImageList = this.ImageList1;
            this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButton2.Location = new System.Drawing.Point(255, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(20, 20);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.ToolTip = "全选";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(6, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "设定范围:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelKind
            // 
            this.panelKind.BackColor = System.Drawing.Color.Transparent;
            this.panelKind.Controls.Add(this.cListBoxKind3);
            this.panelKind.Controls.Add(this.panel9);
            this.panelKind.Controls.Add(this.cListBoxKind2);
            this.panelKind.Controls.Add(this.panel8);
            this.panelKind.Controls.Add(this.cListBoxKind);
            this.panelKind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKind.Location = new System.Drawing.Point(0, 0);
            this.panelKind.Name = "panelKind";
            this.panelKind.Padding = new System.Windows.Forms.Padding(7, 2, 5, 2);
            this.panelKind.Size = new System.Drawing.Size(317, 150);
            this.panelKind.TabIndex = 18;
            // 
            // cListBoxKind3
            // 
            this.cListBoxKind3.CheckOnClick = true;
            this.cListBoxKind3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cListBoxKind3.Location = new System.Drawing.Point(223, 2);
            this.cListBoxKind3.Name = "cListBoxKind3";
            this.cListBoxKind3.Size = new System.Drawing.Size(89, 146);
            this.cListBoxKind3.TabIndex = 11;
            this.cListBoxKind3.SelectedIndexChanged += new System.EventHandler(this.cListBoxKind3_SelectedIndexChanged);
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(218, 2);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(5, 146);
            this.panel9.TabIndex = 13;
            // 
            // cListBoxKind2
            // 
            this.cListBoxKind2.CheckOnClick = true;
            this.cListBoxKind2.Dock = System.Windows.Forms.DockStyle.Left;
            this.cListBoxKind2.Location = new System.Drawing.Point(106, 2);
            this.cListBoxKind2.Name = "cListBoxKind2";
            this.cListBoxKind2.Size = new System.Drawing.Size(112, 146);
            this.cListBoxKind2.TabIndex = 10;
            this.cListBoxKind2.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cListBoxKind2_ItemCheck);
            this.cListBoxKind2.SelectedIndexChanged += new System.EventHandler(this.cListBoxKind2_SelectedIndexChanged);
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(101, 2);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(5, 146);
            this.panel8.TabIndex = 12;
            // 
            // cListBoxKind
            // 
            this.cListBoxKind.CheckOnClick = true;
            this.cListBoxKind.Dock = System.Windows.Forms.DockStyle.Left;
            this.cListBoxKind.Location = new System.Drawing.Point(7, 2);
            this.cListBoxKind.Name = "cListBoxKind";
            this.cListBoxKind.Size = new System.Drawing.Size(94, 146);
            this.cListBoxKind.TabIndex = 9;
            this.cListBoxKind.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cListBoxKind_ItemCheck);
            this.cListBoxKind.SelectedIndexChanged += new System.EventHandler(this.cListBoxKind_SelectedIndexChanged);
            // 
            // labelKind
            // 
            this.labelKind.BackColor = System.Drawing.Color.Transparent;
            this.labelKind.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelKind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelKind.ImageIndex = 13;
            this.labelKind.ImageList = this.ImageList1;
            this.labelKind.Location = new System.Drawing.Point(0, 0);
            this.labelKind.Name = "labelKind";
            this.labelKind.Size = new System.Drawing.Size(325, 23);
            this.labelKind.TabIndex = 19;
            this.labelKind.Text = "    设计类型:";
            this.labelKind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelKind.Visible = false;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup1;
            this.navBarControl1.ContentButtonHint = null;
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer2);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer3);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer4);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer5);
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1,
            this.navBarGroup2,
            this.navBarGroup3,
            this.navBarGroup4,
            this.navBarGroup5});
            this.navBarControl1.Location = new System.Drawing.Point(0, 51);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 279;
            this.navBarControl1.Size = new System.Drawing.Size(325, 542);
            this.navBarControl1.SmallImages = this.ImageList1;
            this.navBarControl1.StoreDefaultPaintStyleName = true;
            this.navBarControl1.TabIndex = 20;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "设计类型";
            this.navBarGroup1.ControlContainer = this.navBarGroupControlContainer1;
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupClientHeight = 154;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.SmallImageIndex = 28;
            // 
            // navBarGroupControlContainer1
            // 
            this.navBarGroupControlContainer1.Controls.Add(this.panelKind);
            this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
            this.navBarGroupControlContainer1.Size = new System.Drawing.Size(317, 150);
            this.navBarGroupControlContainer1.TabIndex = 0;
            // 
            // navBarGroupControlContainer2
            // 
            this.navBarGroupControlContainer2.Controls.Add(this.xtraTabControl1);
            this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
            this.navBarGroupControlContainer2.Size = new System.Drawing.Size(317, 206);
            this.navBarGroupControlContainer2.TabIndex = 1;
            // 
            // navBarGroupControlContainer3
            // 
            this.navBarGroupControlContainer3.Controls.Add(this.cListBoxLayer);
            this.navBarGroupControlContainer3.Name = "navBarGroupControlContainer3";
            this.navBarGroupControlContainer3.Padding = new System.Windows.Forms.Padding(5);
            this.navBarGroupControlContainer3.Size = new System.Drawing.Size(323, 92);
            this.navBarGroupControlContainer3.TabIndex = 2;
            // 
            // navBarGroupControlContainer4
            // 
            this.navBarGroupControlContainer4.Controls.Add(this.panel4);
            this.navBarGroupControlContainer4.Controls.Add(this.panel1);
            this.navBarGroupControlContainer4.Name = "navBarGroupControlContainer4";
            this.navBarGroupControlContainer4.Size = new System.Drawing.Size(323, 209);
            this.navBarGroupControlContainer4.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.buttonEditSavePath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 153);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(7, 2, 7, 9);
            this.panel1.Size = new System.Drawing.Size(323, 56);
            this.panel1.TabIndex = 12;
            // 
            // buttonEditSavePath
            // 
            this.buttonEditSavePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEditSavePath.Location = new System.Drawing.Point(7, 25);
            this.buttonEditSavePath.Name = "buttonEditSavePath";
            this.buttonEditSavePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditSavePath.Size = new System.Drawing.Size(309, 20);
            this.buttonEditSavePath.TabIndex = 10;
            this.buttonEditSavePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditSavePath_ButtonClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(7, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "存放路径:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // navBarGroupControlContainer5
            // 
            this.navBarGroupControlContainer5.Controls.Add(this.panel10);
            this.navBarGroupControlContainer5.Controls.Add(this.panel11);
            this.navBarGroupControlContainer5.Controls.Add(this.panel7);
            this.navBarGroupControlContainer5.Name = "navBarGroupControlContainer5";
            this.navBarGroupControlContainer5.Size = new System.Drawing.Size(323, 349);
            this.navBarGroupControlContainer5.TabIndex = 4;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Transparent;
            this.panel10.Controls.Add(this.gridControl1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 170);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(7, 2, 7, 9);
            this.panel10.Size = new System.Drawing.Size(323, 179);
            this.panel10.TabIndex = 14;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(7, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.gridControl1.Size = new System.Drawing.Size(309, 168);
            this.gridControl1.TabIndex = 9;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "目标属性字段";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "匹配源属性字段";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Transparent;
            this.panel11.Controls.Add(this.label7);
            this.panel11.Controls.Add(this.simpleButtonClear);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 140);
            this.panel11.Name = "panel11";
            this.panel11.Padding = new System.Windows.Forms.Padding(7, 0, 7, 5);
            this.panel11.Size = new System.Drawing.Size(323, 30);
            this.panel11.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(7, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 25);
            this.label7.TabIndex = 12;
            this.label7.Text = "属性字段匹配:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // simpleButtonClear
            // 
            this.simpleButtonClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonClear.ImageIndex = 56;
            this.simpleButtonClear.ImageList = this.ImageList1;
            this.simpleButtonClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonClear.Location = new System.Drawing.Point(290, 0);
            this.simpleButtonClear.Name = "simpleButtonClear";
            this.simpleButtonClear.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonClear.TabIndex = 11;
            this.simpleButtonClear.Click += new System.EventHandler(this.simpleButtonClear_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.Controls.Add(this.listBoxDataList);
            this.panel7.Controls.Add(this.panel12);
            this.panel7.Controls.Add(this.buttonEditDataPath);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(7, 2, 7, 7);
            this.panel7.Size = new System.Drawing.Size(323, 140);
            this.panel7.TabIndex = 13;
            // 
            // listBoxDataList
            // 
            this.listBoxDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDataList.Location = new System.Drawing.Point(7, 75);
            this.listBoxDataList.Name = "listBoxDataList";
            this.listBoxDataList.Size = new System.Drawing.Size(309, 58);
            this.listBoxDataList.TabIndex = 12;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Transparent;
            this.panel12.Controls.Add(this.simpleButtonAdd);
            this.panel12.Controls.Add(this.simpleButtonRemove);
            this.panel12.Controls.Add(this.label9);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(7, 45);
            this.panel12.Name = "panel12";
            this.panel12.Padding = new System.Windows.Forms.Padding(0, 2, 5, 2);
            this.panel12.Size = new System.Drawing.Size(309, 30);
            this.panel12.TabIndex = 13;
            // 
            // simpleButtonAdd
            // 
            this.simpleButtonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonAdd.ImageIndex = 55;
            this.simpleButtonAdd.ImageList = this.ImageList1;
            this.simpleButtonAdd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonAdd.Location = new System.Drawing.Point(252, 2);
            this.simpleButtonAdd.Name = "simpleButtonAdd";
            this.simpleButtonAdd.Size = new System.Drawing.Size(26, 26);
            this.simpleButtonAdd.TabIndex = 10;
            this.simpleButtonAdd.ToolTip = "添加";
            this.simpleButtonAdd.Click += new System.EventHandler(this.simpleButtonAdd_Click);
            // 
            // simpleButtonRemove
            // 
            this.simpleButtonRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonRemove.ImageIndex = 57;
            this.simpleButtonRemove.ImageList = this.ImageList1;
            this.simpleButtonRemove.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonRemove.Location = new System.Drawing.Point(278, 2);
            this.simpleButtonRemove.Name = "simpleButtonRemove";
            this.simpleButtonRemove.Size = new System.Drawing.Size(26, 26);
            this.simpleButtonRemove.TabIndex = 9;
            this.simpleButtonRemove.ToolTip = "移除";
            this.simpleButtonRemove.Click += new System.EventHandler(this.simpleButtonRemove_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.ImageList = this.ImageList1;
            this.label9.Location = new System.Drawing.Point(0, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 26);
            this.label9.TabIndex = 8;
            this.label9.Text = "导入数据列表:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonEditDataPath
            // 
            this.buttonEditDataPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonEditDataPath.Location = new System.Drawing.Point(7, 25);
            this.buttonEditDataPath.Name = "buttonEditDataPath";
            this.buttonEditDataPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditDataPath.Size = new System.Drawing.Size(309, 20);
            this.buttonEditDataPath.TabIndex = 10;
            this.buttonEditDataPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditDataPath_ButtonClick);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(7, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(309, 23);
            this.label6.TabIndex = 8;
            this.label6.Text = "导入数据路径:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "设计范围";
            this.navBarGroup2.ControlContainer = this.navBarGroupControlContainer2;
            this.navBarGroup2.Expanded = true;
            this.navBarGroup2.GroupClientHeight = 210;
            this.navBarGroup2.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup2.Name = "navBarGroup2";
            this.navBarGroup2.SmallImageIndex = 17;
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "设定图层";
            this.navBarGroup3.ControlContainer = this.navBarGroupControlContainer3;
            this.navBarGroup3.GroupClientHeight = 93;
            this.navBarGroup3.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup3.Name = "navBarGroup3";
            this.navBarGroup3.SmallImageIndex = 3;
            // 
            // navBarGroup4
            // 
            this.navBarGroup4.Caption = "任务名称";
            this.navBarGroup4.ControlContainer = this.navBarGroupControlContainer4;
            this.navBarGroup4.GroupClientHeight = 210;
            this.navBarGroup4.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup4.Name = "navBarGroup4";
            this.navBarGroup4.SmallImageIndex = 6;
            // 
            // navBarGroup5
            // 
            this.navBarGroup5.Caption = "导入数据";
            this.navBarGroup5.ControlContainer = this.navBarGroupControlContainer5;
            this.navBarGroup5.Expanded = true;
            this.navBarGroup5.GroupClientHeight = 350;
            this.navBarGroup5.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup5.Name = "navBarGroup5";
            this.navBarGroup5.SmallImageIndex = 4;
            this.navBarGroup5.Visible = false;
            // 
            // UserControlTaskCreate
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(207)))), ((int)(((byte)(247)))));
            this.Appearance.BackColor2 = System.Drawing.Color.White;
            this.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseBorderColor = true;
            this.Controls.Add(this.navBarControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.labelKind);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "UserControlTaskCreate";
            this.Size = new System.Drawing.Size(325, 700);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tList)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDist)).EndInit();
            this.panel13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxName)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            this.panelKind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.navBarControl1.ResumeLayout(false);
            this.navBarGroupControlContainer1.ResumeLayout(false);
            this.navBarGroupControlContainer2.ResumeLayout(false);
            this.navBarGroupControlContainer3.ResumeLayout(false);
            this.navBarGroupControlContainer4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSavePath.Properties)).EndInit();
            this.navBarGroupControlContainer5.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDataList)).EndInit();
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditDataPath.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private void InitialKindList()
        {
            try
            {
                if (this.mKindTable != null)
                {
                    this.cListBoxKind.Items.Clear();
                    this.cListBoxKind2.Items.Clear();
                    this.cListBoxKind3.Items.Clear();
                    DataTable dataTable = null;// this.GetDataTable(this.mDBAccess, "select * from T_Zaolin where ( code like '%0000')");
                    string str = "";
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        this.mSelected = true;
                        this.cListBoxKind.Items.Add(dataTable.Rows[i]["name"].ToString());
                        this.mSelected = false;
                    }
                    if (this.cListBoxKind.Items.Count > 0)
                    {
                        this.mSelected = true;
                        this.cListBoxKind.SelectedIndex = 0;
                        this.cListBoxKind.Items[0].CheckState = CheckState.Checked;
                        this.mSelected = false;
                    }
                    if (this.cListBoxKind.Items.Count != 0)
                    {
                        this.cListBoxKind.Tag = dataTable;
                        str = dataTable.Rows[this.cListBoxKind.SelectedIndex]["code"].ToString().Substring(0, 2);
                        DataTable table2 = null;// this.GetDataTable(this.mDBAccess, "select * from T_Zaolin where ( code like '" + str + "%' and right(code ,4 )<>'0000' and right(code ,2 )='00')");
                        if (table2 != null)
                        {
                            for (int j = 0; j < table2.Rows.Count; j++)
                            {
                                this.mSelected = true;
                                this.cListBoxKind2.Items.Add(table2.Rows[j]["name"].ToString());
                                this.mSelected = false;
                            }
                            if (this.cListBoxKind2.Items.Count > 0)
                            {
                                this.mSelected = true;
                                this.cListBoxKind2.SelectedIndex = 0;
                                this.cListBoxKind2.Items[0].CheckState = CheckState.Checked;
                                this.mSelected = false;
                            }
                            if (this.cListBoxKind2.Items.Count != 0)
                            {
                                this.cListBoxKind2.Tag = table2;
                                DataTable table3 = null;// this.GetDataTable(this.mDBAccess, "select * from T_Zaolin where (code like '" + table2.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString().Substring(0, 4) + "%' and right(code ,4 )<>'0000') and right(code ,2 )<>'00')");
                                if (table3 != null)
                                {
                                    for (int k = 0; k < table3.Rows.Count; k++)
                                    {
                                        this.mSelected = true;
                                        this.cListBoxKind3.Items.Add(table2.Rows[k]["name"].ToString());
                                        this.mSelected = false;
                                    }
                                    if (this.cListBoxKind3.Items.Count > 0)
                                    {
                                        this.mSelected = true;
                                        this.cListBoxKind3.SelectedIndex = 0;
                                        this.cListBoxKind3.Items[0].CheckState = CheckState.Checked;
                                        this.mSelected = false;
                                    }
                                    this.cListBoxKind3.Tag = table3;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate", "InitialKindList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitialList()
        {
            try
            {
                TreeListNode node = null;
                TreeListNode parentNode = null;
                TreeListNode node3 = null;
                TreeListNode node4 = null;
                this.tList.ClearNodes();
                this.tList.OptionsView.ShowRoot = true;
                this.tList.SelectImageList = null;
                this.tList.StateImageList = this.ImageList1;
                this.tList.OptionsView.ShowButtons = true;
                this.tList.TreeLineStyle = LineStyle.None;
                this.tList.RowHeight = 20;
                this.tList.OptionsBehavior.AutoPopulateColumns = true;
                this.m_CountyLayer.FeatureClass.FeatureCount(null);
                IFeatureCursor cursor = this.m_CountyLayer.FeatureClass.Search(null, false);
                IFeature feature = cursor.NextFeature();
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyFieldCode");
                int index = feature.Fields.FindField(configValue);
                string str2 = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode");
                while (feature != null)
                {
                    IQueryFilter queryFilter = new QueryFilterClass {
                        WhereClause = str2 + "='" + feature.get_Value(index).ToString() + "'"
                    };
                    ICursor cursor2 = this.m_CountyTable.Search(queryFilter, false);
                    ESRI.ArcGIS.Geodatabase.IRow row = cursor2.NextRow();
                    int num2 = row.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode"));
                    int num3 = row.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName"));
                    while (row != null)
                    {
                        if (row.get_Value(num2).ToString() == feature.get_Value(index).ToString())
                        {
                            node3 = this.tList.AppendNode(row.get_Value(num3).ToString(), node4);
                            node3.ImageIndex = -1;
                            node3.StateImageIndex = 0;
                            node3.SelectImageIndex = -1;
                            node3.SetValue(0, row.get_Value(num3).ToString());
                            node3.Tag = row.get_Value(num2).ToString();
                            IQueryFilter filter2 = new QueryFilterClass {
                                WhereClause = str2 + " like '" + row.get_Value(num2).ToString() + "*'"
                            };
                            ICursor cursor3 = this.m_TownTable.Search(filter2, false);
                            for (ESRI.ArcGIS.Geodatabase.IRow row2 = cursor3.NextRow(); row2 != null; row2 = cursor3.NextRow())
                            {
                                parentNode = this.tList.AppendNode(row2.get_Value(num3).ToString(), node3);
                                parentNode.ImageIndex = -1;
                                parentNode.StateImageIndex = 0;
                                parentNode.SelectImageIndex = -1;
                                parentNode.SetValue(0, row2.get_Value(num3).ToString());
                                parentNode.Tag = row2.get_Value(num2).ToString();
                                parentNode.Expanded = false;
                                if (!this.mIsBatch)
                                {
                                    IQueryFilter filter3 = new QueryFilterClass {
                                        WhereClause = str2 + " like '" + row2.get_Value(num2).ToString() + "*'"
                                    };
                                    ICursor cursor4 = this.m_VillageTable.Search(filter3, false);
                                    for (ESRI.ArcGIS.Geodatabase.IRow row3 = cursor3.NextRow(); row3 != null; row3 = cursor4.NextRow())
                                    {
                                        node = this.tList.AppendNode(row3.get_Value(num3).ToString(), parentNode);
                                        node.ImageIndex = -1;
                                        node.StateImageIndex = 0;
                                        node.SelectImageIndex = -1;
                                        node.SetValue(0, row3.get_Value(num3).ToString());
                                        node.Tag = row3.get_Value(num2).ToString();
                                        node.Expanded = false;
                                    }
                                }
                            }
                        }
                        row = cursor2.NextRow();
                    }
                    feature = cursor.NextFeature();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate", "InitialList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private bool InitialValue()
        {
            try
            {

                this.mKindTable = null;// this.GetDataTable(this.mDBAccess, "select * from T_Zaolin");
                this.InitialKindList();
                IMap focusMap = this.mHookHelper.FocusMap;
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyLayerName");
                this.m_CountyLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                if (this.m_CountyLayer == null)
                {
                    return false;
                }
                configValue = UtilFactory.GetConfigOpt().GetConfigValue("TownLayerName");
                this.m_TownLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                if (this.m_TownLayer == null)
                {
                    return false;
                }
                configValue = UtilFactory.GetConfigOpt().GetConfigValue("VillageLayerName");
                this.m_VillageLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                if (this.m_VillageLayer == null)
                {
                    return false;
                }
                string sSourceFile = UtilFactory.GetConfigOpt().RootPath + @"\" + PathManager.FindValue("EditDataPath");
                IFeatureWorkspace featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSAccessWorkspaceFactory);
                if (featureWorkspace == null)
                {
                    return false;
                }
                string name = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableName");
                this.m_CountyTable = featureWorkspace.OpenTable(name);
                if (this.m_CountyTable == null)
                {
                    return false;
                }
                name = UtilFactory.GetConfigOpt().GetConfigValue("TownCodeTableName");
                this.m_TownTable = featureWorkspace.OpenTable(name);
                if (this.m_TownTable == null)
                {
                    return false;
                }
                name = UtilFactory.GetConfigOpt().GetConfigValue("VillageCodeTableName");
                this.m_VillageTable = featureWorkspace.OpenTable(name);
                if (this.m_VillageTable == null)
                {
                    return false;
                }
                if (this.mEditKind == "小班")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLayer");
                }
                else if (this.mEditKind == "造林")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditZLLayer");
                }
                else if (this.mEditKind == "采伐")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditCFLayer");
                }
                else if (this.mEditKind == "林改")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLGLayer");
                }
                this.m_EditLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                this.mRangeList = new ArrayList();
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private void listBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LoadDataFromMap(string sLayerName, IGeometry pRangeGeo, IFeatureLayer pSourceLayer, IFeatureWorkspace pFeatureWorkspace)
        {
            try
            {
                IWorkspaceEdit edit = pFeatureWorkspace as IWorkspaceEdit;
                if (edit != null)
                {
                    edit.StartEditing(false);
                    edit.StartEditOperation();
                    IWorkspace workspace = pFeatureWorkspace as IWorkspace;
                    IDataset dataset = workspace as IDataset;
                    IEnumDataset subsets = dataset.Subsets;
                    IDataset dataset3 = subsets.Next();
                    IFeatureClass class2 = null;
                    IFeatureLayer layer = null;
                    while (dataset3 != null)
                    {
                        class2 = dataset3 as IFeatureClass;
                        layer = new FeatureLayerClass {
                            FeatureClass = class2
                        };
                        if (layer.FeatureClass.AliasName == sLayerName)
                        {
                            ISpatialFilter filter = new SpatialFilterClass {
                                Geometry = pRangeGeo,
                                GeometryField = pSourceLayer.FeatureClass.ShapeFieldName,
                                SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
                            };
                            GC.Collect();
                            IFeatureCursor cursor = pSourceLayer.FeatureClass.Search(filter, false);
                            for (IFeature feature = cursor.NextFeature(); feature != null; feature = cursor.NextFeature())
                            {
                                IFeature feature2 = class2.CreateFeature();
                                feature2.Shape = feature.Shape;
                                for (int i = 0; i < feature2.Fields.FieldCount; i++)
                                {
                                    if (((feature2.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeOID) && (feature2.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)) && ((class2.LengthField.Name != feature2.Fields.get_Field(i).Name) && (class2.AreaField.Name != feature2.Fields.get_Field(i).Name)))
                                    {
                                        feature2.set_Value(i, feature.get_Value(feature.Fields.FindField(feature2.Fields.get_Field(i).Name)));
                                    }
                                }
                                feature2.Store();
                            }
                            break;
                        }
                        dataset3 = subsets.Next();
                    }
                    try
                    {
                        edit.StopEditOperation();
                    }
                    catch (Exception)
                    {
                        edit.StopEditOperation();
                    }
                    edit.StopEditing(true);
                }
            }
            catch (Exception)
            {
            }
        }

        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            this.listBoxDataList.Items.Add(this.buttonEditDataPath.Text);
            this.buttonEditDataPath.Text = "";
            this.InitialFieldGrid();
        }

        private void simpleButtonClear_Click(object sender, EventArgs e)
        {
        }

        private void simpleButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.listBoxName.Items.Count; i++)
                {
                    IFeature feature = this.mRangeList[i] as IFeature;
                    IFeatureWorkspace pFeatureWorkspace = this.CopyDatabase(this.listBoxName.Items[i].ToString());
                    string name = "T_EditInfo";
                    ITable table = pFeatureWorkspace.OpenTable(name);
                    table.GetRow(1).set_Value(2, this.mEditKindCode);
                    table.Update(null, false);
                    for (int j = 0; j < this.cListBoxLayer.Items.Count; j++)
                    {
                        string sLayerName = this.cListBoxLayer.Items[j].ToString();
                        IFeatureLayer pSourceLayer = GISFunFactory.LayerFun.FindFeatureLayer(this.mHookHelper.FocusMap as IBasicMap, sLayerName, true);
                        if (pSourceLayer != null)
                        {
                            this.LoadDataFromMap(sLayerName, feature.Shape, pSourceLayer, pFeatureWorkspace);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void simpleButtonRemove_Click(object sender, EventArgs e)
        {
            this.listBoxDataList.Items.Remove(this.listBoxDataList.SelectedIndex);
        }

        private void tList_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void tList_StateChanged(object sender, EventArgs e)
        {
        }

        private void tList_StateImageClick(object sender, NodeClickEventArgs e)
        {
            try
            {
                if (e.Node.StateImageIndex == 0)
                {
                    e.Node.StateImageIndex = 1;
                }
                else
                {
                    e.Node.StateImageIndex = 0;
                }
                if (e.Node.Nodes.Count > 0)
                {
                    for (int i = 0; i < e.Node.Nodes.Count; i++)
                    {
                        string configValue = UtilFactory.GetConfigOpt().GetConfigValue("TownFieldCode");
                        IQueryFilter queryFilter = new QueryFilterClass {
                            WhereClause = configValue + "='" + e.Node.Nodes[i].Tag.ToString() + "'"
                        };
                        IFeature feature = this.m_TownLayer.Search(queryFilter, false).NextFeature();
                        e.Node.Nodes[i].StateImageIndex = e.Node.StateImageIndex;
                        if (e.Node.Nodes[i].StateImageIndex == 1)
                        {
                            try
                            {
                                this.listBoxName.Items.Add(e.Node.Nodes[i].GetValue(0));
                                if (feature != null)
                                {
                                    this.mRangeList.Add(feature);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            this.listBoxName.Items.Remove(e.Node.Nodes[i].GetValue(0));
                            if (feature != null)
                            {
                                this.mRangeList.Remove(feature);
                            }
                        }
                    }
                }
                else
                {
                    string str2 = UtilFactory.GetConfigOpt().GetConfigValue("TownFieldCode");
                    IQueryFilter filter2 = new QueryFilterClass {
                        WhereClause = str2 + "='" + e.Node.Tag.ToString() + "'"
                    };
                    IFeature feature2 = this.m_TownLayer.Search(filter2, false).NextFeature();
                    if (e.Node.StateImageIndex == 1)
                    {
                        this.listBoxName.Items.Add(e.Node.GetValue(0));
                        this.CheckSelect(e.Node.ParentNode);
                        this.mRangeList.Add(feature2);
                    }
                    else
                    {
                        this.listBoxName.Items.Remove(e.Node.GetValue(0));
                        this.CheckSelect(e.Node.ParentNode);
                        this.mRangeList.Remove(feature2);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public bool IsBatch
        {
            get
            {
                return this.mIsBatch;
            }
            set
            {
                this.mIsBatch = value;
                this.InitialControl();
            }
        }
    }
}

