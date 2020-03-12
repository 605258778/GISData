namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 编辑--追踪(捕捉)类
    /// </summary>
    [Guid("da7d92ea-fd6e-4898-85cf-7cfaecbb2a2a"), ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.SnapPro")]
    public sealed class SnapPro : BaseCommand
    {
        private Form _form;
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.SnapPro";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private List<SnapAgent> _snapAgents = new List<SnapAgent>();
        private SnapProperty _sp;

        /// <summary>
        /// 追踪（捕捉）窗体:构造器
        /// </summary>
        /// <param name="pForm"></param>
        public SnapPro(Form pForm)
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "捕捉";
            base.m_message = "捕捉";
            base.m_toolTip = "捕捉";
            base.m_name = "ShapeEdit_SnapPro";
            this._form = pForm;
        }

        private void activeEvents_ItemAdded(object Item)
        {
            if ((Item is IFeatureLayer) && (this._snapAgents.Count >= 1))
            {
                IFeatureLayer pLayer = Item as IFeatureLayer;
                this._snapAgents.Add(new SnapAgent(pLayer, pLayer.Name, false, false, false));
            }
        }

        private void activeEvents_ItemDeleted(object Item)
        {
            if (Item is IFeatureLayer)
            {
                IFeatureLayer layer = Item as IFeatureLayer;
                int index = -1;
                foreach (SnapAgent agent in this._snapAgents)
                {
                    index++;
                    if (agent.FeatureLayer == layer)
                    {
                        break;
                    }
                }
                if (index != -1)
                {
                    this._snapAgents.RemoveAt(index);
                }
            }
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
            if ((this._sp == null) || this._sp.IsDisposed)
            {
                SnapProperty property = new SnapProperty();
                this._sp = property;
                property.Binding(this._hookHelper.FocusMap, ref this._snapAgents);
                property.Owner = this._form;
                property.FormClosed += new FormClosedEventHandler(this.sp_FormClosed);
                property.Show();
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
                IActiveViewEvents_Event focusMap = this._hookHelper.ActiveView.FocusMap as IActiveViewEvents_Event;
                focusMap.ItemAdded+=(new IActiveViewEvents_ItemAddedEventHandler(this.activeEvents_ItemAdded));
                focusMap.ItemDeleted+=(new IActiveViewEvents_ItemDeletedEventHandler(this.activeEvents_ItemDeleted));
            }
        }

        [ComVisible(false), ComRegisterFunction]
        private static void RegisterFunction(System.Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        private void sp_FormClosed(object sender, FormClosedEventArgs e)
        {
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
                return ((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null));
            }
        }
    }
}

