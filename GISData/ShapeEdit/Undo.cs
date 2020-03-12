namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Controls;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 撤销工具类
    /// </summary>
    [ProgId("ShapeEdit.Undo"), Guid("f239e4ec-07db-44de-8453-0ac095eee76b"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Undo : BaseCommand
    {
        private IHookHelper _hookHelper;

        /// <summary>
        /// 撤销工具类
        /// </summary>
        public Undo()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "撤销";
            base.m_message = "撤销";
            base.m_toolTip = "撤销";
            base.m_name = "ShapeEdit_Undo";
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
            Editor.UniqueInstance.OperationStack.Undo();
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

        [ComVisible(false), ComUnregisterFunction]
        private static void UnregisterFunction(Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public override bool Enabled
        {
            get
            {
                return ((Editor.UniqueInstance.OperationStack.UndoOperation != null) && Editor.UniqueInstance.OperationStack.UndoOperation.CanUndo);
            }
        }
    }
}

