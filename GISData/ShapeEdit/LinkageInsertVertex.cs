namespace ShapeEdit
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using FunFactory;
    using System;
    using System.Collections.Generic;
    using Utilities;

    /// <summary>
    /// 添加顶点工具类
    /// </summary>
    public class LinkageInsertVertex : ITool, ICommand
    {
        private IActiveView _ac;
        private List<LinkArgs> _las = Editor.UniqueInstance.LinkArgs;
        private const string _mClassName = "ShapeEdit.LinkageInsertVertex";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        public bool Deactivate()
        {
            Editor.UniqueInstance.LinageShape = null;
            return true;
        }

        public void OnClick()
        {
            Editor.UniqueInstance.LinageShape = Editor.UniqueInstance.ReservedLinkShape;
        }

        public bool OnContextMenu(int x, int y)
        {
            return false;
        }

        public void OnCreate(object hook)
        {
            IMapControl4 control = hook as IMapControl4;
            if (control != null)
            {
                this._ac = control.ActiveView;
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
            if (button == 1)
            {
                try
                {
                    IGeometry shapeCopy = this._las[0].feature.ShapeCopy;
                    IGeometry geometry2 = this._las[1].feature.ShapeCopy;
                    IRelationalOperator @operator = shapeCopy as IRelationalOperator;
                    IRelationalOperator operator2 = geometry2 as IRelationalOperator;
                    IPoint queryPoint = this._ac.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    IPoint pGeometry = ((IClone) queryPoint).Clone() as IPoint;
                    pGeometry = GISFunFactory.UnitFun.ConvertPoject(pGeometry, shapeCopy.SpatialReference) as IPoint;
                    if (@operator.Contains(pGeometry) || operator2.Contains(pGeometry))
                    {
                        IPoint hitPoint = null;
                        double hitDistance = 0.0;
                        int hitPartIndex = -1;
                        int hitSegmentIndex = -1;
                        bool bRightSide = false;
                        double searchRadius = 1.0 * this._ac.FocusMap.MapScale;
                        IHitTest linageShape = Editor.UniqueInstance.LinageShape as IHitTest;
                        if (linageShape.HitTest(queryPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartBoundary, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                        {
                            object missing = Type.Missing;
                            object after = hitSegmentIndex;
                            IGeometryCollection geometrys = Editor.UniqueInstance.LinageShape as IGeometryCollection;
                            (geometrys.get_Geometry(hitPartIndex) as IPointCollection).AddPoint(queryPoint, ref missing, ref after);
                            try
                            {
                                Editor.UniqueInstance.StartEditOperation();
                                foreach (LinkArgs args in this._las)
                                {
                                    (args.feature.Shape as IHitTest).HitTest(pGeometry, searchRadius, esriGeometryHitPartType.esriGeometryPartBoundary, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);
                                    IFeature feature = args.feature;
                                    IGeometryCollection shape = feature.Shape as IGeometryCollection;
                                    IPointCollection points2 = shape.get_Geometry(hitPartIndex) as IPointCollection;
                                    after = hitSegmentIndex;
                                    points2.AddPoint(pGeometry, ref missing, ref after);
                                    shape.RemoveGeometries(hitPartIndex, 1);
                                    after = hitPartIndex;
                                    shape.AddGeometry(points2 as IGeometry, ref after, ref missing);
                                    feature.Shape = shape as IGeometry;
                                    feature.Store();
                                }
                                Editor.UniqueInstance.StopEditOperation("Linkage Insert Vertex");
                            }
                            catch
                            {
                                Editor.UniqueInstance.AbortEditOperation();
                            }
                            this._ac.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this._ac.Extent);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.LinkageInsertVertex", "OnMouseDown", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public void Refresh(int hdc)
        {
        }

        public int Bitmap
        {
            get
            {
                return 0;
            }
        }

        public string Caption
        {
            get
            {
                return "添加顶点";
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
                return ToolCursor.InsertVertex;
            }
        }

        public bool Enabled
        {
            get
            {
                return (((Editor.UniqueInstance.ReservedLinkShape != null) && !Editor.UniqueInstance.ReservedLinkShape.IsEmpty) && ((Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon) && (Editor.UniqueInstance.ReservedLinkShape.GeometryType == esriGeometryType.esriGeometryPolyline)));
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

        public string Message
        {
            get
            {
                return "添加顶点";
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
                return "添加顶点";
            }
        }
    }
}

