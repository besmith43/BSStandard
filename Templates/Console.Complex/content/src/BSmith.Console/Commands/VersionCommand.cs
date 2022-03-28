using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BSmith.Console.Commands
{
    public class VersionCommand : ICommand
    {
		public const string COMMAND_NAME = "version";
		public string versionText = $"BSmith.Console { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				Console.WriteLine(versionText);
				return true;
			});
		}
    }
}