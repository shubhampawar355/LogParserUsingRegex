using System;
using System.Collections.Generic;
using System.Text;

namespace logParser
{
    public class Application : IApplication
    {
        private readonly ILogParser logParser;
        private readonly IUserInput userInput;

        public Application(IUserInput userInput, ILogParser logParser)
        {
            this.userInput = userInput;
            this.logParser = logParser;
        }
        public void Run()
        {
            logParser.Parse();
        }

    }
}
