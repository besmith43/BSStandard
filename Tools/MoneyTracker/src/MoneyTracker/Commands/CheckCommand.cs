using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MoneyTracker.Data;
using ConsoleTables;

namespace MoneyTracker.Commands
{
    public class CheckCommand : ICommand
    {
		public const string COMMAND_NAME = "check";
		public bool helpFlag = false;
		public bool listFlag = false;
		public string month = DateTime.Now.Month.ToString();
		public string year = DateTime.Now.Year.ToString();
		public string monthCSV;
		public string budgetCSV;
		public string category;
		public double groceriesTotal = 0.0;
		public double toysTotal = 0.0;
		public double foodTotal = 0.0;
		public double rentTotal = 0.0;
		public double electricTotal = 0.0;
		public double gasTotal = 0.0;
		public double loanTotal = 0.0;
		public double lifeTotal = 0.0;
		public double carTotal = 0.0;
		public double internetTotal = 0.0;
		public double cellphoneTotal = 0.0;

		public CheckCommand(string[] _args)
		{
			ParseArgs(_args);

			monthCSV = Program.SetPath(new string[] { Program.GetLocalDir(), year, $"{ month }.csv" });

			budgetCSV = Program.SetPath(new string[] { Program.GetLocalDir(), year, "budget.csv" });
		}

        public Task<bool> ExecuteAsync()
		{
			return Task.Run(() => {
				if (helpFlag)
                {
                    helpScreen();
                    return true;
                }

				if (!File.Exists(monthCSV) || !File.Exists(budgetCSV))
				{
					Console.WriteLine("The budget or month csv file doesn't exists");
					return false;
				}

				GetCategoryTotals(File.ReadAllLines(monthCSV));

				if (!string.IsNullOrEmpty(category) && !listFlag)
				{
					OutputCategoryTotal(File.ReadAllLines(budgetCSV));
					return true;
				}

				CompareAgainstBudget(File.ReadAllLines(budgetCSV));

				if (listFlag)
				{
					ListMonthTransactions(File.ReadAllLines(monthCSV));
				}

				return true;
			});
		}

		private void ParseArgs(string[] args)
		{
			for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-m" || args[i] == "--month")
                {
					month = args[i+1];
					i++;
				}
				else if (args[i] == "-y" || args[i] == "--year")
				{
					year = args[i+1];
					i++;
				}
				else if (args[i] == "-l" || args[i] == "--list")
				{
					listFlag = true;
				}
				else if (args[i] == "-c" || args[i] == "--category")
				{
					switch (args[i+1])
					{
						case CategoryEnum.toys:
							category = CategoryEnum.toys;
							break;
						case CategoryEnum.food:
							category = CategoryEnum.food;
							break;
						case CategoryEnum.groceries:
							category = CategoryEnum.groceries;
							break;
						case CategoryEnum.rent:
							category = CategoryEnum.rent;
							break;
						case CategoryEnum.electric:
							category = CategoryEnum.electric;
							break;
						case CategoryEnum.gas:
							category = CategoryEnum.gas;
							break;
						case CategoryEnum.loan:
							category = CategoryEnum.loan;
							break;
						case "life":
							category = CategoryEnum.life;
							break;
						case "car":
							category = CategoryEnum.car;
							break;
						case CategoryEnum.internet:
							category = CategoryEnum.internet;
							break;
						case CategoryEnum.cellphone:
							category = CategoryEnum.cellphone;
							break;
					}
					i++;
				}
				else if (args[i] == "-h" || args[i] == "--help")
				{
					helpFlag = true;
				}
			}
		}

		private void GetCategoryTotals(string[] csvContent)
		{
			for (int i = 1; i < csvContent.Length; i++)
			{
				string[] line = csvContent[i].Split(',');

				double cost = Math.Round(double.Parse(line[3]),2);

				switch (line[2])
				{
					case CategoryEnum.groceries:
						groceriesTotal += cost;
						break;
					case CategoryEnum.toys:
						toysTotal += cost;
						break;
					case CategoryEnum.food:
						foodTotal += cost;
						break;
					case CategoryEnum.rent:
						rentTotal += cost;
						break;
					case CategoryEnum.electric:
						electricTotal += cost;
						break;
					case CategoryEnum.gas:
						gasTotal += cost;
						break;
					case CategoryEnum.loan:
						loanTotal += cost;
						break;
					case CategoryEnum.life:
						lifeTotal += cost;
						break;
					case CategoryEnum.car:
						carTotal += cost;
						break;
					case CategoryEnum.internet:
						internetTotal += cost;
						break;
					case CategoryEnum.cellphone:
						cellphoneTotal += cost;
						break;
				}
			}
		}

