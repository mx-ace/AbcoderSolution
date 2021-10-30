using Abcoder.Tools.NamedPipe.NPEnum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Abcoder.Tools.NamedPipe.NPBean
{
    /// <summary>
    /// 命令管道数据基类
    /// </summary>
    public class NPBaseBean
    {
        internal NPBaseBean()
        {

        }

        public NPBaseBean(NPMessageTypeEnum actionCode)
        {
            MessageType = actionCode;
        }

        /// <summary>
        /// 业务标识
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public NPMessageTypeEnum MessageType { get; set; }

    }
}
