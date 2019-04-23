namespace GISData.CheckConfig.CheckTopo.CheckDialog
{
    partial class Formxbm
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
            this.textBoxinput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxwhere = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "最小面积:";
            // 
            // textBoxinput
            // 
            this.textBoxinput.Location = new System.Drawing.Point(198, 168);
            this.textBoxinput.Name = "textBoxinput";
            this.textBoxinput.Size = new System.Drawing.Size(194, 25);
            this.textBoxinput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "筛选条件:";
            // 
            // textBoxwhere
            // 
            this.textBoxwhere.Location = new System.Drawing.Point(198, 25);
            this.textBoxwhere.Multiline = true;
            this.textBoxwhere.Name = "textBoxwhere";
            this.textBoxwhere.Size = new System.Drawing.Size(194, 97);
            this.textBoxwhere.TabIndex = 1;
            // 
            // Formxbm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 312);
            this.Controls.Add(this.textBoxwhere);
            this.Controls.Add(this.textBoxinput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Formxbm";
            this.Text = "Formxbm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxwhere;
        private System.Windows.Forms.TextBox textBoxinput;
    }
}