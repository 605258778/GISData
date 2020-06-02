using ESRI.ArcGIS.esriSystem;
using GISData.Common;
using GISData.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GISData
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);
            //ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            //IAoInitialize aoInitialize = new AoInitialize();
            //esriLicenseStatus licenseStatus = esriLicenseStatus.esriLicenseUnavailable;
            //licenseStatus = aoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
            //if (licenseStatus == esriLicenseStatus.esriLicenseNotInitialized)
            //{
            //    MessageBox.Show("没有esriLicenseProductCodeArcInfo许可！");
            //    Application.Exit();
            //}

            FormLogin login = new FormLogin();
            LogHelper.WriteLog(typeof(Program), "登录");
            DialogResult result = login.ShowDialog();
            if (result == DialogResult.OK)
            {
                LogHelper.WriteLog(typeof(Program), "开始运行");
                try 
                {
                    Application.Run(new FormMain());
                }
                catch(Exception exc)
                {
                    LogHelper.WriteLog(typeof(Program), exc);
                }
            }
            //Application.Run(new FormMain());
        }
    }
}
