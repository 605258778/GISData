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
    /// 创建多边形工具类
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.Create"), Guid("7c79a52a-106b-49c1-80c1-143cd5df7f01")]
    public sealed class Create : BaseCommand, ITool
    {
        private PointArrayClass _arrayPoints;
        private INewPolygonFeedback _feedback;
        private IHookHelper _hookHelper;
        private bool _isStarted;
        private const string _mClassName = "ShapeEdit.Create";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 创建多边形工具类：构造器
        /// </summary>
        public Create()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "创建多边形";
            base.m_message = "创建多边形";
            base.m_toolTip = "创建多边形";
            base.m_name = "ShapeEdit_Create";
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
                this._feedback = new NewPolygonFeedbackClass();
                this._feedback.Display = this._hookHelper.ActiveView.ScreenDisplay;
                this._arrayPoints = new PointArrayClass();
            }
        }

        public void OnDblClick()
        {
            if (this._isStarted)
            {
                this._isStarted = false;
                this._arrayPoints.RemoveAll();
                IPolygon polygon = this._feedback.Stop();
                if ((polygon != null) && !polygon.IsEmpty)
                {
                    (polygon as ITopologicalOperator).Simplify();
                    try
                    {
                        Editor.UniqueInstance.StartEditOperation();
                        IFeature feature = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                        (feature as IRowSubtypes).InitDefaultValues();
                        feature.Shape = polygon;
                        Editor.UniqueInstance.AddAttribute = true;
                        feature.Store();
                        Editor.UniqueInstance.AddAttribute = false;
                        Editor.UniqueInstance.StopEditOperation("create");
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
            }
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                this._feedback.Stop();
                this._isStarted = false;
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            }
            if ((keyCode == 8) && this._isStarted)
            {
                int index = this._arrayPoints.Count - 1;
                if (index >= 0)
                {
                    int hDC = this._hookHelper.ActiveView.ScreenDisplay.hDC;
                    this._feedback.Stop();
                    this._feedback.Refresh(hDC);
                    this._arrayPoints.Remove(index);
                    if (index == 0)
                    {
                        this._isStarted = false;
                        this._arrayPoints.RemoveAll();
                    }
                    else
                    {
                        this._feedback.Start(this._arrayPoints.get_Element(0));
                        for (int i = 1; i < index; i++)
                        {
                            this._feedback.AddPoint(this._arrayPoints.get_Element(i));
                        }
                    }
                }
            }
        }

        public void OnKeyUp(int keyCode, int shift)
        {
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
            if (button == 1)
            {
                IPoint point = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                if (this._isStarted)
                {
                    this._feedback.AddPoint(point);
                }
                else
                {
                    this._feedback.Start(point);
                    this._isStarted = true;
                }
                this._arrayPoints.Add(point);
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            if (this._isStarted)
            {
                IPoint point = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                this._feedback.MoveTo(point);
            }
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
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

        [ComUnregisterFunction, ComVisible(false)]
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

