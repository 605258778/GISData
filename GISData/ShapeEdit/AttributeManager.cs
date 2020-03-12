namespace ShapeEdit
{
    using System;

    public class AttributeManager
    {
        private static IAttributeAdd _AttributeAddHandleClass;
        private static IAttributeCombine _AttributeCombineHandleClass;
        private static IAttributeDelete _AttributeDeleteHandleClass;
        private static IAttributeSelected _AttributeSelectedHandleClass;
        private static IAttributeSplit _AttributeSplitHandleClass;
        private static IAttributeUndo _AttributeUndoHandleClass;

        public static IAttributeAdd AttributeAddHandleClass
        {
            get
            {
                return _AttributeAddHandleClass;
            }
            set
            {
                _AttributeAddHandleClass = value;
            }
        }

        public static IAttributeCombine AttributeCombineHandleClass
        {
            get
            {
                return _AttributeCombineHandleClass;
            }
            set
            {
                _AttributeCombineHandleClass = value;
            }
        }

        public static IAttributeDelete AttributeDeleteHandleClass
        {
            get
            {
                return _AttributeDeleteHandleClass;
            }
            set
            {
                _AttributeDeleteHandleClass = value;
            }
        }

        public static IAttributeSelected AttributeSelectedHandleClass
        {
            get
            {
                return _AttributeSelectedHandleClass;
            }
            set
            {
                _AttributeSelectedHandleClass = value;
            }
        }

        public static IAttributeSplit AttributeSplitHandleClass
        {
            get
            {
                return _AttributeSplitHandleClass;
            }
            set
            {
                _AttributeSplitHandleClass = value;
            }
        }

        public static IAttributeUndo AttributeUndoHandleClass
        {
            get
            {
                return _AttributeUndoHandleClass;
            }
            set
            {
                _AttributeUndoHandleClass = value;
            }
        }
    }
}

