using System;
using BulkRename.cmd;
using BulkRename.Class;

namespace BulkRename
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

            Run();
        }

        public static void Run()
        {
            Class1 classLib = new Class1();

            Console.WriteLine($"2 + 3 = { classLib.Add(2,3) }");
        }
    }
}
