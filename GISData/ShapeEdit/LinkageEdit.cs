namespace ShapeEdit
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using FunFactory;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TaskManage;
    using Utilities;

    /// <summary>
    /// 联动编辑工具类
    /// </summary>
    public class LinkageEdit : ITool, ICommand
    {
        private int _bitmap;
        private int _cursor;
        private int _cursorNormal;
        private int _cursorSnap;
        private IGeometry _editBound;
        private ArrayList _editInfoList = new ArrayList();
        private IGeometry _editOrigShape;
        private IPoint _endPoint;
        private IReshapeFeedback _feedBack = new ReshapeFeedbackClass();
        private bool _isSnaped;
        private bool _isVertexEditing;
        private bool _isVertexSelected;
        private List<LinkArgs> _las = Editor.UniqueInstance.LinkArgs;
        private IFeatureLayer[] _linkLayers;
        private IMapControl4 _mapControl;
        private const string _mClassName = "ShapeEdit.LinkageEdit";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private int _vertexIndex = -1;
        private IActiveView _viewMap;

        private void ave_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if ((phase == esriViewDrawPhase.esriViewForeground) && (Editor.UniqueInstance.LinageShape != null))
            {
                Editor.UniqueInstance.DrawShape(Display, Editor.UniqueInstance.LinageShape);
            }
        }

        public void ClearUp()
        {
            this._isVertexEditing = false;
            this._isVertexSelected = false;
            this._isSnaped = false;
            this._editOrigShape = null;
            this._editBound = null;
            Editor.UniqueInstance.ReservedLinkShape = Editor.UniqueInstance.LinageShape;
            Editor.UniqueInstance.LinageShape = null;
        }

        public bool Deactivate()
        {
            this.ClearUp();
            return true;
        }

        public void InitLinkage(IPoint mousePoint, double buffer)
        {
            this._isVertexEditing = false;
            this._editOrigShape = null;
            this._editBound = null;
            this._las.Clear();
            IFeatureClass featureClass = Editor.UniqueInstance.TargetLayer.FeatureClass;
            ISpatialFilter queryFilter = new SpatialFilterClass {
                Geometry = (mousePoint as ITopologicalOperator).Buffer(buffer),
                GeometryField = featureClass.ShapeFieldName,
                SubFields = featureClass.ShapeFieldName,
                SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects
            };
            IFeatureCursor o = Editor.UniqueInstance.TargetLayer.Search(queryFilter, false);
            IFeature pFeature = o.NextFeature();
            if (pFeature == null)
            {
                Marshal.ReleaseComObject(o);
                o = null;
                this.ClearUp();
            }
            else
            {
                IFeature feature = o.NextFeature();
                if (feature == null)
                {
                    Marshal.ReleaseComObject(o);
                    o = null;
                    this.ClearUp();
                }
                else if (o.NextFeature() != null)
                {
                    Marshal.ReleaseComObject(o);
                    o = null;
                    this.ClearUp();
                }
                else
                {
                    pFeature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(pFeature.OID);
                    feature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(feature.OID);
                    IGeometry pGeometry = null;
                    IGeometry shapeCopy = pFeature.ShapeCopy;
                    IGeometry other = feature.ShapeCopy;
                    ITopologicalOperator2 @operator = shapeCopy as ITopologicalOperator2;
                    @operator.IsKnownSimple_2 = false;
                    @operator.Simplify();
                    ITopologicalOperator2 operator2 = other as ITopologicalOperator2;
                    operator2.IsKnownSimple_2 = false;
                    operator2.Simplify();
                    pGeometry = @operator.Intersect(other, esriGeometryDimension.esriGeometry1Dimension);
                    if (pGeometry.IsEmpty)
                    {
                        this.ClearUp();
                    }
                    else
                    {
                        if ((pGeometry as IGeometryCollection).GeometryCount > 1)
                        {
                            IHitTest test = pGeometry as IHitTest;
                            IPoint hitPoint = null;
                            double hitDistance = 0.0;
                            int hitPartIndex = -1;
                            int hitSegmentIndex = -1;
                            bool bRightSide = false;
                            if (!test.HitTest(mousePoint, buffer, esriGeometryHitPartType.esriGeometryPartBoundary, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                            {
                                return;
                            }
                            pGeometry = (pGeometry as IGeometryCollection).get_Geometry(hitPartIndex);
                        }
                        pGeometry = GISFunFactory.UnitFun.ConvertPoject(pGeometry, this._viewMap.FocusMap.SpatialReference);
                        IPolyline polyline = null;
                        if (pGeometry.GeometryType != esriGeometryType.esriGeometryPolyline)
                        {
                            IPath path = pGeometry as IPath;
                            if ((path != null) && path.IsClosed)
                            {
                                this.ClearUp();
                                return;
                            }
                            IPointCollection newPoints = pGeometry as IPointCollection;
                            polyline = new PolylineClass {
                                SpatialReference = pGeometry.SpatialReference
                            };
                            (polyline as IPointCollection).AddPointCollection(newPoints);
                        }
                        IGeometry geometry4 = @operator.Union(other);
                        this._editBound = GISFunFactory.UnitFun.ConvertPoject(geometry4, this._viewMap.FocusMap.SpatialReference);
                        this._las.Add(new LinkArgs(pFeature));
                        this._las.Add(new LinkArgs(feature));
                        this.InitOtherLayer(pGeometry);
                        if (polyline != null)
                        {
                            this._editOrigShape = (polyline as IClone).Clone() as IGeometry;
                            Editor.UniqueInstance.LinageShape = polyline;
                            Editor.UniqueInstance.ReservedLinkShape = polyline;
                        }
                        else
                        {
                            this._editOrigShape = (pGeometry as IClone).Clone() as IGeometry;
                            Editor.UniqueInstance.LinageShape = pGeometry;
                            Editor.UniqueInstance.ReservedLinkShape = pGeometry;
                        }
                        this._isVertexEditing = true;
                    }
                }
            }
        }

        private void InitOtherLayer(IGeometry pGeo)
        {
            if (this._linkLayers != null)
            {
                ISpatialFilter filter = new SpatialFilterClass {
                    Geometry = pGeo
                };
                foreach (IFeatureLayer layer in this._linkLayers)
                {
                    if (layer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        filter.SubFields = layer.FeatureClass.ShapeFieldName;
                        filter.GeometryField = layer.FeatureClass.ShapeFieldName;
                        filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
                        IFeatureCursor o = layer.FeatureClass.Search(filter, false);
                        IFeature pFeature = o.NextFeature();
                        IFeature feature2 = o.NextFeature();
                        IFeature feature3 = o.NextFeature();
                        Marshal.ReleaseComObject(o);
                        o = null;
                        if (((pFeature != null) && (feature2 != null)) && (feature3 == null))
                        {
                            ITopologicalOperator2 shapeCopy = pFeature.ShapeCopy as ITopologicalOperator2;
                             shapeCopy.IsKnownSimple_2 = false;
                            shapeCopy.Simplify();
                            shapeCopy = feature2.ShapeCopy as ITopologicalOperator2;
                             shapeCopy.IsKnownSimple_2 = false;
                            shapeCopy.Simplify();
                            if (!shapeCopy.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension).IsEmpty)
                            {
                                IRelationalOperator operator2 = shapeCopy.Union(pFeature.ShapeCopy) as IRelationalOperator;
                                if (operator2.Contains(this._editBound))
                                {
                                    this._las.Add(new LinkArgs(pFeature));
                                    this._las.Add(new LinkArgs(feature2));
                                    Marshal.ReleaseComObject(o);
                                    o = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnClick()
        {
            IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
            if (targetLayer.SelectionSet.Count >= 1)
            {
                targetLayer.Clear();
                this._viewMap.Refresh();
            }
            IActiveViewEvents_Event event2 = this._viewMap as IActiveViewEvents_Event;
            event2.AfterDraw-=(new IActiveViewEvents_AfterDrawEventHandler(this.ave_AfterDraw));
            event2.AfterDraw+=(new IActiveViewEvents_AfterDrawEventHandler(this.ave_AfterDraw));
        }

        public bool OnContextMenu(int x, int y)
        {
            return false;
        }

        public void OnCreate(object hook)
        {
            this._mapControl = hook as IMapControl4;
            if (this._mapControl != null)
            {
                this._viewMap = this._mapControl.Map as IActiveView;
                this._feedBack.Display = this._viewMap.ScreenDisplay;
                this._cursor = this._cursorNormal = ToolCursor.Editing;
                this._cursorSnap = ToolCursor.VertexSnaped;
            }
        }

        public void OnDblClick()
        {
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                this.ClearUp();
            }
            if ((keyCode == 8) || ((shift == 2) && (keyCode == 90)))
            {
                int count = this._editInfoList.Count;
                if (count == 0)
                {
                    this.ClearUp();
                }
                else
                {
                    EditInfo info = this._editInfoList[count - 1] as EditInfo;
                    int vertexIndex = info.VertexIndex;
                    IPoint oldPoint = info.OldPoint;
                    (Editor.UniqueInstance.LinageShape as IPointCollection).UpdatePoint(vertexIndex, oldPoint);
                    this._editInfoList.RemoveAt(count - 1);
                    this._feedBack.Start((Editor.UniqueInstance.LinageShape as IGeometryCollection).get_Geometry(0) as IPath, vertexIndex, false);
                    this._feedBack.Refresh(this._viewMap.ScreenDisplay.hDC);
                    this._viewMap.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
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
                double mouseTolerance;
                IPoint queryPoint = this._viewMap.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                if (queryPoint.SpatialReference != (EditTask.EditLayer.FeatureClass as IGeoDataset).SpatialReference)
                {
                    queryPoint.Project((EditTask.EditLayer.FeatureClass as IGeoDataset).SpatialReference);
                    queryPoint.SpatialReference = (EditTask.EditLayer.FeatureClass as IGeoDataset).SpatialReference;
                    mouseTolerance = ToolConfig.MouseTolerance1;
                }
                else
                {
                    mouseTolerance = ToolConfig.MouseTolerance;
                }
                double buffer = this._viewMap.ScreenDisplay.DisplayTransformation.FromPoints(mouseTolerance);
                if (this._isVertexEditing)
                {
                    if (this._isSnaped)
                    {
                        this._isVertexSelected = true;
                        int hitSegmentIndex = -1;
                        double searchRadius = buffer;
                        IHitTest linageShape = Editor.UniqueInstance.LinageShape as IHitTest;
                        IPoint hitPoint = null;
                        double hitDistance = 0.0;
                        bool bRightSide = false;
                        int hitPartIndex = -1;
                        foreach (LinkArgs args in this._las)
                        {
                            (args.feature.ShapeCopy as IHitTest).HitTest(queryPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartVertex, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);
                            args.PartIndex = hitPartIndex;
                            args.VertexIndex.Add(hitSegmentIndex);
                        }
                        this._feedBack.Start((Editor.UniqueInstance.LinageShape as IGeometryCollection).get_Geometry(0) as IPath, this._vertexIndex, false);
                    }
                    else
                    {
                        if (!(Editor.UniqueInstance.LinageShape as IRelationalOperator).Equals(this._editOrigShape))
                        {
                            this.SaveEdit();
                            this._viewMap.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this._editBound.Envelope);
                        }
                        Marshal.ReleaseComObject(this._editOrigShape);
                        Marshal.ReleaseComObject(Editor.UniqueInstance.LinageShape);
                        Editor.UniqueInstance.LinageShape = null;
                        this._editInfoList.Clear();
                        this.InitLinkage(queryPoint, buffer);
                    }
                }
                else
                {
                    this._editInfoList.Clear();
                    this.InitLinkage(queryPoint, buffer);
                }
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            if (this._isVertexEditing)
            {
                IPoint other = this._viewMap.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                if (this._isVertexSelected)
                {
                    IRelationalOperator @operator = this._editBound as IRelationalOperator;
                    if (@operator.Contains(other))
                    {
                        this._feedBack.MoveTo(other);
                    }
                    else
                    {
                        this.ClearUp();
                        this._cursor = this._cursorNormal;
                    }
                }
                else
                {
                    this.SnapVertex(other);
                }
            }
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            if ((button == 1) && this._isVertexSelected)
            {
                this._feedBack.Stop();
                this._feedBack.Refresh(this._viewMap.ScreenDisplay.hDC);
                this._endPoint = this._viewMap.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                this._isVertexSelected = false;
                IPointCollection linageShape = Editor.UniqueInstance.LinageShape as IPointCollection;
                IPoint point = linageShape.get_Point(this._vertexIndex);
                linageShape.UpdatePoint(this._vertexIndex, this._endPoint);
                EditInfo info = new EditInfo {
                    VertexIndex = this._vertexIndex,
                    OldPoint = point,
                    NewPoint = this._endPoint
                };
                this._editInfoList.Add(info);
                this._viewMap.PartialRefresh(esriViewDrawPhase.esriViewForeground, Editor.UniqueInstance.TargetLayer, this._editBound.Envelope);
            }
        }

        public void Refresh(int hdc)
        {
        }

        public void SaveEdit()
        {
            try
            {
                Editor.UniqueInstance.StartEditOperation();
                object before = new object();
                object missing = Type.Missing;
                foreach (LinkArgs args in this._las)
                {
                    if (args.PartIndex >= 0)
                    {
                        IFeature feature = args.feature;
                        IGeometryCollection shapeCopy = feature.ShapeCopy as IGeometryCollection;
                        IPointCollection points = shapeCopy.get_Geometry(args.PartIndex) as IPointCollection;
                        for (int i = 0; i < this._editInfoList.Count; i++)
                        {
                            EditInfo info = this._editInfoList[i] as EditInfo;
                            int index = args.VertexIndex[i];
                            IPoint p = (info.NewPoint as IClone).Clone() as IPoint;
                            if (p.SpatialReference != (EditTask.EditLayer.FeatureClass as IGeoDataset).SpatialReference)
                            {
                                p.Project((EditTask.EditLayer.FeatureClass as IGeoDataset).SpatialReference);
                                p.SpatialReference = (EditTask.EditLayer.FeatureClass as IGeoDataset).SpatialReference;
                            }
                            if (index == 0)
                            {
                                points.UpdatePoint(0, p);
                            }
                            else
                            {
                                points.ReplacePoints(index, 1, 1, ref p);
                            }
                        }
                        shapeCopy.RemoveGeometries(args.PartIndex, 1);
                        before = args.PartIndex;
                        shapeCopy.AddGeometry(points as IGeometry, ref before, ref missing);
                        IGeometry geometry = (IGeometry) shapeCopy;
                        feature.Shape = geometry;
                        feature.Store();
                    }
                }
                Editor.UniqueInstance.StopEditOperation("Linkage Edit");
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.LinkageEdit", "SaveEdit", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void SnapVertex(IPoint mousePoint)
        {
            int hitPartIndex = -1;
            this._vertexIndex = -1;
            this._isSnaped = false;
            double searchRadius = this._viewMap.ScreenDisplay.DisplayTransformation.FromPoints(ToolConfig.MouseTolerance);
            IHitTest linageShape = Editor.UniqueInstance.LinageShape as IHitTest;
            IPoint hitPoint = null;
            double hitDistance = 0.0;
            bool bRightSide = false;
            bool flag2 = linageShape.HitTest(mousePoint, searchRadius, esriGeometryHitPartType.esriGeometryPartVertex, hitPoint, ref hitDistance, ref hitPartIndex, ref this._vertexIndex, ref bRightSide);
            this._isSnaped = (flag2 && (this._vertexIndex > 0)) && (this._vertexIndex < ((Editor.UniqueInstance.LinageShape as IPointCollection).PointCount - 1));
            if (this._isSnaped)
            {
                this._cursor = this._cursorSnap;
            }
            else
            {
                this._cursor = this._cursorNormal;
            }
        }

        public int Bitmap
        {
            get
            {
                return this._bitmap;
            }
        }

        public string Caption
        {
            get
            {
                return "边界联动";
            }
        }

        public string Category
        {
            get
            {
                return null;
            }
        }

        public bool Checked
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
                return this._cursor;
            }
        }

        public bool Enabled
        {
            get
            {
                if (!Editor.UniqueInstance.IsBeingEdited)
                {
                    return false;
                }
                return ((((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null)) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)) && Editor.UniqueInstance.TargetLayer.Visible);
            }
        }

        public int HelpContextID
        {
            get
            {
                return -1;
            }
        }

        public string HelpFile
        {
            get
            {
                return null;
            }
        }

        private IFeatureLayer[] LinkLayers
        {
            set
            {
                this._linkLayers = value;
            }
        }

        public string Message
        {
            get
            {
                return "边界联动";
            }
        }

        public string Name
        {
            get
            {
                return null;
            }
        }

        public string Tooltip
        {
            get
            {
                return "边界联动";
            }
        }

        private class EditInfo
        {
            public IPoint NewPoint;
            public IPoint OldPoint;
            public int VertexIndex = -1;
        }
    }
}

