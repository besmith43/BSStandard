using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleTables;

namespace MoodTracker.Commands
{
    public class CheckCommand : ICommand 
    {
		public const string COMMAND_NAME = "check";
		bool helpFlag = false;
		public string month = DateTime.Now.Month.ToString();
		public string year = DateTime.Now.Year.ToString();
		public string monthCSV;
		public string[] csvContents;
		public int totalDaysinMonth;
		public Dictionary<int, int> monthTotals = new();

		public CheckCommand(string[] _args)
		{
			ParseArgs(_args);

			totalDaysinMonth = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));

			monthCSV = Program.SetPath(new string[] { Program.GetLocalDir(), year, $"{ month }.csv" });

			csvContents = File.ReadAllLines(monthCSV);
		}

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				if (helpFlag)
				{
					helpScreen();
					return true;
				}

				if (!File.Exists(Program.SetPath(new string[] { Program.GetLocalDir(), year, $"{ month }.csv" })))
				{
					Console.WriteLine("The Month and Year selected doesn't have any entries");
					return false;
				}

				for (int i = 1; i <= totalDaysinMonth; i++)
				{
					monthTotals.Add(i, getTotals(i));
				}

				PrintMonth();

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
				else
				{

				}
			}
		}

		private void helpScreen()
        {
            StringBuilder helpScreenText = new();

			helpScreenText.AppendLine(Program.versionText);
			helpScreenText.AppendLine("Usage: MoodTracker check [options]  ");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    -h | --help			Display this help screen");
			helpScreenText.AppendLine("");

			Console.WriteLine(helpScreenText.ToString());
		}

		public int getTotals(int day)
		{
			int total = 0;
			int count = 0;

			for (int i = 1; i < csvContents.Length; i++)
			{
				string[] line = csvContents[i].Split(',');

				if (line[0] == $"{ day }/{ month }/{ year }")
				{
					total += int.Parse(line[1]);
					count++;
				}
			}

			return count is 0 ? 0 : total/count;
		}

		public void PrintMonth()
		{
			ConsoleTable table = new("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");

			DateTime firstDayValue = new(int.Parse(year), int.Parse(month), 1);
			int nextWeekIndex = 0;

			switch (firstDayValue.ToString("dddd"))
			{
				case "Sunday":
					table.AddRow(monthTotals[1],monthTotals[2],monthTotals[3],monthTotals[4],monthTotals[5],monthTotals[6],monthTotals[7]);
					nextWeekIndex = 8;
					break;
				case "Monday":
					table.AddRow(0,monthTotals[1],monthTotals[2],monthTotals[3],monthTotals[4],monthTotals[5],monthTotals[6]);
					nextWeekIndex = 7;
					break;
				case "Tuesday":
					table.AddRow(0,0,monthTotals[1],monthTotals[2],monthTotals[3],monthTotals[4],monthTotals[5]);
					nextWeekIndex = 6;
					break;
				case "Wednesday":
					table.AddRow(0,0,0,monthTotals[1],monthTotals[2],monthTotals[3],monthTotals[4]);
					nextWeekIndex = 5;
					break;
				case "Thursday":
					table.AddRow(0,0,0,0,monthTotals[1],monthTotals[2],monthTotals[3]);
					nextWeekIndex = 4;
					break;
				case "Friday":
					table.AddRow(0,0,0,0,0,monthTotals[1],monthTotals[2]);
					nextWeekIndex = 3;
					break;
				case "Saturday":
					table.AddRow(0,0,0,0,0,0,monthTotals[1]);
					nextWeekIndex = 2;
					break;
			}

			for (int i = 2; i <= Math.Ceiling((double)((totalDaysinMonth-nextWeekIndex+1)/7)); i++)
			{
				table.AddRow(monthTotals[nextWeekIndex],monthTotals[nextWeekIndex + 1],monthTotals[nextWeekIndex + 2],monthTotals[nextWeekIndex + 3],monthTotals[nextWeekIndex + 4],monthTotals[nextWeekIndex + 5],monthTotals[nextWeekIndex + 6]);
				nextWeekIndex += 7;
			}

			int lastWeekIndex = nextWeekIndex;
			DateTime lastDayValue = new(int.Parse(year), int.Parse(month), totalDaysinMonth);

			switch (lastDayValue.ToString("dddd"))
			{
				case "Sunday":
					table.AddRow(monthTotals[lastWeekIndex],0,0,0,0,0,0);
					break;
				case "Monday":
					table.AddRow(monthTotals[lastWeekIndex],monthTotals[lastWeekIndex+1],0,0,0,0,0);
					break;
				case "Tuesday":
					table.AddRow(monthTotals[lastWeekIndex],monthTotals[lastWeekIndex+1],monthTotals[lastWeekIndex+2],0,0,0,0);
					break;
				case "Wednesday":
					table.AddRow(monthTotals[lastWeekIndex],monthTotals[lastWeekIndex+1],monthTotals[lastWeekIndex+2],monthTotals[lastWeekIndex+3],0,0,0);
					break;
				case "Thursday":
					table.AddRow(monthTotals[lastWeekIndex],monthTotals[lastWeekIndex+1],monthTotals[lastWeekIndex+2],monthTotals[lastWeekIndex+3],monthTotals[lastWeekIndex+4],0,0);
					break;
				case "Friday":
					table.AddRow(monthTotals[lastWeekIndex],monthTotals[lastWeekIndex+1],monthTotals[lastWeekIndex+2],monthTotals[lastWeekIndex+3],monthTotals[lastWeekIndex+4],monthTotals[lastWeekIndex+5],0);
					break;
				case "Saturday":
					table.AddRow(monthTotals[lastWeekIndex],monthTotals[lastWeekIndex+1],monthTotals[lastWeekIndex+2],monthTotals[lastWeekIndex+3],monthTotals[lastWeekIndex+4],monthTotals[lastWeekIndex+5],monthTotals[lastWeekIndex+6]);
					break;
			}

			table.Write();
		}
    }
}
