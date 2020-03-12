namespace FunFactory
{
    using System;

    /// <summary>
    /// 不同类型工作空间的枚举类
    /// </summary>
    public enum WorkspaceSource : long
    {
        /// <summary>
        /// Access工作空间工厂类=1L
        /// </summary>
        esriWSAccessWorkspaceFactory = 1L,
        /// <summary>
        /// 微软工作空间工厂类=2L
        /// </summary>
        esriWSAMSWorkspaceFactory = 2L,
        /// <summary>
        /// ArcInfo工作空间工厂类=3L
        /// </summary>
        esriWSArcInfoWorkspaceFactory = 3L,
        esriWSCadWorkspaceFactory = 4L,
        esriWSFileGDBSqlWorkspaceFactory = 0x13L,
        esriWSFileGDBWorkspaceFactory = 0x12L,
        esriWSIMSWorkspaceFactory = 5L,
        esriWSOLEDBWorkspaceFactory = 6L,
        esriWSPCCoverageWorkspaceFactory = 7L,
        esriWSPlugInWorkspaceFactory = 8L,
        esriWSRasterWorkspaceFactory = 9L,
        esriWSSDCWorkspaceFactory = 10L,
        esriWSSdeWorkspaceFactory = 11L,
        esriWSShapefileWorkspaceFactory = 12L,
        /// <summary>
        /// 维基地图的工作空间=13L
        /// </summary>
        esriWSStreetMapWorkspaceFactory = 13L,
        esriWSTextFileWorkspaceFactory = 14L,
        esriWSTinWorkspaceFactory = 15L,
        esriWSToolboxWorkspaceFactory = 0x10L,
        esriWSVpfWorkspaceFactory = 0x11L
    }
}

