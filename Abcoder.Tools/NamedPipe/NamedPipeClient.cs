using Abcoder.Tools.ABLoger;
using Abcoder.Tools.ABUtility;
using Abcoder.Tools.NamedPipe.NPBean;
using Abcoder.Tools.NamedPipe.NPEnum;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Abcoder.Tools.NamedPipe
{
    /// <summary>
    /// 命名管道客户端对象
    /// </summary>
    public class NamedPipeClient
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object lockObj = new object();

        /// <summary>
        /// 消息发送（管道）
        /// </summary>
        /// <param name="applyBean">消息内容对象</param>
        /// <param name="name">命名管道名称,缺省值为appSettings的“PipeName”</param>
        /// <param name="timeout">结果等待超时时间</param>
        /// <returns>响应结果</returns>
        public static NPBaseReplyBean SendMessage(NPBaseApplyBean applyBean, string name = "", int timeout = 5000)
        {
            try
            {
                var pipeMessage = JsonConvert.SerializeObject(applyBean);
                ABLogHelper.WriteLog("管道消息Apply：" + pipeMessage, ABLogTypeEnum.NamedPipe);
                lock (lockObj)
                {
                    var result = "";
                    if (string.IsNullOrWhiteSpace(name))
                        name = ABConfigUtility.AppSettingsGetString("PipeName", "Default_Name");
                    using (var pipeClient = new NamedPipeClientStream("localhost", name, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None))
                    {
                        pipeClient.Connect(timeout);
                        using (var streamReader = new StreamReader(pipeClient))
                        using (var streamWriter = new StreamWriter(pipeClient))
                        {
                            streamWriter.WriteLine(pipeMessage);
                            streamWriter.Flush();
                            var readerTask = new Task<string>(streamReader.ReadLine);
                            readerTask.Start();
                            if (readerTask.Wait(15000))
                            {
                                result = readerTask.Result;
                                ABLogHelper.WriteLog($"管道消息Reply:" + result, ABLogTypeEnum.NamedPipe);
                            }
                            else
                            {
                                //todo 管道消息处理超时 特殊处理--暂不处理
                                ABLogHelper.WriteLog($"管道消息响应超时", ABLogTypeEnum.NamedPipe);
                            }
                        }
                        pipeClient.Close();
                    }
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return new NPBaseReplyBean(applyBean.MessageType, NPResultCodeEnum.Failure, "管道消息响应超时");
                    }
                    try
                    {
                        var replyBean = JsonConvert.DeserializeObject<NPBaseReplyBean>(result);
                        return replyBean;
                    }
                    catch (Exception ex)
                    {
                        ABLogHelper.WriteLog(ex.Message, ABLogTypeEnum.NamedPipe);
                        ABLogHelper.WriteLog("命名管道应答消息处理异常,Result:" + result, ABLogTypeEnum.NamedPipe);
                        return new NPBaseReplyBean(applyBean.MessageType, NPResultCodeEnum.Failure, "管道消息处理错误:" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = "命名管道操作异常:" + ex.Message;
                ABLogHelper.WriteLog(message, ABLogTypeEnum.NamedPipe);
                return new NPBaseReplyBean(applyBean.MessageType, NPResultCodeEnum.Failure, message);
            }
        }
    }
}
