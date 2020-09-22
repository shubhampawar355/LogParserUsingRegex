using logParser;
using Moq;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace moqLogParser
{
    public class FileWriterTest
    {
        [Fact]
        public void DependencyToFileWriterResovledCorrectly()
        {
            var mockInfo = new Mock<IUserInput>();
            mockInfo.SetupGet(i => i.Destination).Returns(@".\log.csv").Verifiable("Failed to use");
            var fileWriter = new FileWriter(mockInfo.Object);
            mockInfo.VerifyAll();
        }
    }
}
