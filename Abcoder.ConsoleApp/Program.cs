using Abcoder.Tools.ABLoger;
using Abcoder.Tools.ABUtility;
using Abcoder.Tools.NamedPipe;
using Abcoder.Tools.NamedPipe.NPBean;
using Abcoder.Tools.NamedPipe.NPEnum;
using Newtonsoft.Json;
using System;

namespace Abcoder.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //NamedPipeServer.StartService(PipePassBeanHandler);
                while (true) {
                    Console.Write("Key:");
                    var key = Console.ReadKey();
                    var applyBean = new NPBaseApplyBean()
                    {
                        MessageType = NPMessageTypeEnum.Json,
                        ApplyData = $"DT:{DateTime.Now:yyy-MM-dd HH:mm:ss}"
                    };
                    var result = NamedPipeClient.SendMessage(applyBean, "Abcoder");
                    Console.WriteLine("Result:" + result.ReplyData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static NPBaseReplyBean PipePassBeanHandler(NPBaseApplyBean applyBean)
        {
            ABLogHelper.WriteLog($"管道消息：{JsonConvert.SerializeObject(applyBean)}", ABLogTypeEnum.Mark);
            return new NPBaseReplyBean()
            {
                ResultCode = NPResultCodeEnum.Success
            };
        }
    }
}
