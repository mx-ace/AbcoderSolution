using Abcoder.Tools.NamedPipe;
using Abcoder.Tools.NamedPipe.NPBean;
using Abcoder.Tools.NamedPipe.NPEnum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Abcoder.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var applyBean = new NPBaseApplyBean()
            {
                MessageType = NPMessageTypeEnum.Json
            };
            var result = NamedPipeClient.SendMessage(applyBean, "");
            Console.WriteLine("");
        }
    }
}
