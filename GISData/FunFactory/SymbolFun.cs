namespace FunFactory
{
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using stdole;
    using System;
    using System.Drawing;
    using Utilities;

    public class SymbolFun
    {
        private const string mClassName = "FunFactory.SymbolFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private IColor mFillColor;
        private IColor mFontColor;
        private IColor mLineColor;
        private IColor mMarkerColor;
        private IColor mOutlineColor;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private ITextSymbol mTextSymbol;

        internal SymbolFun()
        {
        }

        public Image CreateImageFromStyleGalleryClass(IStyleGalleryClass pStyleGalleryClass, object pSymbol, int iWidth, int iHeight, Color pBackColor)
        {
            try
            {
                if (pStyleGalleryClass == null)
                {
                    return null;
                }
                if (pSymbol == null)
                {
                    return null;
                }
                if (iWidth <= 0)
                {
                    iWidth = 0x20;
                }
                if (iHeight <= 0)
                {
                    iHeight = 0x20;
                }
                Bitmap image = null;
                image = new Bitmap(iWidth, iHeight);
                Graphics graphics = null;
                graphics = Graphics.FromImage(image);
                if (!pBackColor.IsEmpty)
                {
                    SolidBrush brush = new SolidBrush(pBackColor);
                    graphics.FillRectangle(brush, 0, 0, iWidth, iHeight);
                }
                IntPtr hdc = new IntPtr();
                hdc = graphics.GetHdc();
                if (hdc.ToInt64() == 0L)
                {
                    return null;
                }
                tagRECT rectangle = new tagRECT();
                try
                {
                    pStyleGalleryClass.Preview(pSymbol, int.Parse(hdc.ToInt64().ToString()), ref rectangle);
                }
                catch (Exception)
                {
                }
                graphics.ReleaseHdc(hdc);
                return image;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "CreateImageFromStyleGalleryClass", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public Image CreateImageFromSymbol(ISymbol pSymbol, int iWidth, int iHeight, Color pBackColor, int iGap)
        {
            try
            {
                if (pSymbol != null)
                {
                    if (iWidth == 0)
                    {
                        return null;
                    }
                    if (iHeight == 0)
                    {
                        return null;
                    }
                    Bitmap image = null;
                    image = new Bitmap(iWidth, iHeight);
                    Graphics graphics = null;
                    graphics = Graphics.FromImage(image);
                    if (!pBackColor.IsEmpty)
                    {
                        SolidBrush brush = new SolidBrush(pBackColor);
                        graphics.FillRectangle(brush, 0, 0, iWidth, iHeight);
                    }
                    long lDpi = 0L;
                    lDpi = (long) graphics.DpiX;
                    IntPtr hDC = new IntPtr();
                    hDC = graphics.GetHdc();
                    if (hDC.ToInt64() == 0L)
                    {
                        return null;
                    }
                    bool flag = false;
                    flag = this.DrawSymbolToDC(hDC, pSymbol, iWidth, iHeight, lDpi, iGap);
                    graphics.ReleaseHdc(hDC);
                    if (flag)
                    {
                        return image;
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "CreateImageFromSymbol", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public IGeometry CreateSymbolShape(ISymbol pSymbol, IEnvelope pEnvelope)
        {
            try
            {
                if (pSymbol == null)
                {
                    return null;
                }
                if (pEnvelope == null)
                {
                    return null;
                }
                if (pSymbol is IMarkerSymbol)
                {
                    IArea area = null;
                    area = pEnvelope as IArea;
                    return area.Centroid;
                }
                if ((pSymbol is ILineSymbol) | (pSymbol is ITextSymbol))
                {
                    IPoint point = null;
                    point = new PointClass();
                    point.PutCoords(pEnvelope.XMin, pEnvelope.YMax - (pEnvelope.Height / 2.0));
                    IPoint point2 = null;
                    point2 = new PointClass();
                    point2.PutCoords(pEnvelope.XMax, pEnvelope.YMax - (pEnvelope.Height / 2.0));
                    return new PolylineClass { 
                        FromPoint = point,
                        ToPoint = point2
                    };
                }
                return pEnvelope;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "CreateSymbolShape", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public unsafe ITransformation CreateTransFromDC(int iWidth, int iHeight, long lDpi)
        {
            try
            {
                if (iWidth == 0)
                {
                    return null;
                }
                if (iHeight == 0)
                {
                    return null;
                }
                if (lDpi == 0L)
                {
                    return null;
                }
                IEnvelope envelope = null;
                envelope = new EnvelopeClass();
                envelope.PutCoords(0.0, 0.0, (double) iWidth, (double) iHeight);
                tagRECT grect = new tagRECT();
                IDisplayTransformation transformation = null;
                transformation = new DisplayTransformationClass();
                IDisplayTransformation transformation2 = transformation;
                transformation2.VisibleBounds = envelope;
                transformation2.Bounds = envelope;
                transformation2.set_DeviceFrame(ref grect);
                transformation2.Resolution = lDpi;
                return transformation;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "CreateTransFromDC", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        public bool DrawSymbolToDC(IntPtr hDC, ISymbol pSymbol, int iWidth, int iHeight, long lDpi, int iGap)
        {
            try
            {
                if (hDC.ToInt64() == 0L)
                {
                    return false;
                }
                if (pSymbol == null)
                {
                    return false;
                }
                if (iWidth == 0)
                {
                    return false;
                }
                if (iHeight == 0)
                {
                    return false;
                }
                if (lDpi == 0L)
                {
                    return false;
                }
                IEnvelope pEnvelope = null;
                pEnvelope = new EnvelopeClass();
                pEnvelope.PutCoords((double) iGap, (double) iGap, (double) (iWidth - iGap), (double) (iHeight - iGap));
                IGeometry geometry = null;
                geometry = this.CreateSymbolShape(pSymbol, pEnvelope);
                if (geometry == null)
                {
                    return false;
                }
                ITransformation transformation = null;
                transformation = this.CreateTransFromDC(iWidth, iHeight, lDpi);
                if (transformation == null)
                {
                    return false;
                }
                pSymbol.SetupDC((int) hDC.ToInt64(), transformation);
                bool flag = false;
                try
                {
                    pSymbol.Draw(geometry);
                    flag = true;
                }
                catch (Exception)
                {
                    flag = false;
                }
                pSymbol.ResetDC();
                return flag;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DrawSymbolToDC", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private IColor GetDefaultColor(string sColorKey)
        {
            try
            {
                if (string.IsNullOrEmpty(sColorKey))
                {
                    return null;
                }
                Color black = Color.Black;
                black = Color.FromArgb(int.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("Symbol", sColorKey)));
                return GISFunFactory.ColorFun.GetRGBColor(black);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "GetDefaultColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        private ITextSymbol GetDefaultTextSymbol()
        {
            try
            {
                ITextSymbol symbol = null;
                symbol = new TextSymbolClass {
                    Angle = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "Angle")),
                    Color = this.DefaultFontColor,
                    Size = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "Size")),
                    Text = UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "Text")
                };
                if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextHorizontalAlignment.esriTHACenter.ToString())
                {
                    symbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                }
                else if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextHorizontalAlignment.esriTHAFull.ToString())
                {
                    symbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull;
                }
                else if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextHorizontalAlignment.esriTHALeft.ToString())
                {
                    symbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                }
                else if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextHorizontalAlignment.esriTHARight.ToString())
                {
                    symbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                }
                if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextVerticalAlignment.esriTVABaseline.ToString())
                {
                    symbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                }
                else if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextVerticalAlignment.esriTVABottom.ToString())
                {
                    symbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                }
                else if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextVerticalAlignment.esriTVACenter.ToString())
                {
                    symbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                }
                else if (UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "HorizontalAlignment") == esriTextVerticalAlignment.esriTVATop.ToString())
                {
                    symbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                }
                IFontDisp font = null;
                if (symbol.Font == null)
                {
                    font = new StdFontClass() as IFontDisp;
                }
                else
                {
                    font = symbol.Font;
                }
                font.Name = UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "FontName");
                font.Size = decimal.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "FontSize"));
                font.Bold = bool.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "FontBold"));
                font.Italic = bool.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "FontItalic"));
                font.Underline = bool.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "FontUnderline"));
                font.Strikethrough = bool.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "FontStrikethrough"));
                symbol.Font = font;
                IFormattedTextSymbol symbol2 = null;
                symbol2 = symbol as IFormattedTextSymbol;
                if (esriTextDirection.esriTDAngle.ToString() == UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "Direction"))
                {
                    symbol2.Direction = esriTextDirection.esriTDAngle;
                }
                else if (esriTextDirection.esriTDHorizontal.ToString() == UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "Direction"))
                {
                    symbol2.Direction = esriTextDirection.esriTDHorizontal;
                }
                else if (esriTextDirection.esriTDVertical.ToString() == UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "Direction"))
                {
                    symbol2.Direction = esriTextDirection.esriTDVertical;
                }
                ISimpleTextSymbol symbol3 = null;
                symbol3 = symbol as ISimpleTextSymbol;
                symbol3.XOffset = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "XOffset"));
                symbol3.YOffset = double.Parse(UtilFactory.GetConfigOpt().GetConfigValue2("TextSymbol", "YOffset"));
                return symbol;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "GetDefaultTextSymbol", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return null;
            }
        }

        private bool SetDefaultColor(string sColorKey, IColor pNewColor)
        {
            try
            {
                if (string.IsNullOrEmpty(sColorKey))
                {
                    return false;
                }
                if (pNewColor == null)
                {
                    return false;
                }
                if (!(pNewColor is IRgbColor))
                {
                    return false;
                }
                IRgbColor color = null;
                color = pNewColor as IRgbColor;
                Color black = Color.Black;
                black = Color.FromArgb(color.Red, color.Green, color.Blue);
                UtilFactory.GetConfigOpt().SetConfigValue(sColorKey, black.ToArgb().ToString());
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "SetDefaultColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool SetDefaultTextSymbol(ITextSymbol pTextSymbol)
        {
            try
            {
                if (pTextSymbol == null)
                {
                    return false;
                }
                UtilFactory.GetConfigOpt().SetConfigValue("Angle", pTextSymbol.Angle.ToString());
                this.DefaultFontColor = pTextSymbol.Color;
                UtilFactory.GetConfigOpt().SetConfigValue("Size", pTextSymbol.Size.ToString());
                UtilFactory.GetConfigOpt().SetConfigValue("Text", pTextSymbol.Text.ToString());
                UtilFactory.GetConfigOpt().SetConfigValue("HorizontalAlignment", pTextSymbol.HorizontalAlignment.ToString());
                UtilFactory.GetConfigOpt().SetConfigValue("VerticalAlignment", pTextSymbol.VerticalAlignment.ToString());
                if (pTextSymbol.Font != null)
                {
                    UtilFactory.GetConfigOpt().SetConfigValue("FontName", pTextSymbol.Font.Name);
                    UtilFactory.GetConfigOpt().SetConfigValue("FontSize", pTextSymbol.Font.Size.ToString());
                    UtilFactory.GetConfigOpt().SetConfigValue("FontBold", pTextSymbol.Font.Bold.ToString());
                    UtilFactory.GetConfigOpt().SetConfigValue("FontItalic", pTextSymbol.Font.Italic.ToString());
                    UtilFactory.GetConfigOpt().SetConfigValue("FontUnderline", pTextSymbol.Font.Underline.ToString());
                    UtilFactory.GetConfigOpt().SetConfigValue("FontStrikethrough", pTextSymbol.Font.Strikethrough.ToString());
                }
                IFormattedTextSymbol symbol = null;
                symbol = pTextSymbol as IFormattedTextSymbol;
                UtilFactory.GetConfigOpt().SetConfigValue("Direction", symbol.Direction.ToString());
                ISimpleTextSymbol symbol2 = null;
                symbol2 = pTextSymbol as ISimpleTextSymbol;
                UtilFactory.GetConfigOpt().SetConfigValue("XOffset", symbol2.XOffset.ToString());
                UtilFactory.GetConfigOpt().SetConfigValue("YOffset", symbol2.YOffset.ToString());
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "SetDefaultTextSymbol", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        public IColor DefaultFillColor
        {
            get
            {
                try
                {
                    if (this.mFillColor == null)
                    {
                        this.mFillColor = this.GetDefaultColor("FillColor");
                    }
                    if (this.mFillColor == null)
                    {
                        this.mFillColor = GISFunFactory.ColorFun.GetRGBColor(0xff, 0xff, 190, true);
                    }
                    return (GISFunFactory.SystemFun.CloneObejct(this.mFillColor as IClone) as IColor);
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultFillColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.mFillColor = GISFunFactory.SystemFun.CloneObejct(value as IClone) as IColor;
                        this.SetDefaultColor("FillColor", this.mFillColor);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultFillColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public IColor DefaultFontColor
        {
            get
            {
                try
                {
                    if (this.mFontColor == null)
                    {
                        this.mFontColor = this.GetDefaultColor("FontColor");
                    }
                    if (this.mFontColor == null)
                    {
                        this.mFontColor = GISFunFactory.ColorFun.GetRGBColor(0, 0, 0, true);
                    }
                    return (GISFunFactory.SystemFun.CloneObejct(this.mFontColor as IClone) as IColor);
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultFontColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.mFontColor = GISFunFactory.SystemFun.CloneObejct(value as IClone) as IColor;
                        this.SetDefaultColor("FontColor", this.mFontColor);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultFontColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public IColor DefaultLineColor
        {
            get
            {
                try
                {
                    if (this.mLineColor == null)
                    {
                        this.mLineColor = this.GetDefaultColor("LineColor");
                    }
                    if (this.mLineColor == null)
                    {
                        this.mLineColor = GISFunFactory.ColorFun.GetRGBColor(0, 0, 0, true);
                    }
                    return (GISFunFactory.SystemFun.CloneObejct(this.mLineColor as IClone) as IColor);
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultLineColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.mLineColor = GISFunFactory.SystemFun.CloneObejct(value as IClone) as IColor;
                        this.SetDefaultColor("LineColor", this.mLineColor);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultLineColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public IColor DefaultMarkerColor
        {
            get
            {
                try
                {
                    if (this.mMarkerColor == null)
                    {
                        this.mMarkerColor = this.GetDefaultColor("MarkerColor");
                    }
                    if (this.mMarkerColor == null)
                    {
                        this.mMarkerColor = GISFunFactory.ColorFun.GetRGBColor(0, 0, 0, true);
                    }
                    return (GISFunFactory.SystemFun.CloneObejct(this.mMarkerColor as IClone) as IColor);
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultMarkerColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.mMarkerColor = GISFunFactory.SystemFun.CloneObejct(value as IClone) as IColor;
                        this.SetDefaultColor("MarkerColor", this.mMarkerColor);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultMarkerColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public IColor DefaultOutlineColor
        {
            get
            {
                try
                {
                    if (this.mOutlineColor == null)
                    {
                        this.mOutlineColor = this.GetDefaultColor("OutlineColor");
                    }
                    if (this.mOutlineColor == null)
                    {
                        this.mOutlineColor = GISFunFactory.ColorFun.GetRGBColor(0, 0, 0, true);
                    }
                    return (GISFunFactory.SystemFun.CloneObejct(this.mOutlineColor as IClone) as IColor);
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultOutlineColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.mOutlineColor = GISFunFactory.SystemFun.CloneObejct(value as IClone) as IColor;
                        this.SetDefaultColor("OutlineColor", this.mOutlineColor);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultOutlineColor", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }

        public ITextSymbol DefaultTextSymbol
        {
            get
            {
                try
                {
                    if (this.mTextSymbol == null)
                    {
                        this.mTextSymbol = this.GetDefaultTextSymbol();
                    }
                    if (this.mTextSymbol == null)
                    {
                        this.mTextSymbol = new TextSymbolClass();
                        this.mTextSymbol.Angle = 0.0;
                        this.mTextSymbol.Color = this.DefaultFontColor;
                        this.mTextSymbol.Size = 10.0;
                        this.mTextSymbol.Text = "文本";
                        this.mTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        this.mTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                    }
                    return (GISFunFactory.SystemFun.CloneObejct(this.mTextSymbol as IClone) as ITextSymbol);
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultTextSymbol", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.mTextSymbol = GISFunFactory.SystemFun.CloneObejct(value as IClone) as ITextSymbol;
                        this.SetDefaultTextSymbol(this.mTextSymbol);
                    }
                }
                catch (Exception exception)
                {
                    this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.SymbolFun", "DefaultTextSymbol", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                }
            }
        }
    }
}

