namespace TaskManage
{
    using System;

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskState : long
    {
        /// <summary>
        /// 任务状态:关闭
        /// </summary>
        Close = 0L,
        /// <summary>
        /// 任务状态:开启
        /// </summary>
        Open = 1L
    }
}

