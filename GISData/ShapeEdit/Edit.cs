namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Runtime.InteropServices;
    using Utilities;

    /// <summary>
    /// 编辑班块类
    /// </summary>
    [ProgId("ShapeEdit.Edit"), Guid("d7e0b552-94f0-4dc1-a120-75da1ed50022"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Edit : BaseCommand, ITool
    {
        private ICommand _command;
        private IHookHelper _hookHelper;
        private ITool _tool;
        private const string mClassName = "ShapeEdit.Edit";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 编辑班块类:构造器
        /// </summary>
        public Edit()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "编辑班块";
            base.m_message = "编辑班块";
            base.m_toolTip = "编辑班块";
            base.m_name = "ShapeEdit_Edit";
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
            this._command.OnClick();
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
                this._tool = new ControlsEditingEditToolClass();
                this._command = (ICommand) this._tool;
                this._command.OnCreate(hook);
            }
        }

        public void OnDblClick()
        {
            if (Editor.UniqueInstance.EngineEditor.SelectionCount == 1)
            {
                IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                if (targetLayer.SelectionSet.Count != 0)
                {
                    this._tool.OnDblClick();
                }
            }
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x2e)
            {
                try
                {
                    IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                    if (targetLayer.SelectionSet.Count == Editor.UniqueInstance.EngineEditor.SelectionCount)
                    {
                        this._tool.OnKeyDown(keyCode, shift);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.Edit", "OnKeyDown", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
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
            this._tool.OnMouseDown(button, shift, x, y);
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            if (button == 1)
            {
                IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                if (targetLayer.SelectionSet.Count != Editor.UniqueInstance.EngineEditor.SelectionCount)
                {
                    return;
                }
            }
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
        private static void RegisterFunction(Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        public override string ToString()
        {
            return this._command.ToString();
        }

        [ComUnregisterFunction, ComVisible(false)]
        private static void UnregisterFunction(Type registerType)
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

