namespace GISData.Dictionary
{
    partial class FormDictionary
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
            this.ExportDic = new System.Windows.Forms.Button();
            this.treeViewDic = new System.Windows.Forms.TreeView();
            this.comboBoxDic = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ExportDic);
            this.splitContainer1.Panel1.Controls.Add(this.treeViewDic);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxDic);
            this.splitContainer1.Size = new System.Drawing.Size(1013, 532);
            this.splitContainer1.SplitterDistance = 337;
            this.splitContainer1.TabIndex = 0;
            // 
            // ExportDic
            // 
            this.ExportDic.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExportDic.Location = new System.Drawing.Point(0, 502);
            this.ExportDic.Name = "ExportDic";
            this.ExportDic.Size = new System.Drawing.Size(337, 30);
            this.ExportDic.TabIndex = 2;
            this.ExportDic.Text = "导出";
            this.ExportDic.UseVisualStyleBackColor = true;
            // 
            // treeViewDic
            // 
            this.treeViewDic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDic.Location = new System.Drawing.Point(0, 23);
            this.treeViewDic.Name = "treeViewDic";
            this.treeViewDic.Size = new System.Drawing.Size(337, 509);
            this.treeViewDic.TabIndex = 1;
            // 
            // comboBoxDic
            // 
            this.comboBoxDic.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxDic.FormattingEnabled = true;
            this.comboBoxDic.Items.AddRange(new object[] {
            "政区数据字典",
            "资源数据字典"});
            this.comboBoxDic.Location = new System.Drawing.Point(0, 0);
            this.comboBoxDic.Name = "comboBoxDic";
            this.comboBoxDic.Size = new System.Drawing.Size(337, 23);
            this.comboBoxDic.TabIndex = 0;
            this.comboBoxDic.SelectedIndexChanged += new System.EventHandler(this.comboBoxDic_SelectedIndexChanged);
            // 
            // FormDictionary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 532);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormDictionary";
            this.Text = "字典管理";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormDictionary_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewDic;
        private System.Windows.Forms.ComboBox comboBoxDic;
        private System.Windows.Forms.Button ExportDic;
    }
}