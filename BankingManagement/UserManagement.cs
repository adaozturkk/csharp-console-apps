namespace BankingManagement;

public class UserManagement
{
    public User User { get; set; }
    private List<User> Users;
    private BankingManager Manager;

    public UserManagement(User user, List<User> users, BankingManager manager)
    {
        User = user;
        Users = users;
        Manager = manager;
    }

    public void DisplayMenu()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("================ MAIN MENU ================");
            Console.WriteLine("1. View Accounts");
            Console.WriteLine("2. Open New Account");
            Console.WriteLine("3. View Balance");
            Console.WriteLine("4. Deposit Money");
            Console.WriteLine("5. Withdraw Money");
            Console.WriteLine("6. Transfer Money");
            Console.WriteLine("7. View Transaction History");
            Console.WriteLine("8. Log Out");
            Console.WriteLine("-------------------------------------------");
            Console.Write("Choose an option (1-8): ");

            int choice = Validator.GetInteger();
            Console.Clear();

            switch (choice)
            {
                case 1:
                    DisplayAccounts();
                    break;
                case 2:
                    CreateAccount();
                    break;
                case 3:
                    DisplayBalance();
                    break;
                case 4:
                    DepositMoney();
                    break;
                case 5:
                    WithdrawMoney();
                    break;
                case 6:
                    TransferMoney();
                    break;
                case 7:
                    DisplayTransactions();
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Logging out...");

                    Manager.CurrentUser = null;
                    Console.ResetColor();
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number between 1 and 8.");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Console.Clear();
            Console.ResetColor();
        }
    }

    // Display all accounts user has.
    private void DisplayAccounts()
    {
        Console.WriteLine("================ YOUR ACCOUNTS ================");

        var accounts = User.BankAccounts;

        if (IsEmpty(accounts))
            return;

        for (int i = 0; i < accounts.Count; i++)
        {
            var account = accounts[i];
            Console.WriteLine($"{i + 1}) [{account.AccountNumber}] - {account.Name} | Balance: {Validator.FormatCurrency(account.Balance)}");
        }
    }

    // Create a new bank account.
    private void CreateAccount()
    {
        Console.WriteLine("================ OPEN NEW ACCOUNT ================");

        Console.Write("Enter a name for your new account: ");
        string name = Validator.GetString();

        Console.Write("Initial deposit amount: ");
        decimal amount = Validator.GetDecimal();

        BankAccount account = new BankAccount(amount, name);
        User.BankAccounts.Add(account);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Account created successfully!");
        Console.WriteLine($"Your account number is: {account.AccountNumber}");

        BankingManager.SaveUsers(Users);
    }

    // Display total balance of user.
    private void DisplayBalance()
    {
        Console.WriteLine("================ DISPLAY BALANCE ================");

        var accounts = User.BankAccounts;

        if (IsEmpty(accounts))
            return;

        decimal balance = accounts.Sum(a => a.Balance);
        Console.WriteLine($"Total balance across all accounts: {Validator.FormatCurrency(balance)}");
    }

    // Deposit money to preferred account.
    private void DepositMoney()
    {
        Console.WriteLine("================ DEPOSIT FUNDS ================");

        if (IsEmpty(User.BankAccounts))
            return;

        // Get account and check its validity.
        var account = GetAccount(User, "Select account to deposit into:");

        if (account == null)
            return;

        // Get a valid amount for deposit.
        Console.Write("Enter amount: ");
        decimal amount = Validator.GetDecimal();

        if (amount <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid amount.");
            return;
        }

        account.Balance += amount;
        account.Transactions.Add(new Transaction(DateTime.Now, "Deposit", amount, account.Balance));
        BankingManager.SaveUsers(Users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Deposit successful! New balance: {Validator.FormatCurrency(account.Balance)}");
    }

    // Withdraw money from preferred account.
    private void WithdrawMoney()
    {
        Console.WriteLine("================ WITHDRAW FUNDS ================");

        if (IsEmpty(User.BankAccounts))
            return;

        // Get account and check its validity.
        var account = GetAccount(User, "Select account to withdraw from:");

        if (account == null)
            return;

        // Get a valid amount for withdraw.
        Console.Write("Enter amount: ");
        decimal amount = Validator.GetDecimal();

        if (amount <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid amount.");
            return;
        }

        if (account.Balance < amount)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't withdraw more than your current balance.");
            return;
        }

        account.Balance -= amount;
        account.Transactions.Add(new Transaction(DateTime.Now, "Withdraw", -amount, account.Balance));
        BankingManager.SaveUsers(Users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Withdraw successful! New balance: {Validator.FormatCurrency(account.Balance)}");
    }

    // Let user transfer money with preferred method.
    private void TransferMoney()
    {
        Console.WriteLine("================ TRANSFER FUNDS ================");

        if (IsEmpty(User.BankAccounts))
            return;

        Console.WriteLine("1 - Transfer between my own accounts");
        Console.WriteLine("2 - Transfer to another user");

        Console.WriteLine("------------------------------------------------");

        Console.Write("Choose an option: ");
        int option = Validator.GetInteger();

        switch(option)
        {
            case 1:
                TransferBetweenAccounts();
                break;
            case 2:
                TransferToAnotherUser();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option.");
                return;
        }
    }

    // Let user to do a money transfer between their accounts.
    private void TransferBetweenAccounts()
    {
        // Get source and destination accounts.
        var sourceAccount = GetAccount(User, "\nSelect source account:");
        if (sourceAccount == null) return;

        var destinationAccount = GetAccount(User, "\nSelect destination account:");
        if (destinationAccount == null) return;

        if (sourceAccount == destinationAccount)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't transfer between same accounts.");
            return;
        }

        // Get a valid amount to transfer.
        Console.Write("Enter amount to transfer: ");
        decimal amount = Validator.GetDecimal();

        if (amount <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid amount.");
            return;
        }

        if (sourceAccount.Balance < amount)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't transfer more than your current balance.");
            return;
        }

        sourceAccount.Balance -= amount;
        destinationAccount.Balance += amount;
        sourceAccount.Transactions.Add(new Transaction(DateTime.Now, "Transfer Out", -amount, sourceAccount.Balance));
        destinationAccount.Transactions.Add(new Transaction(DateTime.Now, "Transfer In", amount, destinationAccount.Balance));
        BankingManager.SaveUsers(Users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nTransfer successful!");
        Console.WriteLine($"[{sourceAccount.AccountNumber}] New Balance: {Validator.FormatCurrency(sourceAccount.Balance)}");
        Console.WriteLine($"[{destinationAccount.AccountNumber}] New Balance: {Validator.FormatCurrency(destinationAccount.Balance)}");
    }

    // Let user to do a money transfer to another user's account.
    private void TransferToAnotherUser()
    {
        var accounts = User.BankAccounts;

        // Get recipient username.
        Console.Write("\nEnter recipient username: ");
        string username = Validator.GetString();

        var user = Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No account found.");
            return;
        }

        if (user == User)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't transfer to your own account.");
            return;
        }

        // Get recipient account number.
        Console.Write("Enter recipient account number: ");
        int accountNumber = Validator.GetInteger();

        var recipientAccount = user.BankAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

        if (recipientAccount == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An account with this account number doesn't exist.");
            return;
        }

        // Get source account from user.
        var sourceAccount = GetAccount(User, "\nSelect your source account:");
        if (sourceAccount == null) return;

        // Get a valid amount to transfer.
        Console.Write("Enter amount to transfer: ");
        decimal amount = Validator.GetDecimal();

        if (amount <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid amount.");
            return;
        }

        if (sourceAccount.Balance < amount)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't transfer more than your current balance.");
            return;
        }

        sourceAccount.Balance -= amount;
        recipientAccount.Balance += amount;
        sourceAccount.Transactions.Add(new Transaction(DateTime.Now, "Transfer Out", -amount, sourceAccount.Balance));
        recipientAccount.Transactions.Add(new Transaction(DateTime.Now, "Transfer In", amount, recipientAccount.Balance));
        BankingManager.SaveUsers(Users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nTransfer successful!");
        Console.WriteLine($"You sent {Validator.FormatCurrency(amount)} to {user.Username} [{recipientAccount.AccountNumber}].");
        Console.WriteLine($"Your new balance on [{sourceAccount.AccountNumber}]: {Validator.FormatCurrency(sourceAccount.Balance)}");
    }

    // Show preferred account's transaction history.
    private void DisplayTransactions()
    {
        Console.WriteLine("================ TRANSACTION HISTORY ================");

        if (IsEmpty(User.BankAccounts))
            return;

        // Get a valid account from user.
        var account = GetAccount(User, "Select account to view history:");

        if (account == null)
            return;

        Console.WriteLine($"\nTransaction history for account [{account.AccountNumber}]:");
        Console.WriteLine("Date                   Type           Amount        Balance After");
        Console.WriteLine("-------------------------------------------------------------------");

        if (account.Transactions.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("You haven't done any transactions.");
            return;
        }

        foreach (var transaction in account.Transactions)
        {
            Console.WriteLine($"{transaction.Date, -22} {transaction.Type, -13} {Validator.FormatCurrency(transaction.Amount), 11} {Validator.FormatCurrency(transaction.Balance), 18}");
        }
    }

    // Helper method to get accounts according to user's choice.
    private BankAccount GetAccount(User user, string prompt)
    {
        Console.WriteLine(prompt);

        for (int i = 0; i < user.BankAccounts.Count; i++)
        {
            var acc = user.BankAccounts[i];
            Console.WriteLine($"{i + 1}) [{acc.AccountNumber}] - {acc.Name} (Balance: {Validator.FormatCurrency(acc.Balance)})");
        }

        Console.WriteLine("-----------------------------------------------");

        Console.Write("Enter choice: ");
        int choice = Validator.GetInteger();

        if (choice < 1 || choice > user.BankAccounts.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid account.");
            return null;
        }

        return user.BankAccounts[choice - 1];
    }

    // Helper method for deciding if given accounts list empty or not.
    private bool IsEmpty(List<BankAccount> accounts)
    {
        if (accounts.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("You haven't created any accounts yet.");
            return true;
        }

        return false;
    }
}
