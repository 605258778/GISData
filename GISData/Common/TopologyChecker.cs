using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataManagementTools;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geoprocessor;

namespace GISData.Common
{
    class TopologyChecker//功能：构建拓扑，拓扑检测
    {
        Geoprocessor GP_Tool = new Geoprocessor();//GP运行工具
        ITopology Topology;//生成的拓扑
        string TP_topoName;
        IFeatureDataset FeatureDataset_Main;//拓扑所属的要素数据集
        List<IFeatureClass> LI_FeatureClass = new List<IFeatureClass>();//要素数据集所包含的所有要素类
        List<IFeatureClass> LI_TopoErrorFeatureClass = new List<IFeatureClass>();
        List<ITopologyRule> LI_ITopologyRule = new List<ITopologyRule>();
        List<string> LI_AllErrorInfo = new List<string>();//记录所有错误信息
        ITopologyLayer L_TopoLayer;//记录拓扑的图层
        int[] selectRows;
        public Dictionary<string, string> DicTopoData = new Dictionary<string, string>();
        public Dictionary<string, int> DicTopoError = new Dictionary<string, int>();
        #region 辅助函数
        /// <summary>
        /// 获取拓扑图层
        /// </summary>
        /// <returns>拓扑图层Ilayer</returns>
        public ILayer PUB_GetTopoLayer()
        {
            if (L_TopoLayer == null)
            {
                L_TopoLayer = new TopologyLayerClass();
                L_TopoLayer.Topology = Topology;
            }
            return L_TopoLayer as ILayer;
        }

        public interface ITopologyRule1 : ITopologyRule
        {
            string AliasName { get; set; }
        }


        /// <summary>
        /// 构造拓扑检验类
        /// </summary>
        /// <param name="IN_MainlogyDataSet">输入的要素数据集</param>
        public TopologyChecker(IFeatureDataset IN_MainlogyDataSet)
        {
            FeatureDataset_Main = IN_MainlogyDataSet;
            if (LI_FeatureClass.Count != 0)
                LI_FeatureClass.Clear();
            PUB_GetAllFeatureClass();//获取数据集中所有包含的要素类
        }

        /// <summary>
        /// 获取数据集中所有包含的要素类
        /// </summary>
        /// <returns>返回数据集中所有包含的要素类 List<IFeatureClass></returns>
        public List<IFeatureClass> PUB_GetAllFeatureClass()
        {
            if (LI_FeatureClass.Count == 0)
            {
                IFeatureClassContainer Temp_FeatureClassContainer = (IFeatureClassContainer)FeatureDataset_Main;
                IEnumFeatureClass Temp_EnumFeatureClass = Temp_FeatureClassContainer.Classes;
                IFeatureClass Temp_FeatureClass = Temp_EnumFeatureClass.Next();

                while (Temp_FeatureClass != null)
                {
                    LI_FeatureClass.Add(Temp_FeatureClass);
                    Temp_FeatureClass = Temp_EnumFeatureClass.Next();
                }
                if (LI_FeatureClass.Count == 0)
                {
                    MessageBox.Show("空数据集！");
                }
            }
            return LI_FeatureClass;
        }
        /// <summary>
        /// 获取数据集中的要素类
        /// </summary>
        /// <returns>返回数据集中所有包含的要素类 List<IFeatureClass></returns>
        public IFeatureClass PUB_GetAllFeatureClassByName(string FeatureName)
        {
            IFeatureClass returnFeatureClass = null;
            if (LI_FeatureClass.Count != 0)
            {
                foreach(IFeatureClass itemFeatureClass in LI_FeatureClass )
                {
                    if (itemFeatureClass.AliasName == FeatureName)
                    {
                        returnFeatureClass = itemFeatureClass;
                    }
                }
                
            }
            return returnFeatureClass;
        }
        #endregion

        #region 构建拓扑

