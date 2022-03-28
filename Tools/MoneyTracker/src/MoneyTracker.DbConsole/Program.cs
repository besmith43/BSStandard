using MoneyTracker.Data;
using MoneyTracker.Class;
using MoneyTracker.Class.Enums;

DbActions db = new DbActions();

var budgetSummaries = db.GetBudgetSummaries();

foreach (var bsItem in budgetSummaries)
{
    Console.WriteLine("==================================================");
    Console.WriteLine($"Category: { bsItem.Category }");
    Console.WriteLine($"Budget Total: { bsItem.BudgetTotal }");
    Console.WriteLine($"Spent: { bsItem.Spent }");
    Console.WriteLine($"Percent: { bsItem.Percent }"); 
    Console.WriteLine("==================================================");
}

var newPurchase = new Transaction
{
    Category = CategoryEnum.food,
    Vendor = "McDonalds",
    Date = DateTime.Today,
    Price = 5.00M,
    PaymentSource = "Capital One"
};

db.AddTransaction(newPurchase);

budgetSummaries = db.GetBudgetSummaries();

foreach (var bsItem in budgetSummaries)
{
    Console.WriteLine("==================================================");
    Console.WriteLine($"Category: { bsItem.Category }");
    Console.WriteLine($"Budget Total: { bsItem.BudgetTotal }");
    Console.WriteLine($"Spent: { bsItem.Spent }");
    Console.WriteLine($"Percent: { bsItem.Percent }"); 
    Console.WriteLine("==================================================");
}