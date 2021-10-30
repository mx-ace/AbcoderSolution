using Abcoder.Tools.NamedPipe.NPEnum;

namespace Abcoder.Tools.NamedPipe.NPBean
{
    /// <summary>
    /// 命名管道应答消息基类
    /// </summary>
    public class NPBaseReplyBean : NPBaseBean
    {
        public NPBaseReplyBean() { }

        /// <summary>
        /// 构建一个操作执行的应答结果对象
        /// </summary>
        /// <param name="messageType">应答对应的操作类型</param>
        /// <param name="resultCode">操作结果</param>
        /// <param name="message">结果对应的文本信息</param>
        public NPBaseReplyBean(NPMessageTypeEnum messageType, NPResultCodeEnum resultCode, string message)
        {
            MessageType = messageType;
            ResultCode = resultCode;
            Message = message;
        }

        /// <summary>
        /// 当前应答是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return ResultCode == NPResultCodeEnum.Success;
            }
        }

        /// <summary>
        /// 请求执行的结果
        /// </summary>
        public NPResultCodeEnum ResultCode { get; set; }

        /// <summary>
        /// 应答数据结果
        /// </summary>
        public string ReplyData { get; set; }

        /// <summary>
        /// 执行结果的文本消息
        /// </summary>
        public string Message { get; set; }
    }
}
