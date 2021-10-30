namespace Abcoder.Tools.NamedPipe.NPEnum
{
    /// <summary>
    /// 命名管道应答消息状态嘛
    /// </summary>
    public enum NPResultCodeEnum
    {
        /// <summary>
        /// 业务处理成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 处理失败
        /// </summary>
        Failure = 500,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParamError = 501,

        /// <summary>
        /// 设备未认证
        /// </summary>
        Unautherized = 505,
    }
}
