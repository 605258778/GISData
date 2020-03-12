namespace TopologyCheck.Base
{
    using ESRI.ArcGIS.AnalysisTools;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.DataManagementTools;
    using ESRI.ArcGIS.DataSourcesFile;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using ESRI.ArcGIS.Geoprocessing;
    using ESRI.ArcGIS.Geoprocessor;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public class SpatialAnalysis
    {
        public static bool Buffer(IFeatureClass pInFClass, double bufferDistance, string sBufferFile)
        {
            try
            {
                if (0.0 == bufferDistance)
                {
                    return false;
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(sBufferFile)) || (".shp" != System.IO.Path.GetExtension(sBufferFile)))
                {
                    return false;
                }
                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoprocessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Buffer process = new ESRI.ArcGIS.AnalysisTools.Buffer(pInFClass, sBufferFile, bufferDistance);
                IGeoProcessorResult result = (IGeoProcessorResult) geoprocessor.Execute(process, null);
                if (result.Status != esriJobStatus.esriJobSucceeded)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Clip(IFeatureClass pInFClass, IFeatureClass pClipFClass, string sFile)
        {
            try
            {
                if ((pInFClass == null) || (pClipFClass == null))
                {
                    return false;
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(sFile)) || (".shp" != System.IO.Path.GetExtension(sFile)))
                {
                    return false;
                }
                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoprocessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Clip process = new ESRI.ArcGIS.AnalysisTools.Clip();
                process.in_features = pInFClass;
                process.clip_features = pClipFClass;
                process.out_feature_class = sFile;
                IGeoProcessorResult result = (IGeoProcessorResult) geoprocessor.Execute(process, null);
                if (result.Status != esriJobStatus.esriJobSucceeded)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static IFeatureClass CreatePolygonFeatureClass(string featureClassName, IFeatureWorkspace featureWorkspace, ISpatialReference pSR)
        {
            try
            {
                IFeatureClass class2 = featureWorkspace.OpenFeatureClass(featureClassName);
                if (class2 != null)
                {
                    ((IDataset) class2).Delete();
                }
            }
            catch
            {
            }
            try
            {
                IFeatureClassDescription description = new FeatureClassDescriptionClass();
                IObjectClassDescription description2 = (IObjectClassDescription) description;
                UID classExtensionCLSID = description2.ClassExtensionCLSID;
                IFields inputField = new FieldsClass();
                IFieldsEdit edit = (IFieldsEdit) inputField;
                IField field = new FieldClass();
                IFieldEdit edit2 = (IFieldEdit) field;
                edit2.Name_2 = "OID";
                edit2.Type_2 = esriFieldType.esriFieldTypeOID;
                edit.AddField(field);
                IGeometryDef def = new GeometryDefClass();
                IGeometryDefEdit edit3 = (IGeometryDefEdit) def;
                edit3.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                edit3.SpatialReference_2 = pSR;
                IField field2 = new FieldClass();
                IFieldEdit edit4 = (IFieldEdit) field2;
                edit4.Name_2 = "Shape";
                edit4.Type_2 = esriFieldType.esriFieldTypeGeometry;
                edit4.GeometryDef_2 = def;
                edit.AddField(field2);
                IFieldChecker checker = new FieldCheckerClass();
                IEnumFieldError error = null;
                IFields fixedFields = null;
                checker.ValidateWorkspace = (IWorkspace) featureWorkspace;
                checker.Validate(inputField, out error, out fixedFields);
                return featureWorkspace.CreateFeatureClass(featureClassName, fixedFields, null, classExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
            }
            catch
            {
                return null;
            }
        }

        public static IFeatureClass Dissolve(IFeatureClass pInFClass, string sFile)
        {
            try
            {
                IFeatureClass class2;
                IQueryFilter filter;
                if (pInFClass == null)
                {
                    return null;
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(sFile)) || (".shp" != System.IO.Path.GetExtension(sFile)))
                {
                    return null;
                }
                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoprocessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.Dissolve process = new ESRI.ArcGIS.DataManagementTools.Dissolve();
                process.in_features = pInFClass;
                process.out_feature_class = sFile;
                IGeoProcessorResult result = (IGeoProcessorResult) geoprocessor.Execute(process, null);
                if (result.Status != esriJobStatus.esriJobSucceeded)
                {
                    return null;
                }
                IGPUtilities utilities = new GPUtilitiesClass();
                utilities.DecodeFeatureLayer(result.GetOutput(0), out class2, out filter);
                return class2;
            }
            catch
            {
                return null;
            }
        }

        public static IGeometry DissolveGeo(IFeatureClass pInFClass, string sFile)
        {
            try
            {
                IFeatureClass class2 = Dissolve(pInFClass, sFile);
                if (class2 == null)
                {
                    return null;
                }
                IFeatureCursor o = class2.Search(null, false);
                IFeature feature = o.NextFeature();
                if (feature == null)
                {
                    return null;
                }
                IGeometry shapeCopy = null;
                while (feature != null)
                {
                    if (shapeCopy == null)
                    {
                        shapeCopy = feature.ShapeCopy;
                    }
                    else
                    {
                        shapeCopy = ((ITopologicalOperator2) shapeCopy).Union(feature.ShapeCopy);
                    }
                    feature = o.NextFeature();
                }
                Marshal.ReleaseComObject(o);
                return shapeCopy;
            }
            catch
            {
                return null;
            }
        }

        public static bool Erase(IFeatureClass pInFClass, IFeatureClass pEraseFClass, string sFile)
        {
            try
            {
                if ((pInFClass == null) || (pEraseFClass == null))
                {
                    return false;
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(sFile)) || (".shp" != System.IO.Path.GetExtension(sFile)))
                {
                    return false;
                }
                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoprocessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Erase process = new ESRI.ArcGIS.AnalysisTools.Erase();
                process.in_features = pInFClass;
                process.erase_features = pEraseFClass;
                process.out_feature_class = sFile;
                IGeoProcessorResult result = (IGeoProcessorResult) geoprocessor.Execute(process, null);
                if (result.Status != esriJobStatus.esriJobSucceeded)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Erase2(IFeatureClass pInFClass, IFeatureClass pEraseFClass, string sFile)
        {
            try
            {
                if ((pInFClass == null) || (pEraseFClass == null))
                {
                    return false;
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(sFile)) || (".shp" != System.IO.Path.GetExtension(sFile)))
                {
                    return false;
                }
                string str = System.IO.Path.GetDirectoryName(sFile) + @"\diff.shp";
                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoprocessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                SymDiff process = new SymDiff();
                process.in_features = pInFClass;
                process.update_features = pEraseFClass;
                process.out_feature_class = str;
                IGeoProcessorResult result = (IGeoProcessorResult) geoprocessor.Execute(process, null);
                if (result.Status != esriJobStatus.esriJobSucceeded)
                {
                    return false;
                }
                ESRI.ArcGIS.AnalysisTools.Clip clip = new ESRI.ArcGIS.AnalysisTools.Clip();
                clip.in_features = str;
                clip.clip_features = pInFClass;
                clip.out_feature_class = sFile;
                IGeoProcessorResult result2 = (IGeoProcessorResult) geoprocessor.Execute(clip, null);
                if (result2.Status != esriJobStatus.esriJobSucceeded)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IFeatureClass Erase3(IGeometry pBoundryGeo, IFeatureClass pEraseFClass, string sTempPath, string sFClassName)
        {
            if (pBoundryGeo == null)
            {
                return null;
            }
            if (pEraseFClass == null)
            {
                return null;
            }
            try
            {
                IGeometry other = null;
                IFeatureCursor cursor = pEraseFClass.Search(null, false);
                for (IFeature feature = cursor.NextFeature(); feature != null; feature = cursor.NextFeature())
                {
                    if (other == null)
                    {
                        other = feature.ShapeCopy;
                    }
                    else
                    {
                        ITopologicalOperator2 @operator = (ITopologicalOperator2) other;
                        @operator.IsKnownSimple_2 = true;
                        @operator.Simplify();
                        other = @operator.Union(feature.ShapeCopy);
                    }
                }
                if (pBoundryGeo.GeometryType == esriGeometryType.esriGeometryEnvelope)
                {
                    IPolygon polygon = new PolygonClass();
                    polygon.SpatialReference = pBoundryGeo.SpatialReference;
                    object missing = Type.Missing;
                    IPointCollection points = polygon as IPointCollection;
                    IPoint inPoint = new PointClass();
                    inPoint.X = pBoundryGeo.Envelope.XMin;
                    inPoint.Y = pBoundryGeo.Envelope.YMax;
                    points.AddPoint(inPoint, ref missing, ref missing);
                    IPoint point2 = new PointClass();
                    point2.X = pBoundryGeo.Envelope.XMax;
                    point2.Y = pBoundryGeo.Envelope.YMax;
                    points.AddPoint(point2, ref missing, ref missing);
                    IPoint point3 = new PointClass();
                    point3.X = pBoundryGeo.Envelope.XMax;
                    point3.Y = pBoundryGeo.Envelope.YMin;
                    points.AddPoint(point3, ref missing, ref missing);
                    IPoint point4 = new PointClass();
                    point4.X = pBoundryGeo.Envelope.XMin;
                    point4.Y = pBoundryGeo.Envelope.YMin;
                    points.AddPoint(point4, ref missing, ref missing);
                    pBoundryGeo = polygon;
                }
                ITopologicalOperator2 operator2 = (ITopologicalOperator2) pBoundryGeo;
                operator2.IsKnownSimple_2 = true;
                operator2.Simplify();
                IGeometry pGeo = operator2.Difference(other);
                if (pGeo.IsEmpty)
                {
                    return null;
                }
                IList<IGeometry> geometrys = GetGeometrys(pGeo);
                if (geometrys == null)
                {
                    return null;
                }
                IWorkspaceName name = new WorkspaceNameClass();
                name.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";
                name.PathName = sTempPath;
                IName name2 = (IName) name;
                IWorkspace workspace = (IWorkspace) name2.Open();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace) workspace;
                IFeatureClass class2 = CreatePolygonFeatureClass(sFClassName, featureWorkspace, (pEraseFClass as IGeoDataset).SpatialReference);
                if (class2 == null)
                {
                    return null;
                }
                IWorkspaceEdit edit = featureWorkspace as IWorkspaceEdit;
                edit.StartEditing(false);
                edit.StartEditOperation();
                for (int i = 0; i < geometrys.Count; i++)
                {
                    IGeometry geometry3 = geometrys[i];
                    IFeature feature2 = class2.CreateFeature();
                    feature2.Shape = geometry3;
                    feature2.Store();
                }
                edit.StopEditOperation();
                edit.StopEditing(true);
                return class2;
            }
            catch
            {
                return null;
            }
        }

        public static IFeatureClass ExportToShp(IWorkspace pSourceWS, IFeatureClass pSourceFC, string sTargetPath, string sTargetName, IQueryFilter pQueryFilter)
        {
            IFeatureClass class4;
            try
            {
                if (pSourceWS == null)
                {
                    return null;
                }
                if (pSourceFC == null)
                {
                    return null;
                }
                string name = ((IDataset) pSourceFC).Name;
                if (sTargetName == "")
                {
                    sTargetName = name + "_Export";
                }
                IWorkspaceFactory factory = new ShapefileWorkspaceFactoryClass();
                IWorkspace workspace = factory.OpenFromFile(sTargetPath, 0);
                if (workspace == null)
                {
                    return null;
                }
                try
                {
                    IFeatureClass class2 = ((IFeatureWorkspace) workspace).OpenFeatureClass(sTargetName);
                    if (class2 != null)
                    {
                        ((IDataset) class2).Delete();
                    }
                }
                catch
                {
                }
                IDataset dataset = (IDataset) pSourceWS;
                IWorkspaceName fullName = (IWorkspaceName) dataset.FullName;
                IFeatureClassName inputDatasetName = new FeatureClassNameClass();
                IDatasetName name4 = (IDatasetName) inputDatasetName;
                name4.Name = name;
                name4.WorkspaceName = fullName;
                IDataset dataset2 = (IDataset) workspace;
                IWorkspaceName name6 = (IWorkspaceName) dataset2.FullName;
                IFeatureClassName outputFClassName = new FeatureClassNameClass();
                IDatasetName name8 = (IDatasetName) outputFClassName;
                name8.Name = sTargetName;
                name8.WorkspaceName = name6;
                IFieldChecker checker = new FieldCheckerClass();
                IFields inputField = pSourceFC.Fields;
                IFields fixedFields = null;
                IEnumFieldError error = null;
                checker.InputWorkspace = pSourceWS;
                checker.ValidateWorkspace = workspace;
                checker.Validate(inputField, out error, out fixedFields);
                string shapeFieldName = pSourceFC.ShapeFieldName;
                int index = pSourceFC.FindField(shapeFieldName);
                IClone geometryDef = (IClone) inputField.get_Field(index).GeometryDef;
                IGeometryDef outputGeometryDef = (IGeometryDef) geometryDef.Clone();
                IFeatureDataConverter converter = new FeatureDataConverterClass();
                IEnumInvalidObject obj2 = converter.ConvertFeatureClass(inputDatasetName, pQueryFilter, null, outputFClassName, outputGeometryDef, fixedFields, "", 0x3e8, 0);
                obj2.Reset();
                if (obj2.Next() != null)
                {
                    class4 = null;
                }
                else
                {
                    try
                    {
                        class4 = ((IFeatureWorkspace) workspace).OpenFeatureClass(sTargetName);
                    }
                    catch
                    {
                        class4 = null;
                    }
                }
            }
            catch (Exception)
            {
                class4 = null;
            }
            return class4;
        }

        public static IList<IGeometry> GetGeometrys(IGeometry pGeo)
        {
            IList<IGeometry> list = new List<IGeometry>();
            try
            {
                IPolygon4 polygon = pGeo as IPolygon4;
                if (polygon.ExteriorRingCount < 2)
                {
                    list.Add(pGeo);
                    return list;
                }
                IEnumGeometry exteriorRingBag = polygon.ExteriorRingBag as IEnumGeometry;
                exteriorRingBag.Reset();
                for (IRing ring = exteriorRingBag.Next() as IRing; ring != null; ring = exteriorRingBag.Next() as IRing)
                {
                    IGeometryBag bag2 = polygon.get_InteriorRingBag(ring);
                    object missing = Type.Missing;
                    IGeometryCollection geometrys = null;
                    geometrys = new PolygonClass();
                    geometrys.AddGeometry(ring, ref missing, ref missing);
                    IPolygon item = geometrys as IPolygon;
                    item.SpatialReference = pGeo.SpatialReference;
                    ITopologicalOperator2 @operator = (ITopologicalOperator2) item;
                    @operator.IsKnownSimple_2 = false;
                    @operator.Simplify();
                    if (!bag2.IsEmpty)
                    {
                        IGeometryCollection geometrys2 = new PolygonClass();
                        IEnumGeometry geometry2 = bag2 as IEnumGeometry;
                        geometry2.Reset();
                        for (IRing ring2 = geometry2.Next() as IRing; ring2 != null; ring2 = geometry2.Next() as IRing)
                        {
                            geometrys2.AddGeometry(ring2, ref missing, ref missing);
                        }
                        IPolygon other = geometrys2 as IPolygon;
                        other.SpatialReference = pGeo.SpatialReference;
                        ITopologicalOperator2 operator2 = (ITopologicalOperator2) other;
                        operator2.IsKnownSimple_2 = false;
                        operator2.Simplify();
                        IGeometry geometry3 = @operator.Difference(other);
                        list.Add(geometry3);
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
            }
            catch
            {
                return null;
            }
            return list;
        }

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

        public static IFeatureClass Intersect(IList<IFeatureClass> pFCList, string sFile)
        {
            try
            {
                IFeatureClass class2;
                IQueryFilter filter;
                if ((pFCList == null) || (pFCList.Count < 1))
                {
                    return null;
                }
                IGpValueTableObject obj2 = new GpValueTableObjectClass();
                obj2.SetColumns(1);
                object row = null;
                for (int i = 0; i < pFCList.Count; i++)
                {
                    row = pFCList[i];
                    obj2.AddRow(ref row);
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(sFile)) || (".shp" != System.IO.Path.GetExtension(sFile)))
                {
                    return null;
                }
                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoprocessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Intersect process = new ESRI.ArcGIS.AnalysisTools.Intersect(obj2, sFile);
                IGeoProcessorResult result = (IGeoProcessorResult) geoprocessor.Execute(process, null);
                if (result.Status != esriJobStatus.esriJobSucceeded)
                {
                    return null;
                }
                IGPUtilities utilities = new GPUtilitiesClass();
                utilities.DecodeFeatureLayer(result.GetOutput(0), out class2, out filter);
                return class2;
            }
            catch
            {
                return null;
            }
        }
    }
}

