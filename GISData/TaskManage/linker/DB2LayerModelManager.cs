using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using td.db.mid.sys;
using td.db.orm;
using td.logic.sys;
using System.Windows.Forms;
using Utilities;

namespace td.forest.task.linker
{
    /// <summary>
    /// DB2数据库图层模型管理类
    /// </summary>
    public class DB2LayerModelManager
    {
        /// <summary>
        /// 图层管理类：获取DB数据库Server服务
        /// </summary>
        private LayerManager LM
        {
            get
            {
                return DBServiceFactory<LayerManager>.Service;
            }
        }

        /// <summary>
        /// 路径管理类
        /// </summary>
        public PathManager PathManager
        {
            get
            {
                return DBServiceFactory<PathManager>.Service;
            }
        }
        public string CartoTemplatePath
        {
            get
            {
                return UtilFactory.GetConfigOpt().RootPath + @"\" + PathManager.FindValue(CartoTemplatePath) + @"\";
            }
        }

        /// <summary>
        /// 判断检查数据源路径是否合法
        /// </summary>
        /// <param name="skey"></param>
        /// <param name="spath"></param>
        /// <returns></returns>
        private bool CheckDataSource(string skey, string spath)
        {
            try
            {
                // 提供创建、复制、删除、移动和打开文件的实例方法，并且帮助创建 System.IO.FileStream 对象。此类不能被继承。
                FileInfo info = new FileInfo(spath);
                //spath=template/年度小班.mxd
                if (info.Exists)
                {
                    string configValue = PathManager.FindValue(skey);
                    //string str2 = PathManager.FindValue2("SqlServer", "DataSource");
                    //string str3 = PathManager.FindValue2("SqlServer", "InitialCatalog");
                    //if (configValue == (str2 + "," + str3))
                    //{
                    //    return true;
                    //}
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Tuple<bool, string> GetSavePath(string mEditKind, bool bDefaultPath, string sMxdPath)
        {         
            if (mEditKind == "造林")
            {
                if (PathManager.FindValue("EditMapZLPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                   
                }
                sMxdPath = RootPath + @"\" + PathManager.FindValue("EditMapZLPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "采伐")
            {
                if (PathManager.FindValue("EditMapCFPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                   
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapCFPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "火灾")
            {
                if (PathManager.FindValue("EditMapHZPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapHZPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "征占用")
            {
                if (PathManager.FindValue("EditMapZZYPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                    
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapZZYPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "林业工程")
            {
                if (PathManager.FindValue("EditMapGCPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                  
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapGCPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "林业案件")
            {
                if (PathManager.FindValue("EditMapAJPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapAJPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "自然灾害")
            {
                if (PathManager.FindValue("EditMapZHPath2").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                   
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapZHPath2");
                bDefaultPath = false;
            }
            else if (mEditKind == "小班变更")
            {
                if (PathManager.FindValue("EditMapXBPath3").Trim() == "")
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                   
                }
                sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath3");
                bDefaultPath = false;
            }
            else if (mEditKind == "年度小班")
            {
                if (PathManager.FindValue("EditMapXBPath4").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath4");
                    bDefaultPath = false;
                }
                else
                {
                    MessageBox.Show("无已保存工作空间", "提示", MessageBoxButtons.OK);
                  
                }
            }
            return new Tuple<bool, string>(bDefaultPath, sMxdPath);
        }

        public void SavePath(string mEditKind)
        {
                   string name = "";
                string str3 = "";
                if (mEditKind == "造林")
                {
                    name = "QueryMapZLPath2";
                    str3 = "QueryMapZLDataSource";
                }
                if (mEditKind == "采伐")
                {
                    name = "QueryMapCFPath2";
                    str3 = "QueryMapCFDataSource";
                }
                if (mEditKind == "火灾")
                {
                    name = "QueryMapHZPath2";
                    str3 = "QueryMapHZDataSource";
                }
                if (mEditKind == "征占用")
                {
                    name = "QueryMapZZYPath2";
                    str3 = "QueryMapZZYDataSource";
                }
                if (mEditKind == "林业工程")
                {
                    name = "QueryMapGCPath2";
                    str3 = "QueryMapGCDataSource";
                }
                if (mEditKind == "林业案件")
                {
                    name = "QueryMapAJPath2";
                    str3 = "QueryMapAJDataSource";
                }
                if (mEditKind == "自然灾害")
                {
                    name = "QueryMapZHPath2";
                    str3 = "QueryMapZHDataSource";
                }
                if (mEditKind == "小班变更")
                {
                    name = "QueryMapXBPath3";
                    str3 = "QueryMapXBDataSource";
                }
                if (mEditKind == "年度小班")
                {
                    name = "QueryMapXBPath4";
                    str3 = "QueryMapXBDataSource2";
                }
                if (name != "")
                {
                  
                    PathManager.SaveValue(name, @"Template\" + mEditKind + ".mxd");
                }
                string str4 = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "DataSource");
                string str5 = UtilFactory.GetConfigOpt().GetConfigValue2("SqlServer", "InitialCatalog");
                if (str3 != "")
                {                    
                    PathManager.SaveValue(str3, str4 + "," + str5);
                }
        }

        /// <summary>
        /// 获取和设置源文件路径在（bin/DeBug中）
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// 获取打开文件
        /// </summary>
        /// <param name="mEditKind"></param>
        /// <param name="sMxdPath"></param>
        /// <param name="bDefaultPath"></param>
        /// <returns></returns>
        public Tuple<bool, string> GetOpenFile(string mEditKind, string sMxdPath, bool bDefaultPath)
        {
            if (mEditKind == "造林")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapZLPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "采伐")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapCFPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "火灾")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapHZPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "征占用")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapZZYPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "林业工程")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapGCPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "林业案件")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapAJPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "自然灾害")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapZHPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "小班变更")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapXBPath");
                bDefaultPath = true;
            }
            else if (mEditKind == "年度小班")
            {
                sMxdPath = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditMapXBPath2");
                bDefaultPath = true;
            }
            return new Tuple<bool, string>(bDefaultPath, sMxdPath);
        }
        /// <summary>
        /// 准备加载图层的路径信息。参数（mEditKind 传入的信息：有造林、采伐、火灾等）
        /// </summary>
        /// <param name="root"></param>
        /// <param name="mEditKind"></param>
        /// <returns>1.路径,2.是否默认,3.configpath</returns>
        public Tuple<string, bool> PreparePathInfo( string mEditKind)
        {
            RootPath = UtilFactory.GetConfigOpt().RootPath;
            string sMxdPath = RootPath + PathManager.FindValue("EditMapPath");
            bool bDefaultPath = true;
            if (mEditKind == "造林")
            {
                if (PathManager.FindValue("EditMapZLPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapZLPath2");
                    if (CheckDataSource("EditMapZLDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapZLPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapZLPath");
                }
            }
            else if (mEditKind == "采伐")
            {
                if (PathManager.FindValue("EditMapCFPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapCFPath2");
                    if (CheckDataSource("EditMapCFDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapCFPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapCFPath");
                }
            }
            else if (mEditKind == "火灾")
            {
                if (PathManager.FindValue("EditMapHZPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapHZPath2");
                    if (CheckDataSource("EditMapHZDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapHZPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapHZPath");
                }
            }
            else if (mEditKind == "征占用")
            {
                if (PathManager.FindValue("EditMapZZYPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapZZYPath2");
                    if (CheckDataSource("EditMapZZYDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapZZYPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapZZYPath");
                }
            }
            else if (mEditKind == "林业工程")
            {
                if (PathManager.FindValue("EditMapGCPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapGCPath2");
                    if (CheckDataSource("EditMapGCDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapGCPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapGCPath");
                }
            }
            else if (mEditKind == "林业案件")
            {
                if (PathManager.FindValue("EditMapAJPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapAJPath2");
                    if (CheckDataSource("EditMapAJDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapAJPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapAJPath");
                }
            }
            else if (mEditKind == "自然灾害")
            {
                if (PathManager.FindValue("EditMapZHPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapZHPath2");
                    if (CheckDataSource("EditMapZHDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapZHPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapZHPath");
                }
            }
            else if (mEditKind == "遥感")
            {
                if (PathManager.FindValue("EditMapYGPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapYGPath2");
                    if (CheckDataSource("EditMapYGDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapYGPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapYGPath");
                }
            }
            else if (mEditKind == "小班变更")
            {
                if (PathManager.FindValue("EditMapXBPath3").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath3");
                    if (CheckDataSource("EditMapXBDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath");
                }
            }
            else if (mEditKind == "年度小班")//二维浏览在此加载图层视图
            {
                if (PathManager.FindValue("EditMapXBPath4").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath4");
                    if (CheckDataSource("EditMapXBDataSource2", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath2");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapXBPath2");
                }
            }
            else if (mEditKind == "二类变化")
            {
                if (PathManager.FindValue("EditMapELBHPath2").Trim() != "")
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapELBHPath2");
                    if (CheckDataSource("EditMapELBHDataSource", sMxdPath))
                    {
                        bDefaultPath = false;
                    }
                    else
                    {
                        sMxdPath = RootPath + PathManager.FindValue("EditMapELBHPath");
                    }
                }
                else
                {
                    sMxdPath = RootPath + PathManager.FindValue("EditMapELBHPath");
                }
            }
            return new Tuple<string, bool>(sMxdPath, bDefaultPath);
        }
        public Tuple<bool, IList<ILayer>> SetFeatureLayerResource(IMap map, IFeatureWorkspace mFeatureWorkspace,bool HasLayerResource,string taskYear)
        {
            IList<ILayer> mLayerList = new List<ILayer>();
            try
            {           
                string name = "";
                string str2 = "";
                for (int i = 0; i < map.LayerCount; i++)
                {

                    IDataset dataset4;
                    IFeatureClass class2;
                    ILayer layer = map.get_Layer(i);
                    if (layer is IGroupLayer)
                    {
                        mLayerList.Add(layer);
                        ICompositeLayer layer2 = layer as ICompositeLayer;
                        for (int j = 0; j < layer2.Count; j++)
                        {
                            IEnumDataset dataset;
                            IDataset dataset2;
                            string[] strArray;
                            string str4;
                            IRasterCatalog catalog;
                            IGeoDataset dataset3;
                            IGdbRasterCatalogLayer layer4;
                            int num3;
                            ILayer layer3 = layer2.get_Layer(j);
                            mLayerList.Add(layer3);
                            string str3 = "";
                            if (j < 9)
                            {
                                num3 = i + 1;
                                num3 = j + 1;
                                str3 = num3.ToString() + "0" + num3.ToString();
                            }
                            else
                            {
                                num3 = i + 1;
                                num3 = j + 1;
                                string introduced26 = num3.ToString();
                                str3 = introduced26 + num3.ToString();
                            }
                            //rowArray = dataTable.Select("groupname='" + layer.Name + "' and layername='" + layer3.Name + "' and code='" + str3 + "'");
                            T_MapLayer_Mid layerMid = LM.FindByGLC(layer.Name, layer3.Name, str3);
                            if (layer3 is IGdbRasterCatalogLayer)
                            {
                                dataset = (mFeatureWorkspace as IWorkspace).get_Datasets(esriDatasetType.esriDTRasterCatalog);
                                dataset2 = dataset.Next();
                                while (dataset2 != null)
                                {
                                    if (null != layerMid)
                                    {
                                        strArray = dataset2.Name.Split(new char[] { '.' });
                                        str4 = strArray[strArray.Length - 1];
                                        if (str4.Contains(layerMid.classname))
                                        {
                                            catalog = dataset2 as IRasterCatalog;
                                            dataset3 = catalog as IGeoDataset;
                                            layer4 = layer3 as IGdbRasterCatalogLayer;
                                            if (layer4.Setup(catalog as ITable))
                                            {
                                                layer3.Name = layerMid.layername;
                                                break;
                                            }
                                        }
                                    }
                                    dataset2 = dataset.Next();
                                }
                            }
                            else if ((layer3 is IFeatureLayer) && !layer3.Name.Contains("DEM"))
                            {
                                if (layerMid != null)
                                {
                                    name = layerMid.datasetname;
                                    if (layerMid.yearflag == "1")
                                    {
                                        str2 = layerMid.classname + "_" + taskYear;
                                    }
                                    else if (layerMid.yearflag == "2")
                                    {
                                        num3 = int.Parse(taskYear) - 1;
                                        str2 = layerMid.classname + "_" + num3.ToString();
                                    }
                                    else
                                    {
                                        str2 = layerMid.classname;
                                    }
                                    dataset4 = mFeatureWorkspace.OpenFeatureDataset(name);
                                    class2 = mFeatureWorkspace.OpenFeatureClass(str2);
                                    if (HasLayerResource)
                                    {
                                        if ((layer3 as IFeatureLayer).FeatureClass == null)
                                        {
                                            HasLayerResource = false;
                                        }
                                        else if ((layer3 as IFeatureLayer).FeatureClass != class2)
                                        {
                                            HasLayerResource = false;
                                        }
                                    }
                                    (layer3 as IFeatureLayer).FeatureClass = class2;
                                }
                            }
                            else if (layer3 is IRasterLayer)
                            {
                                dataset = (mFeatureWorkspace as IWorkspace).get_Datasets(esriDatasetType.esriDTRasterDataset);
                                dataset2 = dataset.Next();
                                while (dataset2 != null)
                                {
                                    if (null != layerMid)
                                    {
                                        strArray = dataset2.Name.Split(new char[] { '.' });
                                        str4 = strArray[strArray.Length - 1];
                                        if (str4.Contains(layerMid.classname))
                                        {
                                            (layer3 as IRasterLayer).CreateFromDataset(dataset2 as IRasterDataset);
                                            layer3.Name = layerMid.layername;
                                            break;
                                        }
                                    }
                                    dataset2 = dataset.Next();
                                }
                            }
                            else if (layer3.Name.Contains("DEM"))
                            {
                                dataset = (mFeatureWorkspace as IWorkspace).get_Datasets(esriDatasetType.esriDTRasterCatalog);
                                for (dataset2 = dataset.Next(); dataset2 != null; dataset2 = dataset.Next())
                                {
                                    if (null != layerMid)
                                    {
                                        strArray = dataset2.Name.Split(new char[] { '.' });
                                        str4 = strArray[strArray.Length - 1];
                                        if (str4.Contains(layerMid.classname))
                                        {
                                            catalog = dataset2 as IRasterCatalog;
                                            dataset3 = catalog as IGeoDataset;
                                            layer4 = layer3 as IGdbRasterCatalogLayer;
                                            if (layer4.Setup(catalog as ITable))
                                            {
                                                layer3.Name = layerMid.layername;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (layer is IFeatureLayer)
                    {
                        mLayerList.Add(layer);
                        T_MapLayer_Mid layerMid = LM.FindByGL(layer.Name, layer.Name);
                        if (null != layerMid)
                        {
                            name = layerMid.datasetname;
                            if (layerMid.yearflag == "1")
                            {
                                str2 = layerMid.classname + "_" + taskYear;
                            }
                            else if (layerMid.yearflag == "2")
                            {
                                str2 = layerMid.classname + "_" + ((int.Parse(taskYear) - 1)).ToString();
                            }
                            else
                            {
                                str2 = layerMid.classname;
                            }
                            dataset4 = mFeatureWorkspace.OpenFeatureDataset(name);
                            class2 = mFeatureWorkspace.OpenFeatureClass(str2);
                            (layer as FeatureLayer).FeatureClass = class2;
                        }
                    }
                }
                return new Tuple<bool, IList<ILayer>>(HasLayerResource, mLayerList);
            }
            catch (Exception ex)
            {
                
            }
            return new Tuple<bool, IList<ILayer>>(HasLayerResource, mLayerList);
        }
    }
}
