namespace FunFactory
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using Microsoft.VisualBasic;
    using System;
    using System.Threading;
    using Utilities;

    public class FlashFun
    {
        private const string mClassName = "FunFactory.FlashFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal FlashFun()
        {
        }

        public void FlashElement(IMap pMap, IElement pElement, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                if ((pMap != null) && (pElement != null))
                {
                    if (lFlashRate <= 0L)
                    {
                        lFlashRate = 200L;
                    }
                    IActiveView view = pMap as IActiveView;
                    IScreenDisplay screenDisplay = view.ScreenDisplay;
                    screenDisplay.StartDrawing(screenDisplay.hDC, Convert.ToInt16(esriScreenCache.esriNoScreenCache));
                    if (pElement is ILineElement)
                    {
                        this.FlashLine(screenDisplay, pElement.Geometry, true, lFlashRate, bDoubleFlag);
                    }
                    else
                    {
                        IPolygon outline = new PolygonClass();
                        pElement.QueryOutline(screenDisplay, outline);
                        this.FlashPolygon(screenDisplay, outline, true, lFlashRate, bDoubleFlag);
                    }
                    screenDisplay.FinishDrawing();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashElement", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void FlashElements(IMap pMap, IElement[] pElements, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                if ((pMap != null) && (pElements != null))
                {
                    if (lFlashRate <= 0L)
                    {
                        lFlashRate = 200L;
                    }
                    long num = 0L;
                    num = Information.UBound(pElements, 1);
                    if (num > 0L)
                    {
                        IActiveView view = pMap as IActiveView;
                        IScreenDisplay screenDisplay = view.ScreenDisplay;
                        screenDisplay.StartDrawing(screenDisplay.hDC, Convert.ToInt16(esriScreenCache.esriNoScreenCache));
                        IElement element = null;
                        IPolygon outline = null;
                        long num2 = 0L;
                        for (num2 = 0L; num2 <= (num - 1L); num2 += 1L)
                        {
                            element = pElements[(int) ((IntPtr) num2)];
                            if (element != null)
                            {
                                if (element is ILineElement)
                                {
                                    this.FlashLine(screenDisplay, element.Geometry, true, lFlashRate, bDoubleFlag);
                                }
                                else
                                {
                                    outline = new PolygonClass();
                                    element.QueryOutline(screenDisplay, outline);
                                    this.FlashPolygon(screenDisplay, outline, true, lFlashRate, bDoubleFlag);
                                }
                            }
                        }
                        screenDisplay.FinishDrawing();
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashElements", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void FlashFeature(IMap pMap, IFeature pFeature, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                if ((pMap != null) && (pFeature != null))
                {
                    if (lFlashRate <= 0L)
                    {
                        lFlashRate = 200L;
                    }
                    IActiveView view = pMap as IActiveView;
                    IScreenDisplay screenDisplay = view.ScreenDisplay;
                    screenDisplay.StartDrawing(screenDisplay.hDC, Convert.ToInt16(esriScreenCache.esriNoScreenCache));
                    switch (pFeature.Shape.GeometryType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            this.FlashPoint(screenDisplay, pFeature.Shape, true, lFlashRate, bDoubleFlag);
                            break;

                        case esriGeometryType.esriGeometryPolyline:
                            this.FlashLine(screenDisplay, pFeature.Shape, true, lFlashRate, bDoubleFlag);
                            break;

                        case esriGeometryType.esriGeometryPolygon:
                            this.FlashPolygon(screenDisplay, pFeature.Shape, true, lFlashRate, bDoubleFlag);
                            break;
                    }
                    screenDisplay.FinishDrawing();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashFeature", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void FlashGeometry(IMap pMap, IGeometry pGeometry, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                if ((pMap != null) && (pGeometry != null))
                {
                    if (lFlashRate <= 0L)
                    {
                        lFlashRate = 200L;
                    }
                    IActiveView view = pMap as IActiveView;
                    IScreenDisplay screenDisplay = view.ScreenDisplay;
                    screenDisplay.StartDrawing(screenDisplay.hDC, Convert.ToInt16(esriScreenCache.esriNoScreenCache));
                    switch (pGeometry.GeometryType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            this.FlashPoint(screenDisplay, pGeometry, true, lFlashRate, bDoubleFlag);
                            break;

                        case esriGeometryType.esriGeometryPolyline:
                            this.FlashLine(screenDisplay, pGeometry, true, lFlashRate, bDoubleFlag);
                            break;

                        case esriGeometryType.esriGeometryPolygon:
                            this.FlashPolygon(screenDisplay, pGeometry, true, lFlashRate, bDoubleFlag);
                            break;
                    }
                    screenDisplay.FinishDrawing();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashGeometry", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void FlashLine(IScreenDisplay pScreenDisplay, IGeometry pGeometry, bool bFlashFlag, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                ISimpleLineSymbol symbol = new SimpleLineSymbolClass {
                    Width = 4.0
                };
                ISymbol symbol2 = symbol as ISymbol;
                symbol2.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                pScreenDisplay.SetSymbol(symbol as ISymbol);
                pScreenDisplay.DrawPolyline(pGeometry);
                if (bFlashFlag)
                {
                    Thread.Sleep((int) lFlashRate);
                    pScreenDisplay.DrawPolyline(pGeometry);
                }
                if (bDoubleFlag)
                {
                    Thread.Sleep((int) (((int) lFlashRate) / 2));
                    pScreenDisplay.DrawPolyline(pGeometry);
                    Thread.Sleep((int) (((int) lFlashRate) / 2));
                    pScreenDisplay.DrawPolyline(pGeometry);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashLine", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void FlashPoint(IScreenDisplay pScreenDisplay, IGeometry pGeometry, bool bFlashFlag, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                ISimpleMarkerSymbol symbol = new SimpleMarkerSymbolClass {
                    Style = esriSimpleMarkerStyle.esriSMSCircle
                };
                ISymbol symbol2 = symbol as ISymbol;
                symbol2.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                pScreenDisplay.SetSymbol(symbol as ISymbol);
                pScreenDisplay.DrawPoint(pGeometry);
                if (bFlashFlag)
                {
                    Thread.Sleep((int) lFlashRate);
                    pScreenDisplay.DrawPoint(pGeometry);
                }
                if (bDoubleFlag)
                {
                    Thread.Sleep((int) (((int) lFlashRate) / 2));
                    pScreenDisplay.DrawPoint(pGeometry);
                    Thread.Sleep((int) (((int) lFlashRate) / 2));
                    pScreenDisplay.DrawPoint(pGeometry);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashPoint", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void FlashPolygon(IScreenDisplay pScreenDisplay, IGeometry pGeometry, bool bFlashFlag, long lFlashRate, bool bDoubleFlag)
        {
            try
            {
                ISimpleFillSymbol symbol = new SimpleFillSymbolClass {
                    Outline = null
                };
                ISymbol symbol2 = symbol as ISymbol;
                symbol2.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                pScreenDisplay.SetSymbol(symbol as ISymbol);
                pScreenDisplay.DrawPolygon(pGeometry);
                if (bFlashFlag)
                {
                    Thread.Sleep((int) lFlashRate);
                    pScreenDisplay.DrawPolygon(pGeometry);
                }
                if (bDoubleFlag)
                {
                    Thread.Sleep((int) (((int) lFlashRate) / 2));
                    pScreenDisplay.DrawPolygon(pGeometry);
                    Thread.Sleep((int) (((int) lFlashRate) / 2));
                    pScreenDisplay.DrawPolygon(pGeometry);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FlashFun", "FlashPolygon", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
    }
}

