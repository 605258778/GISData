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
    using Utilities;

    /// <summary>
    /// 删除面积错误的要素的工具类
    /// </summary>
    [ProgId("ShapeEdit.DeleteEx"), Guid("0d3a519b-0b04-4fca-8ac0-d5da6c59958a"), ClassInterface(ClassInterfaceType.None)]
    public sealed class DeleteEx : BaseCommand, ITool, IFeatureTool
    {
        private IFeature _feature;
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.DeleteEx";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 删除面积错误的要素的工具类：构造器
        /// </summary>
        public DeleteEx()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "删除";
            base.m_message = "删除面积错误的要素";
            base.m_toolTip = "删除面积错误的要素";
            base.m_name = "ShapeEdit_DeleteEx";
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
            try
            {
                Editor.UniqueInstance.StartEditOperation();
                AttributeManager.AttributeDeleteHandleClass.AttributeDelete(this._feature);
                this._feature.Delete();
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, Editor.UniqueInstance.TargetLayer, this._hookHelper.ActiveView.Extent);
                Editor.UniqueInstance.StopEditOperation("deleteex");
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.DeleteEx", "OnClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public bool OnContextMenu(int x, int y)
        {
            return false;
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
        }

        public void Refresh(int hdc)
        {
        }

        [ComVisible(false), ComRegisterFunction]
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
                return 0;
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
                return (((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null)) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon));
            }
        }

        public IFeature Feature
        {
            set
            {
                this._feature = value;
            }
        }
    }
}

