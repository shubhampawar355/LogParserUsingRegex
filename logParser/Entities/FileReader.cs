namespace logParser {
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;

    public class FileReader : IFileReader
    {
        private string _source;
        private string _destination;

        public FileReader(IUserInput userInput) {
            _destination = userInput.Destination;
            _source = userInput.Source;
        }

        public string[] GetAllLogFiles (){
            if(Path.HasExtension(this._source))
                return new string[]{_source};
           return Directory.GetFiles (this._source, "*.log", SearchOption.AllDirectories);
        }

        public int GetLastLogIdFromOldCSVFile () {
            int num = 0;
            if (System.IO.File.Exists (_destination)) {
                try {
                    string line = Enumerable.Last<string>(System.IO.File.ReadLines(_destination));
                    num = int.Parse(line.Split(line[0], (StringSplitOptions)StringSplitOptions.None)[1]);        
                } catch (Exception) { }
            }
            return num;
        }
        public List<string> ReadAllLines (string @file) {
            return File.ReadAllLines (file).ToList ();
        }
    }
}