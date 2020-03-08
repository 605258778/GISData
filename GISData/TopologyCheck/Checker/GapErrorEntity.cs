namespace TopologyCheck.Checker
{
    using System;

    public class GapErrorEntity
    {
        private object _errGeo;
        private string _featureId;

        public GapErrorEntity(string pFeatureId, object pErrGeo)
        {
            this._featureId = pFeatureId;
            this._errGeo = pErrGeo;
        }

        public object ErrGeo
        {
            get
            {
                return this._errGeo;
            }
        }

        public string FeatureID
        {
            get
            {
                return this._featureId;
            }
        }
    }
}

