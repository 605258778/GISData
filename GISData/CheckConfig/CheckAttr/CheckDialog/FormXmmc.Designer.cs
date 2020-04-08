namespace GISData.CheckConfig.CheckAttr.CheckDialog
{
    partial class FormXmmc
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
            this.字段 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.对应 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.对应字段 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 76);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(605, 422);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.字段,
            this.对应,
            this.对应字段});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // 字段
            // 
            this.字段.Caption = "字段";
            this.字段.FieldName = "FIELD";
            this.字段.MinWidth = 25;
            this.字段.Name = "字段";
            this.字段.Visible = true;
            this.字段.VisibleIndex = 0;
            this.字段.Width = 94;
            // 
            // 对应
            // 
            this.对应.AppearanceCell.Options.UseTextOptions = true;
            this.对应.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.对应.Caption = "对应";
            this.对应.FieldName = "RELATION";
            this.对应.MinWidth = 25;
            this.对应.Name = "对应";
            this.对应.Visible = true;
            this.对应.VisibleIndex = 1;
            this.对应.Width = 94;
            // 
            // 对应字段
            // 
            this.对应字段.Caption = "对应字段";
            this.对应字段.FieldName = "TASKFIELD";
            this.对应字段.MinWidth = 25;
            this.对应字段.Name = "对应字段";
            this.对应字段.Visible = true;
            this.对应字段.VisibleIndex = 2;
            this.对应字段.Width = 94;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(356, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 39);
            this.button2.TabIndex = 2;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormXmmc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 537);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gridControl1);
            this.Name = "FormXmmc";
            this.Text = "FormXmmc";
            this.Load += new System.EventHandler(this.FormXmmc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn 字段;
        private DevExpress.XtraGrid.Columns.GridColumn 对应;
        private DevExpress.XtraGrid.Columns.GridColumn 对应字段;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}