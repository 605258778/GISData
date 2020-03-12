namespace ShapeEdit
{
    using DevExpress.XtraEditors;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.SystemUI;
    using FunFactory;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using TaskManage;
    using Utilities;

    public class Editor
    {
        private bool _addAttribute;
        private bool _bCreateNew;
        private bool _canEdit = true;
        private bool _checkBoundary = true;
        private bool _checkOverlap = true;
        private IFeature _copiedFeature;
        private IEngineEditor _editor = new EngineEditorClass();
        private bool _hasCopied;
        private IGeometry _linkageShape;
        private List<ShapeEdit.LinkArgs> _linkArgs = new List<ShapeEdit.LinkArgs>();
        private IMap _map;
        private IOperationStack _oStack;
        private IGeometry _reservedLinkShape;
        private bool _setArea = true;
        private List<IFeatureLayer> _snapLayers = new List<IFeatureLayer>();
        private IWorkspace _workspace;
        private const string mClassName = "ShapeEdit.Editor";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private IVersionedWorkspace pNewVerWS;
        private IVersion pVersion;
        private static Editor UniqueEditor = new Editor();

        public event UpdateFeature UpdateFeatureEvent;

        private Editor()
        {
            this._editor.EnableUndoRedo(true);
            IEngineEditEvents_Event event2 = this._editor as IEngineEditEvents_Event;
            event2.OnStopOperation += (new IEngineEditEvents_OnStopOperationEventHandler(this.editEvents_OnStopOperation));
            event2.OnCreateFeature += (new IEngineEditEvents_OnCreateFeatureEventHandler(this.editEvents_OnCreateFeature));
            event2.OnDeleteFeature += (new IEngineEditEvents_OnDeleteFeatureEventHandler(this.editEvents_OnDeleteFeature));
            event2.OnChangeFeature += (new IEngineEditEvents_OnChangeFeatureEventHandler(this.editEvents_OnChangeFeature));
        }

        public void AbortEditOperation()
        {
            this._editor.AbortOperation();
        }

        public void BegingUpdateFeature(IFeature pFeature)
        {
            if (this.UpdateFeatureEvent != null)
            {
                this.UpdateFeatureEvent(pFeature);
            }
        }

        public void BuddyToolBarControl(IToolbarControl pControl)
        {
            this._oStack = new ControlsOperationStackClass();
            pControl.OperationStack = this._oStack;
            IExtension extension = this._editor as IExtension;
            object initializationData = pControl.Object;
            extension.Startup(ref initializationData);
        }

        public void CancleSketch()
        {
            IEngineEditSketch sketch = this._editor as IEngineEditSketch;
            IPointCollection geometry = (IPointCollection)sketch.Geometry;
            while (geometry.PointCount > 0)
            {
                geometry.RemovePoints(geometry.PointCount - 1, geometry.PointCount);
            }
            sketch.Geometry = (IGeometry)geometry;
            sketch.RefreshSketch();
        }

        private bool CheckFeatureBoundary(IFeature pFeature)
        {
            try
            {
                return true;
            }
            catch
            {
                return true;
            }
        }

        public bool CheckFeatureOverlap(IGeometry pGeo, bool bChang)
        {
            try
            {
                IFeatureClass featureClass = UniqueInstance.TargetLayer.FeatureClass;
                ISpatialFilter filter = new SpatialFilterClass
                {
                    Geometry = pGeo,
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps,
                    GeometryField = featureClass.ShapeFieldName
                };
                if (featureClass.Search(filter, false).NextFeature() != null)
                {
                    return false;
                }
                filter = new SpatialFilterClass
                {
                    GeometryField = featureClass.ShapeFieldName,
                    Geometry = pGeo,
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
                };
                IFeatureCursor cursor = null;
                cursor = featureClass.Search(filter, false);
                IFeature feature = cursor.NextFeature();
                if (bChang)
                {
                    feature = cursor.NextFeature();
                }
                if (feature != null)
                {
                    return false;
                }
                filter.Geometry = pGeo;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                cursor = featureClass.Search(filter, false);
                feature = cursor.NextFeature();
                if (bChang)
                {
                    feature = cursor.NextFeature();
                }
                if (feature != null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        public void DrawShape(IDisplay pScreenDisplay, IGeometry pPolyLine)
        {
            IEngineEditProperties properties = this._editor as IEngineEditProperties;
            ILineSymbol sketchSymbol = properties.SketchSymbol;
            IMarkerSymbol sketchVertexSymbol = properties.SketchVertexSymbol;
            ITransformation displayTransformation = pScreenDisplay.DisplayTransformation;
            ISymbol symbol3 = sketchSymbol as ISymbol;
            symbol3.SetupDC(pScreenDisplay.hDC, displayTransformation);
            symbol3.Draw(pPolyLine);
            symbol3.ResetDC();
            symbol3 = sketchVertexSymbol as ISymbol;
            IPointCollection points = pPolyLine as IPointCollection;
            symbol3.SetupDC(pScreenDisplay.hDC, displayTransformation);
            for (int i = 0; i < points.PointCount; i++)
            {
                IPoint geometry = points.get_Point(i);
                symbol3.Draw(geometry);
            }
            symbol3.ResetDC();
        }

        private void editEvents_OnChangeFeature(IObject Object)
        {
            IFeatureChanges changes = Object as IFeatureChanges;
            if (this._bCreateNew)
            {
                this._bCreateNew = false;
            }
            else if ((changes != null) && changes.ShapeChanged)
            {
                if (this._setArea)
                {
                    IFeature pFeature = UniqueInstance.TargetLayer.FeatureClass.GetFeature(Object.OID);
                    this.SetFeatureArea(pFeature);
                }
                if (this._checkOverlap && (UniqueInstance.LinageShape == null))
                {
                    IFeature feature = UniqueInstance.TargetLayer.FeatureClass.GetFeature(Object.OID);
                    if ((feature != null) && !this.CheckFeatureOverlap(feature.ShapeCopy, true))
                    {
                        XtraMessageBox.Show("要素与其他要素重叠！");
                    }
                }
            }
        }

        private void editEvents_OnCreateFeature(IObject Object)
        {
            IFeature pFeature = UniqueInstance.TargetLayer.FeatureClass.GetFeature(Object.OID);
            this.BegingUpdateFeature(pFeature);
            IAttributeAdd attributeAddHandleClass = AttributeManager.AttributeAddHandleClass;
            if ((attributeAddHandleClass != null) && this._addAttribute)
            {
                this._addAttribute = false;
                this._bCreateNew = true;
                if (this._setArea)
                {
                    IFeature feature = UniqueInstance.TargetLayer.FeatureClass.GetFeature(Object.OID);
                    this.SetFeatureArea(feature);
                }
                attributeAddHandleClass.AttributeAdd(ref pFeature);
                (this._map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, pFeature.Shape.Envelope);
                (this._map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeography, null, pFeature.Shape.Envelope);
            }
        }

        private void editEvents_OnDeleteFeature(IObject Object)
        {
        }

        private void editEvents_OnStopOperation()
        {
            if (this._editor.HasEdits())
            {
                EditTask.ToplogicChkState = ToplogicCheckState.Failure;
            }
        }

        public void FinishSketch()
        {
            (this._editor as IEngineEditSketch).FinishSketch();
        }

        public List<IFeature> GetSelectedFeatures()
        {
            List<IFeature> list = new List<IFeature>();
            if (this.TargetLayer != null)
            {
                IFeatureSelection targetLayer = this.TargetLayer as IFeatureSelection;
                IEnumIDs iDs = targetLayer.SelectionSet.IDs;
                iDs.Reset();
                for (int i = iDs.Next(); i != -1; i = iDs.Next())
                {
                    IFeature item = this.TargetLayer.FeatureClass.GetFeature(i);
                    list.Add(item);
                }
            }
            return list;
        }

        private bool IsGeometryEqual(IGeometry shape1, IGeometry shape2)
        {
            if ((shape1 == null) & (shape2 == null))
            {
                return true;
            }
            if ((shape1 == null) ^ (shape2 == null))
            {
                return false;
            }
            IClone clone = (IClone)shape1;
            IClone other = (IClone)shape2;
            return clone.IsEqual(other);
        }

        private bool IsShapeInConflict(IFeature commonAncestorFeature, IFeature preReconcileFeature, IFeature reconcileFeature)
        {
            return ((!this.IsGeometryEqual(commonAncestorFeature.ShapeCopy, preReconcileFeature.ShapeCopy) && !this.IsGeometryEqual(commonAncestorFeature.ShapeCopy, reconcileFeature.ShapeCopy)) && !this.IsGeometryEqual(reconcileFeature.ShapeCopy, preReconcileFeature.ShapeCopy));
        }

        private bool ReconcileVersionFindConflicts(IFeatureWorkspace pFeatureWorkspace, string strTargetVersion)
        {
            try
            {
                bool flag = false;
                IVersionEdit edit = pFeatureWorkspace as IVersionEdit;
                edit.CanPost();
                if (edit.Reconcile(strTargetVersion))
                {
                    IVersion startEditingVersion = edit.StartEditingVersion;
                    IVersion preReconcileVersion = edit.PreReconcileVersion;
                    IVersion commonAncestorVersion = edit.CommonAncestorVersion;
                    IVersion reconcileVersion = edit.ReconcileVersion;
                    IEnumConflictClass conflictClasses = edit.ConflictClasses;
                    conflictClasses.Reset();
                    for (IConflictClass class3 = conflictClasses.Next(); class3 != null; class3 = conflictClasses.Next())
                    {
                        IDataset dataset = (IDataset)class3;
                        if (class3.HasConflicts)
                        {
                            flag = true;
                            string str = dataset.Name + ":";
                            IEnumIDs iDs = class3.UpdateUpdates.IDs;
                            iDs.Reset();
                            int num = iDs.Next();
                            while (num != -1)
                            {
                                str = str + num.ToString() + ";";
                                num = iDs.Next();
                            }
                            iDs = class3.DeleteUpdates.IDs;
                            iDs.Reset();
                            num = iDs.Next();
                            while (num != -1)
                            {
                                str = str + num.ToString() + ";";
                                num = iDs.Next();
                            }
                            iDs = class3.UpdateDeletes.IDs;
                            iDs.Reset();
                            for (num = iDs.Next(); num != -1; num = iDs.Next())
                            {
                                str = str + num.ToString() + ";";
                            }
                            MessageBox.Show("发现冲突," + str, "版本编辑", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show(dataset.Name + " Does Not Have Conflicts", "版本编辑", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.Editor", "ReconcileVersionFindConflicts", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public void RefreshSnapAgent(List<SnapAgent> pSnapAgents, bool pSnapTip)
        {
            IEngineSnapEnvironment environment = this._editor as IEngineSnapEnvironment;
            environment.ClearSnapAgents();
            this._snapLayers.Clear();
            foreach (SnapAgent agent in pSnapAgents)
            {
                try
                {
                    esriGeometryHitPartType esriGeometryPartNone = esriGeometryHitPartType.esriGeometryPartNone;
                    if (agent.Vertex)
                    {
                        esriGeometryPartNone ^= esriGeometryHitPartType.esriGeometryPartVertex;
                    }
                    if (agent.Edge)
                    {
                        esriGeometryPartNone ^= esriGeometryHitPartType.esriGeometryPartBoundary;
                    }
                    if (agent.EndPoint)
                    {
                        esriGeometryPartNone ^= esriGeometryHitPartType.esriGeometryPartEndpoint;
                    }
                    if (esriGeometryPartNone != esriGeometryHitPartType.esriGeometryPartNone)
                    {
                        IEngineFeatureSnapAgent snapAgent = new EngineFeatureSnapClass
                        {
                            FeatureClass = agent.FeatureLayer.FeatureClass,
                            HitType = esriGeometryPartNone
                        };
                        environment.AddSnapAgent(snapAgent);
                        this._snapLayers.Add(agent.FeatureLayer);
                    }
                }
                catch
                {
                }
            }
            ((IEngineEditProperties2)this._editor).SnapTips = pSnapTip;
        }

        private void SetFeatureArea(IFeature pFeature)
        {
            if (pFeature != null)
            {
                try
                {
                    IGeometry shapeCopy = pFeature.ShapeCopy;
                    if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        double area = ((IArea)GISFunFactory.UnitFun.ConvertPoject(shapeCopy, this._map.SpatialReference)).Area;
                        string str = EditTask.KindCode.Substring(0, 2);
                        string name = "";
                        string str3 = "";
                        string str4 = "";
                        if (str == "01")
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "Afforest";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else if (str == "02")
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "Harvest";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                            str4 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "ZTAreaField");
                        }
                        else if (str == "06")
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "Disaster";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else if (str == "07")
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "ForestCase";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else if (str == "04")
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "Expropriation";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                            str4 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "ZTAreaField");
                        }
                        else if (str == "05")
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "Fire";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else
                        {
                            area = Math.Round(Math.Abs((double)(area / 10000.0)), 2);
                            name = "Sub";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        int index = pFeature.Fields.FindField(str3);
                        if (index > -1)
                        {
                            pFeature.set_Value(index, area);
                        }
                        string[] strArray = str4.Split(new char[] { ',' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            index = pFeature.Fields.FindField(strArray[i]);
                            if (index > -1)
                            {
                                pFeature.set_Value(index, area);
                            }
                        }
                        bool flag = this._checkOverlap;
                        this._checkOverlap = false;
                        this._setArea = false;
                        pFeature.Store();
                        this._setArea = true;
                        if (flag)
                        {
                            this._checkOverlap = true;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void SetLayerSelect()
        {
            UID uid = new UIDClass
            {
                Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}"
            };
            IEnumLayer layer = this._map.get_Layers(uid, true);
            layer.Reset();
            for (ILayer layer2 = layer.Next(); layer2 != null; layer2 = layer.Next())
            {
                ((IFeatureLayer)layer2).Selectable = false;
            }
        }

        public void StartEdit(IWorkspace pWs, IMap pMap)
        {
            try
            {
                this._map = pMap;
                this._workspace = pWs;

                ////if (pWs is IRemoteDatabaseWorkspace)
                ////{
                ////    if (pWs is IVersionedWorkspace)
                ////    {
                ////        _editor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeVersioned;
                ////    }
                ////    else
                ////    {
                ////        _editor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeNonVersioned;
                ////    }
                ////}

                this._editor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeNonVersioned;
                ////this._editor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeVersioned;
                this._editor.StartEditing(pWs, pMap);
                this.pVersion = this._workspace as IVersion;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.Editor", "StartEdit", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void StartEditOperation()
        {
            this._editor.StartOperation();
        }

        public void StopEdit()
        {
            //try
            //{
            //    IWorkspaceEdit pVersion = (IWorkspaceEdit2)this.pVersion;
            //    ///IVersionEdit4界面提供对返回有关版本和版本的信息的成员的访问。
            //    ///IVersionEdit4接口用于将源版本与目标版本进行协调，并且可选地在进程期间不获取版本锁定，如果检测到冲突，则中止协调进程，解决有利于源版本的冲突，并将冲突检测粒度设置为属性级别而不是行级别。一旦对帐，该对象提供了在开始编辑之前处理版本表示的能力，调和版本，协调版本以及用于冲突解决的共同祖先版本。
            //    IVersionEdit4 edit2 = (IVersionEdit4)pVersion;
            //    bool abortIfConflicts = false;
            //    bool childWins = false;
            //    ///string versionName = "dbo.Default";

            //    ///为了解决版本冲突导致的无法停止编辑，将编辑数据保存在数据中将版本名称改变
            //    string versionName = "SDE.DEFAULT";
            //    if (this.pVersion == null)
            //    {
            //        if (this._editor.HasEdits())
            //        {
            //            this._editor.StopEditing(true);
            //        }
            //        else
            //        {
            //            this._editor.StopEditing(false);
            //        }
            //    }
            //    else
            //    {
            //        if (this.pVersion.VersionInfo.Parent != null)
            //        {
            //            versionName = this.pVersion.VersionInfo.Parent.VersionName;
            //        }
            //        bool columnLevel = true;
            //        try
            //        {///IVersionEdit4.Reconcile4方法将当前版本与目标版本进行协调。
            //            ///Reconcile4功能将当前的源版本与指定的目标版本协调一致。目标版本必须是当前版本的祖先，否则将返回错误。传递的目标版本名称区分大小写，应采用{owner}。{version_name}的形式，例如SDE.DEFAULT。第一个布尔值参数acquireLock指定是否应该获取锁 - true获取锁，false不会获取锁。第二个布尔参数abortIfConflicts指定如果检测到任何类的冲突，协调进程是否应该中止协调。理想情况下，在用户无法交互地解决冲突的自动批处理类型环境中执行协调时，第二个布尔值仅设置为true。默认情况下，解决所有冲突有利于目标版本，但是将第三个布尔参数childWins设置为true，所有检测到的冲突都将被解析为有利于源版本。通过将Boolean参数columnLevel设置为true，只有在源和目标版本中更新了相同的属性时才会检测冲突。例如，如果源版本修改了包裹的所有者属性，并且目标版本已修改了宗地的街道地址属性，则该功能将不会发生冲突。只有当每个版本修改对象将处于冲突的相同属性时。
            //            ///如果从Reconcile4返回的布尔值为true，则检测到冲突。否则没有发现冲突。
            //            if (!edit2.Reconcile4(versionName, false, abortIfConflicts, childWins, columnLevel))
            //            {
            //                if (edit2.CanPost())
            //                {
            //                    edit2.Post(versionName);
            //                    this._editor.StopOperation(this._editor.CurrentTask.ToString());
            //                }
            //                else
            //                {
            //                    this._editor.AbortOperation();
            //                }
            //            }
            //            else
            //            {
            //                this.ReconcileVersionFindConflicts(this._workspace as IFeatureWorkspace, versionName);
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            try
            //            {
            //                edit2.Reconcile4(versionName, false, abortIfConflicts, childWins, columnLevel);
            //                if (edit2.CanPost())
            //                {
            //                    edit2.Post(versionName);
            //                    this._editor.StopOperation(this._editor.CurrentTask.ToString());
            //                }
            //                else
            //                {
            //                    this._editor.AbortOperation();
            //                }
            //            }
            //            catch (Exception)
            //            {
            //                MessageBox.Show("提交数据到服务器时有冲突，请重点保存再次提交", "数据保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //            }
            //        }
            //        if (this._editor.HasEdits())
            //        {
            //            this._editor.StopEditing(true);
            //        }
            //        else
            //        {
            //            this._editor.StopEditing(false);
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{
            //    this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.Editor", "StopEdit", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            //}
            this._editor.StopEditing(true);
        }

        public bool StopEdit2()
        {
            //try
            //{
            //    bool flag = true;
            //    IWorkspaceEdit pVersion = (IWorkspaceEdit2)this.pVersion;
            //    IVersionEdit4 edit2 = (IVersionEdit4)pVersion;
            //    bool abortIfConflicts = false;
            //    bool childWins = false;
            //    string versionName = "dbo.Default";
            //    if (this.pVersion.VersionInfo.Parent != null)
            //    {
            //        versionName = this.pVersion.VersionInfo.Parent.VersionName;
            //    }
            //    bool columnLevel = true;
            //    try
            //    {
            //        if (!edit2.Reconcile4(versionName, false, abortIfConflicts, childWins, columnLevel))
            //        {
            //            if (edit2.CanPost())
            //            {
            //                edit2.Post(versionName);
            //                this._editor.StopOperation(this._editor.CurrentTask.ToString());
            //            }
            //            else
            //            {
            //                this._editor.AbortOperation();
            //            }
            //        }
            //        else
            //        {
            //            this.ReconcileVersionFindConflicts(this._workspace as IFeatureWorkspace, versionName);
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        try
            //        {
            //            edit2.Reconcile4(versionName, false, abortIfConflicts, childWins, columnLevel);
            //            if (edit2.CanPost())
            //            {
            //                edit2.Post(versionName);
            //                this._editor.StopOperation(this._editor.CurrentTask.ToString());
            //            }
            //            else
            //            {
            //                this._editor.AbortOperation();
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            flag = false;
            //        }
            //    }
            //    if (this._editor.HasEdits())
            //    {
            //        this._editor.StopEditing(true);
            //    }
            //    else
            //    {
            //        this._editor.StopEditing(false);
            //    }
            //    return flag;
            //}
            //catch (Exception exception)
            //{
            //    this.mErrOpt.ErrorOperate(this.mSubSysName, "ShapeEdit.Editor", "StopEdit", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            //    return false;
            //}
            this._editor.StopEditing(true);
            return true;
        }
        public void StopEdit3() {
            this._editor.StopEditing(true);
        }

        public void StopEditOperation()
        {
            this._editor.StopOperation("");
        }

        public void StopEditOperation(string pOpName)
        {
            this._editor.StopOperation(pOpName);
        }

        public bool AddAttribute
        {
            set
            {
                this._addAttribute = value;
            }
        }

        public bool CanEdit
        {
            get
            {
                return this._canEdit;
            }
        }

        public bool CheckBoundary
        {
            get
            {
                return this._checkBoundary;
            }
            set
            {
                this._checkBoundary = value;
            }
        }

        public bool CheckOverlap
        {
            get
            {
                return this._checkOverlap;
            }
            set
            {
                this._checkOverlap = value;
            }
        }

        public IFeature CopiedFeature
        {
            get
            {
                return this._copiedFeature;
            }
            set
            {
                this._copiedFeature = value;
            }
        }

        public IGeometry EditShape
        {
            get
            {
                IEngineEditSketch sketch = this._editor as IEngineEditSketch;
                return sketch.Geometry;
            }
        }

        public IEngineEditor EngineEditor
        {
            get
            {
                return this._editor;
            }
        }

        public bool HasCopied
        {
            get
            {
                return this._hasCopied;
            }
            set
            {
                this._hasCopied = value;
            }
        }

        public bool IsBeingEdited
        {
            get
            {
                return (this._editor.EditState != esriEngineEditState.esriEngineStateNotEditing);
            }
        }

        public IGeometry LinageShape
        {
            get
            {
                return this._linkageShape;
            }
            set
            {
                this._linkageShape = value;
                ((IActiveView)this._map).PartialRefresh(esriViewDrawPhase.esriViewForeground, value, null);
            }
        }

        public List<ShapeEdit.LinkArgs> LinkArgs
        {
            get
            {
                return this._linkArgs;
            }
        }

        public IMap Map
        {
            get
            {
                return this._map;
            }
        }

        public IOperationStack OperationStack
        {
            get
            {
                return this._oStack;
            }
        }

        public IGeometry ReservedLinkShape
        {
            get
            {
                return this._reservedLinkShape;
            }
            set
            {
                this._reservedLinkShape = value;
            }
        }

        public bool SetArea
        {
            set
            {
                this._setArea = value;
            }
        }

        public List<IFeatureLayer> SnapLayers
        {
            get
            {
                return this._snapLayers;
            }
        }

        public IFeatureLayer TargetLayer
        {
            get
            {
                return EditTask.EditLayer;
            }
            set
            {
                IEngineEditLayers layers = this._editor as IEngineEditLayers;
                if ((value != null) && (value != null))
                {
                    IFeatureLayer layer = value;
                    layers.SetTargetLayer(layer, -1);
                }
            }
        }

        public static Editor UniqueInstance
        {
            get
            {
                return UniqueEditor;
            }
        }

        public IWorkspace Workspace
        {
            get
            {
                return this._workspace;
            }
        }

        public delegate void UpdateFeature(IFeature pFeature);
    }
}

