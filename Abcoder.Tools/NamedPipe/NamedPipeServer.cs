using Abcoder.Tools.ABLoger;
using Abcoder.Tools.ABUtility;
using Abcoder.Tools.NamedPipe.NPBean;
using Abcoder.Tools.NamedPipe.NPEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Abcoder.Tools.NamedPipe
{
    /// <summary>
    /// 命名管道服务端
    /// </summary>
    public class NamedPipeServer
    {
        private static NamedPipeServer _instance;

        /// <summary>
        /// 命名管道处理方法
        /// </summary>
        private Func<NPBaseApplyBean, NPBaseReplyBean> messageHandler;

        private static int pipeCount = 5;

        /// <summary>
        /// 够着函数私有化
        /// </summary>
        private NamedPipeServer()
        {
        }

        private static readonly object lockObj = new object();
        /// <summary>
        /// 单例对象
        /// </summary>
        public static NamedPipeServer Instance
        {
            get
            {
                if (_instance == null)
                    lock (lockObj)
                    {
                        if (_instance == null)
                            _instance = new NamedPipeServer();
                    }
                return _instance;
            }
        }

        /// <summary>
        /// 命名管道工作任务列表
        /// </summary>
        public readonly List<Task> NamedPipeWorkTasks = new List<Task>();

        /// <summary>
        /// 开始管道处理线程
        /// </summary>
        /// <param name="action">管道消息处理函数</param>
        /// <param name="name">命名管道名称</param>
        /// <param name="count">设置管道数量</param>
        public static void StartService(Func<NPBaseApplyBean, NPBaseReplyBean> action, string name, int count = 5)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("命名管道名称不能为空");
            pipeCount = count;
            ABLogHelper.WriteLog("开启命名管道服务", ABLogTypeEnum.NamedPipe);
            //业务处理逻辑对象
            Instance.messageHandler = action;
            for (var i = 0; i < count; i++)
            {
                Instance.NamedPipeWorkTasks.Add(Task.Factory.StartNew(BusinessHandler, name));
            }
        }

        /// <summary>
        /// 命名管道业务处理
        /// </summary>
        /// <param name="count"></param>
        private static void BusinessHandler(object pipeName)
        {
            try
            {
                var pipeServer = new NamedPipeServerStream(pipeName.ToString(), PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
                ABLogHelper.WriteLog("管道等待链接：" + pipeName, ABLogTypeEnum.NamedPipe);
                while (true)
                {
                    //监听命名管道用户连接
                    pipeServer.WaitForConnection();
                    try
                    {
                        lock (pipeServer)
                        {
                            var pipeStatus = new NamedPipeTaskState(pipeServer);
                            Task.Factory.StartNew((ts) =>
                            {
                                var taskState = (NamedPipeTaskState)ts;
                                while (true)
                                {
                                    var pipeMessage = taskState.Reader.ReadLine();
                                    if (string.IsNullOrWhiteSpace(pipeMessage))
                                    {
                                        ABLogHelper.WriteLog("命名管道断开", ABLogTypeEnum.NamedPipe);
                                        pipeServer.Disconnect();
                                        break;
                                    }
                                    //等待消息异步处理任务完成
                                    NamedPipeMessageHandlerAsync(pipeMessage, taskState).Wait();
                                    //等待客户端的下一次管道消息
                                }
                            }, pipeStatus).Wait();
                        }
                    }
                    catch (Exception e)
                    {
                        //业务处理错误
                        ABLogHelper.WriteLog(e.Message, ABLogTypeEnum.NamedPipe);
                    }
                }
            }
            catch (Exception e)
            {
                //配置错误
                ABLogHelper.WriteLog($"命名管道服务开启失败[msg]:{e.Message}", ABLogTypeEnum.NamedPipe);
            }
        }
        /// <summary>
        /// 命名管道业务处理函数
        /// </summary>
        /// <param name="pipeMessage">命名管道消息</param>
        /// <param name="taskStatus">任务状态类</param>
        private static async Task<bool> NamedPipeMessageHandlerAsync(string pipeMessage, NamedPipeTaskState taskStatus)
        {
            ABLogHelper.WriteLog("管道消息[Apply]：" + pipeMessage, ABLogTypeEnum.NamedPipe);
            if (pipeMessage == null) return false;
            try
            {
                var applyBean = JsonConvert.DeserializeObject<NPBaseApplyBean>(pipeMessage);
                var resultData = await Task.Factory.StartNew(
                    (arg) => Instance.messageHandler.Invoke((NPBaseApplyBean)arg), applyBean);
                //等待处理
                //未返回结果消息,则直接等待下一消息
                if (resultData == null)
                    resultData = new NPBaseReplyBean(applyBean.MessageType, NPResultCodeEnum.Failure, "未知错误");
                string replyResult = JsonConvert.SerializeObject(resultData);
                ABLogHelper.WriteLog("管道消息[Reply]：" + replyResult, ABLogTypeEnum.NamedPipe);
                lock (taskStatus)
                {
                    //向客户端发送结果消息
                    taskStatus.Writer.WriteLine(replyResult);
                    taskStatus.Writer.Flush();
                    return true;
                }
            }
            catch (Exception e)
            {
                ABLogHelper.WriteLog(e.Message, ABLogTypeEnum.NamedPipe);
                lock (taskStatus)
                {
                    //向客户端发送结果消息
                    taskStatus.Writer.WriteLine($"命令执行错误:{e.Message}");
                    taskStatus.Writer.Flush();
                    return false;
                }
            }
        }

    }
}
