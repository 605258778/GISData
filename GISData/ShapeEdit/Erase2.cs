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
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 裁切工具类
    /// </summary>
    [Guid("6c1ae647-9c36-4e3d-a544-136e54e581bd"), ProgId("ShapeEdit.Tool1"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Erase2 : BaseTool
    {
        private int _cursor;
        private IFeature m_Feature1;
        private IFeature m_Feature2;
        private IHookHelper m_hookHelper;
        private const string mClassName = "ShapeEdit.Erase2";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 裁切工具类：构造器
        /// </summary>
        public Erase2()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "裁切";
            base.m_message = "裁切";
            base.m_toolTip = "裁切";
            base.m_name = "ShapeEdit_Erase2";
            try
            {
                string resource = base.GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(base.GetType(), resource);
                base.m_cursor = new System.Windows.Forms.Cursor(base.GetType(), base.GetType().Name + ".cur");
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message, "Invalid Bitmap");
            }
        }

        private static void ArcGISCategoryRegistration(System.Type registerType)
        {
            ControlsCommands.Register(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        private static void ArcGISCategoryUnregistration(System.Type registerType)
        {
            ControlsCommands.Unregister(string.Format(@"HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID));
        }

        public override void OnClick()
        {
        }

        public override void OnCreate(object hook)
        {
            if (this.m_hookHelper == null)
            {
                this.m_hookHelper = new HookHelperClass();
            }
            this.m_hookHelper.Hook = hook;
            this._cursor = ToolCursor.Erase2;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            IPoint point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            try
            {
                IFeatureLayer targetLayer = Editor.UniqueInstance.TargetLayer;
                IFeatureClass featureClass = targetLayer.FeatureClass;
                ISpatialFilter queryFilter = null;
                queryFilter = new SpatialFilterClass {
                    Geometry = point,
                    GeometryField = featureClass.ShapeFieldName,
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin
                };
                IFeature feature = null;
                IFeatureCursor cursor = targetLayer.Search(queryFilter, false);
                IFeature item = cursor.NextFeature();
                int num = 0;
                IList<IFeature> pList = new List<IFeature>();
                while (item != null)
                {
                    if ((this.m_Feature1 != null) && (item.OID == this.m_Feature1.OID))
                    {
                        item = cursor.NextFeature();
                    }
                    else
                    {
                        feature = item;
                        num++;
                        pList.Add(item);
                        item = cursor.NextFeature();
                    }
                }
                if (num < 1)
                {
                    MessageBox.Show("当前无要素可进行裁切！", "提示");
                }
                else
                {
                    if (num > 1)
                    {
                        OverlapSublot sublot = new OverlapSublot(this.m_hookHelper.Hook, pList) {
                            Text = "裁切",
                            Tip = "超过一个要素被选中，请选择需要的小班ID"
                        };
                        if (sublot.ShowDialog() == DialogResult.OK)
                        {
                            int selectedIndex = sublot.SelectedIndex;
                            feature = pList[selectedIndex];
                        }
                        else
                        {
                            MessageBox.Show("超过一个要素被选中，无法进行裁切！", "提示");
                            return;
                        }
                    }
                    IFeatureSelection selection = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                    if (this.m_Feature1 == null)
                    {
                        this.m_Feature1 = feature;
                        selection.Add(this.m_Feature1);
                        this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                        this._cursor = ToolCursor.Erase22;
                    }
                    else
                    {
                        this.m_Feature2 = feature;
                        selection.Add(this.m_Feature2);
                        this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                        IGeometry shapeCopy = this.m_Feature1.ShapeCopy;
                        IGeometry other = this.m_Feature2.ShapeCopy;
                        ITopologicalOperator2 @operator = shapeCopy as ITopologicalOperator2;
                         @operator.IsKnownSimple_2 = false;
                        @operator.Simplify();
                        IGeometry geometry3 = @operator.Intersect(other, esriGeometryDimension.esriGeometry2Dimension);
                        if (geometry3.IsEmpty)
                        {
                            MessageBox.Show("选中两个要素无相交部分，无法进行裁切！", "提示");
                            this.m_Feature1 = null;
                            this.m_Feature2 = null;
                            this._cursor = ToolCursor.Erase2;
                        }
                        else
                        {
                            ITopologicalOperator2 operator2 = geometry3 as ITopologicalOperator2;
                            operator2.IsKnownSimple_2 = false;
                            operator2.Simplify();
                            IGeometry geometry4 = (other as ITopologicalOperator2).Difference(geometry3);
                            if (geometry4.IsEmpty)
                            {
                                MessageBox.Show("选中两个要素完全重合，无法进行裁切！", "提示");
                                this.m_Feature1 = null;
                                this.m_Feature2 = null;
                                this._cursor = ToolCursor.Erase2;
                            }
                            else
                            {
                                Editor.UniqueInstance.CheckOverlap = false;
                                Editor.UniqueInstance.StartEditOperation();
                                this.m_Feature2.Shape = geometry4;
                                this.m_Feature2.Store();
                                Editor.UniqueInstance.StopEditOperation();
                                Editor.UniqueInstance.CheckOverlap = true;
                                this.m_Feature1 = null;
                                this.m_Feature2 = null;
                                this._cursor = ToolCursor.Erase2;
                                (Editor.UniqueInstance.TargetLayer as IFeatureSelection).Clear();
                                this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this.m_Feature1 = null;
                this.m_Feature2 = null;
                this._cursor = ToolCursor.Erase2;
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.Erase2", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
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

        public override int Cursor
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
                return (((Editor.UniqueInstance.TargetLayer != null) && (Editor.UniqueInstance.TargetLayer.FeatureClass != null)) && (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon));
            }
        }
    }
}

