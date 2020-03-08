namespace TopologyCheck.Error
{
    using System;

    public class ErrorEntity
    {
        private string _errMsg;
        private string _errPos;
        private TopologyCheck.Error.ErrType _errType;
        private string _featureId;

        public ErrorEntity(string pFeatureId, string pErrMsg, string pErrPos, TopologyCheck.Error.ErrType pErrType)
        {
            this._featureId = pFeatureId;
            this._errMsg = pErrMsg;
            this._errPos = pErrPos;
            this._errType = pErrType;
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
    }
}

