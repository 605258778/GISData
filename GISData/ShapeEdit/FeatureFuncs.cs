namespace ShapeEdit
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using FunFactory;
    using System;
    using TaskManage;
    using Utilities;

    public class FeatureFuncs
    {
        public static IEnvelope GetSearchEnvelope(IActiveView pActiveView, IPoint pPoint)
        {
            try
            {
                double num = 6.0;
                IEnvelope visibleBounds = null;
                if (pActiveView != null)
                {
                    IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
                    visibleBounds = displayTransformation.VisibleBounds;
                    tagRECT deviceFrame = displayTransformation.get_DeviceFrame();
                    double height = 0.0;
                    long num3 = 0L;
                    height = visibleBounds.Height;
                    num3 = deviceFrame.bottom - deviceFrame.top;
                    double num4 = 0.0;
                    num4 = height / ((double) num3);
                    num *= num4;
                }
                if (pPoint == null)
                {
                    return null;
                }
                visibleBounds = pPoint.Envelope;
                visibleBounds.Width = num;
                visibleBounds.Height = num;
                visibleBounds.CenterAt(pPoint);
                return visibleBounds;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IFeatureCursor SearchFeatures(IFeatureLayer pFLayer, IGeometry pFilterGeometry, esriSpatialRelEnum enumSpatialRel)
        {
            if (pFLayer == null)
            {
                return null;
            }
            IFeatureClass featureClass = pFLayer.FeatureClass;
            if (featureClass == null)
            {
                return null;
            }
            try
            {
                if (pFilterGeometry == null)
                {
                    return null;
                }
                ISpatialFilter queryFilter = new SpatialFilterClass {
                    Geometry = pFilterGeometry,
                    GeometryField = featureClass.ShapeFieldName,
                    SpatialRel = enumSpatialRel
                };
                return pFLayer.Search(queryFilter, false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetFeatureArea(IFeature pFeature)
        {
            if (pFeature != null)
            {
                try
                {
                    IGeometry shapeCopy = pFeature.ShapeCopy;
                    if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        double area = ((IArea) GISFunFactory.UnitFun.ConvertPoject(shapeCopy, Editor.UniqueInstance.Map.SpatialReference)).Area;
                        string str = EditTask.KindCode.Substring(0, 2);
                        string name = "";
                        string str3 = "";
                        string str4 = "";
                        if (str == "01")
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 2);
                            name = "Afforest";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else if (str == "02")
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 2);
                            name = "Harvest";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                            str4 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "ZTAreaField");
                        }
                        else if (str == "06")
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 2);
                            name = "Disaster";
                            str4 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "ZTAreaField");
                        }
                        else if (str == "07")
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 2);
                            name = "ForestCase";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else if (str == "04")
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 4);
                            name = "Expropriation";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                            str4 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "ZTAreaField");
                        }
                        else if (str == "05")
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 2);
                            name = "Fire";
                            str3 = UtilFactory.GetConfigOpt().GetConfigValue2(name, "AreaField");
                        }
                        else
                        {
                            area = Math.Round(Math.Abs((double) (area / 10000.0)), 2);
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
                        pFeature.Store();
                    }
                }
                catch
                {
                }
            }
        }
    }
}

