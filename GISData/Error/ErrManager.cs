namespace TopologyCheck.Error
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    //using FunFactory;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using TopologyCheck.Properties;

    public class ErrManager
    {
        public static Dictionary<int, List<IElement>> ErrElements = new Dictionary<int, List<IElement>>();
        public static Dictionary<string, dynamic> errorDic = new Dictionary<string, dynamic>();
        public static void AddErrAreaElement(IActiveView pActiveView, IFeature pFeature, ref List<IElement> pElements)
        {
            IGeometry shapeCopy = pFeature.ShapeCopy;
            IElement element = CreatePolygonElement(pActiveView, shapeCopy);
            (pActiveView as IGraphicsContainer).AddElement(element, 0);
            pElements.Add(element);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
        }

        public static void AddErrPointElement(IActiveView pActiveView, string pPos, ISpatialReference pDataSR, int OID)
        {
            List<IElement> list = new List<IElement>();
            string[] strArray = pPos.Split(new char[] { ';' });
            IGraphicsContainer container = pActiveView as IGraphicsContainer;
            foreach (string str in strArray)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strArray2 = str.Split(new char[] { ',' });
                    double pX = double.Parse(strArray2[0]);
                    double pY = double.Parse(strArray2[1]);
                    IElement item = CreateMarkerElement(pActiveView, pX, pY, pDataSR);
                    list.Add(item);
                    container.AddElement(item, 0);
                    
                }
            }
            ErrElements.Add(OID, list);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, pActiveView.Extent);
        }

        public static void AddErrTopoElement(IActiveView pActiveView, IGeometry pGeo)
        {
            IElement element = CreateElement(pActiveView, pGeo);
            (pActiveView as IGraphicsContainer).AddElement(element, 0);
            //pElements.Add(element);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
        }
        public static void AddErrTopoElement(IActiveView pActiveView, IGeometry pGeo,int OID)
        {
            List<IElement> list = new List<IElement>();
            IElement element = CreateElement(pActiveView, pGeo);
            list.Add(element);
            (pActiveView as IGraphicsContainer).AddElement(element, 0);
            ErrElements.Add(OID,list);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
        }
        public static void AddErrTopoElement(IActiveView pActiveView, object errs, IFeature pFeature)
        {
            IList<IGeometry> list = (IList<IGeometry>) errs;
            if ((list != null) && (list.Count >= 1))
            {
                IGraphicsContainer container = pActiveView as IGraphicsContainer;
                List<IElement> list2 = null;
                if (ErrElements.ContainsKey(pFeature.OID))
                {
                    list2 = ErrElements[pFeature.OID];
                    foreach (IElement element in list2)
                    {
                        container.DeleteElement(element);
                    }
                    list2.Clear();
                }
                else
                {
                    list2 = new List<IElement>();
                    ErrElements.Add(pFeature.OID, list2);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    IElement element2 = CreateElement(pActiveView, list[i]);
                    if (element2 == null)
                    {
                        return;
                    }
                    container.AddElement(element2, 0);
                    list2.Add(element2);
                }
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
            }
        }

        private static IElement CreateElement(IActiveView pActiveView, IGeometry pGeo)
        {
            if (pGeo is IPolygon)
            {
                return CreatePolygonElement(pActiveView, pGeo);
            }
            if ((pGeo is IPolyline) || (pGeo is ILine))
            {
                return CreateLineElement(pActiveView, pGeo);
            }
            if (pGeo is IPoint)
            {
                return CreatePointElement(pActiveView, pGeo);
            }
            return null;
        }

        private static IElement CreateLineElement(IActiveView pActiveView, IGeometry pGeo)
        {
            IRgbColor color = new RgbColorClass();
            color.Blue = 0xff;
            color.Green = 0;
            color.Red = 0xc5;
            IColor color2 = color;
            ISimpleLineSymbol symbol = new SimpleLineSymbolClass();
            symbol.Style = esriSimpleLineStyle.esriSLSSolid;
            symbol.Color = color2;
            symbol.Width = 2.0;
            pGeo = ErrManager.ConvertPoject(pGeo, pActiveView.FocusMap.SpatialReference);
            IElement element = new LineElementClass();
            element.Geometry = pGeo;
            ILineElement element2 = element as ILineElement;
            element2.Symbol = symbol;
            return element;
        }
        private static IGeometry ConvertPoject(IGeometry pGeometry, ISpatialReference pSpatialReference)
        {
            try
            {
                if (pGeometry.SpatialReference != pSpatialReference)
                {
                    pGeometry.Project(pSpatialReference);
                    pGeometry.SpatialReference = pSpatialReference;
                }
                return pGeometry;
            }
            catch (Exception)
            {
                return pGeometry;
            }
        }

        public static IElement CreateMarkerElement(IActiveView pActiveView, double pX, double pY, ISpatialReference pDataSR)
        {
            new MarkerElementClass();
            IPoint point = new PointClass();
            point.X = pX;
            point.Y = pY;
            IGeometry pGeo = point;
            pGeo.SpatialReference = pDataSR;
            return CreatePointElement(pActiveView, pGeo);
        }

        private static IElement CreatePointElement(IActiveView pActiveView, IGeometry pGeo)
        {
            pGeo = ErrManager.ConvertPoject(pGeo, pActiveView.FocusMap.SpatialReference);
            IMarkerElement element = new MarkerElementClass();
            IElement element2 = element as IElement;
            IRgbColor color = new RgbColorClass();
            color.Blue = 0xff;
            color.Green = 0;
            color.Red = 0xc5;
            IColor color2 = color;
            ISimpleMarkerSymbol symbol = new SimpleMarkerSymbolClass();
            symbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            symbol.Color = color2;
            symbol.Size = 8.0;
            element.Symbol = symbol;
            element2.Geometry = pGeo;
            return element2;
        }

        public static IElement CreatePolygonElement(IActiveView pActiveView, IGeometry pGeo)
        {
            Random rnd = new Random();
            IRgbColor color = new RgbColorClass();
            color.Blue = rnd.Next(0, 255);
            color.Green = rnd.Next(0, 255);
            color.Red = rnd.Next(0, 255);
            IColor color2 = color;
            ISimpleLineSymbol symbol = new SimpleLineSymbolClass();
            symbol.Style = esriSimpleLineStyle.esriSLSSolid;
            symbol.Color = color2;
            symbol.Width = 2.0;
            ISimpleFillSymbol symbol2 = new SimpleFillSymbolClass();
            symbol2.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
            IRgbColor color3 = new RgbColorClass();
            color.Blue = rnd.Next(0, 255);
            color.Green = rnd.Next(0, 255);
            color.Red = rnd.Next(0, 255);
            symbol2.Color = color3;
            symbol2.Outline = symbol;
            pGeo = ErrManager.ConvertPoject(pGeo, pActiveView.FocusMap.SpatialReference);
            ISimpleFillSymbol symbol3 = symbol2;
            IElement element = new PolygonElementClass();
            element.Geometry = pGeo;
            IFillShapeElement element2 = element as IFillShapeElement;
            element2.Symbol = symbol3;
            return element;
        }

        public static void ZoomToErr(IActiveView pActiveView, IFeature pFeature)
        {
            IGeometry shapeCopy = pFeature.ShapeCopy;
            ZoomToErr(pActiveView, shapeCopy);
        }

        public static void ZoomToErr(IActiveView pActiveView, IGeometry pGeo)
        {
            pGeo = ErrManager.ConvertPoject(pGeo, pActiveView.FocusMap.SpatialReference);
            IEnvelope envelope = new EnvelopeClass();
            envelope.SpatialReference = pActiveView.FocusMap.SpatialReference;
            envelope.PutCoords(pGeo.Envelope.XMin, pGeo.Envelope.YMin, pGeo.Envelope.XMax, pGeo.Envelope.YMax);
            envelope.Expand(1.3, 1.3, false);
            pActiveView.Extent = envelope;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, pActiveView.Extent);
        }

        public static void ClearElement(IActiveView pActiveView)
        {
            IGraphicsContainer focusMap = pActiveView.FocusMap as IGraphicsContainer;
            foreach (KeyValuePair<int, List<IElement>> pair in ErrManager.ErrElements)
            {
                List<IElement> list = pair.Value;
                foreach (IElement element in list)
                {
                    focusMap.DeleteElement(element);
                }
                list.Clear();
            }
            ErrManager.ErrElements.Clear();
        }
    }
}

