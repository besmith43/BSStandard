using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MoneyTracker.Data;

namespace MoneyTracker.Commands
{
    public class InitCommand : ICommand
    {
		// create a new yearly budget
		public const string COMMAND_NAME = "init";
		public bool helpFlag = false;
		public string year = DateTime.Now.Year.ToString();
		public string csvFileName;
		public StringBuilder csvContent = new();

		public InitCommand(string[] _args)
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

			csvFileName = Program.SetPath(new string[] { Program.GetLocalDir(), year, "budget.csv" });
			csvContent.AppendLine(GetCsvHeaders());
		}

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				if (helpFlag)
                {
                    helpScreen();
                    return true;
                }

				if (File.Exists(csvFileName))
				{
					Console.WriteLine("The budget.csv alreaedy exists");
					Console.Write("Would you like to overwrite it? (Y/n)  ");
					string answer = Console.ReadLine();

					if (answer != "n" || answer != "N")
					{
						File.Delete(csvFileName);
					}
					else
					{
						return true;
					}
				}

				SetBudget();

				saveCSV();

				return true;
			});
		}

		private void ParseArgs(string[] args)
		{
			for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-y" || args[i] == "--year")
                {
					year = args[i+1];
					i++;
				}
				else if (args[i] == "-h" || args[i] == "--help")
				{
					helpFlag = true;
				}
			}
		}

		private string GetCsvHeaders()
		{
			return "item,limit";
		}

		private void SetBudget()
		{
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.groceries));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.toys));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.food));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.rent));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.electric));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.gas));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.loan));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.life));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.car));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.internet));
			csvContent.AppendLine(SetBudgetLine(CategoryEnum.cellphone));
		}

		private string SetBudgetLine(string category)
		{
			bool result = false;
			string response;
			do
			{
				Console.Write($"What is the dollar limit for { category }?  ");
				response = Console.ReadLine();

				double testResponse;
				result = double.TryParse(response, out testResponse);
			} while (!result);

			return $"{ category },{ response }";
		}

		private void saveCSV()
		{
			File.WriteAllText(csvFileName, csvContent.ToString());
		}

		private void helpScreen()
        {
            StringBuilder helpScreenText = new();

			helpScreenText.AppendLine(Program.versionText);
			helpScreenText.AppendLine("Usage: MoneyTracker init [options]  ");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    -y | --year			Set the year of the new budget");
			helpScreenText.AppendLine("    -h | --help			Display this help screen");
			helpScreenText.AppendLine("");

			Console.WriteLine(helpScreenText.ToString());
		}
    }
}