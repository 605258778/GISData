namespace FunFactory
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geodatabase;
    using Microsoft.VisualBasic;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using Utilities;

    public class LayerFun
    {
        private const string mClassName = "FunFactory.LayerFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal LayerFun()
        {
        }

        public bool AddDatasetLayer(IBasicMap pBasicMap, IGroupLayer pParentLayer, IDataset pDataset)
        {
            try
            {
                if ((pBasicMap == null) & (pParentLayer == null))
                {
                    return false;
                }
                if (pDataset == null)
                {
                    return false;
                }
                Collection pLayersColl = new Collection();
                if (!this.GetLayerFormDataset(pDataset, pLayersColl))
                {
                    return false;
                }
                if (pLayersColl.Count <= 0)
                {
                    return false;
                }
                IMapLayers layers = pBasicMap as IMapLayers;
                foreach (ILayer layer in pLayersColl)
                {
                    if (layer != null)
                    {
                        if (pParentLayer != null)
                        {
                            pParentLayer.Add(layer);
                        }
                        else
                        {
                            pBasicMap.AddLayer(layer);
                        }
                        if (pParentLayer == null)
                        {
                            layers.MoveLayer(layer, 0);
                        }
                        else if (pBasicMap is IMap)
                        {
                            layers.MoveLayerEx(pParentLayer, pParentLayer, layer, 0);
                        }
                    }
                }
                pLayersColl = null;
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "AddDataLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool AddDatasetToGroupLayer(IGroupLayer pGroupLayer, IDataset pDataset)
        {
            try
            {
                if (pGroupLayer == null)
                {
                    return false;
                }
                if (pDataset == null)
                {
                    return false;
                }
                return this.AddDatasetLayer(null, pGroupLayer, pDataset);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "AddDataToGroupLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool AddGraphicsLayer(IBasicMap pBasicMap, IGroupLayer pParentLayer, string sLayerName)
        {
            try
            {
                if (sLayerName == null)
                {
                    sLayerName = "";
                }
                if (pBasicMap == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sLayerName))
                {
                    sLayerName = "<新建图像图层>";
                }
                IGraphicsLayer layer = null;
                if (this.FindGraphicsLayer(pBasicMap, sLayerName) == null)
                {
                    layer = new CompositeGraphicsLayerClass();
                    ILayer layer2 = null;
                    layer2 = layer as ILayer;
                    layer2.Name = sLayerName;
                    pBasicMap.AddLayer(layer as ILayer);
                    IMapLayers layers = pBasicMap as IMapLayers;
                    if (pParentLayer == null)
                    {
                        layers.MoveLayer(layer2, 0);
                    }
                    else if (pBasicMap is IMap)
                    {
                        layers.MoveLayerEx(pParentLayer, pParentLayer, layer2, 0);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "AddGraphicsLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool AddGroupLayer(IBasicMap pBasicMap, IGroupLayer pParentLayer, string sLayerName)
        {
            try
            {
                if (sLayerName == null)
                {
                    sLayerName = "";
                }
                if (pBasicMap == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sLayerName))
                {
                    sLayerName = "新建图层组";
                }
                IGroupLayer pLayer = null;
                pLayer = new GroupLayerClass {
                    Name = sLayerName
                };
                if (pParentLayer == null)
                {
                    pBasicMap.AddLayer(pLayer);
                }
                else
                {
                    pParentLayer.Add(pLayer);
                }
                IMapLayers layers = pBasicMap as IMapLayers;
                if (pParentLayer == null)
                {
                    layers.MoveLayer(pLayer, 0);
                }
                else if (pBasicMap is IMap)
                {
                    layers.MoveLayerEx(pParentLayer, pParentLayer, pLayer, 0);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "AddGroupLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool AddGxFileLayer(IBasicMap pBasicMap, IGroupLayer pParentLayer, string sFileName)
        {
            try
            {
                if (sFileName == null)
                {
                    sFileName = "";
                }
                if (pBasicMap == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sFileName))
                {
                    OpenFileDialog dialog = new OpenFileDialog {
                        Filter = "图层文件 (*.lyr)|*.lyr",
                        Multiselect = false,
                        Title = "选择输入的图层文件"
                    };
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    sFileName = dialog.FileName;
                    dialog = null;
                }
                if (string.IsNullOrEmpty(sFileName))
                {
                    return false;
                }
                if (!File.Exists(sFileName))
                {
                    Interaction.MsgBox("地图图层加载失败，图层文件 " + sFileName + " 不存在。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                IMapDocument document = null;
                document = new MapDocumentClass();
                if (!document.get_IsMapDocument(sFileName))
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法加载。\r\n文件不是 MapDocument 文件。", MsgBoxStyle.Exclamation, "失败");
                    return false;
                }
                if (document.get_IsRestricted(sFileName))
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法加载。\r\n文件受到限制，无权使用。", MsgBoxStyle.Exclamation, "失败");
                    return false;
                }
                document.Open(sFileName, null);
                ILayer pLayer = null;
                if (document.DocumentType == esriMapDocumentType.esriMapDocumentTypeLyr)
                {
                    pLayer = document.get_Layer(0, 0);
                    if (pLayer == null)
                    {
                        Interaction.MsgBox("文件 " + sFileName + " 无法加载。\r\n图层文件或数据错误。", MsgBoxStyle.Exclamation, "加载失败");
                    }
                }
                else
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法加载。\r\n文件不是地图图层文件。", MsgBoxStyle.Exclamation, "加载失败");
                }
                document.Close();
                document = null;
                if (pLayer == null)
                {
                    return false;
                }
                if (pParentLayer == null)
                {
                    pBasicMap.AddLayer(pLayer);
                }
                else
                {
                    pParentLayer.Add(pLayer);
                }
                IMapLayers layers = pBasicMap as IMapLayers;
                if (pParentLayer == null)
                {
                    layers.MoveLayer(pLayer, 0);
                }
                else if (pBasicMap is IMap)
                {
                    layers.MoveLayerEx(pParentLayer, pParentLayer, pLayer, 0);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "AddGxFileLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool CheckLayerValid(IMap pMap, ILayer pLayer)
        {
            try
            {
                double mapScale = 0.0;
                mapScale = pMap.MapScale;
                if (!pLayer.Valid)
                {
                    return false;
                }
                if (!pLayer.Visible)
                {
                    return false;
                }
                if ((pLayer.MinimumScale > 0.0) && (pLayer.MinimumScale < mapScale))
                {
                    return false;
                }
                if ((pLayer.MaximumScale > 0.0) && (pLayer.MaximumScale > mapScale))
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "CheckLayerValid", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool DelLayerFromGroupLayer(IGroupLayer pGroupLayer, ILayer pLayer)
        {
            try
            {
                if (pGroupLayer == null)
                {
                    return false;
                }
                if (pLayer == null)
                {
                    return false;
                }
                pGroupLayer.Delete(pLayer);
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "DelLayerFromGroupLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public IFeatureLayer FindFeatureLayer(IBasicMap pBasicMap, string sLayerName, bool bRecursion)
        {
            try
            {
                if (pBasicMap != null)
                {
                    if (pBasicMap.LayerCount <= 0)
                    {
                        return null;
                    }
                    if (string.IsNullOrEmpty(sLayerName))
                    {
                        return null;
                    }
                    ILayer layer = null;
                    int index = 0;
                    for (index = 0; index <= (pBasicMap.LayerCount - 1); index++)
                    {
                        layer = pBasicMap.get_Layer(index);
                        if ((layer is IFeatureLayer) && (Strings.UCase(layer.Name) == Strings.UCase(sLayerName)))
                        {
                            return (layer as IFeatureLayer);
                        }
                        if (bRecursion && (layer is IGroupLayer))
                        {
                            layer = this.FindFeatureLayerGroupRecursion(layer as IGroupLayer, sLayerName, true);
                            if (layer != null)
                            {
                                return (layer as IFeatureLayer);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindFeatureLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        private IFeatureLayer FindFeatureLayerGroupRecursion(IGroupLayer pGroupLayer, string sLayerName, bool bRecursion)
        {
            try
            {
                if (pGroupLayer != null)
                {
                    ICompositeLayer layer = pGroupLayer as ICompositeLayer;
                    if (layer.Count <= 0)
                    {
                        return null;
                    }
                    ILayer layer2 = null;
                    int index = 0;
                    for (index = 0; index <= (layer.Count - 1); index++)
                    {
                        layer2 = layer.get_Layer(index);
                        if ((layer2 is IFeatureLayer) && (Strings.UCase(layer2.Name) == Strings.UCase(sLayerName)))
                        {
                            return (layer2 as IFeatureLayer);
                        }
                        if (bRecursion && (layer2 is IGroupLayer))
                        {
                            layer2 = this.FindFeatureLayerGroupRecursion(layer2 as IGroupLayer, sLayerName, true);
                            if (layer2 != null)
                            {
                                return (layer2 as IFeatureLayer);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindFeatureLayerGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IGraphicsLayer FindGraphicsLayer(IBasicMap pBasicMap, string sLayerName)
        {
            try
            {
                if (pBasicMap == null)
                {
                    return null;
                }
                if (pBasicMap.LayerCount <= 0)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(sLayerName))
                {
                    return null;
                }
                ICompositeGraphicsLayer basicGraphicsLayer = null;
                basicGraphicsLayer = pBasicMap.BasicGraphicsLayer as ICompositeGraphicsLayer;
                IGraphicsLayer layer2 = null;
                try
                {
                    layer2 = basicGraphicsLayer.FindLayer(sLayerName);
                }
                catch (Exception)
                {
                    return pBasicMap.BasicGraphicsLayer;
                }
                if (layer2 == null)
                {
                    return null;
                }
                return layer2;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindGraphicsLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public ILayer FindLayer(IBasicMap pBasicMap, string sLayerName, bool bRecursion)
        {
            try
            {
                if (pBasicMap != null)
                {
                    if (pBasicMap.LayerCount <= 0)
                    {
                        return null;
                    }
                    if (string.IsNullOrEmpty(sLayerName))
                    {
                        return null;
                    }
                    ILayer layer = null;
                    int index = 0;
                    for (index = 0; index <= (pBasicMap.LayerCount - 1); index++)
                    {
                        layer = pBasicMap.get_Layer(index);
                        if (Strings.UCase(layer.Name) == Strings.UCase(sLayerName))
                        {
                            return layer;
                        }
                        if (bRecursion && (layer is IGroupLayer))
                        {
                            layer = this.FindLayerGroupRecursion(layer as IGroupLayer, sLayerName, true);
                            if (layer != null)
                            {
                                return layer;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        /// <summary>
        /// 通过递归的形式查找图层。参数（IGroupLayer，sLayerName,bool 是否递归）。
        /// IGroupLayer接口提供对控制一个行为类似于单个图层的图层集合的成员的访问。IGroupLayer接口提供了管理GroupLayers内容的方法。使用ILayerinterface设置GroupLayer的属性，或使用适当的界面来调整构成图层的属性。
        /// 组层是复合层的特殊情况。 IGroupLayer的自定义层实现很少需要，但是完成后，它是importat来实现ICompositeLayeras的。
        /// </summary>
        /// <param name="pGroupLayer"></param>
        /// <param name="sLayerName"></param>
        /// <param name="bRecursion"></param>
        /// <returns></returns>
        private ILayer FindLayerGroupRecursion(IGroupLayer pGroupLayer, string sLayerName, bool bRecursion)
        {
            try
            {
                if (pGroupLayer != null)
                {
                    //ICompositeLayer接口提供对使用一个行为像一个图层的图层集合的成员的访问。ICompositeLayer是处理包含和管理其他图层属性（即GroupLayers）的图层的最通用界面。
                    ICompositeLayer layer = pGroupLayer as ICompositeLayer;
                    if (layer.Count <= 0)
                    {
                        return null;
                    }
                    ILayer layer2 = null;
                    int index = 0;
                    for (index = 0; index <= (layer.Count - 1); index++)
                    {
                        layer2 = layer.get_Layer(index);
                        if (Strings.UCase(layer2.Name) == Strings.UCase(sLayerName))
                        {
                            return layer2;
                        }
                        if (bRecursion && (layer2 is IGroupLayer))
                        {
                            layer2 = this.FindLayerGroupRecursion(layer2 as IGroupLayer, sLayerName, true);
                            if (layer2 != null)
                            {
                                return layer2;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindLayerGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public ILayer FindLayerInGroupLayer(IGroupLayer pGroupLayer, string sLayerName, bool bRecursion)
        {
            try
            {
                if (pGroupLayer == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(sLayerName))
                {
                    return null;
                }
                return this.FindLayerGroupRecursion(pGroupLayer, sLayerName, bRecursion);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindLayerInGroupLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public int FindLayerPosition(IBasicMap pBasicMap, ILayer pLayer, ref IGroupLayer pParentLayer)
        {
            try
            {
                pParentLayer = null;
                if (pBasicMap != null)
                {
                    if (pBasicMap.LayerCount <= 0)
                    {
                        return -1;
                    }
                    if (pLayer == null)
                    {
                        return -1;
                    }
                    int index = 0;
                    int num2 = -1;
                    ILayer objA = null;
                    for (index = 0; index <= (pBasicMap.LayerCount - 1); index++)
                    {
                        objA = pBasicMap.get_Layer(index);
                        if (object.Equals(objA, pLayer))
                        {
                            pParentLayer = null;
                            return index;
                        }
                        if (objA is IGroupLayer)
                        {
                            num2 = this.FindLayerPositionGroupRecursion(objA as IGroupLayer, pLayer, ref pParentLayer);
                            if (num2 >= 0)
                            {
                                return num2;
                            }
                        }
                    }
                }
                return -1;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindLayerPosition", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1;
            }
        }

        private int FindLayerPositionGroupRecursion(IGroupLayer pGroupLayer, ILayer pLayer, ref IGroupLayer pParentLayer)
        {
            try
            {
                if (pGroupLayer != null)
                {
                    ICompositeLayer layer = pGroupLayer as ICompositeLayer;
                    if (layer.Count <= 0)
                    {
                        return -1;
                    }
                    int index = 0;
                    int num2 = -1;
                    ILayer objA = null;
                    for (index = 0; index <= (layer.Count - 1); index++)
                    {
                        objA = layer.get_Layer(index);
                        if (object.Equals(objA, pLayer))
                        {
                            pParentLayer = pGroupLayer;
                            return index;
                        }
                        if (objA is IGroupLayer)
                        {
                            num2 = this.FindLayerPositionGroupRecursion(objA as IGroupLayer, pLayer, ref pParentLayer);
                            if (num2 >= 0)
                            {
                                return num2;
                            }
                        }
                    }
                }
                return -1;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "FindLayerPositionGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1;
            }
        }

        public bool GetLayerFormDataset(IDataset pDataset, Collection pLayersColl)
        {
            try
            {
                if (pDataset == null)
                {
                    return false;
                }
                if (pLayersColl == null)
                {
                    return false;
                }
                if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    IFeatureLayer item = null;
                    IFeatureClassContainer container = pDataset as IFeatureClassContainer;
                    if (container.ClassCount > 0)
                    {
                        int classIndex = 0;
                        IFeatureClass class2 = null;
                        for (classIndex = 0; classIndex <= (container.ClassCount - 1); classIndex++)
                        {
                            class2 = container.get_Class(classIndex);
                            if (class2.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                item = new FDOGraphicsLayerClass() as IFeatureLayer;
                            }
                            else if (class2.FeatureType == esriFeatureType.esriFTCoverageAnnotation)
                            {
                                item = new CoverageAnnotationLayerClass() as IFeatureLayer;
                            }
                            else if (class2.FeatureType == esriFeatureType.esriFTDimension)
                            {
                                item = new DimensionLayerClass() as IFeatureLayer;
                            }
                            else
                            {
                                item = new FeatureLayerClass();
                            }
                            item.FeatureClass = class2;
                            item.Name = pDataset.Name + " " + class2.AliasName;
                            pLayersColl.Add(item, item.Name, null, null);
                        }
                    }
                }
                else if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    IFeatureLayer layer2 = null;
                    IFeatureClass class3 = pDataset as IFeatureClass;
                    if (class3.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        layer2 = new FDOGraphicsLayerClass() as IFeatureLayer;
                    }
                    else if (class3.FeatureType == esriFeatureType.esriFTCoverageAnnotation)
                    {
                        layer2 = new CoverageAnnotationLayerClass() as IFeatureLayer;
                    }
                    else if (class3.FeatureType == esriFeatureType.esriFTDimension)
                    {
                        layer2 = new DimensionLayerClass() as IFeatureLayer;
                    }
                    else
                    {
                        layer2 = new FeatureLayerClass();
                    }
                    layer2.FeatureClass = class3;
                    layer2.Name = class3.AliasName;
                    pLayersColl.Add(layer2, layer2.Name, null, null);
                }
                else if (pDataset.Type == esriDatasetType.esriDTPlanarGraph)
                {
                    Interaction.MsgBox("暂时不支持 Planar Graph 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTGeometricNetwork)
                {
                    Interaction.MsgBox("暂时不支持 Geometric Network 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTTopology)
                {
                    Interaction.MsgBox("暂时不支持 Topology 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTText)
                {
                    Interaction.MsgBox("暂时不支持 Text Dataset 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTTable)
                {
                    Interaction.MsgBox("暂时不支持 Table Dataset 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTRelationshipClass)
                {
                    Interaction.MsgBox("暂时不支持 Relationship Class 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTRasterDataset)
                {
                    IRasterDataset rasterDataset = pDataset as IRasterDataset;
                    IRasterLayer layer3 = new RasterLayerClass();
                    layer3.CreateFromDataset(rasterDataset);
                    layer3.Name = pDataset.Name;
                    pLayersColl.Add(layer3, layer3.Name, null, null);
                }
                else if (pDataset.Type == esriDatasetType.esriDTRasterBand)
                {
                    Interaction.MsgBox("暂时不支持 Raster Band 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTTin)
                {
                    ITinLayer layer4 = new TinLayerClass {
                        Dataset = pDataset as ITin,
                        Name = pDataset.Name
                    };
                    pLayersColl.Add(layer4, layer4.Name, null, null);
                }
                else if (pDataset.Type == esriDatasetType.esriDTCadDrawing)
                {
                    Interaction.MsgBox("暂时不支持 Cad Drawing 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else if (pDataset.Type == esriDatasetType.esriDTRasterCatalog)
                {
                    Interaction.MsgBox("暂时不支持 Raster Catalog 格式的数据。", MsgBoxStyle.Information, "数据格式不支持");
                }
                else
                {
                    Interaction.MsgBox("无法识别的数据格式。", MsgBoxStyle.Information, "数据格式错误");
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "GetLayerFormDataset", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool SetLayerPosition(IBasicMap pBasicMap, ILayer pSourceLayer, ILayer pTargetLayer)
        {
            try
            {
                if (pBasicMap == null)
                {
                    return false;
                }
                if (pSourceLayer == null)
                {
                    return false;
                }
                int num = 0;
                IGroupLayer pParentLayer = null;
                num = this.FindLayerPosition(pBasicMap, pSourceLayer, ref pParentLayer);
                if (num == -1)
                {
                    return false;
                }
                int toIndex = 0;
                IGroupLayer layer2 = null;
                if (pTargetLayer == null)
                {
                    toIndex = 0;
                    layer2 = null;
                }
                else if (pTargetLayer is IGroupLayer)
                {
                    toIndex = 0;
                    layer2 = pTargetLayer as IGroupLayer;
                }
                else
                {
                    toIndex = this.FindLayerPosition(pBasicMap, pTargetLayer, ref layer2) + 1;
                }
                if (object.Equals(pParentLayer, layer2))
                {
                    if (num == toIndex)
                    {
                        return false;
                    }
                    if (num < toIndex)
                    {
                        toIndex--;
                    }
                }
                IMapLayers layers = pBasicMap as IMapLayers;
                if ((pParentLayer == null) & (layer2 == null))
                {
                    layers.MoveLayer(pSourceLayer, toIndex);
                }
                else if (pBasicMap is IMap)
                {
                    layers.MoveLayerEx(pParentLayer, layer2, pSourceLayer, toIndex);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "SetLayerPosition", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool ZoomToLayer(IActiveView pActiveView, ILayer pLayer)
        {
            try
            {
                if (pActiveView == null)
                {
                    return false;
                }
                if (pLayer == null)
                {
                    return false;
                }
                try
                {
                    pActiveView.Extent = pLayer.AreaOfInterest;
                    pActiveView.Refresh();
                }
                catch (Exception)
                {
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.LayerFun", "ZoomToLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }
    }
}

