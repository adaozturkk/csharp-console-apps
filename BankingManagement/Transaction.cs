namespace BankingManagement;

public class Transaction(DateTime date, string type, decimal amount, decimal balance)
{
    public DateTime Date { get; set; } = date;
    public string Type { get; set; } = type;
    public decimal Amount { get; set; } = amount;
    public decimal Balance { get; set; } = balance;

}
