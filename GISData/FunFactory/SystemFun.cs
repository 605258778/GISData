namespace FunFactory
{
    using ESRI.ArcGIS.esriSystem;
    using Microsoft.VisualBasic;
    using System;
    using System.IO;
    using Utilities;

    public class SystemFun
    {
        private const string mClassName = "FunFactory.SystemFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal SystemFun()
        {
        }

        public void AssignObjectProperties(IClone pSource, IClone pTarget)
        {
            try
            {
                if ((pSource != null) && (pTarget != null))
                {
                    pTarget.Assign(pSource);
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SystemFun", "AssignObjectProperties", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public IClone CloneObejct(IClone pCloner)
        {
            try
            {
                if (pCloner == null)
                {
                    return null;
                }
                return pCloner.Clone();
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SystemFun", "CloneObejct", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public string GetTempPath()
        {
            try
            {
                string path = null;
                path = Environment.GetEnvironmentVariable("Temp");
                if ((path == null) || !Directory.Exists(path))
                {
                    path = Directory.GetCurrentDirectory();
                    if ((path == null) || !Directory.Exists(path))
                    {
                        path = @"C:\Temp";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
                if (Strings.Right(path, 1) != @"\")
                {
                    path = path + @"\";
                }
                return path;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SystemFun", "GetTempPath", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool IsEqualObjectProperties(IClone pOriginal, IClone pOther)
        {
            try
            {
                if (pOriginal == null)
                {
                    return false;
                }
                if (pOther == null)
                {
                    return false;
                }
                return pOriginal.IsEqual(pOther);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SystemFun", "IsEqualObjectProperties", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }
    }
}

