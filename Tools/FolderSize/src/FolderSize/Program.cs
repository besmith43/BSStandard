using System;
using System.IO;
using System.Collections.Generic;
using FolderSize.cmd;

namespace FolderSize
{
    class Program
    {
        public static Options cmdFlags;
        public static List<string> errorPaths = new();
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

            Run();
        }

        public static void Run()
        {
            if (!String.IsNullOrEmpty(cmdFlags.file))
            {
                checkFile();
            }
            else if (!String.IsNullOrEmpty(cmdFlags.folder))
            {
                checkFolder();
            }
            else
            {
                cmdFlags.folder = Environment.CurrentDirectory;
                checkFolder();
            }

            if (errorPaths.Count > 0)
            {
                Console.Error.WriteLine("The following files and folders had an error: ");
                foreach(string epath in errorPaths)
                {
                    Console.Error.WriteLine(epath);
                }
            }
        }

        public static void checkFile()
        {
            if (File.Exists(cmdFlags.file))
            {
                try
                {
                    PrintTotal(getFileInfo(cmdFlags.file));
                }
                catch
                {
                    errorPaths.Add(cmdFlags.file);
                }
            }
            else
            {
                Environment.Exit(1);
            }
        }

        public static void checkFolder()
        {
            PrintTotal(checkFolderRecursive(cmdFlags.folder));
        }

        private static long checkFolderRecursive(string tempFolder)
        {
            string[] files = Directory.GetFiles(tempFolder);
            string[] subfolders = Directory.GetDirectories(tempFolder);

            long total = 0;

            foreach (var file in files)
            {
                try
                {
                    total += getFileInfo(file);
                }
                catch
                {
                    errorPaths.Add(file);
                }
            }

            foreach (var subfolder in subfolders)
            {
                try
                {
                    total += checkFolderRecursive(subfolder);
                }
                catch
                {
                    errorPaths.Add(subfolder);
                }
            }

            return total;
        }

        public static long getFileInfo(string temp)
        {
            var fileInfo = new FileInfo(temp);
            return fileInfo.Length;
        }

        public static void PrintTotal(long bytes)
        {
            Console.WriteLine(DetermineSize.ToPrettySize(bytes, 2));
        }
    }
}
 