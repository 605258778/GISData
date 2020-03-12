namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Controls;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 重做工具类
    /// </summary>
    [ProgId("ShapeEdit.Redo"), ClassInterface(ClassInterfaceType.None), Guid("1e869c2e-e959-4785-9b56-dd4795f5bd71")]
    public sealed class Redo : BaseCommand
    {
        private IHookHelper _hookHelper;

        /// <summary>
        /// 重做工具类
        /// </summary>
        public Redo()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "重做";
            base.m_message = "重做";
            base.m_toolTip = "重做";
            base.m_name = "ShapeEdit_Redo";
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
            Editor.UniqueInstance.OperationStack.Redo();
            Editor.UniqueInstance.LinageShape = null;
            Editor.UniqueInstance.ReservedLinkShape = null;
            IAttributeUndo attributeUndoHandleClass = AttributeManager.AttributeUndoHandleClass;
            if (attributeUndoHandleClass != null)
            {
                attributeUndoHandleClass.AttributeUndo();
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
        private static void RegisterFunction(Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [ComUnregisterFunction, ComVisible(false)]
        private static void UnregisterFunction(Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override bool Enabled
        {
            get
            {
                return ((Editor.UniqueInstance.OperationStack.RedoOperation != null) && Editor.UniqueInstance.OperationStack.RedoOperation.CanRedo);
            }
        }
    }
}

