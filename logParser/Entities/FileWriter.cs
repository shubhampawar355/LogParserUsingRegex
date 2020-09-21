using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace logParser 
{
    public class FileWriter : IFileWriter
    {
        private string _destination;
        public FileWriter (IUserInput userInput) {
            this._destination = userInput.Destination; ;
            Directory.CreateDirectory (Path.GetDirectoryName (this._destination));
        }

        public void AddHeaderToDestination (char delime) {
            if(delime=='\0')
                delime='|';
            string str = "" + delime + " No " + delime + " Level " + delime + " Date " + delime + " Time " + delime + " Text " + delime + "\n";
            if (!File.Exists (_destination))
                File.Create (_destination).Close ();
            if (new FileInfo (_destination).Length == 0)
                File.AppendAllText (_destination, str);
        }
        
        public void WriteToFile (List<string> lines) {
            if (lines.Count > 0) {
                File.AppendAllLines (this._destination, (IEnumerable<string>) lines);
            }
        }

        public void WriteToFile (string line) {
            if (line != null) {
                line = line + "\n";
                File.AppendAllText (this._destination, line);
            }
        }
    }
}