namespace GISData.DataRegister
{
    partial class FormDomain
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFieldName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFieldAliName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDomain = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDomainWhere = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxIsShow = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxIsEdit = new System.Windows.Forms.ComboBox();
            this.buttonEditSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "字段名：";
            // 
            // textBoxFieldName
            // 
            this.textBoxFieldName.Enabled = false;
            this.textBoxFieldName.Location = new System.Drawing.Point(105, 16);
            this.textBoxFieldName.Name = "textBoxFieldName";
            this.textBoxFieldName.Size = new System.Drawing.Size(138, 25);
            this.textBoxFieldName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "字段别名：";
            // 
            // textBoxFieldAliName
            // 
            this.textBoxFieldAliName.Enabled = false;
            this.textBoxFieldAliName.Location = new System.Drawing.Point(368, 14);
            this.textBoxFieldAliName.Name = "textBoxFieldAliName";
            this.textBoxFieldAliName.Size = new System.Drawing.Size(138, 25);
            this.textBoxFieldAliName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "字典域：";
            // 
            // comboBoxDomain
            // 
            this.comboBoxDomain.FormattingEnabled = true;
            this.comboBoxDomain.Items.AddRange(new object[] {
            "政区数据字典",
            "资源数据字典"});
            this.comboBoxDomain.Location = new System.Drawing.Point(105, 143);
            this.comboBoxDomain.Name = "comboBoxDomain";
            this.comboBoxDomain.Size = new System.Drawing.Size(138, 23);
            this.comboBoxDomain.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(265, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "字典域条件：";
            // 
            // textBoxDomainWhere
            // 
            this.textBoxDomainWhere.Location = new System.Drawing.Point(368, 141);
            this.textBoxDomainWhere.Name = "textBoxDomainWhere";
            this.textBoxDomainWhere.Size = new System.Drawing.Size(138, 25);
            this.textBoxDomainWhere.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "是否显示：";
            // 
            // comboBoxIsShow
            // 
            this.comboBoxIsShow.FormattingEnabled = true;
            this.comboBoxIsShow.Items.AddRange(new object[] {
            "是",
            "否"});
            this.comboBoxIsShow.Location = new System.Drawing.Point(105, 78);
            this.comboBoxIsShow.Name = "comboBoxIsShow";
            this.comboBoxIsShow.Size = new System.Drawing.Size(138, 23);
            this.comboBoxIsShow.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(280, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "是否编辑：";
            // 
            // comboBoxIsEdit
            // 
            this.comboBoxIsEdit.FormattingEnabled = true;
            this.comboBoxIsEdit.Items.AddRange(new object[] {
            "是",
            "否"});
            this.comboBoxIsEdit.Location = new System.Drawing.Point(368, 78);
            this.comboBoxIsEdit.Name = "comboBoxIsEdit";
            this.comboBoxIsEdit.Size = new System.Drawing.Size(138, 23);
            this.comboBoxIsEdit.TabIndex = 2;
            // 
            // buttonEditSave
            // 
            this.buttonEditSave.Location = new System.Drawing.Point(421, 195);
            this.buttonEditSave.Name = "buttonEditSave";
            this.buttonEditSave.Size = new System.Drawing.Size(85, 42);
            this.buttonEditSave.TabIndex = 3;
            this.buttonEditSave.Text = "确认";
            this.buttonEditSave.UseVisualStyleBackColor = true;
            this.buttonEditSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormDomain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 252);
            this.Controls.Add(this.buttonEditSave);
            this.Controls.Add(this.comboBoxIsEdit);
            this.Controls.Add(this.comboBoxIsShow);
            this.Controls.Add(this.comboBoxDomain);
            this.Controls.Add(this.textBoxDomainWhere);
            this.Controls.Add(this.textBoxFieldAliName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxFieldName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "FormDomain";
            this.Text = "字段信息";
            this.Load += new System.EventHandler(this.FormDomain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFieldAliName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxDomain;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDomainWhere;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxIsShow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxIsEdit;
        private System.Windows.Forms.Button buttonEditSave;
    }
}