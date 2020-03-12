namespace TaskManage
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraTab;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using FormBase;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using Utilities;
    using DevExpress.XtraGrid.Views.Base;
    using td.db.service.sys;
    using td.db.orm;
    using td.db.mid.sys;
    using FunFactory;

    public class UserControlTaskInfo : UserControlBase1
    {
        private CheckedListBoxControl checkedListBoxControl1;
        private ColumnHeader columnHeader1;
        private ComboBoxEdit comboBoxEditField;
        private ComboBoxEdit comboBoxEditValue;
        private IContainer components;
        private GridColumn gridColumn3;
        private GridControl gridControl1;
        private GridView gridView1;
        public ImageListBoxControl ilistModaltable;
        internal ImageList ImageList1;
        private ImageList imageList2;
        private Label label1;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private ListView listViewModaltable;
        private ITable m_CountyTable;
        private ITable m_Table;
        private ITable m_Table2;
        private ITable m_Table3;
        private ITable m_TownTable;
        private ITable m_VillageTable;
        private const string mClassName = "TaskManage.UserControlTaskInfo";
       
        private string mEditKind = "";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private IFeatureLayer mFeatureLayer;
        private IFeatureWorkspace mfWorkspace;
        private HookHelper mHookHelper;
        private ITable mRelationTable;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private DataTable mTable;
        private int objectid = -1;
        private Panel panel;
        private Panel panel1;
        private Panel panel10;
        private Panel panel11;
        private Panel panel12;
        private Panel panel13;
        private Panel panel15;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private PanelControl panelControl1;
        private PanelControl panelControl2;
        private PanelControl panelControl4;
        private PanelControl panelControl5;
        private Panel panelDesignTable;
        private Panel panelModalTable;
        private Panel panelSet;
        internal PopupContainerControl PopupContainer;
        internal PopupContainerEdit PopupContainerEdit1;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private SimpleButton simpleButtonDesignTable;
        private SimpleButton simpleButtonFind;
        private SimpleButton simpleButtonRead;
        private SimpleButton simpleButtonReset;
        private SimpleButton simpleButtonView;
        private SimpleButton simpleButtonViewExcel;
        private TreeView treeView1;
        private Label txtTaskName;
        private XtraTabControl xtraTabControl1;
        private XtraTabPage xtraTabPage1;
        private XtraTabPage xtraTabPage2;

        public event DesignTableClickhandler OnDesignTableClick;

        public event ModalClickHandler OnModalClick;

        public event ViewExcelClickHandler OnViewExcelClick;

        public UserControlTaskInfo()
        {
            this.InitializeComponent();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Checked)
            {
                this.gridView1.Columns[e.Index].Visible = true;
            }
            else
            {
                this.gridView1.Columns[e.Index].Visible = false;
            }
        }

        private bool CheckFieldVisiable(string sname, string skeyvalue, bool flag)
        {
            try
            {
                if (skeyvalue == "")
                {
                    skeyvalue = "XiaobanFieldString2";
                }
                string[] strArray = UtilFactory.GetConfigOpt().GetConfigValue(skeyvalue).Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].ToLower() == sname.ToLower())
                    {
                        return flag;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskInfo", "CheckFieldVisiable", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
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

        private string GetDistName(string distcode)
        {
            try
            {
                string configValue = "CCODE";
                string name = "CNAME";
                IQueryFilter queryFilter = new QueryFilterClass();
                ITable countyTable = null;
                if (distcode.Length == 6)
                {
                    countyTable = this.m_CountyTable;
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode");
                    name = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName");
                }
                else if (distcode.Length == 9)
                {
                    countyTable = this.m_TownTable;
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("TownCodeTableFieldCode");
                    name = UtilFactory.GetConfigOpt().GetConfigValue("TownCodeTableFieldName");
                }
                else if (distcode.Length == 12)
                {
                    countyTable = this.m_VillageTable;
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("VillageCodeTableFieldCode");
                    name = UtilFactory.GetConfigOpt().GetConfigValue("VillageCodeTableFieldName");
                }
                if (countyTable != null)
                {
                    queryFilter.WhereClause = configValue + "='" + distcode + "'";
                    IRow row = countyTable.Search(queryFilter, false).NextRow();
                    int index = row.Fields.FindField(name);
                    if (index != -1)
                    {
                        return row.get_Value(index).ToString();
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string GetTaskState(TaskState2 state)
        {
            switch (state)
            {
                case TaskState2.Create:
                    return "创建";

                case TaskState2.Edit:
                    return "编辑";

                case TaskState2.Check:
                    return "通过校验";

                case TaskState2.Submit:
                    return "已提交送审";

                case TaskState2.Unapprove:
                    return "批复修改";

                case TaskState2.Pass:
                    return "已提审批通过";
            }
            return "";
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = this.gridView1.GetSelectedRows()[0];
                DataRow dataRow = this.gridView1.GetDataRow(rowHandle);
                if (!(dataRow[this.objectid] is IFeature))
                {
                    IQueryFilter filter = new QueryFilterClass();
                    IRelationshipClass mRelationTable = (this.mRelationTable as IObjectClass) as IRelationshipClass;
                    if (mRelationTable != null)
                    {
                        filter.WhereClause = string.Concat(new object[] { mRelationTable.DestinationPrimaryKey, "='", this.mTable.Rows[rowHandle][mRelationTable.DestinationPrimaryKey], "'" });
                    }
                    else
                    {
                        filter.WhereClause = string.Concat(new object[] { "Task_ID='", EditTask.TaskID, "' and ", this.mFeatureLayer.FeatureClass.OIDFieldName, "=", dataRow[this.objectid] });
                    }
                    dataRow[this.objectid] = this.mFeatureLayer.FeatureClass.Search(filter, false).NextFeature();
                }
                IFeature feature = dataRow[this.objectid] as IFeature;
                if (feature != null)
                {
                    this.mHookHelper.FocusMap.ClearSelection();
                    this.mHookHelper.FocusMap.SelectFeature(this.mFeatureLayer, feature);
                    GISFunFactory.FeatureFun.ZoomToFeature(this.mHookHelper.FocusMap, feature);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskInfo", "gridView1_DoubleClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void InitialControl(object hook, IFeatureLayer pFeatureLayer)
        {
            try
            {
                this.mHookHelper = new HookHelperClass();
                if (hook != null)
                {
                    this.mHookHelper.Hook = hook;
                }
                this.mFeatureLayer = pFeatureLayer;
                if (EditTask.KindCode.Substring(0, 2) == "01")
                {
                    this.mEditKind = "造林";
                }
                else if (EditTask.KindCode.Substring(0, 2) == "02")
                {
                    this.mEditKind = "采伐";
                    this.panelDesignTable.Visible = true;
                }
                else if (EditTask.KindCode.Substring(0, 2) == "04")
                {
                    this.mEditKind = "征占用";
                    this.panelDesignTable.Visible = false;
                    this.panelControl2.Visible = false;
                }
               
                this.mfWorkspace = EditTask.EditWorkspace;
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableName");
                this.m_CountyTable = this.mfWorkspace.OpenTable(configValue);
                if (this.m_CountyTable != null)
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("TownCodeTableName");
                    this.m_TownTable = this.mfWorkspace.OpenTable(configValue);
                    if (this.m_TownTable != null)
                    {
                        configValue = UtilFactory.GetConfigOpt().GetConfigValue("VillageCodeTableName");
                        this.m_VillageTable = this.mfWorkspace.OpenTable(configValue);
                        this.InitialTaskInfo();
                        this.InitialTaskInfo2(true);
                        this.simpleButtonRead.Visible = false;
                        this.PopupContainerEdit1.Visible = false;
                        if (EditTask.KindCode.Substring(0, 2) == "01")
                        {
                            this.panelDesignTable.Visible = false;
                            this.panelModalTable.Visible = true;
                        }
                        else if (EditTask.KindCode.Substring(0, 2) == "02")
                        {
                            this.panelDesignTable.Visible = false;
                            this.panelModalTable.Visible = false;
                            this.panel.Visible = false;
                        }
                        else if (EditTask.KindCode.Substring(0, 2) == "04")
                        {
                            this.panelDesignTable.Visible = false;
                            this.panelModalTable.Visible = false;
                            this.panel.Visible = false;
                        }
                        this.panelSet.Height = 0x16;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskInfo", "InitialControl", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("行政区划：林翔区");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("设计类型：主伐—皆伐");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("设计状态：编辑");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("设计年度：2016");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("创建时间：2016-09-01");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("小班总数：0");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("小班面积：0");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("逻辑校验：未通过");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("图形校验：未通过");
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("1[核桃]", 7);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("2", 17);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("16", 22);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("15", 23);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("4");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("5");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("6");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("7");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("8");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTaskInfo));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label10 = new System.Windows.Forms.Label();
            this.panelModalTable = new System.Windows.Forms.Panel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.listViewModaltable = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.ilistModaltable = new DevExpress.XtraEditors.ImageListBoxControl();
            this.simpleButtonView = new DevExpress.XtraEditors.SimpleButton();
            this.panelDesignTable = new System.Windows.Forms.Panel();
            this.simpleButtonDesignTable = new DevExpress.XtraEditors.SimpleButton();
            this.label7 = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.simpleButtonViewExcel = new DevExpress.XtraEditors.SimpleButton();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtTaskName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonReset = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonFind = new DevExpress.XtraEditors.SimpleButton();
            this.panel10 = new System.Windows.Forms.Panel();
            this.comboBoxEditValue = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxEditField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.PopupContainerEdit1 = new DevExpress.XtraEditors.PopupContainerEdit();
            this.PopupContainer = new DevExpress.XtraEditors.PopupContainerControl();
            this.checkedListBoxControl1 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.simpleButtonRead = new DevExpress.XtraEditors.SimpleButton();
            this.panelSet = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel13.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            this.panelModalTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ilistModaltable)).BeginInit();
            this.panelDesignTable.SuspendLayout();
            this.panel.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainer)).BeginInit();
            this.PopupContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.panelSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Transparent;
            this.panel13.Controls.Add(this.panel7);
            this.panel13.Controls.Add(this.panelModalTable);
            this.panel13.Controls.Add(this.panelDesignTable);
            this.panel13.Controls.Add(this.panel);
            this.panel13.Controls.Add(this.panel11);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Name = "panel13";
            this.panel13.Padding = new System.Windows.Forms.Padding(1);
            this.panel13.Size = new System.Drawing.Size(1008, 146);
            this.panel13.TabIndex = 85;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel8);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(211, 1);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.panel7.Size = new System.Drawing.Size(600, 144);
            this.panel7.TabIndex = 97;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panelControl5);
            this.panel8.Controls.Add(this.label10);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(593, 144);
            this.panel8.TabIndex = 90;
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.treeView1);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(0, 26);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(593, 118);
            this.panelControl5.TabIndex = 89;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ItemHeight = 16;
            this.treeView1.Location = new System.Drawing.Point(2, 2);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "节点0";
            treeNode1.Text = "行政区划：林翔区";
            treeNode2.Name = "节点8";
            treeNode2.Text = "设计类型：主伐—皆伐";
            treeNode3.Name = "节点1";
            treeNode3.Text = "设计状态：编辑";
            treeNode4.Name = "节点2";
            treeNode4.Text = "设计年度：2016";
            treeNode5.Name = "节点3";
            treeNode5.Text = "创建时间：2016-09-01";
            treeNode6.Name = "节点6";
            treeNode6.Text = "小班总数：0";
            treeNode7.Name = "节点7";
            treeNode7.Text = "小班面积：0";
            treeNode8.Name = "节点4";
            treeNode8.Text = "逻辑校验：未通过";
            treeNode9.Name = "节点5";
            treeNode9.Text = "图形校验：未通过";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(589, 114);
            this.treeView1.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(593, 26);
            this.label10.TabIndex = 88;
            this.label10.Text = "主要设计信息:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelModalTable
            // 
            this.panelModalTable.Controls.Add(this.panelControl2);
            this.panelModalTable.Controls.Add(this.label1);
            this.panelModalTable.Controls.Add(this.ilistModaltable);
            this.panelModalTable.Controls.Add(this.simpleButtonView);
            this.panelModalTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModalTable.Location = new System.Drawing.Point(211, 1);
            this.panelModalTable.Name = "panelModalTable";
            this.panelModalTable.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.panelModalTable.Size = new System.Drawing.Size(600, 144);
            this.panelModalTable.TabIndex = 93;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.listViewModaltable);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 26);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(598, 118);
            this.panelControl2.TabIndex = 91;
            // 
            // listViewModaltable
            // 
            this.listViewModaltable.BackColor = System.Drawing.Color.White;
            this.listViewModaltable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewModaltable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewModaltable.FullRowSelect = true;
            this.listViewModaltable.GridLines = true;
            this.listViewModaltable.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            this.listViewModaltable.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9});
            this.listViewModaltable.LargeImageList = this.ImageList1;
            this.listViewModaltable.Location = new System.Drawing.Point(2, 2);
            this.listViewModaltable.MultiSelect = false;
            this.listViewModaltable.Name = "listViewModaltable";
            this.listViewModaltable.Size = new System.Drawing.Size(594, 114);
            this.listViewModaltable.SmallImageList = this.imageList2;
            this.listViewModaltable.TabIndex = 90;
            this.listViewModaltable.UseCompatibleStateImageBehavior = false;
            this.listViewModaltable.Click += new System.EventHandler(this.listViewModaltable_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 30;
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
            this.ImageList1.Images.SetKeyName(85, "arrow_refresh_small.png");
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "allpanel.ico");
            this.imageList2.Images.SetKeyName(1, "blue_gallerylink_24x24.gif");
            this.imageList2.Images.SetKeyName(2, "blue_view_24x24.gif");
            this.imageList2.Images.SetKeyName(3, "emblem-web_24x24.png");
            this.imageList2.Images.SetKeyName(4, "Windows.png");
            this.imageList2.Images.SetKeyName(5, "Red%20tag.png");
            this.imageList2.Images.SetKeyName(6, "ksirtet24.png");
            this.imageList2.Images.SetKeyName(7, "tag_purple_edit.png");
            this.imageList2.Images.SetKeyName(8, "tag_yellow_edit.png");
            this.imageList2.Images.SetKeyName(9, "tags_edit.png");
            this.imageList2.Images.SetKeyName(10, "page_white_world.png");
            this.imageList2.Images.SetKeyName(11, "map3-24.png");
            this.imageList2.Images.SetKeyName(12, "file.png");
            this.imageList2.Images.SetKeyName(13, "Documents.png");
            this.imageList2.Images.SetKeyName(14, "onebit_20.png");
            this.imageList2.Images.SetKeyName(15, "onebit_39.png");
            this.imageList2.Images.SetKeyName(16, "Web.png");
            this.imageList2.Images.SetKeyName(17, "onebit_20.png");
            this.imageList2.Images.SetKeyName(18, "books_23.png");
            this.imageList2.Images.SetKeyName(19, "books_25.png");
            this.imageList2.Images.SetKeyName(20, "books_26.png");
            this.imageList2.Images.SetKeyName(21, "books_27.png");
            this.imageList2.Images.SetKeyName(22, "books_28.png");
            this.imageList2.Images.SetKeyName(23, "books_29.png");
            this.imageList2.Images.SetKeyName(24, "application_view_columns.png");
            this.imageList2.Images.SetKeyName(25, "List.png");
            this.imageList2.Images.SetKeyName(26, "page.png");
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(598, 26);
            this.label1.TabIndex = 88;
            this.label1.Text = "典型设计模式表:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ilistModaltable
            // 
            this.ilistModaltable.Appearance.BackColor = System.Drawing.Color.White;
            this.ilistModaltable.Appearance.Options.UseBackColor = true;
            this.ilistModaltable.ImageList = this.ImageList1;
            this.ilistModaltable.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("模式表1", 7),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("模式表2", 7)});
            this.ilistModaltable.Location = new System.Drawing.Point(7, 124);
            this.ilistModaltable.Name = "ilistModaltable";
            this.ilistModaltable.Size = new System.Drawing.Size(61, 61);
            this.ilistModaltable.TabIndex = 89;
            this.ilistModaltable.Visible = false;
            // 
            // simpleButtonView
            // 
            this.simpleButtonView.Enabled = false;
            this.simpleButtonView.ImageIndex = 8;
            this.simpleButtonView.ImageList = this.ImageList1;
            this.simpleButtonView.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonView.Location = new System.Drawing.Point(120, -1);
            this.simpleButtonView.Name = "simpleButtonView";
            this.simpleButtonView.Size = new System.Drawing.Size(28, 28);
            this.simpleButtonView.TabIndex = 92;
            this.simpleButtonView.ToolTip = "更改视图";
            this.simpleButtonView.Click += new System.EventHandler(this.simpleButtonView_Click);
            // 
            // panelDesignTable
            // 
            this.panelDesignTable.Controls.Add(this.simpleButtonDesignTable);
            this.panelDesignTable.Controls.Add(this.label7);
            this.panelDesignTable.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelDesignTable.Location = new System.Drawing.Point(811, 1);
            this.panelDesignTable.Name = "panelDesignTable";
            this.panelDesignTable.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.panelDesignTable.Size = new System.Drawing.Size(98, 144);
            this.panelDesignTable.TabIndex = 96;
            this.panelDesignTable.Visible = false;
            // 
            // simpleButtonDesignTable
            // 
            this.simpleButtonDesignTable.ImageIndex = 1;
            this.simpleButtonDesignTable.ImageList = this.imageList2;
            this.simpleButtonDesignTable.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonDesignTable.Location = new System.Drawing.Point(42, 30);
            this.simpleButtonDesignTable.Name = "simpleButtonDesignTable";
            this.simpleButtonDesignTable.Size = new System.Drawing.Size(42, 42);
            this.simpleButtonDesignTable.TabIndex = 89;
            this.simpleButtonDesignTable.ToolTip = "查看";
            this.simpleButtonDesignTable.Click += new System.EventHandler(this.simpleButtonDesignTable_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(9, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 26);
            this.label7.TabIndex = 88;
            this.label7.Text = "调查设计表:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.simpleButtonViewExcel);
            this.panel.Controls.Add(this.panel12);
            this.panel.Controls.Add(this.label6);
            this.panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel.Location = new System.Drawing.Point(909, 1);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.panel.Size = new System.Drawing.Size(98, 144);
            this.panel.TabIndex = 94;
            this.panel.Visible = false;
            // 
            // simpleButtonViewExcel
            // 
            this.simpleButtonViewExcel.ImageIndex = 0;
            this.simpleButtonViewExcel.ImageList = this.imageList2;
            this.simpleButtonViewExcel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonViewExcel.Location = new System.Drawing.Point(43, 30);
            this.simpleButtonViewExcel.Name = "simpleButtonViewExcel";
            this.simpleButtonViewExcel.Size = new System.Drawing.Size(42, 42);
            this.simpleButtonViewExcel.TabIndex = 89;
            this.simpleButtonViewExcel.ToolTip = "查看";
            this.simpleButtonViewExcel.Click += new System.EventHandler(this.simpleButtonViewExcel_Click);
            // 
            // panel12
            // 
            this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel12.Location = new System.Drawing.Point(9, 28);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(23, 114);
            this.panel12.TabIndex = 90;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(9, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 26);
            this.label6.TabIndex = 88;
            this.label6.Text = "设计一览表:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.panel5);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel11.Location = new System.Drawing.Point(1, 1);
            this.panel11.Name = "panel11";
            this.panel11.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.panel11.Size = new System.Drawing.Size(210, 144);
            this.panel11.TabIndex = 95;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panelControl1);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(203, 144);
            this.panel5.TabIndex = 90;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtTaskName);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(203, 118);
            this.panelControl1.TabIndex = 89;
            // 
            // txtTaskName
            // 
            this.txtTaskName.BackColor = System.Drawing.Color.White;
            this.txtTaskName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTaskName.Location = new System.Drawing.Point(2, 2);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(199, 114);
            this.txtTaskName.TabIndex = 0;
            this.txtTaskName.Text = "2\r\n3";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(203, 26);
            this.label8.TabIndex = 88;
            this.label8.Text = "设计名称:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.gridControl1);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 146);
            this.panel1.TabIndex = 86;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 37);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox2});
            this.gridControl1.Size = new System.Drawing.Size(1008, 109);
            this.gridControl1.TabIndex = 90;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "名称";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.panel4.Size = new System.Drawing.Size(1008, 37);
            this.panel4.TabIndex = 89;
            this.panel4.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelControl4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel9);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(5, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.panel2.Size = new System.Drawing.Size(1118, 35);
            this.panel2.TabIndex = 94;
            this.panel2.Visible = false;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.simpleButtonReset);
            this.panelControl4.Controls.Add(this.simpleButtonFind);
            this.panelControl4.Controls.Add(this.panel10);
            this.panelControl4.Controls.Add(this.comboBoxEditValue);
            this.panelControl4.Controls.Add(this.label3);
            this.panelControl4.Controls.Add(this.comboBoxEditField);
            this.panelControl4.Controls.Add(this.label2);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl4.Location = new System.Drawing.Point(119, 2);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelControl4.Size = new System.Drawing.Size(463, 31);
            this.panelControl4.TabIndex = 93;
            this.panelControl4.Visible = false;
            // 
            // simpleButtonReset
            // 
            this.simpleButtonReset.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButtonReset.ImageIndex = 81;
            this.simpleButtonReset.ImageList = this.ImageList1;
            this.simpleButtonReset.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonReset.Location = new System.Drawing.Point(423, 4);
            this.simpleButtonReset.Name = "simpleButtonReset";
            this.simpleButtonReset.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonReset.TabIndex = 97;
            this.simpleButtonReset.ToolTip = "重置";
            // 
            // simpleButtonFind
            // 
            this.simpleButtonFind.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButtonFind.ImageIndex = 16;
            this.simpleButtonFind.ImageList = this.ImageList1;
            this.simpleButtonFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonFind.Location = new System.Drawing.Point(397, 4);
            this.simpleButtonFind.Name = "simpleButtonFind";
            this.simpleButtonFind.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonFind.TabIndex = 90;
            this.simpleButtonFind.ToolTip = "查找";
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel10.Location = new System.Drawing.Point(388, 4);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(9, 25);
            this.panel10.TabIndex = 96;
            // 
            // comboBoxEditValue
            // 
            this.comboBoxEditValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxEditValue.Location = new System.Drawing.Point(248, 4);
            this.comboBoxEditValue.Name = "comboBoxEditValue";
            this.comboBoxEditValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditValue.Size = new System.Drawing.Size(140, 20);
            this.comboBoxEditValue.TabIndex = 93;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(224, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 25);
            this.label3.TabIndex = 92;
            this.label3.Text = "值";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxEditField
            // 
            this.comboBoxEditField.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxEditField.Location = new System.Drawing.Point(84, 4);
            this.comboBoxEditField.Name = "comboBoxEditField";
            this.comboBoxEditField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditField.Size = new System.Drawing.Size(140, 20);
            this.comboBoxEditField.TabIndex = 91;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(2, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.TabIndex = 89;
            this.label2.Text = "查找:  字段";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(110, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(9, 31);
            this.panel3.TabIndex = 99;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(1090, 2);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(28, 31);
            this.panel9.TabIndex = 95;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(0, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 31);
            this.label5.TabIndex = 94;
            this.label5.Text = "班块信息列表:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PopupContainerEdit1
            // 
            this.PopupContainerEdit1.Dock = System.Windows.Forms.DockStyle.Right;
            this.PopupContainerEdit1.EditValue = "显示字段";
            this.PopupContainerEdit1.Location = new System.Drawing.Point(163, 0);
            this.PopupContainerEdit1.Name = "PopupContainerEdit1";
            this.PopupContainerEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.PopupContainerEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.PopupContainerEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("PopupContainerEdit1.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.PopupContainerEdit1.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.PopupContainerEdit1.Properties.PopupControl = this.PopupContainer;
            this.PopupContainerEdit1.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.PopupContainerEdit1.Properties.ShowPopupShadow = false;
            this.PopupContainerEdit1.Size = new System.Drawing.Size(117, 22);
            this.PopupContainerEdit1.TabIndex = 91;
            // 
            // PopupContainer
            // 
            this.PopupContainer.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PopupContainer.Appearance.Options.UseBackColor = true;
            this.PopupContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.PopupContainer.Controls.Add(this.checkedListBoxControl1);
            this.PopupContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PopupContainer.Location = new System.Drawing.Point(489, 59);
            this.PopupContainer.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.PopupContainer.Name = "PopupContainer";
            this.PopupContainer.Padding = new System.Windows.Forms.Padding(1);
            this.PopupContainer.Size = new System.Drawing.Size(163, 166);
            this.PopupContainer.TabIndex = 88;
            // 
            // checkedListBoxControl1
            // 
            this.checkedListBoxControl1.CheckOnClick = true;
            this.checkedListBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.checkedListBoxControl1.Name = "checkedListBoxControl1";
            this.checkedListBoxControl1.Size = new System.Drawing.Size(157, 160);
            this.checkedListBoxControl1.TabIndex = 0;
            this.checkedListBoxControl1.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBoxControl1_ItemCheck);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(5, 5);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1014, 175);
            this.xtraTabControl1.TabIndex = 89;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panel13);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1008, 146);
            this.xtraTabPage1.Text = "专题综合信息";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panel1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1008, 146);
            this.xtraTabPage2.Text = "班块记录信息";
            // 
            // simpleButtonRead
            // 
            this.simpleButtonRead.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonRead.ImageIndex = 85;
            this.simpleButtonRead.ImageList = this.ImageList1;
            this.simpleButtonRead.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButtonRead.Location = new System.Drawing.Point(89, 0);
            this.simpleButtonRead.Name = "simpleButtonRead";
            this.simpleButtonRead.Size = new System.Drawing.Size(65, 26);
            this.simpleButtonRead.TabIndex = 98;
            this.simpleButtonRead.Text = "刷新";
            this.simpleButtonRead.ToolTip = "读取属性信息";
            this.simpleButtonRead.Click += new System.EventHandler(this.simpleButtonRead_Click);
            // 
            // panelSet
            // 
            this.panelSet.Controls.Add(this.simpleButtonRead);
            this.panelSet.Controls.Add(this.panel15);
            this.panelSet.Controls.Add(this.PopupContainerEdit1);
            this.panelSet.Location = new System.Drawing.Point(239, 0);
            this.panelSet.Name = "panelSet";
            this.panelSet.Size = new System.Drawing.Size(280, 26);
            this.panelSet.TabIndex = 2;
            this.panelSet.Visible = false;
            // 
            // panel15
            // 
            this.panel15.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel15.Location = new System.Drawing.Point(154, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(9, 26);
            this.panel15.TabIndex = 99;
            // 
            // UserControlTaskInfo
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Controls.Add(this.panelSet);
            this.Controls.Add(this.PopupContainer);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "UserControlTaskInfo";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.Size = new System.Drawing.Size(1024, 180);
            this.panel13.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            this.panelModalTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ilistModaltable)).EndInit();
            this.panelDesignTable.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainer)).EndInit();
            this.PopupContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.panelSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void InitialTaskInfo()
        {
            try
            {
                string str = "Task_ID";
                if ((this.mEditKind == "造林") || (this.mEditKind == "采伐"))
                {
                    str = "Task_ID";
                    this.xtraTabPage1.Text = this.mEditKind + "作业设计综合信息";
                }
                if (this.mEditKind == "征占用")
                {
                    str = "XMBH";
                }
                this.txtTaskName.Text = EditTask.TaskName;
                this.treeView1.Nodes.Clear();
                this.treeView1.Nodes.Add("行政区划：" + this.GetDistName(EditTask.DistCode));
                DataTable dataTable = null; //TaskManageClass.GetDataTable(this.mDBAccess, string.Concat(new object[] { "SELECT '0'+[kind]+[code] as [kindcode],name,code FROM T_DesignKind where (kind ='", int.Parse(EditTask.KindCode.Substring(1, 1)), "'and code like '", EditTask.KindCode.Substring(2, 2), "%') order by code" }));
                string str2 = "";
                T_DESIGNKIND_Mid mid = DBServiceFactory<T_DESIGNKIND_Service>.Service.FindKindCode(EditTask.KindCode.Substring(1, 1), EditTask.KindCode.Substring(2, 2));

                if (EditTask.KindCode.Contains("0000"))
                {
                    str2 = mid.name;
                }
                else if (EditTask.KindCode.Contains("00"))
                {
                    str2 = mid.name + "-" + dataTable.Select("Code='" + EditTask.KindCode.Substring(2, 4) + "00'")[0]["name"].ToString();
                }
                else
                {
                    str2 = dataTable.Rows[0]["name"].ToString() + "-" + dataTable.Select("Code='" + EditTask.KindCode.Substring(2, 4) + "00'")[0]["name"].ToString() + "-" + dataTable.Select("Code='" + EditTask.KindCode.Substring(2, 6) + "'")[0]["name"].ToString();
                }
                if ((this.mEditKind == "造林") || (this.mEditKind == "采伐"))
                {
                    this.treeView1.Nodes.Add("设计类型：" + str2);
                    this.treeView1.Nodes.Add("设计年度：" + EditTask.TaskYear);
                    this.treeView1.Nodes.Add("创建时间：" + EditTask.CreateTime);
                }
                else if (this.mEditKind == "征占用")
                {
                    this.treeView1.Nodes.Add("林地用途：" + str2);
                    this.treeView1.Nodes.Add("项目年度：" + EditTask.TaskYear);
                    this.treeView1.Nodes.Add("创建时间：" + EditTask.CreateTime);
                }
                int num = 0;
                IQueryFilter queryFilter = new QueryFilterClass {
                    WhereClause = string.Concat(new object[] { 
                        str,
                        " = '",
                        EditTask.TaskID,
                        "'"
                    })
                };
                num = this.mFeatureLayer.FeatureClass.FeatureCount(queryFilter);
                queryFilter.WhereClause = string.Concat(new object[] { str, " like ',", EditTask.TaskID, "'" });
                num += this.mFeatureLayer.FeatureClass.FeatureCount(queryFilter);
                queryFilter.WhereClause = string.Concat(new object[] { str, " like '", EditTask.TaskID, ",'" });
                num += this.mFeatureLayer.FeatureClass.FeatureCount(queryFilter);
                this.treeView1.Nodes.Add("小班总数：" + num.ToString());
                double num2 = 0.0;
                if (num == 0)
                {
                    num2 = 0.0;
                }
                else
                {
                    string name = "";
                    if (this.mEditKind == "造林")
                    {
                        name = "MIAN_JI";
                    }
                    else if (this.mEditKind == "采伐")
                    {
                        name = "CFMJ";
                    }
                    else if (this.mEditKind == "征占用")
                    {
                        name = "MIAN_JI";
                    }
                    IQueryFilter filter = new QueryFilterClass {
                        WhereClause = string.Concat(new object[] { 
                            str,
                            "='",
                            EditTask.TaskID,
                            "'"
                        })
                    };
                    IFeatureCursor cursor = this.mFeatureLayer.FeatureClass.Search(filter, false);
                    for (IFeature feature = cursor.NextFeature(); feature != null; feature = cursor.NextFeature())
                    {
                        int index = feature.Fields.FindField(name);
                        try
                        {
                            if ((feature.get_Value(index) != null) && (feature.get_Value(index).ToString().Trim() != ""))
                            {
                                num2 += double.Parse(feature.get_Value(index).ToString());
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                this.treeView1.Nodes.Add("小班总面积：" + num2);
                if (EditTask.LogicChkState == LogicCheckState.Failure)
                {
                    this.treeView1.Nodes.Add("逻辑校验：未通过");
                }
                else if (EditTask.LogicChkState == LogicCheckState.Success)
                {
                    this.treeView1.Nodes.Add("逻辑校验：通过");
                }
                if (EditTask.ToplogicChkState == ToplogicCheckState.Failure)
                {
                    this.treeView1.Nodes.Add("图形校验：未通过");
                }
                else if (EditTask.ToplogicChkState == ToplogicCheckState.Success)
                {
                    this.treeView1.Nodes.Add("图形校验：通过");
                }
                if (EditTask.KindCode.ToString().Substring(0, 2) == "01")
                {
                    dataTable=null;// TaskManageClass.GetDataTable(this.mDBAccess, string.Concat(new object[] { "select * from T_Sheet where ", str, "=", EditTask.TaskID }));
                    this.listViewModaltable.Items.Clear();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        this.listViewModaltable.Items.Add(dataTable.Rows[i]["Sheet_Code"].ToString(), 7);
                    }
                }
                else
                {
                    dataTable = null;// TaskManageClass.GetDataTable(this.mDBAccess, string.Concat(new object[] { "select * from T_Sheet where ", str, "=", EditTask.TaskID }));
                    if (dataTable.Rows.Count == 0)
                    {
                        this.simpleButtonDesignTable.Enabled = false;
                    }
                    else if (dataTable.Rows.Count > 0)
                    {
                        this.simpleButtonDesignTable.Enabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskInfo", "InitialTaskInfo", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitialTaskInfo2(bool bfirst)
        {
            try
            {
                IFields fields = null;
                string s = "";
                IRow row = null;
                IMap focusMap = this.mHookHelper.FocusMap;
                IFeatureClass featureClass = this.mFeatureLayer.FeatureClass;
                IObjectClass class3 = featureClass;
                if (class3 == null)
                {
                    return;
                }
                IEnumRelationshipClass class4 = class3.get_RelationshipClasses(esriRelRole.esriRelRoleOrigin);
                class4.Reset();
                IRelationshipClass class5 = class4.Next();
            Label_0048:
                if (class5 == null)
                {
                    fields = this.mFeatureLayer.FeatureClass.Fields;
                    if ((this.mEditKind == "造林") || (this.mEditKind == "采伐"))
                    {
                        s = "Task_ID";
                        s = fields.FindField("Task_ID").ToString();
                    }
                    if (this.mEditKind == "征占用")
                    {
                        s = "XMBH";
                        s = fields.FindField("XMBH").ToString();
                    }
                }
                else if (class5 != null)
                {
                    IObjectClass destinationClass = class5.DestinationClass;
                    this.mRelationTable = destinationClass as ITable;
                    IDataset mRelationTable = this.mRelationTable as IDataset;
                    if (mRelationTable.Name != EditTask.TableName)
                    {
                        class5 = class4.Next();
                        destinationClass = class5.DestinationClass;
                        this.mRelationTable = destinationClass as ITable;
                        if ((this.mRelationTable as IDataset).Name != EditTask.TableName)
                        {
                            class5 = null;
                            goto Label_0048;
                        }
                    }
                    s = class5.OriginForeignKey;
                    fields = this.mRelationTable.Fields;
                }
                if (bfirst)
                {
                    this.mTable = new DataTable();
                    this.checkedListBoxControl1.Items.Clear();
                    string skeyvalue = "";
                    bool flag = false;
                    if (this.mEditKind.Contains("造林"))
                    {
                        skeyvalue = "ZaoLinFieldString2";
                        flag = false;
                    }
                    if (this.mEditKind.Contains("采伐"))
                    {
                        skeyvalue = "CaiFaFieldString2";
                        flag = false;
                    }
                    if (this.mEditKind.Contains("征占用"))
                    {
                        skeyvalue = "ZZYFieldString2";
                        flag = false;
                    }
                    for (int i = 0; i < fields.FieldCount; i++)
                    {
                        IField field = fields.get_Field(i);
                        if ((((field.Type != esriFieldType.esriFieldTypeOID) && (field.Type != esriFieldType.esriFieldTypeGeometry)) && ((featureClass.LengthField.Name != field.Name) && (featureClass.AreaField.Name != field.Name))) && this.CheckFieldVisiable(field.Name, skeyvalue, flag))
                        {
                            DataColumn column = new DataColumn(fields.get_Field(i).Name, typeof(string)) {
                                Caption = fields.get_Field(i).AliasName
                            };
                            this.mTable.Columns.Add(column);
                            this.checkedListBoxControl1.Items.Add(column.Caption, true);
                        }
                    }
                    for (int j = 0; j < fields.FieldCount; j++)
                    {
                        if (fields.get_Field(j).Type == esriFieldType.esriFieldTypeOID)
                        {
                            DataColumn column2 = new DataColumn(fields.get_Field(j).Name, typeof(object));
                            this.mTable.Columns.Add(column2);
                            this.objectid = this.mTable.Columns.Count - 1;
                        }
                        if (fields.get_Field(j).Name == s)
                        {
                            DataColumn column3 = new DataColumn(fields.get_Field(j).Name, typeof(string));
                            this.mTable.Columns.Add(column3);
                        }
                    }
                }
                this.mTable.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                if ((this.mEditKind == "造林") || (this.mEditKind == "采伐"))
                {
                    queryFilter.WhereClause = "Task_ID='" + EditTask.TaskID + "'";
                }
                if (this.mEditKind == "征占用")
                {
                    queryFilter.WhereClause = "XMBH='" + EditTask.TaskID + "'";
                }
                if (this.mRelationTable == null)
                {
                    this.mRelationTable = (this.mFeatureLayer.FeatureClass as IDataset) as ITable;
                }
                if (this.mRelationTable != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    ICursor cursor = this.mRelationTable.Search(queryFilter, false);
                    for (row = cursor.NextRow(); row != null; row = cursor.NextRow())
                    {
                        DataRow row2 = this.mTable.NewRow();
                        for (int k = 0; k < this.mTable.Columns.Count; k++)
                        {
                            DataColumn column4 = this.mTable.Columns[k];
                            int index = fields.FindField(column4.ColumnName);
                            IField field3 = fields.get_Field(index);
                            string str3 = "";
                            if (index == -1)
                            {
                                continue;
                            }
                            if ((field3.Domain != null) && (field3.Domain.Type == esriDomainType.esriDTCodedValue))
                            {
                                str3 = "";
                                try
                                {
                                    ICodedValueDomain domain = (ICodedValueDomain) fields.get_Field(index).Domain;
                                    long num5 = -1L;
                                    if ((row != null) && (this.mRelationTable != null))
                                    {
                                        num5 = Convert.ToInt64(row.get_Value(index));
                                    }
                                    else if ((row == null) && (this.mRelationTable != null))
                                    {
                                        num5 = -1L;
                                    }
                                    for (int m = 0; m < domain.CodeCount; m++)
                                    {
                                        if (num5 == Convert.ToInt64(domain.get_Value(m)))
                                        {
                                            str3 = domain.get_Name(m);
                                            goto Label_0596;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    if (row != null)
                                    {
                                        str3 = row.get_Value(index).ToString();
                                    }
                                }
                            }
                            else if (row != null)
                            {
                                str3 = row.get_Value(index).ToString();
                            }
                            else if (this.mRelationTable != null)
                            {
                                str3 = "";
                            }
                            else
                            {
                                str3 = "";
                            }
                        Label_0596:
                            row2[k] = str3;
                        }
                        this.mTable.Rows.Add(row2);
                    }
                }
                this.gridControl1.DataSource = null;
                this.gridView1.Columns.Clear();
                this.gridControl1.DataSource = this.mTable;
                this.gridView1.RefreshData();
                this.gridView1.OptionsBehavior.Editable = false;
                this.gridControl1.Enabled = true;
                if (bfirst)
                {
                    for (int n = 0; n < (this.gridView1.Columns.Count - 1); n++)
                    {
                        if (n < 15)
                        {
                            this.gridView1.Columns[n].Visible = true;
                            this.checkedListBoxControl1.Items[n].CheckState = CheckState.Checked;
                        }
                        else
                        {
                            this.gridView1.Columns[n].Visible = false;
                            if (n < this.checkedListBoxControl1.Items.Count)
                            {
                                this.checkedListBoxControl1.Items[n].CheckState = CheckState.Unchecked;
                            }
                        }
                    }
                }
                else
                {
                    for (int num8 = 0; num8 < (this.checkedListBoxControl1.Items.Count - 1); num8++)
                    {
                        if (this.checkedListBoxControl1.Items[num8].CheckState == CheckState.Checked)
                        {
                            this.gridView1.Columns[num8].Visible = true;
                        }
                        else
                        {
                            this.gridView1.Columns[num8].Visible = false;
                        }
                    }
                }
                this.gridView1.Columns[this.objectid].Visible = false;
                if (int.Parse(s) < this.gridView1.Columns.Count)
                {
                    this.gridView1.Columns[s].Visible = false;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception exception)
            {
                this.Cursor = Cursors.Default;
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskInfo", "InitialTaskInfo2", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void listViewModaltable_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItems = this.listViewModaltable.SelectedItems;
            if ((selectedItems != null) && (selectedItems.Count >= 1))
            {
                string text = selectedItems[0].Text;
                this.OnModalClick(text);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panelControl5_Paint(object sender, PaintEventArgs e)
        {
        }

        private void simpleButtonDesignTable_Click(object sender, EventArgs e)
        {
        }

        private void simpleButtonRead_Click(object sender, EventArgs e)
        {
            if (this.xtraTabControl1.SelectedTabPageIndex == 1)
            {
                this.InitialTaskInfo2(false);
            }
            else if (this.xtraTabControl1.SelectedTabPageIndex == 0)
            {
                this.InitialTaskInfo();
            }
        }

        private void simpleButtonView_Click(object sender, EventArgs e)
        {
            if (this.listViewModaltable.View == View.LargeIcon)
            {
                this.listViewModaltable.View = View.SmallIcon;
            }
            else if (this.listViewModaltable.View == View.SmallIcon)
            {
                this.listViewModaltable.View = View.List;
            }
            else if (this.listViewModaltable.View == View.List)
            {
                this.listViewModaltable.View = View.LargeIcon;
            }
        }

        private void simpleButtonViewExcel_Click(object sender, EventArgs e)
        {
            this.OnViewExcelClick();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (this.xtraTabControl1.SelectedTabPageIndex == 1)
            {
                this.InitialTaskInfo2(false);
                this.simpleButtonRead.Visible = true;
                this.PopupContainerEdit1.Visible = true;
                this.panelSet.Visible = true;
            }
            else if (this.xtraTabControl1.SelectedTabPageIndex == 0)
            {
                this.InitialTaskInfo();
                this.simpleButtonRead.Visible = true;
                this.PopupContainerEdit1.Visible = false;
                this.panelSet.Visible = true;
            }
        }

        public delegate void DesignTableClickhandler(string sCode);

        public delegate void ModalClickHandler(string sCode);

        public delegate void ViewExcelClickHandler();
    }
}

