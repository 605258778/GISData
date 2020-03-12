namespace FunFactory
{
    using System;
    using Utilities;

    public class RasterFun
    {
        private const string mClassName = "FunFactory.RasterFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal RasterFun()
        {
        }
    }
}

