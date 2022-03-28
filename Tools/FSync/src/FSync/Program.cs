using System;
using System.IO;
using System.Collections.Generic;
using FSync.cmd;

namespace FSync
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

                List<SyncNode> missingNodes = sourceTree.CompareTo(compTree);

                if (missingNodes.Count > 0)
                {
                    Console.WriteLine("The following files or directories were not in the second folder");
                    foreach (var missingNode in missingNodes)
                    {
                        Console.WriteLine($"Original Node: { missingNode.Origin.ToString()}");
						Console.WriteLine($"Missing Node: { missingNode.Destination}");

						if (missingNode.Origin.Type == TreeNodeType.File && IsWindows())
						{
							File.Copy($"{ missingNode.Origin.NodePath }\\{ missingNode.Origin.Name }", missingNode.Destination, true);
						}
						else if (missingNode.Origin.Type == TreeNodeType.File && IsLinux())
						{
							File.Copy($"{ missingNode.Origin.NodePath }/{ missingNode.Origin.Name }", missingNode.Destination, true);
						}
						else
						{
							Directory.CreateDirectory(missingNode.Destination);
						}
                    }
                }
                else
                {
                    Console.WriteLine("Both Folder Structures are the same");
                }
            }           
        }

		public static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

		public static bool IsLinux() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

    }
}
