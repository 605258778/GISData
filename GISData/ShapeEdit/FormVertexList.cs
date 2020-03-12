namespace ShapeEdit
{
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using FormBase;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormVertexList : FormBase3
    {
        private IHookHelper _hookHelper;
        private bool _isFirst;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private BarDockControl barDockControlTop;
        private BarManager barManager1;
        private BarButtonItem bbiDeletePoint;
        private BarButtonItem bbiZoomToPoint;
        private SimpleButton btnFinish;
        private IContainer components;
        private GridColumn gcID;
        private GridControl gcVertex;
        private GridColumn gcX;
        private GridColumn gcY;
        private GridView gridView1;
        private ListBoxControl listPart;
        private IGeometry m_Geometry;
        private PanelControl panelControl1;
        private PanelControl panelControl2;
        private PopupMenu popupMenu1;

        public FormVertexList(object hook)
        {
            this.InitializeComponent();
            this._hookHelper = new HookHelperClass();
            this._hookHelper.Hook = hook;
        }

        private void bbiDeletePoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.DeletePoint();
        }

        private void bbiZoomToPoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ZoomToPoint();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Editor.UniqueInstance.FinishSketch();
        }

        private void ClearVertexList()
        {
            this.gcVertex.DataSource = null;
            this.listPart.Items.Clear();
        }

        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("序号", typeof(string));
            table.Columns.Add("X", typeof(string));
            table.Columns.Add("Y", typeof(string));
            return table;
        }

        private void DeletePoint()
        {
            try
            {
                DataRow focusedDataRow = this.gridView1.GetFocusedDataRow();
                if (focusedDataRow != null)
                {
                    int i = Convert.ToInt32(focusedDataRow[0]);
                    int selectedIndex = this.listPart.SelectedIndex;
                    if (selectedIndex >= 0)
                    {
                        IGeometryCollection geometry = this.m_Geometry as IGeometryCollection;
                        IPointCollection points = geometry.get_Geometry(selectedIndex) as IPointCollection;
                        IPoint data = points.get_Point(i);
                        points.RemovePoints(i, 1);
                        IEngineSketchOperation operation = new EngineSketchOperationClass();
                        operation.Start(Editor.UniqueInstance.EngineEditor);
                        operation.SetMenuString("Delete Vertex");
                        esriEngineSketchOperationType esriEngineSketchOperationVertexDeleted = esriEngineSketchOperationType.esriEngineSketchOperationVertexDeleted;
                        object missing = System.Type.Missing;
                        object before = new object();
                        before = selectedIndex;
                        geometry.RemoveGeometries(selectedIndex, 1);
                        geometry.AddGeometry(points as IGeometry, ref before, ref missing);
                        operation.Finish(null, esriEngineSketchOperationVertexDeleted, data);
                        Editor.UniqueInstance.FinishSketch();
                        IEngineEditTask taskByUniqueName = Editor.UniqueInstance.EngineEditor.GetTaskByUniqueName("ControlToolsEditing_ModifyFeatureTask");
                        Editor.UniqueInstance.EngineEditor.CurrentTask = taskByUniqueName;
                        int index = this.listPart.SelectedIndex;
                        if (index >= 0)
                        {
                            this.ShowPointList(index);
                        }
                    }
                }
            }
            catch
            {
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

        private void FormVertexList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        private void FormVertexList_Load(object sender, EventArgs e)
        {
        }

        private void gcVertex_Click(object sender, EventArgs e)
        {
        }

        private void gcVertex_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (this.gridView1.GetSelectedRows().Length > 0))
            {
                this.popupMenu1.ShowPopup(base.PointToScreen(new System.Drawing.Point(e.X + 80, e.Y)));
            }
        }

        private IPoint GetSelectedVertex()
        {
            try
            {
                DataRow focusedDataRow = this.gridView1.GetFocusedDataRow();
                if (focusedDataRow == null)
                {
                    return null;
                }
                int i = Convert.ToInt32(focusedDataRow[0]);
                int selectedIndex = this.listPart.SelectedIndex;
                if (selectedIndex < 0)
                {
                    return null;
                }
                IGeometryCollection geometry = this.m_Geometry as IGeometryCollection;
                IPointCollection points = geometry.get_Geometry(selectedIndex) as IPointCollection;
                return points.get_Point(i);
            }
            catch
            {
                return null;
            }
        }

        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string name = e.Column.Name;
            try
            {
                double pValue = Convert.ToDouble(e.Value);
                this.MovePoint(name, pValue);
            }
            catch
            {
                IPoint selectedVertex = this.GetSelectedVertex();
                if (selectedVertex != null)
                {
                    int focusedRowHandle = this.gridView1.FocusedRowHandle;
                    switch (name)
                    {
                        case "gcX":
                            this.gridView1.SetFocusedValue(selectedVertex.X);
                            break;

                        case "gcY":
                            this.gridView1.SetFocusedValue(selectedVertex.Y);
                            break;
                    }
                }
            }
        }

        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.HighlightPoint();
        }

        private void HighlightPoint()
        {
            try
            {
                IPoint selectedVertex = this.GetSelectedVertex();
                if (selectedVertex != null)
                {
                    IArray pArray = new ArrayClass();
                    pArray.Add(selectedVertex);
                    ((IHookActions) this._hookHelper).DoActionOnMultiple(pArray, esriHookActions.esriHookActionsFlash);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            base.Focus();
            IGeometry editShape = Editor.UniqueInstance.EditShape;
            if (this.m_Geometry != editShape)
            {
                this.m_Geometry = editShape;
                this.InitVertexList();
            }
        }

        private void InitializeComponent()
        {
            this.gcVertex = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.listPart = new DevExpress.XtraEditors.ListBoxControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnFinish = new DevExpress.XtraEditors.SimpleButton();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu();
            this.bbiZoomToPoint = new DevExpress.XtraBars.BarButtonItem();
            this.bbiDeletePoint = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcVertex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcVertex
            // 
            this.gcVertex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcVertex.Location = new System.Drawing.Point(78, 0);
            this.gcVertex.MainView = this.gridView1;
            this.gcVertex.Name = "gcVertex";
            this.gcVertex.Size = new System.Drawing.Size(349, 260);
            this.gcVertex.TabIndex = 1;
            this.gcVertex.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gcVertex.Click += new System.EventHandler(this.gcVertex_Click);
            this.gcVertex.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gcVertex_MouseUp);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcID,
            this.gcX,
            this.gcY});
            this.gridView1.GridControl = this.gcVertex;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowRowSizing = true;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // gcID
            // 
            this.gcID.Caption = "序号";
            this.gcID.FieldName = "序号";
            this.gcID.Name = "gcID";
            this.gcID.OptionsColumn.AllowEdit = false;
            this.gcID.Visible = true;
            this.gcID.VisibleIndex = 0;
            this.gcID.Width = 70;
            // 
            // gcX
            // 
            this.gcX.Caption = "X";
            this.gcX.FieldName = "X";
            this.gcX.Name = "gcX";
            this.gcX.Visible = true;
            this.gcX.VisibleIndex = 1;
            this.gcX.Width = 149;
            // 
            // gcY
            // 
            this.gcY.Caption = "Y";
            this.gcY.FieldName = "Y";
            this.gcY.Name = "gcY";
            this.gcY.Visible = true;
            this.gcY.VisibleIndex = 2;
            this.gcY.Width = 150;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.gcVertex);
            this.panelControl1.Controls.Add(this.listPart);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(427, 260);
            this.panelControl1.TabIndex = 2;
            // 
            // listPart
            // 
            this.listPart.Dock = System.Windows.Forms.DockStyle.Left;
            this.listPart.Location = new System.Drawing.Point(0, 0);
            this.listPart.Name = "listPart";
            this.listPart.Size = new System.Drawing.Size(78, 260);
            this.listPart.TabIndex = 2;
            this.listPart.SelectedIndexChanged += new System.EventHandler(this.listPart_SelectedIndexChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.btnFinish);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 260);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Padding = new System.Windows.Forms.Padding(0, 10, 30, 10);
            this.panelControl2.Size = new System.Drawing.Size(427, 50);
            this.panelControl2.TabIndex = 3;
            // 
            // btnFinish
            // 
            this.btnFinish.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFinish.Location = new System.Drawing.Point(311, 10);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(86, 30);
            this.btnFinish.TabIndex = 0;
            this.btnFinish.Text = "结束节点状态";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiZoomToPoint),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiDeletePoint)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // bbiZoomToPoint
            // 
            this.bbiZoomToPoint.Caption = "缩放到节点";
            this.bbiZoomToPoint.Id = 15;
            this.bbiZoomToPoint.Name = "bbiZoomToPoint";
            this.bbiZoomToPoint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiZoomToPoint_ItemClick);
            // 
            // bbiDeletePoint
            // 
            this.bbiDeletePoint.Caption = "删除";
            this.bbiDeletePoint.Id = 16;
            this.bbiDeletePoint.Name = "bbiDeletePoint";
            this.bbiDeletePoint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiDeletePoint_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bbiZoomToPoint,
            this.bbiDeletePoint});
            this.barManager1.MaxItemId = 17;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(427, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 308);
            this.barDockControlBottom.Size = new System.Drawing.Size(427, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 308);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(427, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 308);
            // 
            // FormVertexList
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.ClientSize = new System.Drawing.Size(427, 308);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVertexList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "节点列表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVertexList_FormClosing);
            this.Load += new System.EventHandler(this.FormVertexList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcVertex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 初始化节点列表
        /// </summary>
        private void InitVertexList()
        {
            this._isFirst = true;
            try
            {
                this.ClearVertexList();
                if (this.m_Geometry != null)
                {
                    IGeometryCollection geometry = this.m_Geometry as IGeometryCollection;
                    for (int i = 0; i < geometry.GeometryCount; i++)
                    {
                        geometry.get_Geometry(i);
                        this.listPart.Items.Add(i);
                    }
                    this.listPart.SelectedIndex = 0;
                    this._isFirst = false;
                    this.ShowPointList(0);
                }
            }
            catch
            {
            }
        }

        private void listPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.listPart.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.ShowPointList(selectedIndex);
            }
        }

        private void MovePoint(string sColumn, double pValue)
        {
            try
            {
                int num;
                int selectedIndex;
                IGeometryCollection geometry;
                IPointCollection points;
                IPoint point;
                IEngineSketchOperation operation;
                DataRow focusedDataRow = this.gridView1.GetFocusedDataRow();
                if (focusedDataRow != null)
                {
                    num = Convert.ToInt32(focusedDataRow[0]);
                    selectedIndex = this.listPart.SelectedIndex;
                    if (selectedIndex >= 0)
                    {
                        geometry = this.m_Geometry as IGeometryCollection;
                        points = geometry.get_Geometry(selectedIndex) as IPointCollection;
                        point = points.get_Point(num);
                        if (point != null)
                        {
                            if (sColumn == "gcX")
                            {
                                point.X = pValue;
                                goto Label_0096;
                            }
                            if (sColumn == "gcY")
                            {
                                point.Y = pValue;
                                goto Label_0096;
                            }
                        }
                    }
                }
                return;
            Label_0096:
                operation = new EngineSketchOperationClass();
                operation.Start(Editor.UniqueInstance.EngineEditor);
                points.UpdatePoint(num, point);
                operation.SetMenuString("Move Vertex");
                esriEngineSketchOperationType esriEngineSketchOperationVertexMoved = esriEngineSketchOperationType.esriEngineSketchOperationVertexMoved;
                object missing = System.Type.Missing;
                object before = new object();
                before = selectedIndex;
                geometry.RemoveGeometries(selectedIndex, 1);
                geometry.AddGeometry(points as IGeometry, ref before, ref missing);
                operation.Finish(null, esriEngineSketchOperationVertexMoved, point);
                Editor.UniqueInstance.FinishSketch();
                IEngineEditTask taskByUniqueName = Editor.UniqueInstance.EngineEditor.GetTaskByUniqueName("ControlToolsEditing_ModifyFeatureTask");
                Editor.UniqueInstance.EngineEditor.CurrentTask = taskByUniqueName;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 显示点列表
        /// </summary>
        /// <param name="index"></param>
        private void ShowPointList(int index)
        {
            if (!this._isFirst)
            {
                try
                {
                    DataTable dataSource = this.gridView1.DataSource as DataTable;
                    if (dataSource == null)
                    {
                        dataSource = this.CreateTable();
                    }
                    dataSource.Clear();
                    IGeometryCollection geometry = this.m_Geometry as IGeometryCollection;
                    IPointCollection points = geometry.get_Geometry(index) as IPointCollection;
                    IPoint point = null;
                    DataRow row = null;
                    for (int i = 0; i < (points.PointCount - 1); i++)
                    {
                        point = points.get_Point(i);
                        row = dataSource.NewRow();
                        row[0] = i.ToString();
                        row[1] = point.X;
                        row[2] = point.Y;
                        dataSource.Rows.Add(row);
                    }
                    this.gcVertex.DataSource = dataSource;
                    this.gcVertex.Refresh();
                }
                catch
                {
                }
            }
        }

        private void ZoomToPoint()
        {
            try
            {
                IPoint selectedVertex = this.GetSelectedVertex();
                if (selectedVertex != null)
                {
                    IEnvelope envelope = new EnvelopeClass();
                    envelope = selectedVertex.Envelope;
                    double num = 0.0;
                    double num2 = 0.0;
                    num = this._hookHelper.ActiveView.FullExtent.Width / 3000.0;
                    num2 = this._hookHelper.ActiveView.FullExtent.Height / 3000.0;
                    if (!((num == 0.0) | (num2 == 0.0)))
                    {
                        envelope.Width = num;
                        envelope.Height = num2;
                        envelope.CenterAt(selectedVertex);
                        this._hookHelper.ActiveView.Extent = envelope;
                        this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this._hookHelper.ActiveView.Extent);
                        this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this._hookHelper.ActiveView.Extent);
                    }
                }
            }
            catch
            {
            }
        }
    }
}

