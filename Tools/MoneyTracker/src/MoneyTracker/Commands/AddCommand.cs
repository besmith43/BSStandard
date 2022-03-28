using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MoneyTracker.Data;

namespace MoneyTracker.Commands
{
    public class AddCommand : ICommand
    {
		public const string COMMAND_NAME = "add";
		public bool helpFlag = false;
		public string day = DateTime.Now.Day.ToString();
		public string month = DateTime.Now.Month.ToString();
		public string year = DateTime.Now.Year.ToString();
		public string csvFileName;
		public StringBuilder csvContent = new();
		public string company;
		public string category = "food";
		public string price;
		public string paidWith = "BarClay";

		public AddCommand(string[] _args)
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

				if (string.IsNullOrEmpty(company))
				{
					Console.WriteLine("Where the purchase was made wasn't given");
					return false;
				}

				if (string.IsNullOrEmpty(category))
				{
					Console.WriteLine("The budgetary category of the purchase wasn't given");
					return false;
				}

				if (string.IsNullOrEmpty(price))
				{
					Console.WriteLine("The cost of the purchase wasn't given");
					return false;
				}
				
				double testPrice;
				var result = double.TryParse(price, out testPrice);

				if (!result)
				{
					Console.WriteLine("The cost of purchase given isn't a valid dollar amount");
					return false;
				}

				if (string.IsNullOrEmpty(paidWith))
				{
					Console.WriteLine("The payment method used was not given");
					return false;
				}

				csvContent.AppendLine($"{ company },{ day }/{ month }/{ year },{ category },{ price },{ paidWith }");

				saveCSV();

				return true;
			});
		}

		private void ParseArgs(string[] args)
		{
			for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-d" || args[i] == "--day")
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
				else if (args[i] == "-w" || args[i] == "--where")
				{
					company = args[i+1];
					i++;
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
							if (string.IsNullOrEmpty(company))
							{
								company = "GBP";
							}
							break;
						case CategoryEnum.electric:
							category = CategoryEnum.electric;
							if (string.IsNullOrEmpty(company))
							{
								company = "City of Cookeville";
							}
							break;
						case CategoryEnum.gas:
							category = CategoryEnum.gas;
							if (string.IsNullOrEmpty(company))
							{
								company = "Krogers";
							}
							break;
						case CategoryEnum.loan:
							category = CategoryEnum.loan;
							if (string.IsNullOrEmpty(company))
							{
								company = "Upstart";
							}
							break;
						case "life":
							category = CategoryEnum.life;
							if (string.IsNullOrEmpty(company))
							{
								company = "Woodmen Life";
							}
							break;
						case "car":
							category = CategoryEnum.car;
							if (string.IsNullOrEmpty(company))
							{
								company = "Penn National";
							}
							break;
						case CategoryEnum.internet:
							category = CategoryEnum.internet;
							if (string.IsNullOrEmpty(company))
							{
								company = "Spectrum";
							}
							break;
						case CategoryEnum.cellphone:
							category = CategoryEnum.cellphone;
							if (string.IsNullOrEmpty(company))
							{
								company = "Verizon";
							}
							break;
					}
					i++;
				}
				else if (args[i] == "-p" || args[i] == "--price")
				{
					price = args[i+1];
					i++;
				}
				else if (args[i] == "-f" || args[i] == "--from")
				{
					switch (args[i+1])
					{
						case PaidWithEnum.cash:
							paidWith = "Cash";
							break;
						case PaidWithEnum.check:
							paidWith = "Check";
							break;
						case PaidWithEnum.barclay:
							paidWith = "BarClay";
							break;
						case PaidWithEnum.paypal:
							paidWith = "PayPal";
							break;
						case PaidWithEnum.paypalCredit:
							paidWith = "PayPalCredit";
							break;
						case PaidWithEnum.capitalOne:
							paidWith = "Capital One";
							break;
					}
					i++;
				}
				else if (args[i] == "-h" || args[i] == "--help")
				{
					helpFlag = true;
				}
				else
				{
					// positional parameters
				}
			}
		}

		private string GetCsvHeaders()
		{
			return "Company,Date,Category,Price,PaidWith";
		}

		private void saveCSV()
		{
			File.WriteAllText(csvFileName, csvContent.ToString());
		}

		private void helpScreen()
        {
            StringBuilder helpScreenText = new();

			helpScreenText.AppendLine(Program.versionText);
			helpScreenText.AppendLine("Usage: MoneyTracker add [options]  ");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    -d | --day			Set the day of purchase");
			helpScreenText.AppendLine("    -m | --month		Set the month of purchase");
			helpScreenText.AppendLine("    -y | --year			Set the year of purchase");
			helpScreenText.AppendLine("    -w | --where		Set where the purchase was made");
			helpScreenText.AppendLine("    -c | --category		Set the category that the purchase fits into");
			helpScreenText.AppendLine("    -p | --price		Set the dollar amount spent");
			helpScreenText.AppendLine("    -f | --from			Set how the purchase was paid for");
			helpScreenText.AppendLine("    -h | --help			Show this print out");
			helpScreenText.AppendLine("");
			helpScreenText.AppendLine("    Category Enums:");
			helpScreenText.AppendLine($"    { CategoryEnum.groceries }, { CategoryEnum.toys }, { CategoryEnum.food }, { CategoryEnum.rent }, { CategoryEnum.electric }, { CategoryEnum.gas }, { CategoryEnum.loan }, { CategoryEnum.life }, { CategoryEnum.car }, { CategoryEnum.internet }, & { CategoryEnum.cellphone }");
			helpScreenText.AppendLine("    From Enums");
			helpScreenText.AppendLine($"    { PaidWithEnum.cash }, { PaidWithEnum.check }, { PaidWithEnum.barclay }, { PaidWithEnum.capitalOne }, { PaidWithEnum.paypal }, { PaidWithEnum.paypalCredit }");
			helpScreenText.AppendLine("");

			Console.WriteLine(helpScreenText.ToString());
		}
    }
}