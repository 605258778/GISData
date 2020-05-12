namespace GISData.TaskManage
{
    partial class TaskFieldRegister
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
            this.textEditField = new DevExpress.XtraEditors.TextEdit();
            this.textEditWhere = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.comboBoxDic = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.textEditField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWhere.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEditField
            // 
            this.textEditField.Enabled = false;
            this.textEditField.Location = new System.Drawing.Point(108, 31);
            this.textEditField.Name = "textEditField";
            this.textEditField.Size = new System.Drawing.Size(266, 24);
            this.textEditField.TabIndex = 0;
            // 
            // textEditWhere
            // 
            this.textEditWhere.Location = new System.Drawing.Point(108, 145);
            this.textEditWhere.Name = "textEditWhere";
            this.textEditWhere.Size = new System.Drawing.Size(266, 24);
            this.textEditWhere.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(52, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 18);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "字段：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(37, 88);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "字典表：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(22, 148);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 18);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "字典条件：";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(124, 198);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(94, 42);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(247, 198);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(94, 42);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // comboBoxDic
            // 
            this.comboBoxDic.FormattingEnabled = true;
            this.comboBoxDic.Location = new System.Drawing.Point(108, 87);
            this.comboBoxDic.Name = "comboBoxDic";
            this.comboBoxDic.Size = new System.Drawing.Size(266, 23);
            this.comboBoxDic.TabIndex = 4;
            // 
            // TaskFieldRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 283);
            this.Controls.Add(this.comboBoxDic);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.textEditWhere);
            this.Controls.Add(this.textEditField);
            this.Name = "TaskFieldRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormRegister";
            this.Load += new System.EventHandler(this.TaskFieldRegister_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEditField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditWhere.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEditField;
        private DevExpress.XtraEditors.TextEdit textEditWhere;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.ComboBox comboBoxDic;
    }
}