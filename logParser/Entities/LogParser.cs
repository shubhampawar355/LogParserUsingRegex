using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace logParser
{
    public class LogParser : ILogParser
    {
        private IUserInput _userInput;
        private IFileReader _fileReader;
        private IFileWriter _fileWriter;
        private int _lastLogIdFromOldCSVFile;
        private int _skippedLinesCount = 0;

        public LogParser(IUserInput userInput,IFileReader fileReader,IFileWriter fileWriter)
        {
            this._userInput = userInput;
            this._fileReader = fileReader;
            this._fileWriter = fileWriter;
            this._lastLogIdFromOldCSVFile = fileReader.GetLastLogIdFromOldCSVFile();
        }
        public void Parse()
        {
            _fileWriter.AddHeaderToDestination(_userInput.Delemeter);
            Log.SetLevelsToConsider(_userInput.UserGivenLevels);
            Log.Delimeter = _userInput.Delemeter;
            string[] allLogFiles = _fileReader.GetAllLogFiles();
            if (allLogFiles.Length != 0)
            {
                foreach (string file in allLogFiles)
                {
                    AddLogsIfFileContainsLogs(file);
                }
            }
            else
            {
                Console.WriteLine("Source directory " + _userInput.Source + " does not contain any *.log file ");
                Environment.Exit(1);
            }
            Console.WriteLine("Log Parsing done Successfully.Skipped Lines: " + _skippedLinesCount);
        }

        public void AddLogsIfFileContainsLogs(string file)
        {
            List<string> lines = _fileReader.ReadAllLines(file);
            List<string> logsToWrite = new List<string>();
            string tabVal = new string(' ', 1);
            foreach (string line in lines)
            {
                string formattedLine = line.Replace("\t", tabVal);
                if (!Log.IsLog(formattedLine))
                {
                    _skippedLinesCount++;
                    continue;
                }
                string[] lineSplit = formattedLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                logsToWrite.Add(new Log(++_lastLogIdFromOldCSVFile,
                    Regex.Match(formattedLine, "(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])").ToString(),
                    Regex.Match(formattedLine, "(0[1-9]|1[012])[:](0[1-9]|[12345][0-9])[:](0[1-9]|[12345][0-9])").ToString(),
                    Regex.Match(formattedLine, "(INFO|WARN|DEBUG|TRACE|ERROR|EVENT)").ToString(),
                    string.Join(" ", Enumerable.Skip<string>(lineSplit, 3))).ToString());
            }
            _fileWriter.WriteToFile(logsToWrite);
        }


        public static void PrintHelp()
        {
            Console.WriteLine("\nUse case 1) logParser --log-dir <dir> --log-level <level> --csv <out>" +
                "\n  --log-dir ==> <Directory to parse recursively for .log files >" +
                "\n      --csv ==> <Out file-path (absolute/relative)>" +
                "\n--log-level ==> <INFO|WARN|DEBUG|TRACE|ERROR|EVENT> default will be all levels or you can give more than one by giving space between  \n" +
                "\nUse case 2) logParser <Source dir> <level> <Destination dir>\n" +
                "\nUse case 3) logParser <Source dir> <Destination dir>\n  In this case all levels will be considered\n" +
                "\n Use --delime label to set Delemeter");
        }

    }
}
