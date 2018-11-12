namespace GISData.MainMap
{
    partial class FormAttribute
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
            this.components = new System.ComponentModel.Container();
            this.dataGridViewAttr = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.addfield = new System.Windows.Forms.ToolStripMenuItem();
            this.toolEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolExpXLS = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttr)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewAttr
            // 
            this.dataGridViewAttr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAttr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAttr.Location = new System.Drawing.Point(0, 28);
            this.dataGridViewAttr.Name = "dataGridViewAttr";
            this.dataGridViewAttr.RowTemplate.Height = 27;
            this.dataGridViewAttr.Size = new System.Drawing.Size(891, 428);
            this.dataGridViewAttr.TabIndex = 2;
            this.dataGridViewAttr.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewAttr_CellMouseDown);
            this.dataGridViewAttr.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAttr_CellValueChanged);
            this.dataGridViewAttr.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewAttr_RowHeaderMouseClick);
            this.dataGridViewAttr.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewAttr_RowHeaderMouseDoubleClick);
            this.dataGridViewAttr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewAttr_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addfield,
            this.toolEditor,
            this.toolSave,
            this.toolDelSelect,
            this.toolExpXLS});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(891, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // addfield
            // 
            this.addfield.Name = "addfield";
            this.addfield.Size = new System.Drawing.Size(81, 24);
            this.addfield.Text = "增加字段";
            this.addfield.Click += new System.EventHandler(this.addfield_Click);
            // 
            // toolEditor
            // 
            this.toolEditor.Name = "toolEditor";
            this.toolEditor.Size = new System.Drawing.Size(81, 24);
            this.toolEditor.Text = "编辑属性";
            this.toolEditor.Click += new System.EventHandler(this.toolEditor_Click);
            // 
            // toolSave
            // 
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(81, 24);
            this.toolSave.Text = "保存编辑";
            this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
            // 
            // toolDelSelect
            // 
            this.toolDelSelect.Name = "toolDelSelect";
            this.toolDelSelect.Size = new System.Drawing.Size(81, 24);
            this.toolDelSelect.Text = "删除选择";
            this.toolDelSelect.Click += new System.EventHandler(this.toolDelSelect_Click);
            // 
            // toolExpXLS
            // 
            this.toolExpXLS.Name = "toolExpXLS";
            this.toolExpXLS.Size = new System.Drawing.Size(88, 24);
            this.toolExpXLS.Text = "导出Excel";
            this.toolExpXLS.Click += new System.EventHandler(this.toolExpXLS_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 100);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItem1.Text = "顺序排列";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItem2.Text = "逆序排列";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItem3.Text = "删除字段";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItem4.Text = "字段计算";
            // 
            // FormAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 456);
            this.Controls.Add(this.dataGridViewAttr);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormAttribute";
            this.Text = "属性表";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormAttribute_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttr)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAttr;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addfield;
        private System.Windows.Forms.ToolStripMenuItem toolEditor;
        private System.Windows.Forms.ToolStripMenuItem toolSave;
        private System.Windows.Forms.ToolStripMenuItem toolDelSelect;
        private System.Windows.Forms.ToolStripMenuItem toolExpXLS;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
    }
}