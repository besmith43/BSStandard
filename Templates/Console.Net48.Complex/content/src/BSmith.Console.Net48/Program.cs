using System;
using System.Linq;
using BSmith.Console.Net48.Commands;

namespace BSmith.Console.Net48
{
    class Program
    {

        static void Main(string[] args)
        {
            ICommand command = null;

            switch (args[0].ToLower())
            {
                case VersionCommand.COMMAND_NAME:
                    command = new VersionCommand(args.Skip(1).ToArray());
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
    }
}
