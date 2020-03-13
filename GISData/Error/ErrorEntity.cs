namespace TopologyCheck.Error
{
    using System;

    public class ErrorEntity
    {
        private string _errMsg;
        private string _errPos;
        private TopologyCheck.Error.ErrType _errType;
        private string _featureId;
        private object _errGeo;

        public ErrorEntity(string pFeatureId, string pErrMsg, string pErrPos, TopologyCheck.Error.ErrType pErrType, object pErrGeo)
        {
            this._featureId = pFeatureId;
            this._errMsg = pErrMsg;
            this._errPos = pErrPos;
            this._errType = pErrType;
            this._errGeo = pErrGeo;
        }

        public string ErrMsg
        {
            get
            {
                return this._errMsg;
            }
            set
            {
                this._errMsg = value;
            }
        }

        public string ErrPos
        {
            get
            {
                return this._errPos;
            }
        }

        public TopologyCheck.Error.ErrType ErrType
        {
            get
            {
                return this._errType;
            }
        }

        public string FeatureID
        {
            get
            {
                return this._featureId;
            }
        }
        public object ErrGeo
        {
            get
            {
                return this._errGeo;
            }
        }
    }
}

