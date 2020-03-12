namespace TaskManage
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using FormBase;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using Utilities;

    public class UserControlTaskOpen : UserControlBase1
    {
        private IContainer components;
        private GroupControl groupControl1;
        private GroupControl groupControl2;
        private GroupControl groupControl3;
        private ImageList imageList1;
        public ImageListBoxControl imageListBoxControl1;
        public ImageListBoxControl imageListBoxControl2;
        private Label label1;
        private Label label2;
        public LinkLabel linkLabel1;
        private string mTaskPath;
        private SimpleButton simpleButton1;
        public SimpleButton simpleButtonOK;

        public UserControlTaskOpen()
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTaskOpen));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.imageListBoxControl2 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.imageListBoxControl1 = new DevExpress.XtraEditors.ImageListBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButtonOK);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.groupControl3);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.groupControl2);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.linkLabel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Padding = new System.Windows.Forms.Padding(4);
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(245, 383);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "打开数据编辑任务";
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonOK.Image")));
            this.simpleButtonOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonOK.Location = new System.Drawing.Point(210, 146);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(22, 22);
            this.simpleButtonOK.TabIndex = 9;
            this.simpleButtonOK.Visible = false;
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButton1.Location = new System.Drawing.Point(182, 142);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(22, 22);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Visible = false;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.imageListBoxControl2);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(6, 259);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(233, 118);
            this.groupControl3.TabIndex = 11;
            this.groupControl3.Text = "系统编辑任务路径";
            // 
            // imageListBoxControl2
            // 
            this.imageListBoxControl2.Appearance.BackColor = System.Drawing.Color.White;
            this.imageListBoxControl2.Appearance.Options.UseBackColor = true;
            this.imageListBoxControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imageListBoxControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControl2.ImageList = this.imageList1;
            this.imageListBoxControl2.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageListBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务1", 9),
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("编辑任务2", 9)});
            this.imageListBoxControl2.Location = new System.Drawing.Point(2, 22);
            this.imageListBoxControl2.Name = "imageListBoxControl2";
            this.imageListBoxControl2.Size = new System.Drawing.Size(229, 94);
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
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(6, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 57);
            this.label2.TabIndex = 5;
            this.label2.Text = "系统任务路径[D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务]";
            this.label2.Visible = false;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.imageListBoxControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(6, 52);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(233, 150);
            this.groupControl2.TabIndex = 10;
            this.groupControl2.Text = "最近的编辑任务";
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
            new DevExpress.XtraEditors.Controls.ImageListBoxItem("D:\\VSS_LC\\LCGISSetUp\\Data\\编辑任务", 9)});
            this.imageListBoxControl1.Location = new System.Drawing.Point(2, 22);
            this.imageListBoxControl1.Name = "imageListBoxControl1";
            this.imageListBoxControl1.Size = new System.Drawing.Size(229, 126);
            this.imageListBoxControl1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "最近的任务";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.BackColor = System.Drawing.Color.White;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.ImageIndex = 1;
            this.linkLabel1.ImageList = this.imageList1;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(7, 13);
            this.linkLabel1.Location = new System.Drawing.Point(6, 6);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(233, 21);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "        打开...";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.UseCompatibleTextRendering = true;
            // 
            // UserControlTaskOpen
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(207)))), ((int)(((byte)(247)))));
            this.Appearance.BackColor2 = System.Drawing.Color.White;
            this.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseBorderColor = true;
            this.Controls.Add(this.groupControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "UserControlTaskOpen";
            this.Size = new System.Drawing.Size(245, 383);
            this.DoubleClick += new System.EventHandler(this.UserControlTaskOpen_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControl1)).EndInit();
            this.ResumeLayout(false);

        }

        public void InitialValue(string mEditKind)
        {
            try
            {
                string name = "";
                if (mEditKind == "小班")
                {
                    name = "EditMapPath";
                }
                else if (mEditKind == "造林")
                {
                    name = "EditMapZLPath";
                }
                else if (mEditKind == "采伐")
                {
                    name = "EditMapCFPath";
                }
                else if (mEditKind == "林改")
                {
                    name = "EditMapLGPath";
                }
                string path = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue(name);
                this.imageListBoxControl1.Items.Clear();
                this.imageListBoxControl1.Items.Add(Directory.GetParent(path).ToString(), 7);
                path = UtilFactory.GetConfigOpt().RootPath + @"\Data\编辑任务";
                this.label2.Text = path;
                this.imageListBoxControl2.Items.Clear();
                string[] directories = Directory.GetDirectories(path);
                for (int i = 0; i < directories.Length; i++)
                {
                    this.imageListBoxControl2.Items.Add(directories[i].ToString(), 9);
                }
            }
            catch (Exception)
            {
            }
        }

        public void simpleButtonOK_Click(object sender, EventArgs e)
        {
        }

        private void UserControlTaskOpen_DoubleClick(object sender, EventArgs e)
        {
        }

        public string TaskPath
        {
            get
            {
                return this.mTaskPath;
            }
            set
            {
                this.mTaskPath = value;
            }
        }
    }
}

