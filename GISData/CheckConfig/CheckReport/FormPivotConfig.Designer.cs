namespace GISData.CheckConfig.CheckReport
{
    partial class FormPivotConfig
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
            this.checkedComboBoxEdit1 = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedComboBoxEdit2 = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.字段 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedComboBoxEdit1
            // 
            this.checkedComboBoxEdit1.Location = new System.Drawing.Point(67, 44);
            this.checkedComboBoxEdit1.Name = "checkedComboBoxEdit1";
            this.checkedComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEdit1.Size = new System.Drawing.Size(198, 24);
            this.checkedComboBoxEdit1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "行：";
            // 
            // checkedComboBoxEdit2
            // 
            this.checkedComboBoxEdit2.Location = new System.Drawing.Point(390, 44);
            this.checkedComboBoxEdit2.Name = "checkedComboBoxEdit2";
            this.checkedComboBoxEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEdit2.Size = new System.Drawing.Size(198, 24);
            this.checkedComboBoxEdit2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(347, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "列：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "值：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "类型：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(473, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 43);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.comboBox2);
            this.groupControl1.Controls.Add(this.comboBox1);
            this.groupControl1.Controls.Add(this.gridControl1);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.button2);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Location = new System.Drawing.Point(12, 88);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(603, 207);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "计算";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "求和",
            "计数",
            "平均数",
            "最大值",
            "最小值"});
            this.comboBox2.Location = new System.Drawing.Point(55, 43);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(199, 26);
            this.comboBox2.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "求和",
            "计数",
            "平均数",
            "最大值",
            "最小值"});
            this.comboBox1.Location = new System.Drawing.Point(54, 103);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(199, 26);
            this.comboBox1.TabIndex = 4;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(313, 44);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(263, 149);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.字段,
            this.gridColumn1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // 字段
            // 
            this.字段.Caption = "字段";
            this.字段.FieldName = "FIELDNAME";
            this.字段.MinWidth = 25;
            this.字段.Name = "字段";
            this.字段.Visible = true;
            this.字段.VisibleIndex = 0;
            this.字段.Width = 94;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "计算类型";
            this.gridColumn1.FieldName = "TYPE";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 94;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(92, 155);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 38);
            this.button2.TabIndex = 2;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormPivotConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 383);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedComboBoxEdit2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedComboBoxEdit1);
            this.Name = "FormPivotConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPivotConfig";
            this.Load += new System.EventHandler(this.FormPivotConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEdit1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEdit2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Button button2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn 字段;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}