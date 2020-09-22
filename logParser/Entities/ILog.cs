using System;

namespace logParser
{
    public interface ILog
    {
        DateTime DateTime { get; set; }
        string Info { get; set; }
        string Level { get; set; }
        int? LogId { get; set; }

        string ToString();
    }
}