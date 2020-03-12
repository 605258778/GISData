namespace ShapeEdit
{
    using ESRI.ArcGIS.SystemUI;
    using System;

    /// <summary>
    /// 工具的工厂类
    /// </summary>
    public class ToolFactory
    {
        private static ITool _autocomTool;
        private static ITool _combineexTool;
        private static ITool _createTool;
        private static ITool _createTool2;
        private static ITool _deleteexTool;
        private static ITool _deleteTool;
        private static ITool _deleteVertexTool;
        private static ITool _editTool;
        private static ITool _erase2Tool;
        private static ITool _eraseTool;
        private static ITool _hx;
        private static ITool _insertVertexTool;
        private static ITool _linkageDeleteVertex;
        private static ITool _linkageEdit;
        private static ITool _linkageInsertVertex;
        private static ITool _overlapCombineTool;
        private static ITool _overlapConvertTool;
        private static ITool _overlapDeleteTool;
        private static ITool _quickSnapTool;
        private static ITool _rpointDeleteExTool;
        private static ITool _simplifyTool;
        private static ITool _sketchTool;
        private static ITool _splitTool;

        /// <summary>
        /// 自动完成多边形工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetAutocompleteTool()
        {
            if (_autocomTool == null)
            {
                _autocomTool = new AutoComplete();
            }
            return _autocomTool;
        }

        /// <summary>
        /// 合并的工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetCombineExTool()
        {
            if (_combineexTool == null)
            {
                _combineexTool = new CombineEx();
            }
            return _combineexTool;
        }

        /// <summary>
        /// 创建多边形工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetCreateTool()
        {
            if (_createTool == null)
            {
                _createTool = new Create();
            }
            return _createTool;
        }

        /// <summary>
        /// 创建圆形的工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetCreateTool2()
        {
            if (_createTool2 == null)
            {
                _createTool2 = new Create2();
            }
            return _createTool2;
        }

        /// <summary>
        /// 删除面积错误的要素的工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetDeleteExTool()
        {
            if (_deleteexTool == null)
            {
                _deleteexTool = new DeleteEx();
            }
            return _deleteexTool;
        }

        /// <summary>
        /// 删除选中的要素的工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetDeleteTool()
        {
            if (_deleteTool == null)
            {
                _deleteTool = new Delete();
            }
            return _deleteTool;
        }

        /// <summary>
        /// 获取删除节点工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetDeleteVertexTool()
        {
            if (_deleteVertexTool == null)
            {
                _deleteVertexTool = new DeleteVertex();
            }
            return _deleteVertexTool;
        }

        /// <summary>
        /// 获取编辑班块类
        /// </summary>
        /// <returns></returns>
        public static ITool GetEditTool()
        {
            if (_editTool == null)
            {
                _editTool = new Edit();
            }
            return _editTool;
        }

        /// <summary>
        /// 获取裁切工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetErase2Tool()
        {
            if (_erase2Tool == null)
            {
                _erase2Tool = new Erase2();
            }
            return _erase2Tool;
        }

        /// <summary>
        /// 获取挖空工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetEraseTool()
        {
            if (_eraseTool == null)
            {
                _eraseTool = new Erase();
            }
            return _eraseTool;
        }

        /// <summary>
        /// 获取画红线工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetHxTool()
        {
            if (_hx == null)
            {
                _hx = new HX();
            }
            return _hx;
        }

        /// <summary>
        /// 获取插入节点工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetInsertVertexTool()
        {
            if (_insertVertexTool == null)
            {
                _insertVertexTool = new InsertVertex();
            }
            return _insertVertexTool;
        }

        /// <summary>
        ///  删除顶点工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetLinkageDeleteVertexTool()
        {
            if (_linkageDeleteVertex == null)
            {
                _linkageDeleteVertex = new LinkageDeleteVertex();
            }
            return _linkageDeleteVertex;
        }

        /// <summary>
        /// 获取联动编辑的工具
        /// </summary>
        /// <returns></returns>
        public static ITool GetLinkageEditTool()
        {
            if (_linkageEdit == null)
            {
                _linkageEdit = new LinkageEdit();
            }
            return _linkageEdit;
        }

        /// <summary>
        /// 添加顶点工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetLinkageInsertVertexTool()
        {
            if (_linkageInsertVertex == null)
            {
                _linkageInsertVertex = new LinkageInsertVertex();
            }
            return _linkageInsertVertex;
        }

        /// <summary>
        /// 获取合并重叠部分工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetOverlapCombineTool()
        {
            if (_overlapCombineTool == null)
            {
                _overlapCombineTool = new OverlapCombine();
            }
            return _overlapCombineTool;
        }

        /// <summary>
        /// 获取将重叠部分转化为小班工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetOverlapConvertTool()
        {
            if (_overlapConvertTool == null)
            {
                _overlapConvertTool = new OverlapConvert();
            }
            return _overlapConvertTool;
        }

        /// <summary>
        /// 获取删除重叠部分工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetOverlapDeleteTool()
        {
            if (_overlapDeleteTool == null)
            {
                _overlapDeleteTool = new OverlapDelete();
            }
            return _overlapDeleteTool;
        }

        /// <summary>
        /// 获取删除重复点工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetRPointDeleteExTool()
        {
            if (_rpointDeleteExTool == null)
            {
                _rpointDeleteExTool = new RPointDeleteEx();
            }
            return _rpointDeleteExTool;
        }

        /// <summary>
        /// 拓扑简化工具类
        /// </summary>
        /// <returns></returns>
        public static ITool GetSimplifyTool()
        {
            if (_simplifyTool == null)
            {
                _simplifyTool = new Simplify();
            }
            return _simplifyTool;
        }

        /// <summary>
        /// 获取编辑草图的工具
        /// </summary>
        /// <returns></returns>
        public static ITool GetSketchTool()
        {
            if (_sketchTool == null)
            {
                _sketchTool = new EditingSketch();
            }
            return _sketchTool;
        }

        /// <summary>
        /// 获取分割工具
        /// </summary>
        /// <returns></returns>
        public static ITool GetSplitTool()
        {
            if (_splitTool == null)
            {
                _splitTool = new Split();
            }
            return _splitTool;
        }
        
        /// <summary>
        /// 快速捕捉工具
        /// </summary>
        /// <returns></returns>
        public static ITool QuickSnapTool()
        {
            if (_quickSnapTool == null)
            {
                _quickSnapTool = new SnapEx();
            }
            return _quickSnapTool;
        }
    }
}

