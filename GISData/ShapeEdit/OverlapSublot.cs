namespace ShapeEdit
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using FormBase;
    using FunFactory;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class OverlapSublot : FormBase3
    {
        private int _selectedIndex;
        private IContainer components;
        private LabelControl lbTip;
        private IHookHelper m_HookHelper;
        private IList<IFeature> m_OverlapFeatures;
        private RadioGroup rgSublot;
        private SimpleButton sbCancel;
        private SimpleButton sbOK;

        public OverlapSublot(object pHook, IList<IFeature> pList)
        {
            this.InitializeComponent();
            this.m_HookHelper = new HookHelperClass();
            this.m_HookHelper.Hook = pHook;
            this.m_OverlapFeatures = pList;
            IFeature feature = null;
            this.rgSublot.Properties.Items.Clear();
            for (int i = 0; i < this.m_OverlapFeatures.Count; i++)
            {
                RadioGroupItem item = new RadioGroupItem();
                feature = this.m_OverlapFeatures[i];
                string str = feature.OID.ToString();
                item.Description = str;
                this.rgSublot.Properties.Items.Add(item);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbTip = new LabelControl();
            this.rgSublot = new RadioGroup();
            this.sbOK = new SimpleButton();
            this.sbCancel = new SimpleButton();
            this.rgSublot.Properties.BeginInit();
            base.SuspendLayout();
            this.lbTip.Location = new System.Drawing.Point(12, 5);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new Size(0xa8, 14);
            this.lbTip.TabIndex = 0;
            this.lbTip.Text = "请选择要保留重叠部分的小班ID";
            this.rgSublot.Location = new System.Drawing.Point(12, 0x19);
            this.rgSublot.Name = "rgSublot";
            this.rgSublot.Properties.Appearance.BackColor = Color.FromArgb(0xe3, 0xf1, 0xfe);
            this.rgSublot.Properties.Appearance.Options.UseBackColor = true;
            this.rgSublot.Properties.BorderStyle = BorderStyles.Simple;
            this.rgSublot.Properties.Items.AddRange(new RadioGroupItem[] { new RadioGroupItem(null, "1"), new RadioGroupItem(null, "2") });
            this.rgSublot.Size = new Size(0x110, 30);
            this.rgSublot.TabIndex = 1;
            this.rgSublot.SelectedIndexChanged += new EventHandler(this.rgSublot_SelectedIndexChanged);
            this.sbOK.Location = new System.Drawing.Point(0x80, 0x3d);
            this.sbOK.Name = "sbOK";
            this.sbOK.Size = new Size(60, 0x17);
            this.sbOK.TabIndex = 2;
            this.sbOK.Text = "确定";
            this.sbOK.Click += new EventHandler(this.sbOK_Click);
            this.sbCancel.Location = new System.Drawing.Point(0xd1, 0x3d);
            this.sbCancel.Name = "sbCancel";
            this.sbCancel.Size = new Size(60, 0x17);
            this.sbCancel.TabIndex = 3;
            this.sbCancel.Text = "取消";
            this.sbCancel.Click += new EventHandler(this.sbCancel_Click);
            base.Appearance.BackColor = Color.FromArgb(0xe3, 0xf1, 0xfe);
            base.Appearance.Options.UseBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x121, 0x5b);
            base.Controls.Add(this.sbCancel);
            base.Controls.Add(this.sbOK);
            base.Controls.Add(this.rgSublot);
            base.Controls.Add(this.lbTip);
//            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.LookAndFeel.SkinName = "Blue";
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "OverlapSublot";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "合并";
            this.rgSublot.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void rgSublot_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.rgSublot.SelectedIndex;
            IFeature feature = null;
            feature = this.m_OverlapFeatures[selectedIndex];
            IGeometry shapeCopy = feature.ShapeCopy;
            shapeCopy = GISFunFactory.UnitFun.ConvertPoject(shapeCopy, this.m_HookHelper.ActiveView.FocusMap.SpatialReference);
            (this.m_HookHelper.Hook as IMapControl2).FlashShape(shapeCopy, 3, 300, null);
        }

        private void sbCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void sbOK_Click(object sender, EventArgs e)
        {
            this._selectedIndex = this.rgSublot.SelectedIndex;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }
        }

        public string Tip
        {
            set
            {
                this.lbTip.Text = value;
            }
        }
    }
}

