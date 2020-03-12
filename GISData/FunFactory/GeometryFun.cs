namespace FunFactory
{
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using Microsoft.VisualBasic;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using Utilities;

    public class GeometryFun
    {
        private const string mClassName = "FunFactory.GeometryFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal GeometryFun()
        {
        }

        public IGeometry ConvertGeometrySpatialReference(IGeometry pGeometry, ISpatialReference pSpatRef, ISpatialReference pProject)
        {
            try
            {
                if (pGeometry == null)
                {
                    return null;
                }
                IGeometry geometry = null;
                geometry = GISFunFactory.SystemFun.CloneObejct(pGeometry as IClone) as IGeometry;
                if (pSpatRef != null)
                {
                    geometry.SpatialReference = pSpatRef;
                }
                if (pProject != null)
                {
                    geometry.Project(pProject);
                }
                return geometry;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "ConvertGeometrySpatialReference", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IPolyline ConvertPointsToPolyline(IPointCollection pPoints)
        {
            try
            {
                if (pPoints == null)
                {
                    return null;
                }
                IClone clone = pPoints as IClone;
                IPointCollection points = null;
                points = clone.Clone() as IPointCollection;
                if (points.PointCount < 0)
                {
                    return null;
                }
                IPointCollection points2 = null;
                points2 = new PolylineClass();
                int i = 0;
                object before = null;
                object after = null;
                for (i = 0; i <= (points.PointCount - 1); i++)
                {
                    points2.AddPoint(points.get_Point(i), ref before, ref after);
                }
                (points2 as ITopologicalOperator).Simplify();
                return (points2 as IPolyline);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "ConvertPointsToPolyline", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IPolygon ConvertPolylineToPolygon(IPolyline pPolyline)
        {
            try
            {
                if (pPolyline == null)
                {
                    return null;
                }
                IClone clone = pPolyline as IClone;
                IGeometryCollection geometrys = null;
                geometrys = clone.Clone() as IGeometryCollection;
                if (geometrys.GeometryCount < 0)
                {
                    return null;
                }
                IGeometryCollection geometrys2 = null;
                geometrys2 = new PolygonClass();
                ISegmentCollection segments = null;
                int index = 0;
                object before = null;
                object after = null;
                for (index = 0; index <= (geometrys.GeometryCount - 1); index++)
                {
                    segments = new RingClass();
                    segments.AddSegmentCollection(geometrys.get_Geometry(index) as ISegmentCollection);
                    geometrys2.AddGeometry(segments as IGeometry, ref before, ref after);
                }
                (geometrys2 as ITopologicalOperator).Simplify();
                return (geometrys2 as IPolygon);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "ConvertPolylineToPolygon", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public ISpatialReference CreateSpatialReferenceFromPRJFile(string sPRJFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(sPRJFileName) | !File.Exists(sPRJFileName))
                {
                    Interaction.MsgBox("空间参考文件 " + sPRJFileName + " 丢失。", MsgBoxStyle.Exclamation, "创建空间参考错误");
                    return null;
                }
                ISpatialReferenceFactory factory = null;
                factory = new SpatialReferenceEnvironmentClass();
                ISpatialReference reference = null;
                reference = factory.CreateESRISpatialReferenceFromPRJFile(sPRJFileName);
                factory = null;
                return reference;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "CreateSpatialReferenceFromPRJFile", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public ISpatialReference CreateSpatialReferenceGCS(string sSRString)
        {
            try
            {
                if (string.IsNullOrEmpty(sSRString))
                {
                    return null;
                }
                IESRISpatialReference reference = null;
                reference = new GeographicCoordinateSystemClass();
                int cBytesRead = 0;
                reference.ImportFromESRISpatialReference(sSRString, out cBytesRead);
                return (reference as ISpatialReference);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "CreateSpatialReferenceGCS", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public ISpatialReference CreateSpatialReferencePCS(string sSRString)
        {
            try
            {
                if (string.IsNullOrEmpty(sSRString))
                {
                    return null;
                }
                IESRISpatialReference reference = null;
                reference = new ProjectedCoordinateSystemClass();
                int cBytesRead = 0;
                reference.ImportFromESRISpatialReference(sSRString, out cBytesRead);
                return (reference as ISpatialReference);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "CreateSpatialReferencePCS", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public ISpatialReference CreateSpatialReferenceUCS(string sSRString)
        {
            try
            {
                if (string.IsNullOrEmpty(sSRString))
                {
                    return null;
                }
                IESRISpatialReference reference = null;
                reference = new UnknownCoordinateSystemClass();
                int cBytesRead = 0;
                reference.ImportFromESRISpatialReference(sSRString, out cBytesRead);
                return (reference as ISpatialReference);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "CreateSpatialReferenceUCS", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool ExportSpatialReferenceToPRJFile(ISpatialReference pSpatRef, string sPRJFileName, bool bOverwritePrompt)
        {
            try
            {
                if (pSpatRef == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sPRJFileName))
                {
                    SaveFileDialog dialog = new SaveFileDialog {
                        DefaultExt = "prj",
                        FileName = pSpatRef.Name,
                        Filter = "空间参考文件 (*.prj)|*.prj",
                        OverwritePrompt = bOverwritePrompt,
                        Title = "输出空间参考到文件"
                    };
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    sPRJFileName = dialog.FileName;
                    dialog = null;
                    if (string.IsNullOrEmpty(sPRJFileName))
                    {
                        return false;
                    }
                }
                else if ((File.Exists(sPRJFileName) & bOverwritePrompt) && (Interaction.MsgBox("文件 " + sPRJFileName + " 已存在。\r\n是否要替换？", MsgBoxStyle.YesNo, "确认替换") != MsgBoxResult.Yes))
                {
                    return false;
                }
                if (File.Exists(sPRJFileName))
                {
                    FileSystem.Kill(sPRJFileName);
                }
                if (File.Exists(sPRJFileName))
                {
                    Interaction.MsgBox("文件 " + sPRJFileName + " 无法删除。\r\n共享冲突，文件正在使用。", MsgBoxStyle.Exclamation, "删除文件错误");
                    return false;
                }
                ISpatialReferenceFactory factory = null;
                factory = new SpatialReferenceEnvironmentClass();
                factory.ExportESRISpatialReferenceToPRJFile(sPRJFileName, pSpatRef);
                factory = null;
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "ExportSpatialReferenceToPRJFile", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public double MeasureArea(IGeometry pGeometry, ISpatialReference pSpatRef, ISpatialReference pProject)
        {
            try
            {
                if (pGeometry == null)
                {
                    return -1.0;
                }
                if (!(pGeometry is IArea))
                {
                    return -1.0;
                }
                IGeometry geometry = null;
                geometry = this.ConvertGeometrySpatialReference(pGeometry, pSpatRef, pProject);
                if (geometry == null)
                {
                    return -1.0;
                }
                IArea area = null;
                area = geometry as IArea;
                double num = 0.0;
                num = Math.Abs(area.Area);
                geometry = null;
                return num;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "MeasureArea", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1.0;
            }
        }

        public double MeasureLength(IGeometry pGeometry, ISpatialReference pSpatRef, ISpatialReference pProject)
        {
            try
            {
                if (pGeometry == null)
                {
                    return -1.0;
                }
                if (!(pGeometry is ICurve))
                {
                    return -1.0;
                }
                IGeometry geometry = null;
                geometry = this.ConvertGeometrySpatialReference(pGeometry, pSpatRef, pProject);
                if (geometry == null)
                {
                    return -1.0;
                }
                ICurve curve = null;
                curve = geometry as ICurve;
                double num = 0.0;
                num = Math.Abs(curve.Length);
                geometry = null;
                return num;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.GeometryFun", "MeasureLength", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1.0;
            }
        }
    }
}

