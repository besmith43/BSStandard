using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Commands
{
    public class NewCommand : ICommand 
    {
		public const string COMMAND_NAME = "add";
		bool helpFlag = false;
		public string day = DateTime.Now.Day.ToString();
		public string month = DateTime.Now.Month.ToString();
		public string year = DateTime.Now.Year.ToString();
		public string csvFileName;
		public StringBuilder csvContent = new();
		public string mood;

		public NewCommand(string[] _args)
		{
			ParseArgs(_args);

			if (!Directory.Exists(Program.GetLocalDir()))
			{
				Directory.CreateDirectory(Program.GetLocalDir());
			}

			if (!Directory.Exists(Program.SetPath(new string[] { Program.GetLocalDir(), year })))
			{
				Directory.CreateDirectory(Program.SetPath(new string[] { Program.GetLocalDir(), year }));
			}

			csvFileName = Program.SetPath(new string[] { Program.GetLocalDir(), year, $"{ month }.csv" });

			if (File.Exists(csvFileName))
			{
				string[] csvTemp = File.ReadAllLines(csvFileName);
				foreach (string line in csvTemp)
				{
					csvContent.AppendLine(line);
				}
			}
			else
			{
				csvContent.AppendLine(GetCsvHeaders());
			}
		}

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				if (helpFlag)
				{
					helpScreen();
					return true;
				}

				if (string.IsNullOrWhiteSpace(mood))
				{
					Console.WriteLine("Please pick a number between 1 and 10 based on your mood, where 1 is bad and 10 is great");
					mood = Console.ReadLine();
				}

				int moodTest;
				var result = int.TryParse(mood, out moodTest);

				if (!result)
				{
					Console.WriteLine("The input given for mood wasn't a valid whole number");
					return false;
				}

				WriteMood(moodTest);

				return true;
			});
		}

		public void ParseArgs(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "-h" || args[i] == "--help")
				{
					helpFlag = true;
				}
				else if (args[i] == "-d" || args[i] == "--day")
                {
					day = args[i+1];
					i++;
				}
				else if (args[i] == "-m" || args[i] == "--month")
				{
					month = args[i+1];
					i++;
				}
				else if (args[i] == "-y" || args[i] == "--year")
				{
					year = args[i+1];
					i++;
				}
				else if (args[i] == "-i" || args[i] == "--mood")
				{
					mood = args[i+1];
					i++;
				}
				else
				{

				}
			}
		}

		private void helpScreen()
        {
            StringBuilder helpScreenText = new();

			helpScreenText.AppendLine(Program.versionText);
			helpScreenText.AppendLine("Usage: MoodTracker add [options]  ");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    -d | --day			Set the day of purchase");
			helpScreenText.AppendLine("    -m | --month		Set the month of purchase");
			helpScreenText.AppendLine("    -y | --year			Set the year of purchase");
			helpScreenText.AppendLine("    -i | --mood      Set the mood that you're feeling at this moment");
			helpScreenText.AppendLine("");

			Console.WriteLine(helpScreenText.ToString());
		}

		private string GetCsvHeaders()
		{
			return "Date,Mood";
		}

		public void WriteMood(int moodIndex)
		{
			csvContent.AppendLine($"{ day }/{ month }/{ year },{ moodIndex }");

			File.WriteAllText(csvFileName, csvContent.ToString());
		}
    }
}
