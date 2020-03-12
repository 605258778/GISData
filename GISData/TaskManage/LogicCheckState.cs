namespace TaskManage
{
    using System;

    /// <summary>
    /// 日志检查状态的枚举类
    /// </summary>
    public enum LogicCheckState : long
    {
        /// <summary>
        /// 日志检查失败
        /// </summary>
        Failure = 0L,
        /// <summary>
        /// 日志检查成功
        /// </summary>
        Success = 1L
    }
}

