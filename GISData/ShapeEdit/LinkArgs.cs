namespace ShapeEdit
{
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.Collections.Generic;

    public class LinkArgs
    {
        private IFeature _feature;
        private int _partIndex;
        private IList<int> _vertexIndex;

        public LinkArgs(IFeature pFeature)
        {
            this._feature = pFeature;
        }

        public IFeature feature
        {
            get
            {
                return this._feature;
            }
        }

        public int PartIndex
        {
            get
            {
                return this._partIndex;
            }
            set
            {
                this._partIndex = value;
            }
        }

        public IList<int> VertexIndex
        {
            get
            {
                if (this._vertexIndex == null)
                {
                    this._vertexIndex = new List<int>();
                }
                return this._vertexIndex;
            }
            set
            {
                this._vertexIndex = value;
            }
        }
    }
}

