namespace GISData.TaskManage
{
    partial class FormTaskManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.gridControlGLDW = new DevExpress.XtraGrid.GridControl();
            this.gridViewGLDW = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GLDW = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GLDWNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CONTACTS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STARTTIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CHECKLOG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OTHER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlGLDW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewGLDW)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(997, 489);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.splitContainer1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(990, 453);
            this.xtraTabPage1.Text = "管理单位";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridControlGLDW);
            this.splitContainer1.Size = new System.Drawing.Size(990, 453);
            this.splitContainer1.SplitterDistance = 39;
            this.splitContainer1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(909, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "导入";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gridControlGLDW
            // 
            this.gridControlGLDW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlGLDW.Location = new System.Drawing.Point(0, 0);
            this.gridControlGLDW.MainView = this.gridViewGLDW;
            this.gridControlGLDW.Name = "gridControlGLDW";
            this.gridControlGLDW.Size = new System.Drawing.Size(990, 410);
            this.gridControlGLDW.TabIndex = 0;
            this.gridControlGLDW.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewGLDW});
            // 
            // gridViewGLDW
            // 
            this.gridViewGLDW.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.GLDW,
            this.GLDWNAME,
            this.CONTACTS,
            this.TEL,
            this.STARTTIME,
            this.STATE,
            this.CHECKLOG,
            this.OTHER});
            this.gridViewGLDW.GridControl = this.gridControlGLDW;
            this.gridViewGLDW.Name = "gridViewGLDW";
            this.gridViewGLDW.OptionsView.ShowGroupPanel = false;
            // 
            // GLDW
            // 
            this.GLDW.Caption = "管理单位代码";
            this.GLDW.FieldName = "GLDW";
            this.GLDW.MinWidth = 25;
            this.GLDW.Name = "GLDW";
            this.GLDW.Visible = true;
            this.GLDW.VisibleIndex = 0;
            this.GLDW.Width = 94;
            // 
            // GLDWNAME
            // 
            this.GLDWNAME.Caption = "管理单位名称";
            this.GLDWNAME.FieldName = "GLDWNAME";
            this.GLDWNAME.MinWidth = 25;
            this.GLDWNAME.Name = "GLDWNAME";
            this.GLDWNAME.Visible = true;
            this.GLDWNAME.VisibleIndex = 1;
            this.GLDWNAME.Width = 94;
            // 
            // CONTACTS
            // 
            this.CONTACTS.Caption = "联系人";
            this.CONTACTS.FieldName = "CONTACTS";
            this.CONTACTS.MinWidth = 25;
            this.CONTACTS.Name = "CONTACTS";
            this.CONTACTS.Visible = true;
            this.CONTACTS.VisibleIndex = 2;
            this.CONTACTS.Width = 94;
            // 
            // TEL
            // 
            this.TEL.Caption = "联系电话";
            this.TEL.FieldName = "TEL";
            this.TEL.MinWidth = 25;
            this.TEL.Name = "TEL";
            this.TEL.Visible = true;
            this.TEL.VisibleIndex = 3;
            this.TEL.Width = 94;
            // 
            // STARTTIME
            // 
            this.STARTTIME.Caption = "开始检查时间";
            this.STARTTIME.FieldName = "STARTTIME";
            this.STARTTIME.MinWidth = 25;
            this.STARTTIME.Name = "STARTTIME";
            this.STARTTIME.Visible = true;
            this.STARTTIME.VisibleIndex = 4;
            this.STARTTIME.Width = 94;
            // 
            // STATE
            // 
            this.STATE.Caption = "检查状态";
            this.STATE.FieldName = "STATE";
            this.STATE.MinWidth = 25;
            this.STATE.Name = "STATE";
            this.STATE.Visible = true;
            this.STATE.VisibleIndex = 5;
            this.STATE.Width = 94;
            // 
            // CHECKLOG
            // 
            this.CHECKLOG.Caption = "检查日志";
            this.CHECKLOG.FieldName = "CHECKLOG";
            this.CHECKLOG.MinWidth = 25;
            this.CHECKLOG.Name = "CHECKLOG";
            this.CHECKLOG.Visible = true;
            this.CHECKLOG.VisibleIndex = 6;
            this.CHECKLOG.Width = 94;
            // 
            // OTHER
            // 
            this.OTHER.Caption = "其他资料上交情况";
            this.OTHER.FieldName = "OTHER";
            this.OTHER.MinWidth = 25;
            this.OTHER.Name = "OTHER";
            this.OTHER.Visible = true;
            this.OTHER.VisibleIndex = 7;
            this.OTHER.Width = 94;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.splitContainer2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(990, 453);
            this.xtraTabPage2.Text = "任务管理";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridControl1);
            this.splitContainer2.Size = new System.Drawing.Size(990, 453);
            this.splitContainer2.SplitterDistance = 43;
            this.splitContainer2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(909, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 0;
            this.button2.Text = "导入";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(990, 406);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridView1_PopupMenuShowing);
            // 
            // FormTaskManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 489);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "FormTaskManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormTaskManage";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormTaskManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlGLDW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewGLDW)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraGrid.GridControl gridControlGLDW;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewGLDW;
        private DevExpress.XtraGrid.Columns.GridColumn GLDW;
        private DevExpress.XtraGrid.Columns.GridColumn GLDWNAME;
        private DevExpress.XtraGrid.Columns.GridColumn CONTACTS;
        private DevExpress.XtraGrid.Columns.GridColumn TEL;
        private DevExpress.XtraGrid.Columns.GridColumn STARTTIME;
        private DevExpress.XtraGrid.Columns.GridColumn STATE;
        private DevExpress.XtraGrid.Columns.GridColumn CHECKLOG;
        private DevExpress.XtraGrid.Columns.GridColumn OTHER;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;

    }
}