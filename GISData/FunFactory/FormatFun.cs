namespace FunFactory
{
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using Microsoft.VisualBasic;
    using System;
    using Utilities;

    public class FormatFun
    {
        private const string mClassName = "FunFactory.FormatFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal FormatFun()
        {
        }

        public string FormatMapCoordinate(IPoint pPoint, esriUnits eInUnits)
        {
            try
            {
                if (pPoint == null)
                {
                    return "";
                }
                return this.FormatMapCoordinate(pPoint.X, pPoint.Y, eInUnits);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FormatFun", "FormatMapCoordinate", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public string FormatMapCoordinate(double dMapValue, esriUnits eInUnits)
        {
            try
            {
                double num = 0.0;
                if (eInUnits == esriUnits.esriDecimalDegrees)
                {
                    num = dMapValue;
                }
                else
                {
                    num = GISFunFactory.UnitFun.ConvertESRIUnits(dMapValue, eInUnits, esriUnits.esriDecimalDegrees);
                }
                return this.FormatMapDegreeMinuteSecond(num);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FormatFun", "FormatMapCoordinate", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public string FormatMapCoordinate(double dMapX, double dMapY, esriUnits eInUnits)
        {
            try
            {
                string str = null;
                string str2 = null;
                str = this.FormatMapCoordinate(dMapX, eInUnits);
                str2 = this.FormatMapCoordinate(dMapY, eInUnits);
                return (str + " " + str2);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FormatFun", "FormatMapCoordinate", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public string FormatMapDegreeMinuteSecond(double Value)
        {
            try
            {
                string str = null;
                if (Value >= 0.0)
                {
                    str = "";
                }
                else
                {
                    str = "-";
                }
                Value = Math.Abs(Value);
                long num = 0L;
                num = Convert.ToInt64((double) (Value - 0.5));
                long expression = 0L;
                expression = Convert.ToInt64((double) (((Value - num) * 60.0) - 0.5));
                long num3 = 0L;
                num3 = Convert.ToInt64((double) (((((Value - num) * 60.0) - expression) * 60.0) * 10.0)) / 10L;
                return (str + Convert.ToString(num) + "\x00b0" + Strings.Format(expression, "00") + "′" + Strings.Format(num3, "00") + "″");
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.FormatFun", "FormatMapDegreeMinuteSecond", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }
    }
}

