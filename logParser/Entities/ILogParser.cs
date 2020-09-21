using System;
using System.Collections.Generic;
using System.Text;

namespace logParser
{
    public interface ILogParser
    {
        void Parse();
        void AddLogsIfFileContainsLogs(string file);
    }
}
