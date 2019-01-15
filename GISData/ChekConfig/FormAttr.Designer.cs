namespace GISData.ChekConfig
{
    partial class FormAttr
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
            this.treeViewAttr = new System.Windows.Forms.TreeView();
            this.属性删除 = new System.Windows.Forms.Button();
            this.属性编辑 = new System.Windows.Forms.Button();
            this.添加项 = new System.Windows.Forms.Button();
            this.添加分组 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewAttr
            // 
            this.treeViewAttr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAttr.Location = new System.Drawing.Point(0, 0);
            this.treeViewAttr.Name = "treeViewAttr";
            this.treeViewAttr.Size = new System.Drawing.Size(788, 469);
            this.treeViewAttr.TabIndex = 0;
            // 
            // 属性删除
            // 
            this.属性删除.Location = new System.Drawing.Point(559, 6);
            this.属性删除.Name = "属性删除";
            this.属性删除.Size = new System.Drawing.Size(75, 23);
            this.属性删除.TabIndex = 8;
            this.属性删除.Text = "删除";
            this.属性删除.UseVisualStyleBackColor = true;
            // 
            // 属性编辑
            // 
            this.属性编辑.Location = new System.Drawing.Point(473, 6);
            this.属性编辑.Name = "属性编辑";
            this.属性编辑.Size = new System.Drawing.Size(75, 23);
            this.属性编辑.TabIndex = 9;
            this.属性编辑.Text = "编辑";
            this.属性编辑.UseVisualStyleBackColor = true;
            // 
            // 添加项
            // 
            this.添加项.Location = new System.Drawing.Point(351, 6);
            this.添加项.Name = "添加项";
            this.添加项.Size = new System.Drawing.Size(116, 23);
            this.添加项.TabIndex = 10;
            this.添加项.Text = "添加项";
            this.添加项.UseVisualStyleBackColor = true;
            this.添加项.Click += new System.EventHandler(this.添加项_Click);
            // 
            // 添加分组
            // 
            this.添加分组.Location = new System.Drawing.Point(239, 6);
            this.添加分组.Name = "添加分组";
            this.添加分组.Size = new System.Drawing.Size(106, 23);
            this.添加分组.TabIndex = 11;
            this.添加分组.Text = "添加分组";
            this.添加分组.UseVisualStyleBackColor = true;
            this.添加分组.Click += new System.EventHandler(this.添加分组_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.属性删除);
            this.splitContainer1.Panel1.Controls.Add(this.属性编辑);
            this.splitContainer1.Panel1.Controls.Add(this.添加分组);
            this.splitContainer1.Panel1.Controls.Add(this.添加项);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.treeViewAttr);
            this.splitContainer1.Size = new System.Drawing.Size(788, 505);
            this.splitContainer1.SplitterDistance = 32;
            this.splitContainer1.TabIndex = 12;
            // 
            // FormAttr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 505);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormAttr";
            this.Text = "FormAttr";
            this.Load += new System.EventHandler(this.FormAttr_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewAttr;
        private System.Windows.Forms.Button 属性删除;
        private System.Windows.Forms.Button 属性编辑;
        private System.Windows.Forms.Button 添加项;
        private System.Windows.Forms.Button 添加分组;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}