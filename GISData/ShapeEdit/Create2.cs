namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Runtime.InteropServices;
    using Utilities;

    /// <summary>
    /// 创建圆形的工具类
    /// </summary>
    [ProgId("ShapeEdit.Create2"), Guid("7c79a52a-106b-49c1-80c1-143cd5df7f02"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Create2 : BaseCommand, ITool
    {
        private PointArrayClass _arrayPoints;
        private INewCircleFeedback _feedback;
        private IHookHelper _hookHelper;
        private bool _isStarted;
        private const string _mClassName = "ShapeEdit.Create2";
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private bool mInUsing;

        /// <summary>
        /// 创建圆形的工具类：构造器
        /// </summary>
        public Create2()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "创建圆形";
            base.m_message = "创建圆形";
            base.m_toolTip = "创建圆形";
            base.m_name = "ShapeEdit_Create2";
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
            this.Init();
            return true;
        }

        [DllImport("User32", CharSet=CharSet.Auto)]
        private static extern int GetCapture();
        private void Init()
        {
            this._isStarted = false;
            this._arrayPoints.RemoveAll();
            this._feedback.Stop();
            this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Editor.UniqueInstance.TargetLayer, this._hookHelper.ActiveView.Extent);
        }

        public override void OnClick()
        {
            this.Init();
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
            if (keyCode == 0x1b)
            {
                this._feedback.Stop();
                this._isStarted = false;
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                this.ResetTool();
            }
        }

        public void OnKeyUp(int keyCode, int shift)
        {
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
            try
            {
                if (button == 1)
                {
                    this.ResetTool();
                    IPoint point = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    this._feedback = new NewCircleFeedbackClass();
                    this._feedback.Display = this._hookHelper.ActiveView.ScreenDisplay;
                    this._feedback.Start(point);
                    this._isStarted = true;
                    this.mInUsing = true;
                    SetCapture(this._hookHelper.ActiveView.ScreenDisplay.hWnd);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Create2", "OnMouseDown", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            if (this._isStarted)
            {
                IPoint point = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                this._feedback.MoveTo(point);
                try
                {
                    if (this.mInUsing)
                    {
                        if (this._feedback != null)
                        {
                            this._feedback.MoveTo(this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y));
                        }
                        else
                        {
                            this.mInUsing = false;
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Create2", "OnMouseMove", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            try
            {
                IGeometry geometry = this._feedback.Stop();
                this.mInUsing = false;
                this.ResetTool();
                ISegmentCollection segments = new PolygonClass();
                object missing = Type.Missing;
                segments.AddSegment(geometry as ISegment, ref missing, ref missing);
                IPolygon polygon = segments as IPolygon;
                try
                {
                    Editor.UniqueInstance.StartEditOperation();
                    IFeature feature = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                    (feature as IRowSubtypes).InitDefaultValues();
                    feature.Shape = polygon;
                    Editor.UniqueInstance.AddAttribute = true;
                    feature.Store();
                    Editor.UniqueInstance.StopEditOperation("create");
                    Editor.UniqueInstance.AddAttribute = false;
                    IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                    targetLayer.Clear();
                    targetLayer.Add(feature);
                    this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, Editor.UniqueInstance.TargetLayer, feature.Extent);
                }
                catch
                {
                    Editor.UniqueInstance.AbortEditOperation();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Create2", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void Refresh(int hdc)
        {
            if (this._feedback != null)
            {
                this._feedback.Refresh(hdc);
            }
        }

        [ComVisible(false), ComRegisterFunction]
        private static void RegisterFunction(Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        [DllImport("User32", CharSet=CharSet.Auto)]
        private static extern int ReleaseCapture();
        private void ResetTool()
        {
            try
            {
                if (this._hookHelper.ActiveView.ScreenDisplay.hWnd == GetCapture())
                {
                    ReleaseCapture();
                }
                this.mInUsing = false;
                if (this._feedback != null)
                {
                    try
                    {
                        this._feedback.Stop();
                        this._feedback = null;
                    }
                    catch (Exception)
                    {
                    }
                }
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Create2", "ResetTool", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        [DllImport("User32", CharSet=CharSet.Auto)]
        private static extern int SetCapture(int hWnd);
        [ComVisible(false), ComUnregisterFunction]
        private static void UnregisterFunction(Type registerType)
        {
            ArcGISCategoryUnregistration(registerType);
        }

        public int Cursor
        {
            get
            {
                return ToolCursor.Add;
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
                return (((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null)) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon));
            }
        }
    }
}

