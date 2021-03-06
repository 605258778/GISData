﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Data.OleDb;
using GISData.Common;
using GISData.Dictionary;
using GISData.DataRegister;
using GISData.ChekConfig;
using GISData.DataCheck;
using GISData.Parameter;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;
using GISData.TaskManage;
using GISData.Report;
using GISData.CheckBegin;
using GISData.User;


namespace GISData
{
    public partial class FormMain : Form
    {
        public ESRI.ArcGIS.Controls.IMapControl3 m_mapControl = null;
        //TOCControl控件变量
        private ITOCControl2 m_tocControl = null;
        //TOCControl中Map菜单
        private IToolbarMenu m_menuMap = null;
        //TOCControl中图层菜单
        private IToolbarMenu m_menuLayer = null;
        public IHookHelper m_hookHelper = new HookHelperClass(); 
        public FormMain()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LogHelper.WriteLog(typeof(FormMain), "窗体初始化开始");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //获取用户权限
            CommonClass common = new CommonClass();
            string user = common.GetConfigValue("USER");
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_USER WHERE USER ='" + user + "'");
            DataRow dr = dt.Select(null)[0];
            string ROLE = dr["ROLE"].ToString();
            if (ROLE == "超级管理员")
            {
                this.用户管理ToolStripMenuItem.Visible = true;
                this.报表设计ToolStripMenuItem.Visible = true;
                this.任务管理ToolStripMenuItem.Visible = true;
                this.质检配置ToolStripMenuItem.Visible = true;
                this.字典管理ToolStripMenuItem.Visible = true;
                this.数据注册ToolStripMenuItem.Visible = true;
            }
            else 
            {
                this.用户管理ToolStripMenuItem.Visible = false;
                this.报表设计ToolStripMenuItem.Visible = false;
                this.任务管理ToolStripMenuItem.Visible = false;
                this.质检配置ToolStripMenuItem.Visible = false;
                this.字典管理ToolStripMenuItem.Visible = false;
                this.数据注册ToolStripMenuItem.Visible = false;
            }
            try
            {
                //在Form1_Load函数进行初始化，即菜单的创建：
                m_menuMap = new ToolbarMenuClass();
                //添加自定义菜单项到TOCCOntrol的图层菜单中
                m_menuLayer = new ToolbarMenuClass();

                m_tocControl = (ITOCControl2)this.axTOCControl1.Object;
                // 取得 MapControl 和 PageLayoutControl 的引用
                m_mapControl = (IMapControl3)this.axMapControl1.Object;

                //这样就可以把AxMapControl传递给其它要用到的地方
                //添加“移除图层”菜单项
                m_menuLayer.AddItem(new RemoveLayer(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
                //添加“放大到整个图层”菜单项
                m_menuLayer.AddItem(new ZoomToLayer(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);
                //查看属性表  
                m_menuLayer.AddItem(new OpenAttribute(this.axMapControl1), -1, 2, true, esriCommandStyles.esriCommandStyleTextOnly);
                //设置菜单的Hook
                m_menuLayer.SetHook(m_mapControl);
                m_menuMap.SetHook(m_mapControl);
                m_hookHelper.Hook = this.axMapControl1.Object;
            }catch(Exception exc)
            {
                LogHelper.WriteLog(typeof(FormMain), exc);
            }
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            //如果不是右键按下直接返回
            if (e.button != 2) return;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer layer = null;
            object other = null;
            object index = null;
            //判断所选菜单的类型
            m_tocControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            //确定选定的菜单类型，Map或是图层菜单
            if (item == esriTOCControlItem.esriTOCControlItemMap)
                m_tocControl.SelectItem(map, null);
            else
                m_tocControl.SelectItem(layer, null);
            //设置CustomProperty为layer (用于自定义的Layer命令)                  
            m_mapControl.CustomProperty = layer;
            //弹出右键菜单
            if (item == esriTOCControlItem.esriTOCControlItemMap)
                m_menuMap.PopupMenu(e.x, e.y, m_tocControl.hWnd);
            if (item == esriTOCControlItem.esriTOCControlItemLayer)
                m_menuLayer.PopupMenu(e.x, e.y, m_tocControl.hWnd);

            
        }

        private void 字典管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDictionary formDictionary = new FormDictionary();
            formDictionary.Show(this);
        }

        private void 数据注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister();
            formRegister.Show(this);
        }

