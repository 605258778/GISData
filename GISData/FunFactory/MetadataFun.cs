namespace FunFactory
{
    using System;
    using Utilities;

    public class MetadataFun
    {
        private const string mClassName = "FunFactory.MetadataFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal MetadataFun()
        {
        }
    }
}

