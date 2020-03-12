namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using TaskManage;
    using Utilities;

    /// <summary>
    /// 画红线工具类
    /// </summary>
    [ProgId("ShapeEdit.HX"), Guid("54fa1829-4b7d-4192-a919-6ce0659120d8"), ClassInterface(ClassInterfaceType.None)]
    public sealed class HX : BaseCommand, ITool
    {
        private ICommand _command;
        private object _firstValue;
        private IHookHelper _hookHelper;
        private ITool _tool;
        private const string mClassName = "ShapeEdit.HX";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private int ps;

        /// <summary>
        /// 画红线工具类：构造器
        /// </summary>
        public HX()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "画红线";
            base.m_message = "画红线";
            base.m_toolTip = "画红线";
            base.m_name = "ShapeEdit_HX";
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        public bool Deactivate()
        {
            IMapControl4 hook = this._hookHelper.Hook as IMapControl4;
            hook.ShowMapTips = false;
            this.ps = 0;
            Editor.UniqueInstance.UpdateFeatureEvent -= new Editor.UpdateFeature(this.UniqueInstance_UpdateFeatureEvent);
            return this._tool.Deactivate();
        }

        public override bool Equals(object obj)
        {
            return this._command.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this._command.GetHashCode();
        }

        public override void OnClick()
        {
            this.ps = 0;
            Editor.UniqueInstance.UpdateFeatureEvent += new Editor.UpdateFeature(this.UniqueInstance_UpdateFeatureEvent);
            IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
            IFeatureLayer pLayer = EditTask.UnderLayers[0] as IFeatureLayer;
            List<SnapAgent> pSnapAgents = new List<SnapAgent> {
                new SnapAgent(pLayer, pLayer.Name, true, false, false)
            };
            Editor.UniqueInstance.RefreshSnapAgent(pSnapAgents, false);
            IMapControl4 hook = this._hookHelper.Hook as IMapControl4;
            hook.ShowMapTips = true;
            pLayer.ShowTips = true;
            int index = pLayer.FeatureClass.Fields.FindField("XMBH");
            if (index != -1)
            {
                pLayer.DisplayField = pLayer.FeatureClass.Fields.get_Field(index).Name;
            }
        }

        public bool OnContextMenu(int x, int y)
        {
            return this._tool.OnContextMenu(x, y);
        }

        public override void OnCreate(object hook)
        {
            if (hook != null)
            {
                if (this._hookHelper == null)
                {
                    this._hookHelper = new HookHelperClass();
                }
                this._hookHelper.Hook = hook;
                this._tool = new ControlsEditingSketchToolClass();
                this._command = (ICommand) this._tool;
                this._command.OnCreate(hook);
            }
        }

        public void OnDblClick()
        {
            this.ps = 0;
            IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
            this._tool.OnDblClick();
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                Editor.UniqueInstance.CancleSketch();
                this._hookHelper.ActiveView.Refresh();
                this.ps = 0;
            }
            else
            {
                this._tool.OnKeyDown(keyCode, shift);
            }
        }

        public void OnKeyUp(int keyCode, int shift)
        {
            this._tool.OnKeyUp(keyCode, shift);
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
            this._tool.OnMouseDown(button, shift, x, y);
            IEngineSnapEnvironment engineEditor = Editor.UniqueInstance.EngineEditor as IEngineSnapEnvironment;
            if (engineEditor.SnapAgentCount < 1)
            {
                Editor.UniqueInstance.OperationStack.Undo();
                MessageBox.Show("没有设置捕捉图层！");
            }
            else
            {
                IFeatureLayer layer = EditTask.UnderLayers[0] as IFeatureLayer;
                IFeatureClass featureClass = layer.FeatureClass;
                IEngineFeatureSnapAgent agent = engineEditor.get_SnapAgent(0) as IEngineFeatureSnapAgent;
                IDataset dataset = featureClass as IDataset;
                IDataset dataset2 = agent.FeatureClass as IDataset;
                if (!dataset.Name.Equals(dataset2.Name))
                {
                    Editor.UniqueInstance.OperationStack.Undo();
                    MessageBox.Show("没有将征占点图层设置为捕捉图层！");
                }
                else
                {
                    IEngineEditSketch sketch = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
                    int index = featureClass.FindField("XMBH");
                    IClone lastPoint = sketch.LastPoint as IClone;
                    IPoint point = lastPoint.Clone() as IPoint;
                    IGeoDataset dataset3 = featureClass as IGeoDataset;
                    if (point.SpatialReference.Name != dataset3.SpatialReference.Name)
                    {
                        point.Project(dataset3.SpatialReference);
                    }
                    IGeometry geometry = (point as ITopologicalOperator).Buffer(0.0002);
                    ISpatialFilter filter = new SpatialFilterClass {
                        Geometry = geometry,
                        SpatialRel = esriSpatialRelEnum.esriSpatialRelEnvelopeIntersects,
                        SubFields = "XMBH",
                        GeometryField = featureClass.ShapeFieldName
                    };
                    IFeatureCursor o = featureClass.Search(filter, false);
                    IFeature feature = o.NextFeature();
                    Marshal.ReleaseComObject(o);
                    o = null;
                    if (feature == null)
                    {
                        Editor.UniqueInstance.OperationStack.Undo();
                        MessageBox.Show("必须捕捉征占点！");
                    }
                    else if (!feature.HasOID || feature.Shape.IsEmpty)
                    {
                        Editor.UniqueInstance.OperationStack.Undo();
                        MessageBox.Show("捕捉的征占点没有OID或图形有问题！");
                    }
                    else
                    {
                        IPolyline polyline = sketch.Geometry as IPolyline;
                        if (this.ps == 0)
                        {
                            int num2;
                            int num3;
                            this._firstValue = feature.get_Value(index);
                            IPoint mapPoint = (feature.Shape as IClone).Clone() as IPoint;
                            mapPoint.Project(polyline.SpatialReference);
                            this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(mapPoint, out num2, out num3);
                            polyline.FromPoint = mapPoint;
                            polyline.ToPoint = mapPoint;
                        }
                        else if (this.ps >= 1)
                        {
                            object obj2 = feature.get_Value(index);
                            if (!this._firstValue.Equals(obj2))
                            {
                                Editor.UniqueInstance.OperationStack.Undo();
                                MessageBox.Show("必须捕捉项目编号相同的征占点！");
                                return;
                            }
                            IPoint point3 = (feature.Shape as IClone).Clone() as IPoint;
                            point3.Project(polyline.SpatialReference);
                            polyline.ToPoint = point3;
                        }
                        sketch.Geometry = polyline;
                        this.ps++;
                    }
                }
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            this._tool.OnMouseMove(button, shift, x, y);
            this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            this._tool.OnMouseUp(button, shift, x, y);
        }

        public void Refresh(int hdc)
        {
            this._tool.Refresh(hdc);
        }

        [ComRegisterFunction, ComVisible(false)]
        private static void RegisterFunction(System.Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        public override string ToString()
        {
            return this._command.ToString();
        }

        private void UniqueInstance_UpdateFeatureEvent(IFeature pFeature)
        {
            if (pFeature != null)
            {
                int index = pFeature.Fields.FindField("XMBH");
                pFeature.set_Value(index, this._firstValue);
                pFeature.Store();
            }
        }

        [ComUnregisterFunction, ComVisible(false)]
        private static void UnregisterFunction(System.Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override int Bitmap
        {
            get
            {
                return this._command.Bitmap;
            }
        }

        public override string Caption
        {
            get
            {
                return base.Caption;
            }
        }

        public override string Category
        {
            get
            {
                return base.Category;
            }
        }

        public override bool Checked
        {
            get
            {
                return this._command.Checked;
            }
        }

        public int Cursor
        {
            get
            {
                return this._tool.Cursor;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (!Editor.UniqueInstance.IsBeingEdited)
                {
                    return false;
                }
                if (((Editor.UniqueInstance.TargetLayer == null) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null)) || (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolyline))
                {
                    return false;
                }
                IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
                return this._command.Enabled;
            }
        }

        public override int HelpContextID
        {
            get
            {
                return this._command.HelpContextID;
            }
        }

        public override string HelpFile
        {
            get
            {
                return this._command.HelpFile;
            }
        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
        }

        public override string Tooltip
        {
            get
            {
                return this._command.Tooltip;
            }
        }
    }
}

