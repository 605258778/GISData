namespace GISData.ChekConfig
{
    partial class FormStep
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
            this.textBoxStepName = new System.Windows.Forms.TextBox();
            this.labelStepName = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxStepName
            // 
            this.textBoxStepName.Location = new System.Drawing.Point(85, 21);
            this.textBoxStepName.Name = "textBoxStepName";
            this.textBoxStepName.Size = new System.Drawing.Size(210, 25);
            this.textBoxStepName.TabIndex = 0;
            // 
            // labelStepName
            // 
            this.labelStepName.AutoSize = true;
            this.labelStepName.Location = new System.Drawing.Point(29, 27);
            this.labelStepName.Name = "labelStepName";
            this.labelStepName.Size = new System.Drawing.Size(52, 15);
            this.labelStepName.TabIndex = 1;
            this.labelStepName.Text = "名称：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "结构检查",
            "数据排查",
            "变化提取",
            "自动计算",
            "属性检查",
            "图形检查",
            "统计报表"});
            this.comboBox1.Location = new System.Drawing.Point(85, 84);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(210, 23);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "类型：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 38);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FormStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 265);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelStepName);
            this.Controls.Add(this.textBoxStepName);
            this.Name = "FormStep";
            this.Text = "添加";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxStepName;
        private System.Windows.Forms.Label labelStepName;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}