        /// <summary>
        /// 在数据集中构建拓扑(GP方法)
        /// </summary>
        /// <param name="IN_TopoName">要生成拓扑的名称</param>
        /// <param name="IN_Tolerance">拓扑容差，可选，默认0.001</param>
        public void PUB_TopoBuildWithGP(string IN_TopoName, double IN_Tolerance = 0.001)
        {
            TP_topoName = IN_TopoName;
            IWorkspace FeatureWorkSpace = FeatureDataset_Main.Workspace;
            ITopologyWorkspace TopoWorkSpace = FeatureWorkSpace as ITopologyWorkspace;
            try//若不存在同名拓扑则添加
            {
                Topology = TopoWorkSpace.OpenTopology(IN_TopoName);
                CommonClass common = new CommonClass();
                common.DeleteTopolgyFromGISDB(Topology);
                DeleteFeature(TP_topoName);
            }
            catch
            {
                
            }
            CreateTopology Topotool = new CreateTopology();//拓扑GP工具
            Topotool.in_dataset = FeatureDataset_Main; ;
            Topotool.out_name = IN_TopoName;
            Topotool.in_cluster_tolerance = IN_Tolerance;
            try
            {
                GP_Tool.Execute(Topotool, null);
                Topology = TopoWorkSpace.OpenTopology(IN_TopoName);
            }
            catch (COMException comExc)
            {
                MessageBox.Show(String.Format("拓扑创建出错: {0} 描述: {1}", comExc.ErrorCode, comExc.Message));
            }
        }


        /// <summary>
        /// 在数据集中构建拓扑
        /// </summary>
        /// <param name="IN_TopoName">要生成拓扑的名称</param>
        /// <param name="IN_Tolerance">拓扑容差，可选，默认0.001</param>
        public void PUB_TopoBuild(string IN_TopoName, double IN_Tolerance = 0.001)
        {
            TP_topoName = IN_TopoName;
            ITopologyContainer topologyContainer = (ITopologyContainer)FeatureDataset_Main;
            try//若不存在同名拓扑则添加
            {
                Topology = topologyContainer.get_TopologyByName(IN_TopoName);
                CommonClass common = new CommonClass();
                common.DeleteTopolgyFromGISDB(Topology);
                DeleteFeature(TP_topoName);
            }
            catch
            {
                
            }
            try
            {
                Topology = topologyContainer.CreateTopology(IN_TopoName, IN_Tolerance, -1, "");
            }
            catch (COMException comExc)
            {
                MessageBox.Show(String.Format("拓扑创建出错: {0} 描述: {1}", comExc.ErrorCode, comExc.Message));
            }
        }

        #endregion

        #region 添加要素类

