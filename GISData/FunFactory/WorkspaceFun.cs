namespace FunFactory
{
    using ESRI.ArcGIS.DataSourcesFile;
    using ESRI.ArcGIS.DataSourcesGDB;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// 工作空间类
    /// </summary>
    public class WorkspaceFun
    {
        private const string mClassName = "FunFactory.WorkspaceFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        /// <summary>
        /// 项目的工作空间接口
        /// </summary>
        internal WorkspaceFun()
        {
        }

        public bool CompactPersonalGeodatabase(string sPGDBFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sPGDBFile) | !File.Exists(sPGDBFile))
                {
                    MessageBox.Show("数据文件 " + sPGDBFile + " 丢失。", "压缩数据错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                bool flag = false;
                IWorkspaceFactory factory = null;
                factory = new AccessWorkspaceFactoryClass();
                IWorkspace workspace = null;
                workspace = factory.OpenFromFile(sPGDBFile, 0);
                if ((workspace != null) && (workspace is IDatabaseCompact))
                {
                    IDatabaseCompact compact = null;
                    compact = workspace as IDatabaseCompact;
                    if (compact.CanCompact())
                    {
                        compact.Compact();
                        flag = true;
                    }
                }
                workspace = null;
                factory = null;
                return flag;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.WorkspaceFun", "CompactPersonalGeodatabase", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public IFeatureWorkspace GetFeatureWorkspace(WorkspaceSource pSourceType)
        {
            try
            {
                if (pSourceType != WorkspaceSource.esriWSSdeWorkspaceFactory)
                {
                    return null;
                }
                IWorkspace workspace = null;
                string serr = "";
                string server = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "DataSource");
                string instance = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "Service");
                string database = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "InitialCatalog");
                string version = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "Version");
                string user = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "UserID");
                string password = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "Password");
                workspace = this.open_ArcSDE_Workspace(server, instance, user, password, database, version, out serr);
                if (workspace == null)
                {
                    MessageBox.Show("服务名、服务器名、数据库名、用户名或密码输入错误", "数据源链接失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                return (workspace as IFeatureWorkspace);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.WorkspaceFun", "GetFeatureWorkspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        /// <summary>
        /// 获取初始化后的各种工作空间
        /// </summary>
        /// <param name="sSourceFile"></param>
        /// <param name="pSourceType"></param>
        /// <returns></returns>
        public IFeatureWorkspace GetFeatureWorkspace(string sSourceFile, WorkspaceSource pSourceType)
        {
            try
            {
                if ((string.IsNullOrEmpty(sSourceFile) | !File.Exists(sSourceFile)) & (string.IsNullOrEmpty(sSourceFile) | !Directory.Exists(sSourceFile)))
                {
                    MessageBox.Show("数据文件 " + sSourceFile + " 丢失。", "打开工作空间错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                IWorkspaceFactory factory = null;
                WorkspaceSource source = pSourceType;
                if (source <= WorkspaceSource.esriWSStreetMapWorkspaceFactory)
                {
                    if (source < WorkspaceSource.esriWSAccessWorkspaceFactory)
                    {
                        goto Label_010D;//当source<1.返回空
                    }
                    switch (((int)(source - 1L)))
                    {
                        case 0:
                            factory = new AccessWorkspaceFactoryClass();
                            goto Label_0111;

                        case 1:
                        case 4:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                            goto Label_010D;

                        case 2:
                            factory = new ArcInfoWorkspaceFactoryClass();
                            goto Label_0111;

                        case 3:
                            factory = new CadWorkspaceFactoryClass();
                            goto Label_0111;

                        case 6:
                            factory = new PCCoverageWorkspaceFactoryClass();
                            goto Label_0111;

                        case 9:
                            factory = new SDCWorkspaceFactoryClass();
                            goto Label_0111;

                        case 11:
                            factory = new ShapefileWorkspaceFactoryClass();
                            goto Label_0111;

                        case 12:
                            factory = new StreetMapWorkspaceFactoryClass() as IWorkspaceFactory;
                            goto Label_0111;
                    }
                }
                if ((source <= WorkspaceSource.esriWSFileGDBWorkspaceFactory) && (source >= WorkspaceSource.esriWSVpfWorkspaceFactory))
                {
                    switch (((int)(source - 0x11L)))
                    {
                        case 0:
                            factory = new VpfWorkspaceFactoryClass();
                            goto Label_0111;

                        case 1:
                            factory = new FileGDBWorkspaceFactoryClass();
                            goto Label_0111;
                    }
                }
            Label_010D:
                return null;//返回空
            Label_0111:
                if (!factory.IsWorkspace(sSourceFile))
                {
                    return null;
                }
                return (factory.OpenFromFile(sSourceFile, 0) as IFeatureWorkspace);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.WorkspaceFun", "GetFeatureWorkspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IFeatureWorkspace GetFeatureWorkspace(WorkspaceSource pSourceType, string sDataSource, string sService, string sInitialCatalog, string sVersion, string sUserID, string sPassword, bool bShowErr)
        {
            try
            {
                if (pSourceType != WorkspaceSource.esriWSSdeWorkspaceFactory)
                {
                    return null;
                }
                IWorkspace workspace = null;
                string serr = "";
                string server = sDataSource;
                string instance = sService;
                string database = sInitialCatalog;
                string version = sVersion;
                string user = sUserID;
                string password = sPassword;
                workspace = this.open_ArcSDE_Workspace(server, instance, user, password, database, version, out serr);
                if (workspace == null)
                {
                    if (bShowErr)
                    {
                        MessageBox.Show("服务名、服务器名、数据库名、用户名或密码输入错误", "数据源链接失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return null;
                }
                return (workspace as IFeatureWorkspace);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.WorkspaceFun", "GetFeatureWorkspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IFeatureWorkspace GetSdeFeatureWorkspace(string version)
        {
            try
            {
                IWorkspace workspace = null;
                string serr = "";
                string server = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "DataSource");
                string instance = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "Service");
                string database = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "InitialCatalog");
                if (version == "")
                {
                    version = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "Version");
                }
                string user = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "UserID");
                string password = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "Password");
                workspace = this.open_ArcSDE_Workspace(server, instance, user, password, database, version, out serr);
                if (workspace == null)
                {
                    MessageBox.Show("服务名、服务器名、数据库名、用户名或密码输入错误", "数据源链接失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                return (workspace as IFeatureWorkspace);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.WorkspaceFun", "GetFeatureWorkspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        private IWorkspace open_ArcSDE_Workspace(string server, string instance, string user, string password, string database, string version, out string serr)
        {
            try
            {
                ///IPropertySet接口提供对用于管理PropertySet的成员的访问。IPropertySet接口包含在PropertySet中设置和检索命名值对集合的方法。
                ///PropertySet是一个通用类，用于保存任何东西的一组属性。使用属性集的一个示例是保存打开SDE工作空间所需的属性，如示例代码所示。
                ///通常，属性集可以被认为是一组键（字符串）和值（变体/对象）。一个值得注意的例外是在XmlPropertySet对象上使用IPropertySet接口。 XML文档可以包含具有相同名称的多个元素（即“属性/子”）和不同的值。因此，此接口从XmlPropertySet返回的值可能是锯齿状的二维数组（包含其他数组的数组）。有关更多详细信息和代码示例，请参阅Geodatabase库中的XmlPropertySet coclass'文档。
                IPropertySet connectionProperties = new PropertySetClass();
                connectionProperties.SetProperty("SERVER", server);
                connectionProperties.SetProperty("INSTANCE", instance);
                connectionProperties.SetProperty("DATABASE", database);
                connectionProperties.SetProperty("USER", user);
                connectionProperties.SetProperty("PASSWORD", password);
                connectionProperties.SetProperty("VERSION", version);
                IWorkspaceFactory factory = new SdeWorkspaceFactoryClass();
                IWorkspace workspace = factory.Open(connectionProperties, 0);
                serr = "";
                return workspace;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.WorkspaceFun", "open_ArcSDE_Workspace", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                serr = exception.Message;
                return null;
            }
        }
    }
}

