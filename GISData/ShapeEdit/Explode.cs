namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 打散
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.Explode"), Guid("79828737-c198-4c3b-91e6-1893f93b2de4")]
    public sealed class Explode : BaseCommand
    {
        private const string _mClassName = "ShapeEdit.Explode";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private IHookHelper m_hookHelper;

        /// <summary>
        /// 打散
        /// </summary>
        public Explode()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "打散";
            base.m_message = "打散";
            base.m_toolTip = "打散";
            base.m_name = "ShapeEdit_Explode";
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private void CopyAttributes(IObject pSrcObj, IObject pTargObj)
        {
            if ((pSrcObj != null) && (pTargObj != null))
            {
                IFields fields = pTargObj.Fields;
                try
                {
                    for (int i = 0; i < fields.FieldCount; i++)
                    {
                        IField field = fields.get_Field(i);
                        if ((field.Type != esriFieldType.esriFieldTypeGeometry) && field.Editable)
                        {
                            string name = field.Name;
                            object obj2 = pSrcObj.get_Value(i);
                            pTargObj.set_Value(i, obj2);
                        }
                    }
                    pTargObj.Store();
                }
                catch
                {
                }
            }
        }

        public bool Deactivate()
        {
            return true;
        }

        private void ExplodeFeature(IFeature pFeature)
        {
            IFeatureLayer targetLayer = Editor.UniqueInstance.TargetLayer;
            if (targetLayer != null)
            {
                IFeatureClass featureClass = targetLayer.FeatureClass;
                if (featureClass != null)
                {
                    IGeometry shapeCopy = pFeature.ShapeCopy;
                    IList<IGeometry> geometrys = this.GetGeometrys(shapeCopy);
                    if (geometrys == null)
                    {
                        MessageBox.Show("选中要素获取多部件，无法打散！", "打散");
                    }
                    else
                    {
                        int count = geometrys.Count;
                        if (count < 2)
                        {
                            MessageBox.Show("选中要素没有多个部件，无法打散！", "打散");
                        }
                        else
                        {
                            Editor.UniqueInstance.CheckOverlap = false;
                            Editor.UniqueInstance.AddAttribute = false;
                            IAttributeSplit attributeSplitHandleClass = AttributeManager.AttributeSplitHandleClass;
                            if (attributeSplitHandleClass != null)
                            {
                                Editor.UniqueInstance.StartEditOperation();
                                try
                                {
                                    int num2 = 0;
                                    List<IFeature> pFeatureList = new List<IFeature>();
                                    for (int i = 0; i < count; i++)
                                    {
                                        IGeometry geometry2 = geometrys[i];
                                        IFeature item = null;
                                        if (num2 == 0)
                                        {
                                            item = pFeature;
                                        }
                                        else
                                        {
                                            item = featureClass.CreateFeature();
                                        }
                                        item.Shape = geometry2;
                                        item.Store();
                                        pFeatureList.Add(item);
                                        num2++;
                                    }
                                    attributeSplitHandleClass.AttributeSplit(pFeature, ref pFeatureList);
                                    Editor.UniqueInstance.StopEditOperation("Explode");
                                }
                                catch (Exception exception)
                                {
                                    Editor.UniqueInstance.AbortEditOperation();
                                    this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Explode", "ExplodeFeature", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                                }
                                Editor.UniqueInstance.CheckOverlap = true;
                            }
                        }
                    }
                }
            }
        }

        private IList<IGeometry> GetGeometrys(IGeometry pGeo)
        {
            IList<IGeometry> list = new List<IGeometry>();
            try
            {
                if (pGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    IGeometryCollection geometrys = pGeo as IGeometryCollection;
                    int geometryCount = geometrys.GeometryCount;
                    for (int i = 0; i < geometryCount; i++)
                    {
                        IGeometry inGeometry = geometrys.get_Geometry(i);
                        if (!inGeometry.IsEmpty)
                        {
                            IGeometryCollection geometrys2 = new PolylineClass();
                            object missing = System.Type.Missing;
                            geometrys2.AddGeometry(inGeometry, ref missing, ref missing);
                            IGeometry item = geometrys2 as IGeometry;
                            item.SpatialReference = pGeo.SpatialReference;
                            list.Add(item);
                        }
                    }
                    return list;
                }
                if (pGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    IPolygon4 polygon = pGeo as IPolygon4;
                    if (polygon.ExteriorRingCount < 2)
                    {
                        list.Add(pGeo);
                        return list;
                    }
                    IEnumGeometry exteriorRingBag = polygon.ExteriorRingBag as IEnumGeometry;
                    exteriorRingBag.Reset();
                    for (IRing ring = exteriorRingBag.Next() as IRing; ring != null; ring = exteriorRingBag.Next() as IRing)
                    {
                        IGeometryBag bag2 = polygon.get_InteriorRingBag(ring);
                        object before = System.Type.Missing;
                        IGeometryCollection geometrys3 = null;
                        geometrys3 = new PolygonClass();
                        geometrys3.AddGeometry(ring, ref before, ref before);
                        IPolygon polygon2 = geometrys3 as IPolygon;
                        polygon2.SpatialReference = pGeo.SpatialReference;
                        ITopologicalOperator2 @operator = (ITopologicalOperator2) polygon2;
                        @operator.IsKnownSimple_2 = false;
                        @operator.Simplify();
                        if (!bag2.IsEmpty)
                        {
                            IGeometryCollection geometrys4 = new PolygonClass();
                            IEnumGeometry geometry4 = bag2 as IEnumGeometry;
                            geometry4.Reset();
                            for (IRing ring2 = geometry4.Next() as IRing; ring2 != null; ring2 = geometry4.Next() as IRing)
                            {
                                geometrys4.AddGeometry(ring2, ref before, ref before);
                            }
                            IPolygon other = geometrys4 as IPolygon;
                            other.SpatialReference = pGeo.SpatialReference;
                            ITopologicalOperator2 operator2 = (ITopologicalOperator2) other;
                              operator2.IsKnownSimple_2 = false;
                            operator2.Simplify();
                            IGeometry geometry5 = @operator.Difference(other);
                            list.Add(geometry5);
                        }
                        else
                        {
                            list.Add(polygon2);
                        }
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
            return list;
        }

        public override void OnClick()
        {
            IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
            IEnumIDs iDs = targetLayer.SelectionSet.IDs;
            iDs.Reset();
            int iD = iDs.Next();
            if (iD != -1)
            {
                IFeature pFeature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(iD);
                this.ExplodeFeature(pFeature);
            }
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
        }

        public void Refresh(int hdc)
        {
        }

        [ComVisible(false), ComRegisterFunction]
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
                return 0;
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

