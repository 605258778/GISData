namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Runtime.InteropServices;
    using Utilities;

    /// <summary>
    /// 插入节点工具类
    /// </summary>
    [ProgId("ShapeEdit.InsertVertex"), Guid("7d5ad026-1510-4732-b550-d204993b51bc"), ClassInterface(ClassInterfaceType.None)]
    public sealed class InsertVertex : BaseCommand, ITool
    {
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.InsertVertex";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 插入节点工具类：构造器
        /// </summary>
        public InsertVertex()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "插入节点";
            base.m_message = "插入节点";
            base.m_toolTip = "插入节点";
            base.m_name = "ShapeEdit_InsertVertex";
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
            Editor.UniqueInstance.FinishSketch();
            return true;
        }

        public override void OnClick()
        {
            IEngineEditTask taskByUniqueName = Editor.UniqueInstance.EngineEditor.GetTaskByUniqueName("ControlToolsEditing_ModifyFeatureTask");
            Editor.UniqueInstance.EngineEditor.CurrentTask = taskByUniqueName;
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
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            if (button != 2)
            {
                try
                {
                    IGeometry editShape = Editor.UniqueInstance.EditShape;
                    IPoint queryPoint = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    IHitTest test = editShape as IHitTest;
                    IPoint hitPoint = new PointClass();
                    double hitDistance = 0.0;
                    int hitPartIndex = 0;
                    int hitSegmentIndex = 0;
                    bool bRightSide = false;
                    double searchRadius = 1.0 * this._hookHelper.ActiveView.FocusMap.MapScale;
                    esriGeometryHitPartType esriGeometryPartBoundary = esriGeometryHitPartType.esriGeometryPartBoundary;
                    test.HitTest(queryPoint, searchRadius, esriGeometryPartBoundary, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);
                    if (!hitPoint.IsEmpty)
                    {
                        IEngineSketchOperation operation = new EngineSketchOperationClass();
                        operation.Start(Editor.UniqueInstance.EngineEditor);
                        IGeometryCollection geometrys = editShape as IGeometryCollection;
                        IPointCollection points = geometrys.get_Geometry(hitPartIndex) as IPointCollection;
                        object missing = Type.Missing;
                        object after = new object();
                        after = hitSegmentIndex;
                        object before = new object();
                        before = hitPartIndex;
                        points.AddPoint(queryPoint, ref missing, ref after);
                        operation.SetMenuString("Insert Vertex");
                        esriEngineSketchOperationType esriEngineSketchOperationVertexAdded = esriEngineSketchOperationType.esriEngineSketchOperationVertexAdded;
                        geometrys.RemoveGeometries(hitPartIndex, 1);
                        geometrys.AddGeometry(points as IGeometry, ref before, ref missing);
                        operation.Finish(null, esriEngineSketchOperationVertexAdded, queryPoint);
                        Editor.UniqueInstance.FinishSketch();
                        this.OnClick();
                    }
                }
                catch (Exception exception)
                {
                    Editor.UniqueInstance.AbortEditOperation();
                    this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.InsertVertex", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
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

        public int Cursor
        {
            get
            {
                return ToolCursor.InsertVertex;
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
                if (((Editor.UniqueInstance.TargetLayer == null) || (Editor.UniqueInstance.TargetLayer.FeatureClass == null)) || (Editor.UniqueInstance.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon))
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

