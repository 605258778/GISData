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
    using Utilities;

    /// <summary>
    /// 删除重复点工具类
    /// </summary>
    [Guid("af75432c-e2b3-46bc-9da9-11a20a2757ea"), ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.RPointDeleteEx")]
    public sealed class RPointDeleteEx : BaseCommand, ITool
    {
        private int _cursor;
        private IHookHelper m_hookHelper;
        private const string mClassName = "ShapeEdit.RPointDeleteEx";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 删除重复点工具类：构造器
        /// </summary>
        public RPointDeleteEx()
        {
            base.m_category = "Topo";
            base.m_caption = "删除重复点";
            base.m_message = "删除重复点";
            base.m_toolTip = "删除重复点";
            base.m_name = "Topo_RPointDeleteEx";
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
            this._cursor = ToolCursor.Delete;
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
                IPoint pPoint = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                IEnvelope searchEnvelope = FeatureFuncs.GetSearchEnvelope(this.m_hookHelper.ActiveView, pPoint);
                IFeature feature = FeatureFuncs.SearchFeatures(Editor.UniqueInstance.TargetLayer, searchEnvelope, esriSpatialRelEnum.esriSpatialRelIntersects).NextFeature();
                if (feature != null)
                {
                    bool flag = false;
                    Editor.UniqueInstance.StartEditOperation();
                    IGeometryCollection shape = feature.Shape as IGeometryCollection;
                    int geometryCount = shape.GeometryCount;
                    List<string> list = new List<string>();
                    for (int i = 0; i < geometryCount; i++)
                    {
                        flag = false;
                        list.Clear();
                        IGeometry geometry = shape.get_Geometry(i);
                        int num3 = 0;
                        if ((geometry.GeometryType == esriGeometryType.esriGeometryPolygon) || (geometry.GeometryType == esriGeometryType.esriGeometryRing))
                        {
                            IRing ring = geometry as IRing;
                            if (ring.IsClosed)
                            {
                                num3 = 1;
                            }
                        }
                        IPointCollection points = geometry as IPointCollection;
                        int pointCount = points.PointCount;
                        for (int j = num3; j < pointCount; j++)
                        {
                            IPoint point2 = points.get_Point(j);
                            string item = string.Format("{0:F3},{1:F3}", point2.X, point2.Y);
                            if (list.Contains(item))
                            {
                                points.RemovePoints(j, 1);
                                j--;
                                pointCount--;
                                flag = true;
                            }
                            else
                            {
                                list.Add(item);
                            }
                        }
                        if (flag)
                        {
                            object missing = Type.Missing;
                            object before = new object();
                            before = i;
                            shape.RemoveGeometries(i, 1);
                            shape.AddGeometry(points as IGeometry, ref before, ref missing);
                        }
                    }
                    feature.Shape = shape as IGeometry;
                    feature.Store();
                    Editor.UniqueInstance.StopEditOperation("delete repeat point");
                    this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.m_hookHelper.ActiveView.Extent);
                }
            }
        }

        public void Refresh(int hdc)
        {
        }

        [ComRegisterFunction, ComVisible(false)]
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
                return this._cursor;
            }
        }

        public override bool Enabled
        {
            get
            {
                if ((!Editor.UniqueInstance.IsBeingEdited || (Editor.UniqueInstance.TargetLayer == null)) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null))
                {
                    return false;
                }
                if (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

