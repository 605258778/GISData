namespace ShapeEdit
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.esriSystem;
    using FormBase;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 追踪（捕捉）窗体
    /// </summary>
    public class SnapProperty : FormBase3
    {
        private List<SnapAgent> _snapAgent;
        private SimpleButton btApply;
        private SimpleButton btClose;
        private SimpleButton btMoveDown;
        private SimpleButton btMoveUp;
        private CheckEdit ceSnapTip;
        private IContainer components;
        private GridColumn gcEdge;
        private GridColumn gcEndPoint;
        private GridColumn gcLayerName;
        private GridControl gcSnapPro;
        private GridColumn gcVertex;
        private GridView gridView1;
        private RepositoryItemCheckEdit repositoryItemCheckEdit1;

        /// <summary>
        /// 追踪（捕捉）窗体:构造器
        /// </summary>
        public SnapProperty()
        {
            this.InitializeComponent();
            base.FormClosing += new FormClosingEventHandler(this.SnapProperty_FormClosing);
        }

        public void Binding(IMap pMap, ref List<SnapAgent> pSnapAgents)
        {
            this._snapAgent = pSnapAgents;
            if (this._snapAgent.Count < 1)
            {
                UID uid = new UIDClass {
                    Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}"
                };
                IEnumLayer layer = pMap.get_Layers(uid, true);
                layer.Reset();
                IFeatureLayer pLayer = layer.Next() as IFeatureLayer;
                pSnapAgents.Add(new SnapAgent(Editor.UniqueInstance.TargetLayer, Editor.UniqueInstance.TargetLayer.Name, false, false, false));
                while (pLayer != null)
                {
                    if (pLayer.Name.Equals(Editor.UniqueInstance.TargetLayer.Name))
                    {
                        pLayer = layer.Next() as IFeatureLayer;
                    }
                    else
                    {
                        if (pLayer.FeatureClass != null)
                        {
                            pSnapAgents.Add(new SnapAgent(pLayer, pLayer.Name, false, false, false));
                        }
                        pLayer = layer.Next() as IFeatureLayer;
                    }
                }
            }
            this.gcSnapPro.BeginInit();
            this.gcSnapPro.DataSource = this._snapAgent;
            this.gcSnapPro.EndInit();
            if (((IEngineEditProperties2) Editor.UniqueInstance.EngineEditor).SnapTips)
            {
                this.ceSnapTip.Checked = true;
            }
            else
            {
                this.ceSnapTip.Checked = false;
            }
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            Editor.UniqueInstance.RefreshSnapAgent(this._snapAgent, this.ceSnapTip.Checked);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Editor.UniqueInstance.RefreshSnapAgent(this._snapAgent, this.ceSnapTip.Checked);
            base.Close();
        }

        private void btMoveDown_Click(object sender, EventArgs e)
        {
            ColumnView mainView = this.gcSnapPro.MainView as ColumnView;
            int[] selectedRows = mainView.GetSelectedRows();
            if (selectedRows.Length != 0)
            {
                int index = selectedRows[0];
                if (index != (this._snapAgent.Count - 1))
                {
                    SnapAgent item = this._snapAgent[index];
                    this._snapAgent.RemoveAt(index);
                    this._snapAgent.Insert(index + 1, item);
                    this.gcSnapPro.RefreshDataSource();
                    mainView.FocusedRowHandle = index + 1;
                }
            }
        }

        private void btMoveUp_Click(object sender, EventArgs e)
        {
            ColumnView mainView = this.gcSnapPro.MainView as ColumnView;
            int[] selectedRows = mainView.GetSelectedRows();
            if (selectedRows.Length != 0)
            {
                int index = selectedRows[0];
                if (index != 0)
                {
                    SnapAgent item = this._snapAgent[index];
                    this._snapAgent.RemoveAt(index);
                    this._snapAgent.Insert(index - 1, item);
                    this.gcSnapPro.RefreshDataSource();
                    mainView.FocusedRowHandle = index - 1;
                }
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
            this.gcSnapPro = new GridControl();
            this.gridView1 = new GridView();
            this.gcLayerName = new GridColumn();
            this.gcVertex = new GridColumn();
            this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
            this.gcEdge = new GridColumn();
            this.gcEndPoint = new GridColumn();
            this.btMoveDown = new SimpleButton();
            this.btMoveUp = new SimpleButton();
            this.ceSnapTip = new CheckEdit();
            this.btApply = new SimpleButton();
            this.btClose = new SimpleButton();
            this.gcSnapPro.BeginInit();
            this.gridView1.BeginInit();
            this.repositoryItemCheckEdit1.BeginInit();
            this.ceSnapTip.Properties.BeginInit();
            base.SuspendLayout();
            this.gcSnapPro.Location = new Point(2, 2);
            this.gcSnapPro.MainView = this.gridView1;
            this.gcSnapPro.Name = "gcSnapPro";
            this.gcSnapPro.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemCheckEdit1 });
            this.gcSnapPro.Size = new Size(0x14e, 0xe9);
            this.gcSnapPro.TabIndex = 0;
            this.gcSnapPro.ViewCollection.AddRange(new BaseView[] { this.gridView1 });
            this.gridView1.Columns.AddRange(new GridColumn[] { this.gcLayerName, this.gcVertex, this.gcEdge, this.gcEndPoint });
            this.gridView1.GridControl = this.gcSnapPro;
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
            this.gridView1.OptionsView.ShowHorzLines = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.OptionsView.ShowPreviewLines = false;
            this.gridView1.OptionsView.ShowVertLines = false;
            this.gcLayerName.Caption = "图层";
            this.gcLayerName.FieldName = "FeatureLayerName";
            this.gcLayerName.Name = "gcLayerName";
            this.gcLayerName.OptionsColumn.AllowEdit = false;
            this.gcLayerName.Visible = true;
            this.gcLayerName.VisibleIndex = 0;
            this.gcVertex.Caption = "顶点";
            this.gcVertex.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gcVertex.FieldName = "Vertex";
            this.gcVertex.Name = "gcVertex";
            this.gcVertex.Visible = true;
            this.gcVertex.VisibleIndex = 1;
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.gcEdge.Caption = "边";
            this.gcEdge.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gcEdge.FieldName = "Edge";
            this.gcEdge.Name = "gcEdge";
            this.gcEdge.Visible = true;
            this.gcEdge.VisibleIndex = 2;
            this.gcEndPoint.Caption = "端点";
            this.gcEndPoint.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gcEndPoint.FieldName = "EndPoint";
            this.gcEndPoint.Name = "gcEndPoint";
            this.gcEndPoint.Visible = true;
            this.gcEndPoint.VisibleIndex = 3;
            this.btMoveDown.Location = new Point(0x164, 0x7a);
            this.btMoveDown.Name = "btMoveDown";
            this.btMoveDown.Size = new Size(0x3d, 0x1b);
            this.btMoveDown.TabIndex = 2;
            this.btMoveDown.Text = "下移";
            this.btMoveDown.Click += new EventHandler(this.btMoveDown_Click);
            this.btMoveUp.Location = new Point(0x165, 0x57);
            this.btMoveUp.Name = "btMoveUp";
            this.btMoveUp.Size = new Size(0x3b, 0x1b);
            this.btMoveUp.TabIndex = 3;
            this.btMoveUp.Text = "上移";
            this.btMoveUp.Click += new EventHandler(this.btMoveUp_Click);
            this.ceSnapTip.Location = new Point(0x152, 14);
            this.ceSnapTip.Name = "ceSnapTip";
            this.ceSnapTip.Properties.Caption = "显示捕捉信息";
            this.ceSnapTip.Size = new Size(0x6c, 0x13);
            this.ceSnapTip.TabIndex = 9;
            this.btApply.Location = new Point(0x164, 0x9c);
            this.btApply.Name = "btApply";
            this.btApply.Size = new Size(0x3d, 0x1b);
            this.btApply.TabIndex = 10;
            this.btApply.Text = "应用";
            this.btApply.Click += new EventHandler(this.btApply_Click);
            this.btClose.Location = new Point(0x164, 190);
            this.btClose.Name = "btClose";
            this.btClose.Size = new Size(0x3d, 0x1b);
            this.btClose.TabIndex = 11;
            this.btClose.Text = "关闭";
            this.btClose.Click += new EventHandler(this.btClose_Click);
            base.Appearance.BackColor = Color.FromArgb(0xe3, 0xf1, 0xfe);
            base.Appearance.Options.UseBackColor = true;
            base.AutoScaleDimensions = new SizeF(7f, 14f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1bc, 0xed);
            base.Controls.Add(this.btClose);
            base.Controls.Add(this.btApply);
            base.Controls.Add(this.ceSnapTip);
            base.Controls.Add(this.btMoveUp);
            base.Controls.Add(this.btMoveDown);
            base.Controls.Add(this.gcSnapPro);
//            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.LookAndFeel.SkinName = "Blue";
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SnapProperty";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "捕捉属性";
            this.gcSnapPro.EndInit();
            this.gridView1.EndInit();
            this.repositoryItemCheckEdit1.EndInit();
            this.ceSnapTip.Properties.EndInit();
            base.ResumeLayout(false);
        }

        private void SnapProperty_FormClosing(object sender, FormClosingEventArgs e)
        {
            Editor.UniqueInstance.RefreshSnapAgent(this._snapAgent, this.ceSnapTip.Checked);
        }
    }
}

