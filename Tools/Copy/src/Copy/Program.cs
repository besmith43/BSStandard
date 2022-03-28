using System;
using System.IO;
using Copy.cmd;
using Copy.Class;
using Copy.TUI;

namespace Copy
{
    class Program
    {
        public static Options cmdFlags;

        static void Main(string[] args)
        {
            cmdParser cmdP = new(args);

            cmdFlags = cmdP.Parse();

            if (cmdFlags.versionFlag)
            {
                Console.WriteLine(cmdFlags.versionText);
                return;
            }

            if (cmdFlags.helpFlag)
            {
                Console.WriteLine(cmdFlags.helpText);
                return;
            }

            if (String.IsNullOrEmpty(cmdFlags.source) && String.IsNullOrEmpty(cmdFlags.destination))
            {
                App.Start();
                return;
            }

            Run();
        }

        private static void Run()
        {
            if (!File.Exists(cmdFlags.source) && !Directory.Exists(cmdFlags.source))
            {
                cmdFlags.source = Path.GetFullPath(cmdFlags.source);

                if (!File.Exists(cmdFlags.source) && !Directory.Exists(cmdFlags.source))
                {
                    Console.WriteLine("Source is not a file or Directory that exists");
                    return;
                }
            }

            // check if source is file or folder
            if (File.Exists(cmdFlags.source))
            {
                Class1.CopyFile(cmdFlags.source, cmdFlags.destination);
            }
            else if (Directory.Exists(cmdFlags.source))
            {
                Class1.CopyDirectory(cmdFlags.source, cmdFlags.destination);
            }
            else
            {
                Console.WriteLine("Source isn't given");
            }
        }
    }
}
