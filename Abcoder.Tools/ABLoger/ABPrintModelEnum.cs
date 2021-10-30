namespace Abcoder.Tools.ABLoger
{
    /// <summary>
    /// 日志打印模式
    /// </summary>
    public enum ABPrintModelEnum
    {
        /// <summary>
        /// 自动,根据日志队列参数确定是否打印控制台
        /// </summary>
        Auto = 0,
        /// <summary>
        /// 把所有日志都打印到控制台
        /// </summary>
        PrintAll = 1,
        /// <summary>
        /// 禁用打印,不打印任何日志信息到控制台
        /// </summary>
        PrintDisable = 2
    }
}
