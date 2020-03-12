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
    /// 删除节点工具类
    /// </summary>
    [Guid("57fe68f0-727c-49ec-8023-384e3dc49837"), ProgId("ShapeEdit.DeleteVertex"), ClassInterface(ClassInterfaceType.None)]
    public sealed class DeleteVertex : BaseCommand, ITool
    {
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.DeleteVertex";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 删除节点工具类：构造器
        /// </summary>
        public DeleteVertex()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "删除节点";
            base.m_message = "删除节点";
            base.m_toolTip = "删除节点";
            base.m_name = "ShapeEdit_DeleteVertex";
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
                    double searchRadius = this._hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.FromPoints(ToolConfig.MouseTolerance);
                    esriGeometryHitPartType esriGeometryPartVertex = esriGeometryHitPartType.esriGeometryPartVertex;
                    test.HitTest(queryPoint, searchRadius, esriGeometryPartVertex, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);
                    if (!hitPoint.IsEmpty)
                    {
                        IEngineSketchOperation operation = new EngineSketchOperationClass();
                        operation.Start(Editor.UniqueInstance.EngineEditor);
                        IGeometryCollection geometrys = editShape as IGeometryCollection;
                        IPointCollection points = geometrys.get_Geometry(hitPartIndex) as IPointCollection;
                        object missing = Type.Missing;
                        new object();
                        object before = new object();
                        before = hitPartIndex;
                        points.RemovePoints(hitSegmentIndex, 1);
                        operation.SetMenuString("Delete Vertex");
                        esriEngineSketchOperationType esriEngineSketchOperationVertexDeleted = esriEngineSketchOperationType.esriEngineSketchOperationVertexDeleted;
                        geometrys.RemoveGeometries(hitPartIndex, 1);
                        geometrys.AddGeometry(points as IGeometry, ref before, ref missing);
                        operation.Finish(null, esriEngineSketchOperationVertexDeleted, queryPoint);
                        Editor.UniqueInstance.FinishSketch();
                        this.OnClick();
                    }
                }
                catch (Exception exception)
                {
                    Editor.UniqueInstance.AbortEditOperation();
                    this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.DeleteVertex", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
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
                return ToolCursor.DeleteVertex;
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

