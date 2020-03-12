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
    /// 自动完成多边形工具类
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.AutoComplete"), Guid("5467092c-4793-4a24-98f8-1bc56b61aeb5")]
    public sealed class AutoComplete : BaseCommand, ITool
    {
        private int m_Cursor;
        private IHookHelper m_hookHelper;
        private bool m_IsUsed;
        private INewLineFeedback m_LineFeedback;
        private PointArrayClass m_LinePoints = new PointArrayClass();
        private int m_Step;
        private const string mClassName = "ShapeEdit.AutoComplete";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 自动完成多边形工具类:构造器
        /// </summary>
        public AutoComplete()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "自动完成多边形";
            base.m_message = "自动完成多边形";
            base.m_toolTip = "自动完成多边形";
            base.m_name = "ShapeEdit_AutoComplete";
        }

        private static void ArcGISCategoryRegistration(Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private void CreatePolygonsFromFeatures(IFeatureClass getpolygon_fc)
        {
            IDataset dataset = getpolygon_fc as IDataset;
            IWorkspaceEdit workspace = dataset.Workspace as IWorkspaceEdit;
            if (!workspace.IsBeingEdited())
            {
                workspace.StartEditing(true);
                if (!workspace.IsBeingEdited())
                {
                    return;
                }
                workspace.StartEditOperation();
            }
            IFeatureConstruction construction = new FeatureConstructionClass();
            IEnumFeature featureSelection = this.m_hookHelper.FocusMap.FeatureSelection as IEnumFeature;
            if (featureSelection.Next() != null)
            {
                construction.ConstructPolygonsFromFeatures(null, getpolygon_fc, null, false, false, featureSelection, null, 0.01, null);
            }
        }

        public bool Deactivate()
        {
            return true;
        }

        public IGeometry Erase(IGeometry source, IGeometry other)
        {
            ITopologicalOperator @operator = source as ITopologicalOperator;
            if (!@operator.IsSimple)
            {
                @operator.Simplify();
            }
            IGeometry geometry = @operator.Difference(other);
            @operator = geometry as ITopologicalOperator;
            if (!@operator.IsSimple)
            {
                @operator.Simplify();
            }
            return geometry;
        }

        private void Init()
        {
            this.m_Step = 0;
            this.m_IsUsed = false;
            this.m_LineFeedback.Stop();
            this.m_LinePoints.RemoveAll();
        }

        public override void OnClick()
        {
            this.Init();
            this.m_Cursor = ToolCursor.Add;
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
                this.m_LineFeedback = new NewLineFeedbackClass();
                this.m_LineFeedback.Display = this.m_hookHelper.ActiveView.ScreenDisplay;
            }
        }

        public void OnDblClick()
        {
            Editor.UniqueInstance.CheckOverlap = false;
            try
            {
                IFeatureLayer targetLayer = Editor.UniqueInstance.TargetLayer;
                object missing = Type.Missing;
                if (!this.m_IsUsed || (this.m_LineFeedback == null))
                {
                    return;
                }
                if (this.m_Step > 1)
                {
                    IPolyline polyline = this.m_LineFeedback.Stop();
                    if (polyline == null)
                    {
                        this.Init();
                        return;
                    }
                    IFeatureClass featureClass = targetLayer.FeatureClass;
                    if (featureClass == null)
                    {
                        this.Init();
                        return;
                    }
                    IGeoDataset dataset = featureClass as IGeoDataset;
                    if (polyline.SpatialReference != dataset.SpatialReference)
                    {
                        polyline.Project(dataset.SpatialReference);
                        polyline.SpatialReference = dataset.SpatialReference;
                    }
                    IGeometry inGeometry = polyline;
                    GeometryBagClass lineSrc = new GeometryBagClass();
                    lineSrc.AddGeometry(inGeometry, ref missing, ref missing);
                    targetLayer.Selectable = true;
                    IDataset dataset2 = featureClass as IDataset;
                    IWorkspace selectionWorkspace = dataset2.Workspace;
                    IEnvelope extent = dataset.Extent;
                    IInvalidArea invalidArea = new InvalidAreaClass();
                    IFeatureConstruction construction = new FeatureConstructionClass();
                    IFeatureSelection selection = targetLayer as IFeatureSelection;
                    ISelectionSet selectionSet = selection.SelectionSet;
                    Editor.UniqueInstance.SetArea = false;
                    Editor.UniqueInstance.StartEditOperation();
                    ISpatialReferenceTolerance spatialReference = (ISpatialReferenceTolerance) dataset.SpatialReference;
                    construction.AutoCompleteFromGeometries(featureClass, extent, lineSrc, invalidArea, spatialReference.XYTolerance, selectionWorkspace, out selectionSet);
                    if ((selectionSet == null) || (selectionSet.Count < 1))
                    {
                        Editor.UniqueInstance.AbortEditOperation();
                    }
                    else
                    {
                        List<IFeature> pFeatureList = new List<IFeature>();
                        IEnumIDs iDs = selectionSet.IDs;
                        iDs.Reset();
                        for (int i = iDs.Next(); i >= 0; i = iDs.Next())
                        {
                            IFeature pFeature = featureClass.GetFeature(i);
                            FeatureFuncs.SetFeatureArea(pFeature);
                            pFeatureList.Add(pFeature);
                        }
                        if (selectionSet.Count == 1)
                        {
                            IFeature feature = pFeatureList[0];
                            selection.Clear();
                            selection.Add(feature);
                            AttributeManager.AttributeAddHandleClass.AttributeAdd(ref feature);
                        }
                        else
                        {
                            AttributeManager.AttributeSplitHandleClass.AttributeSplit(null, ref pFeatureList);
                        }
                        Editor.UniqueInstance.StopEditOperation("AutoComplete");
                    }
                    Editor.UniqueInstance.SetArea = true;
                    this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, this.m_hookHelper.ActiveView.Extent);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.AutoComplete", "OnDblClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
            this.Init();
            Editor.UniqueInstance.CheckOverlap = true;
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                this.Init();
            }
            if (keyCode == 8)
            {
                if (!this.m_IsUsed)
                {
                    this.Init();
                }
                else
                {
                    int index = this.m_LinePoints.Count - 1;
                    if (index >= 0)
                    {
                        int hDC = this.m_hookHelper.ActiveView.ScreenDisplay.hDC;
                        this.m_LineFeedback.Stop();
                        this.m_LineFeedback.Refresh(hDC);
                        this.m_LinePoints.Remove(index);
                        if (index == 0)
                        {
                            this.m_IsUsed = false;
                        }
                        else
                        {
                            this.m_LineFeedback.Start(this.m_LinePoints.get_Element(0));
                            for (int i = 1; i < index; i++)
                            {
                                this.m_LineFeedback.AddPoint(this.m_LinePoints.get_Element(i));
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
            if ((this.m_hookHelper.ActiveView != null) && (button == 1))
            {
                IPoint point = null;
                if (this.m_Step <= 0)
                {
                    point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    this.m_LineFeedback.Start(point);
                    this.m_Step++;
                    this.m_IsUsed = true;
                    this.m_LinePoints.Add(point);
                }
                else
                {
                    point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    this.m_Step++;
                    this.m_LineFeedback.AddPoint(point);
                    this.m_LinePoints.Add(point);
                }
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            IPoint point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            IEngineSnapEnvironment engineEditor = Editor.UniqueInstance.EngineEditor as IEngineSnapEnvironment;
            try
            {
                if (engineEditor.SnapPoint(point))
                {
                    this.m_Cursor = ToolCursor.VertexSelected;
                }
                else
                {
                    this.m_Cursor = ToolCursor.Add;
                }
            }
            catch
            {
            }
            if (this.m_IsUsed)
            {
                IPoint point2 = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                this.m_LineFeedback.MoveTo(point2);
            }
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
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

        public override int Bitmap
        {
            get
            {
                return 0;
            }
        }

        public override string Caption
        {
            get
            {
                return this.Description;
            }
        }

        public override string Category
        {
            get
            {
                return "AutoCompletePolygon";
            }
        }

        public override bool Checked
        {
            get
            {
                return false;
            }
        }

        public int Cursor
        {
            get
            {
                return this.m_Cursor;
            }
        }

        public string Description
        {
            get
            {
                return "自动完成多边形";
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
                if (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
                {
                    return false;
                }
                return true;
            }
        }

        public override int HelpContextID
        {
            get
            {
                return 0;
            }
        }

        public override string HelpFile
        {
            get
            {
                return "";
            }
        }

        public override string Message
        {
            get
            {
                return this.Description;
            }
        }

        public override string Name
        {
            get
            {
                return this.Description;
            }
        }

        public override string Tooltip
        {
            get
            {
                return this.Description;
            }
        }
    }
}

