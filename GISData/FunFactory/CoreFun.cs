namespace FunFactory
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.Output;
    using Microsoft.VisualBasic;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using Utilities;

    public class CoreFun
    {
        private const string mClassName = "FunFactory.CoreFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal CoreFun()
        {
        }

        public void CheckMapFullExtent(IEnvelope pEnvelope)
        {
            try
            {
                if (pEnvelope.XMin < double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "XMin")))
                {
                    pEnvelope.XMin = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "XMin"));
                }
                if (pEnvelope.XMax > double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "XMax")))
                {
                    pEnvelope.XMax = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "XMax"));
                }
                if (pEnvelope.YMin < double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "YMin")))
                {
                    pEnvelope.YMin = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "YMin"));
                }
                if (pEnvelope.YMax > double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "YMax")))
                {
                    pEnvelope.YMax = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("FullExtent", "YMax"));
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "CheckMapFullExtent", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void CheckMapFullExtent(IEnvelope pEnvelope, IActiveView pActiveView)
        {
            try
            {
                if ((pEnvelope != null) && (pActiveView != null))
                {
                    IEnvelope fullExtent = null;
                    fullExtent = pActiveView.FullExtent;
                    if (pEnvelope.XMin < fullExtent.XMin)
                    {
                        pEnvelope.XMin = fullExtent.XMin;
                    }
                    if (pEnvelope.XMax > fullExtent.XMax)
                    {
                        pEnvelope.XMax = fullExtent.XMax;
                    }
                    if (pEnvelope.YMin < fullExtent.YMin)
                    {
                        pEnvelope.YMin = fullExtent.YMin;
                    }
                    if (pEnvelope.YMax > fullExtent.YMax)
                    {
                        pEnvelope.YMax = fullExtent.YMax;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "CheckMapFullExtent", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public bool ExportImage(IActiveView pActiveView, ref string sFileName, ref int iImageWidth, ref int iImageHeight, double dDpi, bool bOverwritePrompt)
        {
            try
            {
                if (pActiveView == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sFileName))
                {
                    sFileName = GISFunFactory.SystemFun.GetTempPath() + "~ExportImage.Jpg";
                    bOverwritePrompt = false;
                }
                if ((File.Exists(sFileName) & bOverwritePrompt) && (Interaction.MsgBox("文件 " + sFileName + " 已存在。\r\n是否要替换？", MsgBoxStyle.YesNo, "确认替换") != MsgBoxResult.Yes))
                {
                    return false;
                }
                if (File.Exists(sFileName))
                {
                    FileSystem.Kill(sFileName);
                }
                if (File.Exists(sFileName))
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法删除。\r\n共享冲突，文件正在使用。", MsgBoxStyle.Exclamation, "删除文件错误");
                    return false;
                }
                double resolution = 0.0;
                IDisplayTransformation displayTransformation = null;
                displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
                if (displayTransformation.ZoomResolution)
                {
                    displayTransformation.ZoomResolution = false;
                    resolution = displayTransformation.Resolution;
                    displayTransformation.ZoomResolution = true;
                }
                else
                {
                    resolution = displayTransformation.Resolution;
                }
                if (resolution <= 0.0)
                {
                    resolution = 96.0;
                }
                if (dDpi <= 0.0)
                {
                    dDpi = resolution;
                }
                tagRECT pixelBounds = new tagRECT();
                pixelBounds = pActiveView.ExportFrame;
                pixelBounds.bottom = (int) (double.Parse(pixelBounds.bottom.ToString()) * (dDpi / resolution));
                pixelBounds.right = (int) (double.Parse(pixelBounds.right.ToString()) * (dDpi / resolution));
                if (iImageWidth > 0)
                {
                    dDpi *= (double) (iImageWidth / pixelBounds.right);
                    pixelBounds.bottom *= iImageWidth / pixelBounds.right;
                    pixelBounds.right = iImageWidth;
                }
                if ((iImageHeight > 0) & (pixelBounds.bottom > iImageHeight))
                {
                    dDpi *= (double) (iImageHeight / pixelBounds.bottom);
                    pixelBounds.right *= iImageHeight / pixelBounds.bottom;
                    pixelBounds.bottom = iImageHeight;
                }
                IExporter exporter = null;
                exporter = new JpegExporterClass();
                if (exporter == null)
                {
                    return false;
                }
                exporter.Resolution = (short) dDpi;
                exporter.ExportFileName = sFileName;
                IEnvelope envelope = null;
                envelope = new EnvelopeClass();
                envelope.PutCoords((double) pixelBounds.left, (double) pixelBounds.bottom, (double) pixelBounds.right, (double) pixelBounds.top);
                exporter.PixelBounds = envelope;
                int hDC = 0;
                hDC = exporter.StartExporting();
                pActiveView.Output(hDC, (int) dDpi, ref pixelBounds, null, null);
                exporter.FinishExporting();
                iImageWidth = (int) envelope.Width;
                iImageHeight = (int) envelope.Height;
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "ExportImage", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public IPoint FindEnvelopeCenter(IEnvelope pElementEnv)
        {
            try
            {
                return new PointClass { 
                    X = (pElementEnv.XMin + pElementEnv.XMax) / 2.0,
                    Y = (pElementEnv.YMin + pElementEnv.YMax) / 2.0
                };
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "FindEnvelopeCenter", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool LoadMapDocument(IMapControlDefault pMapControl, string sMxDocID)
        {
            try
            {
                if (pMapControl == null)
                {
                    MessageBox.Show("地图文档加载失败，控件 MapControl 加载失败。", "错误警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (string.IsNullOrEmpty(sMxDocID.Trim()))
                {
                    MessageBox.Show("地图文档加载失败，文档ID " + sMxDocID + " 错误。", "错误警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                string path = null;
                path = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue(sMxDocID);
                if (!File.Exists(path))
                {
                    MessageBox.Show("地图文档加载失败，文档文件 " + path + " 不存在。", "错误警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (!pMapControl.CheckMxFile(path))
                {
                    MessageBox.Show("地图文档加载失败，文档文件 " + path + " 内存在错误。", "错误警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                pMapControl.LoadMxFile(path, null, null);
                IActiveView activeView = pMapControl.ActiveView;
                if (activeView.GraphicsContainer != null)
                {
                    IViewManager manager = activeView as IViewManager;
                    ISelection elementSelection = manager.ElementSelection;
                    activeView.Selection = elementSelection;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "LoadMapDocument", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool LoadMapDocument(ref IMap pMap, string sFileName, string sPassword)
        {
            try
            {
                if (sFileName == null)
                {
                    sFileName = "";
                }
                if (sPassword == null)
                {
                    sPassword = "";
                }
                if (pMap == null)
                {
                    Interaction.MsgBox("地图文档加载失败，Map 加载失败。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                IMapDocument document = null;
                document = this.OpenMapDocument(sFileName, sPassword);
                if (document == null)
                {
                    return false;
                }
                pMap = document.get_Map(0);
                document.Close();
                document = null;
                IActiveView view = pMap as IActiveView;
                if (view.GraphicsContainer != null)
                {
                    IViewManager manager = view as IViewManager;
                    ISelection elementSelection = manager.ElementSelection;
                    view.Selection = elementSelection;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "LoadMapDocument", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool LoadMapDocument(ref IPageLayout pPageLayout, string sFileName, string sPassword)
        {
            try
            {
                if (sFileName == null)
                {
                    sFileName = "";
                }
                if (sPassword == null)
                {
                    sPassword = "";
                }
                if (pPageLayout == null)
                {
                    MessageBox.Show("地图文档加载失败，PageLayout 加载失败。", "错误警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                IMapDocument document = null;
                document = this.OpenMapDocument(sFileName, sPassword);
                if (document == null)
                {
                    return false;
                }
                pPageLayout = document.PageLayout;
                document.Close();
                document = null;
                IActiveView view = pPageLayout as IActiveView;
                if (view.GraphicsContainer != null)
                {
                    IViewManager manager = view as IViewManager;
                    ISelection elementSelection = manager.ElementSelection;
                    view.Selection = elementSelection;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "LoadMapDocument", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool LoadPageLayoutTemplate(IPageLayoutControl pPageControl, string sTemplateID)
        {
            try
            {
                if (pPageControl == null)
                {
                    Interaction.MsgBox("地图模板加载失败，控件 PageLayoutControl 加载失败。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                if (string.IsNullOrEmpty(Strings.Trim(sTemplateID)))
                {
                    Interaction.MsgBox("地图模板加载失败，模板ID " + sTemplateID + " 错误。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                string configValue = null;
                configValue = UtilFactory.GetConfigOpt().GetConfigValue(sTemplateID);
                if (string.IsNullOrEmpty(configValue))
                {
                    Interaction.MsgBox("地图模板加载失败，目录 " + configValue + " 错误。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                if (Strings.Right(configValue, 1) != @"\")
                {
                    configValue = configValue + @"\";
                }
                string path = null;
                path = configValue + sTemplateID + ".mxt";
                if (!File.Exists(path))
                {
                    Interaction.MsgBox("地图模板加载失败，模板文件 " + path + " 不存在。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                if (!pPageControl.CheckMxFile(path))
                {
                    Interaction.MsgBox("地图模板加载失败，模板文件 " + path + " 内存在错误。", MsgBoxStyle.Exclamation, "错误警告");
                    return false;
                }
                pPageControl.LoadMxFile(path, null);
                IActiveView activeView = pPageControl.ActiveView;
                if (activeView.GraphicsContainer != null)
                {
                    IViewManager manager = activeView as IViewManager;
                    ISelection elementSelection = manager.ElementSelection;
                    activeView.Selection = elementSelection;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "LoadPageLayoutTemplate", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private IMapDocument OpenMapDocument(string sFileName, string sPassword)
        {
            try
            {
                if (sFileName == null)
                {
                    sFileName = "";
                }
                if (sPassword == null)
                {
                    sPassword = "";
                }
                if (string.IsNullOrEmpty(sFileName))
                {
                    OpenFileDialog dialog = new OpenFileDialog {
                        Filter = "所有支持文件|*.mxd;*.mxt;*.pmf;*.lyr|地图文档 (*.mxd)|*.mxd|地图模板 (*.mxt)|*.mxt|发布地图 (*.pmf)|*.pmf|地图图层 (*.lyr)|*.lyr",
                        Multiselect = false,
                        Title = "选择的地图文件"
                    };
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return null;
                    }
                    sFileName = dialog.FileName;
                    dialog = null;
                }
                if (string.IsNullOrEmpty(sFileName))
                {
                    return null;
                }
                if (!File.Exists(sFileName))
                {
                    Interaction.MsgBox("地图文档加载失败，文档文件 " + sFileName + " 不存在。", MsgBoxStyle.Exclamation, "错误警告");
                    return null;
                }
                IMapDocument document = null;
                document = new MapDocumentClass();
                if (!document.get_IsMapDocument(sFileName))
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法加载。\r\n文件不是 MapDocument 文件。", MsgBoxStyle.Exclamation, "失败");
                    return null;
                }
                if (document.get_IsRestricted(sFileName))
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法加载。\r\n文件受到限制，无权使用。", MsgBoxStyle.Exclamation, "失败");
                    return null;
                }
                if (document.get_IsPasswordProtected(sFileName))
                {
                    document.Open(sFileName, sPassword);
                }
                else
                {
                    document.Open(sFileName, "");
                }
                return document;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "OpenMapDocument", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool SaveMapDocument(IMxdContents pMxdContents, string sFileName, bool bOverwritePrompt, bool bUseRelativePaths, bool bCreateThumnbail, bool bSilent)
        {
            try
            {
                if (sFileName == null)
                {
                    sFileName = "";
                }
                if (pMxdContents == null)
                {
                    MessageBox.Show("地图文档保存失败，Map 内容加载失败。", "错误警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (string.IsNullOrEmpty(sFileName))
                {
                    SaveFileDialog dialog = new SaveFileDialog {
                        DefaultExt = "mxd",
                        FileName = "无标题",
                        Filter = "地图文档 (*.mxd)|*.mxd",
                        OverwritePrompt = bOverwritePrompt,
                        Title = "保存地图到文件"
                    };
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    sFileName = dialog.FileName;
                    dialog = null;
                    if (string.IsNullOrEmpty(sFileName))
                    {
                        return false;
                    }
                }
                else if (((File.Exists(sFileName) & bOverwritePrompt) && !bSilent) && (Interaction.MsgBox("文件 " + sFileName + " 已存在。\r\n是否要替换？", MsgBoxStyle.YesNo, "确认替换") != MsgBoxResult.Yes))
                {
                    return false;
                }
                if (File.Exists(sFileName))
                {
                    FileSystem.Kill(sFileName);
                }
                if (File.Exists(sFileName))
                {
                    MessageBox.Show("文件 " + sFileName + " 无法删除。\r\n共享冲突，文件正在使用。", "覆盖文件错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                IMapDocument document = null;
                document = new MapDocumentClass();
                document.New(sFileName);
                document.ReplaceContents(pMxdContents);
                document.SetActiveView(pMxdContents.ActiveView);
                if (!document.get_IsMapDocument(sFileName))
                {
                    MessageBox.Show("文件 " + sFileName + " 无法保存。\r\n文件不是 MapDocument 文件。", "保存文件错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (document.get_IsRestricted(sFileName))
                {
                    Interaction.MsgBox("文件 " + sFileName + " 无法保存。\r\n文件受到限制，无权使用。", MsgBoxStyle.Exclamation, "失败");
                    return false;
                }
                document.Save(bUseRelativePaths, bCreateThumnbail);
                document.Close();
                document = null;
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SaveMapDocument", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public void SyncMapExtent(IMap pSourceMap, IMap pTargetMap, bool bFixed)
        {
            try
            {
                if ((pSourceMap != null) && (pTargetMap != null))
                {
                    IActiveView pActiveView = null;
                    IActiveView view2 = null;
                    pActiveView = pSourceMap as IActiveView;
                    view2 = pTargetMap as IActiveView;
                    IEnvelope pEnvelope = null;
                    pEnvelope = new EnvelopeClass();
                    if (bFixed)
                    {
                        tagRECT deviceFrame = pActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
                        tagRECT grect2 = view2.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
                        long num = deviceFrame.right - deviceFrame.left;
                        long num2 = deviceFrame.bottom - deviceFrame.top;
                        long num3 = grect2.right - grect2.left;
                        long num4 = grect2.bottom - grect2.top;
                        if ((num / num2) > (num3 / num4))
                        {
                            pEnvelope.YMin = pActiveView.Extent.YMin;
                            pEnvelope.YMax = pActiveView.Extent.YMax;
                            pEnvelope.XMin = (pActiveView.Extent.XMin + ((pActiveView.Extent.XMax - pActiveView.Extent.XMin) / 2.0)) - 0.0001;
                            pEnvelope.XMax = (pActiveView.Extent.XMin + ((pActiveView.Extent.XMax - pActiveView.Extent.XMin) / 2.0)) + 0.0001;
                        }
                        else if ((num / num2) < (num3 / num4))
                        {
                            pEnvelope.XMin = pActiveView.Extent.XMin;
                            pEnvelope.XMax = pActiveView.Extent.XMax;
                            pEnvelope.YMin = (pActiveView.Extent.YMin + ((pActiveView.Extent.YMax - pActiveView.Extent.YMin) / 2.0)) - 0.0001;
                            pEnvelope.YMax = (pActiveView.Extent.YMin + ((pActiveView.Extent.YMax - pActiveView.Extent.YMin) / 2.0)) + 0.0001;
                        }
                        else
                        {
                            pEnvelope.XMin = pActiveView.Extent.XMin;
                            pEnvelope.XMax = pActiveView.Extent.XMax;
                            pEnvelope.YMin = pActiveView.Extent.YMin;
                            pEnvelope.YMax = pActiveView.Extent.YMax;
                        }
                    }
                    else
                    {
                        pEnvelope.XMin = pActiveView.Extent.XMin;
                        pEnvelope.XMax = pActiveView.Extent.XMax;
                        pEnvelope.YMin = pActiveView.Extent.YMin;
                        pEnvelope.YMax = pActiveView.Extent.YMax;
                    }
                    this.CheckMapFullExtent(pEnvelope, pActiveView);
                    view2.Extent = pEnvelope;
                    view2.Refresh();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncMapExtent", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void SyncMapLayers(IMap pSourceMap, IMap pTargetMap)
        {
            try
            {
                if ((pSourceMap != null) && (pTargetMap != null))
                {
                    IEnumLayer layers = null;
                    layers = pSourceMap.get_Layers(null, true);
                    pTargetMap.ClearLayers();
                    pTargetMap.AddLayers(layers, false);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncMapLayers", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void SyncMapObject(IMap pSourceMap, ref IMap pTargetMap, bool bClearElements)
        {
            try
            {
                if ((pSourceMap != null) && (pTargetMap != null))
                {
                    IObjectCopy copy = null;
                    copy = new ObjectCopyClass();
                    object pInObject = null;
                    pInObject = pSourceMap;
                    object obj3 = null;
                    obj3 = copy.Copy(pInObject);
                    object pOverwriteObject = null;
                    pOverwriteObject = pTargetMap;
                    copy.Overwrite(obj3, ref pOverwriteObject);
                    IActiveView view = pTargetMap as IActiveView;
                    IGraphicsContainer graphicsContainer = view.GraphicsContainer;
                    if (graphicsContainer != null)
                    {
                        IViewManager manager = view as IViewManager;
                        ISelection elementSelection = manager.ElementSelection;
                        view.Selection = elementSelection;
                    }
                    if (bClearElements)
                    {
                        graphicsContainer.DeleteAllElements();
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncMapObject", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public bool SyncMapToPageLayoutFocusMap(IMap pMap, IPageLayout pPageLayout, bool bClearElements)
        {
            try
            {
                if (pMap == null)
                {
                    return false;
                }
                if (pPageLayout == null)
                {
                    return false;
                }
                IActiveView view = pPageLayout as IActiveView;
                if (view == null)
                {
                    return false;
                }
                IMap focusMap = view.FocusMap;
                if (focusMap == null)
                {
                    return false;
                }
                this.SyncMapObject(pMap, ref focusMap, bClearElements);
                this.SyncMapExtent(pMap, focusMap, false);
                IMapFrame mapFrameMain = null;
                mapFrameMain = GISFunFactory.ElementFun.GetMapFrameMain(pPageLayout, "");
                if (mapFrameMain != null)
                {
                    GISFunFactory.ElementFun.SetMapSurroundFrameMap(pPageLayout, mapFrameMain.Map);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncMapToPageLayoutFocusMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool SyncMapToPageLayoutMainMap(IMap pMap, IPageLayout pPageLayout, bool bClearElements)
        {
            try
            {
                if (pMap == null)
                {
                    return false;
                }
                if (pPageLayout == null)
                {
                    return false;
                }
                IMapFrame mapFrameMain = null;
                mapFrameMain = GISFunFactory.ElementFun.GetMapFrameMain(pPageLayout, "");
                if (mapFrameMain == null)
                {
                    return false;
                }
                IMap pTargetMap = mapFrameMain.Map;
                if (pTargetMap == null)
                {
                    return false;
                }
                this.SyncMapObject(pMap, ref pTargetMap, bClearElements);
                this.SyncMapExtent(pMap, pTargetMap, false);
                GISFunFactory.ElementFun.SetMapSurroundFrameMap(pPageLayout, mapFrameMain.Map);
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncMapToPageLayoutMainMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool SyncPageLayoutFocusMapToMap(IPageLayout pPageLayout, ref IMap pMap, bool bClearElements)
        {
            try
            {
                if (pPageLayout == null)
                {
                    return false;
                }
                if (pMap == null)
                {
                    return false;
                }
                IActiveView view = pPageLayout as IActiveView;
                if (view == null)
                {
                    return false;
                }
                IMap focusMap = view.FocusMap;
                if (focusMap == null)
                {
                    return false;
                }
                this.SyncMapObject(focusMap, ref pMap, bClearElements);
                this.SyncMapExtent(focusMap, pMap, false);
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncPageLayoutFocusMapToMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool SyncPageLayoutMainMapToMap(IPageLayout pPageLayout, ref IMap pMap, bool bClearElements)
        {
            try
            {
                if (pPageLayout == null)
                {
                    return false;
                }
                if (pMap == null)
                {
                    return false;
                }
                IMapFrame mapFrameMain = null;
                mapFrameMain = GISFunFactory.ElementFun.GetMapFrameMain(pPageLayout, "");
                if (mapFrameMain == null)
                {
                    return false;
                }
                IMap pSourceMap = mapFrameMain.Map;
                if (pSourceMap == null)
                {
                    return false;
                }
                this.SyncMapObject(pSourceMap, ref pMap, bClearElements);
                this.SyncMapExtent(pSourceMap, pMap, false);
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncPageLayoutMainMapToMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public void SyncPageLayoutObject(IPageLayout pSourcePage, ref IPageLayout pTargetPage)
        {
            try
            {
                if ((pSourcePage != null) && (pTargetPage != null))
                {
                    IObjectCopy copy = null;
                    copy = new ObjectCopyClass();
                    object pInObject = null;
                    pInObject = pSourcePage;
                    object obj3 = null;
                    obj3 = copy.Copy(pInObject);
                    object pOverwriteObject = null;
                    pOverwriteObject = pTargetPage;
                    copy.Overwrite(obj3, ref pOverwriteObject);
                    IActiveView view = pTargetPage as IActiveView;
                    if (view.GraphicsContainer != null)
                    {
                        IViewManager manager = view as IViewManager;
                        ISelection elementSelection = manager.ElementSelection;
                        view.Selection = elementSelection;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.CoreFun", "SyncPageLayoutObject", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
    }
}

