namespace FunFactory
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using Microsoft.VisualBasic;
    using System;
    using Utilities;

    public class ElementFun
    {
        private const string mClassName = "FunFactory.ElementFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal ElementFun()
        {
        }

        public bool AddElement(IActiveView pActiveView, IElement pElement, bool bSelect, bool bPreserve)
        {
            try
            {
                if (pActiveView != null)
                {
                    if (pElement == null)
                    {
                        return false;
                    }
                    if (pActiveView is IGraphicsContainer)
                    {
                        (pActiveView as IGraphicsContainer).AddElement(pElement, 0);
                        goto Label_0031;
                    }
                }
                return false;
            Label_0031:
                if (bSelect && (pActiveView is IGraphicsContainerSelect))
                {
                    IGraphicsContainerSelect select = pActiveView as IGraphicsContainerSelect;
                    if (!bPreserve)
                    {
                        select.UnselectAllElements();
                    }
                    select.SelectElement(pElement);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "AddElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool AddElement(IGraphicsContainer pGraphicsContainer, IElement pElement, bool bSelect, bool bPreserve)
        {
            try
            {
                if (pGraphicsContainer == null)
                {
                    return false;
                }
                if (pElement == null)
                {
                    return false;
                }
                pGraphicsContainer.AddElement(pElement, 0);
                if (bSelect && (pGraphicsContainer is IGraphicsContainerSelect))
                {
                    IGraphicsContainerSelect select = pGraphicsContainer as IGraphicsContainerSelect;
                    if (!bPreserve)
                    {
                        select.UnselectAllElements();
                    }
                    select.SelectElement(pElement);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "AddElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool AddElement(IGraphicsLayer pGraphicsLayer, IElement pElement, bool bSelect, bool bPreserve)
        {
            try
            {
                if (pGraphicsLayer != null)
                {
                    if (pElement == null)
                    {
                        return false;
                    }
                    if (pGraphicsLayer is IGraphicsContainer)
                    {
                        (pGraphicsLayer as IGraphicsContainer).AddElement(pElement, 0);
                        goto Label_0031;
                    }
                }
                return false;
            Label_0031:
                if (bSelect && (pGraphicsLayer is IGraphicsContainerSelect))
                {
                    IGraphicsContainerSelect select = pGraphicsLayer as IGraphicsContainerSelect;
                    if (!bPreserve)
                    {
                        select.UnselectAllElements();
                    }
                    select.SelectElement(pElement);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "AddElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool CopyGraphicsLayerElements(IActiveView pSourceActiveView, IGraphicsLayer pTargetGLayer, bool bSelect, bool bPreserve)
        {
            try
            {
                if (pSourceActiveView == null)
                {
                    return false;
                }
                if (pTargetGLayer == null)
                {
                    return false;
                }
                if (!(pSourceActiveView is IGraphicsContainer))
                {
                    return false;
                }
                IGraphicsContainer container = null;
                container = pSourceActiveView as IGraphicsContainer;
                container.Reset();
                container.LocateElementsByEnvelope(pSourceActiveView.FullExtent).Reset();
                IElement element2 = null;
                IElement pElement = null;
                for (element2 = container.Next(); element2 != null; element2 = container.Next())
                {
                    pElement = GISFunFactory.SystemFun.CloneObejct(element2 as IClone) as IElement;
                    if (!this.AddElement(pTargetGLayer, pElement, bSelect, bPreserve))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "CopyGraphicsLayerElements", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool CopyGraphicsLayerElements(IGraphicsLayer pSourceGLayer, IGraphicsLayer pTargetGLayer, bool bSelect, bool bPreserve)
        {
            try
            {
                if (pSourceGLayer == null)
                {
                    return false;
                }
                if (pTargetGLayer == null)
                {
                    return false;
                }
                if (!(pSourceGLayer is IGraphicsContainer))
                {
                    return false;
                }
                IGraphicsContainer container = null;
                container = pSourceGLayer as IGraphicsContainer;
                container.Reset();
                IElement element = null;
                IElement pElement = null;
                for (element = container.Next(); element != null; element = container.Next())
                {
                    pElement = GISFunFactory.SystemFun.CloneObejct(element as IClone) as IElement;
                    if (!this.AddElement(pTargetGLayer, pElement, bSelect, bPreserve))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "CopyGraphicsLayerElements", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public IElement CreateBaseElementByGeometry(IGeometry pGeometry, ISymbol pSymbol)
        {
            IElement element7;
            try
            {
                IElement element;
                if (pGeometry != null)
                {
                    if (pGeometry.IsEmpty)
                    {
                        return null;
                    }
                    IGeometry geometry = null;
                    geometry = GISFunFactory.SystemFun.CloneObejct(pGeometry as IClone) as IGeometry;
                    element = null;
                    switch (geometry.GeometryType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            element = new MarkerElementClass {
                                Geometry = geometry
                            };
                            if ((pSymbol != null) && (pSymbol is IMarkerSymbol))
                            {
                                IMarkerElement element2 = element as IMarkerElement;
                                element2.Symbol = pSymbol as IMarkerSymbol;
                            }
                            goto Label_0166;

                        case esriGeometryType.esriGeometryPolyline:
                            element = new LineElementClass {
                                Geometry = geometry
                            };
                            if ((pSymbol != null) && (pSymbol is ILineSymbol))
                            {
                                ILineElement element4 = element as ILineElement;
                                element4.Symbol = pSymbol as ILineSymbol;
                            }
                            goto Label_0166;

                        case esriGeometryType.esriGeometryPolygon:
                            element = new PolygonElementClass {
                                Geometry = geometry
                            };
                            if ((pSymbol != null) && (pSymbol is IFillSymbol))
                            {
                                IFillShapeElement element5 = element as IFillShapeElement;
                                element5.Symbol = pSymbol as IFillSymbol;
                            }
                            goto Label_0166;

                        case esriGeometryType.esriGeometryEnvelope:
                            element = new RectangleElementClass {
                                Geometry = geometry
                            };
                            if ((pSymbol != null) && (pSymbol is IFillSymbol))
                            {
                                IFillShapeElement element6 = element as IFillShapeElement;
                                element6.Symbol = pSymbol as IFillSymbol;
                            }
                            goto Label_0166;

                        case esriGeometryType.esriGeometryLine:
                            element = new LineElementClass {
                                Geometry = geometry
                            };
                            if ((pSymbol != null) && (pSymbol is ILineSymbol))
                            {
                                ILineElement element3 = element as ILineElement;
                                element3.Symbol = pSymbol as ILineSymbol;
                            }
                            goto Label_0166;
                    }
                }
                return null;
            Label_0166:
                element7 = element;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "CreateBaseElementByGeometry", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                element7 = null;
            }
            return element7;
        }

        public bool DeleteAllElements(IActiveView pActiveView)
        {
            try
            {
                if ((pActiveView != null) && (pActiveView is IGraphicsContainer))
                {
                    IGraphicsContainer container = pActiveView as IGraphicsContainer;
                    try
                    {
                        container.DeleteAllElements();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "DeleteAllElements", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool DeleteAllElements(IGraphicsLayer pGraphicsLayer)
        {
            try
            {
                if ((pGraphicsLayer != null) && (pGraphicsLayer is IGraphicsContainer))
                {
                    IGraphicsContainer container = pGraphicsLayer as IGraphicsContainer;
                    try
                    {
                        container.DeleteAllElements();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "DeleteAllElements", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool DeleteElement(IActiveView pActiveView, IElement pElement)
        {
            bool flag;
            try
            {
                if (pActiveView != null)
                {
                    if (pElement == null)
                    {
                        return false;
                    }
                    if (pActiveView is IGraphicsContainer)
                    {
                        if (pElement is IElementShutdown)
                        {
                            (pElement as IElementShutdown).Shutdown();
                        }
                        IGraphicsContainer container = pActiveView as IGraphicsContainer;
                        try
                        {
                            container.DeleteElement(pElement);
                        }
                        catch (Exception)
                        {
                        }
                        goto Label_004A;
                    }
                }
                return false;
            Label_004A:
                flag = true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "DeleteElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                flag = false;
            }
            return flag;
        }

        public IElement GetHitTestElement(IActiveView pActiveView, long x, long y, bool bMapUnit)
        {
            try
            {
                if (pActiveView != null)
                {
                    IPoint point = null;
                    if (bMapUnit)
                    {
                        point = new PointClass {
                            X = x,
                            Y = y
                        };
                    }
                    else
                    {
                        point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint((int) x, (int) y);
                    }
                    double dPixelUnits = 3.0;
                    dPixelUnits = GISFunFactory.UnitFun.ConvertPixelsToMapUnits(pActiveView, dPixelUnits, true);
                    IEnumElement element = null;
                    element = (pActiveView as IGraphicsContainer).LocateElements(point, dPixelUnits);
                    if (element == null)
                    {
                        return null;
                    }
                    IElement element2 = null;
                    element.Reset();
                    for (element2 = element.Next(); element2 != null; element2 = element.Next())
                    {
                        if (!element2.Locked)
                        {
                            return element2;
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "HitTestElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public Collection GetMapFrameCollection(IPageLayout pPageLayout, string sMapClassifyName)
        {
            try
            {
                if (pPageLayout == null)
                {
                    return null;
                }
                IActiveView view = null;
                view = pPageLayout as IActiveView;
                if (view == null)
                {
                    return null;
                }
                IGraphicsContainer graphicsContainer = null;
                graphicsContainer = view.GraphicsContainer;
                if (graphicsContainer == null)
                {
                    return null;
                }
                graphicsContainer.Reset();
                Collection pMapColl = null;
                pMapColl = new Collection();
                IElement item = null;
                for (item = graphicsContainer.Next(); item != null; item = graphicsContainer.Next())
                {
                    if (item is IMapFrame)
                    {
                        IElementProperties properties = null;
                        properties = item as IElementProperties;
                        if (string.IsNullOrEmpty(sMapClassifyName))
                        {
                            pMapColl.Add(item, "", null, null);
                        }
                        else if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase(sMapClassifyName))
                        {
                            pMapColl.Add(item, "", null, null);
                        }
                    }
                    else if (item is IGroupElement)
                    {
                        this.GetMapFrameCollGroupRecursion(item as IGroupElement, sMapClassifyName, pMapColl);
                    }
                }
                if (pMapColl.Count <= 0)
                {
                    return null;
                }
                return pMapColl;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "GetMapFrameCollection", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        private void GetMapFrameCollGroupRecursion(IGroupElement pGroupElement, string sMapClassifyName, Collection pMapColl)
        {
            try
            {
                if (((pGroupElement != null) && (pGroupElement.ElementCount > 0)) && (pMapColl != null))
                {
                    IEnumElement elements = null;
                    elements = pGroupElement.Elements;
                    elements.Reset();
                    IElement item = null;
                    for (item = elements.Next(); item != null; item = elements.Next())
                    {
                        if (item is IMapFrame)
                        {
                            IElementProperties properties = null;
                            properties = item as IElementProperties;
                            if (string.IsNullOrEmpty(sMapClassifyName))
                            {
                                pMapColl.Add(item, "", null, null);
                            }
                            else if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase(sMapClassifyName))
                            {
                                pMapColl.Add(item, "", null, null);
                            }
                        }
                        else if (item is IGroupElement)
                        {
                            this.GetMapFrameCollGroupRecursion(item as IGroupElement, sMapClassifyName, pMapColl);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "GetMapFrameCollGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private IMapFrame GetMapFrameGroupRecursion(IGroupElement pGroupElement, string sMapClassifyName, string sMapName)
        {
            try
            {
                if (pGroupElement != null)
                {
                    if (pGroupElement.ElementCount <= 0)
                    {
                        return null;
                    }
                    IEnumElement elements = null;
                    elements = pGroupElement.Elements;
                    elements.Reset();
                    IMapFrame frame = null;
                    IElement element2 = null;
                    for (element2 = elements.Next(); element2 != null; element2 = elements.Next())
                    {
                        if (element2 is IMapFrame)
                        {
                            frame = element2 as IMapFrame;
                            IElementProperties properties = null;
                            properties = frame as IElementProperties;
                            if ((Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase(sMapClassifyName)) && (string.IsNullOrEmpty(sMapName) | (frame.Map.Name == sMapName)))
                            {
                                return frame;
                            }
                        }
                        else if (element2 is IGroupElement)
                        {
                            frame = this.GetMapFrameGroupRecursion(element2 as IGroupElement, sMapClassifyName, sMapName);
                            if (frame != null)
                            {
                                return frame;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "GetMapFrameGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IMapFrame GetMapFrameMain(IPageLayout pPageLayout, string sMapName)
        {
            try
            {
                if (pPageLayout != null)
                {
                    IActiveView view = null;
                    view = pPageLayout as IActiveView;
                    if (view == null)
                    {
                        return null;
                    }
                    IGraphicsContainer graphicsContainer = null;
                    graphicsContainer = view.GraphicsContainer;
                    if (graphicsContainer == null)
                    {
                        return null;
                    }
                    graphicsContainer.Reset();
                    IMapFrame frame = null;
                    IElement element = null;
                    for (element = graphicsContainer.Next(); element != null; element = graphicsContainer.Next())
                    {
                        if (element is IMapFrame)
                        {
                            IElementProperties properties = null;
                            properties = element as IElementProperties;
                            if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase("MainMap"))
                            {
                                frame = element as IMapFrame;
                                if (string.IsNullOrEmpty(sMapName) | (frame.Map.Name == sMapName))
                                {
                                    return frame;
                                }
                            }
                        }
                        else if (element is IGroupElement)
                        {
                            frame = this.GetMapFrameGroupRecursion(element as IGroupElement, "MainMap", sMapName);
                            if (frame != null)
                            {
                                return frame;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "GetMapFrameMain", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IMapFrame GetMapFrameOverview(IPageLayout pPageLayout, string sMapName)
        {
            try
            {
                if (pPageLayout != null)
                {
                    IActiveView view = null;
                    view = pPageLayout as IActiveView;
                    if (view == null)
                    {
                        return null;
                    }
                    IGraphicsContainer graphicsContainer = null;
                    graphicsContainer = view.GraphicsContainer;
                    if (graphicsContainer == null)
                    {
                        return null;
                    }
                    graphicsContainer.Reset();
                    IMapFrame frame = null;
                    IElement element = null;
                    for (element = graphicsContainer.Next(); element != null; element = graphicsContainer.Next())
                    {
                        if (element is IMapFrame)
                        {
                            IElementProperties properties = null;
                            properties = element as IElementProperties;
                            if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase("OverviewMap"))
                            {
                                frame = element as IMapFrame;
                                if (string.IsNullOrEmpty(sMapName) | (frame.Map.Name == sMapName))
                                {
                                    return frame;
                                }
                            }
                        }
                        else if (element is IGroupElement)
                        {
                            frame = this.GetMapFrameGroupRecursion(element as IGroupElement, "OverviewMap", sMapName);
                            if (frame != null)
                            {
                                return frame;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "GetMapFrameOverview", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool HitTestElement(IActiveView pActiveView, long x, long y, bool bMapUnit)
        {
            try
            {
                if (pActiveView != null)
                {
                    IPoint point = null;
                    if (bMapUnit)
                    {
                        point = new PointClass {
                            X = x,
                            Y = y
                        };
                    }
                    else
                    {
                        point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint((int) x, (int) y);
                    }
                    double dPixelUnits = 3.0;
                    dPixelUnits = GISFunFactory.UnitFun.ConvertPixelsToMapUnits(pActiveView, dPixelUnits, true);
                    IEnumElement element = null;
                    element = (pActiveView as IGraphicsContainer).LocateElements(point, dPixelUnits);
                    if (element == null)
                    {
                        return false;
                    }
                    IElement element2 = null;
                    element.Reset();
                    for (element2 = element.Next(); element2 != null; element2 = element.Next())
                    {
                        if (!element2.Locked)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "HitTestElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool HitTestSelectionElement(IActiveView pActiveView, long x, long y, bool bMapUnit)
        {
            try
            {
                if (pActiveView != null)
                {
                    IGraphicsContainerSelect select = pActiveView as IGraphicsContainerSelect;
                    if (select.ElementSelectionCount <= 0)
                    {
                        return false;
                    }
                    IPoint point = null;
                    if (bMapUnit)
                    {
                        point = new PointClass {
                            X = x,
                            Y = y
                        };
                    }
                    else
                    {
                        point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint((int) x, (int) y);
                    }
                    IEnvelope bounds = new EnvelopeClass();
                    IBoundsProperties properties = null;
                    IGeometry other = null;
                    IRelationalOperator @operator = point as IRelationalOperator;
                    IEnumElement selectedElements = null;
                    selectedElements = select.SelectedElements;
                    if (selectedElements == null)
                    {
                        return false;
                    }
                    IElement element2 = null;
                    selectedElements.Reset();
                    for (element2 = selectedElements.Next(); element2 != null; element2 = selectedElements.Next())
                    {
                        if (!element2.Locked)
                        {
                            properties = element2 as IBoundsProperties;
                            if (properties.FixedSize)
                            {
                                element2.QueryBounds(pActiveView.ScreenDisplay, bounds);
                            }
                            else
                            {
                                bounds = element2.Geometry.Envelope;
                            }
                            other = bounds;
                            if (@operator.Within(other))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "HitTestSelectionElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public void SetMapFrameClassify(IPageLayout pPageLayout)
        {
            try
            {
                if (pPageLayout != null)
                {
                    IActiveView view = null;
                    view = pPageLayout as IActiveView;
                    if (view != null)
                    {
                        IGraphicsContainer graphicsContainer = null;
                        graphicsContainer = view.GraphicsContainer;
                        if (graphicsContainer != null)
                        {
                            graphicsContainer.Reset();
                            long num = 0L;
                            IMapFrame pMapFrame = null;
                            IMapFrame pMapFrameMain = null;
                            IElement element = null;
                            element = graphicsContainer.Next();
                            while (element != null)
                            {
                                if (element is IMapFrame)
                                {
                                    num += 1L;
                                    pMapFrame = (IMapFrame) element;
                                    this.SetMapFrameClassifyProcess(pMapFrame, ref pMapFrameMain);
                                }
                                else if (element is IGroupElement)
                                {
                                    num += this.SetMapFrameClassifyGroupRecursion(element as IGroupElement, ref pMapFrame, ref pMapFrameMain);
                                }
                                element = graphicsContainer.Next();
                            }
                            if (num != 0L)
                            {
                                if (pMapFrameMain == null)
                                {
                                    IElementProperties properties = null;
                                    pMapFrameMain = pMapFrame;
                                    properties = pMapFrameMain as IElementProperties;
                                    properties.Name = pMapFrameMain.Map.Name;
                                    properties.CustomProperty = "00_MainMap";
                                }
                                else
                                {
                                    view.FocusMap = pMapFrameMain.Map;
                                    Collection mapFrameCollection = null;
                                    mapFrameCollection = this.GetMapFrameCollection(pPageLayout, "OverviewMap");
                                    if ((mapFrameCollection != null) && (mapFrameCollection.Count > 0))
                                    {
                                        int num2 = 0;
                                        IElementProperties properties2 = null;
                                        foreach (IMapFrame frame3 in mapFrameCollection)
                                        {
                                            element = frame3 as IElement;
                                            properties2 = element as IElementProperties;
                                            num2++;
                                            properties2.CustomProperty = Strings.Right("0" + num2.ToString(), 2) + "_OverviewMap";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapFrameClassify", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private long SetMapFrameClassifyGroupRecursion(IGroupElement pGroupElement, ref IMapFrame pMapFrame, ref IMapFrame pMapFrameMain)
        {
            try
            {
                if (pGroupElement == null)
                {
                    return 0L;
                }
                if (pGroupElement.ElementCount <= 0)
                {
                    return 0L;
                }
                IEnumElement elements = null;
                elements = pGroupElement.Elements;
                elements.Reset();
                long num = 0L;
                IElement element2 = null;
                for (element2 = elements.Next(); element2 != null; element2 = elements.Next())
                {
                    if (element2 is IMapFrame)
                    {
                        num += 1L;
                        pMapFrame = (IMapFrame) element2;
                        this.SetMapFrameClassifyProcess(pMapFrame, ref pMapFrameMain);
                    }
                    else if (element2 is IGroupElement)
                    {
                        num += this.SetMapFrameClassifyGroupRecursion(element2 as IGroupElement, ref pMapFrame, ref pMapFrameMain);
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapFrameClassifyGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1L;
            }
        }

        private void SetMapFrameClassifyProcess(IMapFrame pMapFrame, ref IMapFrame pMapFrameMain)
        {
            try
            {
                if (pMapFrame != null)
                {
                    IElementProperties properties = null;
                    properties = pMapFrame as IElementProperties;
                    properties.Name = pMapFrame.Map.Name;
                    if (pMapFrameMain != null)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else if (pMapFrame.Map.Name.IndexOf("鹰眼") >= 0)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else if (pMapFrame.Map.Name.IndexOf("OV") >= 0)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else if (pMapFrame.Map.Name.IndexOf("Overview") >= 0)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else if (properties.Name.IndexOf("鹰眼") >= 0)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else if (properties.Name.IndexOf("OV") >= 0)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else if (properties.Name.IndexOf("Overview") >= 0)
                    {
                        properties.CustomProperty = "00_OverviewMap";
                    }
                    else
                    {
                        pMapFrameMain = pMapFrame;
                        properties.CustomProperty = "00_MainMap";
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapFrameClassifyProcess", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void SetMapFrameMainMap(IPageLayout pPageLayout, IMap pMap, bool bClearElements)
        {
            try
            {
                if ((pPageLayout != null) && (pMap != null))
                {
                    IActiveView view = null;
                    view = pPageLayout as IActiveView;
                    if (view != null)
                    {
                        IGraphicsContainer graphicsContainer = null;
                        graphicsContainer = view.GraphicsContainer;
                        if (graphicsContainer != null)
                        {
                            graphicsContainer.Reset();
                            IMapFrame frame = null;
                            IElement element = null;
                            for (element = graphicsContainer.Next(); element != null; element = graphicsContainer.Next())
                            {
                                if (element is IMapFrame)
                                {
                                    frame = element as IMapFrame;
                                    IElementProperties properties = null;
                                    properties = frame as IElementProperties;
                                    if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase("MainMap"))
                                    {
                                        IMap pTargetMap = frame.Map;
                                        GISFunFactory.CoreFun.SyncMapObject(pMap, ref pTargetMap, bClearElements);
                                        frame.Map = pTargetMap;
                                    }
                                }
                                else if (element is IGroupElement)
                                {
                                    this.SetMapFrameMapGroupRecursion(element as IGroupElement, "MainMap", pMap, bClearElements);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapFrameMainMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void SetMapFrameMapGroupRecursion(IGroupElement pGroupElement, string sMapClassifyName, IMap pMap, bool bClearElements)
        {
            try
            {
                if (((pGroupElement != null) && (pGroupElement.ElementCount > 0)) && (pMap != null))
                {
                    IEnumElement elements = null;
                    elements = pGroupElement.Elements;
                    elements.Reset();
                    IMapFrame frame = null;
                    IElement element2 = null;
                    for (element2 = elements.Next(); element2 != null; element2 = elements.Next())
                    {
                        if (element2 is IMapFrame)
                        {
                            frame = element2 as IMapFrame;
                            IElementProperties properties = null;
                            properties = frame as IElementProperties;
                            if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase(sMapClassifyName))
                            {
                                IMap pTargetMap = frame.Map;
                                GISFunFactory.CoreFun.SyncMapObject(pMap, ref pTargetMap, bClearElements);
                                frame.Map = pTargetMap;
                            }
                        }
                        else if (element2 is IGroupElement)
                        {
                            this.SetMapFrameMapGroupRecursion(element2 as IGroupElement, sMapClassifyName, pMap, bClearElements);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapFrameMapGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void SetMapFrameOverviewMap(IPageLayout pPageLayout, IMap pMap, bool bClearElements)
        {
            try
            {
                if ((pPageLayout != null) && (pMap != null))
                {
                    IActiveView view = null;
                    view = pPageLayout as IActiveView;
                    if (view != null)
                    {
                        IGraphicsContainer graphicsContainer = null;
                        graphicsContainer = view.GraphicsContainer;
                        if (graphicsContainer != null)
                        {
                            graphicsContainer.Reset();
                            IMapFrame frame = null;
                            IElement element = null;
                            for (element = graphicsContainer.Next(); element != null; element = graphicsContainer.Next())
                            {
                                if (element is IMapFrame)
                                {
                                    frame = element as IMapFrame;
                                    IElementProperties properties = null;
                                    properties = frame as IElementProperties;
                                    if (Strings.LCase(Strings.Mid(Convert.ToString(properties.CustomProperty), 4)) == Strings.LCase("OverviewMap"))
                                    {
                                        IMap pTargetMap = frame.Map;
                                        GISFunFactory.CoreFun.SyncMapObject(pMap, ref pTargetMap, bClearElements);
                                        frame.Map = pTargetMap;
                                    }
                                }
                                else if (element is IGroupElement)
                                {
                                    this.SetMapFrameMapGroupRecursion(element as IGroupElement, "OverviewMap", pMap, bClearElements);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapFrameOverviewMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void SetMapSurroundFrameMap(IPageLayout pPageLayout, IMap pMap)
        {
            try
            {
                if ((pPageLayout != null) && (pMap != null))
                {
                    IActiveView view = null;
                    view = pPageLayout as IActiveView;
                    if (view != null)
                    {
                        IGraphicsContainer graphicsContainer = null;
                        graphicsContainer = view.GraphicsContainer;
                        if (graphicsContainer != null)
                        {
                            graphicsContainer.Reset();
                            IMapSurroundFrame frame = null;
                            IElement element = null;
                            for (element = graphicsContainer.Next(); element != null; element = graphicsContainer.Next())
                            {
                                if (element is IMapSurroundFrame)
                                {
                                    frame = element as IMapSurroundFrame;
                                    frame.MapSurround.Map = pMap;
                                }
                                else if (element is IGroupElement)
                                {
                                    this.SetMapSurroundFrameMapGroupRecursion(element as IGroupElement, pMap);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapSurroundFrameMap", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void SetMapSurroundFrameMapGroupRecursion(IGroupElement pGroupElement, IMap pMap)
        {
            try
            {
                if (((pGroupElement != null) && (pGroupElement.ElementCount > 0)) && (pMap != null))
                {
                    IEnumElement elements = null;
                    elements = pGroupElement.Elements;
                    elements.Reset();
                    IMapSurroundFrame frame = null;
                    IElement element2 = null;
                    for (element2 = elements.Next(); element2 != null; element2 = elements.Next())
                    {
                        if (element2 is IMapSurroundFrame)
                        {
                            frame = element2 as IMapSurroundFrame;
                            frame.MapSurround.Map = pMap;
                        }
                        else if (element2 is IGroupElement)
                        {
                            this.SetMapSurroundFrameMapGroupRecursion(element2 as IGroupElement, pMap);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ElementFun", "SetMapSurroundFrameMapGroupRecursion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
    }
}

