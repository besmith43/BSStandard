﻿using System;
using System.Linq;
using System.Reflection;
using MoodTracker.Commands;

namespace MoodTracker
{
    class Program
    {
		public static string WindowsLocalDir = $"C:\\Users\\{ Environment.UserName }\\AppData\\Local\\MoodTracker";
        public static string LinuxLocalDir = $"/home/{ Environment.UserName }/.local/moodtracker";
        public static string MacOSLocalDir = $"/Users/{ Environment.UserName }/.local/moodtracker";
        public static string versionText = $"MoodTracker { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";



        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "help" };
            }

            ICommand command = null;

            switch (args[0].ToLower())
            {
				case NewCommand.COMMAND_NAME:
					command = new NewCommand(args.Skip(1).ToArray());
					break;
				case CheckCommand.COMMAND_NAME:
					command = new CheckCommand(args.Skip(1).ToArray());
					break;
                case VersionCommand.COMMAND_NAME:
                    command = new VersionCommand();
                    break;
                case "--help":
                case "-h":
                case HelpCommand.COMMAND_NAME:
                    command = new HelpCommand(args.Skip(1).ToArray());
                    break;
                default:
                    Console.Error.WriteLine($"Unknown command {args[0]}");
                    Environment.Exit(-1);
                    break;
            }

            Run(command);
        }

        public static void Run(ICommand cmd)
        {
            if (cmd != null)
            {
                var success = cmd.ExecuteAsync().Result;

                if (!success)
                {
                    Environment.Exit(-1);
                }
            }
        }

		public static string GetLocalDir()
		{
			if (IsWindows())
			{
				return WindowsLocalDir;
			}
			else if (IsLinux())
			{
				return LinuxLocalDir;
			}
			else
			{
				return MacOSLocalDir;
			}
		}

        public static string SetPath(string[] path)
        {
            if (IsWindows())
            {
                return string.Join("\\", path);
            }
            else
            {
                return string.Join("/", path);
            }
        }

		public static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

		public static bool IsLinux() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
    }
}
