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
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Utilities;

    /// <summary>
    /// 拓扑简化工具类
    /// </summary>
    [ProgId("ShapeEdit.Simplify"), Guid("1c5793c2-64eb-4845-9234-a9e8ddf3c2dc"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Simplify : BaseCommand, ITool
    {
        private int _cursor;
        private IHookHelper m_hookHelper;
        private const string mClassName = "ShapeEdit.Simplify";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 拓扑简化工具类：构造器
        /// </summary>
        public Simplify()
        {
            base.m_category = "Topo";
            base.m_caption = "拓扑简化";
            base.m_message = "使拓扑正确";
            base.m_toolTip = "拓扑简化";
            base.m_name = "Topo_Simplify";
            try
            {
                this._cursor = ToolCursor.Cross;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message, "Invalid Bitmap");
            }
        }

        private static void ArcGISCategoryRegistration(Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        public bool Deactivate()
        {
            return true;
        }

        public override void OnClick()
        {
        }

        public bool OnContextMenu(int x, int y)
        {
            return false;
        }

        public override void OnCreate(object hook)
        {
            if (hook != null)
            {
                if (this.m_hookHelper == null)
                {
                    this.m_hookHelper = new HookHelperClass();
                }
                this.m_hookHelper.Hook = hook;
            }
        }

        public void OnDblClick()
        {
        }

        public void OnKeyDown(int keyCode, int shift)
        {
        }

        public void OnKeyUp(int keyCode, int shift)
        {
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            if (button != 2)
            {
                IPoint pPoint = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                IEnvelope searchEnvelope = FeatureFuncs.GetSearchEnvelope(this.m_hookHelper.ActiveView, pPoint);
                IFeature feature = FeatureFuncs.SearchFeatures(Editor.UniqueInstance.TargetLayer, searchEnvelope, esriSpatialRelEnum.esriSpatialRelIntersects).NextFeature();
                if (feature != null)
                {
                    ITopologicalOperator2 shape = feature.Shape as ITopologicalOperator2;
                    Editor.UniqueInstance.StartEditOperation();
                    shape.IsKnownSimple_2 = false;
                    shape.Simplify();
                    Editor.UniqueInstance.StopEditOperation("simplify");
                    IActiveView activeView = this.m_hookHelper.ActiveView;
                    IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                    targetLayer.Clear();
                    targetLayer.Add(feature);
                    activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, null);
                }
            }
        }

        public void Refresh(int hdc)
        {
        }

        [ComRegisterFunction, ComVisible(false)]
        private static void RegisterFunction(Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [ComUnregisterFunction, ComVisible(false)]
        private static void UnregisterFunction(Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
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
                if ((!Editor.UniqueInstance.IsBeingEdited || (Editor.UniqueInstance.TargetLayer == null)) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null))
                {
                    return false;
                }
                if (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

