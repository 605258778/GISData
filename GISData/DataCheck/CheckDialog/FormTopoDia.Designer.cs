namespace GISData.DataCheck.CheckDialog
{
    partial class FormTopoDia
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ERROR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TABLENAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WHERESTRING = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SUPTABLE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl1.Size = new System.Drawing.Size(527, 443);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControl1_MouseDown);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.NAME,
            this.STATE,
            this.ERROR,
            this.TYPE,
            this.TABLENAME,
            this.WHERESTRING,
            this.SUPTABLE,
            this.gridColumn8});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gridView1_SelectionChanged);
            // 
            // NAME
            // 
            this.NAME.Caption = "名称";
            this.NAME.FieldName = "NAME";
            this.NAME.MinWidth = 25;
            this.NAME.Name = "NAME";
            this.NAME.Visible = true;
            this.NAME.VisibleIndex = 1;
            this.NAME.Width = 157;
            // 
            // STATE
            // 
            this.STATE.Caption = "状态";
            this.STATE.FieldName = "STATE";
            this.STATE.MinWidth = 25;
            this.STATE.Name = "STATE";
            this.STATE.Visible = true;
            this.STATE.VisibleIndex = 2;
            this.STATE.Width = 157;
            // 
            // ERROR
            // 
            this.ERROR.Caption = "错误数";
            this.ERROR.FieldName = "ERROR";
            this.ERROR.MinWidth = 25;
            this.ERROR.Name = "ERROR";
            this.ERROR.Visible = true;
            this.ERROR.VisibleIndex = 3;
            this.ERROR.Width = 163;
            // 
            // TYPE
            // 
            this.TYPE.Caption = "TYPE";
            this.TYPE.FieldName = "TYPE";
            this.TYPE.MinWidth = 25;
            this.TYPE.Name = "TYPE";
            this.TYPE.Width = 94;
            // 
            // TABLENAME
            // 
            this.TABLENAME.Caption = "TABLENAME";
            this.TABLENAME.FieldName = "TABLENAME";
            this.TABLENAME.MinWidth = 25;
            this.TABLENAME.Name = "TABLENAME";
            this.TABLENAME.Width = 94;
            // 
            // WHERESTRING
            // 
            this.WHERESTRING.Caption = "WHERESTRING";
            this.WHERESTRING.FieldName = "WHERESTRING";
            this.WHERESTRING.MinWidth = 25;
            this.WHERESTRING.Name = "WHERESTRING";
            this.WHERESTRING.Width = 94;
            // 
            // SUPTABLE
            // 
            this.SUPTABLE.Caption = "SUPTABLE";
            this.SUPTABLE.FieldName = "SUPTABLE";
            this.SUPTABLE.MinWidth = 25;
            this.SUPTABLE.Name = "SUPTABLE";
            this.SUPTABLE.Width = 94;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "gridColumn8";
            this.gridColumn8.MinWidth = 25;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Width = 94;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // FormTopoDia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 443);
            this.Controls.Add(this.gridControl1);
            this.Name = "FormTopoDia";
            this.Text = "FormTopoDia";
            this.Load += new System.EventHandler(this.FormTopoDia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn NAME;
        private DevExpress.XtraGrid.Columns.GridColumn STATE;
        private DevExpress.XtraGrid.Columns.GridColumn ERROR;
        private DevExpress.XtraGrid.Columns.GridColumn TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn TABLENAME;
        private DevExpress.XtraGrid.Columns.GridColumn WHERESTRING;
        private DevExpress.XtraGrid.Columns.GridColumn SUPTABLE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}