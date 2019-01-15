namespace GISData.ChekConfig
{
    partial class FormConfigMain
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DelScheme = new System.Windows.Forms.Button();
            this.EditScheme = new System.Windows.Forms.Button();
            this.AddScheme = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonAddStep = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.DelScheme);
            this.splitContainer1.Panel1.Controls.Add(this.EditScheme);
            this.splitContainer1.Panel1.Controls.Add(this.AddScheme);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1164, 543);
            this.splitContainer1.SplitterDistance = 37;
            this.splitContainer1.TabIndex = 0;
            // 
            // DelScheme
            // 
            this.DelScheme.Location = new System.Drawing.Point(597, 6);
            this.DelScheme.Name = "DelScheme";
            this.DelScheme.Size = new System.Drawing.Size(86, 23);
            this.DelScheme.TabIndex = 1;
            this.DelScheme.Text = "删除方案";
            this.DelScheme.UseVisualStyleBackColor = true;
            // 
            // EditScheme
            // 
            this.EditScheme.Location = new System.Drawing.Point(488, 6);
            this.EditScheme.Name = "EditScheme";
            this.EditScheme.Size = new System.Drawing.Size(86, 23);
            this.EditScheme.TabIndex = 1;
            this.EditScheme.Text = "编辑方案";
            this.EditScheme.UseVisualStyleBackColor = true;
            // 
            // AddScheme
            // 
            this.AddScheme.Location = new System.Drawing.Point(376, 7);
            this.AddScheme.Name = "AddScheme";
            this.AddScheme.Size = new System.Drawing.Size(86, 23);
            this.AddScheme.TabIndex = 1;
            this.AddScheme.Text = "添加方案";
            this.AddScheme.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(328, 23);
            this.comboBox1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.buttonAddStep);
            this.splitContainer2.Size = new System.Drawing.Size(1164, 502);
            this.splitContainer2.SplitterDistance = 240;
            this.splitContainer2.TabIndex = 0;
            // 
            // buttonAddStep
            // 
            this.buttonAddStep.Location = new System.Drawing.Point(80, 17);
            this.buttonAddStep.Name = "buttonAddStep";
            this.buttonAddStep.Size = new System.Drawing.Size(75, 38);
            this.buttonAddStep.TabIndex = 0;
            this.buttonAddStep.Text = "添加";
            this.buttonAddStep.UseVisualStyleBackColor = true;
            this.buttonAddStep.Click += new System.EventHandler(this.buttonAddStep_Click);
            // 
            // FormConfigMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 543);
            this.Controls.Add(this.splitContainer1);
            this.IsMdiContainer = true;
            this.Name = "FormConfigMain";
            this.Text = "质检配置";
            this.Load += new System.EventHandler(this.FormConfigMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button DelScheme;
        private System.Windows.Forms.Button EditScheme;
        private System.Windows.Forms.Button AddScheme;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonAddStep;

    }
}