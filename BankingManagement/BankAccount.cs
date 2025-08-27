namespace BankingManagement;

public class BankAccount
{
    public int AccountNumber {  get; set; }
    public decimal Balance { get; set; }
    public string Name { get; set; }
    public List<Transaction> Transactions { get; set; }

    private static int Counter = 1000;

    public BankAccount(decimal balance, string name )
    {
        Balance = balance;
        Transactions = new List<Transaction>();
        AccountNumber = Counter++;
        Name = name;
    }

    public static void SetCounter(int number) => Counter = number;
}
