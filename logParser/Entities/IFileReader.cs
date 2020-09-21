using System;
using System.Collections.Generic;
using System.Text;

namespace logParser
{
    public interface IFileReader
    {
        string[] GetAllLogFiles();
        int GetLastLogIdFromOldCSVFile();
        List<string> ReadAllLines(string @file);
    }
}
