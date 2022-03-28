namespace MoneyTracker.Class;

public class Transaction
{
    public int ID { get; set; }
    public string Category { get; set; }
    public string Vendor { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public string PaymentSource { get; set; }
}