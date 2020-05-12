namespace GISData.Parameter
{
    partial class FormSetpara
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SpatialRSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "坐标系统：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(170, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(304, 25);
            this.textBox1.TabIndex = 2;
            // 
            // SpatialRSelect
            // 
            this.SpatialRSelect.Location = new System.Drawing.Point(495, 32);
            this.SpatialRSelect.Name = "SpatialRSelect";
            this.SpatialRSelect.Size = new System.Drawing.Size(75, 25);
            this.SpatialRSelect.TabIndex = 3;
            this.SpatialRSelect.Text = "选择";
            this.SpatialRSelect.UseVisualStyleBackColor = true;
            this.SpatialRSelect.Click += new System.EventHandler(this.SpatialRSelect_Click);
            // 
            // FormSetpara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 507);
            this.Controls.Add(this.SpatialRSelect);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "FormSetpara";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormSetpara";
            this.Load += new System.EventHandler(this.FormSetpara_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SpatialRSelect;
    }
}