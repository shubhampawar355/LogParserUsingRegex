namespace logParser {
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System;

    public class LogParser {
        public static UserInput userInput;
        public static FileReadWrite FileHandler;
        public static int lastLogIdFromOldCSVFile;
        public static int SkippedLinesCount=0;
    
        private static void Main (string[] args) {
            if (args.Length >= 2) {
                userInput = UserInput.GetInstance (args);
                FileHandler = new FileReadWrite (userInput.Source, userInput.Destination);
                FileHandler.AddHeaderToDestination ('|');
                if (FileHandler.IsSourceValid ()) {
                    string[] allLogFiles = FileHandler.GetAllLogFiles ();
                    if (allLogFiles.Length != 0) {
                        lastLogIdFromOldCSVFile = FileHandler.GetLastLogIdFromOldCSVFile ();
                        foreach (string file in allLogFiles) {
                            AddLogsIfFileContainsLogs(file);
                        }
                    } else {
                        Console.WriteLine ("Source directory does not contain any *.log file ");
                        return;
                    }
                } else {
                    Console.WriteLine ("Source directory = " + userInput.Source + " is Invalid!! Please enter valid source directory.");
                }
                Console.WriteLine ("Log Parsing done Successfully.Skipped Lines: " + SkippedLinesCount);
            } else {
                PrintHelp ();
            }
        }

        
        private static void AddLogsIfFileContainsLogs (string file) {
            List<string> lines = FileHandler.ReadAllLines (file);
            List<string> logsToWrite = new List<string> ();
            Log.SetLogFormat (userInput.UserGivenLevels);
            Log.Delimeter = '|';
            string tabVal = new string (' ', 1);
            foreach (string line in lines) {
                string formattedLine = line.Replace ("\t", tabVal);
                if (!Log.IsLog (formattedLine)) {
                    SkippedLinesCount++;
                    continue;
                }
                string[] lineSplit = formattedLine.Split (' ', StringSplitOptions.RemoveEmptyEntries);
                logsToWrite.Add (new Log (++lastLogIdFromOldCSVFile,
                    Regex.Match (formattedLine, "(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])").ToString (),
                    Regex.Match (formattedLine, "(0[1-9]|1[012])[:](0[1-9]|[12345][0-9])[:](0[1-9]|[12345][0-9])").ToString (),
                    Regex.Match (formattedLine, "(INFO|WARN|DEBUG|TRACE|ERROR|EVENT)").ToString (),
                    string.Join (" ", Enumerable.Skip<string> (lineSplit, 3))).ToString ());
            }
            FileHandler.WriteToFile (logsToWrite);
        }


        private static void PrintHelp () {
            Console.WriteLine ("\nUse case 1) logParser --log-dir <dir> --log-level <level> --csv <out>" +
                " \n--log-dir   ==> <Directory to parse recursively for .log files >" +
                "\n    --csv   ==> <Out file-path (absolute/relative)>" +
                "\n--log-level ==> <INFO|WARN|DEBUG|TRACE|ERROR|EVENT> default will be all levels or you can give more than one by giving space between  \n" +
                "\nUse case 2) logParser <Source dir> <level> <Destination dir>\n" +
                "\nUse case 3) logParser <Source dir> <Destination dir>\n  In this case all levels will be considered\n");
        }
    }
}