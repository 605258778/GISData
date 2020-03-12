namespace FunFactory
{
    using Microsoft.VisualBasic;
    using System;

    /// <summary>
    /// GIS初始化各种对象的工厂类
    /// </summary>
    public class GISFunFactory
    {
        private const string mClassName = "FunFactory.GISFunFactory";
        private static FunFactory.AssemblyFun sAssemblyFun;
        private static FunFactory.ColorFun sColorFun;
        private static FunFactory.CoreFun sCoreFun;
        private static FunFactory.ElementFun sElementFun;
        private static FunFactory.FeatureFun sFeatureFun;
        private static FunFactory.FeedbackFun sFeedbackFun;
        private static FunFactory.FlashFun sFlashFun;
        private static FunFactory.FormatFun sFormatFun;
        private static FunFactory.GeometryFun sGeometryFun;
        private static FunFactory.LayerFun sLayerFun;
        private static FunFactory.MetadataFun sMetadataFun;
        private static FunFactory.RasterFun sRasterFun;
        private static FunFactory.SymbolFun sSymbolFun;
        private static FunFactory.SystemFun sSystemFun;
        private static FunFactory.UnitFun sUnitFun;
        private static FunFactory.WorkspaceFun sWorkspaceFun;

        public static FunFactory.AssemblyFun AssemblyFun
        {
            get
            {
                try
                {
                    if (sAssemblyFun == null)
                    {
                        sAssemblyFun = new FunFactory.AssemblyFun();
                    }
                    return sAssemblyFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property AssemblyFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.ColorFun ColorFun
        {
            get
            {
                try
                {
                    if (sColorFun == null)
                    {
                        sColorFun = new FunFactory.ColorFun();
                    }
                    return sColorFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property ColorFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.CoreFun CoreFun
        {
            get
            {
                try
                {
                    if (sCoreFun == null)
                    {
                        sCoreFun = new FunFactory.CoreFun();
                    }
                    return sCoreFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property CoreFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.ElementFun ElementFun
        {
            get
            {
                try
                {
                    if (sElementFun == null)
                    {
                        sElementFun = new FunFactory.ElementFun();
                    }
                    return sElementFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property ElementFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.FeatureFun FeatureFun
        {
            get
            {
                try
                {
                    if (sFeatureFun == null)
                    {
                        sFeatureFun = new FunFactory.FeatureFun();
                    }
                    return sFeatureFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property FeatureFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.FeedbackFun FeedbackFun
        {
            get
            {
                try
                {
                    if (sFeedbackFun == null)
                    {
                        sFeedbackFun = new FunFactory.FeedbackFun();
                    }
                    return sFeedbackFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property FeedbackFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.FlashFun FlashFun
        {
            get
            {
                try
                {
                    if (sFlashFun == null)
                    {
                        sFlashFun = new FunFactory.FlashFun();
                    }
                    return sFlashFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property FlashFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.FormatFun FormatFun
        {
            get
            {
                try
                {
                    if (sFormatFun == null)
                    {
                        sFormatFun = new FunFactory.FormatFun();
                    }
                    return sFormatFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property FormatFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.GeometryFun GeometryFun
        {
            get
            {
                try
                {
                    if (sGeometryFun == null)
                    {
                        sGeometryFun = new FunFactory.GeometryFun();
                    }
                    return sGeometryFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property GeometryFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.LayerFun LayerFun
        {
            get
            {
                try
                {
                    if (sLayerFun == null)
                    {
                        sLayerFun = new FunFactory.LayerFun();
                    }
                    return sLayerFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property LayerFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.MetadataFun MetadataFun
        {
            get
            {
                try
                {
                    if (sMetadataFun == null)
                    {
                        sMetadataFun = new FunFactory.MetadataFun();
                    }
                    return sMetadataFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property MetadataFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.RasterFun RasterFun
        {
            get
            {
                try
                {
                    if (sRasterFun == null)
                    {
                        sRasterFun = new FunFactory.RasterFun();
                    }
                    return sRasterFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property RasterFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.SymbolFun SymbolFun
        {
            get
            {
                try
                {
                    if (sSymbolFun == null)
                    {
                        sSymbolFun = new FunFactory.SymbolFun();
                    }
                    return sSymbolFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property SymbolFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.SystemFun SystemFun
        {
            get
            {
                try
                {
                    if (sSystemFun == null)
                    {
                        sSystemFun = new FunFactory.SystemFun();
                    }
                    return sSystemFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property SystemFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        public static FunFactory.UnitFun UnitFun
        {
            get
            {
                try
                {
                    if (sUnitFun == null)
                    {
                        sUnitFun = new FunFactory.UnitFun();
                    }
                    return sUnitFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property UnitFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }

        /// <summary>
        /// 初始化GIS的工作空间
        /// </summary>
        public static FunFactory.WorkspaceFun WorkspaceFun
        {
            get
            {
                try
                {
                    if (sWorkspaceFun == null)
                    {
                        sWorkspaceFun = new FunFactory.WorkspaceFun();
                    }
                    return sWorkspaceFun;
                }
                catch (Exception exception)
                {
                    Interaction.MsgBox("错误类　 : FunFactory.GISFunFactory\r\n错误出处 : Property WorkspaceFun\r\n错误来源 : " + exception.Source + "\r\n错误描述 : " + exception.Message, MsgBoxStyle.Exclamation, "错误");
                    return null;
                }
            }
        }
    }
}

