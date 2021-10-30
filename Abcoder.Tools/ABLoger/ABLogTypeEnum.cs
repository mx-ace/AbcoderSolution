namespace Abcoder.Tools.ABLoger
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum ABLogTypeEnum
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        Normal,
        /// <summary>
        /// 标记日志
        /// </summary>
        Mark,

        /// <summary>
        /// 异常日志
        /// </summary>
        Exception,

        /// <summary>
        /// 配置文件日志
        /// </summary>
        Config,

        /// <summary>
        /// 命名管道
        /// </summary>
        NamedPipe
    }
}
