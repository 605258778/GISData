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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBoxDataReg = new System.Windows.Forms.GroupBox();
            this.dataGridViewDataReg = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddConnect = new System.Windows.Forms.Button();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.buttonDelConnecte = new System.Windows.Forms.Button();
            this.groupBoxLoadReg = new System.Windows.Forms.GroupBox();
            this.treeViewReg = new System.Windows.Forms.TreeView();
            this.dataGridViewFieldView = new System.Windows.Forms.DataGridView();
            this.ZDMC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZDBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZDLX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZDZZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SFZJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZDCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XSWS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NFWK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SFZD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SFXS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZDY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZDYTJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.panel1.SuspendLayout();
            this.groupBoxLoadReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFieldView)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewFieldView);
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
            this.dataGridViewDataReg.AllowUserToAddRows = false;
            this.dataGridViewDataReg.AllowUserToDeleteRows = false;
            this.dataGridViewDataReg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDataReg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDataReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDataReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDataReg.Location = new System.Drawing.Point(3, 21);
            this.dataGridViewDataReg.Name = "dataGridViewDataReg";
            this.dataGridViewDataReg.ReadOnly = true;
            this.dataGridViewDataReg.RowTemplate.Height = 27;
            this.dataGridViewDataReg.Size = new System.Drawing.Size(271, 240);
            this.dataGridViewDataReg.TabIndex = 0;
            this.dataGridViewDataReg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDataReg_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonAddConnect);
            this.panel1.Controls.Add(this.buttonRegister);
            this.panel1.Controls.Add(this.buttonDelConnecte);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 40);
            this.panel1.TabIndex = 2;
            // 
            // buttonAddConnect
            // 
            this.buttonAddConnect.Location = new System.Drawing.Point(12, 8);
            this.buttonAddConnect.Name = "buttonAddConnect";
            this.buttonAddConnect.Size = new System.Drawing.Size(75, 29);
            this.buttonAddConnect.TabIndex = 1;
            this.buttonAddConnect.Text = "添加连接";
            this.buttonAddConnect.UseVisualStyleBackColor = true;
            this.buttonAddConnect.Click += new System.EventHandler(this.buttonAddConnect_Click);
            // 
            // buttonRegister
            // 
            this.buttonRegister.Location = new System.Drawing.Point(174, 8);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(75, 29);
            this.buttonRegister.TabIndex = 1;
            this.buttonRegister.Text = "注册";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // buttonDelConnecte
            // 
            this.buttonDelConnecte.Location = new System.Drawing.Point(93, 8);
            this.buttonDelConnecte.Name = "buttonDelConnecte";
            this.buttonDelConnecte.Size = new System.Drawing.Size(75, 29);
            this.buttonDelConnecte.TabIndex = 1;
            this.buttonDelConnecte.Text = "删除连接";
            this.buttonDelConnecte.UseVisualStyleBackColor = true;
            this.buttonDelConnecte.Click += new System.EventHandler(this.buttonDelConnecte_Click);
            // 
            // groupBoxLoadReg
            // 
            this.groupBoxLoadReg.Controls.Add(this.treeViewReg);
            this.groupBoxLoadReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLoadReg.Location = new System.Drawing.Point(0, 0);
            this.groupBoxLoadReg.Name = "groupBoxLoadReg";
            this.groupBoxLoadReg.Size = new System.Drawing.Size(277, 269);
            this.groupBoxLoadReg.TabIndex = 0;
            this.groupBoxLoadReg.TabStop = false;
            this.groupBoxLoadReg.Text = "加载数据";
            // 
            // treeViewReg
            // 
            this.treeViewReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewReg.Location = new System.Drawing.Point(3, 21);
            this.treeViewReg.Name = "treeViewReg";
            this.treeViewReg.Size = new System.Drawing.Size(271, 245);
            this.treeViewReg.TabIndex = 0;
            this.treeViewReg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewReg_MouseDown);
            // 
            // dataGridViewFieldView
            // 
            this.dataGridViewFieldView.AllowUserToAddRows = false;
            this.dataGridViewFieldView.AllowUserToDeleteRows = false;
            this.dataGridViewFieldView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFieldView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFieldView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ZDMC,
            this.ZDBM,
            this.ZDLX,
            this.ZDZZ,
            this.SFZJ,
            this.ZDCD,
            this.XSWS,
            this.NFWK,
            this.SFZD,
            this.SFXS,
            this.ZDY,
            this.ZDYTJ});
            this.dataGridViewFieldView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFieldView.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFieldView.Name = "dataGridViewFieldView";
            this.dataGridViewFieldView.ReadOnly = true;
            this.dataGridViewFieldView.RowTemplate.Height = 27;
            this.dataGridViewFieldView.Size = new System.Drawing.Size(781, 537);
            this.dataGridViewFieldView.TabIndex = 0;
            // 
            // ZDMC
            // 
            this.ZDMC.HeaderText = "字段名称";
            this.ZDMC.Name = "ZDMC";
            this.ZDMC.ReadOnly = true;
            // 
            // ZDBM
            // 
            this.ZDBM.HeaderText = "字段别名";
            this.ZDBM.Name = "ZDBM";
            this.ZDBM.ReadOnly = true;
            // 
            // ZDLX
            // 
            this.ZDLX.HeaderText = "字段类型";
            this.ZDLX.Name = "ZDLX";
            this.ZDLX.ReadOnly = true;
            // 
            // ZDZZ
            // 
            this.ZDZZ.HeaderText = "自动增值";
            this.ZDZZ.Name = "ZDZZ";
            this.ZDZZ.ReadOnly = true;
            // 
            // SFZJ
            // 
            this.SFZJ.HeaderText = "是否主键";
            this.SFZJ.Name = "SFZJ";
            this.SFZJ.ReadOnly = true;
            // 
            // ZDCD
            // 
            this.ZDCD.HeaderText = "字段长度";
            this.ZDCD.Name = "ZDCD";
            this.ZDCD.ReadOnly = true;
            // 
            // XSWS
            // 
            this.XSWS.HeaderText = "小数位数";
            this.XSWS.Name = "XSWS";
            this.XSWS.ReadOnly = true;
            // 
            // NFWK
            // 
            this.NFWK.HeaderText = "能否为空";
            this.NFWK.Name = "NFWK";
            this.NFWK.ReadOnly = true;
            // 
            // SFZD
            // 
            this.SFZD.HeaderText = "是否只读";
            this.SFZD.Name = "SFZD";
            this.SFZD.ReadOnly = true;
            // 
            // SFXS
            // 
            this.SFXS.HeaderText = "是否显示";
            this.SFXS.Name = "SFXS";
            this.SFXS.ReadOnly = true;
            // 
            // ZDY
            // 
            this.ZDY.HeaderText = "字典域";
            this.ZDY.Name = "ZDY";
            this.ZDY.ReadOnly = true;
            // 
            // ZDYTJ
            // 
            this.ZDYTJ.HeaderText = "字典域条件";
            this.ZDYTJ.Name = "ZDYTJ";
            this.ZDYTJ.ReadOnly = true;
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 537);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormRegister";
            this.Text = "数据注册";
            this.Load += new System.EventHandler(this.FormRegister_Load);
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
            this.panel1.ResumeLayout(false);
            this.groupBoxLoadReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFieldView)).EndInit();
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
        private System.Windows.Forms.TreeView treeViewReg;
        private System.Windows.Forms.DataGridView dataGridViewFieldView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDMC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDBM;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDLX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDZZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn SFZJ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn XSWS;
        private System.Windows.Forms.DataGridViewTextBoxColumn NFWK;
        private System.Windows.Forms.DataGridViewTextBoxColumn SFZD;
        private System.Windows.Forms.DataGridViewTextBoxColumn SFXS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZDYTJ;
        private System.Windows.Forms.Button buttonRegister;
    }
}