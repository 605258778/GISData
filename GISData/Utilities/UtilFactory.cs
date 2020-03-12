namespace Utilities
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.Windows.Forms;

    /// <summary>
    /// 公有的工厂类
    /// </summary>
    public class UtilFactory
    {
        private const string mClassName = "Utilities.UtilFactory";
        /// <summary>
        /// 配置MXL文件操作类
        /// </summary>
        private static ConfigOpt mConfigOpt;
        private static Hashtable mDBAccessColl;
        //private static DBOpt mDBOpt;
        private static string mDefaultDBKey = "";
        private static ErrorOpt mErrorOpt;
        //private static Collection mRegOptColl = new Collection();
        private static string mSystemKey = "";
        private static string mSystemName = "";

        /// <summary>
        /// 以下源代码被注销
        /// </summary>
        /// <param name="sDBKey"></param>
        /// <returns></returns>
        //private static IDBAccess CreateDBAccess(string sDBKey)
        //{
        //    try
        //    {
        //        IDBAccess access = null;
        //        switch (GetConfigOpt().GetConfigValue2(sDBKey, "DBProvider").ToLower().Trim())
        //        {
        //            case "odbc":
        //                access = new DBAccessOdbc(sDBKey);
        //                break;

        //            case "oledb":
        //                access = new DBAccessOleDb(sDBKey);
        //                break;

        //            case "oracleclient":
        //                access = new DBAccessOracleClient(sDBKey);
        //                break;

        //            case "sqlclient":
        //                access = new DBAccessSqlClient(sDBKey);
        //                break;

        //            default:
        //                return null;
        //        }
        //        return access;
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function CreateDBAccess\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获取配置XML文件的操作方法。返回配置XML文件操作类
        /// </summary>
        /// <returns></returns>
        public static ConfigOpt GetConfigOpt()
        {
            try
            {
                if (mConfigOpt == null)
                {
                    mConfigOpt = new ConfigOpt();
                }
                return mConfigOpt;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function GetConfigOpt\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        public static ConfigOpt GetConfigOpt2()
        {
            try
            {
                mConfigOpt = new ConfigOpt();
                return mConfigOpt;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function GetConfigOpt\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        /// <summary>
        /// 以下源代码被注销
        /// </summary>
        /// <param name="sDBKey"></param>
        /// <returns></returns>
        //public static IDBAccess GetDBAccess(string sDBKey)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(sDBKey))
        //        {
        //            sDBKey = GetConfigOpt().GetConfigValue("DBKey");
        //        }
        //        if (mDBAccessColl == null)
        //        {
        //            mDBAccessColl = new Hashtable();
        //        }
        //        IDBAccess access = null;
        //        try
        //        {
        //            access = mDBAccessColl[sDBKey] as IDBAccess;
        //        }
        //        catch (Exception)
        //        {
        //            access = null;
        //        }
        //        if (access == null)
        //        {
        //            access = CreateDBAccess(sDBKey);
        //            if (access == null)
        //            {
        //                return null;
        //            }
        //            if (!access.Enabled)
        //            {
        //                return null;
        //            }
        //            mDBAccessColl.Add(sDBKey, access);
        //        }
        //        return access;
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function GetDBAccess\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return null;
        //    }
        //}

        //public static DBOpt GetDBOpt()
        //{
        //    try
        //    {
        //        if (mDBOpt == null)
        //        {
        //            mDBOpt = new DBOpt();
        //        }
        //        return mDBOpt;
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function GetDBOpt\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获取错误信息的操作
        /// </summary>
        /// <returns></returns>
        public static ErrorOpt GetErrorOpt()
        {
            try
            {
                if (mErrorOpt == null)
                {
                    mErrorOpt = new ErrorOpt();
                }
                return mErrorOpt;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function GetErrorOpt\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        /// <summary>
        /// 以下源代码被注销
        /// </summary>
        /// <param name="sDBKey"></param>
        //public static void ReSetDBAccess(string sDBKey)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(sDBKey))
        //        {
        //            sDBKey = GetConfigOpt().GetConfigValue("DBKey");
        //        }
        //        if (mDBAccessColl == null)
        //        {
        //            mDBAccessColl = new Hashtable();
        //        }
        //        if (mDBAccessColl.ContainsKey(sDBKey))
        //        {
        //            mDBAccessColl.Remove(sDBKey);
        //        }
        //        IDBAccess access = CreateDBAccess(sDBKey);
        //        mDBAccessColl.Add(sDBKey, access);
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show("错误类　 : Utilities.UtilFactory\n错误出处 : Function ReGetDBAccess\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}
    }
}

