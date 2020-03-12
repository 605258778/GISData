namespace ShapeEdit
{
    using DevExpress.XtraEditors;
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 快速捕捉工具类
    /// </summary>
    [ProgId("ShapeEdit.SnapEx"), ClassInterface(ClassInterfaceType.None), Guid("3feb403a-440d-47fd-8439-c679760c94bf")]
    public sealed class SnapEx : BaseCommand, ITool
    {
        private bool _bFinished;
        private bool _bMouse;
        private bool _bStarted;
        private ICommand _command;
        private int _cursor;
        private IHookHelper _hookHelper;
        private INewLineFeedback _lineFeedback;
        private ISnappingFeedback _snapFeedback;
        private ISnappingEnvironment _snappingEnv;
        private ITool _tool;
        private const string mClassName = "ShapeEdit.SnapEx";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 快速捕捉工具类：构造器
        /// </summary>
        public SnapEx()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "快速捕捉";
            base.m_message = "快速捕捉";
            base.m_toolTip = "快速捕捉";
            base.m_name = "ShapeEdit_SnapEx";
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private bool ContainFeature(List<IFeature> pFeatures, IFeature pFeature)
        {
            for (int i = 0; i < pFeatures.Count; i++)
            {
                if (pFeatures[i].OID == pFeature.OID)
                {
                    return true;
                }
            }
            return false;
        }

        private void CreateFeed()
        {
            this._lineFeedback = new NewLineFeedbackClass();
            this._lineFeedback.Display = this._hookHelper.ActiveView.ScreenDisplay;
            IRgbColor color = new RgbColorClass {
                Blue = 0xff,
                Green = 0,
                Red = 0xc5
            };
            IColor color2 = color;
            ILineSymbol symbol = this._lineFeedback.Symbol as ILineSymbol;
            symbol.Color = color2;
            symbol.Width = 2.0;
        }

        public bool Deactivate()
        {
            return this._tool.Deactivate();
        }

        private void DrawLinefeedback(IPoint pPoint)
        {
            try
            {
                int hDC = this._hookHelper.ActiveView.ScreenDisplay.hDC;
                IEngineSnapEnvironment engineEditor = Editor.UniqueInstance.EngineEditor as IEngineSnapEnvironment;
                if (this._snappingEnv != null)
                {
                    ISnappingResult snappingResult = this._snappingEnv.PointSnapper.Snap(pPoint);
                    this._snapFeedback.Update(snappingResult, hDC);
                    if ((this._lineFeedback != null) && this._bStarted)
                    {
                        IPoint location = null;
                        if (snappingResult == null)
                        {
                            location = pPoint;
                        }
                        else
                        {
                            location = snappingResult.Location;
                        }
                        try
                        {
                            this._lineFeedback.AddPoint(location);
                        }
                        catch
                        {
                        }
                        IPolyline polyline = this._lineFeedback.Stop();
                        if (polyline == null)
                        {
                            if (snappingResult != null)
                            {
                                this._lineFeedback.Start(location);
                            }
                        }
                        else
                        {
                            IPointCollection points = (IPointCollection) polyline;
                            if (points.PointCount > 2)
                            {
                                points.RemovePoints(points.PointCount - 1, 1);
                            }
                            double snapTolerance = engineEditor.SnapTolerance;
                            double hitDistance = -1.0;
                            int hitSegmentIndex = -1;
                            int hitPartIndex = -1;
                            bool bRightSide = false;
                            IGeometry geometry = (IGeometry) points;
                            IHitTest test = geometry as IHitTest;
                            if (!test.HitTest(location, snapTolerance, esriGeometryHitPartType.esriGeometryPartVertex, null, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                            {
                                hitSegmentIndex = -1;
                            }
                            this._lineFeedback.Start(points.get_Point(0));
                            if (hitSegmentIndex != 0)
                            {
                                for (int i = 1; i < points.PointCount; i++)
                                {
                                    IPoint point = points.get_Point(i);
                                    this._lineFeedback.AddPoint(point);
                                    if (i == hitSegmentIndex)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (snappingResult != null)
                            {
                                this._lineFeedback.AddPoint(location);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.SnapEx", "DrawLinefeedback", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void editEvents_OnSketchModified()
        {
            if (!this._bMouse)
            {
                try
                {
                    IEngineEditSketch engineEditor = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
                    IPointCollection geometry = engineEditor.Geometry as IPointCollection;
                    if ((geometry == null) || (geometry.PointCount == 0))
                    {
                        if (this._lineFeedback != null)
                        {
                            this._lineFeedback.Stop();
                        }
                        this._lineFeedback = null;
                        this._bStarted = false;
                    }
                    if (this._lineFeedback != null)
                    {
                        int num = 1;
                        if (engineEditor.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            num = 2;
                        }
                        this._lineFeedback.Stop();
                        this._lineFeedback.Start(geometry.get_Point(geometry.PointCount - num));
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.SnapEx", "editEvents_OnSketchModified", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
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
            this._cursor = this._tool.Cursor;
            this._lineFeedback = null;
            this._bStarted = false;
            this._bFinished = false;
            this._bMouse = false;
            try
            {
                IEngineSnapEnvironment engineEditor = Editor.UniqueInstance.EngineEditor as IEngineSnapEnvironment;
                engineEditor.SnapTolerance = 1.0;
                (Editor.UniqueInstance.EngineEditor as IEngineEditEvents_Event).OnSketchModified+=(new IEngineEditEvents_OnSketchModifiedEventHandler(this.editEvents_OnSketchModified));
                IHookHelper2 helper = this._hookHelper as IHookHelper2;
                IExtensionManager extensionManager = helper.ExtensionManager;
                if (extensionManager != null)
                {
                    UID nameOrID = new UIDClass {
                        Value = "{E07B4C52-C894-4558-B8D4-D4050018D1DA}"
                    };
                    IExtension extension = extensionManager.FindExtension(nameOrID);
                    this._snappingEnv = extension as ISnappingEnvironment;
                    this._snapFeedback = new SnappingFeedbackClass();
                    this._snapFeedback.Initialize(this._hookHelper.Hook, this._snappingEnv, true);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.SnapEx", "OnClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
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
                this._tool = new ControlsEditingSketchToolClass();
                this._command = (ICommand) this._tool;
                this._command.OnCreate(hook);
                this._cursor = this._tool.Cursor;
            }
        }

        public void OnDblClick()
        {
            this._bStarted = false;
            this._bFinished = true;
            this._bMouse = true;
            try
            {
                this._lineFeedback.Stop();
                this._lineFeedback.Refresh(this._hookHelper.ActiveView.ScreenDisplay.hDC);
                this._lineFeedback = null;
                IEngineEditor engineEditor = Editor.UniqueInstance.EngineEditor;
                IEngineEditSketch sketch = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
                IGeometry pGeo = sketch.Geometry;
                if (pGeo == null)
                {
                    return;
                }
                if ((Editor.UniqueInstance.CheckOverlap && !Editor.UniqueInstance.CheckFeatureOverlap(pGeo, false)) && (XtraMessageBox.Show("要素与其他要素重叠！是否保留此要素？", "", MessageBoxButtons.YesNo) == DialogResult.No))
                {
                    Editor.UniqueInstance.CancleSketch();
                    this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, this._hookHelper.ActiveView.Extent);
                    return;
                }
                Editor.UniqueInstance.AddAttribute = true;
                this._tool.OnDblClick();
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.SnapEx", "OnDblClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
            this._bMouse = false;
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                this._bMouse = true;
                Editor.UniqueInstance.CancleSketch();
                this._bMouse = false;
                if (this._lineFeedback != null)
                {
                    this._lineFeedback.Stop();
                }
                this._lineFeedback = null;
                this._bStarted = false;
                this._hookHelper.ActiveView.Refresh();
            }
            else if (keyCode == 8)
            {
                Editor.UniqueInstance.OperationStack.Undo();
                Editor.UniqueInstance.LinageShape = null;
                Editor.UniqueInstance.ReservedLinkShape = null;
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
            this._bMouse = true;
            IPoint point = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            this._tool.OnMouseDown(1, shift, x, y);
            if (this._bFinished)
            {
                this._bFinished = false;
            }
            else
            {
                try
                {
                    if (this._snappingEnv == null)
                    {
                        return;
                    }
                    IPointSnapper pointSnapper = this._snappingEnv.PointSnapper;
                    ISnappingResult result = pointSnapper.Snap(point);
                    IEngineEditSketch engineEditor = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
                    IPointCollection points = engineEditor.Geometry as IPointCollection;
                    if (!this._bStarted)
                    {
                        this.CreateFeed();
                        if (result != null)
                        {
                            this._lineFeedback.Start(result.Location);
                        }
                        this._bStarted = true;
                        return;
                    }
                    IGeometry geometry = engineEditor.Geometry;
                    if (geometry == null)
                    {
                        return;
                    }
                    IPointCollection points2 = geometry as IPointCollection;
                    IPolyline polyline = this._lineFeedback.Stop();
                    int index = 0;
                    if (engineEditor.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        index = points.PointCount - 2;
                    }
                    else
                    {
                        index = points.PointCount - 1;
                    }
                    if (polyline != null)
                    {
                        IPointCollection points3 = (IPointCollection) polyline;
                        for (int i = 1; i < (points3.PointCount - 1); i++)
                        {
                            IPoint point2 = points3.get_Point(i);
                            IPoint newPoints = (point2 as IClone).Clone() as IPoint;
                            if (pointSnapper.Snap(point2).Type == esriSnappingType.esriSnappingTypeVertex)
                            {
                                points2.InsertPoints(index, 1, ref newPoints);
                                index++;
                            }
                        }
                    }
                    int hDC = this._hookHelper.ActiveView.ScreenDisplay.hDC;
                    this._lineFeedback.Stop();
                    this._lineFeedback.Refresh(hDC);
                    if (result != null)
                    {
                        this._lineFeedback.Start(result.Location);
                    }
                    if (button == 2)
                    {
                        this.OnDblClick();
                    }
                    engineEditor.RefreshSketch();
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.SnapEx", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
                this._bMouse = false;
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            this._tool.OnMouseMove(1, shift, x, y);
            IPoint pPoint = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            this.DrawLinefeedback(pPoint);
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            this._tool.OnMouseUp(1, shift, x, y);
        }

        private IGeometry ProjectGeometry(IGeometry pGeo)
        {
            IClone clone = pGeo as IClone;
            IGeometry geometry = clone.Clone() as IGeometry;
            ISpatialReference spatialReference = this._hookHelper.FocusMap.SpatialReference;
            if (geometry.SpatialReference.Name != spatialReference.Name)
            {
                geometry.Project(spatialReference);
            }
            return geometry;
        }

        public void Refresh(int hdc)
        {
            this._tool.Refresh(hdc);
            this._lineFeedback.Refresh(hdc);
        }

        [ComVisible(false), ComRegisterFunction]
        private static void RegisterFunction(System.Type registerType)
        {
            ArcGISCategoryRegistration(registerType);
        }

        private void SnapPoint()
        {
            IEngineSnapEnvironment engineEditor = Editor.UniqueInstance.EngineEditor as IEngineSnapEnvironment;
            IEngineEditSketch sketch = Editor.UniqueInstance.EngineEditor as IEngineEditSketch;
            IPointCollection points = sketch.Geometry as IPointCollection;
            bool flag = false;
            if (sketch.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                flag = true;
            }
            if ((points.PointCount != 1) && (!flag || (points.PointCount != 2)))
            {
                IEngineFeatureSnapAgent agent2 = engineEditor.get_SnapAgent(0) as IEngineFeatureSnapAgent;
                esriSpatialRelEnum esriSpatialRelIntersects = esriSpatialRelEnum.esriSpatialRelIntersects;
                if (agent2.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    esriSpatialRelIntersects = esriSpatialRelEnum.esriSpatialRelTouches;
                }
                IPoint lastPoint = sketch.LastPoint;
                List<IFeature> pFeatures = new List<IFeature>();
                ISpatialFilter filter = new SpatialFilterClass {
                    Geometry = lastPoint,
                    SpatialRel = esriSpatialRelIntersects,
                    SubFields = agent2.FeatureClass.OIDFieldName + "," + agent2.FeatureClass.ShapeFieldName,
                    GeometryField = agent2.FeatureClass.ShapeFieldName
                };
                IFeatureCursor o = agent2.FeatureClass.Search(filter, false);
                IFeature item = o.NextFeature();
                while (item != null)
                {
                    if (item.HasOID && !item.Shape.IsEmpty)
                    {
                        pFeatures.Add(item);
                        item = o.NextFeature();
                    }
                }
                Marshal.ReleaseComObject(o);
                if (pFeatures.Count >= 1)
                {
                    IPoint queryPoint = null;
                    if (flag)
                    {
                        queryPoint = points.get_Point(points.PointCount - 3);
                    }
                    else
                    {
                        queryPoint = points.get_Point(points.PointCount - 2);
                    }
                    List<IFeature> list2 = new List<IFeature>();
                    filter = new SpatialFilterClass {
                        Geometry = queryPoint,
                        SpatialRel = esriSpatialRelIntersects,
                        SubFields = agent2.FeatureClass.OIDFieldName + "," + agent2.FeatureClass.ShapeFieldName,
                        GeometryField = agent2.FeatureClass.ShapeFieldName
                    };
                    o = null;
                    o = agent2.FeatureClass.Search(filter, false);
                    item = o.NextFeature();
                    while (item != null)
                    {
                        if (item.HasOID && !item.Shape.IsEmpty)
                        {
                            list2.Add(item);
                            item = o.NextFeature();
                        }
                    }
                    Marshal.ReleaseComObject(o);
                    if (list2.Count >= 1)
                    {
                        IList<IFeature> list3 = new List<IFeature>();
                        for (int i = 0; i < list2.Count; i++)
                        {
                            if (this.ContainFeature(pFeatures, list2[i]))
                            {
                                list3.Add(list2[i]);
                            }
                        }
                        IFeature feature2 = null;
                        if (list3.Count == 1)
                        {
                            feature2 = list3[0];
                        }
                        else
                        {
                            if (list3.Count == 0)
                            {
                                return;
                            }
                            if (list3.Count == pFeatures.Count)
                            {
                                IFeatureLayer pLayer = Editor.UniqueInstance.SnapLayers[0];
                                SnapExFeatures features = new SnapExFeatures(pFeatures, this._hookHelper, pLayer);
                                features.ShowDialog();
                                if (features.Index == -1)
                                {
                                    return;
                                }
                                feature2 = pFeatures[features.Index];
                                features = null;
                            }
                        }
                        double hitDistance = -1.0;
                        int hitSegmentIndex = -1;
                        int hitPartIndex = -1;
                        bool bRightSide = false;
                        IGeometry geometry = this.ProjectGeometry(feature2.Shape);
                        IHitTest test = geometry as IHitTest;
                        if (test.HitTest(lastPoint, engineEditor.SnapTolerance, esriGeometryHitPartType.esriGeometryPartVertex, null, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                        {
                            int num5 = -1;
                            double num6 = -1.0;
                            int num7 = -1;
                            int num8 = -1;
                            bool flag4 = false;
                            int part = sketch.Part;
                            IHitTest test2 = geometry as IHitTest;
                            if (test2.HitTest(queryPoint, engineEditor.SnapTolerance, esriGeometryHitPartType.esriGeometryPartVertex, null, ref num6, ref num8, ref num7, ref flag4))
                            {
                                IPolyline polyline3;
                                if (flag)
                                {
                                    num5 = points.PointCount - 2;
                                }
                                else
                                {
                                    num5 = points.PointCount - 1;
                                }
                                IGeometryCollection geometrys = geometry as IGeometryCollection;
                                IPointCollection points2 = geometrys.get_Geometry(hitPartIndex) as IPointCollection;
                                IPolyline polyline = new PolylineClass();
                                IPolyline polyline2 = new PolylineClass();
                                IPointCollection points3 = polyline as IPointCollection;
                                IPointCollection points4 = polyline2 as IPointCollection;
                                object before = Missing.Value;
                                object after = Missing.Value;
                                IPoint inPoint = null;
                                bool flag6 = false;
                                if (num7 > hitSegmentIndex)
                                {
                                    int num9 = hitSegmentIndex;
                                    hitSegmentIndex = num7;
                                    num7 = num9;
                                    flag6 = true;
                                }
                                int num10 = 0;
                                if (flag)
                                {
                                    for (num10 = num7; num10 <= hitSegmentIndex; num10++)
                                    {
                                        inPoint = points2.get_Point(num10);
                                        points3.AddPoint(inPoint, ref before, ref after);
                                    }
                                    int num11 = points2.PointCount - 1;
                                    for (num10 = hitSegmentIndex; num10 < num11; num10++)
                                    {
                                        inPoint = points2.get_Point(num10);
                                        points4.AddPoint(inPoint, ref before, ref after);
                                    }
                                    for (num10 = 0; num10 <= num7; num10++)
                                    {
                                        inPoint = points2.get_Point(num10);
                                        points4.AddPoint(inPoint, ref before, ref after);
                                    }
                                    if (polyline.Length <= polyline2.Length)
                                    {
                                        polyline3 = polyline;
                                    }
                                    else
                                    {
                                        polyline3 = polyline2;
                                        flag6 = true;
                                    }
                                }
                                else
                                {
                                    num10 = num7;
                                    while (num10 <= hitSegmentIndex)
                                    {
                                        inPoint = points2.get_Point(num10);
                                        points3.AddPoint(inPoint, ref before, ref after);
                                        num10++;
                                    }
                                    polyline3 = polyline;
                                }
                                IPointCollection points5 = polyline3 as IPointCollection;
                                int index = num5;
                                if (flag6)
                                {
                                    for (num10 = points5.PointCount - 2; num10 > 0; num10--)
                                    {
                                        IPoint newPoints = (points5.get_Point(num10) as IClone).Clone() as IPoint;
                                        points.InsertPoints(index, 1, ref newPoints);
                                        index++;
                                    }
                                }
                                else
                                {
                                    for (num10 = 1; num10 < (points5.PointCount - 1); num10++)
                                    {
                                        IPoint point7 = (points5.get_Point(num10) as IClone).Clone() as IPoint;
                                        points.InsertPoints(index, 1, ref point7);
                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return this._command.ToString();
        }

        [ComUnregisterFunction, ComVisible(false)]
        private static void UnregisterFunction(System.Type registerType)
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
                return this._cursor;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
                {
                    return false;
                }
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
                return base.Tooltip;
            }
        }
    }
}

