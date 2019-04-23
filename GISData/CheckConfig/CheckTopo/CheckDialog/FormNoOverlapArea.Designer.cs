namespace GISData.CheckConfig.CheckTopo.CheckDialog
{
    partial class FormNoOverlapArea
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
            this.comboBoxOverLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxOverLayer
            // 
            this.comboBoxOverLayer.FormattingEnabled = true;
            this.comboBoxOverLayer.Location = new System.Drawing.Point(208, 68);
            this.comboBoxOverLayer.Name = "comboBoxOverLayer";
            this.comboBoxOverLayer.Size = new System.Drawing.Size(249, 23);
            this.comboBoxOverLayer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "重叠图层：";
            // 
            // FormNoOverlapArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 189);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxOverLayer);
            this.Name = "FormNoOverlapArea";
            this.Text = "FormNoOverlapArea";
            this.Load += new System.EventHandler(this.FormNoOverlapArea_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOverLayer;
    }
}