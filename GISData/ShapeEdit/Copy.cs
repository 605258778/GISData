namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 复制要素工具类
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.Copy"), Guid("277e39d2-1728-4937-9792-9f1953263992")]
    public sealed class Copy : BaseCommand
    {
        private IHookHelper _hookHelper;

        /// <summary>
        /// 复制要素工具类
        /// </summary>
        public Copy()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "复制要素";
            base.m_message = "复制要素";
            base.m_toolTip = "复制要素";
            base.m_name = "ShapeEdit_Copy";
        }

        private static void ArcGISCategoryRegistration(Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        public override void OnClick()
        {
            Editor.UniqueInstance.HasCopied = true;
            IFeature feature2 = (this._hookHelper.FocusMap.FeatureSelection as IEnumFeature).Next();
            Editor.UniqueInstance.CopiedFeature = feature2;
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

        [ComRegisterFunction, ComVisible(false)]
        private static void RegisterFunction(Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [ComVisible(false), ComUnregisterFunction]
        private static void UnregisterFunction(Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override bool Enabled
        {
            get
            {
                return (this._hookHelper.FocusMap.SelectionCount == 1);
            }
        }
    }
}

