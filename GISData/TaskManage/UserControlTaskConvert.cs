namespace TaskManage
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using FormBase;
    using FunFactory;
    using stdole;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Utilities;

    public class UserControlTaskConvert : UserControlBase1
    {
        private CheckedListBoxControl cListBoxLayer;
        private IContainer components;
        private GroupControl groupControl1;
        private GroupControl groupControl5;
        private Label label1;
        private Label label13;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label labelInfo;
        private const string mClassName = "TaskManage.UserControlTaskConvert";
        private IDataset mDatasetHis;
        private IDataset mDatasetNow;
    
        private string mEditKind;
        private string mEditKind2;
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private IFeatureClass mFeatureClass;
        private IFeatureClass mFeatureClass2;
        private IFeatureClass mFeatureClass3;
        private IFeatureLayer mFeatureLayer;
        private IFeatureLayer mFeatureLayer2;
        private IFeatureLayer mFeatureLayer3;
        private IFeatureWorkspace mFWorkspace;
        private bool mHasHis;
        private bool mHasNow;
        private ArrayList mHisFClassList;
        private ArrayList mHisFClassList2;
        private ArrayList mHisFClassList3;
        private ArrayList mHisTableList;
        private HookHelper mHookHelper;
        private string mKindCode;
        private string mKindName;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private DataTable mTable;
        private ITable mTableHis;
        private ITable mTableNow;
        private ITable mTableNow2;
        private string mTaskID;
        private int objectid = -1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel4;
        private Panel panel6;
        private Panel panelButtons;
        private PanelControl panelControl1;
        private Panel panelCreate;
        private SimpleButton simpleButtonBackup;
        private SimpleButton simpleButtonBackup2;
        private SimpleButton simpleButtonConvertHis;
        private SimpleButton simpleButtonConvertNow;
        private SimpleButton simpleButtonCreate;
        private SimpleButton simpleButtonDelete;
        private SimpleButton simpleButtonDelete2;
        private SimpleButton simpleButtonView;
        private SimpleButton simpleButtonView2;
        private SpinEdit spinEdit1;

        public UserControlTaskConvert()
        {
            this.InitializeComponent();
        }

        private void AddLayer(string kind)
        {
            try
            {
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "GroupName2");
                IGroupLayer pglayer = GISFunFactory.LayerFun.FindLayer(this.mHookHelper.FocusMap as IBasicMap, configValue, true) as IGroupLayer;
                if (pglayer == null)
                {
                    GISFunFactory.LayerFun.AddGroupLayer(this.mHookHelper.FocusMap as IBasicMap, null, configValue);
                    pglayer = GISFunFactory.LayerFun.FindLayer(this.mHookHelper.FocusMap as IBasicMap, configValue, true) as IGroupLayer;
                }
                if (kind == "Now")
                {
                    if (this.mFeatureClass != null)
                    {
                        if (this.mFeatureLayer == null)
                        {
                            this.mFeatureLayer = new FeatureLayerClass();
                        }
                        else
                        {
                            this.mHookHelper.FocusMap.DeleteLayer(this.mFeatureLayer);
                        }
                        this.mFeatureLayer.FeatureClass = this.mFeatureClass;
                        string[] strArray = (this.mFeatureClass as IDataset).Name.Split(new char[] { '_' });
                        if (this.mEditKind == "征占用")
                        {
                            this.mFeatureLayer.Name = strArray[strArray.Length - 1] + "年" + this.mEditKind + this.mKindName + "面";
                        }
                        else
                        {
                            this.mFeatureLayer.Name = strArray[strArray.Length - 1] + "年" + this.mEditKind + this.mKindName;
                        }
                        string str2 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "FieldName");
                        this.mFeatureLayer.DisplayField = str2;
                        if (pglayer != null)
                        {
                            pglayer.Add(this.mFeatureLayer);
                        }
                        else
                        {
                            this.mHookHelper.FocusMap.AddLayer(this.mFeatureLayer);
                        }
                        this.RendererLayer(this.mFeatureLayer, 0xff, 0, 0);
                        IGeoDataset featureClass = this.mFeatureLayer.FeatureClass as IGeoDataset;
                        IGeometry extent = featureClass.Extent;
                        if (featureClass.SpatialReference != this.mHookHelper.FocusMap.SpatialReference)
                        {
                            extent.Project(this.mHookHelper.FocusMap.SpatialReference);
                        }
                        this.mHookHelper.ActiveView.FullExtent = extent.Envelope;
                        this.mHookHelper.ActiveView.Refresh();
                        if (this.mFeatureClass2 != null)
                        {
                            if (this.mFeatureLayer2 == null)
                            {
                                this.mFeatureLayer2 = new FeatureLayerClass();
                            }
                            else
                            {
                                this.mHookHelper.FocusMap.DeleteLayer(this.mFeatureLayer2);
                            }
                            this.mFeatureLayer2.FeatureClass = this.mFeatureClass2;
                            strArray = (this.mFeatureClass2 as IDataset).Name.Split(new char[] { '_' });
                            this.mFeatureLayer2.Name = strArray[strArray.Length - 1] + "年" + this.mEditKind + this.mKindName + "点";
                            str2 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "FieldName");
                            this.mFeatureLayer2.DisplayField = str2;
                            if (pglayer != null)
                            {
                                pglayer.Add(this.mFeatureLayer2);
                            }
                            else
                            {
                                this.mHookHelper.FocusMap.AddLayer(this.mFeatureLayer2);
                            }
                            this.RendererLayer(this.mFeatureLayer2, 0xff, 0, 0);
                            featureClass = this.mFeatureLayer2.FeatureClass as IGeoDataset;
                            extent = featureClass.Extent;
                            if (featureClass.SpatialReference != this.mHookHelper.FocusMap.SpatialReference)
                            {
                                extent.Project(this.mHookHelper.FocusMap.SpatialReference);
                            }
                            IEnvelope fullExtent = this.mHookHelper.ActiveView.FullExtent;
                            if (fullExtent.XMin > extent.Envelope.XMin)
                            {
                                fullExtent.XMin = extent.Envelope.XMin;
                            }
                            if (fullExtent.YMin > extent.Envelope.YMin)
                            {
                                fullExtent.YMin = extent.Envelope.YMin;
                            }
                            if (fullExtent.XMax < extent.Envelope.XMax)
                            {
                                fullExtent.XMax = extent.Envelope.XMax;
                            }
                            if (fullExtent.YMax < extent.Envelope.YMax)
                            {
                                fullExtent.YMax = extent.Envelope.YMax;
                            }
                            this.mHookHelper.ActiveView.FullExtent = fullExtent;
                            this.mHookHelper.ActiveView.Refresh();
                        }
                        if (this.mFeatureClass3 != null)
                        {
                            if (this.mFeatureLayer3 == null)
                            {
                                this.mFeatureLayer3 = new FeatureLayerClass();
                            }
                            else
                            {
                                this.mHookHelper.FocusMap.DeleteLayer(this.mFeatureLayer3);
                            }
                            this.mFeatureLayer3.FeatureClass = this.mFeatureClass3;
                            strArray = (this.mFeatureClass3 as IDataset).Name.Split(new char[] { '_' });
                            this.mFeatureLayer3.Name = strArray[strArray.Length - 1] + "年" + this.mEditKind + this.mKindName + "线";
                            if (pglayer != null)
                            {
                                pglayer.Add(this.mFeatureLayer3);
                            }
                            else
                            {
                                this.mHookHelper.FocusMap.AddLayer(this.mFeatureLayer3);
                            }
                            this.RendererLayer(this.mFeatureLayer3, 0xff, 0, 0);
                            featureClass = this.mFeatureLayer3.FeatureClass as IGeoDataset;
                            extent = featureClass.Extent;
                            if (featureClass.SpatialReference != this.mHookHelper.FocusMap.SpatialReference)
                            {
                                extent.Project(this.mHookHelper.FocusMap.SpatialReference);
                            }
                            IEnvelope envelope2 = this.mHookHelper.ActiveView.FullExtent;
                            if (envelope2.XMin > extent.Envelope.XMin)
                            {
                                envelope2.XMin = extent.Envelope.XMin;
                            }
                            if (envelope2.YMin > extent.Envelope.YMin)
                            {
                                envelope2.YMin = extent.Envelope.YMin;
                            }
                            if (envelope2.XMax < extent.Envelope.XMax)
                            {
                                envelope2.XMax = extent.Envelope.XMax;
                            }
                            if (envelope2.YMax < extent.Envelope.YMax)
                            {
                                envelope2.YMax = extent.Envelope.YMax;
                            }
                            this.mHookHelper.ActiveView.FullExtent = envelope2;
                            this.mHookHelper.ActiveView.Refresh();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.cListBoxLayer.Items.Count; i++)
                    {
                        string sname = this.cListBoxLayer.Items[i].Value.ToString();
                        string str4 = "";
                        string str5 = "";
                        if (this.mEditKind == "征占用")
                        {
                            sname = this.cListBoxLayer.Items[i].Value.ToString() + "面";
                            str4 = this.cListBoxLayer.Items[i].Value.ToString() + "点";
                            str5 = this.cListBoxLayer.Items[i].Value.ToString() + "线";
                        }
                        if (this.cListBoxLayer.Items[i].CheckState == CheckState.Checked)
                        {
                            IFeatureLayer layer = this.FindLayer(sname, pglayer);
                            IFeatureLayer layer3 = null;
                            IFeatureLayer layer4 = null;
                            if (str4 != "")
                            {
                                layer3 = this.FindLayer(str4, pglayer);
                            }
                            if (str5 != "")
                            {
                                layer4 = this.FindLayer(str5, pglayer);
                            }
                            if (layer == null)
                            {
                                layer = new FeatureLayerClass {
                                    Name = sname
                                };
                                if (str4 != "")
                                {
                                    layer3 = new FeatureLayerClass {
                                        Name = str4
                                    };
                                }
                                if (str5 != "")
                                {
                                    layer4 = new FeatureLayerClass {
                                        Name = str5
                                    };
                                }
                                layer.FeatureClass = this.mHisFClassList[i] as IFeatureClass;
                                if (layer3 != null)
                                {
                                    layer3.FeatureClass = this.mHisFClassList2[i] as IFeatureClass;
                                }
                                if (layer4 != null)
                                {
                                    layer4.FeatureClass = this.mHisFClassList3[i] as IFeatureClass;
                                }
                                if (pglayer != null)
                                {
                                    pglayer.Add(layer);
                                    if (str4 != "")
                                    {
                                        pglayer.Add(layer3);
                                    }
                                    if (str5 != "")
                                    {
                                        pglayer.Add(layer4);
                                    }
                                }
                                else
                                {
                                    this.mHookHelper.FocusMap.AddLayer(layer);
                                    if (str4 != "")
                                    {
                                        this.mHookHelper.FocusMap.AddLayer(layer3);
                                    }
                                    if (str5 != "")
                                    {
                                        this.mHookHelper.FocusMap.AddLayer(layer4);
                                    }
                                }
                            }
                            else
                            {
                                layer.FeatureClass = this.mHisFClassList[i] as IFeatureClass;
                                if (layer3 != null)
                                {
                                    layer3.FeatureClass = this.mHisFClassList2[i] as IFeatureClass;
                                }
                                if (layer4 != null)
                                {
                                    layer4.FeatureClass = this.mHisFClassList3[i] as IFeatureClass;
                                }
                            }
                            string str6 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "FieldName");
                            layer.DisplayField = str6;
                            this.RendererLayer(layer, 200, 0, 250);
                            IGeoDataset dataset2 = layer.FeatureClass as IGeoDataset;
                            IGeometry geometry2 = dataset2.Extent;
                            if (dataset2.SpatialReference != this.mHookHelper.FocusMap.SpatialReference)
                            {
                                geometry2.Project(this.mHookHelper.FocusMap.SpatialReference);
                            }
                            this.mHookHelper.ActiveView.FullExtent = geometry2.Envelope;
                            this.mHookHelper.ActiveView.Refresh();
                            if (layer3 != null)
                            {
                                layer3.DisplayField = str6;
                                this.RendererLayer(layer3, 200, 0, 250);
                            }
                            if (layer4 != null)
                            {
                                this.RendererLayer(layer4, 200, 0, 250);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private bool CheckHas(string year)
        {
            try
            {
                for (int i = 0; i < this.cListBoxLayer.Items.Count; i++)
                {
                    if (this.cListBoxLayer.Items[i].Value.ToString().Contains(year) && (year.Length == 4))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoConvertHis()
        {
            try
            {
                IFeatureClassDescription description = new FeatureClassDescriptionClass();
                IObjectClassDescription description2 = (IObjectClassDescription) description;
                IFeatureClass class2 = null;
                IEnumDataset subsets = this.mDatasetHis.Subsets;
                for (IDataset dataset2 = subsets.Next(); dataset2 != null; dataset2 = subsets.Next())
                {
                    if (dataset2.Name == ((this.mFeatureClass as IDataset).Name + "_His"))
                    {
                        class2 = dataset2 as IFeatureClass;
                        break;
                    }
                }
                if (class2 == null)
                {
                    class2 = (this.mDatasetHis as IFeatureDataset).CreateFeatureClass((this.mFeatureClass as IDataset).Name + "_His", this.mFeatureClass.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                    IWorkspaceEdit mFWorkspace = this.mFWorkspace as IWorkspaceEdit;
                    if (mFWorkspace == null)
                    {
                        return false;
                    }
                    IFeatureCursor cursor = this.mFeatureClass.Search(null, false);
                    IFeature feature = cursor.NextFeature();
                    mFWorkspace.StartEditing(false);
                    mFWorkspace.StartEditOperation();
                    while (feature != null)
                    {
                        IFeature feature2 = class2.CreateFeature();
                        IClone shape = (IClone) feature.Shape;
                        if (shape != null)
                        {
                            IPolygon polygon = (IPolygon) shape.Clone();
                            try
                            {
                                feature2.Shape = new PolygonClass();
                                feature2.Shape = polygon;
                            }
                            catch (Exception)
                            {
                                goto Label_01FC;
                            }
                            IFields fields = feature2.Fields;
                            for (int i = 0; i < fields.FieldCount; i++)
                            {
                                if (((fields.get_Field(i).Name != class2.OIDFieldName) && (fields.get_Field(i).Name != class2.LengthField.Name)) && ((fields.get_Field(i).Name != class2.AreaField.Name) && (fields.get_Field(i).Name != class2.ShapeFieldName)))
                                {
                                    feature2.set_Value(i, feature.get_Value(i));
                                }
                            }
                            try
                            {
                                feature2.Store();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    Label_01FC:
                        feature = cursor.NextFeature();
                    }
                    try
                    {
                        mFWorkspace.StopEditOperation();
                    }
                    catch (Exception)
                    {
                        mFWorkspace.StopEditOperation();
                    }
                    mFWorkspace.StopEditing(true);
                }
                try
                {
                    GC.Collect();
                    this.mHookHelper.FocusMap.ClearLayers();
                    this.mDatasetNow.Delete();
                    (class2 as IDataset).Rename((class2 as IDataset).Name.Replace("_His", ""));
                }
                catch (Exception)
                {
                }
                string[] strArray = (class2 as IDataset).Name.Split(new char[] { '.' });
                strArray = strArray[strArray.Length - 1].Split(new char[] { '_' });
                string str2 = strArray[strArray.Length - 1];
                (this.mTableNow as IDataset).Rename((this.mTableNow as IDataset).Name + "_" + str2);
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoConvertHis", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoConvertHis2()
        {
            try
            {
                IFeatureClassDescription description = new FeatureClassDescriptionClass();
                IObjectClassDescription description2 = (IObjectClassDescription) description;
                IFeatureClass class2 = null;
                IFeatureClass class3 = null;
                IFeatureClass class4 = null;
                IEnumDataset subsets = this.mDatasetHis.Subsets;
                for (IDataset dataset2 = subsets.Next(); dataset2 != null; dataset2 = subsets.Next())
                {
                    if (dataset2.Name == ((this.mFeatureClass as IDataset).Name + "_His"))
                    {
                        class2 = dataset2 as IFeatureClass;
                    }
                    if (dataset2.Name == ((this.mFeatureClass2 as IDataset).Name + "_His"))
                    {
                        class3 = dataset2 as IFeatureClass;
                    }
                    if (dataset2.Name == ((this.mFeatureClass3 as IDataset).Name + "_His"))
                    {
                        class4 = dataset2 as IFeatureClass;
                    }
                }
                if (class2 == null)
                {
                    class2 = (this.mDatasetHis as IFeatureDataset).CreateFeatureClass((this.mFeatureClass as IDataset).Name + "_His", this.mFeatureClass.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                }
                if (class3 == null)
                {
                    class3 = (this.mDatasetHis as IFeatureDataset).CreateFeatureClass((this.mFeatureClass2 as IDataset).Name + "_His", this.mFeatureClass2.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                }
                if (class4 == null)
                {
                    class4 = (this.mDatasetHis as IFeatureDataset).CreateFeatureClass((this.mFeatureClass3 as IDataset).Name + "_His", this.mFeatureClass3.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                }
                IWorkspaceEdit mFWorkspace = this.mFWorkspace as IWorkspaceEdit;
                if (mFWorkspace == null)
                {
                    return false;
                }
                IFeatureCursor cursor = this.mFeatureClass.Search(null, false);
                IFeature feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature2 = class2.CreateFeature();
                    IClone shape = (IClone) feature.Shape;
                    if (shape != null)
                    {
                        IPolygon polygon = (IPolygon) shape.Clone();
                        try
                        {
                            feature2.Shape = new PolygonClass();
                            feature2.Shape = polygon;
                        }
                        catch (Exception)
                        {
                            goto Label_0308;
                        }
                        IFields fields = feature2.Fields;
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (((fields.get_Field(i).Name != class2.OIDFieldName) && (fields.get_Field(i).Name != class2.LengthField.Name)) && ((fields.get_Field(i).Name != class2.AreaField.Name) && (fields.get_Field(i).Name != class2.ShapeFieldName)))
                            {
                                feature2.set_Value(i, feature.get_Value(i));
                            }
                        }
                        try
                        {
                            feature2.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_0308:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                try
                {
                    GC.Collect();
                    this.mHookHelper.FocusMap.ClearLayers();
                    this.mDatasetNow.Delete();
                    (class2 as IDataset).Rename((class2 as IDataset).Name.Replace("_His", ""));
                }
                catch (Exception)
                {
                }
                cursor = this.mFeatureClass2.Search(null, false);
                feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature3 = class3.CreateFeature();
                    IClone clone2 = (IClone) feature.Shape;
                    if (clone2 != null)
                    {
                        IPoint point = (IPoint) clone2.Clone();
                        try
                        {
                            feature3.Shape = new PointClass();
                            feature3.Shape = point;
                        }
                        catch (Exception)
                        {
                            goto Label_0461;
                        }
                        IFields fields2 = feature3.Fields;
                        for (int j = 0; j < fields2.FieldCount; j++)
                        {
                            if ((fields2.get_Field(j).Name != class3.OIDFieldName) && (fields2.get_Field(j).Name != class3.ShapeFieldName))
                            {
                                feature3.set_Value(j, feature.get_Value(j));
                            }
                        }
                        try
                        {
                            feature3.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_0461:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                try
                {
                    GC.Collect();
                    this.mHookHelper.FocusMap.ClearLayers();
                    (this.mFeatureClass2 as IDataset).Delete();
                    (class3 as IDataset).Rename((class3 as IDataset).Name.Replace("_His", ""));
                }
                catch (Exception)
                {
                }
                cursor = this.mFeatureClass3.Search(null, false);
                feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature4 = class4.CreateFeature();
                    IClone clone3 = (IClone) feature.Shape;
                    if (clone3 != null)
                    {
                        IPolyline polyline = (IPolyline) clone3.Clone();
                        try
                        {
                            feature4.Shape = new PolylineClass();
                            feature4.Shape = polyline;
                        }
                        catch (Exception)
                        {
                            goto Label_05E6;
                        }
                        IFields fields3 = feature4.Fields;
                        for (int k = 0; k < fields3.FieldCount; k++)
                        {
                            if (((fields3.get_Field(k).Name != class4.OIDFieldName) && (fields3.get_Field(k).Name != class4.LengthField.Name)) && (fields3.get_Field(k).Name != class4.ShapeFieldName))
                            {
                                feature4.set_Value(k, feature.get_Value(k));
                            }
                        }
                        try
                        {
                            feature4.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_05E6:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                try
                {
                    GC.Collect();
                    this.mHookHelper.FocusMap.ClearLayers();
                    (this.mFeatureClass3 as IDataset).Delete();
                    (class4 as IDataset).Rename((class4 as IDataset).Name.Replace("_His", ""));
                }
                catch (Exception)
                {
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoConvertHis2", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoConvertNow(string year)
        {
            try
            {
                UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Layer");
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset");
                string name = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "TableName");
                string str3 = "";
                if (name.Contains(","))
                {
                    name = name.Split(new char[] { ',' })[0];
                    str3 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Table").Split(new char[] { ',' })[1];
                }
                IDataset dataset = this.mFWorkspace.OpenFeatureDataset(configValue);
                if (dataset == null)
                {
                    dataset = this.mFWorkspace.CreateFeatureDataset(configValue, (this.mDatasetHis as IGeoDataset).SpatialReference);
                }
                this.mDatasetNow = dataset;
                IFeatureDataset dataset2 = dataset as IFeatureDataset;
                IFeatureClass class2 = this.mHisFClassList[this.cListBoxLayer.SelectedIndex] as IFeatureClass;
                ITable table = this.mFWorkspace.OpenTable(name + "_" + year);
                if (str3 != "")
                {
                    try
                    {
                        this.mFWorkspace.OpenTable(str3 + "_" + year);
                    }
                    catch (Exception)
                    {
                    }
                }
                string str4 = (class2 as IDataset).Name;
                (class2 as IDataset).Rename((class2 as IDataset).Name + "_His");
                IFeatureClassDescription description = new FeatureClassDescriptionClass();
                IObjectClassDescription description2 = (IObjectClassDescription) description;
                IFeatureClass class3 = dataset2.CreateFeatureClass(str4, class2.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                IWorkspaceEdit mFWorkspace = this.mFWorkspace as IWorkspaceEdit;
                if (mFWorkspace == null)
                {
                    return false;
                }
                IFeatureCursor cursor = class2.Search(null, false);
                IFeature feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature2 = class3.CreateFeature();
                    IClone shape = (IClone) feature.Shape;
                    if (shape != null)
                    {
                        IPolygon polygon = (IPolygon) shape.Clone();
                        try
                        {
                            feature2.Shape = new PolygonClass();
                            feature2.Shape = polygon;
                        }
                        catch (Exception)
                        {
                            goto Label_02FF;
                        }
                        IFields fields = feature2.Fields;
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (((fields.get_Field(i).Name != class3.OIDFieldName) && (fields.get_Field(i).Name != class3.LengthField.Name)) && ((fields.get_Field(i).Name != class3.AreaField.Name) && (fields.get_Field(i).Name != class3.ShapeFieldName)))
                            {
                                feature2.set_Value(i, feature.get_Value(i));
                            }
                        }
                        try
                        {
                            feature2.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_02FF:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                (table as IDataset).Rename(name);
                GC.Collect();
                if (class2 != null)
                {
                    (class2 as IDataset).Delete();
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoConvertNow", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoConvertNow2(string year)
        {
            try
            {
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Layer");
                string name = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset");
                string str3 = "";
                string str4 = "";
                str3 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "LayerName3");
                if ((str3 != "") && str3.Contains(","))
                {
                    str4 = str3.Split(new char[] { ',' })[1];
                    str3 = str3.Split(new char[] { ',' })[0];
                }
                IDataset dataset = this.mFWorkspace.OpenFeatureDataset(name);
                if (dataset == null)
                {
                    dataset = this.mFWorkspace.CreateFeatureDataset(name, (this.mDatasetHis as IGeoDataset).SpatialReference);
                }
                this.mDatasetNow = dataset;
                IFeatureDataset dataset2 = dataset as IFeatureDataset;
                IFeatureClass class2 = this.mHisFClassList[this.cListBoxLayer.SelectedIndex] as IFeatureClass;
                string str5 = (class2 as IDataset).Name;
                (class2 as IDataset).Rename((class2 as IDataset).Name + "_His");
                IFeatureClass class3 = this.mHisFClassList2[this.cListBoxLayer.SelectedIndex] as IFeatureClass;
                str5 = (class3 as IDataset).Name;
                (class3 as IDataset).Rename((class3 as IDataset).Name + "_His");
                IFeatureClass class4 = this.mHisFClassList3[this.cListBoxLayer.SelectedIndex] as IFeatureClass;
                str5 = (class4 as IDataset).Name;
                (class4 as IDataset).Rename((class4 as IDataset).Name + "_His");
                IFeatureClassDescription description = new FeatureClassDescriptionClass();
                IObjectClassDescription description2 = (IObjectClassDescription) description;
                IWorkspaceEdit mFWorkspace = this.mFWorkspace as IWorkspaceEdit;
                if (mFWorkspace == null)
                {
                    return false;
                }
                str5 = configValue + "_" + year;
                IFeatureClass class5 = dataset2.CreateFeatureClass(str5, class2.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                IFeatureCursor cursor = class2.Search(null, false);
                IFeature feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature2 = class5.CreateFeature();
                    IClone shape = (IClone) feature.Shape;
                    if (shape != null)
                    {
                        IPolygon polygon = (IPolygon) shape.Clone();
                        try
                        {
                            feature2.Shape = new PolygonClass();
                            feature2.Shape = polygon;
                        }
                        catch (Exception)
                        {
                            goto Label_0361;
                        }
                        IFields fields = feature2.Fields;
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (((fields.get_Field(i).Name != class5.OIDFieldName) && (fields.get_Field(i).Name != class5.LengthField.Name)) && ((fields.get_Field(i).Name != class5.AreaField.Name) && (fields.get_Field(i).Name != class5.ShapeFieldName)))
                            {
                                feature2.set_Value(i, feature.get_Value(i));
                            }
                        }
                        try
                        {
                            feature2.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_0361:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                str5 = str3 + "_" + year;
                class5 = dataset2.CreateFeatureClass(str5, class3.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                cursor = class3.Search(null, false);
                feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature3 = class5.CreateFeature();
                    IClone clone2 = (IClone) feature.Shape;
                    if (clone2 != null)
                    {
                        IPoint point = (IPoint) clone2.Clone();
                        try
                        {
                            feature3.Shape = new PointClass();
                            feature3.Shape = point;
                        }
                        catch (Exception)
                        {
                            goto Label_04A8;
                        }
                        IFields fields2 = feature3.Fields;
                        for (int j = 0; j < fields2.FieldCount; j++)
                        {
                            if ((fields2.get_Field(j).Name != class5.OIDFieldName) && (fields2.get_Field(j).Name != class5.ShapeFieldName))
                            {
                                feature3.set_Value(j, feature.get_Value(j));
                            }
                        }
                        try
                        {
                            feature3.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_04A8:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                str5 = str4 + "_" + year;
                class5 = dataset2.CreateFeatureClass(str5, class4.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                cursor = class4.Search(null, false);
                feature = cursor.NextFeature();
                mFWorkspace.StartEditing(false);
                mFWorkspace.StartEditOperation();
                while (feature != null)
                {
                    IFeature feature4 = class5.CreateFeature();
                    IClone clone3 = (IClone) feature.Shape;
                    if (clone3 != null)
                    {
                        IPolyline polyline = (IPolyline) clone3.Clone();
                        try
                        {
                            feature4.Shape = new PolylineClass();
                            feature4.Shape = polyline;
                        }
                        catch (Exception)
                        {
                            goto Label_0613;
                        }
                        IFields fields3 = feature4.Fields;
                        for (int k = 0; k < fields3.FieldCount; k++)
                        {
                            if (((fields3.get_Field(k).Name != class5.OIDFieldName) && (fields3.get_Field(k).Name != class5.LengthField.Name)) && (fields3.get_Field(k).Name != class5.ShapeFieldName))
                            {
                                feature4.set_Value(k, feature.get_Value(k));
                            }
                        }
                        try
                        {
                            feature4.Store();
                        }
                        catch (Exception)
                        {
                        }
                    }
                Label_0613:
                    feature = cursor.NextFeature();
                }
                try
                {
                    mFWorkspace.StopEditOperation();
                }
                catch (Exception)
                {
                    mFWorkspace.StopEditOperation();
                }
                mFWorkspace.StopEditing(true);
                this.mHookHelper.FocusMap.ClearLayers();
                GC.Collect();
                if (class2 != null)
                {
                    (class2 as IDataset).Delete();
                }
                if (class3 != null)
                {
                    (class3 as IDataset).Delete();
                }
                if (class4 != null)
                {
                    (class4 as IDataset).Delete();
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoConvertNow2", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoCreate(string year)
        {
            try
            {
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Layer");
                string name = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset");
                string str3 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset2");
                string str4 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "TableName");
                string str5 = "";
                string str6 = "";
                if (str4.Contains(","))
                {
                    str4 = str4.Split(new char[] { ',' })[0];
                    str5 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Table").Split(new char[] { ',' })[1];
                }
                IDataset dataset = this.mFWorkspace.OpenFeatureDataset(name);
                if (dataset == null)
                {
                    dataset = this.mFWorkspace.CreateFeatureDataset(name, (this.mDatasetHis as IGeoDataset).SpatialReference);
                }
                this.mDatasetNow = dataset;
                IFeatureDataset dataset2 = dataset as IFeatureDataset;
                IDataset dataset3 = null;
                ITable table = null;
                ITable table2 = null;
                ITable table3 = null;
                dataset3 = this.mFWorkspace.OpenFeatureDataset(str3);
                table = this.mFWorkspace.OpenTable(str4 + "_Templ");
                if (str5 != "")
                {
                    try
                    {
                        table2 = this.mFWorkspace.OpenTable(str5 + "_Templ");
                    }
                    catch (Exception)
                    {
                        table2 = this.mFWorkspace.OpenTable(str5 + "_TEMPL");
                    }
                }
                if (str6 != "")
                {
                    try
                    {
                        table3 = this.mFWorkspace.OpenTable(str6 + "_Templ");
                    }
                    catch (Exception)
                    {
                        table3 = this.mFWorkspace.OpenTable(str6 + "_TEMPL");
                    }
                }
                IFeatureClass class2 = null;
                IFeatureClass class3 = null;
                ITable table4 = null;
                IEnumDataset subsets = dataset3.Subsets;
                IDataset dataset5 = subsets.Next();
                bool flag = true;
                while (dataset5 != null)
                {
                    if (dataset5.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        class2 = dataset5 as IFeatureClass;
                        string[] strArray = dataset5.Name.Split(new char[] { '.' });
                        string str7 = strArray[strArray.Length - 1];
                        if ((str7 == (configValue + "_Templ")) || (str7 == (configValue + "_TEMPL")))
                        {
                            class3 = class2;
                            flag = false;
                            break;
                        }
                    }
                    dataset5 = subsets.Next();
                }
                if (class3 != null)
                {
                    IFeatureClassDescription description = new FeatureClassDescriptionClass();
                    IObjectClassDescription description2 = (IObjectClassDescription) description;
                    dataset2.CreateFeatureClass(configValue + "_" + year, class3.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                    UID cLSID = new UIDClass {
                        Value = "esriGeoDatabase.Object"
                    };
                    table4 = this.mFWorkspace.CreateTable(str4, table.Fields, cLSID, null, "");
                    this.mTableNow = table4;
                    if ((str5 != "") && (table2 != null))
                    {
                        this.mFWorkspace.CreateTable(str5 + "_" + year, table2.Fields, cLSID, null, "");
                    }
                    if ((str6 != "") && (table3 != null))
                    {
                        this.mFWorkspace.CreateTable(str6 + "_" + year, table3.Fields, cLSID, null, "");
                    }
                    flag = true;
                }
                return flag;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoCreate", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoCreate2(string year)
        {
            try
            {
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Layer");
                string name = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset");
                string str3 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset2");
                string str4 = "";
                string str5 = "";
                str4 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "LayerName3");
                if ((str4 != "") && str4.Contains(","))
                {
                    str5 = str4.Split(new char[] { ',' })[1];
                    str4 = str4.Split(new char[] { ',' })[0];
                }
                IDataset dataset = this.mFWorkspace.OpenFeatureDataset(name);
                if (dataset == null)
                {
                    dataset = this.mFWorkspace.CreateFeatureDataset(name, (this.mDatasetHis as IGeoDataset).SpatialReference);
                }
                this.mDatasetNow = dataset;
                IFeatureDataset dataset2 = dataset as IFeatureDataset;
                IDataset dataset3 = null;
                IFeatureClass class2 = null;
                IFeatureClass class3 = null;
                IFeatureClass class4 = null;
                dataset3 = this.mFWorkspace.OpenFeatureDataset(str3);
                try
                {
                    class2 = this.mFWorkspace.OpenFeatureClass(configValue + "_Templ");
                }
                catch (Exception)
                {
                    class2 = this.mFWorkspace.OpenFeatureClass(configValue + "_TEMPL");
                }
                if (str4 != "")
                {
                    try
                    {
                        class3 = this.mFWorkspace.OpenFeatureClass(str4 + "_Templ");
                    }
                    catch (Exception)
                    {
                        class3 = this.mFWorkspace.OpenFeatureClass(str4 + "_TEMPL");
                    }
                }
                if (str5 != "")
                {
                    try
                    {
                        class4 = this.mFWorkspace.OpenFeatureClass(str5 + "_Templ");
                    }
                    catch (Exception)
                    {
                        class4 = this.mFWorkspace.OpenFeatureClass(str5 + "_TEMPL");
                    }
                }
                bool flag = true;
                if (class2 != null)
                {
                    IFeatureClassDescription description = new FeatureClassDescriptionClass();
                    IObjectClassDescription description2 = (IObjectClassDescription) description;
                    dataset2.CreateFeatureClass(configValue + "_" + year, class2.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                    flag = true;
                }
                if (class3 != null)
                {
                    IFeatureClassDescription description3 = new FeatureClassDescriptionClass();
                    IObjectClassDescription description4 = (IObjectClassDescription) description3;
                    dataset2.CreateFeatureClass(str4 + "_" + year, class3.Fields, description4.InstanceCLSID, description4.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                    flag = true;
                }
                if (class4 != null)
                {
                    IFeatureClassDescription description5 = new FeatureClassDescriptionClass();
                    IObjectClassDescription description6 = (IObjectClassDescription) description5;
                    dataset2.CreateFeatureClass(str5 + "_" + year, class4.Fields, description6.InstanceCLSID, description6.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                    flag = true;
                }
                return flag;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoCreate2", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoDelete(IDataset pDataset)
        {
            try
            {
                if (pDataset != null)
                {
                    pDataset.Delete();
                }
                else if (this.mDatasetNow != null)
                {
                    this.mDatasetNow.Delete();
                    if (this.mTableNow != null)
                    {
                        (this.mTableNow as IDataset).Delete();
                    }
                    if (this.mTableNow2 != null)
                    {
                        (this.mTableNow2 as IDataset).Delete();
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoDelete", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DoDelete(IFeatureClass pFeatureClass)
        {
            try
            {
                IFeatureDataset featureDataset = pFeatureClass.FeatureDataset;
                IEnumRelationshipClass class2 = pFeatureClass.get_RelationshipClasses(esriRelRole.esriRelRoleAny);
                IRelationshipClass class3 = class2.Next();
                if (class3 != null)
                {
                    IObjectClass destinationClass = class3.DestinationClass;
                    IObjectClass originClass = class3.OriginClass;
                    ITable table = null;
                    ITable table2 = null;
                    if ((destinationClass as IDataset).Name == (pFeatureClass as IDataset).Name)
                    {
                        table = originClass as ITable;
                    }
                    else if ((originClass as IDataset).Name == (pFeatureClass as IDataset).Name)
                    {
                        table = destinationClass as ITable;
                    }
                    class3 = class2.Next();
                    if (class3 != null)
                    {
                        destinationClass = class3.DestinationClass;
                        originClass = class3.OriginClass;
                        if ((destinationClass as IDataset).Name == (pFeatureClass as IDataset).Name)
                        {
                            table2 = originClass as ITable;
                        }
                        else if ((originClass as IDataset).Name == (pFeatureClass as IDataset).Name)
                        {
                            table2 = destinationClass as ITable;
                        }
                    }
                    if (table != null)
                    {
                        (table as IDataset).Delete();
                    }
                    if (table2 != null)
                    {
                        (table2 as IDataset).Delete();
                    }
                }
                if (pFeatureClass != null)
                {
                    (pFeatureClass as IDataset).Delete();
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "DoDelete", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private IFeatureLayer FindLayer(string sname, IGroupLayer pglayer)
        {
            try
            {
                if (pglayer == null)
                {
                    return GISFunFactory.LayerFun.FindFeatureLayer(this.mHookHelper.FocusMap as IBasicMap, sname, true);
                }
                return (GISFunFactory.LayerFun.FindLayerInGroupLayer(pglayer, sname, true) as IFeatureLayer);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void InitialHisList()
        {
            try
            {
                IFeatureClass class2 = null;
                ITable table = null;
                ITable table2 = null;
                this.cListBoxLayer.Items.Clear();
                this.mHisFClassList = new ArrayList();
                this.mHisTableList = new ArrayList();
                this.mHasHis = false;
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Layer2");
                string str2 = "";
                string str3 = "";
                str2 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "LayerName3");
                if ((str2 != "") && str2.Contains(","))
                {
                    str3 = str2.Split(new char[] { ',' })[1];
                    str2 = str2.Split(new char[] { ',' })[0];
                    this.mHisFClassList2 = new ArrayList();
                    this.mHisFClassList3 = new ArrayList();
                }
                string name = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset2");
                string str5 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "TableName");
                IDataset dataset2 = this.mFWorkspace.OpenFeatureDataset(name);
                this.mDatasetHis = dataset2;
                IFeatureDataset dataset3 = dataset2 as IFeatureDataset;
                IEnumDataset subsets = dataset3.Subsets;
                for (IDataset dataset = subsets.Next(); dataset != null; dataset = subsets.Next())
                {
                    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        class2 = dataset as IFeatureClass;
                        if ((dataset.Name.Contains(configValue) && (dataset.Name.Length > configValue.Length)) && (!dataset.Name.Contains("_Templ") && !dataset.Name.Contains("_TEMPL")))
                        {
                            this.mHisFClassList.Add(class2);
                            string[] strArray = dataset.Name.Split(new char[] { '.' });
                            string str7 = strArray[strArray.Length - 1].Replace(configValue + "_", "");
                            if (str5.Contains(","))
                            {
                                try
                                {
                                    table = this.mFWorkspace.OpenTable(str5.Split(new char[] { ',' })[0] + "_" + str7);
                                    table2 = this.mFWorkspace.OpenTable(str5.Split(new char[] { ',' })[1] + "_" + str7);
                                }
                                catch (Exception)
                                {
                                }
                                this.mHisTableList.Add(table);
                                this.mHisTableList.Add(table2);
                            }
                            else if (str5 != "")
                            {
                                try
                                {
                                    table = this.mFWorkspace.OpenTable(str5 + "_" + str7);
                                }
                                catch (Exception)
                                {
                                }
                                this.mHisTableList.Add(table);
                            }
                            this.mHasHis = true;
                            this.cListBoxLayer.Items.Add(str7 + "年" + this.mEditKind + this.mKindName);
                        }
                        if ((((str2 != "") && dataset.Name.Contains(str2)) && ((dataset.Name.Length > str2.Length) && !dataset.Name.Contains("_Templ"))) && !dataset.Name.Contains("_TEMPL"))
                        {
                            this.mHisFClassList2.Add(class2);
                        }
                        if ((((str3 != "") && dataset.Name.Contains(str3)) && ((dataset.Name.Length > str3.Length) && !dataset.Name.Contains("_Templ"))) && !dataset.Name.Contains("_TEMPL"))
                        {
                            this.mHisFClassList3.Add(class2);
                        }
                    }
                }
                if (this.mHasHis)
                {
                    this.simpleButtonView.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                }
                else
                {
                    this.simpleButtonView.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonConvertNow.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "InitialHisList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitializeComponent()
        {
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.cListBoxLayer = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.simpleButtonConvertNow = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleButtonView = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.simpleButtonBackup = new DevExpress.XtraEditors.SimpleButton();
            this.label13 = new System.Windows.Forms.Label();
            this.simpleButtonDelete = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelInfo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.simpleButtonBackup2 = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.simpleButtonView2 = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.simpleButtonConvertHis = new DevExpress.XtraEditors.SimpleButton();
            this.label8 = new System.Windows.Forms.Label();
            this.simpleButtonDelete2 = new DevExpress.XtraEditors.SimpleButton();
            this.panelCreate = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.simpleButtonCreate = new DevExpress.XtraEditors.SimpleButton();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxLayer)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelCreate.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panel6.Controls.Add(this.groupControl5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(6, 10, 6, 0);
            this.panel6.Size = new System.Drawing.Size(380, 200);
            this.panel6.TabIndex = 17;
            // 
            // groupControl5
            // 
            this.groupControl5.AppearanceCaption.Options.UseImage = true;
            this.groupControl5.ContentImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.groupControl5.Controls.Add(this.cListBoxLayer);
            this.groupControl5.Controls.Add(this.panelButtons);
            this.groupControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl5.Location = new System.Drawing.Point(6, 10);
            this.groupControl5.LookAndFeel.SkinName = "Blue";
            this.groupControl5.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Padding = new System.Windows.Forms.Padding(6);
            this.groupControl5.Size = new System.Drawing.Size(368, 190);
            this.groupControl5.TabIndex = 3;
            this.groupControl5.TabStop = true;
            this.groupControl5.Text = "历史作业设计";
            // 
            // cListBoxLayer
            // 
            this.cListBoxLayer.CheckOnClick = true;
            this.cListBoxLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cListBoxLayer.ItemHeight = 24;
            this.cListBoxLayer.Location = new System.Drawing.Point(8, 28);
            this.cListBoxLayer.Name = "cListBoxLayer";
            this.cListBoxLayer.Size = new System.Drawing.Size(352, 122);
            this.cListBoxLayer.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panelButtons.Controls.Add(this.simpleButtonConvertNow);
            this.panelButtons.Controls.Add(this.label3);
            this.panelButtons.Controls.Add(this.simpleButtonView);
            this.panelButtons.Controls.Add(this.label2);
            this.panelButtons.Controls.Add(this.simpleButtonBackup);
            this.panelButtons.Controls.Add(this.label13);
            this.panelButtons.Controls.Add(this.simpleButtonDelete);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(8, 150);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelButtons.Size = new System.Drawing.Size(352, 32);
            this.panelButtons.TabIndex = 80;
            // 
            // simpleButtonConvertNow
            // 
            this.simpleButtonConvertNow.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonConvertNow.Location = new System.Drawing.Point(19, 6);
            this.simpleButtonConvertNow.Name = "simpleButtonConvertNow";
            this.simpleButtonConvertNow.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonConvertNow.TabIndex = 16;
            this.simpleButtonConvertNow.Text = "转为现状";
            this.simpleButtonConvertNow.Click += new System.EventHandler(this.simpleButtonConvertNow_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(97, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(7, 26);
            this.label3.TabIndex = 17;
            // 
            // simpleButtonView
            // 
            this.simpleButtonView.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonView.Location = new System.Drawing.Point(104, 6);
            this.simpleButtonView.Name = "simpleButtonView";
            this.simpleButtonView.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonView.TabIndex = 14;
            this.simpleButtonView.Text = "地图查看";
            this.simpleButtonView.ToolTip = "查看历史年度作业设计图";
            this.simpleButtonView.Click += new System.EventHandler(this.simpleButtonView_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(182, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(7, 26);
            this.label2.TabIndex = 15;
            this.label2.Visible = false;
            // 
            // simpleButtonBackup
            // 
            this.simpleButtonBackup.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonBackup.Enabled = false;
            this.simpleButtonBackup.Location = new System.Drawing.Point(189, 6);
            this.simpleButtonBackup.Name = "simpleButtonBackup";
            this.simpleButtonBackup.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonBackup.TabIndex = 1;
            this.simpleButtonBackup.Text = "备份";
            this.simpleButtonBackup.ToolTip = "备份历史年度作业设计";
            this.simpleButtonBackup.Visible = false;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Right;
            this.label13.Location = new System.Drawing.Point(267, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(7, 26);
            this.label13.TabIndex = 11;
            // 
            // simpleButtonDelete
            // 
            this.simpleButtonDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDelete.Location = new System.Drawing.Point(274, 6);
            this.simpleButtonDelete.Name = "simpleButtonDelete";
            this.simpleButtonDelete.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonDelete.TabIndex = 0;
            this.simpleButtonDelete.Text = "删除";
            this.simpleButtonDelete.ToolTip = "删除历史年度作业设计";
            this.simpleButtonDelete.Click += new System.EventHandler(this.simpleButtonDelete_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panel1.Controls.Add(this.groupControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 200);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6, 10, 6, 0);
            this.panel1.Size = new System.Drawing.Size(380, 190);
            this.panel1.TabIndex = 18;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Options.UseImage = true;
            this.groupControl1.ContentImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Controls.Add(this.panel2);
            this.groupControl1.Controls.Add(this.panelCreate);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(6, 10);
            this.groupControl1.LookAndFeel.SkinName = "Blue";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Padding = new System.Windows.Forms.Padding(5);
            this.groupControl1.Size = new System.Drawing.Size(368, 180);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.TabStop = true;
            this.groupControl1.Text = "当前年度作业设计";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelInfo);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(7, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(354, 76);
            this.panelControl1.TabIndex = 82;
            // 
            // labelInfo
            // 
            this.labelInfo.BackColor = System.Drawing.Color.White;
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo.ImageKey = "color_swatch.png";
            this.labelInfo.Location = new System.Drawing.Point(2, 2);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(350, 72);
            this.labelInfo.TabIndex = 81;
            this.labelInfo.Click += new System.EventHandler(this.labelInfo_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panel2.Controls.Add(this.simpleButtonBackup2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.simpleButtonView2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.simpleButtonConvertHis);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.simpleButtonDelete2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(7, 103);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.panel2.Size = new System.Drawing.Size(354, 38);
            this.panel2.TabIndex = 80;
            // 
            // simpleButtonBackup2
            // 
            this.simpleButtonBackup2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonBackup2.Enabled = false;
            this.simpleButtonBackup2.Location = new System.Drawing.Point(21, 6);
            this.simpleButtonBackup2.Name = "simpleButtonBackup2";
            this.simpleButtonBackup2.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonBackup2.TabIndex = 18;
            this.simpleButtonBackup2.Text = "备份";
            this.simpleButtonBackup2.ToolTip = "备份历史年度作业设计";
            this.simpleButtonBackup2.Visible = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(99, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(7, 26);
            this.label4.TabIndex = 17;
            // 
            // simpleButtonView2
            // 
            this.simpleButtonView2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonView2.Location = new System.Drawing.Point(106, 6);
            this.simpleButtonView2.Name = "simpleButtonView2";
            this.simpleButtonView2.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonView2.TabIndex = 14;
            this.simpleButtonView2.Text = "地图查看";
            this.simpleButtonView2.ToolTip = "查看当前年度作业设计图";
            this.simpleButtonView2.Click += new System.EventHandler(this.simpleButtonView2_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(184, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(7, 26);
            this.label1.TabIndex = 15;
            // 
            // simpleButtonConvertHis
            // 
            this.simpleButtonConvertHis.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonConvertHis.Location = new System.Drawing.Point(191, 6);
            this.simpleButtonConvertHis.Name = "simpleButtonConvertHis";
            this.simpleButtonConvertHis.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonConvertHis.TabIndex = 1;
            this.simpleButtonConvertHis.Text = "转为历史";
            this.simpleButtonConvertHis.ToolTip = "转为历史年度作业设计";
            this.simpleButtonConvertHis.Click += new System.EventHandler(this.simpleButtonConvertHis_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Right;
            this.label8.Location = new System.Drawing.Point(269, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(7, 26);
            this.label8.TabIndex = 20;
            // 
            // simpleButtonDelete2
            // 
            this.simpleButtonDelete2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDelete2.Location = new System.Drawing.Point(276, 6);
            this.simpleButtonDelete2.Name = "simpleButtonDelete2";
            this.simpleButtonDelete2.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonDelete2.TabIndex = 19;
            this.simpleButtonDelete2.Text = "删除";
            this.simpleButtonDelete2.ToolTip = "删除当前年度作业设计";
            this.simpleButtonDelete2.Click += new System.EventHandler(this.simpleButtonDelete2_Click);
            // 
            // panelCreate
            // 
            this.panelCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panelCreate.Controls.Add(this.label6);
            this.panelCreate.Controls.Add(this.panel4);
            this.panelCreate.Controls.Add(this.label5);
            this.panelCreate.Controls.Add(this.simpleButtonCreate);
            this.panelCreate.Controls.Add(this.label7);
            this.panelCreate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCreate.Location = new System.Drawing.Point(7, 141);
            this.panelCreate.Name = "panelCreate";
            this.panelCreate.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelCreate.Size = new System.Drawing.Size(354, 32);
            this.panelCreate.TabIndex = 83;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(181, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 26);
            this.label6.TabIndex = 21;
            this.label6.Text = " 年";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.spinEdit1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(90, 6);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.panel4.Size = new System.Drawing.Size(91, 26);
            this.panel4.TabIndex = 22;
            // 
            // spinEdit1
            // 
            this.spinEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spinEdit1.EditValue = new decimal(new int[] {
            2013,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(5, 5);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit1.Properties.Mask.EditMask = "d";
            this.spinEdit1.Properties.MaxLength = 4;
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            2099,
            0,
            0,
            0});
            this.spinEdit1.Properties.MinValue = new decimal(new int[] {
            1980,
            0,
            0,
            0});
            this.spinEdit1.Size = new System.Drawing.Size(86, 20);
            this.spinEdit1.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(269, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(7, 26);
            this.label5.TabIndex = 19;
            // 
            // simpleButtonCreate
            // 
            this.simpleButtonCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonCreate.Location = new System.Drawing.Point(276, 6);
            this.simpleButtonCreate.Name = "simpleButtonCreate";
            this.simpleButtonCreate.Size = new System.Drawing.Size(78, 26);
            this.simpleButtonCreate.TabIndex = 16;
            this.simpleButtonCreate.Text = "新增";
            this.simpleButtonCreate.ToolTip = "新增年度作业设计";
            this.simpleButtonCreate.Click += new System.EventHandler(this.simpleButtonCreate_Click);
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 26);
            this.label7.TabIndex = 23;
            this.label7.Text = "新增年度设计";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserControlTaskConvert
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Name = "UserControlTaskConvert";
            this.Size = new System.Drawing.Size(380, 400);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxLayer)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panelCreate.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private void InitialNowInfo()
        {
            try
            {
                this.mTableNow = null;
                this.mTableNow2 = null;
                this.mDatasetNow = null;
                this.mFeatureClass = null;
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Layer");
                string str2 = "";
                string str3 = "";
                str2 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "LayerName3");
                if ((str2 != "") && str2.Contains(","))
                {
                    str3 = str2.Split(new char[] { ',' })[1];
                    str2 = str2.Split(new char[] { ',' })[0];
                }
                string name = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "TableName");
                string str5 = UtilFactory.GetConfigOpt().GetConfigValue(this.mEditKind2 + "Dataset");
                string str6 = "";
                IDataset dataset = this.mFWorkspace.OpenFeatureDataset(str5);
                try
                {
                    this.mTableNow = this.mFWorkspace.OpenTable(name);
                }
                catch (Exception)
                {
                }
                IEnumDataset subsets = dataset.Subsets;
                for (IDataset dataset3 = subsets.Next(); dataset3 != null; dataset3 = subsets.Next())
                {
                    if (dataset3.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        string[] strArray = dataset3.Name.ToString().Split(new char[] { '.' });
                        string str7 = strArray[strArray.Length - 1];
                        string[] strArray2 = str7.Split(new char[] { '_' });
                        if (strArray2.Length >= 3)
                        {
                            if ((str7.Contains(configValue) && !str7.Contains("_Templ")) && (str7.Length > configValue.Length))
                            {
                                string str8 = strArray2[strArray2.Length - 1];
                                if (str7 == (configValue + "_" + str8))
                                {
                                    str6 = str8;
                                    this.mHasNow = true;
                                    this.mFeatureClass = dataset3 as IFeatureClass;
                                    this.mDatasetNow = dataset3;
                                }
                            }
                            if (((str2 != "") && str7.Contains(str2)) && (!str7.Contains("_Templ") && (str7.Length > str2.Length)))
                            {
                                string str9 = strArray2[strArray2.Length - 1];
                                if (str7 == (str2 + "_" + str9))
                                {
                                    this.mFeatureClass2 = dataset3 as IFeatureClass;
                                }
                            }
                            if (((str3 != "") && str7.Contains(str3)) && (!str7.Contains("_Templ") && (str7.Length > str3.Length)))
                            {
                                string str10 = strArray2[strArray2.Length - 1];
                                if (str7 == (str3 + "_" + str10))
                                {
                                    this.mFeatureClass3 = dataset3 as IFeatureClass;
                                }
                            }
                        }
                    }
                }
                if (this.mFeatureClass == null)
                {
                    this.labelInfo.Text = "";
                    this.mHasNow = false;
                }
                else
                {
                    this.labelInfo.Text = str6 + "年" + this.mEditKind + this.mKindName;
                    int num = this.mFeatureClass.FeatureCount(null);
                    IFeatureCursor cursor = this.mFeatureClass.Search(null, false);
                    IFeature feature = cursor.NextFeature();
                    string str11 = "";
                    int num2 = 0;
                    string str12 = "";
                    double num3 = 0.0;
                    string str13 = "MIAN_JI";
                    while (feature != null)
                    {
                        int index = feature.Fields.FindField(str13);
                        str12 = feature.get_Value(feature.Fields.FindField(this.mTaskID)).ToString();
                        if (str11 != str12)
                        {
                            num2++;
                            str11 = str12;
                        }
                        if (feature.get_Value(index).ToString().Trim() != "")
                        {
                            num3 += double.Parse(feature.get_Value(index).ToString());
                        }
                        feature = cursor.NextFeature();
                    }
                    this.labelInfo.Text = string.Concat(new object[] { this.labelInfo.Text, "\n\n                    ", num2, "个", this.mEditKind, this.mKindName });
                    this.labelInfo.Text = string.Concat(new object[] { this.labelInfo.Text, "\n                    共计", num, "个", this.mEditKind, "小班，", this.mEditKind, "面积", num3 });
                    num = this.mFeatureClass2.FeatureCount(null);
                    cursor = this.mFeatureClass2.Search(null, false);
                    feature = cursor.NextFeature();
                    num3 = 0.0;
                    while (feature != null)
                    {
                        int num5 = feature.Fields.FindField(str13);
                        str12 = feature.get_Value(feature.Fields.FindField(this.mTaskID)).ToString();
                        if (str11 != str12)
                        {
                            num2++;
                            str11 = str12;
                        }
                        if (feature.get_Value(num5).ToString().Trim() != "")
                        {
                            num3 += double.Parse(feature.get_Value(num5).ToString());
                        }
                        feature = cursor.NextFeature();
                    }
                    this.labelInfo.Text = string.Concat(new object[] { this.labelInfo.Text, "\n                    共计", num, "个", this.mEditKind, "点状数据，占地面积", num3 });
                }
                this.simpleButtonConvertHis.Enabled = this.mHasNow;
                this.simpleButtonDelete2.Enabled = this.mHasNow;
                this.simpleButtonView2.Enabled = this.mHasNow;
                this.panelCreate.Visible = !this.mHasNow;
                if (this.mHasHis && !this.mHasNow)
                {
                    this.simpleButtonConvertNow.Enabled = true;
                }
                else
                {
                    this.simpleButtonConvertNow.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "InitialNowInfo", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void InitialValue(object hook, string sEditKind)
        {
            try
            {
                this.mHookHelper = new HookHelperClass();
                if (hook != null)
                {
                    this.mHookHelper.Hook = hook;
                }
                this.mEditKind = sEditKind;
   
                if (this.mEditKind == "造林")
                {
                    this.mKindCode = "1";
                    this.mEditKind2 = "ZaoLin";
                    this.mTaskID = "Task_ID";
                    this.mKindName = "作业设计";
                }
                else if (this.mEditKind == "采伐")
                {
                    this.mKindCode = "2";
                    this.mEditKind2 = "CaiFa";
                    this.mTaskID = "Task_ID";
                    this.mKindName = "作业设计";
                }
                else if (this.mEditKind == "征占用")
                {
                    this.mKindCode = "4";
                    this.mEditKind2 = "ZZY";
                    this.mTaskID = "XMBH";
                    this.mKindName = "项目";
                }
                else
                {
                    this.mKindCode = "";
                }
                this.mFWorkspace = EditTask.EditWorkspace;
                if (this.mFWorkspace != null)
                {
                    this.InitialHisList();
                    this.InitialNowInfo();
                    if (this.mHasHis && !this.mHasNow)
                    {
                        this.simpleButtonConvertNow.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonConvertNow.Enabled = false;
                    }
                    if (!this.mHasNow)
                    {
                        this.panelCreate.Visible = true;
                        this.simpleButtonConvertHis.Enabled = false;
                        this.simpleButtonBackup2.Enabled = false;
                        this.simpleButtonView2.Enabled = false;
                    }
                    else
                    {
                        this.panelCreate.Visible = false;
                        this.simpleButtonConvertHis.Enabled = true;
                        this.simpleButtonBackup2.Enabled = true;
                        this.simpleButtonView2.Enabled = true;
                    }
                    if (this.mHasHis)
                    {
                        this.simpleButtonView.Enabled = true;
                        this.simpleButtonDelete.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonView.Enabled = false;
                        this.simpleButtonDelete.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void labelInfo_Click(object sender, EventArgs e)
        {
        }

        private void RendererLayer(IFeatureLayer featureLayer, int r, int g, int b)
        {
            try
            {
                IGeoFeatureLayer layer = (IGeoFeatureLayer) featureLayer;
                ISimpleRenderer renderer1 = (ISimpleRenderer) layer.Renderer;
                ISymbol symbol = null;
                ISimpleFillSymbol symbol2 = new SimpleFillSymbolClass();
                ISimpleLineSymbol symbol3 = new SimpleLineSymbolClass();
                new SimpleMarkerSymbolClass();
                if (featureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    IRgbColor color = new RgbColorClass {
                        NullColor = true
                    };
                    symbol2.Color = color;
                    IRgbColor color2 = new RgbColorClass {
                        Red = r,
                        Blue = b,
                        Green = g
                    };
                    symbol3.Color = color2;
                    symbol3.Width = 1.2;
                    symbol2.Outline = symbol3;
                    symbol = symbol2 as ISymbol;
                }
                else if (featureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    IRgbColor color3 = new RgbColorClass {
                        Red = r,
                        Blue = b,
                        Green = g
                    };
                    symbol3.Color = color3;
                    symbol3.Width = 1.2;
                    symbol = symbol3 as ISymbol;
                }
                else if (featureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    ISimpleMarkerSymbol symbol4 = new SimpleMarkerSymbolClass {
                        Style = esriSimpleMarkerStyle.esriSMSCross
                    };
                    IRgbColor color4 = new RgbColorClass {
                        Red = r,
                        Blue = b,
                        Green = g
                    };
                    symbol4.Color = color4;
                    symbol4.Size = 10.0;
                    symbol = symbol4 as ISymbol;
                }
                ISimpleRenderer renderer = new SimpleRendererClass {
                    Symbol = symbol
                };
                layer.Renderer = renderer as IFeatureRenderer;
                IAnnotateLayerPropertiesCollection annotationProperties = layer.AnnotationProperties;
                if (annotationProperties == null)
                {
                    annotationProperties = new AnnotateLayerPropertiesCollectionClass();
                    layer.AnnotationProperties = annotationProperties;
                }
                else
                {
                    annotationProperties.Clear();
                }
                ILineLabelPosition position = new LineLabelPositionClass {
                    Parallel = false,
                    Perpendicular = true
                };
                ILineLabelPlacementPriorities priorities = new LineLabelPlacementPrioritiesClass();
                IBasicOverposterLayerProperties properties2 = new BasicOverposterLayerPropertiesClass {
                    FeatureType = esriBasicOverposterFeatureType.esriOverposterPolyline,
                    LineLabelPlacementPriorities = priorities,
                    LineLabelPosition = position,
                    LabelWeight = esriBasicOverposterWeight.esriHighWeight
                };
                ILabelEngineLayerProperties properties3 = new LabelEngineLayerPropertiesClass {
                    BasicOverposterLayerProperties = properties2,
                    Expression = "[" + featureLayer.DisplayField + "]"
                };
                IAnnotateLayerProperties properties4 = properties3 as IAnnotateLayerProperties;
                properties4.AnnotationMinimumScale = 35000.0;
                ITextSymbol symbol5 = new TextSymbolClass {
                    Size = 12.0
                };
                IColor color5 = symbol5.Color;
                stdole.IFontDisp font = symbol5.Font;
                font.Bold = true;
                font.Name = "宋体";
                font.Size = 12M;
                symbol5.Font = font;
                IRgbColor color6 = new RgbColorClass {
                    Red = r,
                    Blue = b,
                    Green = g
                };
                color5 = color6;
                symbol5.Color = color5;
                properties3.Symbol = symbol5;
                IAnnotateLayerProperties item = properties3 as IAnnotateLayerProperties;
                annotationProperties.Add(item);
                layer.DisplayAnnotation = true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskConvert", "RendererLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonConvertHis_Click(object sender, EventArgs e)
        {
            if (this.mEditKind == "采伐")
            {
                if (this.DoConvertHis())
                {
                    this.simpleButtonConvertNow.Enabled = true;
                    this.simpleButtonConvertHis.Enabled = false;
                    this.simpleButtonView2.Enabled = false;
                    this.simpleButtonDelete2.Enabled = false;
                    this.simpleButtonBackup2.Enabled = false;
                    this.InitialHisList();
                    this.InitialNowInfo();
                    if (this.cListBoxLayer.Items.Count > 0)
                    {
                        this.simpleButtonView.Enabled = true;
                        this.simpleButtonDelete.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonView.Enabled = false;
                        this.simpleButtonDelete.Enabled = false;
                    }
                }
            }
            else if ((this.mEditKind == "征占用") && this.DoConvertHis2())
            {
                this.simpleButtonConvertNow.Enabled = true;
                this.simpleButtonConvertHis.Enabled = false;
                this.simpleButtonView2.Enabled = false;
                this.simpleButtonDelete2.Enabled = false;
                this.simpleButtonBackup2.Enabled = false;
                this.InitialHisList();
                this.InitialNowInfo();
                if (this.cListBoxLayer.Items.Count > 0)
                {
                    this.simpleButtonView.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                }
                else
                {
                    this.simpleButtonView.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                }
            }
        }

        private void simpleButtonConvertNow_Click(object sender, EventArgs e)
        {
            if (this.cListBoxLayer.Items[this.cListBoxLayer.SelectedIndex].CheckState == CheckState.Checked)
            {
                string year = this.cListBoxLayer.Items[this.cListBoxLayer.SelectedIndex].Value.ToString().Split(new char[] { '年' })[0];
                if (this.mEditKind == "采伐")
                {
                    if (this.DoConvertNow(year))
                    {
                        this.InitialHisList();
                        this.InitialNowInfo();
                        this.simpleButtonConvertNow.Enabled = false;
                        this.simpleButtonConvertHis.Enabled = true;
                        this.simpleButtonView2.Enabled = true;
                        this.simpleButtonDelete2.Enabled = true;
                        this.panelCreate.Visible = !this.mHasNow;
                        if (this.cListBoxLayer.Items.Count > 0)
                        {
                            this.simpleButtonView.Enabled = true;
                            this.simpleButtonDelete.Enabled = true;
                        }
                        else
                        {
                            this.simpleButtonView.Enabled = false;
                            this.simpleButtonDelete.Enabled = false;
                        }
                    }
                }
                else if ((this.mEditKind == "征占用") && this.DoConvertNow2(year))
                {
                    this.InitialHisList();
                    this.InitialNowInfo();
                    this.simpleButtonConvertNow.Enabled = false;
                    this.simpleButtonConvertHis.Enabled = true;
                    this.simpleButtonView2.Enabled = true;
                    this.simpleButtonDelete2.Enabled = true;
                    this.panelCreate.Visible = !this.mHasNow;
                    if (this.cListBoxLayer.Items.Count > 0)
                    {
                        this.simpleButtonView.Enabled = true;
                        this.simpleButtonDelete.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonView.Enabled = false;
                        this.simpleButtonDelete.Enabled = false;
                    }
                }
            }
        }

        private void simpleButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string year = this.spinEdit1.Value.ToString();
                if (this.CheckHas(year))
                {
                    MessageBox.Show("本年度" + this.mKindName + "图层已经存在，请重新输入年份", this.mKindName + "管理", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (this.mEditKind == "采伐")
                {
                    if (this.DoCreate(year))
                    {
                        this.simpleButtonConvertNow.Enabled = false;
                        this.AddLayer("Now");
                        this.InitialNowInfo();
                    }
                }
                else if ((this.mEditKind == "征占用") && this.DoCreate2(year))
                {
                    this.simpleButtonConvertNow.Enabled = false;
                    this.AddLayer("Now");
                    this.InitialNowInfo();
                }
            }
            catch (Exception)
            {
            }
        }

        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.cListBoxLayer.Items.Count; i++)
            {
                if ((this.cListBoxLayer.Items[i].CheckState == CheckState.Checked) && this.DoDelete(this.mHisFClassList[i] as IFeatureClass))
                {
                    this.DoDelete(this.mHisTableList[i] as IDataset);
                    this.InitialHisList();
                }
            }
        }

        private void simpleButtonDelete2_Click(object sender, EventArgs e)
        {
            if (this.DoDelete(this.mDatasetNow))
            {
                this.DoDelete(this.mTableNow as IDataset);
                this.InitialNowInfo();
            }
        }

        private void simpleButtonView_Click(object sender, EventArgs e)
        {
            this.AddLayer("His");
        }

        private void simpleButtonView2_Click(object sender, EventArgs e)
        {
            this.AddLayer("Now");
        }
    }
}

