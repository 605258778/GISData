namespace ShapeEdit
{
    using ESRI.ArcGIS.ADF.BaseClasses;
    using ESRI.ArcGIS.ADF.CATIDs;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using System;
    using System.Runtime.InteropServices;
    using TaskManage;
    using Utilities;

    [ClassInterface(ClassInterfaceType.None), ProgId("ShapeEdit.RPointDelete"), Guid("397de8cd-15df-44a8-8bec-00a210a92295")]
    public sealed class RPointDelete : BaseCommand, ITool
    {
        private string _errInf;
        private IFeature _feature;
        private INewPolygonFeedback _feedBack;
        private IHookHelper m_hookHelper;
        private const string mClassName = "ShapeEdit.RPointDelete";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        public RPointDelete(IFeature pFeature, string pErrInf)
        {
            base.m_category = "Topo";
            base.m_caption = "删除重复点";
            base.m_message = "删除重复点";
            base.m_toolTip = "删除重复点";
            base.m_name = "Topo_DeleteRPoint";
            this._feature = pFeature;
            this._errInf = pErrInf;
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
                this._feedBack = new NewPolygonFeedbackClass();
                this._feedBack.Display = this.m_hookHelper.ActiveView.ScreenDisplay;
            }
        }

        public void OnDblClick()
        {
        }

        public void OnKeyDown(int keyCode, int shift)
        {
            if (keyCode == 0x1b)
            {
                this._feedBack.Stop();
            }
        }

        public void OnKeyUp(int keyCode, int shift)
        {
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
            if (button != 2)
            {
                IPoint point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                this._feedBack.Start(point);
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            IPoint point = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            this._feedBack.MoveTo(point);
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            IRelationalOperator envelope = this._feedBack.Stop().Envelope as IRelationalOperator;
            if (!envelope.Disjoint(this._feature.Shape))
            {
                string[] strArray = this._errInf.Split(new char[] { ',' });
                double num = double.Parse(strArray[0]);
                double num2 = double.Parse(strArray[1]);
                IPoint point = new PointClass {
                    X = num,
                    Y = num2
                };
                if (!envelope.Disjoint(point.Envelope))
                {
                    IGeometryCollection shape = this._feature.Shape as IGeometryCollection;
                    try
                    {
                        Editor.UniqueInstance.StartEditOperation();
                        for (int i = 0; i < shape.GeometryCount; i++)
                        {
                            IPointCollection points = shape.get_Geometry(i) as IPointCollection;
                            int num4 = -1;
                            for (int j = 0; j < points.PointCount; j++)
                            {
                                IPoint point2 = points.get_Point(j);
                                if ((point2.X == x) && (point2.Y == y))
                                {
                                    if (num4 == -1)
                                    {
                                        num4 = j;
                                    }
                                    else
                                    {
                                        points.RemovePoints(j, 1);
                                    }
                                }
                            }
                        }
                        Editor.UniqueInstance.StopEditOperation();
                        EditTask.ToplogicChkState = ToplogicCheckState.Failure;
                    }
                    catch (Exception exception)
                    {
                        Editor.UniqueInstance.AbortEditOperation();
                        this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.RPointDelete", "OnMouseUp", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    }
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
                return ToolCursor.DeleteVertex;
            }
        }

        public override bool Enabled
        {
            get
            {
                return ((Editor.UniqueInstance.IsBeingEdited && (this._feature != null)) && (this._errInf != null));
            }
        }
    }
}

