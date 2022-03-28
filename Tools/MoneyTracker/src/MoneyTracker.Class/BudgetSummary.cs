namespace MoneyTracker.Class;

public class BudgetSummary
{
    public int ID { get; set; }
    public string Category { get; set; }
    public decimal BudgetTotal { get; set; }
    public decimal Spent { get; set; }
    public int Percent { get; set; }
}