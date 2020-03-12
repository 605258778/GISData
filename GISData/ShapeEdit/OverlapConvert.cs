namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using FunFactory;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 将重叠部分转化为小班工具类
    /// </summary>
    [Guid("e34abd4a-ffff-4b6b-afd5-18993ba1ee2e"), ProgId("ShapeEdit.OverlapConvert"), ClassInterface(ClassInterfaceType.None)]
    public sealed class OverlapConvert : BaseCommand, ITool
    {
        private List<IFeature> _features = new List<IFeature>();
        private IHookHelper m_hookHelper;
        private const string mClassName = "ShapeEdit.OverlapConvert";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 将重叠部分转化为小班工具类：构造器
        /// </summary>
        public OverlapConvert()
        {
            base.m_category = "Topo";
            base.m_caption = "转化为小班";
            base.m_message = "将重叠部分转化为小班";
            base.m_toolTip = "转化为小班";
            base.m_name = "Topo_OverlapConvert";
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
            if (button != 2)
            {
                try
                {
                    IPoint pGeometry = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    IFeatureClass featureClass = Editor.UniqueInstance.TargetLayer.FeatureClass;
                    ISpatialFilter queryFilter = new SpatialFilterClass {
                        Geometry = pGeometry,
                        GeometryField = featureClass.ShapeFieldName,
                        SubFields = featureClass.ShapeFieldName,
                        SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin
                    };
                    IFeatureCursor o = Editor.UniqueInstance.TargetLayer.Search(queryFilter, false);
                    IFeature feature = o.NextFeature();
                    IFeature feature2 = o.NextFeature();
                    Marshal.ReleaseComObject(o);
                    o = null;
                    if ((feature != null) && (feature2 != null))
                    {
                        feature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(feature.OID);
                        feature2 = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(feature2.OID);
                        ITopologicalOperator2 shape = feature.Shape as ITopologicalOperator2;
                        IGeometry geometry1 = feature2.Shape;
                        IGeometry other = shape.Intersect(feature2.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        if (!other.IsEmpty)
                        {
                            pGeometry = GISFunFactory.UnitFun.ConvertPoject(pGeometry, other.SpatialReference) as IPoint;
                            IGeometryCollection geometrys = other as IGeometryCollection;
                            if (geometrys.GeometryCount > 1)
                            {
                                for (int i = 0; i < geometrys.GeometryCount; i++)
                                {
                                    IGeometry inGeometry = geometrys.get_Geometry(i);
                                    if (!inGeometry.IsEmpty)
                                    {
                                        if (inGeometry.GeometryType == esriGeometryType.esriGeometryRing)
                                        {
                                            object missing = System.Type.Missing;
                                            IGeometryCollection geometrys2 = new PolygonClass();
                                            geometrys2.AddGeometry(inGeometry, ref missing, ref missing);
                                            IPolygon polygon = geometrys2 as IPolygon;
                                            inGeometry = polygon;
                                        }
                                        IRelationalOperator operator2 = inGeometry as IRelationalOperator;
                                        if ((operator2 != null) && operator2.Contains(pGeometry))
                                        {
                                            other = inGeometry;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                IRelationalOperator operator3 = other as IRelationalOperator;
                                if (!operator3.Contains(pGeometry))
                                {
                                    return;
                                }
                            }
                            feature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(feature.OID);
                            feature2 = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(feature2.OID);
                            if (other.GeometryType != esriGeometryType.esriGeometryPolygon)
                            {
                                IPolygon polygon2 = new PolygonClass {
                                    SpatialReference = other.SpatialReference
                                };
                                IPointCollection newPoints = other as IPointCollection;
                                (polygon2 as IPointCollection).AddPointCollection(newPoints);
                                other = polygon2;
                            }
                            Editor.UniqueInstance.StartEditOperation();
                            ITopologicalOperator2 operator4 = feature.Shape as ITopologicalOperator2;
                            IGeometry geometry3 = operator4.Difference(other);
                            if (geometry3.IsEmpty)
                            {
                                feature.Delete();
                            }
                            else
                            {
                                feature.Shape = geometry3;
                                feature.Store();
                            }
                            geometry3 = (feature2.Shape as ITopologicalOperator2).Difference(other);
                            if (geometry3.IsEmpty)
                            {
                                feature2.Delete();
                            }
                            else
                            {
                                feature2.Shape = geometry3;
                                feature2.Store();
                            }
                            Editor.UniqueInstance.AddAttribute = true;
                            IFeature feature3 = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                            feature3.Shape = other;
                            feature3.Store();
                            Editor.UniqueInstance.StopEditOperation();
                            Editor.UniqueInstance.AddAttribute = false;
                            IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                            targetLayer.Clear();
                            targetLayer.Add(feature3);
                            this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, this.m_hookHelper.ActiveView.Extent);
                            MessageBox.Show("修改完成！", "提示");
                        }
                    }
                }
                catch (Exception exception)
                {
                    Editor.UniqueInstance.AbortEditOperation();
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.OverlapConvert", "OnMouseDown", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    MessageBox.Show("修改出错！", "提示");
                }
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
        }

        public void Refresh(int hdc)
        {
        }

        [ComRegisterFunction, ComVisible(false)]
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
                return ToolCursor.Fix;
            }
        }

        public override bool Enabled
        {
            get
            {
                return ((Editor.UniqueInstance.IsBeingEdited && (Editor.UniqueInstance.TargetLayer != null)) && ((Editor.UniqueInstance.TargetLayer.FeatureClass != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)));
            }
        }
    }
}

