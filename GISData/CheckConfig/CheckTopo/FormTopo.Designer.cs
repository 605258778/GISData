namespace GISData.ChekConfig.CheckTopo
{
    partial class FormTopo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewCheck = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.newStep = new System.Windows.Forms.Button();
            this.DeleteStep = new System.Windows.Forms.Button();
            this.comboBoxCheckType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDataSource = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonTopoSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.buttonTopoSave);
            this.splitContainer1.Size = new System.Drawing.Size(923, 439);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewCheck);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 437);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检查项";
            // 
            // dataGridViewCheck
            // 
            this.dataGridViewCheck.AllowUserToAddRows = false;
            this.dataGridViewCheck.AllowUserToDeleteRows = false;
            this.dataGridViewCheck.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCheck.Location = new System.Drawing.Point(3, 21);
            this.dataGridViewCheck.Name = "dataGridViewCheck";
            this.dataGridViewCheck.ReadOnly = true;
            this.dataGridViewCheck.RowTemplate.Height = 27;
            this.dataGridViewCheck.Size = new System.Drawing.Size(237, 413);
            this.dataGridViewCheck.TabIndex = 0;
            this.dataGridViewCheck.DoubleClick += new System.EventHandler(this.dataGridViewCheck_DoubleClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 53);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.newStep);
            this.splitContainer2.Panel1.Controls.Add(this.DeleteStep);
            this.splitContainer2.Panel1.Controls.Add(this.comboBoxCheckType);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.comboBoxDataSource);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.textBoxName);
            this.splitContainer2.Size = new System.Drawing.Size(655, 393);
            this.splitContainer2.SplitterDistance = 95;
            this.splitContainer2.TabIndex = 0;
            // 
            // newStep
            // 
            this.newStep.Location = new System.Drawing.Point(81, 14);
            this.newStep.Name = "newStep";
            this.newStep.Size = new System.Drawing.Size(51, 23);
            this.newStep.TabIndex = 3;
            this.newStep.Text = "+";
            this.newStep.UseVisualStyleBackColor = true;
            this.newStep.Click += new System.EventHandler(this.newStep_Click);
            // 
            // DeleteStep
            // 
            this.DeleteStep.Location = new System.Drawing.Point(19, 14);
            this.DeleteStep.Name = "DeleteStep";
            this.DeleteStep.Size = new System.Drawing.Size(46, 23);
            this.DeleteStep.TabIndex = 3;
            this.DeleteStep.Text = "-";
            this.DeleteStep.UseVisualStyleBackColor = true;
            this.DeleteStep.Click += new System.EventHandler(this.DeleteStep_Click);
            // 
            // comboBoxCheckType
            // 
            this.comboBoxCheckType.FormattingEnabled = true;
            this.comboBoxCheckType.Location = new System.Drawing.Point(385, 55);
            this.comboBoxCheckType.Name = "comboBoxCheckType";
            this.comboBoxCheckType.Size = new System.Drawing.Size(194, 23);
            this.comboBoxCheckType.TabIndex = 2;
            this.comboBoxCheckType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCheckType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(311, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "检查类型：";
            // 
            // comboBoxDataSource
            // 
            this.comboBoxDataSource.FormattingEnabled = true;
            this.comboBoxDataSource.Location = new System.Drawing.Point(81, 56);
            this.comboBoxDataSource.Name = "comboBoxDataSource";
            this.comboBoxDataSource.Size = new System.Drawing.Size(201, 23);
            this.comboBoxDataSource.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据源：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "名称：";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(319, 14);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(263, 25);
            this.textBoxName.TabIndex = 0;
            // 
            // buttonTopoSave
            // 
            this.buttonTopoSave.Location = new System.Drawing.Point(551, 398);
            this.buttonTopoSave.Name = "buttonTopoSave";
            this.buttonTopoSave.Size = new System.Drawing.Size(75, 32);
            this.buttonTopoSave.TabIndex = 1;
            this.buttonTopoSave.Text = "保存";
            this.buttonTopoSave.UseVisualStyleBackColor = true;
            this.buttonTopoSave.Click += new System.EventHandler(this.buttonTopoSave_Click);
            // 
            // FormTopo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 439);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormTopo";
            this.Text = "FormTopo";
            this.Load += new System.EventHandler(this.FormTopo_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCheck)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCheckType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxDataSource;
        private System.Windows.Forms.Button buttonTopoSave;
        private System.Windows.Forms.DataGridView dataGridViewCheck;
        private System.Windows.Forms.Button newStep;
        private System.Windows.Forms.Button DeleteStep;

    }
}