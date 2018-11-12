using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataRegister
{
    public partial class FormDBConnectInfo : Form
    {
        public FormDBConnectInfo()
        {
            InitializeComponent();
        }
        //选择路径
        private void buttonPathSelect_Click(object sender, EventArgs e)
        {
            string selectPath = "";
            if (comboBoxConType.SelectedItem == "Access数据库")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Access数据库(*.mdb)|*.mdb|所有文件|*.*";
                ofd.ValidateNames = true;
                ofd.CheckPathExists = true;
                ofd.CheckFileExists = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectPath = ofd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择文件路径";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    selectPath = fbd.SelectedPath;
                }
            }
            this.textBoxConPath.Text = selectPath;
        }
        //确定添加连接
        private void buttonConOK_Click(object sender, EventArgs e)
        {
            GetAllFeature gaf = new GetAllFeature();
            ConnectDB cd = new ConnectDB();
            string ConName = this.textBoxConName.Text;
            string ConType = this.comboBoxConType.SelectedItem.ToString();
            string ConPath = this.textBoxConPath.Text;
            //插入信息到GISDATA_REGCONNECT
            bool isInsert = cd.Insert("insert into GISDATA_REGCONNECT (REG_NAME,REG_TYPE,REG_PATH) values ('" + ConName + "','" + ConType + "','" + ConPath + "',)");
            if (isInsert) 
            {
                DataTable dt = cd.GetDataBySql("select * from GISDATA_REGCONNECT");
                DataRow[] dr = dt.Select("1=1");
                for (int i = 0; i < dr.Length; i++)
                {
                    string path = dr[i]["sad"].ToString();
                }
            }
        }
    }
}
