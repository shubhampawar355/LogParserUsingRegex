using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LogParser.MoqTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<ILogParser>();
        }
    }
}
