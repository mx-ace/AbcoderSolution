using System;

namespace Abcoder.Tools.ABLoger
{
    /// <summary>
    /// 日志数据对象
    /// </summary>
    public class ABLogBean
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public Enum Type;
        /// <summary>
        /// 日志的记录人时间
        /// </summary>
        public DateTime Dt;
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Msg;

        /// <summary>
        /// 日志内容
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        public ABLogBean(string message, Enum logType)
        {
            Dt = DateTime.Now;
            Msg = message;
            Type = logType;
        }
    }
}
