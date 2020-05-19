namespace TopologyCheck.Checker
{
    using System;

    public class GapErrorEntity
    {
        private object _errGeo;
        private string _featureId;
        private string _idname;

        public GapErrorEntity(string idname, string pFeatureId, object pErrGeo)
        {
            this._idname = idname;
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

        public string idname
        {
            get
            {
                return this._idname;
            }
        }
    }
}

