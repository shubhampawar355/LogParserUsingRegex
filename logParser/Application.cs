using System;
using System.Collections.Generic;
using System.Text;

namespace logParser
{
    public class Application : IApplication
    {
        private readonly ILogParser _logParser;

        public Application(ILogParser logParser)
        {
            this._logParser = logParser;
        }
        public void Run()
        {
            _logParser.Parse();
        }

    }
}
