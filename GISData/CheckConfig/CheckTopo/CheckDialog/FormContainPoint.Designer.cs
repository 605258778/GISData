namespace GISData.CheckConfig.CheckTopo.CheckDialog
{
    partial class FormContainPoint
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
            this.comboBoxPoint = new System.Windows.Forms.ComboBox();
            this.textBoxNumPoint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxWhere = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxPoint
            // 
            this.comboBoxPoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxPoint.FormattingEnabled = true;
            this.comboBoxPoint.Location = new System.Drawing.Point(146, 124);
            this.comboBoxPoint.Name = "comboBoxPoint";
            this.comboBoxPoint.Size = new System.Drawing.Size(276, 23);
            this.comboBoxPoint.TabIndex = 0;
            // 
            // textBoxNumPoint
            // 
            this.textBoxNumPoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxNumPoint.Location = new System.Drawing.Point(145, 192);
            this.textBoxNumPoint.Name = "textBoxNumPoint";
            this.textBoxNumPoint.Size = new System.Drawing.Size(276, 25);
            this.textBoxNumPoint.TabIndex = 1;
            this.textBoxNumPoint.Validated += new System.EventHandler(this.textBoxNumPoint_Validated);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "包含点个数：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "点图层：";
            // 
            // textBoxWhere
            // 
            this.textBoxWhere.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxWhere.Location = new System.Drawing.Point(146, 22);
            this.textBoxWhere.Multiline = true;
            this.textBoxWhere.Name = "textBoxWhere";
            this.textBoxWhere.Size = new System.Drawing.Size(276, 68);
            this.textBoxWhere.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "筛选条件：";
            // 
            // FormContainPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 281);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxWhere);
            this.Controls.Add(this.textBoxNumPoint);
            this.Controls.Add(this.comboBoxPoint);
            this.Name = "FormContainPoint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormContainPoint";
            this.Load += new System.EventHandler(this.FormContainPoint_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWhere;
        private System.Windows.Forms.ComboBox comboBoxPoint;
        private System.Windows.Forms.TextBox textBoxNumPoint;
    }
}