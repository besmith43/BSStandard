using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using MoneyTracker.Data;

namespace MoneyTracker.Commands
{
    public class BackupCommand : ICommand
    {
		public const string COMMAND_NAME = "backup";
		public bool helpFlag = false;
		public string backupPath = Program.SetPath(new string[] { Environment.CurrentDirectory, "MoneyTrackerBackup.zip" });

		public BackupCommand(string[] _args)
		{
			ParseArgs(_args);
		}

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				if (helpFlag)
				{
					helpScreen();
                    return true;
				}

				if (!Directory.Exists(Program.GetLocalDir()))
				{
					Console.WriteLine("There isn't a directory to backup");
					return false;
				}

				ZipFile.CreateFromDirectory(Program.GetLocalDir(), backupPath);

				return true;
			});
		}

		private void ParseArgs(string[] args)
		{
			for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-b" || args[i] == "--backup")
                {
					backupPath = Program.SetPath(new string[] { args[i+1], "MoneyTrackerBackup.zip" });
					i++;
				}
				else if (args[i] == "-h" || args[i] == "--help")
				{
					helpFlag = true;
				}
			}
		}

		private void helpScreen()
        {
            StringBuilder helpScreenText = new();

			helpScreenText.AppendLine(Program.versionText);
			helpScreenText.AppendLine("Usage: MoneyTracker backup [options]  ");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    -b | --backup			Destination for the backup zip file");
			helpScreenText.AppendLine("    -h | --help				Print this print out");
			helpScreenText.AppendLine("");

			Console.WriteLine(helpScreenText.ToString());
		}
    }
}