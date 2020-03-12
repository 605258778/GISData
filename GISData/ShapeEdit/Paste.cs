namespace ShapeEdit
{
    using DevExpress.XtraEditors;
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 粘帖要素工具类
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("74018633-b960-44c6-8900-eeb67db7801e"), ProgId("ShapeEdit.Paste")]
    public sealed class Paste : BaseCommand
    {
        private IHookHelper m_hookHelper;
        private const string mClassName = "ShapeEdit.Paste";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 粘帖要素工具类
        /// </summary>
        public Paste()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "粘帖要素";
            base.m_message = "粘帖要素";
            base.m_toolTip = "粘帖要素";
            base.m_name = "ShapeEdit_Paste";
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        public override void OnClick()
        {
            IFeature copiedFeature = Editor.UniqueInstance.CopiedFeature;
            if (((copiedFeature != null) && (copiedFeature.Shape.GeometryType == Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType)) && ((!Editor.UniqueInstance.CheckOverlap || Editor.UniqueInstance.CheckFeatureOverlap(copiedFeature.ShapeCopy, false)) || (XtraMessageBox.Show("要素与其他要素重叠！是否保留此要素？", "", MessageBoxButtons.YesNo) != DialogResult.No)))
            {
                Editor.UniqueInstance.StartEditOperation();
                IFeature feature = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                (feature as IRowSubtypes).InitDefaultValues();
                feature.Shape = copiedFeature.ShapeCopy;
                Editor.UniqueInstance.AddAttribute = true;
                feature.Store();
                Editor.UniqueInstance.StopEditOperation("create");
                Editor.UniqueInstance.AddAttribute = false;
                IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                targetLayer.Clear();
                targetLayer.Add(feature);
                this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, null);
            }
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

        [ComVisible(false), ComRegisterFunction]
        private static void RegisterFunction(System.Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [ComUnregisterFunction, ComVisible(false)]
        private static void UnregisterFunction(System.Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override bool Enabled
        {
            get
            {
                return Editor.UniqueInstance.HasCopied;
            }
        }
    }
}

