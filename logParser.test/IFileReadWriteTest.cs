namespace logParser.test
{
    public interface IFileReadWriteTest
    {
        void TestAddHeaderToDestination();
        void TestAddHeaderToDestinationWithChangeInDelimeter();
        void TestDestinationPathProcessedCorrectly(string dest);
        void TestFileReadWriteHasDefaultCtor();
        void TestGetAllLogFiles();
        void TestGetLastLogIdFromOldCSVFile();
        void TestIsSourceValid();
        void TestReadAllLines();
        void TestWriteToFileAllLine();
        void TestWriteToFileSingleLine();
    }
}