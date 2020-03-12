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
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 合并的工具类
    /// </summary>
    [Guid("ca5792d0-99d3-46c3-8d3b-bcd30dce3ebc"), ProgId("ShapeEdit.CombineEx"), ClassInterface(ClassInterfaceType.None)]
    public sealed class CombineEx : BaseCommand, ITool, IFeatureTool
    {
        private IFeature _feature;
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.CombineEx";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 合并的工具类:构造器
        /// </summary>
        public CombineEx()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "合并";
            base.m_message = "合并面积错误的要素";
            base.m_toolTip = "合并面积错误的要素";
            base.m_name = "ShapeEdit_CombineEx";
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
        }

        public void OnKeyUp(int keyCode, int shift)
        {
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
            try
            {
                IPoint point = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                IFeatureClass featureClass = Editor.UniqueInstance.TargetLayer.FeatureClass;
                ISpatialFilter queryFilter = new SpatialFilterClass {
                    Geometry = point,
                    GeometryField = featureClass.ShapeFieldName,
                    SubFields = featureClass.ShapeFieldName,
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin
                };
                IFeatureCursor o = Editor.UniqueInstance.TargetLayer.Search(queryFilter, false);
                IFeature feature = o.NextFeature();
                Marshal.ReleaseComObject(o);
                o = null;
                if ((feature != null) && (feature.OID != this._feature.OID))
                {
                    feature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(feature.OID);
                    ITopologicalOperator2 shape = this._feature.Shape as ITopologicalOperator2;
                      shape.IsKnownSimple_2 = false;
                    shape.Simplify();
                    shape = feature.Shape as ITopologicalOperator2;
                        shape.IsKnownSimple_2 = false;
                    shape.Simplify();
                    shape = shape.Union(this._feature.Shape) as ITopologicalOperator2;
                      shape.IsKnownSimple_2 = false;
                    shape.Simplify();
                    Editor.UniqueInstance.StartEditOperation();
                    Editor.UniqueInstance.AddAttribute = false;
                    List<IFeature> pFeatureList = new List<IFeature> {
                        this._feature,
                        feature
                    };
                    IFeature resultFeature = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                    resultFeature.Shape = shape as IGeometry;
                    if (AttributeManager.AttributeCombineHandleClass.AttributeCombine(pFeatureList, ref resultFeature) == DialogResult.OK)
                    {
                        resultFeature.Store();
                        foreach (IFeature feature3 in pFeatureList)
                        {
                            feature3.Delete();
                        }
                        IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                        targetLayer.Clear();
                        targetLayer.Add(resultFeature);
                        this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, Editor.UniqueInstance.TargetLayer, resultFeature.Extent);
                    }
                    Editor.UniqueInstance.StopEditOperation("combineex");
                }
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.CombineEx", "OnMouseDown", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
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
                return ToolCursor.FeatureSelecting;
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

        public IFeature Feature
        {
            set
            {
                this._feature = value;
            }
        }
    }
}

