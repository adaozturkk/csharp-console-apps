namespace BankingManagement;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public static class Validator
{
    // Get a valid integer input from user.
    public static int GetInteger()
    {
        string? input = Console.ReadLine();
        bool validInt = int.TryParse(input, out int number);

        while (string.IsNullOrWhiteSpace(input) || !validInt)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Try again: ");
            input = Console.ReadLine();
            validInt = int.TryParse(input, out number);
        }

        Console.ForegroundColor = ConsoleColor.White;
        return number;
    }

    // Get a valid string input from user.
    public static string GetString()
    {
        string? input = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Try again: ");
            input = Console.ReadLine();
        }

        Console.ForegroundColor = ConsoleColor.White;
        return input;
    }

    // Get a valid decimal input from user.
    public static decimal GetDecimal()
    {
        string? input = Console.ReadLine();
        bool validDecimal = decimal.TryParse(input, out decimal number);

        while (string.IsNullOrWhiteSpace(input) || !validDecimal)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Try again: ");
            input = Console.ReadLine();
            validDecimal = decimal.TryParse(input, out number);
        }

        Console.ForegroundColor = ConsoleColor.White;
        return number;
    }

    // Get a valid password input from user.
    public static string GetPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Ekrandan bir karakter sil
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    // Return given password to hashed version for security..
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }

    // Format given amount to display with currency.
    public static string FormatCurrency(decimal amount)
    {
        return amount.ToString("C", CultureInfo.GetCultureInfo("en-US"));
    }
}