using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISData.Common
{
    class GetAllFeatures
    {
        /// <summary>
        /// 获取所有要素集
        /// </summary>
        /// <param name="workspace">工作空间对象</param>
        /// <returns>要素集列表</returns>
        public List<IFeatureDataset> GetAllFeatureDataset(IWorkspace workspace)
        {
            IEnumDataset dataset = workspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IFeatureDataset featureDataset = dataset.Next() as IFeatureDataset;
 
 
            List<IFeatureDataset> featureDatasetList = new List<IFeatureDataset>();
            while (featureDataset != null)
            {
                featureDatasetList.Add(featureDataset);
                featureDataset = dataset.Next() as IFeatureDataset;
            }
            return featureDatasetList;
        }

        /// <summary>
        /// 获取所有要素类
        /// </summary>
        /// <param name="featureDataset">要素集</param>
        /// <returns>要素类列表</returns>
        public List<IDataset> GetAllFeatureClass(IWorkspace pWs)
        {
            List<IDataset> iDatasetList = new List<IDataset>();

            IEnumDataset pEDataset = pWs.get_Datasets(esriDatasetType.esriDTAny);

            IDataset pDataset = pEDataset.Next();

            while (pDataset != null)
            {
                if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    IFeatureClass ifc = pDataset as IFeatureClass;
                    //pDataset.BrowseName = ifc.AliasName;
                    iDatasetList.Add(pDataset);
                }
                //如果是数据集
                else if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    IEnumDataset pESubDataset = pDataset.Subsets;

                    IDataset pSubDataset = pESubDataset.Next();

                    while (pSubDataset != null)
                    {
                        IFeatureClass ifc = pSubDataset as IFeatureClass;
                        //pSubDataset.BrowseName = ifc.AliasName;
                        iDatasetList.Add(pSubDataset);

                        pSubDataset = pESubDataset.Next();
                    }
                }
                pDataset = pEDataset.Next();
            }
            return iDatasetList;
        }

        /// <summary>
        /// 获取所有要素
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <returns>要素列表</returns>
        public List<IFeature> GetAllFeature(IFeatureClass featureClass)
        {
            List<IFeature> featureList = new List<IFeature>();
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                featureList.Add(feature);
                feature = featureCursor.NextFeature();
            }
            return featureList;
        }
    }
}
