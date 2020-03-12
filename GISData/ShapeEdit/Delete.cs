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
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 删除选中的要素的工具类
    /// </summary>
    [Guid("0717158f-0a41-4c30-a1e7-373749fb6704"), ProgId("ShapeEdit.Delete"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Delete : BaseCommand, ITool
    {
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.Delete";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private bool m_IsSelecting;
        private IList<IFeature> m_SelectedFeature;

        /// <summary>
        /// 删除选中的要素的工具类：构造器
        /// </summary>
        public Delete()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "删除";
            base.m_message = "删除选中的要素";
            base.m_toolTip = "删除选中的要素";
            base.m_name = "ShapeEdit_Delete";
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        public bool Deactivate()
        {
            return true;
        }

        private void DeleteFeatures()
        {
            if (this.m_SelectedFeature.Count >= 1)
            {
                if (MessageBox.Show("删除所选择的要素？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Editor.UniqueInstance.StartEditOperation();
                    for (int i = 0; i < this.m_SelectedFeature.Count; i++)
                    {
                        IFeature editFeature = this.m_SelectedFeature[i];
                        AttributeManager.AttributeDeleteHandleClass.AttributeDelete(editFeature);
                        editFeature.Delete();
                    }
                    Editor.UniqueInstance.StopEditOperation("delete");
                }
                else
                {
                    (Editor.UniqueInstance.TargetLayer as IFeatureSelection).Clear();
                }
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, Editor.UniqueInstance.TargetLayer, this._hookHelper.ActiveView.Extent);
                this.m_SelectedFeature.Clear();
            }
        }

        public override void OnClick()
        {
            this.m_IsSelecting = false;
            this.m_SelectedFeature = new List<IFeature>();
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
            if (keyCode == 0x10)
            {
                this.m_IsSelecting = true;
            }
        }

        public void OnKeyUp(int keyCode, int shift)
        {
            if (keyCode == 0x10)
            {
                this.m_IsSelecting = false;
                this.DeleteFeatures();
            }
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            if (button == 1)
            {
                try
                {
                    if (this.m_SelectedFeature == null)
                    {
                        this.m_SelectedFeature = new List<IFeature>();
                    }
                    IPoint pPoint = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    IFeatureLayer targetLayer = Editor.UniqueInstance.TargetLayer;
                    IFeatureSelection selection = targetLayer as IFeatureSelection;
                    if (!this.m_IsSelecting)
                    {
                        selection.Clear();
                        this.m_SelectedFeature.Clear();
                    }
                    this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, targetLayer, null);
                    IEnvelope searchEnvelope = FeatureFuncs.GetSearchEnvelope(this._hookHelper.ActiveView, pPoint);
                    if (searchEnvelope != null)
                    {
                        IFeature item = FeatureFuncs.SearchFeatures(targetLayer, searchEnvelope, esriSpatialRelEnum.esriSpatialRelIntersects).NextFeature();
                        if (item != null)
                        {
                            if (this.m_SelectedFeature.Contains(item))
                            {
                                this.m_SelectedFeature.Remove(item);
                                selection.Clear();
                                for (int i = 0; i < this.m_SelectedFeature.Count; i++)
                                {
                                    selection.Add(this.m_SelectedFeature[i]);
                                }
                            }
                            else
                            {
                                this.m_SelectedFeature.Add(item);
                                selection.Add(item);
                            }
                            this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, targetLayer, null);
                            if (!this.m_IsSelecting)
                            {
                                this.DeleteFeatures();
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Editor.UniqueInstance.AbortEditOperation();
                    this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Delete", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public void Refresh(int hdc)
        {
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

        public int Cursor
        {
            get
            {
                return ToolCursor.Delete;
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
                return ((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null));
            }
        }
    }
}

