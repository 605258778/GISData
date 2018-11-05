namespace GISData.MainMap
{
    partial class FormAddField
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
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.addfieldattribute = new System.Windows.Forms.GroupBox();
            this.lblScale = new System.Windows.Forms.Label();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.txtFieldAliasName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrecision = new System.Windows.Forms.TextBox();
            this.lblPrecision = new System.Windows.Forms.Label();
            this.cmbFieldType = new System.Windows.Forms.ComboBox();
            this.addfieldltype = new System.Windows.Forms.Label();
            this.txtFieldName = new System.Windows.Forms.TextBox();
            this.addfieldname = new System.Windows.Forms.Label();
            this.addfieldattribute.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(244, 273);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(56, 31);
            this.btnCancle.TabIndex = 13;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(154, 273);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(59, 31);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // addfieldattribute
            // 
            this.addfieldattribute.Controls.Add(this.lblScale);
            this.addfieldattribute.Controls.Add(this.txtScale);
            this.addfieldattribute.Controls.Add(this.txtFieldAliasName);
            this.addfieldattribute.Controls.Add(this.label1);
            this.addfieldattribute.Controls.Add(this.txtPrecision);
            this.addfieldattribute.Controls.Add(this.lblPrecision);
            this.addfieldattribute.Location = new System.Drawing.Point(62, 102);
            this.addfieldattribute.Name = "addfieldattribute";
            this.addfieldattribute.Size = new System.Drawing.Size(238, 165);
            this.addfieldattribute.TabIndex = 11;
            this.addfieldattribute.TabStop = false;
            this.addfieldattribute.Text = "字段属性";
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(25, 122);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(37, 15);
            this.lblScale.TabIndex = 4;
            this.lblScale.Text = "长度";
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(81, 119);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(140, 25);
            this.txtScale.TabIndex = 2;
            // 
            // txtFieldAliasName
            // 
            this.txtFieldAliasName.Location = new System.Drawing.Point(81, 41);
            this.txtFieldAliasName.Name = "txtFieldAliasName";
            this.txtFieldAliasName.Size = new System.Drawing.Size(140, 25);
            this.txtFieldAliasName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "别名";
            // 
            // txtPrecision
            // 
            this.txtPrecision.Location = new System.Drawing.Point(81, 83);
            this.txtPrecision.Name = "txtPrecision";
            this.txtPrecision.Size = new System.Drawing.Size(140, 25);
            this.txtPrecision.TabIndex = 1;
            // 
            // lblPrecision
            // 
            this.lblPrecision.AutoSize = true;
            this.lblPrecision.Location = new System.Drawing.Point(25, 89);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(37, 15);
            this.lblPrecision.TabIndex = 0;
            this.lblPrecision.Text = "精度";
            // 
            // cmbFieldType
            // 
            this.cmbFieldType.FormattingEnabled = true;
            this.cmbFieldType.Location = new System.Drawing.Point(117, 54);
            this.cmbFieldType.Name = "cmbFieldType";
            this.cmbFieldType.Size = new System.Drawing.Size(183, 23);
            this.cmbFieldType.TabIndex = 10;
            this.cmbFieldType.SelectedIndexChanged += new System.EventHandler(this.cmbFieldType_SelectedIndexChanged);
            // 
            // addfieldltype
            // 
            this.addfieldltype.AutoSize = true;
            this.addfieldltype.Location = new System.Drawing.Point(59, 62);
            this.addfieldltype.Name = "addfieldltype";
            this.addfieldltype.Size = new System.Drawing.Size(52, 15);
            this.addfieldltype.TabIndex = 9;
            this.addfieldltype.Text = "类型：";
            // 
            // txtFieldName
            // 
            this.txtFieldName.Location = new System.Drawing.Point(117, 13);
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.Size = new System.Drawing.Size(183, 25);
            this.txtFieldName.TabIndex = 8;
            // 
            // addfieldname
            // 
            this.addfieldname.AutoSize = true;
            this.addfieldname.Location = new System.Drawing.Point(59, 23);
            this.addfieldname.Name = "addfieldname";
            this.addfieldname.Size = new System.Drawing.Size(45, 15);
            this.addfieldname.TabIndex = 7;
            this.addfieldname.Text = "名称:";
            // 
            // FormAddField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 321);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.addfieldattribute);
            this.Controls.Add(this.cmbFieldType);
            this.Controls.Add(this.addfieldltype);
            this.Controls.Add(this.txtFieldName);
            this.Controls.Add(this.addfieldname);
            this.Name = "FormAddField";
            this.Text = "添加字段";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormAddField_Load);
            this.addfieldattribute.ResumeLayout(false);
            this.addfieldattribute.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox addfieldattribute;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.TextBox txtPrecision;
        private System.Windows.Forms.Label lblPrecision;
        private System.Windows.Forms.ComboBox cmbFieldType;
        private System.Windows.Forms.Label addfieldltype;
        private System.Windows.Forms.TextBox txtFieldName;
        private System.Windows.Forms.Label addfieldname;
        private System.Windows.Forms.TextBox txtFieldAliasName;
        private System.Windows.Forms.Label label1;
    }
}