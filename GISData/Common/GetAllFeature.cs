using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISData.Common
{
    class GetAllFeature
    {
        /// <summary>
        /// 获取所有要素集
        /// </summary>
        /// <param name="workspace">工作空间对象</param>
        /// <returns>要素集列表</returns>
        public static List<IFeatureDataset> GetAllFeatureClass(IWorkspace workspace)
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
        public static List<IFeatureClass> GetAllFeatureClass(IFeatureDataset featureDataset)
        {
            IFeatureClassContainer featureClassContainer = (IFeatureClassContainer)featureDataset;
            IEnumFeatureClass enumFeatureClass = featureClassContainer.Classes;
            IFeatureClass featureClass = enumFeatureClass.Next();
 
            List<IFeatureClass> featureClassList = new List<IFeatureClass>();
            while (featureClass != null)
            {
                featureClassList.Add(featureClass);
                featureClass = enumFeatureClass.Next();
            }
            return featureClassList;
        }

        /// <summary>
        /// 获取所有要素
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <returns>要素列表</returns>
        public static List<IFeature> GetAllFeatureClass(IFeatureClass featureClass)
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
