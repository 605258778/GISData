namespace GISData
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工程设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据质检ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据注册ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字典管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.质检配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.任务管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报表设计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.表格ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 28);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1201, 28);
            this.axToolbarControl1.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.开始ToolStripMenuItem,
            this.工程设置ToolStripMenuItem,
            this.数据质检ToolStripMenuItem,
            this.数据注册ToolStripMenuItem,
            this.字典管理ToolStripMenuItem,
            this.质检配置ToolStripMenuItem,
            this.任务管理ToolStripMenuItem,
            this.报表设计ToolStripMenuItem,
            this.用户管理ToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1201, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 开始ToolStripMenuItem
            // 
            this.开始ToolStripMenuItem.Name = "开始ToolStripMenuItem";
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.开始ToolStripMenuItem.Text = "开始";
            this.开始ToolStripMenuItem.Click += new System.EventHandler(this.开始ToolStripMenuItem_Click);
            // 
            // 工程设置ToolStripMenuItem
            // 
            this.工程设置ToolStripMenuItem.Name = "工程设置ToolStripMenuItem";
            this.工程设置ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.工程设置ToolStripMenuItem.Text = "工程设置";
            this.工程设置ToolStripMenuItem.Click += new System.EventHandler(this.工程设置ToolStripMenuItem_Click);
            // 
            // 数据质检ToolStripMenuItem
            // 
            this.数据质检ToolStripMenuItem.Name = "数据质检ToolStripMenuItem";
            this.数据质检ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.数据质检ToolStripMenuItem.Text = "数据质检";
            this.数据质检ToolStripMenuItem.Click += new System.EventHandler(this.数据质检ToolStripMenuItem_Click);
            // 
            // 数据注册ToolStripMenuItem
            // 
            this.数据注册ToolStripMenuItem.Name = "数据注册ToolStripMenuItem";
            this.数据注册ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.数据注册ToolStripMenuItem.Text = "数据注册";
            this.数据注册ToolStripMenuItem.Click += new System.EventHandler(this.数据注册ToolStripMenuItem_Click);
            // 
            // 字典管理ToolStripMenuItem
            // 
            this.字典管理ToolStripMenuItem.Name = "字典管理ToolStripMenuItem";
            this.字典管理ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.字典管理ToolStripMenuItem.Text = "字典管理";
            this.字典管理ToolStripMenuItem.Click += new System.EventHandler(this.字典管理ToolStripMenuItem_Click);
            // 
            // 质检配置ToolStripMenuItem
            // 
            this.质检配置ToolStripMenuItem.Name = "质检配置ToolStripMenuItem";
            this.质检配置ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.质检配置ToolStripMenuItem.Text = "质检配置";
            this.质检配置ToolStripMenuItem.Click += new System.EventHandler(this.质检配置ToolStripMenuItem_Click);
            // 
            // 任务管理ToolStripMenuItem
            // 
            this.任务管理ToolStripMenuItem.Name = "任务管理ToolStripMenuItem";
            this.任务管理ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.任务管理ToolStripMenuItem.Text = "任务管理";
            this.任务管理ToolStripMenuItem.Click += new System.EventHandler(this.任务管理ToolStripMenuItem_Click);
            // 
            // 报表设计ToolStripMenuItem
            // 
            this.报表设计ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.表格ToolStripMenuItem,
            this.文档ToolStripMenuItem});
            this.报表设计ToolStripMenuItem.Name = "报表设计ToolStripMenuItem";
            this.报表设计ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.报表设计ToolStripMenuItem.Text = "报表设计";
            this.报表设计ToolStripMenuItem.Click += new System.EventHandler(this.报表设计ToolStripMenuItem_Click);
            // 
            // 表格ToolStripMenuItem
            // 
            this.表格ToolStripMenuItem.Name = "表格ToolStripMenuItem";
            this.表格ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.表格ToolStripMenuItem.Text = "表格";
            this.表格ToolStripMenuItem.Click += new System.EventHandler(this.表格ToolStripMenuItem_Click);
            // 
            // 文档ToolStripMenuItem
            // 
            this.文档ToolStripMenuItem.Name = "文档ToolStripMenuItem";
            this.文档ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.文档ToolStripMenuItem.Text = "文档";
            this.文档ToolStripMenuItem.Click += new System.EventHandler(this.文档ToolStripMenuItem_Click);
            // 
            // 用户管理ToolStripMenuItem
            // 
            this.用户管理ToolStripMenuItem.Name = "用户管理ToolStripMenuItem";
            this.用户管理ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.用户管理ToolStripMenuItem.Text = "用户管理";
            this.用户管理ToolStripMenuItem.Click += new System.EventHandler(this.用户管理ToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.testToolStripMenuItem.Text = "退出";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 575);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1201, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 56);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.axLicenseControl1);
            this.splitContainer1.Panel1.Controls.Add(this.axTOCControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.axMapControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1201, 519);
            this.splitContainer1.SplitterDistance = 301;
            this.splitContainer1.TabIndex = 5;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(267, 187);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            this.axLicenseControl1.Enter += new System.EventHandler(this.axLicenseControl1_Enter);
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(301, 519);
            this.axTOCControl1.TabIndex = 0;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(896, 519);
            this.axMapControl1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 597);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "数据验收平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据注册ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 字典管理ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.ToolStripMenuItem 质检配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据质检ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工程设置ToolStripMenuItem;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.ToolStripMenuItem 任务管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报表设计ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 表格ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户管理ToolStripMenuItem;
    }
}

