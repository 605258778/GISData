using System;
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


namespace GISData
{
    public partial class FormMain : Form
    {
        private ESRI.ArcGIS.Controls.IMapControl3 m_mapControl = null;
        //TOCControl控件变量
        private ITOCControl2 m_tocControl = null;
        //TOCControl中Map菜单
        private IToolbarMenu m_menuMap = null;
        //TOCControl中图层菜单
        private IToolbarMenu m_menuLayer = null;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //在Form1_Load函数进行初始化，即菜单的创建：
            m_menuMap = new ToolbarMenuClass();
            //添加自定义菜单项到TOCCOntrol的图层菜单中
            m_menuLayer = new ToolbarMenuClass();

            m_tocControl = (ITOCControl2)this.axTOCControl1.Object;
            // 取得 MapControl 和 PageLayoutControl 的引用
            m_mapControl = (IMapControl3)this.axMapControl1.Object;
            //添加“移除图层”菜单项
            m_menuLayer.AddItem(new RemoveLayer(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
            //添加“放大到整个图层”菜单项
            m_menuLayer.AddItem(new ZoomToLayer(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);
            //查看属性表  
            m_menuLayer.AddItem(new OpenAttribute(this.axMapControl1), -1, 2, true, esriCommandStyles.esriCommandStyleTextOnly);
            //设置菜单的Hook
            m_menuLayer.SetHook(m_mapControl);
            m_menuMap.SetHook(m_mapControl);
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
            formDictionary.Show();
        }

        private void 数据注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister();
            formRegister.Show();
        }

        private void 质检配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigMain fcm = new FormConfigMain();
            fcm.Show();
        }

        private void 数据质检ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheckMain fcm = new FormCheckMain();
            fcm.Show();
        }

        private void 工程设置ToolStripMenuItem_Click(object sender, EventArgs e)
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
                Button bt = new Button();
                bt.Location = new System.Drawing.Point(500, 70 + 30 * 2 * (i + 1));
                bt.Text = "浏览";
                bt.Name = dr[i]["REG_ALIASNAME"].ToString() + "Add";
                bt.Click += (se, a) => addFile(tx);
                FormSetparaDig.Controls.Add(lb);
                FormSetparaDig.Controls.Add(tx);
                FormSetparaDig.Controls.Add(bt);
            }
            Button btok = new Button();
            btok.Location = new System.Drawing.Point(500, 70 + 30 * 2 * (dr.Length + 1));
            btok.Text = "确定";
            btok.Click += (se, a) => SelectOk(FormSetparaDig);
            FormSetparaDig.Controls.Add(btok);
            FormSetparaDig.ShowDialog();
        }
        //工程设置确定
        private void SelectOk(FormSetpara FormSetparaDig)
        {
            FormSetparaDig.Close();
        }
        //工程设置添加文件
        private void addFile(TextBox tx)
        {
            string GdbPath = Application.StartupPath + "\\GISData.gdb";
            IGxDialog dlg = new GxDialog();
            IGxObjectFilterCollection filterCollection = dlg as IGxObjectFilterCollection;
            filterCollection.AddFilter(new GxFilterFeatureClasses(),true);
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
                                FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                                IWorkspace workspace = fac.OpenFromFile(GdbPath, 0);
                                IFeatureClass pFc = pDataset as IFeatureClass;
                                IFeatureWorkspace pFeatureWorkspace = workspace as IFeatureWorkspace;
                                IEnumDatasetName datasetnames = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
                                IFeatureDatasetName datasetname = datasetnames.Next() as IFeatureDatasetName;
                                IFeatureDataConverter featureDataConverter = new FeatureDataConverterClass();
                                featureDataConverter.ConvertFeatureClass(pDataset.FullName as IFeatureClassName, null, datasetname, pDataset.FullName as IFeatureClassName, null, null, "", 0, 0);
                                tx.Text = pDataset.BrowseName;
                                tx.Name = pDataset.BrowseName;
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

    }
}
