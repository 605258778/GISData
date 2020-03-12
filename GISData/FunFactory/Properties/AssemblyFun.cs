namespace FunFactory
{
    using System;
    using System.Reflection;
    using System.Resources;
    using Utilities;

    public class AssemblyFun
    {
        private const string mClassName = "FunFactory.AssemblyFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        public ResourceManager GetResourceManager(string resName)
        {
            try
            {
                Assembly callingAssembly = null;
                callingAssembly = Assembly.GetCallingAssembly();
                string[] manifestResourceNames = callingAssembly.GetManifestResourceNames();
                string str = "";
                foreach (string str2 in manifestResourceNames)
                {
                    str = str2;
                    if ((str.Length >= (resName + ".resources").Length) && (str.Substring(str.Length - (resName + ".resources").Length, (resName + ".resources").Length) == (resName + ".resources")))
                    {
                        return new ResourceManager(str.Substring(0, str.Length - ".resources".Length), callingAssembly);
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.AssemblyFun", "GetResourceManager", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }
    }
}

