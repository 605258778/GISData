namespace TaskManage
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraNavBar;
    using DevExpress.XtraNavBar.ViewInfo;
    using ESRI.ArcGIS.Controls;
    using FormBase;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Utilities;

    public class UserControlTaskOpen2 : UserControlBase1
    {
        private ComboBoxEdit comboBoxEdit1;
        private IContainer components;
        private DateEdit dateEdit1;
        private DateEdit dateEdit2;
        private GroupControl groupControl1;
        private GroupControl groupControl2;
        private GroupControl groupControl3;
        private ImageList imageList1;
        private ImageList imageList2;
        public ImageListBoxControl imageListBoxControl1;
        public ImageListBoxControl imageListBoxControl10;
        public ImageListBoxControl imageListBoxControl2;
        public ImageListBoxControl imageListBoxControl3;
        private ImageListBoxControl imageListBoxControl4;
        public ImageListBoxControl imageListBoxControl5;
        public ImageListBoxControl imageListBoxControl6;
        public ImageListBoxControl imageListBoxControl7;
        public ImageListBoxControl imageListBoxControl8;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label8;
        private const string mClassName = "TaskManage.UserControlTaskOpen2";
       
        private string mEditKind = "小班";
        private string mEditKindCode = "";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private DataTable mFieldTable;
        private HookHelper mHookHelper;
        private string mKindCode = "";
        private DataTable mKindTable;
        private string mLayerName;
        public DataRow mRow;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        public DataTable mTable;
        public DataTable mTable2;
        public DataTable mTable3;
        public DataTable mTable4;
        private DataTable mTaskTable;
        private const string myClassName = "任务打开";
        private NavBarControl navBarControl1;
        private NavBarGroup navBarGroup1;
        private NavBarGroup navBarGroup2;
        private NavBarGroup navBarGroup3;
        private NavBarGroup navBarGroup4;
        private NavBarGroup navBarGroup5;
        private NavBarGroup navBarGroup6;
        private NavBarGroup navBarGroup7;
        private NavBarGroup navBarGroup8;
        private NavBarGroupControlContainer navBarGroupControlContainer1;
        private NavBarGroupControlContainer navBarGroupControlContainer2;
        private NavBarGroupControlContainer navBarGroupControlContainer3;
        private NavBarGroupControlContainer navBarGroupControlContainer4;
        private NavBarGroupControlContainer navBarGroupControlContainer5;
        private NavBarGroupControlContainer navBarGroupControlContainer6;
        private NavBarGroupControlContainer navBarGroupControlContainer7;
        private NavBarGroupControlContainer navBarGroupControlContainer8;
        private Panel panel1;
        private Panel panel13;
        private Panel panel14;
        private Panel panel2;
        private Panel panel3;
        private SimpleButton simpleButtonFind;
        private TextEdit txtTaskName;

        public UserControlTaskOpen2()
        {
            this.InitializeComponent();
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
                this.mEditKind = sEditKind;
                if (hook != null)
                {
                    this.mHookHelper = new HookHelperClass();
                    this.mHookHelper.Hook = hook;
                    this.InitialValue();
                    this.InitialControl();
                }
            }
            catch (Exception)
            {
            }
        }

        private void InitialControl()
        {
            try
            {
                this.dateEdit1.Text = DateTime.Now.AddDays(-30.0).Date.ToString();
                this.dateEdit2.Text = DateTime.Now.ToShortDateString() + " 23:59:59";
                this.imageListBoxControl1.Items.Clear();
                this.mTable = TaskManageClass.GetDataTable("T_EditTask", "*", "edittime desc", "taskkind like '0" + this.mKindCode + "%' and cdate(createtime)>#" + this.dateEdit1.Text + "# and cdate(edittime)<#" + this.dateEdit2.Text + "#", false);
                for (int i = 0; i < this.mTable.Rows.Count; i++)
                {
                    if (i == 3)
                    {
                        break;
                    }
                    this.imageListBoxControl1.Items.Add(this.mTable.Rows[i]["taskname"], 7);
                }
                this.imageListBoxControl2.Items.Clear();
                this.mTable2 = TaskManageClass.GetDataTable( "T_EditTask", "*", "createtime desc", "taskkind like '0" + this.mKindCode + "%'", false);
                for (int j = 0; j < this.mTable2.Rows.Count; j++)
                {
                    if (j == 3)
                    {
                        break;
                    }
                    this.imageListBoxControl2.Items.Add(this.mTable2.Rows[j]["taskname"], 8);
                }
                this.imageListBoxControl3.Items.Clear();
                this.mTable3 = TaskManageClass.GetDataTable( "T_EditTask", "*", "", "taskkind like '0" + this.mKindCode + "%'", false);
                for (int k = 0; k < this.mTable3.Rows.Count; k++)
                {
                    this.imageListBoxControl3.Items.Add(this.mTable3.Rows[k]["taskname"], 9);
                }
                this.imageListBoxControl5.Items.Clear();
                this.mTable4 = TaskManageClass.GetDataTable( "T_EditTask", "*", "", "taskkind like '0" + this.mKindCode + "%' and taskstate='5'", false);
                for (int m = 0; m < this.mTable4.Rows.Count; m++)
                {
                    this.imageListBoxControl5.Items.Add(this.mTable4.Rows[m]["taskname"], 9);
                }
            }
            catch (Exception)
            {
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTaskOpen2));
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.imageListBoxControl2 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.imageListBoxControl1 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.imageListBoxControl4 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageListBoxControl3 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.txtTaskName = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.simpleButtonFind = new DevExpress.XtraEditors.SimpleButton();
            this.imageListBoxControl5 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.label5 = new System.Windows.Forms.Label();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer3 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer4 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navBarGroupControlContainer5 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.imageListBoxControl7 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.navBarGroupControlContainer6 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.imageListBoxControl8 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.navBarGroupControlContainer7 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.imageListBoxControl10 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.navBarGroupControlContainer8 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.imageListBoxControl6 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup8 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup5 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup6 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup4 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup7 = new DevExpress.XtraNavBar.NavBarGroup();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl3)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            this.panel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.navBarControl1.SuspendLayout();
            this.navBarGroupControlContainer3.SuspendLayout();
            this.navBarGroupControlContainer1.SuspendLayout();
            this.navBarGroupControlContainer2.SuspendLayout();
            this.navBarGroupControlContainer4.SuspendLayout();
            this.navBarGroupControlContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl7)).BeginInit();
            this.navBarGroupControlContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl8)).BeginInit();
            this.navBarGroupControlContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl10)).BeginInit();
            this.navBarGroupControlContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl6)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.imageListBoxControl2);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.ShowCaption = false;
            this.groupControl3.Size = new System.Drawing.Size(348, 366);
            this.groupControl3.TabIndex = 16;
            this.groupControl3.Text = "打开最新的编辑任务";
            // 
            // imageListBoxControl2
            // 
            this.imageListBoxControl2.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl2.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl2.ImageList = this.imageList1;
            this.imageListBoxControl2.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 49),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 49)});
            this.imageListBoxControl2.Location = new System.Drawing.Point(2, 2);
            this.imageListBoxControl2.Name = "imageListBoxControl2";
            this.imageListBoxControl2.Size = new System.Drawing.Size(344, 362);
            this.imageListBoxControl2.TabIndex = 5;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "allpanel.ico");
            this.imageList1.Images.SetKeyName(1, "blue_gallerylink_24x24.gif");
            this.imageList1.Images.SetKeyName(2, "blue_view_24x24.gif");
            this.imageList1.Images.SetKeyName(3, "emblem-web_24x24.png");
            this.imageList1.Images.SetKeyName(4, "Windows.png");
            this.imageList1.Images.SetKeyName(5, "Red%20tag.png");
            this.imageList1.Images.SetKeyName(6, "ksirtet24.png");
            this.imageList1.Images.SetKeyName(7, "tag_purple_edit.png");
            this.imageList1.Images.SetKeyName(8, "tag_yellow_edit.png");
            this.imageList1.Images.SetKeyName(9, "tags_edit.png");
            this.imageList1.Images.SetKeyName(10, "page_white_world.png");
            this.imageList1.Images.SetKeyName(11, "map3-24.png");
            this.imageList1.Images.SetKeyName(12, "ATLConsumer.ico");
            this.imageList1.Images.SetKeyName(13, "CDImage.ico");
            this.imageList1.Images.SetKeyName(14, "cdproject.ico");
            this.imageList1.Images.SetKeyName(15, "CommandItemInfoEditor.ico");
            this.imageList1.Images.SetKeyName(16, "ContSample.ico");
            this.imageList1.Images.SetKeyName(17, "POINT14.ICO");
            this.imageList1.Images.SetKeyName(18, "TabWiz.ico");
            this.imageList1.Images.SetKeyName(19, "legend.png");
            this.imageList1.Images.SetKeyName(20, "select_by_color.png");
            this.imageList1.Images.SetKeyName(21, "storage.png");
            this.imageList1.Images.SetKeyName(22, "tag_blue_add.png");
            this.imageList1.Images.SetKeyName(23, "tag_blue_edit.png");
            this.imageList1.Images.SetKeyName(24, "tag_green.png");
            this.imageList1.Images.SetKeyName(25, "tag_purple.png");
            this.imageList1.Images.SetKeyName(26, "widgets.png");
            this.imageList1.Images.SetKeyName(27, "zoom.png");
            this.imageList1.Images.SetKeyName(28, "accept.png");
            this.imageList1.Images.SetKeyName(29, "add.png");
            this.imageList1.Images.SetKeyName(30, "notebook.png");
            this.imageList1.Images.SetKeyName(31, "notebook-accept.png");
            this.imageList1.Images.SetKeyName(32, "notebook-edit.png");
            this.imageList1.Images.SetKeyName(33, "tag_purple.png");
            this.imageList1.Images.SetKeyName(34, "tag_red.png");
            this.imageList1.Images.SetKeyName(35, "tag_yellow.png");
            this.imageList1.Images.SetKeyName(36, "tag-blue.png");
            this.imageList1.Images.SetKeyName(37, "tag-blue_edit.png");
            this.imageList1.Images.SetKeyName(38, "Tags.png");
            this.imageList1.Images.SetKeyName(39, "Tools-.png");
            this.imageList1.Images.SetKeyName(40, "three_tags.png");
            this.imageList1.Images.SetKeyName(41, "Red%20tag.png");
            this.imageList1.Images.SetKeyName(42, "Green%20tag.png");
            this.imageList1.Images.SetKeyName(43, "Blue%20tag.png");
            this.imageList1.Images.SetKeyName(44, "001_34.gif");
            this.imageList1.Images.SetKeyName(45, "001_36.gif");
            this.imageList1.Images.SetKeyName(46, "rss2.png");
            this.imageList1.Images.SetKeyName(47, "star.png");
            this.imageList1.Images.SetKeyName(48, "notebook-star.png");
            this.imageList1.Images.SetKeyName(49, "list-star.png");
            this.imageList1.Images.SetKeyName(50, "desktop.png");
            this.imageList1.Images.SetKeyName(51, "history.png");
            this.imageList1.Images.SetKeyName(52, "Black%20tag.png");
            this.imageList1.Images.SetKeyName(53, "Yellow%20tag.png");
            this.imageList1.Images.SetKeyName(54, "Application.png");
            this.imageList1.Images.SetKeyName(55, "Clock.png");
            this.imageList1.Images.SetKeyName(56, "Diagram.png");
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.imageListBoxControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.ShowCaption = false;
            this.groupControl2.Size = new System.Drawing.Size(348, 366);
            this.groupControl2.TabIndex = 15;
            this.groupControl2.Text = "打开最近的编辑任务";
            // 
            // imageListBoxControl1
            // 
            this.imageListBoxControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl1.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl1.ImageList = this.imageList1;
            this.imageListBoxControl1.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务\\编辑任务1", 7),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务\\编辑任务2", 7),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务", 7)});
            this.imageListBoxControl1.Location = new System.Drawing.Point(2, 2);
            this.imageListBoxControl1.Name = "imageListBoxControl1";
            this.imageListBoxControl1.Size = new System.Drawing.Size(344, 362);
            this.imageListBoxControl1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(350, 9);
            this.label3.TabIndex = 17;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(350, 9);
            this.label4.TabIndex = 18;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.imageListBoxControl4);
            this.groupControl1.Controls.Add(this.imageListBoxControl3);
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(342, 437);
            this.groupControl1.TabIndex = 19;
            this.groupControl1.Text = "打开指定编辑任务";
            // 
            // imageListBoxControl4
            // 
            this.imageListBoxControl4.ImageList = this.imageList2;
            this.imageListBoxControl4.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem(null, 11),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem(null, 1)});
            this.imageListBoxControl4.Location = new System.Drawing.Point(170, 121);
            this.imageListBoxControl4.Name = "imageListBoxControl4";
            this.imageListBoxControl4.Size = new System.Drawing.Size(140, 111);
            this.imageListBoxControl4.TabIndex = 7;
            this.imageListBoxControl4.Visible = false;
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
            // 
            // imageListBoxControl3
            // 
            this.imageListBoxControl3.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl3.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl3.ImageList = this.imageList1;
            this.imageListBoxControl3.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 9),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 9)});
            this.imageListBoxControl3.Location = new System.Drawing.Point(2, 124);
            this.imageListBoxControl3.Name = "imageListBoxControl3";
            this.imageListBoxControl3.Size = new System.Drawing.Size(338, 311);
            this.imageListBoxControl3.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel13);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 122);
            this.panel1.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.comboBoxEdit1);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 70);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panel3.Size = new System.Drawing.Size(338, 35);
            this.panel3.TabIndex = 85;
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEdit1.EditValue = "--";
            this.comboBoxEdit1.Location = new System.Drawing.Point(42, 5);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "--",
            "新建",
            "编辑",
            "可申报",
            "已申报",
            "反馈修改",
            "审批通过"});
            this.comboBoxEdit1.Size = new System.Drawing.Size(296, 20);
            this.comboBoxEdit1.TabIndex = 86;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.ImageList = this.imageList1;
            this.label6.Location = new System.Drawing.Point(0, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 25);
            this.label6.TabIndex = 85;
            this.label6.Text = "状态:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.dateEdit2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dateEdit1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panel2.Size = new System.Drawing.Size(338, 35);
            this.panel2.TabIndex = 84;
            // 
            // dateEdit2
            // 
            this.dateEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateEdit2.EditValue = null;
            this.dateEdit2.Location = new System.Drawing.Point(190, 5);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit2.Size = new System.Drawing.Size(148, 20);
            this.dateEdit2.TabIndex = 90;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.ImageList = this.imageList1;
            this.label2.Location = new System.Drawing.Point(164, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 25);
            this.label2.TabIndex = 86;
            this.label2.Text = "至";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateEdit1
            // 
            this.dateEdit1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(42, 5);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit1.Size = new System.Drawing.Size(122, 20);
            this.dateEdit1.TabIndex = 89;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.ImageList = this.imageList1;
            this.label1.Location = new System.Drawing.Point(0, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 25);
            this.label1.TabIndex = 85;
            this.label1.Text = "时间:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Transparent;
            this.panel13.Controls.Add(this.txtTaskName);
            this.panel13.Controls.Add(this.label8);
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Controls.Add(this.simpleButtonFind);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Name = "panel13";
            this.panel13.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panel13.Size = new System.Drawing.Size(338, 35);
            this.panel13.TabIndex = 83;
            // 
            // txtTaskName
            // 
            this.txtTaskName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTaskName.Location = new System.Drawing.Point(42, 5);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(263, 20);
            this.txtTaskName.TabIndex = 78;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.ImageList = this.imageList1;
            this.label8.Location = new System.Drawing.Point(0, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 25);
            this.label8.TabIndex = 85;
            this.label8.Text = "名称:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel14.Location = new System.Drawing.Point(305, 5);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(7, 25);
            this.panel14.TabIndex = 83;
            // 
            // simpleButtonFind
            // 
            this.simpleButtonFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonFind.Image")));
            this.simpleButtonFind.ImageIndex = 2;
            this.simpleButtonFind.ImageList = this.imageList2;
            this.simpleButtonFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonFind.Location = new System.Drawing.Point(312, 5);
            this.simpleButtonFind.Name = "simpleButtonFind";
            this.simpleButtonFind.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonFind.TabIndex = 79;
            this.simpleButtonFind.ToolTip = "查找";
            this.simpleButtonFind.Click += new System.EventHandler(this.simpleButtonFind_Click);
            // 
            // imageListBoxControl5
            // 
            this.imageListBoxControl5.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl5.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl5.ImageList = this.imageList1;
            this.imageListBoxControl5.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务\\编辑任务1", 34),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务\\编辑任务2", 34),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务", 34)});
            this.imageListBoxControl5.Location = new System.Drawing.Point(0, 0);
            this.imageListBoxControl5.Name = "imageListBoxControl5";
            this.imageListBoxControl5.Size = new System.Drawing.Size(348, 366);
            this.imageListBoxControl5.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(350, 9);
            this.label5.TabIndex = 20;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Visible = false;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup3;
            this.navBarControl1.ContentButtonHint = null;
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer2);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer3);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer4);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer5);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer6);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer7);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer8);
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1,
            this.navBarGroup2,
            this.navBarGroup3,
            this.navBarGroup8,
            this.navBarGroup5,
            this.navBarGroup6,
            this.navBarGroup4,
            this.navBarGroup7});
            this.navBarControl1.LargeImages = this.imageList1;
            this.navBarControl1.Location = new System.Drawing.Point(0, 27);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.NavigationPaneOverflowPanelUseSmallImages = false;
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 269;
            this.navBarControl1.Size = new System.Drawing.Size(350, 452);
            this.navBarControl1.SkinExplorerBarViewScrollStyle = DevExpress.XtraNavBar.SkinExplorerBarViewScrollStyle.Buttons;
            this.navBarControl1.SmallImages = this.imageList2;
            this.navBarControl1.StoreDefaultPaintStyleName = true;
            this.navBarControl1.TabIndex = 21;
            this.navBarControl1.Text = "navBarControl1";
            this.navBarControl1.Click += new System.EventHandler(this.navBarControl1_Click);
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "打开指定作业设计";
            this.navBarGroup3.ControlContainer = this.navBarGroupControlContainer3;
            this.navBarGroup3.Expanded = true;
            this.navBarGroup3.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup3.GroupClientHeight = 441;
            this.navBarGroup3.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup3.LargeImageIndex = 40;
            this.navBarGroup3.Name = "navBarGroup3";
            this.navBarGroup3.SmallImageIndex = 9;
            // 
            // navBarGroupControlContainer3
            // 
            this.navBarGroupControlContainer3.Controls.Add(this.groupControl1);
            this.navBarGroupControlContainer3.Name = "navBarGroupControlContainer3";
            this.navBarGroupControlContainer3.Size = new System.Drawing.Size(342, 437);
            this.navBarGroupControlContainer3.TabIndex = 2;
            // 
            // navBarGroupControlContainer1
            // 
            this.navBarGroupControlContainer1.Controls.Add(this.groupControl2);
            this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
            this.navBarGroupControlContainer1.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer1.TabIndex = 0;
            // 
            // navBarGroupControlContainer2
            // 
            this.navBarGroupControlContainer2.Controls.Add(this.groupControl3);
            this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
            this.navBarGroupControlContainer2.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer2.TabIndex = 1;
            // 
            // navBarGroupControlContainer4
            // 
            this.navBarGroupControlContainer4.Controls.Add(this.imageListBoxControl5);
            this.navBarGroupControlContainer4.Name = "navBarGroupControlContainer4";
            this.navBarGroupControlContainer4.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer4.TabIndex = 3;
            // 
            // navBarGroupControlContainer5
            // 
            this.navBarGroupControlContainer5.Controls.Add(this.imageListBoxControl7);
            this.navBarGroupControlContainer5.Name = "navBarGroupControlContainer5";
            this.navBarGroupControlContainer5.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer5.TabIndex = 4;
            // 
            // imageListBoxControl7
            // 
            this.imageListBoxControl7.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl7.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl7.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl7.ImageList = this.imageList1;
            this.imageListBoxControl7.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 37),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 37)});
            this.imageListBoxControl7.Location = new System.Drawing.Point(0, 0);
            this.imageListBoxControl7.Name = "imageListBoxControl7";
            this.imageListBoxControl7.Size = new System.Drawing.Size(348, 366);
            this.imageListBoxControl7.TabIndex = 6;
            // 
            // navBarGroupControlContainer6
            // 
            this.navBarGroupControlContainer6.Controls.Add(this.imageListBoxControl8);
            this.navBarGroupControlContainer6.Name = "navBarGroupControlContainer6";
            this.navBarGroupControlContainer6.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer6.TabIndex = 5;
            // 
            // imageListBoxControl8
            // 
            this.imageListBoxControl8.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl8.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl8.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl8.ImageList = this.imageList1;
            this.imageListBoxControl8.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 50),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 50)});
            this.imageListBoxControl8.Location = new System.Drawing.Point(0, 0);
            this.imageListBoxControl8.Name = "imageListBoxControl8";
            this.imageListBoxControl8.Size = new System.Drawing.Size(348, 366);
            this.imageListBoxControl8.TabIndex = 6;
            // 
            // navBarGroupControlContainer7
            // 
            this.navBarGroupControlContainer7.Controls.Add(this.imageListBoxControl10);
            this.navBarGroupControlContainer7.Name = "navBarGroupControlContainer7";
            this.navBarGroupControlContainer7.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer7.TabIndex = 6;
            // 
            // imageListBoxControl10
            // 
            this.imageListBoxControl10.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl10.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl10.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl10.ImageList = this.imageList1;
            this.imageListBoxControl10.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 24),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 24)});
            this.imageListBoxControl10.Location = new System.Drawing.Point(0, 0);
            this.imageListBoxControl10.Name = "imageListBoxControl10";
            this.imageListBoxControl10.Size = new System.Drawing.Size(348, 366);
            this.imageListBoxControl10.TabIndex = 6;
            // 
            // navBarGroupControlContainer8
            // 
            this.navBarGroupControlContainer8.Controls.Add(this.imageListBoxControl6);
            this.navBarGroupControlContainer8.Name = "navBarGroupControlContainer8";
            this.navBarGroupControlContainer8.Size = new System.Drawing.Size(348, 366);
            this.navBarGroupControlContainer8.TabIndex = 7;
            // 
            // imageListBoxControl6
            // 
            this.imageListBoxControl6.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl6.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl6.ImageList = this.imageList1;
            this.imageListBoxControl6.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 8),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 8)});
            this.imageListBoxControl6.Location = new System.Drawing.Point(0, 0);
            this.imageListBoxControl6.Name = "imageListBoxControl6";
            this.imageListBoxControl6.Size = new System.Drawing.Size(348, 366);
            this.imageListBoxControl6.TabIndex = 6;
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "打开最近的作业设计";
            this.navBarGroup1.ControlContainer = this.navBarGroupControlContainer1;
            this.navBarGroup1.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup1.GroupClientHeight = 366;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup1.LargeImageIndex = 51;
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.SmallImageIndex = 7;
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "打开最新的作业设计";
            this.navBarGroup2.ControlContainer = this.navBarGroupControlContainer2;
            this.navBarGroup2.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup2.GroupClientHeight = 366;
            this.navBarGroup2.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup2.LargeImageIndex = 48;
            this.navBarGroup2.Name = "navBarGroup2";
            this.navBarGroup2.SmallImageIndex = 8;
            // 
            // navBarGroup8
            // 
            this.navBarGroup8.Caption = "正在编辑的作业设计";
            this.navBarGroup8.ControlContainer = this.navBarGroupControlContainer8;
            this.navBarGroup8.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup8.GroupClientHeight = 366;
            this.navBarGroup8.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup8.LargeImageIndex = 53;
            this.navBarGroup8.Name = "navBarGroup8";
            // 
            // navBarGroup5
            // 
            this.navBarGroup5.Caption = "校验通过的作业设计";
            this.navBarGroup5.ControlContainer = this.navBarGroupControlContainer5;
            this.navBarGroup5.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup5.GroupClientHeight = 366;
            this.navBarGroup5.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup5.LargeImageIndex = 43;
            this.navBarGroup5.Name = "navBarGroup5";
            // 
            // navBarGroup6
            // 
            this.navBarGroup6.Caption = "申请上报的作业设计";
            this.navBarGroup6.ControlContainer = this.navBarGroupControlContainer6;
            this.navBarGroup6.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup6.GroupClientHeight = 366;
            this.navBarGroup6.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup6.LargeImageIndex = 44;
            this.navBarGroup6.Name = "navBarGroup6";
            // 
            // navBarGroup4
            // 
            this.navBarGroup4.Caption = "未通过审批的作业设计";
            this.navBarGroup4.ControlContainer = this.navBarGroupControlContainer4;
            this.navBarGroup4.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup4.GroupClientHeight = 366;
            this.navBarGroup4.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup4.LargeImageIndex = 41;
            this.navBarGroup4.Name = "navBarGroup4";
            this.navBarGroup4.SmallImageIndex = 1;
            // 
            // navBarGroup7
            // 
            this.navBarGroup7.Caption = "审批通过的作业设计";
            this.navBarGroup7.ControlContainer = this.navBarGroupControlContainer7;
            this.navBarGroup7.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            this.navBarGroup7.GroupClientHeight = 366;
            this.navBarGroup7.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup7.LargeImageIndex = 42;
            this.navBarGroup7.Name = "navBarGroup7";
            // 
            // UserControlTaskOpen2
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Controls.Add(this.navBarControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "UserControlTaskOpen2";
            this.Size = new System.Drawing.Size(350, 479);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            this.panel13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.navBarControl1.ResumeLayout(false);
            this.navBarGroupControlContainer3.ResumeLayout(false);
            this.navBarGroupControlContainer1.ResumeLayout(false);
            this.navBarGroupControlContainer2.ResumeLayout(false);
            this.navBarGroupControlContainer4.ResumeLayout(false);
            this.navBarGroupControlContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl7)).EndInit();
            this.navBarGroupControlContainer6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl8)).EndInit();
            this.navBarGroupControlContainer7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl10)).EndInit();
            this.navBarGroupControlContainer8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl6)).EndInit();
            this.ResumeLayout(false);

        }

        public void InitialValue()
        {
            try
            {              
                if (this.mEditKind == "造林")
                {
                    this.mKindCode = "1";
                }
                else if (this.mEditKind == "采伐")
                {
                    this.mKindCode = "2";
                }
                else
                {
                    this.mKindCode = "";
                }
                this.mKindTable = TaskManageClass.GetDataTable( "select * from T_DesignKind where kind='" + this.mKindCode + "'");
                this.mTaskTable = TaskManageClass.GetDataTable( "select * from T_EditTask where taskkind like '0" + this.mKindCode + "%'");
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskOpen2", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {
        }

        private void simpleButtonFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.imageListBoxControl3.Items.Clear();
                string whereStr = "";
                whereStr = "taskkind like '0" + this.mKindCode + "%'";
                if (this.txtTaskName.Text != "")
                {
                    if (whereStr != "")
                    {
                        whereStr = whereStr + " and taskname like '%" + this.txtTaskName.Text + "%'";
                    }
                    else
                    {
                        whereStr = "taskname like '" + this.txtTaskName.Text + "%'";
                    }
                }
                if (this.dateEdit1.Text != "")
                {
                    if (whereStr != "")
                    {
                        whereStr = whereStr + " and cdate(createtime)>#" + this.dateEdit1.Text + "#";
                    }
                    else
                    {
                        whereStr = "cdate(createtime)>#" + this.dateEdit1.Text + "#";
                    }
                }
                if (this.dateEdit2.Text != "")
                {
                    if (whereStr != "")
                    {
                        whereStr = whereStr + " and cdate(edittime)<#" + this.dateEdit2.Text + "#";
                    }
                    else
                    {
                        whereStr = "cdate(edittime)<#" + this.dateEdit2.Text + "#";
                    }
                }
                if (this.comboBoxEdit1.Text != "--")
                {
                    if (whereStr != "")
                    {
                        whereStr = string.Concat(new object[] { whereStr, " and taskstate='", this.comboBoxEdit1.SelectedIndex, "'" });
                    }
                    else
                    {
                        whereStr = "taskstate='" + this.comboBoxEdit1.SelectedIndex + "'";
                    }
                }
                this.mTable3 = TaskManageClass.GetDataTable("T_EditTask", "*", "edittime desc", whereStr, false);
                for (int i = 0; i < this.mTable3.Rows.Count; i++)
                {
                    this.imageListBoxControl3.Items.Add(this.mTable3.Rows[i]["taskname"], 9);
                }
            }
            catch (Exception)
            {
            }
        }

        public string LayerName
        {
            get
            {
                return this.mLayerName;
            }
            set
            {
                this.mLayerName = value;
            }
        }
    }
}

