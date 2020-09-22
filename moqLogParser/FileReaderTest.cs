using logParser;
using Moq;
using System;
using Xunit;

namespace moqLogParser
{
    public class FileReaderTest
    {
        [Fact]
        public void DependencyToFileReaderResovledCorrectly()
        {
            var mockInfo = new Mock<IUserInput>();
            mockInfo.SetupGet(i => i.Destination).Returns(@".\log.csv").Verifiable("Failed to use Destination");
            mockInfo.SetupGet(i => i.Source).Returns(@".\logs\").Verifiable("Failed to use Source");
            var fileWriter = new FileReader(mockInfo.Object);
            mockInfo.VerifyAll();
        }
    }
}
