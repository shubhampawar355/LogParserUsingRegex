namespace logParser {
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System;

    public class UserInput : IUserInput
    {
        private string _Source;
        public string Source
        {
            get { return _Source; }
            private set { _Source = value; }
        }

        private string _Destination;
        public string Destination
        {
            get { return _Destination; }
            private set { _Destination = value; }
        }

        private HashSet<string> _UserGivenLevels = new HashSet<string>();
        public HashSet<string> UserGivenLevels
        {
            get { return _UserGivenLevels; }
            private set { _UserGivenLevels = value; }
        }
        private char _Delimeter = '\0';
        public char Delemeter
        {
            get { return _Delimeter; }
            private set { _Delimeter = value; }
        }

        private Regex levelRegex = new Regex(@"^(INFO|WARN|DEBUG|TRACE|ERROR|EVENT)$", RegexOptions.IgnoreCase);
        private Regex flagRegex = new Regex(@"^(--log-dir|--csv|--delime)$", RegexOptions.IgnoreCase);

        public UserInput(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] != null)
                {
                    try
                    {
                        if (levelRegex.IsMatch(args[i]))
                        {
                            this.UserGivenLevels.Add(args[i]);
                            args[i] = null;
                        }
                        else if (flagRegex.IsMatch(args[i]))
                        {
                            if (args[i].Equals("--log-dir", StringComparison.OrdinalIgnoreCase))
                                this.Source = args[i + 1];
                            else if (args[i].Equals("--csv", StringComparison.OrdinalIgnoreCase))
                                this.Destination = args[i + 1];
                            else if (args[i].Equals("--delime", StringComparison.OrdinalIgnoreCase))
                                this.Delemeter = args[i + 1][0];
                            args[i] = args[i + 1] = null;
                        }
                        else if (args[i].Equals("--log-level", StringComparison.OrdinalIgnoreCase))
                            args[i] = null;
                        else if (IsSource(args[i]) && Source == null)
                        {
                            Source = args[i];
                            args[i] = null;
                        }
                        else if (IsDestination(args[i]) && Destination == null)
                        {
                            Destination = args[i];
                            args[i] = null;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Input! after " + args[i] + " flag there must be a value");
                        LogParser.PrintHelp();
                        Environment.Exit(1);
                    }
                }
            }
            this.IfUserGivesNolabels(args);
            this.AssignAllLevelsIfNotGiven();
        }

        private void IfUserGivesNolabels(string[] args)
        {
            foreach (var item in args)
            {
                if (item != null)
                {
                    Console.WriteLine("Invalid Input! does not understand " + item + " flag/value");
                    LogParser.PrintHelp();
                    Environment.Exit(1);
                }
            }
        }

        private void AssignAllLevelsIfNotGiven()
        {
            if (this.UserGivenLevels.Count == 0)
            {
                string[] levels = new string[] { "INFO", "WARN", "DEBUG", "TRACE", "ERROR", "EVENT" };
                foreach (string level in levels)
                {
                    this.UserGivenLevels.Add(level);
                }
            }
        }
        private bool IsSource(string @source)
        {
            if (Path.HasExtension(source) && (Path.GetExtension(source)).Equals(".log") && File.Exists(source))
                return true;
            if (Directory.Exists(source))
                return true;
            return false;
        }
        private bool IsDestination(string @dest)
        {
            if (!Path.HasExtension(dest))
            {
                if (Directory.Exists(dest))
                    dest = Path.EndsInDirectorySeparator(dest) ? (dest + "log.csv") : (dest + "/log.csv");
                else
                    dest = !Path.EndsInDirectorySeparator(dest) ? (dest + ".csv") : (dest + "log.csv");
            }
            if (Path.HasExtension(dest) && (Path.GetExtension(dest)).Equals(".csv"))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dest));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}