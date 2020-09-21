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
        public void AddHeaderToDestination()
        {
            var mockInfo = new Mock<IUserInput>();
            mockInfo.SetupGet(i => i.Destination).Returns(@"..\..\..\SampleInputOutput\Output\checkHeader.csv");

            Action act = () =>
              {
                  var fileWriter = new FileWriter(mockInfo.Object);
                  fileWriter.AddHeaderToDestination('|');
                  Assert.True(File.ReadAllLines(@"..\..\..\SampleInputOutput\Output\checkHeader.csv")[0].Equals("| No | Level | Date | Time | Text |"));
              };
        }

        [Fact]
        public void TestAddHeaderToDestinationWithChangeInDelimeter()
        {
            var mockInfo = new Mock<IUserInput>();
            mockInfo.SetupGet(i => i.Destination).Returns(@"..\..\..\SampleInputOutput\Output\checkHeader.csv");

            Action act = () =>
            {
                var fileWriter = new FileWriter(mockInfo.Object);
                fileWriter.AddHeaderToDestination('#');
                Assert.True(File.ReadAllLines(@"..\..\..\SampleInputOutput\Output\checkHeader.csv")[0].Equals("| No | Level | Date | Time | Text |"));
                File.Delete(@"..\..\..\SampleInputOutput\Output\checkHeader.csv");
            };
        }
    }
}
