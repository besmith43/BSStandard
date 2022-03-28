using System;
using System.Threading.Tasks;
using MoneyTracker.Data;

namespace MoneyTracker.Commands
{
    public class MergeCommand : ICommand
    {
		public const string COMMAND_NAME = "merge";

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				return true;
			});
		}
    }
}