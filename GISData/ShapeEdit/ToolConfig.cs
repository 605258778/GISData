namespace ShapeEdit
{
    using System;

    public class ToolConfig
    {
        private static double _MouseTolerance = 3.0;
        private static double _MouseTolerance1 = 5E-05;

        public static double MouseTolerance
        {
            get
            {
                return _MouseTolerance;
            }
        }

        public static double MouseTolerance1
        {
            get
            {
                return _MouseTolerance1;
            }
        }
    }
}

