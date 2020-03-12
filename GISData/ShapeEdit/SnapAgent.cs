namespace ShapeEdit
{
    using ESRI.ArcGIS.Carto;
    using System;

    public class SnapAgent
    {
        private bool _edge;
        private bool _endPoint;
        private IFeatureLayer _featureLayer;
        private string _featureLayerName;
        private bool _vertex;

        public SnapAgent(IFeatureLayer pLayer, string pLayerName, bool pVertex, bool pEdge, bool pEndPoint)
        {
            this._featureLayer = pLayer;
            this._featureLayerName = pLayerName;
            this._vertex = pVertex;
            this._edge = pEdge;
            this._endPoint = pEndPoint;
        }

        public bool Edge
        {
            get
            {
                return this._edge;
            }
            set
            {
                this._edge = value;
            }
        }

        public bool EndPoint
        {
            get
            {
                return this._endPoint;
            }
            set
            {
                this._endPoint = value;
            }
        }

        public IFeatureLayer FeatureLayer
        {
            get
            {
                return this._featureLayer;
            }
        }

        public string FeatureLayerName
        {
            get
            {
                return this._featureLayerName;
            }
        }

        public bool Vertex
        {
            get
            {
                return this._vertex;
            }
            set
            {
                this._vertex = value;
            }
        }
    }
}