        /// <summary>
        /// 添加特定要素类到拓扑中（GP方法）
        /// </summary>
        /// <param name="IN_TopologyClass">要添加的要素类的集合。输入null为该数据集下所有要素类</param>
        /// <param name="IN_XYRank">XY等级，默认为1。可选。</param>
        /// <param name="IN_ZRank">Z等级，默认为1。可选。</param>
        public void PUB_AddFeatureClassWithGP(List<IFeatureClass> IN_TopologyClass, int IN_XYRank = 1, int IN_ZRank = 1)
        {
            if (Topology != null)
            {
                AddFeatureClassToTopology Temp_AddClassToTopo = new AddFeatureClassToTopology();
                Temp_AddClassToTopo.in_topology = Topology;
                Temp_AddClassToTopo.xy_rank = IN_XYRank;
                Temp_AddClassToTopo.z_rank = IN_ZRank;
                if (IN_TopologyClass == null)
                {
                    IN_TopologyClass = LI_FeatureClass;
                }
                foreach (IFeatureClass EachFeatureCLS in IN_TopologyClass)
                {
                    if (LI_FeatureClass.Contains(EachFeatureCLS))
                    {
                        Temp_AddClassToTopo.in_featureclass = EachFeatureCLS;
                        try
                        {
                            GP_Tool.Execute(Temp_AddClassToTopo, null);
                        }
                        catch (COMException comExc)
                        {
                            MessageBox.Show(String.Format(((FeatureClass)EachFeatureCLS).Name + ":添加失败。 描述: {0}", comExc.Message));
                        }
                    }
                    else
                    {
                        MessageBox.Show("该要素类不属于目标要素集，无法添加！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先构建拓扑");
            }
        }


        /// <summary>
        /// 添加特定要素类到拓扑中
        /// </summary>
        /// <param name="IN_TopologyClass">要添加的要素类的集合。输入null为该数据集下所有要素类</param>
        /// <param name="IN_XYRank">XY等级，默认为1。可选。</param>
        /// <param name="IN_ZRank">Z等级，默认为1。可选。</param>
        /// <param name="IN_Weight">权重，默认为5。可选。</param>
        public void PUB_AddFeatureClass(List<IFeatureClass> IN_TopologyClass, int IN_XYRank = 1, int IN_ZRank = 1, double IN_Weight = 5)
        {
            if (Topology != null)
            {
                if (IN_TopologyClass == null)
                {
                    IN_TopologyClass = LI_FeatureClass;
                }
                foreach (IFeatureClass EachFeatureCLS in IN_TopologyClass)//逐项添加所选的要素类
                {
                    if (LI_FeatureClass.Contains(EachFeatureCLS))//只有相同要素数据集中的要素可以被添加
                    {
                        try
                        {
                            Topology.AddClass(EachFeatureCLS as IClass, IN_Weight, IN_XYRank, IN_ZRank, false);
                        }
                        catch (COMException comExc)
                        {
                            MessageBox.Show(String.Format(((FeatureClass)EachFeatureCLS).Name + ":添加失败。 描述: {0}", comExc.Message));
                        }
                    }
                    else
                    {
                        MessageBox.Show("该要素类不属于目标要素集，无法添加！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先构建拓扑");
            }
        }
        #endregion

        #region 添加规则并检验
        //GP法拓扑验证
        private void PRV_ValidateTopologyWithGP()
        {
            try
            {
                ValidateTopology Temp_Validate = new ValidateTopology(Topology);
                GP_Tool.Execute(Temp_Validate, null);
            }
            catch
            {
                MessageBox.Show("无法完成检测！");
            }
        }

        /// <summary>
        /// 单要素规则
        /// </summary>
        /// <param name="IN_RuleType">要添加的规则</param>
        /// <param name="IN_FeatureClass">添加规则的要素类</param>
        public void PUB_AddRuleToTopology(string idname ,TopoErroType IN_RuleType, IFeatureClass IN_FeatureClass)
        {
            if (Topology != null)
            {
                ITopologyRule Temp_TopologyRule = new TopologyRuleClass();
                //设定参数
                Temp_TopologyRule.TopologyRuleType = PRV_ConvertTopologyRuleType(IN_RuleType);
                Temp_TopologyRule.Name = IN_RuleType.ToString();
                Temp_TopologyRule.OriginClassID = IN_FeatureClass.FeatureClassID;
                Temp_TopologyRule.AllOriginSubtypes = true;
                LI_ITopologyRule.Add(Temp_TopologyRule);
                PRV_AddRuleTool(Temp_TopologyRule, idname);
            }
            else
            {
                MessageBox.Show("请先构建拓扑");
            }
        }

        /// <summary>
        /// 双要素规则
        /// </summary>
        /// <param name="IN_RuleType">要添加的双要素规则</param>
        /// <param name="IN_FeatureClassA">第一个要素</param>
        /// <param name="IN_FeatureClassB">第二个要素</param>
        public void PUB_AddRuleToTopology(string idname,TopoErroType IN_RuleType, IFeatureClass IN_FeatureClassA, IFeatureClass IN_FeatureClassB)
        {
            if (Topology != null)
            {
                ITopologyRule Temp_TopologyRule = new TopologyRuleClass();
                //设定参数
                Temp_TopologyRule.TopologyRuleType = PRV_ConvertTopologyRuleType(IN_RuleType);
                Temp_TopologyRule.Name = IN_RuleType.ToString();
                Temp_TopologyRule.OriginClassID = IN_FeatureClassA.FeatureClassID;
                Temp_TopologyRule.DestinationClassID = IN_FeatureClassB.FeatureClassID;
                Temp_TopologyRule.AllOriginSubtypes = true;
                Temp_TopologyRule.AllDestinationSubtypes = true;
                LI_ITopologyRule.Add(Temp_TopologyRule);
                PRV_AddRuleTool(Temp_TopologyRule, idname);
            }
            else
            {
                MessageBox.Show("请先构建拓扑");
            }
        }

        //规则添加工具
        private void PRV_AddRuleTool(ITopologyRule IN_TopologyRule,string idname)
        {
            ITopologyRuleContainer Temp_TopologyRuleContainer = (ITopologyRuleContainer)Topology;//构建容器
            try
            {
                Temp_TopologyRuleContainer.get_CanAddRule(IN_TopologyRule);//不能添加的话直接报错
                try
                {
                    Temp_TopologyRuleContainer.DeleteRule(IN_TopologyRule);//删除已存在的规则后再添加
                    IN_TopologyRule.OriginSubtype = int.Parse(idname);
                    Temp_TopologyRuleContainer.AddRule(IN_TopologyRule);//规则存在的话直接报错
                }
                catch
                {
                    Temp_TopologyRuleContainer.AddRule(IN_TopologyRule);
                }
            }
            catch
            {
                MessageBox.Show("不支持添加");
            }
            // PRV_ValidateTopologyWithGP();//添加完成后自动检验
            //PUB_GetTopoLayer();//存储创建的拓扑图层
            
            //PRV_GetError(IN_TopologyRule);//输出错误
        }

        public void doValidateTopology(int[] selectRows, Dictionary<string, string> DicTopoData) 
        {
            this.DicTopoData = DicTopoData;
             this.selectRows = selectRows;
            IGeoDataset geoDataset = (IGeoDataset)Topology;
            IEnvelope envelope = geoDataset.Extent;
            ValidateTopology(Topology, envelope);
            PRV_GetErrorFeature();
        }

        
        public void ValidateTopology(ITopology topology, IEnvelope envelope)
        {
            // Get the dirty area within the provided envelope.
            IPolygon locationPolygon = new PolygonClass();
            ISegmentCollection segmentCollection = (ISegmentCollection)locationPolygon;
            segmentCollection.SetRectangle(envelope);
            IPolygon polygon = topology.get_DirtyArea(locationPolygon);
            // If a dirty area exists, validate the topology.    if (!polygon.IsEmpty)
            {        // Define the area to validate and validate the topology.
                IEnvelope areaToValidate = polygon.Envelope;
                IEnvelope areaValidated = topology.ValidateTopology(areaToValidate);
            }
        }

        //获取错误信息
        private void PRV_GetError(ITopologyRule IN_TopologyRule)
        {
            if (Topology != null)
            {
                IEnvelope Temp_Envolope = (this.Topology as IGeoDataset).Extent;
                IErrorFeatureContainer Temp_ErrorContainer = Topology as IErrorFeatureContainer;
                //获取所有信息
                IEnumTopologyErrorFeature Temp_EnumErrorFeature = Temp_ErrorContainer.get_ErrorFeatures(((IGeoDataset)FeatureDataset_Main).SpatialReference, IN_TopologyRule, Temp_Envolope, true, true);
                ITopologyErrorFeature Temp_ErrorFeature = Temp_EnumErrorFeature.Next();
                while (Temp_ErrorFeature != null)
                {
                    IFeature Temp_Feature = Temp_ErrorFeature as IFeature;
                    string Temp_ErrorInfo;
                    if (Temp_ErrorFeature.DestinationClassID != 0)//检测是否是双要素规则
                    {
                        Temp_ErrorInfo = Temp_ErrorFeature.OriginOID + "," + Temp_ErrorFeature.DestinationOID;
                    }
                    else
                        Temp_ErrorInfo = Temp_ErrorFeature.OriginOID.ToString();
                    LI_AllErrorInfo.Add(Temp_ErrorInfo);//将错误信息加入List
                    MessageBox.Show("错误：" + PRV_RecorverTopologyRuleType((int)(Temp_ErrorFeature.TopologyRuleType)) + "\r\n错误ID：" + Temp_ErrorInfo);
                    Temp_ErrorFeature = Temp_EnumErrorFeature.Next();
                }
            }
            else
            {
                MessageBox.Show("请先构建拓扑");
            }
        }

        private void PRV_GetErrorFeature()
        {
            foreach (ITopologyRule IN_RuleType in LI_ITopologyRule)
            {
                int tempCount = 0;
                DicTopoError.Add(IN_RuleType.OriginSubtype.ToString(), tempCount);
                IEnvelope Temp_Envolope = (this.Topology as IGeoDataset).Extent;
                IErrorFeatureContainer Temp_ErrorContainer = Topology as IErrorFeatureContainer;
                //获取该种错误所有的错误要素  
                IEnumTopologyErrorFeature Temp_EnumErrorFeature = Temp_ErrorContainer.get_ErrorFeatures(((IGeoDataset)FeatureDataset_Main).SpatialReference, IN_RuleType, Temp_Envolope, true, false);
                //提取一个错误要素  
                ITopologyErrorFeature Temp_ErrorFeature = Temp_EnumErrorFeature.Next();
                if (Temp_ErrorFeature != null)
                {
                    //作为搭建模型的要素
                    IFeature Temp_MoudleFeature = Temp_ErrorFeature as IFeature;
                    //生成要素类需要CLSID和EXCLSID
                    IFeatureClassDescription Temp_FeatureClassDescription = new FeatureClassDescriptionClass();
                    IObjectClassDescription Temp_ObjectClassDescription = (IObjectClassDescription)Temp_FeatureClassDescription;
                    //以模型要素为模板构建一个要素类  
                    //FeatureDataset_Main.CreateFeatureClass(TP_topoName, Temp_MoudleFeature.Fields, Temp_ObjectClassDescription.InstanceCLSID, Temp_ObjectClassDescription.ClassExtensionCLSID, Temp_MoudleFeature.FeatureType, "Shape", null);
                    //打开生成的目标要素类并加入集合留待输出时使用  
                    IFeatureClass Temp_TargetFeatureClass;
                    string ErrorFeatureName = TP_topoName + "_" + Temp_ErrorFeature.ShapeType.ToString();
                    try
                    {
                        Temp_TargetFeatureClass = (FeatureDataset_Main.Workspace as IFeatureWorkspace).OpenFeatureClass(ErrorFeatureName);
                    }
                    catch
                    {
                        FeatureDataset_Main.CreateFeatureClass(ErrorFeatureName, Temp_MoudleFeature.Fields, Temp_ObjectClassDescription.InstanceCLSID, Temp_ObjectClassDescription.ClassExtensionCLSID, Temp_MoudleFeature.FeatureType, "Shape", null);
                        Temp_TargetFeatureClass = (FeatureDataset_Main.Workspace as IFeatureWorkspace).OpenFeatureClass(ErrorFeatureName);
                    }
                    LI_TopoErrorFeatureClass.Add(Temp_TargetFeatureClass);
                    //将所有错误要素添加进目标要素类  
                    IWorkspaceEdit Temp_WorkspaceEdit = (IWorkspaceEdit)FeatureDataset_Main.Workspace;
                    Temp_WorkspaceEdit.StartEditing(true);
                    Temp_WorkspaceEdit.StartEditOperation();
                    IFeatureBuffer Temp_FeatureBuffer = Temp_TargetFeatureClass.CreateFeatureBuffer();
                    
                    //在目标要素类中插入所有错误要素  
                    IFeatureCursor featureCursor = Temp_TargetFeatureClass.Insert(false);
                    while (Temp_ErrorFeature != null)
                    {
                        
                        IFeature Temp_Feature = Temp_ErrorFeature as IFeature;
                        //给目标要素附属性  
                        Temp_FeatureBuffer.set_Value(1, Temp_ErrorFeature.OriginClassID);
                        Temp_FeatureBuffer.set_Value(2, Temp_ErrorFeature.OriginOID);
                        Temp_FeatureBuffer.set_Value(3, Temp_ErrorFeature.DestinationClassID);
                        Temp_FeatureBuffer.set_Value(4, Temp_ErrorFeature.DestinationOID);
                        Temp_FeatureBuffer.set_Value(5, Temp_ErrorFeature.TopologyRuleType);
                        Temp_FeatureBuffer.set_Value(8, Temp_ErrorFeature.IsException);
                        Temp_FeatureBuffer.Shape = Temp_Feature.Shape;
                        if (this.DicTopoData.ContainsKey(IN_RuleType.OriginSubtype.ToString()))
                        {
                            if (IN_RuleType.Name == "面要素之间无空隙")
                            {
                                IArea pArea = (IArea)Temp_Feature.Shape.Envelope;
                                if (pArea.Area < System.Convert.ToDouble(this.DicTopoData[IN_RuleType.OriginSubtype.ToString()]))
                                {
                                    tempCount++;
                                    DicTopoError[IN_RuleType.OriginSubtype.ToString()] = tempCount;
                                    object featureOID = featureCursor.InsertFeature(Temp_FeatureBuffer);
                                    featureCursor.Flush();//保存要素  
                                }
                            }
                        }
                        else 
                        {
                            tempCount++;
                            DicTopoError[IN_RuleType.OriginSubtype.ToString()] = tempCount;
                            object featureOID = featureCursor.InsertFeature(Temp_FeatureBuffer);
                            featureCursor.Flush();//保存要素  
                        }
                        Temp_ErrorFeature = Temp_EnumErrorFeature.Next();
                    }
                    Temp_WorkspaceEdit.StopEditOperation();
                    Temp_WorkspaceEdit.StopEditing(true);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                }
            }
        }

        private void DeleteFeature(string FeatureName) 
        {
            IFeatureClassContainer pFeatureclassContainer = (IFeatureClassContainer)FeatureDataset_Main;
            IEnumFeatureClass pEnumFeatureClass = (IEnumFeatureClass)pFeatureclassContainer.Classes;
            IFeatureClass pFeatureClass = (IFeatureClass)pEnumFeatureClass.Next();
            while (pFeatureClass != null)//在每一个数据集中遍历数据层IFeatureClass
            {
                if (pFeatureClass.AliasName.Equals(FeatureName))
                {
                    //CommonClass common = new CommonClass();
                    //common.DeleteTopolgyFromGISDB(Topology);
                    IDataset pds = pFeatureClass as IDataset;
                    LI_FeatureClass.Remove(pFeatureClass);
                    pds.Delete();
                    break;
                }
                pFeatureClass = (IFeatureClass)pEnumFeatureClass.Next();

            }
        }

        /// <summary>
        /// 提取所有拓扑错误信息
        /// </summary>
        /// <returns>错误信息集合</returns>
        public List<string> PUB_GetErrorInfo()
        {
            return LI_AllErrorInfo;
        }

        #region 规则翻译

        //根据错误的中文描述转换成esri拓扑枚举
        private esriTopologyRuleType PRV_ConvertTopologyRuleType(TopoErroType IN_TopoRuleType)
        {
            esriTopologyRuleType Temp_TopoRuleType;
            switch (IN_TopoRuleType)
            {
                case TopoErroType.面要素之间无空隙:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaNoGaps;
                    break;
                case TopoErroType.任何规则:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAny;
                    break;
                case TopoErroType.要素大于最小容差:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTFeatureLargerThanClusterTolerance;
                    break;
                case TopoErroType.面要素间无重叠:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaNoOverlap;
                    break;
                case TopoErroType.第二个图层面要素必须被第一个图层任一面要素覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaCoveredByAreaClass;
                    break;
                case TopoErroType.面要素必须只包含一个点要素:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaContainOnePoint;
                    break;
                case TopoErroType.两图层面要素必须互相覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaAreaCoverEachOther;
                    break;
                case TopoErroType.第一个图层面要素必须被第一个图层任一面要素包含:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaCoveredByArea;
                    break;
                case TopoErroType.图层间面要素不能相互覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaNoOverlapArea;
                    break;
                case TopoErroType.线要素必须跟面图层边界的一部分或全部重叠:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary;
                    break;
                case TopoErroType.点要素必须落在面要素边界上:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTPointCoveredByAreaBoundary;
                    break;
                case TopoErroType.点要素必须落在面要素内:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTPointProperlyInsideArea;
                    break;
                case TopoErroType.线要素间不能有相互重叠部分:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoOverlap;
                    break;
                case TopoErroType.线要素之间不能相交:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoIntersection;
                    break;
                case TopoErroType.线要素不允许有悬挂点:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoDangles;
                    break;
                case TopoErroType.线要素不允许有假节点:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoPseudos;
                    break;
                case TopoErroType.第一个图层线要素应被第二个线图层线要素覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineCoveredByLineClass;
                    break;
                case TopoErroType.第一个图层线要素不被第二个线图层线要素覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoOverlapLine;
                    break;
                case TopoErroType.点要素应被线要素覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTPointCoveredByLine;
                    break;
                case TopoErroType.点要素应在线要素的端点上:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint;
                    break;
                case TopoErroType.面要素边界必须被线要素覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine;
                    break;
                case TopoErroType.面要素的边界必须被另一面要素边界覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaBoundaryCoveredByAreaBoundary;
                    break;
                case TopoErroType.线要素不能自重叠:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoSelfOverlap;
                    break;
                case TopoErroType.线要素不能自相交:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoSelfIntersect;
                    break;
                case TopoErroType.线要素间不能重叠和相交:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoIntersectOrInteriorTouch;
                    break;
                case TopoErroType.线要素端点必须被点要素覆盖:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint;
                    break;
                case TopoErroType.面要素内必须包含至少一个点要素:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAreaContainPoint;
                    break;
                case TopoErroType.线不能是多段:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoMultipart;
                    break;
                case TopoErroType.点要素之间不相交:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTPointDisjoint;
                    break;
                case TopoErroType.线要素必须不相交:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoIntersectLine;
                    break;
                case TopoErroType.线必须不相交或内部接触:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoIntersectOrInteriorTouchLine;
                    break;
                case TopoErroType.面要素无多部件:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTLineNoMultipart;
                    break;
                default:
                    Temp_TopoRuleType = esriTopologyRuleType.esriTRTAny;//将此规则赋予拓扑会直接报错
                    break;
            }
            return Temp_TopoRuleType;
        }

        public enum TopoErroType
        {
            任何规则 = -1,
            要素大于最小容差 = 0,
            面要素之间无空隙 = 1,
            面要素间无重叠 = 3,
            第二个图层面要素必须被第一个图层任一面要素覆盖 = 4,
            两图层面要素必须互相覆盖 = 5,
            第一个图层面要素必须被第一个图层任一面要素包含 = 7,
            图层间面要素不能相互覆盖 = 8,
            线要素必须跟面图层边界的一部分或全部重叠 = 10,
            线要素必须在面内 = 11,
            点要素必须落在面要素边界上 = 13,
            点要素必须落在面要素内 = 15,
            面要素必须只包含一个点要素 = 16,
            线要素间不能有相互重叠部分 = 19,
            线要素之间不能相交 = 20,
            线要素不允许有悬挂点 = 21,
            线要素不允许有假节点 = 22,
            第一个图层线要素应被第二个线图层线要素覆盖 = 26,
            第一个图层线要素不被第二个线图层线要素覆盖 = 28,
            点要素应被线要素覆盖 = 29,
            点要素应在线要素的端点上 = 31,
            点要素之间不相交 = 34,
            点要素重合点要素 = 35,
            面要素边界必须被线要素覆盖 = 37,
            面要素的边界必须被另一面要素边界覆盖 = 38,
            线要素不能自重叠 = 39,
            线要素不能自相交 = 40,
            线要素间不能重叠和相交 = 41,
            线要素端点必须被点要素覆盖 = 42,
            面要素内必须包含至少一个点要素 = 43,
            线不能是多段 = 44,
            线要素必须不相交 = 45,
            线必须不相交或内部接触 = 46,
            面要素无多部件 = 47
            
        };

        //根据错误ID获取对应描述
        private string PRV_RecorverTopologyRuleType(int IN_TopoType)
        {
            //根据枚举值获取枚举名
            string Temp_ErrorDiscripe = Enum.GetName(typeof(TopoErroType), IN_TopoType);
            if (Temp_ErrorDiscripe == null)
                return (IN_TopoType.ToString());//若规则不在列表内则直接返回规则号
            else
                return Temp_ErrorDiscripe;
        }
        #endregion

        #endregion
    }
}
