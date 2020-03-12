namespace TaskManage
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Columns;
    using DevExpress.XtraTreeList.Nodes;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using FormBase;
    using FunFactory;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Linq;
    using td.db.mid.sys;
    using td.db.orm;
    using td.db.service.sys;
    using td.forest.task.linker;
    using td.logic.sys;
    using Utilities;

    /// <summary>
    /// 征占用--项目列表窗体
    /// </summary>
    public class UserControlProjectList : UserControlBase1
    {
        private SimpleButton ButtonFind;
        private ComboBoxEdit comboBoxEditKind2;
        private ComboBoxEdit comboBoxKind;
        private IContainer components;
        private PopupContainerEdit curPopupEdit;
        private DateEdit dateEdit1;
        private DateEdit dateEdit2;
        private GridColumn gridColumn3;
        public GridControl gridControl1;
        public GridView gridView1;
        private ImageList imageList1;
        private Label label1;
        private Label label13;
        private Label label14;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label30;
        private Label label31;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelEdit;
        private Label labelInfo;
        private Label labelInfo2;
        private const string mClassName = "TaskManage.UserControlProjectList";
       
        private IFeatureLayer mEditLayer;
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private ArrayList mFeatureList;
        private DataTable mFieldTable;
        private IGroupLayer mGroupLayer;
        private IHookHelper mHookHelper;
        private string mKindCode = "";      
        private IFeatureLayer mLineLayer;
        private IFeatureLayer mLineLayer2;
        private IMap mMap;
        private ArrayList mNameList;
        private IFeatureLayer mPointLayer;
        private IFeatureLayer mPointLayer2;
        private IFeatureLayer mPolyLayer;
        private IFeatureLayer mPolyLayer2;
        private ArrayList mQueryList;
        private bool mSelected;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel32;
        private Panel panel33;
        private Panel panel35;
        private Panel panel4;
        private Panel panel6;
        private Panel panelButtons;
        private PanelControl panelControl2;
        private Panel panelEdit;
        private Panel panelKind;
        private Panel panelName;
        internal PopupContainerControl PopupContainer;
        internal PopupContainerEdit PopupContainerEdit1;
        internal PopupContainerEdit popupContainerEdit2;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private SimpleButton simpleButtonAddNew;
        private SimpleButton simpleButtonDelete;
        private SimpleButton simpleButtonDeleteAll;
        private SimpleButton simpleButtonEdit;
        private SimpleButton simpleButtonEditCancle;
        private SimpleButton simpleButtonEditOK;
        private SimpleButton simpleButtonMore;
        public SimpleButton simpleButtonOpen;
        private SimpleButton simpleButtonReset;
        internal TreeListColumn tcolBase1;
        private TextEdit textName;
        private TextBox textName2;
        private TextEdit textPZWH;
        internal TreeList tListDesignKind;

        public UserControlProjectList()
        {
            this.InitializeComponent();
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            this.FindProject();
        }
        private void OpenProject(T_EDITTASK_ZT_Mid mid)
        {
            EditTask.TaskID = mid.ID;
            EditTask.TaskName = mid.taskname;
            EditTask.KindCode = mid.taskkind;
            EditTask.TaskState = TaskState2.Edit;
            EditTask.PZWH = mid.taskpath;
            this.simpleButtonAddNew.Enabled = false;
            this.simpleButtonDelete.Enabled = false;
            this.simpleButtonDeleteAll.Enabled = false;
            this.simpleButtonEdit.Enabled = false;
            this.SetLayerFilter(true);
            this.simpleButtonOpen.Text = "关闭";
            this.simpleButtonOpen.ImageIndex = 0x10;
            this.gridControl1.Enabled = false;
            this.panel1.Enabled = false;
            this.panel2.Enabled = false;
            this.panel3.Enabled = false;
            this.panel35.Enabled = false;
        }
        public void ButtonOpenClick()
        {
            int focusedDataSourceRowIndex = this.gridView1.GetFocusedDataSourceRowIndex();
            if (focusedDataSourceRowIndex != -1)
            {
                if (this.simpleButtonOpen.Text == "打开")
                {               
               
                    T_EDITTASK_ZT_Mid mid = m_projList[focusedDataSourceRowIndex];
                    OpenProject(mid);
                }
                else
                {
                    EditTask.TaskID = 0L;
                    EditTask.TaskName = "征占用";
                    EditTask.KindCode = "04";
                    this.simpleButtonOpen.Text = "打开";
                    this.simpleButtonOpen.ImageIndex = 15;
                    this.simpleButtonAddNew.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonDeleteAll.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.SetLayerFilter(false);
                    this.gridControl1.Enabled = true;
                    this.panel1.Enabled = true;
                    this.panel2.Enabled = true;
                    this.panel3.Enabled = true;
                    this.panel35.Enabled = true;
                }
            }
        }

        private bool CreateProject()
        {
            try
            {
                T_EDITTASK_ZT_Mid mid = new T_EDITTASK_ZT_Mid();             
                string taskYear = EditTask.TaskYear;
                mid.taskname = this.textName2.Text.Trim();
                mid.createtime = DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                mid.edittime = DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                mid.datasetname = UtilFactory.GetConfigOpt().GetConfigValue("ZZYDataset");
                mid.layername= UtilFactory.GetConfigOpt().GetConfigValue("ZZYLayer") + "_" + taskYear;
                mid.tablename = "";    
                mid .taskpath= this.textPZWH.Text.Trim();     
                mid.taskstate = "1";
                mid.taskyear = taskYear;
                mid.distcode = EditTask.DistCode;
                mid.logiccheckstate = "0";
                mid.toplogiccheckstate = "0";

                this.popupContainerEdit2.Tag.ToString();
                string str4 = "0" + this.mKindCode + this.popupContainerEdit2.Tag.ToString();
                mid.taskkind= str4;
                string str5 = DateTime.Parse(mid.createtime).ToString("yyyyMMddHHmmss");
                mid.bh = EditTask.DistCode + mid.taskkind + str5;
                mid .taskpath= this.textPZWH.Text.Trim();           
                return DBServiceFactory<T_EDITTASK_ZT_Service>.Service.Add(mid);

            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "CreateProject", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DeleteEditLayerFeature(string taskid)
        {
            try
            {
                IWorkspaceEdit editWorkspace = EditTask.EditWorkspace as IWorkspaceEdit;
                if (editWorkspace == null)
                {
                    return false;
                }
                GC.Collect();
                IQueryFilter filter = new QueryFilterClass();
                if (taskid != "")
                {
                    filter.WhereClause = "XMBH='" + taskid + "'";
                }
                for (int i = 0; i < 3; i++)
                {
                    IFeatureLayer layer = EditTask.UnderLayers[i] as IFeatureLayer;
                    IFeatureCursor cursor = layer.FeatureClass.Search(filter, false);
                    IFeature feature = cursor.NextFeature();
                    editWorkspace.StartEditing(false);
                    editWorkspace.StartEditOperation();
                    Application.DoEvents();
                    while (feature != null)
                    {
                        feature.Delete();
                        feature.Store();
                        feature = cursor.NextFeature();
                    }
                    editWorkspace.StopEditOperation();
                    editWorkspace.StopEditing(true);
                    editWorkspace.StartEditing(false);
                    editWorkspace.StartEditOperation();
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "DeleteEditLayerFeature", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
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

        private void FindProject()
        {
            try
            {
                string str = "(taskkind like '0" + this.mKindCode + "%') ";
                if (this.PopupContainerEdit1.Text != "")
                {
                    string str2 = this.PopupContainerEdit1.Tag.ToString();
                    if (str2.Contains("0000"))
                    {
                        str = "taskkind like '0" + this.mKindCode + str2.Replace("0000", "") + "%'";
                    }
                    else
                    {
                        str = "taskkind like '0" + this.mKindCode + str2 + "'";
                    }
                }
                if (this.textName.Text.Trim() != "")
                {
                    string str3 = "taskname like '%" + this.textName.Text.Trim() + "%'";
                    if (str != "")
                    {
                        str = str + " and " + str3;
                    }
                    else
                    {
                        str = str3;
                    }
                }
                if ((this.dateEdit1.Text != "") || (this.dateEdit2.Text != ""))
                {
                    string str4 = "";
            
                        string str5 = "";
                        string str6 = "";
                        if (this.dateEdit1.Text.Trim() != "")
                        {
                            str5 = DateTime.Parse(this.dateEdit1.Text + " 00:00:00").ToString("yyyyMMddHHmmss");
                            str4 = " replace(replace(replace(CONVERT(varchar, cast(createtime as datetime), 120 ),'-',''),' ',''),':','')>'" + str5 + "'";
                        }
                        if (this.dateEdit2.Text.Trim() != "")
                        {
                            str6 = DateTime.Parse(this.dateEdit2.Text + " 23:59:59").ToString("yyyyMMddHHmmss");
                            if (str4 != "")
                            {
                                str4 = str4 + " and replace(replace(replace(CONVERT(varchar, cast(edittime as datetime), 120 ),'-',''),' ',''),':','')<'" + str6 + "'";
                            }
                            else
                            {
                                str4 = "replace(replace(replace(CONVERT(varchar, cast(edittime as datetime), 120 ),'-',''),' ',''),':','')<'" + str6 + "'";
                            }
                        }
                    
                    if (str != "")
                    {
                        str = str + " and " + str4;
                    }
                    else
                    {
                        str = str4;
                    }
                }
                this.m_projList = PM.FindBySql(str);
                if (this.m_projList.Count == 0)
                {
                    this.simpleButtonOpen.Enabled = false;
                    this.simpleButtonEdit.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonDeleteAll.Enabled = false;
                    this.simpleButtonAddNew.Enabled = true;
                }
                else
                {
                    this.simpleButtonOpen.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonDeleteAll.Enabled = true;
                    this.simpleButtonAddNew.Enabled = true;
                }
                this.InitialList();
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "FindProject", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int focusedRowHandle = this.gridView1.FocusedRowHandle;
            if (((focusedRowHandle != -1) && this.panelEdit.Visible) && !this.labelEdit.Text.Contains("新增"))
            {
                T_EDITTASK_ZT_Mid mid = m_projList[focusedRowHandle];
                string str = mid.taskkind;
                str = str.Substring(2, str.Length - 2);
                DataTable dataTable = TaskManageClass.GetDataTable("select * from T_DesignKind where ( code = '" + str + "' and kind='" + this.mKindCode + "') ");
                if (dataTable.Rows.Count > 0)
                {
                    this.popupContainerEdit2.Text = dataTable.Rows[0]["name"].ToString();
                    this.textName2.Text = mid.taskname;
                    this.textPZWH.Text = mid.taskpath;
                }
                this.popupContainerEdit2.Enabled = this.panelEdit.Visible;
                this.textName2.Enabled = this.panelEdit.Visible;
                this.textPZWH.Enabled = this.panelEdit.Visible;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlProjectList));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.labelInfo = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panelButtons = new System.Windows.Forms.Panel();
            this.simpleButtonOpen = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleButtonAddNew = new DevExpress.XtraEditors.SimpleButton();
            this.label14 = new System.Windows.Forms.Label();
            this.simpleButtonEdit = new DevExpress.XtraEditors.SimpleButton();
            this.label13 = new System.Windows.Forms.Label();
            this.simpleButtonDelete = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.simpleButtonDeleteAll = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel35 = new System.Windows.Forms.Panel();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.label31 = new System.Windows.Forms.Label();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.label30 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.simpleButtonReset = new DevExpress.XtraEditors.SimpleButton();
            this.panel33 = new System.Windows.Forms.Panel();
            this.simpleButtonMore = new DevExpress.XtraEditors.SimpleButton();
            this.panel32 = new System.Windows.Forms.Panel();
            this.ButtonFind = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxKind = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PopupContainerEdit1 = new DevExpress.XtraEditors.PopupContainerEdit();
            this.PopupContainer = new DevExpress.XtraEditors.PopupContainerControl();
            this.tListDesignKind = new DevExpress.XtraTreeList.TreeList();
            this.tcolBase1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textName = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelEdit = new System.Windows.Forms.Panel();
            this.panelName = new System.Windows.Forms.Panel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.textName2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textPZWH = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.simpleButtonEditOK = new DevExpress.XtraEditors.SimpleButton();
            this.label18 = new System.Windows.Forms.Label();
            this.simpleButtonEditCancle = new DevExpress.XtraEditors.SimpleButton();
            this.panelKind = new System.Windows.Forms.Panel();
            this.comboBoxEditKind2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.popupContainerEdit2 = new DevExpress.XtraEditors.PopupContainerEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.labelEdit = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            this.panel35.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainer)).BeginInit();
            this.PopupContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tListDesignKind)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textName.Properties)).BeginInit();
            this.panelEdit.SuspendLayout();
            this.panelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textPZWH.Properties)).BeginInit();
            this.panel6.SuspendLayout();
            this.panelKind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditKind2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo.ImageIndex = 8;
            this.labelInfo.ImageList = this.imageList1;
            this.labelInfo.Location = new System.Drawing.Point(4, 288);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(340, 24);
            this.labelInfo.TabIndex = 82;
            this.labelInfo.Text = "     共计";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "drawing_pen-16.png");
            this.imageList1.Images.SetKeyName(1, "monitor_16.png");
            this.imageList1.Images.SetKeyName(2, "clipboard_16.png");
            this.imageList1.Images.SetKeyName(3, "folder_16.png");
            this.imageList1.Images.SetKeyName(4, "plus_16.png");
            this.imageList1.Images.SetKeyName(5, "document_16.png");
            this.imageList1.Images.SetKeyName(6, "pencil_16.png");
            this.imageList1.Images.SetKeyName(7, "label_16.png");
            this.imageList1.Images.SetKeyName(8, "flag_16.png");
            this.imageList1.Images.SetKeyName(9, "info_16.png");
            this.imageList1.Images.SetKeyName(10, "clock_16.png");
            this.imageList1.Images.SetKeyName(11, "search_16.png");
            this.imageList1.Images.SetKeyName(12, "globe_16.png");
            this.imageList1.Images.SetKeyName(13, "folder_blue.png");
            this.imageList1.Images.SetKeyName(14, "27.png");
            this.imageList1.Images.SetKeyName(15, "(01,34).png");
            this.imageList1.Images.SetKeyName(16, "(02,29).png");
            this.imageList1.Images.SetKeyName(17, "delete.png");
            this.imageList1.Images.SetKeyName(18, "Feedicons_v2_031.png");
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panelButtons.Controls.Add(this.simpleButtonOpen);
            this.panelButtons.Controls.Add(this.label3);
            this.panelButtons.Controls.Add(this.simpleButtonAddNew);
            this.panelButtons.Controls.Add(this.label14);
            this.panelButtons.Controls.Add(this.simpleButtonEdit);
            this.panelButtons.Controls.Add(this.label13);
            this.panelButtons.Controls.Add(this.simpleButtonDelete);
            this.panelButtons.Controls.Add(this.label2);
            this.panelButtons.Controls.Add(this.simpleButtonDeleteAll);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(4, 312);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.panelButtons.Size = new System.Drawing.Size(340, 36);
            this.panelButtons.TabIndex = 81;
            // 
            // simpleButtonOpen
            // 
            this.simpleButtonOpen.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonOpen.ImageIndex = 15;
            this.simpleButtonOpen.ImageList = this.imageList1;
            this.simpleButtonOpen.Location = new System.Drawing.Point(52, 6);
            this.simpleButtonOpen.Name = "simpleButtonOpen";
            this.simpleButtonOpen.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonOpen.TabIndex = 12;
            this.simpleButtonOpen.Text = "打开";
            this.simpleButtonOpen.ToolTip = "打开项目";
            this.simpleButtonOpen.Click += new System.EventHandler(this.simpleButtonOpen_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(108, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(2, 24);
            this.label3.TabIndex = 17;
            // 
            // simpleButtonAddNew
            // 
            this.simpleButtonAddNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonAddNew.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAddNew.Image")));
            this.simpleButtonAddNew.Location = new System.Drawing.Point(110, 6);
            this.simpleButtonAddNew.Name = "simpleButtonAddNew";
            this.simpleButtonAddNew.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonAddNew.TabIndex = 14;
            this.simpleButtonAddNew.Text = "增加";
            this.simpleButtonAddNew.ToolTip = "增加项目";
            this.simpleButtonAddNew.Click += new System.EventHandler(this.simpleButtonAddNew_Click);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Right;
            this.label14.Location = new System.Drawing.Point(166, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(2, 24);
            this.label14.TabIndex = 13;
            // 
            // simpleButtonEdit
            // 
            this.simpleButtonEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEdit.Image")));
            this.simpleButtonEdit.Location = new System.Drawing.Point(168, 6);
            this.simpleButtonEdit.Name = "simpleButtonEdit";
            this.simpleButtonEdit.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonEdit.TabIndex = 1;
            this.simpleButtonEdit.Text = "修改";
            this.simpleButtonEdit.ToolTip = "修改项目名称";
            this.simpleButtonEdit.Click += new System.EventHandler(this.simpleButtonEdit_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Right;
            this.label13.Location = new System.Drawing.Point(224, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(2, 24);
            this.label13.TabIndex = 11;
            // 
            // simpleButtonDelete
            // 
            this.simpleButtonDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonDelete.Image")));
            this.simpleButtonDelete.Location = new System.Drawing.Point(226, 6);
            this.simpleButtonDelete.Name = "simpleButtonDelete";
            this.simpleButtonDelete.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonDelete.TabIndex = 0;
            this.simpleButtonDelete.Text = "删除";
            this.simpleButtonDelete.ToolTip = "删除项目";
            this.simpleButtonDelete.Click += new System.EventHandler(this.simpleButtonDelete_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(282, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(2, 24);
            this.label2.TabIndex = 19;
            // 
            // simpleButtonDeleteAll
            // 
            this.simpleButtonDeleteAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDeleteAll.ImageIndex = 18;
            this.simpleButtonDeleteAll.ImageList = this.imageList1;
            this.simpleButtonDeleteAll.Location = new System.Drawing.Point(284, 6);
            this.simpleButtonDeleteAll.Name = "simpleButtonDeleteAll";
            this.simpleButtonDeleteAll.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonDeleteAll.TabIndex = 18;
            this.simpleButtonDeleteAll.Text = "清空";
            this.simpleButtonDeleteAll.ToolTip = "删除所有项目及图斑";
            this.simpleButtonDeleteAll.Click += new System.EventHandler(this.simpleButtonDeleteAll_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(4, 162);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox2});
            this.gridControl1.Size = new System.Drawing.Size(340, 126);
            this.gridControl1.TabIndex = 98;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.Blue;
            this.gridView1.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.Blue;
            this.gridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Yellow;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
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
            // panel35
            // 
            this.panel35.Controls.Add(this.dateEdit2);
            this.panel35.Controls.Add(this.label31);
            this.panel35.Controls.Add(this.dateEdit1);
            this.panel35.Controls.Add(this.label30);
            this.panel35.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel35.Location = new System.Drawing.Point(4, 52);
            this.panel35.Name = "panel35";
            this.panel35.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel35.Size = new System.Drawing.Size(340, 26);
            this.panel35.TabIndex = 100;
            // 
            // dateEdit2
            // 
            this.dateEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateEdit2.EditValue = null;
            this.dateEdit2.Location = new System.Drawing.Point(159, 0);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit2.Size = new System.Drawing.Size(181, 20);
            this.dateEdit2.TabIndex = 24;
            // 
            // label31
            // 
            this.label31.Dock = System.Windows.Forms.DockStyle.Left;
            this.label31.Location = new System.Drawing.Point(147, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(12, 20);
            this.label31.TabIndex = 23;
            this.label31.Text = "-";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dateEdit1
            // 
            this.dateEdit1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(70, 0);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit1.Size = new System.Drawing.Size(77, 20);
            this.dateEdit1.TabIndex = 22;
            // 
            // label30
            // 
            this.label30.Dock = System.Windows.Forms.DockStyle.Left;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(70, 20);
            this.label30.TabIndex = 100;
            this.label30.Text = "创建时间:";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.simpleButtonReset);
            this.panel2.Controls.Add(this.panel33);
            this.panel2.Controls.Add(this.simpleButtonMore);
            this.panel2.Controls.Add(this.panel32);
            this.panel2.Controls.Add(this.ButtonFind);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(4, 104);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.panel2.Size = new System.Drawing.Size(340, 32);
            this.panel2.TabIndex = 101;
            // 
            // simpleButtonReset
            // 
            this.simpleButtonReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonReset.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonReset.Image")));
            this.simpleButtonReset.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButtonReset.Location = new System.Drawing.Point(160, 4);
            this.simpleButtonReset.Name = "simpleButtonReset";
            this.simpleButtonReset.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonReset.TabIndex = 14;
            this.simpleButtonReset.Text = "重置";
            this.simpleButtonReset.ToolTip = "重新设置查询条件";
            this.simpleButtonReset.Click += new System.EventHandler(this.simpleButtonReset_Click);
            // 
            // panel33
            // 
            this.panel33.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel33.Location = new System.Drawing.Point(216, 4);
            this.panel33.Name = "panel33";
            this.panel33.Size = new System.Drawing.Size(6, 24);
            this.panel33.TabIndex = 18;
            this.panel33.Visible = false;
            // 
            // simpleButtonMore
            // 
            this.simpleButtonMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonMore.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonMore.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonMore.Image")));
            this.simpleButtonMore.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButtonMore.Location = new System.Drawing.Point(222, 4);
            this.simpleButtonMore.Name = "simpleButtonMore";
            this.simpleButtonMore.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonMore.TabIndex = 13;
            this.simpleButtonMore.Tag = "基本";
            this.simpleButtonMore.Text = "更多";
            this.simpleButtonMore.ToolTip = "更多查询条件";
            this.simpleButtonMore.Visible = false;
            // 
            // panel32
            // 
            this.panel32.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel32.Location = new System.Drawing.Point(278, 4);
            this.panel32.Name = "panel32";
            this.panel32.Size = new System.Drawing.Size(6, 24);
            this.panel32.TabIndex = 17;
            // 
            // ButtonFind
            // 
            this.ButtonFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFind.Image")));
            this.ButtonFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.ButtonFind.Location = new System.Drawing.Point(284, 4);
            this.ButtonFind.Name = "ButtonFind";
            this.ButtonFind.Size = new System.Drawing.Size(56, 24);
            this.ButtonFind.TabIndex = 12;
            this.ButtonFind.Text = "查找";
            this.ButtonFind.ToolTip = "小班查找";
            this.ButtonFind.Click += new System.EventHandler(this.ButtonFind_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxKind);
            this.panel1.Controls.Add(this.PopupContainerEdit1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(4, 26);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel1.Size = new System.Drawing.Size(340, 26);
            this.panel1.TabIndex = 102;
            // 
            // comboBoxKind
            // 
            this.comboBoxKind.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxKind.Location = new System.Drawing.Point(70, 22);
            this.comboBoxKind.Name = "comboBoxKind";
            this.comboBoxKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxKind.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxKind.Size = new System.Drawing.Size(270, 20);
            this.comboBoxKind.TabIndex = 101;
            this.comboBoxKind.Visible = false;
            // 
            // PopupContainerEdit1
            // 
            this.PopupContainerEdit1.Dock = System.Windows.Forms.DockStyle.Top;
            this.PopupContainerEdit1.EditValue = "";
            this.PopupContainerEdit1.Location = new System.Drawing.Point(70, 0);
            this.PopupContainerEdit1.Name = "PopupContainerEdit1";
            this.PopupContainerEdit1.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.PopupContainerEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.PopupContainerEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("PopupContainerEdit1.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.PopupContainerEdit1.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.PopupContainerEdit1.Properties.PopupControl = this.PopupContainer;
            this.PopupContainerEdit1.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.PopupContainerEdit1.Properties.ShowPopupShadow = false;
            this.PopupContainerEdit1.Size = new System.Drawing.Size(270, 22);
            this.PopupContainerEdit1.TabIndex = 107;
            this.PopupContainerEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.PopupContainerEdit1_ButtonClick);
            this.PopupContainerEdit1.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.PopupContainerEdit1_ButtonPressed);
            // 
            // PopupContainer
            // 
            this.PopupContainer.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PopupContainer.Appearance.Options.UseBackColor = true;
            this.PopupContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.PopupContainer.Controls.Add(this.tListDesignKind);
            this.PopupContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PopupContainer.Location = new System.Drawing.Point(110, 162);
            this.PopupContainer.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.PopupContainer.Name = "PopupContainer";
            this.PopupContainer.Padding = new System.Windows.Forms.Padding(1);
            this.PopupContainer.Size = new System.Drawing.Size(170, 166);
            this.PopupContainer.TabIndex = 108;
            // 
            // tListDesignKind
            // 
            this.tListDesignKind.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.Empty.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.EvenRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.EvenRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.FocusedCell.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tListDesignKind.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDesignKind.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.FocusedRow.BackColor = System.Drawing.Color.DodgerBlue;
            this.tListDesignKind.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDesignKind.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDesignKind.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.GroupButton.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.GroupButton.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDesignKind.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tListDesignKind.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tListDesignKind.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDesignKind.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.HorzLine.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.OddRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.OddRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tListDesignKind.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tListDesignKind.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.Preview.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.Preview.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.Row.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.Row.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tListDesignKind.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tListDesignKind.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.TreeLine.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDesignKind.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.VertLine.Options.UseBackColor = true;
            this.tListDesignKind.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tListDesignKind.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tcolBase1});
            this.tListDesignKind.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tListDesignKind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tListDesignKind.Location = new System.Drawing.Point(3, 3);
            this.tListDesignKind.LookAndFeel.SkinName = "Blue";
            this.tListDesignKind.Name = "tListDesignKind";
            this.tListDesignKind.OptionsBehavior.Editable = false;
            this.tListDesignKind.OptionsView.ShowColumns = false;
            this.tListDesignKind.OptionsView.ShowHorzLines = false;
            this.tListDesignKind.OptionsView.ShowIndicator = false;
            this.tListDesignKind.OptionsView.ShowVertLines = false;
            this.tListDesignKind.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowForFocusedRow;
            this.tListDesignKind.Size = new System.Drawing.Size(164, 160);
            this.tListDesignKind.TabIndex = 78;
            this.tListDesignKind.TreeLevelWidth = 12;
            this.tListDesignKind.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tListDesignKind.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tListDesignKind_FocusedNodeChanged);
            this.tListDesignKind.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tListDesignKind_MouseUp);
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
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 100;
            this.label4.Text = "林地用途:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textName);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(4, 78);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel3.Size = new System.Drawing.Size(340, 26);
            this.panel3.TabIndex = 103;
            // 
            // textName
            // 
            this.textName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName.Location = new System.Drawing.Point(70, 0);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(270, 20);
            this.textName.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 100;
            this.label1.Text = "项目名称:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.ImageIndex = 2;
            this.label5.ImageList = this.imageList1;
            this.label5.Location = new System.Drawing.Point(4, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(340, 26);
            this.label5.TabIndex = 104;
            this.label5.Text = "     项目列表";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(340, 26);
            this.label6.TabIndex = 105;
            this.label6.Text = "     查询条件";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelEdit
            // 
            this.panelEdit.Controls.Add(this.panelName);
            this.panelEdit.Controls.Add(this.panel4);
            this.panelEdit.Controls.Add(this.panel6);
            this.panelEdit.Controls.Add(this.panelKind);
            this.panelEdit.Controls.Add(this.labelEdit);
            this.panelEdit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEdit.Location = new System.Drawing.Point(4, 348);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Size = new System.Drawing.Size(340, 152);
            this.panelEdit.TabIndex = 106;
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.panelControl2);
            this.panelName.Controls.Add(this.label7);
            this.panelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelName.Location = new System.Drawing.Point(0, 78);
            this.panelName.Name = "panelName";
            this.panelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panelName.Size = new System.Drawing.Size(340, 44);
            this.panelName.TabIndex = 105;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.textName2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(70, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(270, 38);
            this.panelControl2.TabIndex = 102;
            // 
            // textName2
            // 
            this.textName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName2.Location = new System.Drawing.Point(2, 2);
            this.textName2.Multiline = true;
            this.textName2.Name = "textName2";
            this.textName2.Size = new System.Drawing.Size(266, 34);
            this.textName2.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 38);
            this.label7.TabIndex = 100;
            this.label7.Text = "项目名称:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textPZWH);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 52);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel4.Size = new System.Drawing.Size(340, 26);
            this.panel4.TabIndex = 108;
            // 
            // textPZWH
            // 
            this.textPZWH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPZWH.Location = new System.Drawing.Point(70, 0);
            this.textPZWH.Name = "textPZWH";
            this.textPZWH.Size = new System.Drawing.Size(270, 20);
            this.textPZWH.TabIndex = 101;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 20);
            this.label9.TabIndex = 100;
            this.label9.Text = "批准文号:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panel6.Controls.Add(this.labelInfo2);
            this.panel6.Controls.Add(this.simpleButtonEditOK);
            this.panel6.Controls.Add(this.label18);
            this.panel6.Controls.Add(this.simpleButtonEditCancle);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 122);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel6.Size = new System.Drawing.Size(340, 30);
            this.panel6.TabIndex = 107;
            // 
            // labelInfo2
            // 
            this.labelInfo2.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo2.ForeColor = System.Drawing.Color.Blue;
            this.labelInfo2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo2.ImageIndex = 9;
            this.labelInfo2.ImageList = this.imageList1;
            this.labelInfo2.Location = new System.Drawing.Point(0, 0);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(220, 24);
            this.labelInfo2.TabIndex = 107;
            this.labelInfo2.Text = "     ";
            this.labelInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo2.Visible = false;
            // 
            // simpleButtonEditOK
            // 
            this.simpleButtonEditOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonEditOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEditOK.Image")));
            this.simpleButtonEditOK.Location = new System.Drawing.Point(220, 0);
            this.simpleButtonEditOK.Name = "simpleButtonEditOK";
            this.simpleButtonEditOK.Size = new System.Drawing.Size(58, 24);
            this.simpleButtonEditOK.TabIndex = 1;
            this.simpleButtonEditOK.Text = "确定";
            this.simpleButtonEditOK.Click += new System.EventHandler(this.simpleButtonEditOK_Click);
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Dock = System.Windows.Forms.DockStyle.Right;
            this.label18.Location = new System.Drawing.Point(278, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(4, 24);
            this.label18.TabIndex = 11;
            // 
            // simpleButtonEditCancle
            // 
            this.simpleButtonEditCancle.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonEditCancle.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEditCancle.Image")));
            this.simpleButtonEditCancle.Location = new System.Drawing.Point(282, 0);
            this.simpleButtonEditCancle.Name = "simpleButtonEditCancle";
            this.simpleButtonEditCancle.Size = new System.Drawing.Size(58, 24);
            this.simpleButtonEditCancle.TabIndex = 0;
            this.simpleButtonEditCancle.Text = "取消";
            this.simpleButtonEditCancle.Click += new System.EventHandler(this.simpleButtonEditCancle_Click);
            // 
            // panelKind
            // 
            this.panelKind.Controls.Add(this.comboBoxEditKind2);
            this.panelKind.Controls.Add(this.popupContainerEdit2);
            this.panelKind.Controls.Add(this.label8);
            this.panelKind.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKind.Location = new System.Drawing.Point(0, 26);
            this.panelKind.Name = "panelKind";
            this.panelKind.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panelKind.Size = new System.Drawing.Size(340, 26);
            this.panelKind.TabIndex = 104;
            // 
            // comboBoxEditKind2
            // 
            this.comboBoxEditKind2.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxEditKind2.Location = new System.Drawing.Point(70, 22);
            this.comboBoxEditKind2.Name = "comboBoxEditKind2";
            this.comboBoxEditKind2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditKind2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditKind2.Size = new System.Drawing.Size(270, 20);
            this.comboBoxEditKind2.TabIndex = 101;
            this.comboBoxEditKind2.Visible = false;
            // 
            // popupContainerEdit2
            // 
            this.popupContainerEdit2.Dock = System.Windows.Forms.DockStyle.Top;
            this.popupContainerEdit2.EditValue = "";
            this.popupContainerEdit2.Location = new System.Drawing.Point(70, 0);
            this.popupContainerEdit2.Name = "popupContainerEdit2";
            this.popupContainerEdit2.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.popupContainerEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.popupContainerEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("popupContainerEdit2.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.popupContainerEdit2.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.popupContainerEdit2.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.popupContainerEdit2.Properties.ShowPopupShadow = false;
            this.popupContainerEdit2.Size = new System.Drawing.Size(270, 22);
            this.popupContainerEdit2.TabIndex = 108;
            this.popupContainerEdit2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupContainerEdit2_ButtonClick);
            this.popupContainerEdit2.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupContainerEdit2_ButtonPressed);
            this.popupContainerEdit2.EditValueChanged += new System.EventHandler(this.popupContainerEdit2_EditValueChanged);
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 20);
            this.label8.TabIndex = 100;
            this.label8.Text = "林地用途:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEdit
            // 
            this.labelEdit.BackColor = System.Drawing.Color.Transparent;
            this.labelEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEdit.ForeColor = System.Drawing.Color.Blue;
            this.labelEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelEdit.ImageIndex = 0;
            this.labelEdit.ImageList = this.imageList1;
            this.labelEdit.Location = new System.Drawing.Point(0, 0);
            this.labelEdit.Name = "labelEdit";
            this.labelEdit.Size = new System.Drawing.Size(340, 26);
            this.labelEdit.TabIndex = 106;
            this.labelEdit.Text = "     新增";
            this.labelEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserControlProjectList
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Controls.Add(this.PopupContainer);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelEdit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel35);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Name = "UserControlProjectList";
            this.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Size = new System.Drawing.Size(348, 500);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            this.panel35.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainer)).EndInit();
            this.PopupContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tListDesignKind)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textName.Properties)).EndInit();
            this.panelEdit.ResumeLayout(false);
            this.panelName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textPZWH.Properties)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panelKind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditKind2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit2.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        public ProjectManager PM
        {
            get { return DBServiceFactory<ProjectManager>.Service; }
        }
        private void InitialKindList()
        {
            try
            {
              
                    TreeListNode node = null;
                    TreeListNode parentNode = null;
                    TreeListNode node3 = null;
                    TreeListNode node4 = null;
                    try
                    {
                        if (this.tListDesignKind.Nodes.Count > 0)
                        {
                            this.tListDesignKind.Nodes.Clear();
                        }
                     
                    }
                    catch (Exception)
                    {
                    }
                    this.tListDesignKind.OptionsView.ShowRoot = true;
                    this.tListDesignKind.SelectImageList = null;
                    this.tListDesignKind.OptionsView.ShowButtons = true;
                    this.tListDesignKind.TreeLineStyle = LineStyle.None;
                    this.tListDesignKind.RowHeight = 20;
                    this.tListDesignKind.OptionsBehavior.AutoPopulateColumns = true;
                   string str = "";
                    IList<T_DESIGNKIND_Mid> dkLst = PM.FindTreeByKindCode(this.mKindCode);
                    for (int i = 0; i < dkLst.Count; i++)
                    {
                        this.mSelected = true;
                        try
                        {
                            node3 = this.tListDesignKind.AppendNode(dkLst[i].name, node4);
                        }
                        catch (Exception)
                        {
                            node3 = this.tListDesignKind.AppendNode(dkLst[i].name, node4);
                        }
                        node3.SetValue(0, dkLst[i].name);
                        node3.Tag = dkLst[i].code;
                        node3.ImageIndex = -1;
                        node3.StateImageIndex = 0;
                        node3.SelectImageIndex = -1;
                        node3.ExpandAll();
                        str = dkLst[i].code.Substring(0, 2);
                         string str2 = "";
                        IList<T_DESIGNKIND_Mid> dkLst2 = dkLst[i].SubList;
                        for (int k = 0; k < dkLst2.Count; k++)
                        {
                            parentNode = this.tListDesignKind.AppendNode(dkLst2[k].name, node3);
                            parentNode.ImageIndex = -1;
                            parentNode.StateImageIndex = 0;
                            parentNode.SelectImageIndex = -1;
                            parentNode.SetValue(0, dkLst2[k].name);
                            parentNode.Tag = dkLst2[k].code;
                            parentNode.Expanded = false;
                            str2 = dkLst2[k].code.Substring(0, 4);
                            IList<T_DESIGNKIND_Mid> dkLst3 = dkLst2[k].SubList;                  
                           for (int m = 0; m < dkLst3.Count; m++)
                            {
                                node = this.tListDesignKind.AppendNode(dkLst3[m].name, parentNode);
                                node.ImageIndex = -1;
                                node.StateImageIndex = 0;
                                node.SelectImageIndex = -1;
                                node.SetValue(0, dkLst3[m].name);
                                node.Tag =dkLst3[m].code;
                                node.Expanded = false;
                            }
                        }
                        node3.ExpandAll();
                        this.tListDesignKind.Selection.Clear();
                        this.tListDesignKind.Refresh();
                        this.tListDesignKind.OptionsSelection.Reset();
                        this.mSelected = false;
                    }
                }
             catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "InitialKindList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
        private DB2GridViewManager DBVM { get { return DBServiceFactory<DB2GridViewManager>.Service; } }
       private void InitialList()
        {
            try
            {
                this.gridControl1.DataSource = m_projList;
                this.gridView1.Columns.Clear();
                DBVM.InitViewColumn("项目名称", "taskname", gridView1);
                this.gridControl1.DataSource = this.m_projList;
                this.gridView1.RefreshData();
                this.gridView1.OptionsBehavior.Editable = false;
                this.gridControl1.Enabled = true;       
                this.labelInfo.Text = "     共计" + this.m_projList.Count + "个项目";
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "InitialList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
       private IList<T_EDITTASK_ZT_Mid> m_projList;
        public void InitialValue(object hook, IFeatureLayer pFeatureLayer)
        {
            try
            {
                this.mHookHelper = new HookHelperClass();
                this.mHookHelper.Hook = hook;
                this.mEditLayer = pFeatureLayer;
                this.mMap = this.mHookHelper.FocusMap;
                string str = "ZZY";
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(str + "GroupName");
                this.mGroupLayer = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, configValue, true) as IGroupLayer;
                this.mPointLayer = EditTask.UnderLayers[0] as IFeatureLayer;
                this.mLineLayer = EditTask.UnderLayers[1] as IFeatureLayer;
                this.mPolyLayer = EditTask.UnderLayers[2] as IFeatureLayer;
                string[] strArray = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName22").Split(new char[] { ',' });
                UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName33").Split(new char[] { ',' });
                if (this.mGroupLayer != null)
                {
                    this.mPointLayer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(this.mGroupLayer, strArray[0], true) as IFeatureLayer;
                    this.mLineLayer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(this.mGroupLayer, strArray[1], true) as IFeatureLayer;
                    this.mPolyLayer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(this.mGroupLayer, strArray[2], true) as IFeatureLayer;
                }
               
                this.mKindCode = "4";
                m_projList = PM.FindByKindCode(mKindCode);
              //  this.mProjectTable = TaskManageClass.GetDataTable("select * from T_EditTask_ZT where (taskkind like '0" + this.mKindCode + "%') ");
                this.InitialKindList();
                this.InitialList();
                this.panelEdit.Visible = false;
                this.textName.Text = "";
                this.textName2.Text = "";
                this.PopupContainer.Width = this.PopupContainerEdit1.Width;
                this.PopupContainerEdit1.Properties.PopupControl = this.PopupContainer;
                this.labelInfo2.Visible = false;
                if (EditTask.TaskID != 0L)
                {
                    var lst=m_projList.Where(p => p.ID == EditTask.TaskID);
                    foreach(var pj in lst)
                    {
                        OpenProject(pj);
                        break;
                    }                 
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void PopupContainerEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void PopupContainerEdit1_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            this.curPopupEdit = this.PopupContainerEdit1;
            this.PopupContainerEdit1.Properties.PopupControl = this.PopupContainer;
        }

        private void popupContainerEdit2_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void popupContainerEdit2_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            this.curPopupEdit = this.popupContainerEdit2;
            this.popupContainerEdit2.Properties.PopupControl = this.PopupContainer;
        }

        private void popupContainerEdit2_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void SetLayerFilter(bool flag)
        {
            try
            {
                if (flag)
                {
                    string str = "";
                    IFeatureLayerDefinition mPointLayer = null;
                    str = "(XMBH = '" + EditTask.TaskID + "')";
                    mPointLayer = (IFeatureLayerDefinition) this.mPointLayer;
                    mPointLayer.DefinitionExpression = str;
                    mPointLayer = (IFeatureLayerDefinition) this.mLineLayer;
                    mPointLayer.DefinitionExpression = str;
                    mPointLayer = (IFeatureLayerDefinition) this.mPolyLayer;
                    mPointLayer.DefinitionExpression = str;
                    this.mHookHelper.ActiveView.Refresh();
                    str = "(XMBH <> '" + EditTask.TaskID + "')";
                    mPointLayer = (IFeatureLayerDefinition) this.mPointLayer2;
                    mPointLayer.DefinitionExpression = str;
                    mPointLayer = (IFeatureLayerDefinition) this.mLineLayer2;
                    mPointLayer.DefinitionExpression = str;
                    mPointLayer = (IFeatureLayerDefinition) this.mPolyLayer2;
                    mPointLayer.DefinitionExpression = str;
                    this.mHookHelper.ActiveView.Refresh();
                }
                else
                {
                    string str2 = "";
                    IFeatureLayerDefinition mLineLayer = null;
                    str2 = "XMBH = '" + EditTask.TaskID + "'";
                    mLineLayer = (IFeatureLayerDefinition) this.mPointLayer;
                    mLineLayer.DefinitionExpression = str2;
                    mLineLayer = (IFeatureLayerDefinition) this.mLineLayer;
                    mLineLayer.DefinitionExpression = str2;
                    mLineLayer = (IFeatureLayerDefinition) this.mPolyLayer;
                    mLineLayer.DefinitionExpression = str2;
                    this.mHookHelper.ActiveView.Refresh();
                    str2 = "";
                    mLineLayer = (IFeatureLayerDefinition) this.mPointLayer2;
                    mLineLayer.DefinitionExpression = str2;
                    mLineLayer = (IFeatureLayerDefinition) this.mLineLayer2;
                    mLineLayer.DefinitionExpression = str2;
                    mLineLayer = (IFeatureLayerDefinition) this.mPolyLayer2;
                    mLineLayer.DefinitionExpression = str2;
                    this.mHookHelper.ActiveView.Refresh();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "SetLayerFilter", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonAddNew_Click(object sender, EventArgs e)
        {
            this.panelKind.Enabled = true;
            this.panelEdit.Visible = true;
            this.labelInfo2.Visible = false;
            this.labelEdit.Text = "    新增";
            this.textName2.Text = "";
            this.textPZWH.Text = "";
            this.popupContainerEdit2.Text = "";
            this.simpleButtonEditOK.Enabled = false;
            this.simpleButtonOpen.Enabled = false;
        }

        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.GetFocusedDataSourceRowIndex() != -1)
                {
                    T_EDITTASK_ZT_Mid mid=m_projList[this.gridView1.GetFocusedDataSourceRowIndex()];
                    if (MessageBox.Show("确定删除征占用项目【" + mid.taskname + "】,相关征占用班块都将被删除。", "删除征占用项目", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                    {
       
                        bool flag = PM.Delete(mid.ID);
                        this.DeleteEditLayerFeature(mid.ID.ToString());
                        if (flag)
                        {
                            this.FindProject();
                            this.labelInfo.Text = "    删除成功";
                            this.labelInfo.Visible = true;
                        }
                        else
                        {
                            this.labelInfo.Text = "    删除失败";
                            this.labelInfo.Visible = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "simpleButtonDelete_Click", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.gridView1.RowCount == 0) && (this.m_projList.Count == 0))
                {
                    bool flag = false;
                    IFeatureLayer layer = EditTask.UnderLayers[2] as IFeatureLayer;
                    if (layer.FeatureClass.FeatureCount(null) > 0)
                    {
                        if (MessageBox.Show("确定清空所有征占用图斑?", "删除征占用项目", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                        flag = true;
                    }
                    else
                    {
                        layer = EditTask.UnderLayers[0] as IFeatureLayer;
                        if (layer.FeatureClass.FeatureCount(null) > 0)
                        {
                            if (MessageBox.Show("确定清空所有征占用图斑?", "删除征占用项目", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        this.DeleteEditLayerFeature("");
                        this.FindProject();
                        this.mHookHelper.ActiveView.Refresh();
                    }
                }
                if (MessageBox.Show("确定删除所有征占用项目,相关征占用班块都将被删除。", "删除征占用项目", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                {
                    for (int i = 0; i < this.m_projList.Count; i++)
                    {
                        string taskid = "";
                        taskid = m_projList[i].ID.ToString();
                        string str2 = m_projList[i].taskpath;
                        bool flag=PM.Delete(int.Parse(taskid));
                        this.DeleteEditLayerFeature(taskid);
                        if (flag)
                        {
                            this.FindProject();
                            this.labelInfo.Text = "    删除成功";
                            this.labelInfo.Visible = true;
                        }
                        else
                        {
                            this.labelInfo.Text = "    删除失败";
                            this.labelInfo.Visible = true;
                        }
                    }
                    this.DeleteEditLayerFeature("");
                    this.FindProject();
                    this.mHookHelper.ActiveView.Refresh();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "simpleButtonDeleteAll_Click", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.labelEdit.Text = "    修改";
                int focusedRowHandle = this.gridView1.FocusedRowHandle;
                if (focusedRowHandle != -1)
                {
                    this.popupContainerEdit2.Text = "";
                    this.textName2.Text = "";
                    this.panelKind.Enabled = false;
                    this.panelEdit.Visible = true;
                    this.labelInfo2.Visible = false;
                    T_EDITTASK_ZT_Mid mid=m_projList[focusedRowHandle];
                    string str = mid.taskkind;
                    str = str.Substring(2, str.Length - 2);
                    IList<T_DESIGNKIND_Mid> lst =PM.FindByKindAndCode(str,mKindCode);
                    if (lst.Count > 0)
                    {
                        this.popupContainerEdit2.Text = lst[0].name;
                        this.textName2.Text = mid.taskname;
                        this.textPZWH.Text = mid.taskpath;
                    }
                    this.simpleButtonEditOK.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlProjectList", "simpleButtonEdit_Click", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonEditCancle_Click(object sender, EventArgs e)
        {
            this.panelEdit.Visible = false;
            this.labelInfo2.Visible = false;
            if ((m_projList.Count > 0) && (this.gridView1.SelectedRowsCount > -1))
            {
                this.simpleButtonOpen.Enabled = true;
            }
        }

        private void simpleButtonEditOK_Click(object sender, EventArgs e)
        {
            if (this.labelEdit.Text.Contains("新增"))
            {
                if (this.CreateProject())
                {
                    this.FindProject();
                    this.labelInfo2.Text = "    创建成功";
                    this.labelInfo2.Visible = true;
                    int num = 0;
                    for (int i = 0; i < 0x186a0; i++)
                    {
                        num++;
                    }
                    this.panelEdit.Visible = false;
                    this.labelInfo2.Visible = false;
                }
                else
                {
                    this.labelInfo2.Text = "    创建失败";
                    this.labelInfo2.Visible = true;
                }
            }
            else if (this.labelEdit.Text.Contains("修改"))
            {
                int focusedRowHandle = this.gridView1.FocusedRowHandle;
                if (focusedRowHandle != -1)
                {
                    T_EDITTASK_ZT_Mid mid = m_projList[focusedRowHandle];
                    int id = mid.ID;
                    mid.taskname=this.textName2.Text.Trim();
                     mid.taskpath=this.textPZWH.Text.Trim();
                    bool flag=PM.TaskService.Edit(mid);
                   
                    if (flag)
                    {
                        this.FindProject();
                        this.labelInfo2.Text = "    修改成功";
                        this.labelInfo2.Visible = true;
                        int num4 = 0;
                        for (int j = 0; j < 0x186a0; j++)
                        {
                            num4++;
                        }
                        this.panelEdit.Visible = false;
                        this.labelInfo2.Visible = false;
                    }
                    else
                    {
                        this.labelInfo2.Text = "    修改失败";
                        this.labelInfo2.Visible = true;
                    }
                }
            }
        }

        private void simpleButtonOpen_Click(object sender, EventArgs e)
        {
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.textName.Text = "";
            this.textName.Tag = null;
            this.PopupContainerEdit1.Text = "";
            this.dateEdit1.Text = "";
            this.dateEdit2.Text = "";
        }

        private void tListDesignKind_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node != null)
            {
                this.curPopupEdit.Text = e.Node.GetDisplayText(0);
                this.curPopupEdit.Tag = e.Node.Tag;
                this.curPopupEdit.Refresh();
                if (this.curPopupEdit.Name == "PopupContainerEdit1")
                {
                    this.PopupContainerEdit1.Text = e.Node.GetDisplayText(0);
                    this.PopupContainerEdit1.Tag = e.Node.Tag;
                }
                if (this.curPopupEdit.Name == "popupContainerEdit2")
                {
                    if (e.Node.GetDisplayText(0).Trim() == "")
                    {
                        this.simpleButtonEditOK.Enabled = false;
                        this.textName2.Text = "";
                    }
                    else
                    {
                        this.simpleButtonEditOK.Enabled = true;
                        this.textName2.Text = EditTask.TaskYear + "年" + this.curPopupEdit.Text + "项目";
                        this.popupContainerEdit2.Text = e.Node.GetDisplayText(0);
                        this.popupContainerEdit2.Tag = e.Node.Tag;
                    }
                }
            }
        }

        private void tListDesignKind_MouseUp(object sender, MouseEventArgs e)
        {
            this.curPopupEdit.ClosePopup();
            this.Refresh();
        }
    }
}

