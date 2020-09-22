using Autofac.Extras.Moq;
using logParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace moqLogParser
{
    public class LogParserTest
    {

        [Fact]
        public void Parse_ShouldWork()
        {
            var input = new Mock<IUserInput>();
            input.SetupGet(u => u.Delemeter).Returns('|').Verifiable();
            input.SetupGet(u => u.UserGivenLevels).Returns(new HashSet<string> { "INFO", "ERROR" }).Verifiable();

            var reader = new Mock<IFileReader>();
            reader.Setup(r => r.GetAllLogFiles()).Returns(new string[] { "myfile" }).Verifiable();
            reader.Setup(r => r.ReadAllLines("myfile")).Returns(GetSampleInputLines()).Verifiable();
            reader.Setup(f => f.GetLastLogIdFromOldCSVFile()).Returns(4).Verifiable();

            List<string> list = new List<string>();

            var writer = new Mock<IFileWriter>();
            writer.Setup(w => w.WriteToFile(It.IsAny<List<string>>())).Callback<List<string>>((obj) => list = obj).Verifiable();

            var logParser = new Mock<LogParser>(input.Object, reader.Object, writer.Object);
            logParser.Object.Parse();
            input.VerifyAll();
            reader.VerifyAll();
            writer.VerifyAll();
            Assert.Equal(2, list.Count);
            Assert.True(list.SequenceEqual(GetSampleOutputLines()));
        }

        [Fact]
        public void Parse_ShouldNotWorkForEmptySource()
        {
            var input = new Mock<IUserInput>();
            input.SetupGet(u => u.Delemeter).Returns('|').Verifiable();
            input.SetupGet(u => u.UserGivenLevels).Returns(new HashSet<string> { "INFO", "ERROR" }).Verifiable();

            var reader = new Mock<IFileReader>();
            reader.Setup(r => r.GetAllLogFiles()).Returns(new string[] {}).Verifiable();

            var writer = new Mock<IFileWriter>();
            writer.Setup(w => w.WriteToFile(It.IsAny<List<string>>())).Verifiable();

            var logParser = new Mock<LogParser>(input.Object, reader.Object, writer.Object);
            logParser.Object.Parse();
            input.VerifyAll();
            reader.VerifyAll();
            writer.Verify(w => w.WriteToFile(It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public void AddLogsIfFileContainsLogs_ShouldWork()
        {
            var input = new Mock<IUserInput>();
            input.SetupGet(u => u.Source).Returns("myfile");
            input.SetupGet(u=>u.Delemeter).Returns('|');
            Log.SetLevelsToConsider(new HashSet<string> { "INFO", "ERROR" });
            var reader = new Mock<IFileReader>();
            reader.Setup(r => r.ReadAllLines("myfile")).Returns(GetSampleInputLines());
            reader.Setup(f => f.GetLastLogIdFromOldCSVFile()).Returns(4);

            var writer = new Mock<IFileWriter>();
            List<string> list=new List<string>();
            writer.Setup(w => w.WriteToFile(It.IsAny<List<string>>())).Callback<List<string>>((obj)=> list=obj);

            var logParser = new Mock<LogParser>(input.Object, reader.Object, writer.Object);
            logParser.Object.AddLogsIfFileContainsLogs("myfile");
            writer.Verify(w => w.WriteToFile(It.IsAny<List<string>>()), Times.Once());
            Assert.Equal(2, list.Count);
            Assert.True(list.SequenceEqual(GetSampleOutputLines()));
        }

        private List<string> GetSampleInputLines()
        {
            List<string> input = new List<string>
            {
               new string ("03/22 08:51:06 INFO   :...read_physical_netif: index #4, interface CTCD0 has address 9.67.116.98, ifidx 4" ),
               new string ( "03/22 08:51:06 DEBUG   :....mailslot_create: creating mailslot for timer"),
               new string ("03/22 08:51:01 ERROR   :..settcpimage: Get TCP images rc - EDC8112I Operation not supported on socket.")
            };
            return input;
        }

        private List<string> GetSampleOutputLines()
        {
            List<string> output = new List<string>
            {
                "|5|INFO|22 Mar 2020|08:51 AM|:...read_physical_netif: index #4, interface CTCD0 has address 9.67.116.98, ifidx 4|",
                "|6|ERROR|22 Mar 2020|08:51 AM|:..settcpimage: Get TCP images rc - EDC8112I Operation not supported on socket.|"
            };
            return output;
        }
        
        [Fact]
        public void GetLogWithLogId_ShouldWork()
        {
            var input = new Mock<IUserInput>();
            var reader = new Mock<IFileReader>();
            reader.Setup(f => f.GetLastLogIdFromOldCSVFile()).Returns(4);
            var writer = new Mock<IFileWriter>();
            var logParser = new Mock<LogParser>(input.Object,reader.Object,writer.Object);
            logParser.SetupAllProperties();
            string line = GetSampleInputLines()[0];
            Log actual=logParser.Object.GetLogWithLogId(line);
            Assert.True(actual.Level.Equals("INFO"));
            Assert.Equal(5, actual.LogId);
        }

    }
}
