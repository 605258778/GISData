namespace ShapeEdit
{
    using DevExpress.XtraEditors;
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
    using Utilities;

    /// <summary>
    /// 编辑草图:工具类（只有在图层为编辑草图的状态下，才能“新增班块”）
    /// </summary>
    [Guid("57c6ec56-9b18-4bca-9207-d216a781b1fb"), ProgId("ShapeEdit.EditingSketch"), ClassInterface(ClassInterfaceType.None)]
    public sealed class EditingSketch : BaseCommand, ITool
    {
        private ICommand _command;
        private IHookHelper _hookHelper;
        private ITool _tool;
        private const string mClassName = "ShapeEdit.EditingSketch";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 编辑草图:工具类（只有在图层为编辑草图的状态下，才能“新增班块”）
        /// </summary>
        public EditingSketch()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "新增班块";
            base.m_message = "新增班块";
            base.m_toolTip = "新增班块";
            base.m_name = "ShapeEdit_EditingSketch";
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
            IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
            IEngineEditSketch sketch = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
            IGeometry pGeo = sketch.Geometry;
            if ((Editor.UniqueInstance.CheckOverlap && !Editor.UniqueInstance.CheckFeatureOverlap(pGeo, false)) && (XtraMessageBox.Show("要素与其他要素重叠！是否保留此要素？", "", MessageBoxButtons.YesNo) == DialogResult.No))
            {
                Editor.UniqueInstance.CancleSketch();
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, this._hookHelper.ActiveView.Extent);
            }
            else
            {
                IFeatureClass featureClass = Editor.UniqueInstance.TargetLayer.FeatureClass;
                IFeatureClassLoad feaLoad = featureClass as IFeatureClassLoad;
                feaLoad.LoadOnlyMode = true;
                this._tool.OnDblClick();
                feaLoad.LoadOnlyMode = false;
            }
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                Editor.UniqueInstance.CancleSketch();
                this._hookHelper.ActiveView.Refresh();
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
            Editor.UniqueInstance.AddAttribute = true;
            this._tool.OnMouseDown(button, shift, x, y);
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            this._tool.OnMouseMove(button, shift, x, y);
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

        [ComVisible(false), ComUnregisterFunction]
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

