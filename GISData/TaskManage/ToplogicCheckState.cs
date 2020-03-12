namespace TaskManage
{
    using System;

    /// <summary>
    /// 顶部日志检查状态
    /// </summary>
    public enum ToplogicCheckState : long
    {
        /// <summary>
        /// 顶部日志检查状态:失败
        /// </summary>
        Failure = 0L,
        /// <summary>
        /// 顶部日志检查状态:成功
        /// </summary>
        Success = 1L
    }
}

