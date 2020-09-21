using System;
using System.IO;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! "+ File.Exists(@".\log.txt"));
            Console.WriteLine("Hello World! "+ File.Exists(@".\log"));
            Console.WriteLine("Hello World! "+ Directory.Exists(@".\log.txt"));
            Console.WriteLine("Hello World! "+ Directory.Exists(@"..\logparser\"));
        }
    }
}
