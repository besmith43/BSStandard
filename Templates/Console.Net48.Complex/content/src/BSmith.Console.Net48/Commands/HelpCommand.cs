using System;
using System.Threading.Tasks;

namespace BSmith.Console.Net48.Commands
{
    public class HelpCommand : ICommand
    {
		public const string COMMAND_NAME = "help";
		public bool all = false;
		public ICommand VersionCommand = new VersionCommand();

		public HelpCommand(string[] _args)
		{
			Parse_Args(_args);
		}

        Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				StringBuilder helpTextBuilder = new StringBuilder();

            	helpTextBuilder.AppendLine("Usage:  BSmith.Console [OPTION]  ");
            	helpTextBuilder.AppendLine("");
            	helpTextBuilder.AppendLine("    -V | --verbose          Set verbose mode");
            	helpTextBuilder.AppendLine("    -v | --version          Display version message");
            	helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            	helpTextBuilder.AppendLine("");

				VersionCommand.ExecuteAsync();
            	Console.WriteLine(helpTextBuilder.ToString());

				if (all)
				{

				}

				return true;
			});
		}

		private void ParseArgs(string[] args)
		{
			for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-a" || args[i] == "--all")
                {
					all = true;
				}
			}
		}
    }
}