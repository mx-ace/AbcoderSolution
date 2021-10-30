using System.IO;

namespace Abcoder.Tools.NamedPipe
{
    /// <summary>
    /// 命名管道任务状态对象
    /// </summary>
    public class NamedPipeTaskState
    {
        /// <summary>
        /// 构造命名管道业务处理任务的状态对象
        /// </summary>
        /// <param name="serverStream">命名管道服务端对象</param>
        public NamedPipeTaskState(Stream serverStream)
        {
            Reader = new StreamReader(serverStream);
            Writer = new StreamWriter(serverStream);
        }

        /// <summary>
        /// 数据读取对象
        /// </summary>
        public StreamReader Reader { get; set; }

        /// <summary>
        /// 数据写入队形
        /// </summary>
        public StreamWriter Writer { get; set; }
    }
}
