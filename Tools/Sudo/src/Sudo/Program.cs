using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sudo.cmd;

namespace Sudo
{
    class Program
    {
        public static Options cmdFlags;
        static void Main(string[] args)
        {
            cmdParser cmdP = new cmdParser(args);

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
        }

        public static void Run()
        {
            ProcessStartInfo CommandInfo = new ProcessStartInfo(cmdFlags.command, cmdFlags.commandArgs);
            CommandInfo.Verb = "runas";
            Process proc = new Process();
            proc.StartInfo = CommandInfo;
            try
            {
                proc.Start();
                proc.WaitForExit();
            }
            catch
            {
                Console.WriteLine("You are not an administrator");
            }
        }
    }
}
