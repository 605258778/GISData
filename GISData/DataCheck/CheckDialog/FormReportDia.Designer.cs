namespace GISData.DataCheck.CheckDialog
{
    partial class FormReportDia
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
            this.REPORTNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ERROR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(524, 430);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.REPORTNAME,
            this.STATE,
            this.ERROR,
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gridView1_SelectionChanged);
            // 
            // REPORTNAME
            // 
            this.REPORTNAME.Caption = "名称";
            this.REPORTNAME.FieldName = "REPORTNAME";
            this.REPORTNAME.MinWidth = 25;
            this.REPORTNAME.Name = "REPORTNAME";
            this.REPORTNAME.Visible = true;
            this.REPORTNAME.VisibleIndex = 1;
            this.REPORTNAME.Width = 94;
            // 
            // STATE
            // 
            this.STATE.Caption = "状态";
            this.STATE.FieldName = "STATE";
            this.STATE.MinWidth = 25;
            this.STATE.Name = "STATE";
            this.STATE.Visible = true;
            this.STATE.VisibleIndex = 2;
            this.STATE.Width = 94;
            // 
            // ERROR
            // 
            this.ERROR.Caption = "备注";
            this.ERROR.FieldName = "ERROR";
            this.ERROR.MinWidth = 25;
            this.ERROR.Name = "ERROR";
            this.ERROR.Visible = true;
            this.ERROR.VisibleIndex = 3;
            this.ERROR.Width = 94;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "REPORTMOULD";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 94;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn2";
            this.gridColumn2.FieldName = "DATASOURCE";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Width = 94;
            // 
            // FormReportDia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 430);
            this.Controls.Add(this.gridControl1);
            this.Name = "FormReportDia";
            this.Text = "FormReportDia";
            this.Load += new System.EventHandler(this.FormReportDia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn REPORTNAME;
        private DevExpress.XtraGrid.Columns.GridColumn STATE;
        private DevExpress.XtraGrid.Columns.GridColumn ERROR;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}