        private void 质检配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigMain fcm = new FormConfigMain();
            fcm.Show(this);
        }

        private void 数据质检ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheckMain fcm = new FormCheckMain(m_hookHelper);
            fcm.Show(this);
        }

        private void 工程设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSetpara();
        }
        /// <summary>
        /// 加载工程设置界面
        /// </summary>
        public void ShowSetpara() 
        {
            FormSetpara FormSetparaDig = new FormSetpara();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_REGINFO");
            DataRow[] dr = dt.Select("1=1");
            for (int i = 0; i < dr.Length; i++)
            {
                Label lb = new Label();
                lb.Location = new System.Drawing.Point(75, 70 + 60 * (i + 1) - 30);
                lb.Text = dr[i]["REG_ALIASNAME"].ToString() + ":";
                lb.Name = "lable" + i.ToString();
                lb.AutoSize = true;
                TextBox tx = new TextBox();
                tx.Location = new System.Drawing.Point(75, 70 + 30 * 2 * (i + 1));
                tx.Size = new System.Drawing.Size(400, 25);
                tx.Tag = dr[i]["ID"].ToString();
                tx.Name = dr[i]["REG_ALIASNAME"].ToString();
                string tablename = dr[i]["REG_NAME"].ToString();
                Button bt = new Button();
                bt.Location = new System.Drawing.Point(500, 70 + 30 * 2 * (i + 1));
                bt.Text = "浏览";
                bt.Name = dr[i]["REG_ALIASNAME"].ToString() + "Add";
                bt.Click += (se, a) => addFile(tx, tablename);
                FormSetparaDig.Controls.Add(lb);
                FormSetparaDig.Controls.Add(tx);
                FormSetparaDig.Controls.Add(bt);
            }
            Button btok = new Button();
            btok.Location = new System.Drawing.Point(410, 70 + 30 * 2 * (dr.Length + 1));
            btok.Text = "确定";
            btok.Click += (se, a) => SelectOk(FormSetparaDig);
            Button btok1 = new Button();
            btok1.Location = new System.Drawing.Point(500, 70 + 30 * 2 * (dr.Length + 1));
            btok1.Text = "下一步";
            btok1.Click += (se, a) => SelectNext(FormSetparaDig);
            FormSetparaDig.Controls.Add(btok);
            FormSetparaDig.Controls.Add(btok1);
            FormSetparaDig.Show(this);
        }
        //工程设置确定
        private void SelectOk(FormSetpara FormSetparaDig)
        {
            FormSetparaDig.Close();
        }//工程设置确定
        private void SelectNext(FormSetpara FormSetparaDig)
        {
            FormSetparaDig.Close();
            FormCheckMain fcm = new FormCheckMain(m_hookHelper);
            fcm.Show(this);
        }
        //工程设置添加文件
        private void addFile(TextBox tx,string tablename)
        {
            try 
            {
                IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();

                IGxDialog dlg = new GxDialog();
                IGxObjectFilterCollection filterCollection = dlg as IGxObjectFilterCollection;
                filterCollection.AddFilter(new GxFilterFeatureClasses(), true);
                IEnumGxObject enumObj;
                dlg.AllowMultiSelect = true;
                dlg.Title = "添加数据";
                dlg.DoModalOpen(0, out enumObj);
                if (enumObj != null)
                {
                    enumObj.Reset();
                    int a = 0;
                    IGxObject gxObj = enumObj.Next();
                    while (gxObj != null)
                    {
                        Console.WriteLine(a++);
                        if (gxObj is IGxDataset)
                        {
                            IGxDataset gxDataset = gxObj as IGxDataset;
                            IDataset pDataset = gxDataset.Dataset;
                            switch (pDataset.Type)
                            {
                                case esriDatasetType.esriDTFeatureClass:
                                    IFeatureClass pFc = pDataset as IFeatureClass;

                                    ISpatialReference pSpatialReference = (pFc as IGeoDataset).SpatialReference;//空间参考
                                    CommonClass common = new CommonClass();
                                    if (pSpatialReference.Name != common.GetConfigValue("SpatialReferenceName"))
                                    {
                                        MessageBox.Show("空间参考错误");
                                        break;
                                    }
                                    else
                                    {
                                        string layerType = "";
                                        if (pDataset.Category.Contains("个人地理数据库") || pDataset.Category.Contains("Personal"))
                                        {
                                            layerType = "Access数据库";
                                        }
                                        else if (pDataset.Category.Contains("文件地理数据库") || pDataset.Category.Contains("File"))
                                        {
                                            layerType = "文件夹数据库";
                                        }

                                        IFields fields = pFc.Fields;
                                        Dictionary<string, List<string>> dicCustom = new Dictionary<string, List<string>>();
                                        Dictionary<string, List<string>> dicSys = new Dictionary<string, List<string>>();

                                        ConnectDB db = new ConnectDB();
                                        DataTable dt = db.GetDataBySql("select FIELD_NAME,DATA_TYPE,MAXLEN from GISDATA_MATEDATA where REG_NAME  = '" + tablename + "'");
                                        DataRow[] dr = dt.Select(null);

                                        string errorString = "";

                                        for (int i = 0; i < dr.Length; i++)
                                        {
                                            string FIELD_NAME = dr[i]["FIELD_NAME"].ToString();
                                            string DATA_TYPE = dr[i]["DATA_TYPE"].ToString();
                                            string MAXLEN = dr[i]["MAXLEN"].ToString();
                                            List<string> list1 = new List<string>();
                                            list1.Add(DATA_TYPE);
                                            list1.Add(MAXLEN);
                                            dicSys.Add(FIELD_NAME, list1);
                                        }

                                        for (int i = 0; i < fields.FieldCount; i++)
                                        {
                                            IField field = fields.get_Field(i);
                                            if (field.Name != pFc.ShapeFieldName && field.Name != pFc.OIDFieldName)
                                            {
                                                List<string> list1 = new List<string>();
                                                list1.Add(field.Type.ToString());
                                                list1.Add(field.Length.ToString());
                                                dicCustom.Add(field.Name.ToString(), list1);
                                                if (dicSys.ContainsKey(field.Name))
                                                {
                                                    if (dicSys[field.Name][0] != field.Type.ToString())
                                                    {
                                                        errorString += "字段类型错误：" + field.Name + "(" + dicSys[field.Name][0] + ")；\r\n";
                                                    }
                                                    else if (dicSys[field.Name][1] != field.Length.ToString())
                                                    {
                                                        errorString += "字段长度错误：" + field.Name + "(" + dicSys[field.Name][0] + ")；\r\n";
                                                    }
                                                }
                                                else
                                                {
                                                    errorString += "多余字段：" + field.Name + "；\r\n";
                                                }
                                            }
                                        }

                                        foreach (KeyValuePair<string, List<string>> itemList in dicSys)
                                        {
                                            if (!dicCustom.ContainsKey(itemList.Key))
                                            {
                                                errorString += "缺少字段：" + itemList.Key + "；\r\n";
                                            }
                                        }

                                        if (errorString != "")
                                        {
                                            FormMessage message = new FormMessage("提示", errorString);
                                            message.ShowDialog();
                                            //MessageBox.Show(errorString);
                                        }
                                        else
                                        {
                                            string path = pDataset.Workspace.PathName.Replace(@"\" , @"\\");
                                            Console.WriteLine(path);
                                            Boolean result = db.Update("update GISDATA_REGINFO set PATH= '" + path + "',DBTYPE = '" + layerType + "',TABLENAME = '" + pDataset.Name + "' where REG_NAME = '" + tablename + "'");
                                            common.updateXmlNode(tablename, path, layerType, pDataset.Name);
                                            tx.Text = pDataset.BrowseName;
                                            tx.Name = pDataset.BrowseName;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (gxObj is IGxLayer)
                        {
                            IGxLayer gxLayer = gxObj as IGxLayer;
                            ILayer pLayer = gxLayer.Layer;
                            break;
                            //do anything you like
                        }
                        gxObj = enumObj.Next();
                    }
                }
            }
            catch(Exception e)
            {
                LogHelper.WriteLog(typeof(FormMain), e);
            }
        }
        //工程设置添加文件
        private void addFile11(TextBox tx, string tablename)
        {
            IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
            string GdbPath = Application.StartupPath + "\\GISData.gdb";
            string MdbPath = Application.StartupPath + "\\GISData.mdb";
            IWorkspaceFactory pWks = new AccessWorkspaceFactoryClass();
            IWorkspace pFwk = pWks.OpenFromFile(MdbPath, 0) as IWorkspace;

            IGxDialog dlg = new GxDialog();
            IGxObjectFilterCollection filterCollection = dlg as IGxObjectFilterCollection;
            filterCollection.AddFilter(new GxFilterFeatureClasses(), true);
            IEnumGxObject enumObj;
            dlg.AllowMultiSelect = true;
            dlg.Title = "添加数据";
            dlg.DoModalOpen(0, out enumObj);
            if (enumObj != null)
            {
                enumObj.Reset();
                int a = 0;
                IGxObject gxObj = enumObj.Next();
                while (gxObj != null)
                {
                    Console.WriteLine(a++);
                    if (gxObj is IGxDataset)
                    {
                        IGxDataset gxDataset = gxObj as IGxDataset;
                        IDataset pDataset = gxDataset.Dataset;
                        switch (pDataset.Type)
                        {
                            case esriDatasetType.esriDTFeatureClass:
                                IFeatureClass pFc = pDataset as IFeatureClass;

                                ISpatialReference pSpatialReference = (pFc as IGeoDataset).SpatialReference;//空间参考
                                CommonClass common = new CommonClass();
                                if (pSpatialReference.Name != common.GetConfigValue("SpatialReferenceName"))
                                {
                                    MessageBox.Show("空间参考错误");
                                    break;
                                }
                                else
                                {
                                    tx.Text = pDataset.BrowseName;
                                    tx.Name = pDataset.BrowseName;
                                }
                                break;
                            case esriDatasetType.esriDTFeatureDataset:
                                IFeatureDataset pFeatureDs = pDataset as IFeatureDataset;
                                //do anyting you like
                                break;
                            case esriDatasetType.esriDTRasterDataset:
                                IRasterDataset rasterDs = pDataset as IRasterDataset;
                                //do anyting you like
                                break;
                            case esriDatasetType.esriDTTable:
                                ITable pTable = pDataset as ITable;
                                //do anyting you like
                                break;
                            case esriDatasetType.esriDTTin:
                                ITin pTin = pDataset as ITin;
                                //do anyting you like
                                break;
                            case esriDatasetType.esriDTRasterCatalog:
                                IRasterCatalog pCatalog = pDataset as IRasterCatalog;
                                //do anyting you like
                                break;
                            default:
                                break;
                        }
                    }
                    else if (gxObj is IGxLayer)
                    {
                        IGxLayer gxLayer = gxObj as IGxLayer;
                        ILayer pLayer = gxLayer.Layer;
                        break;
                        //do anything you like
                    }
                    gxObj = enumObj.Next();
                }
            }
        }
        private void axLicenseControl1_Enter(object sender, EventArgs e)
        {

        }

        private void 任务管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTaskManage taskManage = new FormTaskManage();
            taskManage.Show(this);
        }

        private void 报表设计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormReportDesign report = new FormReportDesign();
            //FormReportDes report = new FormReportDes();
            //report.Show(this);
        }

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBegin begin = new FormBegin();
            begin.Show(this);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReportDes report = new FormReportDes();
            report.Show(this);
        }

        private void 文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReportWord word = new FormReportWord();
            word.Show(this);
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormValidation validation = new FormValidation();
            validation.Show(this);
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Formtest test = new Formtest();
            test.Show(this);
        }

    }
}
