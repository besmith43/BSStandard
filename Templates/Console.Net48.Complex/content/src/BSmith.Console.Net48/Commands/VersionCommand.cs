using System;
using System.Threading.Tasks;

namespace BSmith.Console.Net48.Commands
{
    public class VersionCommand : ICommand
    {
		public const string COMMAND_NAME = "version";
		public const string versionText = $"BSmith.Console { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";
        Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				Console.WriteLine(versionText);
				return true;
			});
		}
    }
}