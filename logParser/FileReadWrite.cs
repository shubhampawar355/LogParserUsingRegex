namespace logParser {
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;

    public class FileReadWrite {
        public string Destination;
        public string Source;

        public FileReadWrite () { }

        public FileReadWrite (string source, string destination) {
            if (!Path.HasExtension (destination)) {
                destination = !Path.EndsInDirectorySeparator (destination) ? (destination + ".csv") : (destination + "log.csv");
            }
            this.Destination = destination;
            this.Source = source;
            Directory.CreateDirectory (Path.GetDirectoryName (this.Destination));
        }

        public void AddHeaderToDestination (char delime) {
            if(delime=='\0')
                delime='|';
            string str = "" + delime + " No " + delime + " Level " + delime + " Date " + delime + " Time " + delime + " Text " + delime + "\n";
            if (!File.Exists (Destination))
                File.Create (Destination).Close ();
            if (new FileInfo (Destination).Length == 0)
                File.AppendAllText (Destination, str);
        }

        public string[] GetAllLogFiles () =>
            Directory.GetFiles (this.Source, "*.log", SearchOption.AllDirectories);

        public int GetLastLogIdFromOldCSVFile () {
            int num = 0;
            if (File.Exists (this.Destination)) {
                try {
                    num = int.Parse (Enumerable.Last<string> (File.ReadLines (this.Destination)).Split ('|', (StringSplitOptions) StringSplitOptions.None) [1]);
                } catch (Exception) { }
            }
            return num;
        }

        public bool IsSourceValid () {
            return Directory.Exists (this.Source);
        }

        public List<string> ReadAllLines (string file) {
            return File.ReadAllLines (file).ToList ();
        }
        public void WriteToFile (List<string> lines) {
            if (lines.Count > 0) {
                File.AppendAllLines (this.Destination, (IEnumerable<string>) lines);
            }
        }

        public void WriteToFile (string line) {
            if (line != null) {
                line = line + "\n";
                File.AppendAllText (this.Destination, line);
            }
        }
    }
}