using System;
using System.Threading.Tasks;

namespace MoneyTracker.Commands
{
    public class VersionCommand : ICommand
    {
		public const string COMMAND_NAME = "version";

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				Console.WriteLine(Program.versionText);
				return true;
			});
		}
    }
}