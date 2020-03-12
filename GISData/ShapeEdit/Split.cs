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
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Utilities;

    /// <summary>
    /// 分割要素:工具类
    /// </summary>
    [Guid("3afb70a3-74ae-490e-a000-a2eb06ef81e5"), ProgId("ShapeEdit.Split"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Split : BaseCommand, ITool
    {
        private int _cursor;
        private INewLineFeedback _feedback;
        private IHookHelper _hookHelper;
        private bool _isLineStarted;
        private bool _isSnapEx;
        private PointArrayClass _linePoints = new PointArrayClass();
        private const string _mClassName = "ShapeEdit.Split";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private IPoint _snapPoint;

        /// <summary>
        /// 分割要素:工具类
        /// </summary>
        public Split()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "分割";
            base.m_message = "分割要素";
            base.m_toolTip = "分割要素";
            base.m_name = "ShapeEdit_Split";
        }

        private void AddPoint(IPoint mousePoint)
        {
            if (this._isLineStarted)
            {
                this._feedback.AddPoint(mousePoint);
            }
            else
            {
                this._feedback.Start(mousePoint);
                this._isLineStarted = true;
                this._linePoints.RemoveAll();
            }
            this._linePoints.Add(mousePoint);
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
            this._feedback.Stop();
            this._isLineStarted = false;
            this._linePoints.RemoveAll();
            this._snapPoint = null;
            this._isSnapEx = true;
        }

        private bool IsSnapPoint(IPoint pPoint)
        {
            IEngineSnapEnvironment engineEditor = Editor.UniqueInstance.EngineEditor as IEngineSnapEnvironment;
            if (engineEditor.SnapAgentCount != 1)
            {
                return false;
            }
            try
            {
                if (engineEditor.SnapPoint(pPoint))
                {
                    this._snapPoint = pPoint;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
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
                this._feedback = new NewLineFeedbackClass();
                this._feedback.Display = this._hookHelper.ActiveView.ScreenDisplay;
                this._cursor = ToolCursor.Cross;
            }
        }

        public void OnDblClick()
        {
            if (this._isLineStarted)
            {
                this._snapPoint = null;
                try
                {
                    this._isLineStarted = false;
                    this._linePoints.RemoveAll();
                    IPolyline other = this._feedback.Stop();
                    IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                    IEnumIDs iDs = targetLayer.SelectionSet.IDs;
                    iDs.Reset();
                    IFeature srcFeature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(iDs.Next());
                    if (other.SpatialReference != srcFeature.Shape.SpatialReference)
                    {
                        other.Project(srcFeature.Shape.SpatialReference);
                        other.SpatialReference = srcFeature.Shape.SpatialReference;
                    }
                    ITopologicalOperator2 shape = srcFeature.Shape as ITopologicalOperator2;
                    if (shape != null)
                    {
                         shape.IsKnownSimple_2 = false;
                        shape.Simplify();
                        ITopologicalOperator2 operator2 = other as ITopologicalOperator2;
                        operator2.IsKnownSimple_2 = false;
                        operator2.Simplify();
                        IGeometry geometry = null;
                        if (srcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            geometry = shape.Intersect(other, esriGeometryDimension.esriGeometry0Dimension);
                        }
                        else
                        {
                            geometry = shape.Intersect(other, esriGeometryDimension.esriGeometry1Dimension);
                        }
                        if (!geometry.IsEmpty)
                        {
                            IGeometry geometry2;
                            IGeometry geometry3;
                            shape.Cut(other, out geometry3, out geometry2);
                            if ((geometry3 != null) && (geometry2 != null))
                            {
                                (geometry3 as ITopologicalOperator).Simplify();
                                (geometry2 as ITopologicalOperator).Simplify();
                                IFeatureClass featureClass = Editor.UniqueInstance.TargetLayer.FeatureClass;
                                IFeature feature = featureClass.CreateFeature();
                                IFeature feature3 = featureClass.CreateFeature();
                                feature.Shape = geometry3;
                                feature3.Shape = geometry2;
                                IAttributeSplit attributeSplitHandleClass = AttributeManager.AttributeSplitHandleClass;
                                if (attributeSplitHandleClass != null)
                                {
                                    try
                                    {
                                        Editor.UniqueInstance.StartEditOperation();
                                        Editor.UniqueInstance.AddAttribute = false;
                                        List<IFeature> pFeatureList = null;
                                        pFeatureList = new List<IFeature> {
                                            feature,
                                            feature3
                                        };
                                        attributeSplitHandleClass.AttributeSplit(srcFeature, ref pFeatureList);
                                        feature = pFeatureList[0];
                                        feature3 = pFeatureList[1];
                                        feature.Store();
                                        feature3.Store();
                                        srcFeature.Delete();
                                        Editor.UniqueInstance.StopEditOperation("split");
                                        targetLayer.Clear();
                                        targetLayer.Add(feature);
                                        this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, null);
                                    }
                                    catch (Exception exception)
                                    {
                                        Editor.UniqueInstance.AbortEditOperation();
                                        this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Split", "OnDblClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception2)
                {
                    this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Split", "OnDblClick", exception2.GetHashCode().ToString(), exception2.Source, exception2.Message, "", "", "");
                }
            }
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                this.Init();
            }
            if (keyCode == 8)
            {
                if (!this._isLineStarted)
                {
                    this.Init();
                }
                else
                {
                    int index = this._linePoints.Count - 1;
                    if (index >= 0)
                    {
                        int hDC = this._hookHelper.ActiveView.ScreenDisplay.hDC;
                        this._feedback.Stop();
                        this._feedback.Refresh(hDC);
                        this._linePoints.Remove(index);
                        if (index == 0)
                        {
                            this._isLineStarted = false;
                        }
                        else
                        {
                            this._feedback.Start(this._linePoints.get_Element(0));
                            for (int i = 1; i < index; i++)
                            {
                                this._feedback.AddPoint(this._linePoints.get_Element(i));
                            }
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
                IPoint mousePoint = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                if (this._cursor == ToolCursor.VertexSelected)
                {
                    mousePoint = this._snapPoint;
                }
                if (this._isSnapEx)
                {
                    this.AddPoint(mousePoint);
                }
                else
                {
                    this.AddPoint(mousePoint);
                }
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            IPoint pPoint = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            if (this.IsSnapPoint(pPoint))
            {
                this._cursor = ToolCursor.VertexSelected;
            }
            else if (this._isSnapEx)
            {
                this._cursor = ToolCursor.SnapEx;
            }
            else
            {
                this._cursor = ToolCursor.Cross;
            }
            if (this._isLineStarted)
            {
                this._feedback.MoveTo(pPoint);
            }
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
        }

        private IGeometry ProjectGeometry(IGeometry pGeo)
        {
            ISpatialReference spatialReference = this._hookHelper.FocusMap.SpatialReference;
            if (pGeo.SpatialReference.Name != spatialReference.Name)
            {
                pGeo.Project(spatialReference);
            }
            return pGeo;
        }

        public void Refresh(int hdc)
        {
            if (this._feedback != null)
            {
                this._feedback.Refresh(hdc);
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

        public int Cursor
        {
            get
            {
                return this._cursor;
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
                if ((Editor.UniqueInstance.TargetLayer == null) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null))
                {
                    return false;
                }
                if ((Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolyline))
                {
                    return false;
                }
                IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                if (targetLayer.SelectionSet.Count != 1)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

