namespace FunFactory
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using Microsoft.VisualBasic;
    using System;
    using Utilities;

    public class FeatureFun
    {
        private const string mClassName = "FunFactory.FeatureFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal FeatureFun()
        {
        }

        public bool ClearFeatureData(string sSourceFile, WorkspaceSource pSourceType, string sFeatureClassName)
        {
            try
            {
                if (string.IsNullOrEmpty(sSourceFile) | string.IsNullOrEmpty(sFeatureClassName))
                {
                    return false;
                }
                IFeatureWorkspace featureWorkspace = null;
                featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, pSourceType);
                if (featureWorkspace == null)
                {
                    return false;
                }
                IFeatureClass class2 = null;
                class2 = featureWorkspace.OpenFeatureClass(sFeatureClassName);
                if (class2 == null)
                {
                    return false;
                }
                if (class2.FeatureCount(null) != 0)
                {
                    IWorkspaceEdit edit = null;
                    edit = featureWorkspace as IWorkspaceEdit;
                    edit.StartEditing(false);
                    IFeatureCursor cursor = null;
                    cursor = class2.Search(null, false);
                    IFeature feature = null;
                    for (feature = cursor.NextFeature(); feature != null; feature = cursor.NextFeature())
                    {
                        feature.Delete();
                    }
                    edit.StopEditing(true);
                }
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "GetFeatureWorkspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public int SearchClosestFeatureInCollection(Collection pFeatureCollection, IMap pMap, IGeometry pPosGeometry)
        {
            try
            {
                if (pFeatureCollection != null)
                {
                    if (pMap == null)
                    {
                        return -1;
                    }
                    if (pPosGeometry == null)
                    {
                        return -1;
                    }
                    if (pFeatureCollection.Count <= 0)
                    {
                        return -1;
                    }
                    IActiveView pActiveView = null;
                    pActiveView = pMap as IActiveView;
                    ISpatialReference pProject = null;
                    pProject = pMap.SpatialReference;
                    double num = 0.0;
                    num = GISFunFactory.UnitFun.ConvertPixelsToMapUnits(pActiveView, 6.0, true);
                    double num2 = 0.0;
                    num2 = GISFunFactory.UnitFun.ConvertPixelsToMapUnits(pActiveView, 4.0, true);
                    double num3 = 0.0;
                    num3 = GISFunFactory.UnitFun.ConvertPixelsToMapUnits(pActiveView, 3.0, true);
                    int num4 = -1;
                    int num5 = -1;
                    int num6 = -1;
                    IProximityOperator @operator = null;
                    @operator = pPosGeometry as IProximityOperator;
                    IFeature feature = null;
                    IGeometry other = null;
                    double num7 = 0.0;
                    long num8 = 0L;
                    for (num8 = 1L; num8 <= pFeatureCollection.Count; num8 += 1L)
                    {
                        feature = pFeatureCollection[num8] as IFeature;
                        other = GISFunFactory.GeometryFun.ConvertGeometrySpatialReference(feature.Shape, null, pProject);
                        num7 = @operator.ReturnDistance(other);
                        switch (other.GeometryType)
                        {
                            case esriGeometryType.esriGeometryPoint:
                                if (num < 0.0)
                                {
                                    num = num7 + 1.0;
                                }
                                if (num7 < num)
                                {
                                    num = num7;
                                    num4 = (int) num8;
                                }
                                break;

                            case esriGeometryType.esriGeometryPolyline:
                                if (num2 < 0.0)
                                {
                                    num2 = num7 + 1.0;
                                }
                                if (num7 < num2)
                                {
                                    num2 = num7;
                                    num5 = (int) num8;
                                }
                                break;

                            case esriGeometryType.esriGeometryPolygon:
                                if (num3 < 0.0)
                                {
                                    num3 = num7 + 1.0;
                                }
                                if (num7 < num3)
                                {
                                    num3 = num7;
                                    num6 = (int) num8;
                                }
                                break;

                            case esriGeometryType.esriGeometryLine:
                                if (num2 < 0.0)
                                {
                                    num2 = num7 + 1.0;
                                }
                                if (num7 < num2)
                                {
                                    num2 = num7;
                                    num5 = (int) num8;
                                }
                                break;
                        }
                    }
                    if (num4 > 0)
                    {
                        return num4;
                    }
                    if (num5 > 0)
                    {
                        return num5;
                    }
                    if (num6 > 0)
                    {
                        return num6;
                    }
                }
                return -1;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SearchClosestFeatureInCollection", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1;
            }
        }

        public IFeatureCursor SearchFeatureCursorFromFeatureClass(IFeatureClass pFClass, IGeometry pFilterGeometry, esriSpatialRelEnum enumSpatialRel)
        {
            try
            {
                if (pFClass == null)
                {
                    return null;
                }
                ISpatialFilter filter = null;
                if (pFilterGeometry == null)
                {
                    filter = null;
                }
                else
                {
                    filter = new SpatialFilterClass {
                        Geometry = pFilterGeometry,
                        GeometryField = pFClass.ShapeFieldName,
                        SpatialRel = enumSpatialRel
                    };
                }
                return pFClass.Search(filter, false);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SearchFeatureCursorFromFeatureClass", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IFeatureCursor SearchFeatureCursorFromFeatureClass(IFeatureClass pFClass, string sWhereClause, string sSubFields)
        {
            try
            {
                if (pFClass == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(sSubFields))
                {
                    sSubFields = "*";
                }
                IQueryFilter filter = null;
                if (string.IsNullOrEmpty(sWhereClause))
                {
                    filter = null;
                }
                else
                {
                    filter = new QueryFilterClass {
                        WhereClause = sWhereClause,
                        SubFields = sSubFields
                    };
                }
                return pFClass.Search(filter, false);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SearchFeatureCursorFromFeatureClass", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IFeatureCursor SearchFeatureCursorFromFeatureLayer(IFeatureLayer pFLayer, IGeometry pFilterGeometry, esriSpatialRelEnum enumSpatialRel)
        {
            try
            {
                if (pFLayer == null)
                {
                    return null;
                }
                return this.SearchFeatureCursorFromFeatureClass(pFLayer.FeatureClass, pFilterGeometry, enumSpatialRel);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SearchFeatureCursorFromFeatureLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IFeatureCursor SearchFeatureCursorFromFeatureLayer(IFeatureLayer pFLayer, string sWhereClause, string sSubFields)
        {
            try
            {
                if (pFLayer == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(sSubFields))
                {
                    sSubFields = "*";
                }
                return this.SearchFeatureCursorFromFeatureClass(pFLayer.FeatureClass, sWhereClause, sSubFields);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SearchFeatureCursorFromFeatureLayer", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool SetFeatureLayerSource(IFeatureLayer pFeatureLayer, IFeatureClass pFeatureClass)
        {
            try
            {
                if (pFeatureLayer == null)
                {
                    return false;
                }
                if (pFeatureClass == null)
                {
                    return false;
                }
                pFeatureLayer.FeatureClass = pFeatureClass;
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SetFeatureLayerSource", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public bool SetFeatureLayerSource(IFeatureLayer pFeatureLayer, string sSourceFile, WorkspaceSource pSourceType, string sFeatureClassName)
        {
            try
            {
                if (pFeatureLayer == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sSourceFile) | string.IsNullOrEmpty(sFeatureClassName))
                {
                    return false;
                }
                IFeatureWorkspace featureWorkspace = null;
                featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, pSourceType);
                if (featureWorkspace == null)
                {
                    return false;
                }
                IFeatureClass class2 = null;
                class2 = featureWorkspace.OpenFeatureClass(sFeatureClassName);
                if (class2 == null)
                {
                    return false;
                }
                pFeatureLayer.FeatureClass = class2;
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "SetFeatureLayerSource", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public void ZoomToFeature(IMap pMap, IFeature pFeature)
        {
            try
            {
                if ((pMap != null) && (pFeature != null))
                {
                    IGeometry shapeCopy = null;
                    IActiveView view = null;
                    IEnvelope envelope = null;
                    shapeCopy = pFeature.ShapeCopy;
                    if (shapeCopy.SpatialReference != pMap.SpatialReference)
                    {
                        shapeCopy.Project(pMap.SpatialReference);
                        shapeCopy.SpatialReference = pMap.SpatialReference;
                    }
                    envelope = new EnvelopeClass();
                    envelope = shapeCopy.Envelope;
                    view = pMap as IActiveView;
                    if (shapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        double num = 0.0;
                        double num2 = 0.0;
                        pMap.MapScale = 6000.0;
                        num = view.Extent.Width / 2.0;
                        num2 = view.Extent.Height / 2.0;
                        IPoint p = null;
                        p = shapeCopy as IPoint;
                        if ((num == 0.0) | (num2 == 0.0))
                        {
                            return;
                        }
                        envelope.Width = num;
                        envelope.Height = num2;
                        envelope.CenterAt(p);
                    }
                    else
                    {
                        envelope.Expand(1.2, 1.2, true);
                    }
                    if ((((view.Extent.Width != envelope.Width) || (view.Extent.Height != envelope.Height)) || ((view.Extent.XMin != envelope.XMin) || (view.Extent.YMin != envelope.YMin))) || ((view.Extent.XMax != envelope.XMax) || (view.Extent.YMax != envelope.YMax)))
                    {
                        view.FullExtent = envelope;
                        view.Refresh();
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FeatureFun", "ZoomToFeature", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
    }
}

