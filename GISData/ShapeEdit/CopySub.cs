namespace ShapeEdit
{
    using DevExpress.XtraEditors;
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    [ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.CopySub"), Guid("56843dc9-44d6-4dd6-9782-997da73f4625")]
    public sealed class CopySub : BaseCommand
    {
        private const string m_ClassName = "ShapeEdit.CopySub";
        private ErrorOpt m_ErrOpt = UtilFactory.GetErrorOpt();
        private IHookHelper m_hookHelper;
        private string m_SubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        public CopySub()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "复制粘贴要素";
            base.m_message = "复制粘贴要素";
            base.m_toolTip = "复制粘贴要素";
            base.m_name = "ShapeEdit_CopySub";
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private void CopyFeature(IFeature pFeature)
        {
            if (((pFeature != null) && (pFeature.Shape.GeometryType == Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType)) && ((!Editor.UniqueInstance.CheckOverlap || Editor.UniqueInstance.CheckFeatureOverlap(pFeature.ShapeCopy, false)) || (XtraMessageBox.Show("要素与其他要素重叠！是否保留此要素？", "", MessageBoxButtons.YesNo) != DialogResult.No)))
            {
                Editor.UniqueInstance.StartEditOperation();
                IFeature feature = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                (feature as IRowSubtypes).InitDefaultValues();
                feature.Shape = pFeature.ShapeCopy;
                Editor.UniqueInstance.AddAttribute = true;
                feature.Store();
                Editor.UniqueInstance.StopEditOperation("copySub");
                Editor.UniqueInstance.AddAttribute = false;
            }
        }

        public override void OnClick()
        {
            try
            {
                IFeature pFeature = (this.m_hookHelper.FocusMap.FeatureSelection as IEnumFeature).Next();
                this.CopyFeature(pFeature);
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this.m_ErrOpt.ErrorOperate(this.m_SubSysName, "ShapeEdit.CopySub", "OnClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public override void OnCreate(object hook)
        {
            if (this.m_hookHelper == null)
            {
                this.m_hookHelper = new HookHelperClass();
            }
            this.m_hookHelper.Hook = hook;
        }

        [ComVisible(false), ComRegisterFunction]
        private static void RegisterFunction(System.Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [ComVisible(false), ComUnregisterFunction]
        private static void UnregisterFunction(System.Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override bool Enabled
        {
            get
            {
                if (!Editor.UniqueInstance.IsBeingEdited)
                {
                    return false;
                }
                if ((Editor.UniqueInstance.TargetLayer == null) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null))
                {
                    return false;
                }
                return (this.m_hookHelper.FocusMap.SelectionCount == 1);
            }
        }
    }
}