		private void CompareAgainstBudget(string[] csvContent)
		{
			var table = new ConsoleTable("Category", "Budget", "Spent", "Percentage");

			for (int i = 1; i < csvContent.Length; i++)
			{
				string[] line = csvContent[i].Split(',');

				double budget = Math.Round(double.Parse(line[1]),2);

				switch (line[0])
				{
					case CategoryEnum.groceries:
						table.AddRow(CategoryEnum.groceries, budget, groceriesTotal, $"{ Math.Round((groceriesTotal/budget)*100) }%");
						break;
					case CategoryEnum.toys:
						table.AddRow(CategoryEnum.toys, budget, toysTotal, $"{ Math.Round((toysTotal/budget)*100) }%");
						break;
					case CategoryEnum.food:
						table.AddRow(CategoryEnum.food, budget, foodTotal, $"{ Math.Round((foodTotal/budget)*100) }%");
						break;
					case CategoryEnum.rent:
						table.AddRow(CategoryEnum.rent, budget, rentTotal, $"{ Math.Round((rentTotal/budget)*100) }%");
						break;
					case CategoryEnum.electric:
						table.AddRow(CategoryEnum.electric, budget, electricTotal, $"{ Math.Round((electricTotal/budget)*100) }%");
						break;
					case CategoryEnum.gas:
						table.AddRow(CategoryEnum.gas, budget, gasTotal, $"{ Math.Round((gasTotal/budget)*100) }%");
						break;
					case CategoryEnum.loan:
						table.AddRow(CategoryEnum.loan, budget, loanTotal, $"{ Math.Round((loanTotal/budget)*100) }%");
						break;
					case CategoryEnum.life:
						table.AddRow(CategoryEnum.life, budget, lifeTotal, $"{ Math.Round((lifeTotal/budget)*100) }%");
						break;
					case CategoryEnum.car:
						table.AddRow(CategoryEnum.car, budget, carTotal, $"{ Math.Round((carTotal/budget)*100) }%");
						break;
					case CategoryEnum.internet:
						table.AddRow(CategoryEnum.internet, budget, internetTotal, $"{ Math.Round((internetTotal/budget)*100) }%");
						break;
					case CategoryEnum.cellphone:
						table.AddRow(CategoryEnum.cellphone, budget, cellphoneTotal, $"{ Math.Round((cellphoneTotal/budget)*100) }%");
						break;
				}
			}

			table.Write();
		}

		public void OutputCategoryTotal(string[] csvContent)
		{
			double totalSpent = 0;

			switch (category)
			{
				case CategoryEnum.groceries:
					totalSpent = groceriesTotal;
					break;
				case CategoryEnum.toys:
					totalSpent = toysTotal;
					break;
				case CategoryEnum.food:
					totalSpent = foodTotal;
					break;
			}

			for (int i = 1; i < csvContent.Length; i++)
			{
				string[] line = csvContent[i].Split(',');

				double budget = double.Parse(line[1]);

				if (line[0] == category)
				{
					Console.WriteLine($"Total: { totalSpent }");
				}
			}
		}

		public void ListMonthTransactions(string[] csvContent)
		{
			var table = new ConsoleTable("Company", "Date", "Category", "Price", "PaidWith");

			if (!string.IsNullOrEmpty(category))
			{
				for(int i = 1; i < csvContent.Length; i++)
				{
					string[] line = csvContent[i].Split(',');

					if (line[2] == category)
					{
						table.AddRow(line[0], line[1], line[2], line[3], line[4]);
					}
				}
			}
			else
			{
				for(int i = 1; i < csvContent.Length; i++)
				{
					string[] line = csvContent[i].Split(',');

					table.AddRow(line[0], line[1], line[2], line[3], line[4]);
				}
			}

			table.Write();
		}

		private void helpScreen()
        {
            StringBuilder helpScreenText = new();

			helpScreenText.AppendLine(Program.versionText);
			helpScreenText.AppendLine("Usage: MoneyTracker check [options]  ");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    -m | --month		Set the month of the budget to be checked");
			helpScreenText.AppendLine("    -y | --year			Set the year of the budget to be checked");
			helpScreenText.AppendLine("    -c | --category		Set the category to be checked");
			helpScreenText.AppendLine("    -l | --list			List all transactions");
			helpScreenText.AppendLine("    -c | --category		Select a specific category to display");
			helpScreenText.AppendLine("    -h | --help			Display this help screen");
			helpScreenText.AppendLine("");

			Console.WriteLine(helpScreenText.ToString());
		}
    }
}