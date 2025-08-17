public static class Validator
{
    public static int GetNumber()
    {
        string? input = Console.ReadLine();
        
        if (input == null)
            return 0;

        bool validInt = int.TryParse(input, out int number);

        if (!validInt)
            return 0;

        return number;
    }

    public static string GetString()
    {
        string? input = Console.ReadLine();

        while (input == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Try again: ");
            input = Console.ReadLine();
        }

        return input;
    }

    public static DateTime GetDate()
    {
        string? input = Console.ReadLine();
        bool validDate = DateTime.TryParse(input, out DateTime date);

        while (input == null || !validDate)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid date. Try again: ");
            input = Console.ReadLine();
            validDate = DateTime.TryParse(input, out date);
        }

        return date;
    }
}