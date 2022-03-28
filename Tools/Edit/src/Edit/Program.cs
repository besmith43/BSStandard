using System;
using System.IO;
using Edit.cmd;
using Edit.Class;
using Edit.Tui;

namespace Edit
{
    class Program
    {
        public static Editor editorInterface;
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

            Run();
        }

        public static void Run()
        {
            if (String.IsNullOrEmpty(cmdFlags.fileArg))
            {
                editorInterface = new Editor();
            }
            else if (File.Exists(cmdFlags.fileArg))
            {
                editorInterface = new Editor(cmdFlags.fileArg);
            }
            else
            {
                try
                {
                    FileInfo fileArgInfo = new FileInfo(cmdFlags.fileArg);
                    Directory.CreateDirectory(fileArgInfo.DirectoryName);
                    StreamWriter sw = File.CreateText(cmdFlags.fileArg);
                    sw.Close();
                    editorInterface = new Editor(cmdFlags.fileArg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
           }
        }
    }
}
