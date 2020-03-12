namespace FunFactory
{
    using ESRI.ArcGIS.Display;
    using System;
    using System.Drawing;
    using Utilities;

    public class ColorFun
    {
        private const string mClassName = "FunFactory.ColorFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal ColorFun()
        {
        }

        private int CheckNumValueRegion(int iValue, int iMinValue, int iMaxValue)
        {
            try
            {
                if (iValue < iMinValue)
                {
                    iValue = iMinValue;
                }
                if (iValue > iMaxValue)
                {
                    iValue = iMaxValue;
                }
                return iValue;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "CheckNumValueRegion", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return iValue;
            }
        }

        public ICmykColor GetCmykColor(int iCyan, int iMagenta, int iYellow, int iBlack, bool bUseWinDithering)
        {
            try
            {
                ICmykColor color = null;
                color = new CmykColorClass();
                iCyan = this.CheckNumValueRegion(iCyan, 0, 0xff);
                iMagenta = this.CheckNumValueRegion(iMagenta, 0, 0xff);
                iYellow = this.CheckNumValueRegion(iYellow, 0, 0xff);
                iBlack = this.CheckNumValueRegion(iBlack, 0, 0xff);
                color.Cyan = iCyan;
                color.Magenta = iMagenta;
                color.Yellow = iYellow;
                color.Black = iBlack;
                color.UseWindowsDithering = bUseWinDithering;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetCmykColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IGrayColor GetGrayColor(int iLevel, bool bUseWinDithering)
        {
            try
            {
                IGrayColor color = null;
                color = new GrayColorClass();
                iLevel = this.CheckNumValueRegion(iLevel, 0, 0xff);
                color.Level = iLevel;
                color.UseWindowsDithering = bUseWinDithering;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetGrayColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IHlsColor GetHlsColor(Color pColor)
        {
            try
            {
                if (pColor.IsEmpty)
                {
                    return null;
                }
                IHlsColor color = null;
                color = new HlsColorClass {
                    Hue = (int) pColor.GetHue(),
                    Lightness = ((int) pColor.GetBrightness()) * 100,
                    Saturation = ((int) pColor.GetSaturation()) * 100
                };
                IColor color2 = null;
                color2 = color;
                color2.Transparency = pColor.A;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetHlsColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IHlsColor GetHlsColor(int iHue, int iLightness, int iSaturation, bool bUseWinDithering)
        {
            try
            {
                IHlsColor color = null;
                color = new HlsColorClass();
                iHue = this.CheckNumValueRegion(iHue, 0, 360);
                iLightness = this.CheckNumValueRegion(iLightness, 0, 100);
                iSaturation = this.CheckNumValueRegion(iSaturation, 0, 100);
                color.Hue = iHue;
                color.Lightness = iLightness;
                color.Saturation = iSaturation;
                color.UseWindowsDithering = bUseWinDithering;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetHlsColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IHsvColor GetHsvColor(int iHue, int iSaturation, int iValue, bool bUseWinDithering)
        {
            try
            {
                IHsvColor color = null;
                color = new HsvColorClass();
                iHue = this.CheckNumValueRegion(iHue, 0, 360);
                iSaturation = this.CheckNumValueRegion(iSaturation, 0, 100);
                iValue = this.CheckNumValueRegion(iValue, 0, 100);
                color.Hue = iHue;
                color.Saturation = iSaturation;
                color.Value = iValue;
                color.UseWindowsDithering = bUseWinDithering;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetHsvColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IRgbColor GetRGBColor(Color pColor)
        {
            try
            {
                if (pColor.IsEmpty)
                {
                    return null;
                }
                IRgbColor color = null;
                color = new RgbColorClass {
                    Red = pColor.R,
                    Green = pColor.G,
                    Blue = pColor.B
                };
                IColor color2 = null;
                color2 = color;
                color2.Transparency = pColor.A;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetRGBColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IRgbColor GetRGBColor(int iRed, int iGreen, int iBlue, bool bUseWinDithering)
        {
            IRgbColor color = null;
            try
            {
                color = new RgbColorClass();
                iRed = this.CheckNumValueRegion(iRed, 0, 0xff);
                iGreen = this.CheckNumValueRegion(iGreen, 0, 0xff);
                iBlue = this.CheckNumValueRegion(iBlue, 0, 0xff);
                color.Red = iRed;
                color.Green = iGreen;
                color.Blue = iBlue;
                color.UseWindowsDithering = bUseWinDithering;
                return color;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetRGBColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return color;
            }
        }

        public Color GetSystemDrawingColor(IColor pESRIColor)
        {
            Color color = new Color();
            try
            {
                if (pESRIColor == null)
                {
                    return color;
                }
                int rGB = 0;
                pESRIColor.UseWindowsDithering = true;
                rGB = pESRIColor.RGB;
                Color baseColor = new Color();
                baseColor = ColorTranslator.FromWin32(rGB);
                return Color.FromArgb(pESRIColor.Transparency, baseColor);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.ColorFun", "GetSystemDrawingColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return color;
            }
        }
    }
}

