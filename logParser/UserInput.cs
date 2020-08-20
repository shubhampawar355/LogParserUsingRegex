namespace logParser {
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System;

    public class UserInput {
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
        
        private HashSet<string> _UserGivenLevels = new HashSet<string> ();
        public HashSet<string> UserGivenLevels
        {
            get { return _UserGivenLevels; }
            private set { _UserGivenLevels = value; }
        }
        private char _Delimeter='\0';
        public char Delemeter
        {
            get { return _Delimeter; }
           private set { _Delimeter = value; }
        }
        
        private static UserInput Singleton = null;

        private UserInput (string[] args) {
            Regex regex = new Regex ("(INFO|WARN|DEBUG|TRACE|ERROR|EVENT)", RegexOptions.IgnoreCase);

            for (int i = 0; i < args.Length; i++) {
                if (args[i] != null) {
                    if (regex.IsMatch (args[i])) {
                        this.UserGivenLevels.Add (args[i]);
                        args[i] = null;
                    } else if (args[i].Equals ("--log-level")) {
                        args[i] = null;
                    } else if (args[i].Equals ("--log-dir")) {
                        this.Source = args[i + 1];
                        args[i] = null;
                        args[i + 1] = null;
                    } else if (args[i].Equals ("--csv")) {
                        this.Destination = args[i + 1];
                        args[i] = null;
                        args[i + 1] = null;
                    }else if(args[i].Equals("--delime")){
                        this.Delemeter=args[i+1][0];
                        args[i] = null;
                        args[i + 1] = null;
                    }
                }
            }
            this.IfUserGivesNolabels(args);
            Singleton=this;
        }

        private void IfUserGivesNolabels(string[] args) {
            if (Source == null || Destination == null) {
                if (Source == null && Destination == null) {
                    Source = @args[0];
                    Destination = @args[args.Length - 1];
                } else if (Destination == null) {
                    foreach (var item in args) {
                        if (item != null)
                            Destination = item;
                    }
                } else if (Source == null) {
                    foreach (var item in args) {
                        if (item != null)
                            Source = item;
                    }
                }
            }
            this.AssignAllLevelsIfNotGiven ();
        }

        private void AssignAllLevelsIfNotGiven () {
            if (this.UserGivenLevels.Count == 0) {
                string[] levels = new string[] { "INFO", "WARN", "DEBUG", "TRACE", "ERROR", "EVENT" };
                foreach (string level in levels) {
                    this.UserGivenLevels.Add (level);
                }
            }
        }

        public static UserInput GetInstance (string[] args) {
            return ((Singleton != null) ? Singleton : new UserInput (args));
        }

        public static UserInput GetInstance () {
            return Singleton;
        }
    }
}