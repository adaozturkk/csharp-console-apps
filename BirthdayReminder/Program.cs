// Create app.
BirthdayReminder reminder = new BirthdayReminder();
reminder.Create();

public class BirthdayReminder
{
    // Create a people list to keep track.
    private List<Person> people = new List<Person>();

    public void Create()
    {
        Console.Title = "Birthday Reminder";

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Welcome to Birthday Reminder!");
        Console.WriteLine("With the help of this app, you can track birthdays of your loved ones.");

        // Created for observing if user wants to exit or not.
        bool isEnded = false;

        // Until user selects exit, display app logic.
        while (!isEnded)
        {
            Console.ForegroundColor= ConsoleColor.White;

            Console.WriteLine();
            Console.WriteLine("Write the number of action you want to do (1 to 6).");
            Console.WriteLine("1. Add a new person");
            Console.WriteLine("2. List all people");
            Console.WriteLine("3. Edit a person");
            Console.WriteLine("4. Delete a person");
            Console.WriteLine("5. Show today's birthdays");
            Console.WriteLine("6. Show upcoming birthdays");
            Console.WriteLine("7. Exit");

            // Get a valid menu choice and apply based on number.
            int choice = Validator.GetValidNumber();
            switch (choice)
            {
                case 1:
                    AddPerson();
                    break;
                case 2:
                    DisplayList();
                    break;
                case 3:
                    EditPerson();
                    break;
                case 4:
                    DeletePerson();
                    break;
                case 5:
                    DisplayTodaysBirthdays();
                    break;
                case 6:
                    DisplayCloseBirthdays();
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Exiting app...");
                    isEnded = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can only write a number in range 1 to 7.");
                    break;
            }
        }

        Console.ResetColor();
    }

    // Add a new person to list and display confirmation message.
    private void AddPerson()
    {
        string name = Validator.GetValidName();
        DateTime birthday = Validator.GetValidBirthday();
        people.Add(new Person(name, birthday));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{name} added successfully!");
    }

    // Display people in list with their characteristics.
    private void DisplayList()
    {
        if (people.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no one in the list yet.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("People in list:");
        for (int count = 0; count < people.Count; count++)
        {
            Person person = people[count];

            Console.WriteLine($"ID: {person.Id} - {person.Name} - {person.Birthday.Day}." +
                $"{person.Birthday.Month}.{person.Birthday.Year} ({people[count].GetAge()} years old)");
        }
    }

    // Edit a person's name or birthday who is in the list.
    private void EditPerson()
    {
        DisplayList();

        if (people.Count == 0) return;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Write the ID of person you want to edit (or -1 to go back to menu): ");
        int id = Validator.GetValidNumber();

        if (id == -1) return;

        bool validId = false;

        for (int count = 0; count < people.Count; count++)
        {
            if (id == people[count].Id)
            {
                Console.Write("Write the part you want to edit (name/birthday): ");
                string? editInput = Console.ReadLine()?.Trim().ToLower();

                while (editInput == null || (editInput != "name" && editInput != "birthday"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid input. Please write name or birthday: ");
                    editInput = Console.ReadLine()?.Trim().ToLower();
                }

                Console.ForegroundColor = ConsoleColor.Green;

                if (editInput == "name")
                {

                    people[count].Name = Validator.GetValidName();
                    Console.WriteLine("Name updated successfully.");
                }
                    
                if (editInput == "birthday")
                {
                    people[count].Birthday = Validator.GetValidBirthday();
                    Console.WriteLine("Birthday updated successfully.");
                }

                validId = true;
                break;
            }
        }

        if (!validId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no one with this ID.");
        }
    }

    // Delete a person who is in the list.
    private void DeletePerson()
    {
        DisplayList();

        if (people.Count == 0) return;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Write the ID of person you want to delete (or -1 to go back to menu): ");
        int id = Validator.GetValidNumber();

        if (id == -1) return;

        bool validId = false;

        for (int count = 0; count < people.Count; count++)
        {
            if (id == people[count].Id)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{people[count].Name} has been successfully removed from the list.");
                people.RemoveAt(count);

                validId = true;
                break;
            }
        }

        if (!validId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no one with this ID.");
        }
    }

    // If a person's birthday is today, display it.
    private void DisplayTodaysBirthdays()
    {
        bool isBirthdayToday = false;

        for (int count = 0; count < people.Count; count++)
        {
            Person person = people[count];

            if (person.Birthday.Day == DateTime.Today.Day && person.Birthday.Month == DateTime.Today.Month)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Today is {person.Name}'s birthday! ({person.GetAge()} years old)");
                isBirthdayToday = true;
            }
        }

        if (!isBirthdayToday)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no birthday today.");
        }    
    }

    // If a person's birthday is within 7 days, display it.
    private void DisplayCloseBirthdays()
    {
        DateTime today = DateTime.Today;

        bool isCloseBirthday = false;

        for (int count = 0; count < people.Count; count++)
        {
            Person person = people[count];

            // Calculate next birthday of person.
            DateTime nextBirthday = new DateTime(today.Year, person.Birthday.Month, person.Birthday.Day);
            if (nextBirthday < today)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }

            TimeSpan span = nextBirthday - today;

            if (span.TotalDays >= 1 && span.TotalDays <= 7)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{person.Name}'s birthday is within {span.TotalDays} days! " +
                    $"({person.Birthday.Day}.{person.Birthday.Month}.{person.Birthday.Year})");

                isCloseBirthday = true;
            }
        }

        if (!isCloseBirthday)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no upcoming birthdays within 7 days.");
        }
    }
}

public class Person
{
    // Created for incrementing ID when a new person instance created.
    private static int idCounter = 1;

    public int Id { get; private set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; }

    public Person(string name, DateTime birthday)
    {
        Id = idCounter++;
        Name = name;
        Birthday = birthday;
    }

    // Return person's age.
    public int GetAge()
    {
        DateTime today = DateTime.Today;
        int age = today.Year - Birthday.Year;

        if (Birthday > today.AddYears(-age))
            age--;

        return age;
    }
}

// Validates user input according to their types.
public static class Validator
{
    public static int GetValidNumber()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        string? input = Console.ReadLine()?.Trim();

        bool isInt = int.TryParse(input, out int number);

        while (input == null || !isInt)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Please enter a valid number: ");
            input = Console.ReadLine()?.Trim();

            isInt = int.TryParse(input, out number);
        }

        return number;
    }

    public static string GetValidName()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Enter name: ");
        string? input = Console.ReadLine()?.Trim();

        while(input == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please enter a name: ");
            input = Console.ReadLine()?.Trim();
        }

        return input;
    }

    public static DateTime GetValidBirthday()
    {
        Console.ForegroundColor= ConsoleColor.Cyan;
        Console.Write("Enter birthday in format dd.MM.yyyy (for instance 12.05.1994): ");
        string? input = Console.ReadLine()?.Trim();

        bool isDate = DateTime.TryParse(input, out DateTime birthday);
        DateTime today = DateTime.Today;

        while (input == null || !isDate || birthday > today)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Please enter a valid birthday: ");
            input = Console.ReadLine()?.Trim();

            isDate = DateTime.TryParse(input, out birthday);
        }

        return birthday;
    }
}