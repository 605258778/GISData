namespace Utilities
{
    //using jn.isos.log;
    using System;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// 返回错误信息的操作类
    /// </summary>
    public class ErrorOpt
    {
        private const string mBaseClassName = "Utilities.ErrorOpt";
        private string mClassName;
        private string mClientIP;
        private string mErrDescription;
        private const string mErrInfoEntryWord = "系统在运行过程中至少产生了一个错误，以下是错误的详细信息：\n";
        private string mErrNumber;
        private string mErrorLogFile;
        private const string mErrorLogFileDefault = @"C:\GISError.log";
        private ErrorOptMode mErrorOptMode;
        private const ErrorOptMode mErrorOptModeDefault = ErrorOptMode.contErrorOptModeNull;
        private string mErrorSendtoMail;
        private const string mErrorSendtoMailDefault = "";
        private string mErrorSimpleInfo;
        private const string mErrorSimpleInfoDefault = "系统在运行过程中产生了一个错误，我们已经记录了此错误信息，并会尽快解决此问题。";
        private string mErrSource;
        private string mOperation;
        private string mProcedureName;
        private string mSystemName;
        private string mUserID;
        //private Logger m_log = LoggerManager.CreateLogger("UI", typeof(ErrorOpt));
        /// <summary>
        /// 返回错误信息的操作接口
        /// </summary>
        internal ErrorOpt()
        {
            try
            {
                if (!string.IsNullOrEmpty(UtilFactory.GetConfigOpt().GetConfigValue("ErrorOptMode")))
                {//获取XML文件中<Error >节点。<ErrorOptMode Name="错误模式">4</ErrorOptMode>。其值为contErrorOptModeWriteLogFile = 4：代表为 4：错误操作类型：错误模式写入日志文件有误
                    this.mErrorOptMode = (ErrorOptMode) int.Parse(UtilFactory.GetConfigOpt().GetConfigValue("ErrorOptMode"));
                    if (this.mErrorOptMode == ErrorOptMode.contErrorOptModeNull)
                    {
                        int num = 0;
                        UtilFactory.GetConfigOpt().SetConfigValue("ErrorOptMode", num.ToString());
                        this.mErrorOptMode = ErrorOptMode.contErrorOptModeNull;
                    }//获取XML文件中<Error >节点。<ErrorSimpleInfo  Name="错误模式"></ErrorSimpleInfo>。 1：错误操作类型：错误模式的简易信息有误
                    this.mErrorSimpleInfo = UtilFactory.GetConfigOpt().GetConfigValue("ErrorSimpleInfo");
                    if (string.IsNullOrEmpty(this.mErrorSimpleInfo))
                    {
                        UtilFactory.GetConfigOpt().SetConfigValue("ErrorSimpleInfo", "系统在运行过程中产生了一个错误，我们已经记录了此错误信息，并会尽快解决此问题。");
                        this.mErrorSimpleInfo = "系统在运行过程中产生了一个错误，我们已经记录了此错误信息，并会尽快解决此问题。";
                    }//<ErrorLogFile Name="错误日志路径">Log\GISError.log</ErrorLogFile>
                    this.mErrorLogFile = UtilFactory.GetConfigOpt().RootPath +@"\" + UtilFactory.GetConfigOpt().GetConfigValue("ErrorLogFile");
                    if (string.IsNullOrEmpty(this.mErrorLogFile))
                    {
                        UtilFactory.GetConfigOpt().SetConfigValue("ErrorLogFile", @"C:\GISError.log");
                        this.mErrorLogFile = @"C:\GISError.log";
                    }
                    this.mErrorSendtoMail = UtilFactory.GetConfigOpt().GetConfigValue("ErrorSendtoMail");
                    if (string.IsNullOrEmpty(this.mErrorSendtoMail))
                    {
                        UtilFactory.GetConfigOpt().SetConfigValue("ErrorSendtoMail", "");
                        this.mErrorSendtoMail = "";
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ErrorOpt\n错误出处 : Sub New\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string CreateErrInfo(string sPrefix, string sPostfix)
        {
            try
            {
                string str = "";
                if (!string.IsNullOrEmpty(this.mSystemName))
                {
                    str = (str + sPrefix + "系统名称 : ") + this.mSystemName + sPostfix + "\r\n";
                }
                if (!string.IsNullOrEmpty(this.mClassName))
                {
                    str = (str + sPrefix + "类名称　 : ") + this.mClassName + sPostfix + "\r\n";
                }
                if (!string.IsNullOrEmpty(this.mProcedureName))
                {
                    str = (str + sPrefix + "过程方法 : ") + this.mProcedureName + sPostfix + "\r\n";
                }
                if (int.Parse(this.mErrNumber) != 0)
                {
                    str = (str + sPrefix + "错误代码 : ") + this.mErrNumber + sPostfix + "\r\n";
                }
                if (!string.IsNullOrEmpty(this.mErrSource))
                {
                    str = (str + sPrefix + "错误来源 : ") + this.mErrSource + sPostfix + "\r\n";
                }
                if (!string.IsNullOrEmpty(this.mErrDescription))
                {
                    str = (str + sPrefix + "错误描述 : ") + this.mErrDescription + sPostfix + "\r\n";
                }
                if (!string.IsNullOrEmpty(this.mUserID + this.mClientIP))
                {
                    str = str + sPrefix + "用户信息 : ";
                    str = str + this.mUserID + "/" + this.mClientIP + sPostfix + "\r\n";
                }
                if (!string.IsNullOrEmpty(this.mOperation))
                {
                    str = (str + sPrefix + "其他信息 : ") + this.mOperation + sPostfix + "\r\n";
                }
                return str;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ErrorOpt\n错误出处 : Function CreateErrInfo\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
            }
        }

        public bool ErrorOperate(string sSystemName, string sClassName, string sProcedureName, string lErrNumber, string sErrSource, string sErrDescription, string sUserID, string sClientIP, string sOperation)
        {
            try
            {
                this.SetLastErrInfo(sSystemName, sClassName, sProcedureName, lErrNumber, sErrSource, sErrDescription, sUserID, sClientIP, sOperation);
                if ((this.mErrorOptMode & ErrorOptMode.contErrorOptModePopupInfo) == ErrorOptMode.contErrorOptModePopupInfo)
                {
                    this.ShowErrorInfo();
                }
                if ((this.mErrorOptMode & ErrorOptMode.contErrorOptModeWriteLogFile) == ErrorOptMode.contErrorOptModeWriteLogFile)
                {
                    this.WriteErrInfoToLogFile();
                }
                if ((this.mErrorOptMode & ErrorOptMode.contErrorOptModeWriteDatabase) == ErrorOptMode.contErrorOptModeWriteDatabase)
                {
                    this.WriteErrInfoToDatabase();
                }
                if ((this.mErrorOptMode & ErrorOptMode.contErrorOptModeSendtoMail) == ErrorOptMode.contErrorOptModeSendtoMail)
                {
                    this.SendErrInfoToEMail();
                }
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ErrorOpt\n错误出处 : Function ErrorOperate\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private bool SendErrInfoToEMail()
        {
            return false;
        }

        public bool SetLastErrInfo(string sSystemName, string sClassName, string sProcedureName, string lErrNumber, string sErrSource, string sErrDescription, string sUserID, string sClientIP, string sOperation)
        {
            try
            {
                this.mSystemName = sSystemName;
                this.mClassName = sClassName;
                this.mProcedureName = sProcedureName;
                this.mErrNumber = lErrNumber;
                this.mErrSource = sErrSource;
                this.mErrDescription = sErrDescription;
                this.mUserID = sUserID;
                this.mClientIP = sClientIP;
                this.mOperation = sOperation;
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ErrorOpt\n错误出处 : Function SetLastErrInfo\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private bool ShowErrorInfo()
        {
            try
            {
                string text = null;
                if ((this.mErrorOptMode & ErrorOptMode.contErrorOptModeSimpleInfo) == ErrorOptMode.contErrorOptModeSimpleInfo)
                {
                    text = this.mErrorSimpleInfo;
                }
                else
                {
                    text = "系统在运行过程中至少产生了一个错误，以下是错误的详细信息：\n\n";
                    text = (text + this.CreateErrInfo("", "\t")) + "\n" + this.mErrorSimpleInfo;
                }
                MessageBox.Show(text, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ErrorOpt\n错误出处 : Function ShowErrorInfo\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private bool WriteErrInfoToDatabase()
        {
            return false;
        }

        private bool WriteErrInfoToLogFile()
        {
            try
            {
                string str = null;
                str = (DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + "\n") + this.CreateErrInfo("\t", "") + "\n";          
                //m_log.Error(str);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ErrorOpt\n错误出处 : Function WriteErrInfoToLogFile\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        /// <summary>
        /// 造成错误操作的枚举类
        /// </summary>
        public enum ErrorOptMode
        {
            /// <summary>
            /// 0：错误操作类型：模式为空有误
            /// </summary>
            contErrorOptModeNull = 0,
            /// <summary>
            ///:2：错误操作类型：错误模式的弹出式信息有误
            /// </summary>
            contErrorOptModePopupInfo = 2,
            /// <summary>
            /// 0x10：错误操作类型：错误模式发送至邮件有误
            /// </summary>
            contErrorOptModeSendtoMail = 0x10,
            /// <summary>
            /// 1：错误操作类型：错误模式的简易信息有误
            /// </summary>
            contErrorOptModeSimpleInfo = 1,
            /// <summary>
            /// 8：错误操作类型：错误模式写入底层数据有误
            /// </summary>
            contErrorOptModeWriteDatabase = 8,
            /// <summary>
            /// 4：错误操作类型：错误模式写入日志文件有误
            /// </summary>
            contErrorOptModeWriteLogFile = 4
        }
    }
}

