using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Abcoder.Tools.ABLoger
{
    public class ABLogHelper
    {
        /// <summary>
        /// 日志打印模式
        /// </summary>
        public static ABPrintModelEnum PrintMode = ABPrintModelEnum.Auto;

        /// <summary>
        /// 单例对象
        /// </summary>
        private static ABLogHelper instance;

        /// <summary>
        /// 日志记录队列
        /// </summary>
        static readonly BlockingCollection<ABLogBean> bufferQueue = new BlockingCollection<ABLogBean>(50);

        /// <summary>
        /// 日志记录文件名
        /// </summary>
        private static string logPath = "/RunLog";

        /// <summary>
        /// 日志记录线程
        /// </summary>
        static Task workTask;

        /// <summary>
        /// 项目根路径
        /// </summary>
        static readonly string AppBasePath = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly object lockObj = new object();
        /// <summary>
        /// 初始化日志处理类
        /// </summary>
        private static void InitLogHelper()
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new ABLogHelper();
                    workTask = new Task(WriteLogerAction);
                    workTask.Start();
                }

            }
        }

        static void WriteLogerAction()
        {
            while (!bufferQueue.IsCompleted)
            {
                RecordLog(bufferQueue.Take());
            }
        }

        private static readonly object lockObj2 = new object();
        static void RecordLog(ABLogBean logBean)
        {
            //日志文件名称
            var fileName = logBean.Dt.ToString("MM-dd-HH");
            var dirName = AppBasePath + logPath;
            lock (lockObj2)
            {
                if (!Directory.Exists(dirName +"/"+ fileName))
                    Directory.CreateDirectory(dirName + "/" + fileName);
            }
            var file = $"{dirName}/{fileName}/{logBean.Type}.html";
            lock (lockObj2)
            {
                try
                {
                    using (var stream = new FileStream(file, FileMode.Append))
                    {
                        using (var sw = new StreamWriter(stream, Encoding.UTF8))
                        {
                            sw.WriteLine($"[{logBean.Dt:HH:mm:ss:fff}][{logBean.Type}]\t{logBean.Msg}<br>");
                            sw.Flush();
                            sw.Close();
                            sw.Dispose();
                        }
                        stream.Close();
                        stream.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// 打印文本日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="type">日志类型,默认为normal</param>
        /// <param name="isPrint">是否把日志也打印到控制台</param>
        public static void WriteLog(string msg, Enum type, bool isPrint = true)
        {
            switch (PrintMode)
            {
                case ABPrintModelEnum.Auto:
                    if (isPrint)
                        goto case ABPrintModelEnum.PrintAll;
                    break;
                case ABPrintModelEnum.PrintAll:
                    Console.WriteLine();
                    Console.WriteLine(msg.Replace("<br>", "\n").Replace("<br/>", "\n"));
                    break;
                case ABPrintModelEnum.PrintDisable:
                    break;
            }
            var logModel = new ABLogBean(msg, type);
            if (instance == null)
            {
                InitLogHelper();
            }
            lock (bufferQueue)
            {
                bufferQueue.Add(logModel);
            }
        }


        /// <summary>
        /// 打印普通文本日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void WriteLog(string msg)
        {
            WriteLog(msg, ABLogTypeEnum.Normal);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        public static void WriteLog(Exception ex, Enum type = null)
        {
            WriteLog(ex.Message, type ?? ABLogTypeEnum.Exception);
        }

    }
}
