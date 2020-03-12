namespace ShapeEdit
{
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using FormBase;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class SnapExFeatures : FormBase3
    {
        private List<IFeature> _features;
        private IHookHelper _hook;
        private int _index = -1;
        private IFeatureLayer _layer;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private BarDockControl barDockControlTop;
        private BarManager barManager1;
        private BarButtonItem bbiFlash;
        private BarButtonItem bbiFlashFeature;
        private BarButtonItem bbiPanTo;
        private BarButtonItem bbiSelect;
        private BarButtonItem bbiSelectFeature;
        private BarButtonItem bbiUnSelect;
        private BarButtonItem bbiUnSelectFeature;
        private BarButtonItem bbiValidate;
        private BarButtonItem bbiZoomToFeature;
        private BarSubItem bsiModify;
        private SimpleButton btCancel;
        private SimpleButton btOK;
        private IContainer components;
        private GridColumn gcFeatureID;
        private GridControl gcFeatures;
        private GridView gridView1;
        private GridView gridView2;
        private PopupMenu popupMenu1;

        public SnapExFeatures(List<IFeature> pFeatures, IHookHelper pHook, IFeatureLayer pLayer)
        {
            this.InitializeComponent();
            this._features = pFeatures;
            this._hook = pHook;
            this._layer = pLayer;
            this.Init();
        }

        private void bbiFlashFeature_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] selectedRows = (this.gcFeatures.MainView as ColumnView).GetSelectedRows();
            IFeature feature = this._features[selectedRows[0]];
            IArray pArray = new ArrayClass();
            pArray.Add(feature.ShapeCopy);
            ((IHookActions) this._hook).DoActionOnMultiple(pArray, esriHookActions.esriHookActionsFlash);
        }

        private void bbiSelectFeature_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] selectedRows = (this.gcFeatures.MainView as ColumnView).GetSelectedRows();
            IFeature feature = this._features[selectedRows[0]];
            this._hook.ActiveView.FocusMap.SelectFeature(this._layer, feature);
            this._hook.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, feature, feature.Shape.Envelope);
        }

        private void bbiUnSelectFeature_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] selectedRows = (this.gcFeatures.MainView as ColumnView).GetSelectedRows();
            IFeature feature = this._features[selectedRows[0]];
            IFeatureSelection selection = (IFeatureSelection) this._layer;
            ISelectionSet selectionSet = selection.SelectionSet;
            int oID = feature.OID;
            selectionSet.RemoveList(1, ref oID);
            this._hook.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this._hook.ActiveView.Extent);
        }

        private void bbiZoomToFeature_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] selectedRows = (this.gcFeatures.MainView as ColumnView).GetSelectedRows();
            IFeature feature = this._features[selectedRows[0]];
            IEnvelope envelope = new EnvelopeClass();
            IPoint p = new PointClass();
            envelope.SpatialReference = p.SpatialReference = feature.Shape.SpatialReference;
            double num = (10.0 * this._hook.ActiveView.FocusMap.MapScale) / 10000.0;
            envelope.PutCoords(feature.Shape.Envelope.XMin - num, feature.Shape.Envelope.YMin - num, feature.Shape.Envelope.XMax + num, feature.Shape.Envelope.YMax + num);
            p.X = envelope.XMin + (envelope.Width / 2.0);
            p.Y = envelope.YMin + (envelope.Height / 2.0);
            envelope.CenterAt(p);
            this._hook.ActiveView.Extent = envelope;
            this._hook.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this._hook.ActiveView.Extent);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            int[] selectedRows = (this.gcFeatures.MainView as ColumnView).GetSelectedRows();
            this._index = selectedRows[0];
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gcFeatures_Click(object sender, EventArgs e)
        {
            int[] selectedRows = (this.gcFeatures.MainView as ColumnView).GetSelectedRows();
            IFeature feature = this._features[selectedRows[0]];
            IArray pArray = new ArrayClass();
            pArray.Add(feature.ShapeCopy);
            ((IHookActions) this._hook).DoActionOnMultiple(pArray, esriHookActions.esriHookActionsFlash);
        }

        private void gcFeatures_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (this.gridView1.GetSelectedRows().Length > 0))
            {
                this.popupMenu1.ShowPopup(base.PointToScreen(new System.Drawing.Point(e.X, e.Y)));
            }
        }

        private void Init()
        {
            DataTable table = new DataTable();
            table.Columns.Add("FEATUREID");
            this.gcFeatures.DataSource = table;
            BaseView mainView = this.gcFeatures.MainView;
            foreach (IFeature feature in this._features)
            {
                DataRow row = table.NewRow();
                row[0] = feature.OID;
                table.Rows.Add(row);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.gridView2 = new GridView();
            this.gcFeatures = new GridControl();
            this.gridView1 = new GridView();
            this.gcFeatureID = new GridColumn();
            this.btOK = new SimpleButton();
            this.btCancel = new SimpleButton();
            this.barManager1 = new BarManager(this.components);
            this.barDockControlTop = new BarDockControl();
            this.barDockControlBottom = new BarDockControl();
            this.barDockControlLeft = new BarDockControl();
            this.barDockControlRight = new BarDockControl();
            this.bbiPanTo = new BarButtonItem();
            this.bbiSelect = new BarButtonItem();
            this.bbiUnSelect = new BarButtonItem();
            this.bbiFlash = new BarButtonItem();
            this.bbiValidate = new BarButtonItem();
            this.bsiModify = new BarSubItem();
            this.bbiSelectFeature = new BarButtonItem();
            this.bbiUnSelectFeature = new BarButtonItem();
            this.bbiZoomToFeature = new BarButtonItem();
            this.bbiFlashFeature = new BarButtonItem();
            this.popupMenu1 = new PopupMenu(this.components);
            this.gridView2.BeginInit();
            this.gcFeatures.BeginInit();
            this.gridView1.BeginInit();
            this.barManager1.BeginInit();
            this.popupMenu1.BeginInit();
            base.SuspendLayout();
            this.gridView2.GridControl = this.gcFeatures;
            this.gridView2.Name = "gridView2";
            this.gcFeatures.Dock = DockStyle.Top;
            this.gcFeatures.EmbeddedNavigator.Name = "";
            this.gcFeatures.Location = new System.Drawing.Point(0, 0);
            this.gcFeatures.MainView = this.gridView1;
            this.gcFeatures.Name = "gcFeatures";
            this.gcFeatures.Size = new Size(0xcb, 0xc0);
            this.gcFeatures.TabIndex = 0;
            this.gcFeatures.ViewCollection.AddRange(new BaseView[] { this.gridView1, this.gridView2 });
            this.gcFeatures.MouseDown += new MouseEventHandler(this.gcFeatures_MouseDown);
            this.gcFeatures.Click += new EventHandler(this.gcFeatures_Click);
            this.gridView1.Columns.AddRange(new GridColumn[] { this.gcFeatureID });
            this.gridView1.GridControl = this.gcFeatures;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gcFeatureID.Caption = "要素编号";
            this.gcFeatureID.FieldName = "FEATUREID";
            this.gcFeatureID.Name = "gcFeatureID";
            this.gcFeatureID.Visible = true;
            this.gcFeatureID.VisibleIndex = 0;
            this.btOK.Location = new System.Drawing.Point(0x17, 0xc6);
            this.btOK.Name = "btOK";
            this.btOK.Size = new Size(0x34, 0x17);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "确定";
            this.btOK.Click += new EventHandler(this.btOK_Click);
            this.btCancel.Location = new System.Drawing.Point(120, 0xc6);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x34, 0x17);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "取消";
            this.btCancel.Click += new EventHandler(this.btCancel_Click);
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new BarItem[] { this.bbiPanTo, this.bbiSelect, this.bbiUnSelect, this.bbiFlash, this.bbiValidate, this.bsiModify, this.bbiSelectFeature, this.bbiUnSelectFeature, this.bbiZoomToFeature, this.bbiFlashFeature });
            this.barManager1.MaxItemId = 14;
            this.bbiPanTo.Caption = "查看错误";
            this.bbiPanTo.Id = 0;
            this.bbiPanTo.Name = "bbiPanTo";
            this.bbiSelect.Caption = "选择要素";
            this.bbiSelect.Id = 2;
            this.bbiSelect.Name = "bbiSelect";
            this.bbiUnSelect.Caption = "取消选择";
            this.bbiUnSelect.Id = 3;
            this.bbiUnSelect.Name = "bbiUnSelect";
            this.bbiFlash.Caption = "闪烁要素";
            this.bbiFlash.Id = 4;
            this.bbiFlash.Name = "bbiFlash";
            this.bbiValidate.Caption = "验证拓扑";
            this.bbiValidate.Id = 7;
            this.bbiValidate.Name = "bbiValidate";
            this.bsiModify.Caption = "修改建议";
            this.bsiModify.Id = 9;
            this.bsiModify.Name = "bsiModify";
            this.bbiSelectFeature.Caption = "选择要素";
            this.bbiSelectFeature.Id = 10;
            this.bbiSelectFeature.Name = "bbiSelectFeature";
            this.bbiSelectFeature.ItemClick += new ItemClickEventHandler(this.bbiSelectFeature_ItemClick);
            this.bbiUnSelectFeature.Caption = "取消选择";
            this.bbiUnSelectFeature.Id = 11;
            this.bbiUnSelectFeature.Name = "bbiUnSelectFeature";
            this.bbiUnSelectFeature.ItemClick += new ItemClickEventHandler(this.bbiUnSelectFeature_ItemClick);
            this.bbiZoomToFeature.Caption = "缩放到要素";
            this.bbiZoomToFeature.Id = 12;
            this.bbiZoomToFeature.Name = "bbiZoomToFeature";
            this.bbiZoomToFeature.ItemClick += new ItemClickEventHandler(this.bbiZoomToFeature_ItemClick);
            this.bbiFlashFeature.Caption = "闪烁要素";
            this.bbiFlashFeature.Id = 13;
            this.bbiFlashFeature.Name = "bbiFlashFeature";
            this.bbiFlashFeature.ItemClick += new ItemClickEventHandler(this.bbiFlashFeature_ItemClick);
            this.popupMenu1.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.bbiSelectFeature), new LinkPersistInfo(this.bbiUnSelectFeature), new LinkPersistInfo(this.bbiFlashFeature), new LinkPersistInfo(this.bbiZoomToFeature) });
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            base.Appearance.BackColor = Color.FromArgb(0xe3, 0xf1, 0xfe);
            base.Appearance.Options.UseBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xcb, 0xe7);
            base.Controls.Add(this.btCancel);
            base.Controls.Add(this.btOK);
            base.Controls.Add(this.gcFeatures);
            base.Controls.Add(this.barDockControlLeft);
            base.Controls.Add(this.barDockControlRight);
            base.Controls.Add(this.barDockControlBottom);
            base.Controls.Add(this.barDockControlTop);
            base.LookAndFeel.SkinName = "Blue";
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SnapExFeatures";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "选择追踪小班";
            this.gridView2.EndInit();
            this.gcFeatures.EndInit();
            this.gridView1.EndInit();
            this.barManager1.EndInit();
            this.popupMenu1.EndInit();
            base.ResumeLayout(false);
        }

        public int Index
        {
            get
            {
                return this._index;
            }
        }
    }
}

