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
            this.comboBoxScheme = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddStep = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxScheme);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1164, 543);
            this.splitContainer1.SplitterDistance = 55;
            this.splitContainer1.TabIndex = 0;
            // 
            // DelScheme
            // 
            this.DelScheme.Location = new System.Drawing.Point(597, 6);
            this.DelScheme.Name = "DelScheme";
            this.DelScheme.Size = new System.Drawing.Size(86, 32);
            this.DelScheme.TabIndex = 1;
            this.DelScheme.Text = "删除方案";
            this.DelScheme.UseVisualStyleBackColor = true;
            // 
            // EditScheme
            // 
            this.EditScheme.Location = new System.Drawing.Point(488, 6);
            this.EditScheme.Name = "EditScheme";
            this.EditScheme.Size = new System.Drawing.Size(86, 32);
            this.EditScheme.TabIndex = 1;
            this.EditScheme.Text = "编辑方案";
            this.EditScheme.UseVisualStyleBackColor = true;
            // 
            // AddScheme
            // 
            this.AddScheme.Location = new System.Drawing.Point(376, 7);
            this.AddScheme.Name = "AddScheme";
            this.AddScheme.Size = new System.Drawing.Size(86, 31);
            this.AddScheme.TabIndex = 1;
            this.AddScheme.Text = "添加方案";
            this.AddScheme.UseVisualStyleBackColor = true;
            this.AddScheme.Click += new System.EventHandler(this.AddScheme_Click);
            // 
            // comboBoxScheme
            // 
            this.comboBoxScheme.FormattingEnabled = true;
            this.comboBoxScheme.Location = new System.Drawing.Point(12, 13);
            this.comboBoxScheme.Name = "comboBoxScheme";
            this.comboBoxScheme.Size = new System.Drawing.Size(328, 23);
            this.comboBoxScheme.TabIndex = 0;
            this.comboBoxScheme.SelectedIndexChanged += new System.EventHandler(this.comboBoxScheme_SelectedIndexChanged);
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
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1164, 484);
            this.splitContainer2.SplitterDistance = 240;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(240, 484);
            this.splitContainer3.SplitterDistance = 67;
            this.splitContainer3.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonAddStep);
            this.panel1.Controls.Add(this.buttonRemove);
            this.panel1.Location = new System.Drawing.Point(13, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 48);
            this.panel1.TabIndex = 2;
            // 
            // buttonAddStep
            // 
            this.buttonAddStep.Location = new System.Drawing.Point(24, 7);
            this.buttonAddStep.Name = "buttonAddStep";
            this.buttonAddStep.Size = new System.Drawing.Size(75, 38);
            this.buttonAddStep.TabIndex = 0;
            this.buttonAddStep.Text = "添加";
            this.buttonAddStep.UseVisualStyleBackColor = true;
            this.buttonAddStep.Click += new System.EventHandler(this.buttonAddStep_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(105, 7);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 38);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "删除";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // FormConfigMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 543);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "质检配置";
            this.Load += new System.EventHandler(this.FormConfigMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button DelScheme;
        private System.Windows.Forms.Button EditScheme;
        private System.Windows.Forms.Button AddScheme;
        private System.Windows.Forms.ComboBox comboBoxScheme;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonAddStep;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Panel panel1;

    }
}