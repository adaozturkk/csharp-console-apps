namespace BankingManagement;
using System.Text.Json;

public class BankingManager
{
    public List<User> Users = LoadUsers();
    public User CurrentUser = null;

    public void RunManagement()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("================ BANKING MANAGEMENT ================");
            Console.WriteLine("1. Sign Up (Create a New User)");
            Console.WriteLine("2. Sign In (Login to Existing Account)");
            Console.WriteLine("3. Exit Program");
            Console.WriteLine("----------------------------------------------------");
            Console.Write("Choose an option (1-3): ");

            int choice = Validator.GetInteger();
            Console.Clear();

            switch (choice)
            {
                case 1:
                    SignUp();
                    break;
                case 2:
                    SignIn();
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Exiting program...");

                    Console.ResetColor();
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number between 1 and 3.");
                    break;
            }

            if (CurrentUser != null)
            {
                UserManagement userManagement = new UserManagement(CurrentUser, Users, this);
                userManagement.DisplayMenu();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Console.Clear();
            Console.ResetColor();
        }
    }

    // Load user data's from file.
    private static List<User> LoadUsers()
    {
        string path = "banking.json";

        if (!File.Exists(path))
        {
            List<User> emptyUsers = new List<User>();
            string emptyJson = JsonSerializer.Serialize(emptyUsers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, emptyJson);
        }

        string json = File.ReadAllText(path);
        List<User> list = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        // Get max bank account number from file to update based on that.
        var allAccounts = list.SelectMany(u => u.BankAccounts);
        int maxAcc = allAccounts.Any() ? allAccounts.Max(a => a.AccountNumber) : 999;
        BankAccount.SetCounter(maxAcc + 1);

        return list;
    }

    // Save user data's to file.
    public static void SaveUsers(List<User> users)
    {
        string path = "banking.json";

        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    // Create a new account.
    private void SignUp()
    {
        Console.WriteLine("================ USER REGISTRATION ================");

        Console.Write("Enter a username: ");
        string username = Validator.GetString();

        // Check if username already used before.
        foreach (User userItem in Users)
        {
            if (userItem.Username == username)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This username already exists.");
                return;
            }
        }

        // Get password from user twice to verify.
        Console.Write("Enter a password: ");
        string password = Validator.GetPassword();

        Console.Write("Confirm password: ");
        string confirmedPassword = Validator.GetPassword();

        Console.WriteLine("---------------------------------------------------");

        // Check if passwords match or not.
        if (!password.Equals(confirmedPassword))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Passwords do not match");
            return;
        }

        // Hash password before saving in file for security.
        string hashedPassword = Validator.HashPassword(password);

        User user = new User(username, hashedPassword);
        Users.Add(user);
        SaveUsers(Users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("User created successfully!");
    }

    // Login to an account.
    private void SignIn()
    {
        Console.WriteLine("================ USER LOGIN ================");

        // Check empty users condition.
        if (Users.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No users have been registered yet.");
            return;
        }

        // Get username.
        Console.Write("Enter your username: ");
        string username = Validator.GetString();

        // Check if a user with this username exists or not.
        var user = Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No account found.");
            return;
        }

        // Get password and hash.
        Console.Write("Enter your password: ");
        string password = Validator.GetPassword();
        string hashedPassword = Validator.HashPassword(password);

        Console.WriteLine("--------------------------------------------");

        if (hashedPassword.Equals(user.Password))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome back, {user.Username}!");
            CurrentUser = user;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong password. Try again.");
        }

        return;
    }
}
