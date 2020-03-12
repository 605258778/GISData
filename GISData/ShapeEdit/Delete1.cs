namespace ShapeEdit
{
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
    /// 删除选中的要素工具类
    /// </summary>
    [ProgId("ShapeEdit.Delete"), Guid("0717158f-0a41-4c30-a1e7-373749fb6709"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Delete1 : BaseCommand
    {
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.Delete1";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 删除选中的要素工具类:构造器
        /// </summary>
        public Delete1()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "删除";
            base.m_message = "删除选中的要素";
            base.m_toolTip = "删除选中的要素";
            base.m_name = "ShapeEdit_Delete1";
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
            try
            {
                if (MessageBox.Show("是否删除所选择的要素？", "提示", MessageBoxButtons.YesNo) != DialogResult.No)
                {
                    Editor.UniqueInstance.StartEditOperation();
                    IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                    IEnumIDs iDs = targetLayer.SelectionSet.IDs;
                    iDs.Reset();
                    for (int i = iDs.Next(); i != -1; i = iDs.Next())
                    {
                        IFeature editFeature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(i);
                        AttributeManager.AttributeDeleteHandleClass.AttributeDelete(editFeature);
                        editFeature.Delete();
                    }
                    this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, Editor.UniqueInstance.TargetLayer, this._hookHelper.ActiveView.Extent);
                    Editor.UniqueInstance.StopEditOperation("delete");
                }
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Delete1", "OnClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
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
                if (!Editor.UniqueInstance.IsBeingEdited)
                {
                    return false;
                }
                if ((Editor.UniqueInstance.TargetLayer == null) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null))
                {
                    return false;
                }
                IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                if (targetLayer.SelectionSet.Count == 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

