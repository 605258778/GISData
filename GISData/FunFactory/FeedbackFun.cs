namespace FunFactory
{
    using System;
    using Utilities;

    public class FeedbackFun
    {
        private const string mClassName = "FunFactory.FeedbackFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal FeedbackFun()
        {
        }
    }
}

