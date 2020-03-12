namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// 编辑--挖空
    /// </summary>
    [Guid("6d419356-c5d5-4a94-a066-9a5d6d37683c"), ProgId("ShapeEdit.Tool1"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Erase : BaseCommand, ITool
    {
        private int _cursor;
        private ICommand m_command;
        private IFeature m_Feature;
        private IHookHelper m_hookHelper;
        private ITool m_tool;

        /// <summary>
        /// 编辑--挖空：构造器
        /// </summary>
        public Erase()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "挖空";
            base.m_message = "挖空";
            base.m_toolTip = "挖空";
            base.m_name = "ShapeEdit_Erase";
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
            return this.m_tool.Deactivate();
        }

        public override bool Equals(object obj)
        {
            return this.m_command.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.m_command.GetHashCode();
        }

        public override void OnClick()
        {
            this.m_command.OnClick();
            this._cursor = this.m_tool.Cursor;
        }

        public bool OnContextMenu(int x, int y)
        {
            return this.m_tool.OnContextMenu(x, y);
        }

        public override void OnCreate(object hook)
        {
            if (this.m_hookHelper == null)
            {
                this.m_hookHelper = new HookHelperClass();
            }
            this.m_hookHelper.Hook = hook;
            this.m_tool = new ControlsEditingSketchToolClass();
            this.m_command = (ICommand) this.m_tool;
            this.m_command.OnCreate(hook);
            this._cursor = this.m_tool.Cursor;
        }

        public void OnDblClick()
        {
            try
            {
                IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
                IEngineEditSketch sketch = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
                IGeometry other = sketch.Geometry;
                if (this.m_Feature == null)
                {
                    MessageBox.Show("当前无要素可进行挖空！", "提示");
                    Editor.UniqueInstance.CancleSketch();
                }
                else
                {
                    if (other.SpatialReference != this.m_Feature.Shape.SpatialReference)
                    {
                        other.Project(this.m_Feature.Shape.SpatialReference);
                        other.SpatialReference = this.m_Feature.Shape.SpatialReference;
                    }
                    ITopologicalOperator2 @operator = other as ITopologicalOperator2;
                    @operator.IsKnownSimple_2 = false;
                    @operator.Simplify();
                    ITopologicalOperator2 shapeCopy = this.m_Feature.ShapeCopy as ITopologicalOperator2;
                    shapeCopy.IsKnownSimple_2 = false;
                    shapeCopy.Simplify();
                    IGeometry geometry2 = shapeCopy.Difference(other);
                    Editor.UniqueInstance.StartEditOperation();
                    this.m_Feature.Shape = geometry2;
                    this.m_Feature.Store();
                    Editor.UniqueInstance.StopEditOperation("erase");
                    Editor.UniqueInstance.CancleSketch();
                    this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, null);
                }
            }
            catch
            {
                Editor.UniqueInstance.CancleSketch();
            }
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            this.m_tool.OnKeyDown(keyCode, shift);
        }

        public void OnKeyUp(int keyCode, int Shift)
        {
            if (keyCode == 0x1b)
            {
                Editor.UniqueInstance.CancleSketch();
                this.m_hookHelper.ActiveView.Refresh();
            }
            else
            {
                this.m_tool.OnKeyDown(keyCode, Shift);
            }
        }

        public void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            this.m_tool.OnMouseDown(Button, Shift, X, Y);
        }

        public void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            this.m_tool.OnMouseMove(Button, Shift, X, Y);
        }

        public void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            this.m_tool.OnMouseUp(Button, Shift, X, Y);
            IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
            IEngineEditSketch sketch = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
            IPointCollection geometry = sketch.Geometry as IPointCollection;
            if (geometry.PointCount == 2)
            {
                IPoint lastPoint = sketch.LastPoint;
                IFeatureLayer targetLayer = Editor.UniqueInstance.TargetLayer;
                IFeatureClass featureClass = targetLayer.FeatureClass;
                ISpatialFilter queryFilter = null;
                queryFilter = new SpatialFilterClass {
                    Geometry = lastPoint,
                    GeometryField = featureClass.ShapeFieldName,
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin
                };
                IFeatureCursor cursor = targetLayer.Search(queryFilter, false);
                IFeature feature = cursor.NextFeature();
                int num = 0;
                while (feature != null)
                {
                    this.m_Feature = feature;
                    num++;
                    feature = cursor.NextFeature();
                }
                switch (num)
                {
                    case 1:
                        return;

                    case 0:
                        MessageBox.Show("当前无要素可进行挖空！", "提示");
                        break;
                }
                if (num > 1)
                {
                    MessageBox.Show("超过一个要素被选中，无法进行挖空！", "提示");
                }
                Editor.UniqueInstance.CancleSketch();
                this.m_hookHelper.ActiveView.Refresh();
            }
        }

        public void Refresh(int hdc)
        {
            this.m_tool.Refresh(hdc);
        }

        [ComRegisterFunction, ComVisible(false)]
        private static void RegisterFunction(System.Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        public override string ToString()
        {
            return this.m_command.ToString();
        }

        [ComVisible(false), ComUnregisterFunction]
        private static void UnregisterFunction(System.Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override int Bitmap
        {
            get
            {
                return this.m_command.Bitmap;
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
                return this.m_command.Checked;
            }
        }

        public int Cursor
        {
            get
            {
                return this._cursor;
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
                return ((((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null)) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)) && this.m_command.Enabled);
            }
        }

        public override int HelpContextID
        {
            get
            {
                return this.m_command.HelpContextID;
            }
        }

        public override string HelpFile
        {
            get
            {
                return this.m_command.HelpFile;
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
                return base.Tooltip;
            }
        }
    }
}

