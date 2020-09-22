using System;
using System.Collections.Generic;
using System.Text;

namespace logParser
{
    public interface IFileWriter
    {
        void AddHeaderToDestination(char delim);
        void WriteToFile(List<string> lines);
    }
}
