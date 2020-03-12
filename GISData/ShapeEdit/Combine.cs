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
    /// 合并选择的要素
    /// </summary>
    [ProgId("ShapeEdit.Combine"), Guid("122cc760-5a33-45d5-997d-f7b499a9ce40"), ClassInterface(ClassInterfaceType.None)]
    public sealed class Combine : BaseCommand
    {
        private IHookHelper _hookHelper;
        private const string _mClassName = "ShapeEdit.Combine";
        private ErrorOpt _mErrOpt = UtilFactory.GetErrorOpt();
        private string _mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 合并选择的要素
        /// </summary>
        public Combine()
        {
            base.m_category = "ShapeEdit";
            base.m_caption = "合并";
            base.m_message = "合并选择的要素";
            base.m_toolTip = "合并选择的要素";
            base.m_name = "ShapeEdit_Combine";
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
            try
            {
                ITopologicalOperator2 @operator;
                List<IFeature> list;
                IFeature feature2;
                IFeatureSelection targetLayer = Editor.UniqueInstance.TargetLayer as IFeatureSelection;
                IEnumIDs iDs = targetLayer.SelectionSet.IDs;
                iDs.Reset();
                int iD = iDs.Next();
                if (iD != -1)
                {
                    IFeature feature = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(iD);
                    @operator = feature.Shape as ITopologicalOperator2;
                    @operator.IsKnownSimple_2 = false;
                    @operator.Simplify();
                    iD = iDs.Next();
                    list = null;
                    if (iD != -1)
                    {
                        list = new List<IFeature> {
                            feature
                        };
                        goto Label_00F4;
                    }
                }
                return;
            Label_0088:
                feature2 = Editor.UniqueInstance.TargetLayer.FeatureClass.GetFeature(iD);
                ITopologicalOperator2 shape = feature2.Shape as ITopologicalOperator2;
                   shape.IsKnownSimple_2 = false;
                shape.Simplify();
                @operator = @operator.Union(feature2.Shape) as ITopologicalOperator2;
                  @operator.IsKnownSimple_2 = false;
                @operator.Simplify();
                iD = iDs.Next();
                list.Add(feature2);
            Label_00F4:
                if (iD != -1)
                {
                    goto Label_0088;
                }
                Editor.UniqueInstance.StartEditOperation();
                Editor.UniqueInstance.AddAttribute = false;
                IFeature resultFeature = Editor.UniqueInstance.TargetLayer.FeatureClass.CreateFeature();
                resultFeature.Shape = @operator as IGeometry;
                if (AttributeManager.AttributeCombineHandleClass.AttributeCombine(list, ref resultFeature) == DialogResult.OK)
                {
                    resultFeature.Store();
                    foreach (IFeature feature4 in list)
                    {
                        feature4.Delete();
                    }
                    targetLayer.Clear();
                    targetLayer.Add(resultFeature);
                }
                Editor.UniqueInstance.StopEditOperation("combine");
                this._hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, null);
            }
            catch (Exception exception)
            {
                Editor.UniqueInstance.AbortEditOperation();
                this._mErrOpt.ErrorOperate(this._mSubSysName, "ShapeEdit.Combine", "OnClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
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
                if (targetLayer.SelectionSet.Count < 2)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

