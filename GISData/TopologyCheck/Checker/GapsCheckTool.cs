namespace TopologyCheck.Checker
{
    using DevExpress.XtraEditors;
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using ShapeEdit;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    //using TaskManage;
    using TopologyCheck.Error;
    using Utilities;

    [ProgId("TopologyCheck.Checker.GapsCheckTool"), ClassInterface(ClassInterfaceType.None), Guid("ebba07e0-8dcc-427e-9758-7485bf1c8eef")]
    public sealed class GapsCheckTool : BaseCommand, ITool
    {
        private GapsChecker _gcChecker;
        private IHookHelper m_hookHelper;
        private const string mClassName = "TopologyCheck.Checker.GapsCheckTool";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        public GapsCheckTool()
        {
            base.m_category = "Topo";
            base.m_caption = "空隙检查";
            base.m_message = "";
            base.m_toolTip = "空隙检查";
            base.m_name = "Topo_GapsCheckTool";
            try
            {
                string resource = base.GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(base.GetType(), resource);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message, "Invalid Bitmap");
            }
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
        }

        public bool OnContextMenu(int x, int y)
        {
            return false;
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
            if (button != 2)
            {
                try
                {
                    IFeatureLayer targetLayer = Editor.UniqueInstance.TargetLayer;
                    if (targetLayer != null)
                    {
                        IPoint point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                        ISpatialFilter queryFilter = new SpatialFilterClass();
                        queryFilter.Geometry = point;
                        queryFilter.GeometryField = targetLayer.FeatureClass.ShapeFieldName;
                        queryFilter.SubFields = targetLayer.FeatureClass.ShapeFieldName;
                        queryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                        IFeature pFeature = targetLayer.Search(queryFilter, false).NextFeature();
                        if (pFeature != null)
                        {
                            object missing = Type.Missing;
                            if (Editor.UniqueInstance.IsBeingEdited)
                            {
                                Editor.UniqueInstance.StopEdit();
                            }
                            this._gcChecker = new GapsChecker(targetLayer, 0);
                            if (this._gcChecker.CheckFeature(pFeature, ref missing))
                            {
                                XtraMessageBox.Show("拓扑正确！");
                            }
                            else
                            {
                                XtraMessageBox.Show("拓扑错误！");
                                ErrManager.AddErrTopoElement(this.m_hookHelper.ActiveView, missing, pFeature);
                            }
                            if (!Editor.UniqueInstance.IsBeingEdited)
                            {
                                Editor.UniqueInstance.StartEdit(Editor.UniqueInstance.Workspace, Editor.UniqueInstance.Map);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!Editor.UniqueInstance.IsBeingEdited)
                    {
                        Editor.UniqueInstance.StartEdit(Editor.UniqueInstance.Workspace, Editor.UniqueInstance.Map);
                    }
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "TopologyCheck.Checker.GapsCheckTool", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    XtraMessageBox.Show(exception.Message);
                }
            }
        }

        public void Refresh(int hdc)
        {
        }

        [ComVisible(false), ComRegisterFunction]
        private static void RegisterFunction(Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [ComVisible(false), ComUnregisterFunction]
        private static void UnregisterFunction(Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public int Cursor
        {
            get
            {
                //return TopologyCheck.Checker.ToolCursor.Validate;
                return 11;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (!(this.m_hookHelper.Hook is IMapControl4))
                {
                    return false;
                }
                return false;
                //if (EditTask.EditLayer == null)
                //{
                //    return false;
                //}
                //return (EditTask.EditLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon);
            }
        }
    }
}

