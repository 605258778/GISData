namespace GISData.DataRegister
{
    partial class FormRegister
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBoxDataReg = new System.Windows.Forms.GroupBox();
            this.dataGridViewDataReg = new System.Windows.Forms.DataGridView();
            this.buttonDelConnecte = new System.Windows.Forms.Button();
            this.buttonAddConnect = new System.Windows.Forms.Button();
            this.groupBoxLoadReg = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBoxDataReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataReg)).BeginInit();
            this.groupBoxLoadReg.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1062, 537);
            this.splitContainer1.SplitterDistance = 277;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBoxDataReg);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Panel2.Controls.Add(this.groupBoxLoadReg);
            this.splitContainer2.Size = new System.Drawing.Size(277, 537);
            this.splitContainer2.SplitterDistance = 264;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBoxDataReg
            // 
            this.groupBoxDataReg.Controls.Add(this.dataGridViewDataReg);
            this.groupBoxDataReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataReg.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDataReg.Name = "groupBoxDataReg";
            this.groupBoxDataReg.Size = new System.Drawing.Size(277, 264);
            this.groupBoxDataReg.TabIndex = 0;
            this.groupBoxDataReg.TabStop = false;
            this.groupBoxDataReg.Text = "数据注册";
            // 
            // dataGridViewDataReg
            // 
            this.dataGridViewDataReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDataReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDataReg.Location = new System.Drawing.Point(3, 21);
            this.dataGridViewDataReg.Name = "dataGridViewDataReg";
            this.dataGridViewDataReg.RowTemplate.Height = 27;
            this.dataGridViewDataReg.Size = new System.Drawing.Size(271, 240);
            this.dataGridViewDataReg.TabIndex = 0;
            // 
            // buttonDelConnecte
            // 
            this.buttonDelConnecte.Location = new System.Drawing.Point(172, 7);
            this.buttonDelConnecte.Name = "buttonDelConnecte";
            this.buttonDelConnecte.Size = new System.Drawing.Size(75, 29);
            this.buttonDelConnecte.TabIndex = 1;
            this.buttonDelConnecte.Text = "删除连接";
            this.buttonDelConnecte.UseVisualStyleBackColor = true;
            // 
            // buttonAddConnect
            // 
            this.buttonAddConnect.Location = new System.Drawing.Point(33, 6);
            this.buttonAddConnect.Name = "buttonAddConnect";
            this.buttonAddConnect.Size = new System.Drawing.Size(75, 29);
            this.buttonAddConnect.TabIndex = 1;
            this.buttonAddConnect.Text = "添加连接";
            this.buttonAddConnect.UseVisualStyleBackColor = true;
            this.buttonAddConnect.Click += new System.EventHandler(this.buttonAddConnect_Click);
            // 
            // groupBoxLoadReg
            // 
            this.groupBoxLoadReg.Controls.Add(this.treeView1);
            this.groupBoxLoadReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLoadReg.Location = new System.Drawing.Point(0, 0);
            this.groupBoxLoadReg.Name = "groupBoxLoadReg";
            this.groupBoxLoadReg.Size = new System.Drawing.Size(277, 269);
            this.groupBoxLoadReg.TabIndex = 0;
            this.groupBoxLoadReg.TabStop = false;
            this.groupBoxLoadReg.Text = "加载数据";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonAddConnect);
            this.panel1.Controls.Add(this.buttonDelConnecte);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 40);
            this.panel1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 21);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(271, 245);
            this.treeView1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(781, 537);
            this.dataGridView1.TabIndex = 0;
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 537);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormRegister";
            this.Text = "数据注册";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBoxDataReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataReg)).EndInit();
            this.groupBoxLoadReg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBoxLoadReg;
        private System.Windows.Forms.GroupBox groupBoxDataReg;
        private System.Windows.Forms.DataGridView dataGridViewDataReg;
        private System.Windows.Forms.Button buttonDelConnecte;
        private System.Windows.Forms.Button buttonAddConnect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}