namespace GISData.DataRegister
{
    partial class FormDBConnectInfo
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
            this.textBoxConName = new System.Windows.Forms.TextBox();
            this.comboBoxConType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxConPath = new System.Windows.Forms.TextBox();
            this.buttonPathSelect = new System.Windows.Forms.Button();
            this.buttonConOK = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxConName
            // 
            this.textBoxConName.Location = new System.Drawing.Point(83, 53);
            this.textBoxConName.Name = "textBoxConName";
            this.textBoxConName.Size = new System.Drawing.Size(315, 25);
            this.textBoxConName.TabIndex = 0;
            // 
            // comboBoxConType
            // 
            this.comboBoxConType.FormattingEnabled = true;
            this.comboBoxConType.Items.AddRange(new object[] {
            "Access数据库",
            "文件夹数据库"});
            this.comboBoxConType.Location = new System.Drawing.Point(83, 108);
            this.comboBoxConType.Name = "comboBoxConType";
            this.comboBoxConType.Size = new System.Drawing.Size(315, 23);
            this.comboBoxConType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "类型:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "路径：";
            // 
            // textBoxConPath
            // 
            this.textBoxConPath.Location = new System.Drawing.Point(83, 163);
            this.textBoxConPath.Name = "textBoxConPath";
            this.textBoxConPath.Size = new System.Drawing.Size(250, 25);
            this.textBoxConPath.TabIndex = 5;
            // 
            // buttonPathSelect
            // 
            this.buttonPathSelect.Location = new System.Drawing.Point(339, 162);
            this.buttonPathSelect.Name = "buttonPathSelect";
            this.buttonPathSelect.Size = new System.Drawing.Size(59, 26);
            this.buttonPathSelect.TabIndex = 6;
            this.buttonPathSelect.Text = "选择";
            this.buttonPathSelect.UseVisualStyleBackColor = true;
            this.buttonPathSelect.Click += new System.EventHandler(this.buttonPathSelect_Click);
            // 
            // buttonConOK
            // 
            this.buttonConOK.Location = new System.Drawing.Point(217, 284);
            this.buttonConOK.Name = "buttonConOK";
            this.buttonConOK.Size = new System.Drawing.Size(75, 40);
            this.buttonConOK.TabIndex = 7;
            this.buttonConOK.Text = "确定";
            this.buttonConOK.UseVisualStyleBackColor = true;
            this.buttonConOK.Click += new System.EventHandler(this.buttonConOK_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(320, 284);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 40);
            this.button3.TabIndex = 7;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // FormDBConnectInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 336);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonConOK);
            this.Controls.Add(this.buttonPathSelect);
            this.Controls.Add(this.textBoxConPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxConType);
            this.Controls.Add(this.textBoxConName);
            this.Name = "FormDBConnectInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库连接";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxConName;
        private System.Windows.Forms.ComboBox comboBoxConType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxConPath;
        private System.Windows.Forms.Button buttonPathSelect;
        private System.Windows.Forms.Button buttonConOK;
        private System.Windows.Forms.Button button3;
    }
}