namespace Utilities
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// 项目的配置xml文件操作类
    /// </summary>
    public class ConfigOpt
    {
        /// <summary>
        /// mClassName = "Utilities.ConfigOpt"不能更改。
        /// const修饰的数据类型是指常类型,常类型的变量或对象的值是不能被更新的。
        /// </summary>
        private const string mClassName = "Utilities.ConfigOpt";
        /// <summary>
        /// 定义XML文档
        /// </summary>
        private static XmlDocument mXmlDocument;
        /// <summary>
        /// XML配置文档路径
        /// </summary>
        private static string mXmlPath;

        /// <summary>
        /// 项目的配置xml文件操作接口。
        /// 在此接口中判断是否可以打开XML配置文件。
        /// </summary>
        internal ConfigOpt()
        {
            try
            {
                mXmlDocument = null;
                if (!this.OpenXML())
                {
                    //MessageBox.Show("读取系统配置文件错误,请确认文件AppConfig.xml是否存在并正确。", "Utilities.ConfigOpt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ConfigOpt\n错误出处 : Sub New\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 创建XML文档的配置信息
        /// </summary>
        /// <returns></returns>
        private XmlDocument CreateConfig()
        {
            try
            {
                XmlDocument document = new XmlDocument();
                Assembly assembly = Assembly.GetAssembly(typeof(ConfigOpt));
                assembly.GetManifestResourceNames();
                ResourceManager manager = null;
                string name = "AppConfig";
                manager = new ResourceManager("Utilities.Resource", assembly);
                document.LoadXml(manager.GetObject(name).ToString());
                mXmlDocument = document;
                this.SaveXMl();
                return document;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取配置XML文件的值。
        /// 从FormLogin4传入的Name值为（LoginUserID）
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string GetConfigValue(string Name)
        {
            try
            {
                if (mXmlDocument == null)
                {
                    return "";
                }
                //Config为父节点，也是XML文档的根节点
                XmlElement element = mXmlDocument.GetElementsByTagName("Config").Item(0) as XmlElement;
                //GetElementsByTagName:返回一个System.Xml.XmlNodeList，其中包含与指定的System.Xml.XmlElement.Name匹配的所有后代元素的列表。
                XmlElement element2 = element.GetElementsByTagName(Name).Item(0) as XmlElement;
                if (element2 == null)
                {
                    return "";
                }
                return element2.InnerXml;//InnerXml:获取或设置只表示此节点子级的标记。
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ConfigOpt\n错误出处 : GetConfigValue\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
            }
        }

        /// <summary>
        /// 获取XML配置文件的值方法2
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ChiName"></param>
        /// <returns></returns>
        public string GetConfigValue2(string Name, string ChiName)
        {
            try
            {
                if (mXmlDocument == null)
                {
                    return "";
                }
                XmlElement element = mXmlDocument.GetElementsByTagName("Config").Item(0) as XmlElement;
                XmlElement element2 = element.GetElementsByTagName(Name).Item(0) as XmlElement;
                if (element2 == null)
                {
                    return "";
                }
                if (string.IsNullOrEmpty(ChiName))
                {
                    return element2.InnerXml;
                }
                XmlElement element3 = element2.GetElementsByTagName(ChiName).Item(0) as XmlElement;
                if (element3 == null)
                {
                    return "";
                }
                return element3.InnerXml;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ConfigOpt\n错误出处 : GetConfigValue2\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
            }
        }

        /// <summary>
        /// 获取父目录
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private DirectoryInfo GetParentDirectory(DirectoryInfo pt, string name)
        {
            try
            {
                if (pt.Parent.GetDirectories(name).Length > 0)
                {
                    return pt.Parent;
                }
                return this.GetParentDirectory(pt.Parent, name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 定义项目的文件路径（默认在bin/Debug中）
        /// </summary>
        private string m_rootPath;

        /// <summary>
        /// 获取项目的文件路径（默认在bin/Debug中）
        /// </summary>
        public string RootPath
        {
            get { return   m_rootPath;}
            set
        {
            m_rootPath = MakeRootPath(value);
        }
        }
        private string MakeRootPath(string path)
        {
           int index= path.LastIndexOf('\\');
           return path.Substring(0, index+1);
        }

        /// <summary>
        /// 获取XML文档里的项目系统名称
        /// </summary>
        /// <returns></returns>
        public string GetSystemName()
        {
            try
            {
                if (mXmlDocument == null)
                {
                    return "";
                }
                XmlElement element = null;
                element = mXmlDocument.GetElementsByTagName("Config").Item(0) as XmlElement;
                return element.GetElementsByTagName("SystemName").Item(0).InnerXml;
            }
            catch (Exception exception)
            {
                MessageBox.Show("错误类　 : Utilities.ConfigOpt\n错误出处 : GetSystemName\n错误来源 : " + exception.Source + "\n错误描述 : " + exception.Message, "程序运行错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
            }
        }

        /// <summary>
        /// 判断是否可以打开XML文件
        /// </summary>
        /// <returns></returns>
        private bool OpenXML()
        {
            try
            {
                //DirectoryInfo类用于创建、移动和枚举目录和子目录的实例方法。
                DirectoryInfo pt = new DirectoryInfo(Application.StartupPath);
                pt = this.GetParentDirectory(pt, "Config");
                mXmlPath = pt.FullName + @"\Config\AppConfig.xml";
                if (mXmlDocument == null)
                {
                    mXmlDocument = new XmlDocument();
                }
                mXmlDocument.Load(mXmlPath);
                this.SetConfigValue("AppPath", pt.FullName);
                return true;
            }
            catch (Exception)
            {
                mXmlDocument = null;
                return false;
            }
        }

        /// <summary>
        /// 判断是否保存XML文档：真保存并返回真。假不保存并返回假
        /// </summary>
        /// <returns></returns>
        private bool SaveXMl()
        {
            try
            {
                if (mXmlDocument == null)
                {
                    return false;
                }
                DirectoryInfo pt = new DirectoryInfo(Application.StartupPath);
                mXmlPath = this.GetParentDirectory(pt, "Config").FullName + @"\Config\AppConfig.xml";
                mXmlDocument.Save(mXmlPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置配置文件的属性名称（处于Config父节点的子节点上）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        private bool SetConfigAttributeName(string Name, string Value)
        {
            try
            {
                if (mXmlDocument == null)
                {
                    this.OpenXML();
                }
                if (mXmlDocument == null)
                {
                    return false;
                }
                XmlElement element = mXmlDocument.GetElementsByTagName("Config").Item(0) as XmlElement;
                element.GetElementsByTagName(Name).Item(0);
                element.SetAttribute("Name", Value);
                return this.SaveXMl();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置配置的值方法1。参数（Name，Value）。
        /// FormLogin4向XML文档传递的值为（"LoginUserID"，"admin"）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool SetConfigValue(string Name, string Value)
        {
            try
            {
                if (mXmlDocument == null)
                {
                    return false;
                }
                XmlElement element = mXmlDocument.GetElementsByTagName("Config").Item(0) as XmlElement;
                XmlElement element2 = element.GetElementsByTagName(Name).Item(0) as XmlElement;
                if (element2 == null)
                {
                    return false;
                }
                element2.InnerXml = Value;
                return this.SaveXMl();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置配置的值方法2。参数（Name，ChiName，Value）。
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ChiName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool SetConfigValue2(string Name, string ChiName, string Value)
        {
            try
            {
                if (mXmlDocument == null)
                {
                    return false;
                }
                XmlElement element = mXmlDocument.GetElementsByTagName("Config").Item(0) as XmlElement;
                XmlElement element2 = element.GetElementsByTagName(Name).Item(0) as XmlElement;
                if (element2 == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(ChiName))
                {
                    return false;
                }
                XmlElement element3 = element2.GetElementsByTagName(ChiName).Item(0) as XmlElement;
                if (element3 == null)
                {
                    return false;
                }
                element3.InnerXml = Value;
                return this.SaveXMl();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置节点名称
        /// </summary>
        /// <param name="pElement"></param>
        /// <param name="NameValue"></param>
        /// <returns></returns>
        private bool SetNodeName(XmlElement pElement, string NameValue)
        {
            pElement.SetAttribute("Name", NameValue);
            this.SaveXMl();
            return true;
        }

        /// <summary>
        /// 设置节点值
        /// </summary>
        /// <param name="pElement"></param>
        /// <param name="NodeValue"></param>
        /// <returns></returns>
        private bool SetNodeValue(XmlElement pElement, string NodeValue)
        {
            pElement.InnerXml = NodeValue;
            return this.SaveXMl();
        }
    }
}

