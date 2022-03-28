using System;
using System.Collections.Generic;
using DiffFolder.cmd;

namespace DiffFolder
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
            if (String.IsNullOrEmpty(cmdFlags.source) || String.IsNullOrEmpty(cmdFlags.compFolder))
            {
                Console.Error.WriteLine("Error: the source or compared folder wasn't passed in.");
                return;
            }
            else
            {
                Tree sourceTree = new (cmdFlags.source);
                Tree compTree = new (cmdFlags.compFolder);

                List<TreeNode> missingNodes = sourceTree.CompareTo(compTree);

                if (missingNodes.Count > 0)
                {
                    Console.WriteLine("The following files or directories were not in the second folder");
                    foreach (var missingNode in missingNodes)
                    {
                        Console.WriteLine(missingNode.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Both Folder Structures are the same");
                }
            }
        }
    }
}
