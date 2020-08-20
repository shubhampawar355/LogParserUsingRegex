namespace logParser.test {
    using System.Collections.Generic;
    using System.IO;
    using System;
    using logParser;
    using Xunit;

    public class FileReadWriteTest {
        [Fact]
        public void TestAddHeaderToDestination () {
            new FileReadWrite (@"..\..\..\SampleInputOutput\Input\logs", @"..\..\..\SampleInputOutput\Output\checkHeader.csv").AddHeaderToDestination ('|');
            Assert.True (File.ReadAllLines (@"..\..\..\SampleInputOutput\Output\checkHeader.csv") [0].Equals ("| No | Level | Date | Time | Text |"));
            File.Delete (@"..\..\..\SampleInputOutput\Output\checkHeader.csv");
        }

        [Fact]
        public void TestAddHeaderToDestinationWithChangeInDelimeter () {
            new FileReadWrite (@"..\..\..\SampleInputOutput\Input\logs", @"..\..\..\SampleInputOutput\Output\checkHeader.csv").AddHeaderToDestination ('#');
            Assert.True (File.ReadAllLines (@"..\..\..\SampleInputOutput\Output\checkHeader.csv") [0].Equals ("# No # Level # Date # Time # Text #"));
            File.Delete (@"..\..\..\SampleInputOutput\Output\checkHeader.csv");
        }

        [Theory, InlineData (new object[] { @"..\..\..\SampleInputOutput\Output\" }), InlineData (new object[] { @"..\..\..\SampleInputOutput\Output\log" }), InlineData (new object[] { @"..\..\..\SampleInputOutput\Output\log.csv" })]
        public void TestDestinationPathProcessedCorrectly (string dest) {
            Assert.True (@"..\..\..\SampleInputOutput\Output\log.csv".Equals (new FileReadWrite (@"..\..\..\SampleInputOutput\Input\logs", dest).Destination));
        }

        [Fact]
        public void TestFileReadWriteHasDefaultCtor () {
            FileReadWrite write = new FileReadWrite ();
            write.Source = @"sample\logs";
            Assert.True (@"sample\logs".Equals (write.Source));
        }

        [Fact]
        public void TestGetAllLogFiles () {
            FileReadWrite write = new FileReadWrite {
                Source = Directory.GetCurrentDirectory () + @"\..\..\..\SampleInputOutput\Input\logs"
            };
            Assert.Equal(3, write.GetAllLogFiles ().Length);
        }

        [Fact]
        public void TestGetLastLogIdFromOldCSVFile () {
            Assert.Equal (15, new FileReadWrite (@"..\..\..\SampleInputOutput\Input\logs", @"..\..\..\SampleInputOutput\Input\old.csv").GetLastLogIdFromOldCSVFile ());
        }

        [Fact]
        public void TestIsSourceValid () {
            Assert.True (new FileReadWrite { Source = @"..\..\..\SampleInputOutput\Input\logs" }.IsSourceValid ());
        }

        [Fact]
        public void TestReadAllLines () {
            Assert.Equal(16, new FileReadWrite ().ReadAllLines (@"..\..\..\SampleInputOutput\Input\old.csv").Count);
        }

        [Fact]
        public void TestWriteToFileAllLine () {
            List<string> lines = new List<string> ();
            lines.Add ("first");
            lines.Add ("second");
            lines.Add ("third");
            new FileReadWrite (@"..\..\..\SampleInputOutput\Input\logs", @"..\..\..\SampleInputOutput\Output\multiLineWrite.csv").WriteToFile (lines);
            Assert.Equal(3, File.ReadAllLines (@"..\..\..\SampleInputOutput\Output\multiLineWrite.csv").Length);
            File.Delete (@"..\..\..\SampleInputOutput\Output\multiLineWrite.csv");
        }

        [Fact]
        public void TestWriteToFileSingleLine () {
            string line = "This is for testing WriteToFile(string line)";
            new FileReadWrite (@"..\..\..\SampleInputOutput\Input\logs", @"..\..\..\SampleInputOutput\Output\singleLineWrite.csv").WriteToFile (line);
            Assert.True (line.Equals (File.ReadAllLines (@"..\..\..\SampleInputOutput\Output\singleLineWrite.csv") [0]));
            File.Delete (@"..\..\..\SampleInputOutput\Output\singleLineWrite.csv");
        }
    }
}