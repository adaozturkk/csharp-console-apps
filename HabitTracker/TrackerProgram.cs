namespace HabitTracker;
using System.Text.Json;

public class TrackerProgram
{
    private List<Habit> Habits = LoadHabits();

    public void CreateProgram()
    {
        while (true)
        {
            // Program menu.
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("============================================");
            Console.WriteLine("                HABIT TRACKER               ");
            Console.WriteLine("============================================");
            Console.WriteLine("1) Add New Habit");
            Console.WriteLine("2) View All Habits");
            Console.WriteLine("3) Mark Habit Completed (today)");
            Console.WriteLine("4) Mark Habit Completed (choose date)");
            Console.WriteLine("5) Unmark Habit Completed");
            Console.WriteLine("6) View Habit Statistics");
            Console.WriteLine("7) Edit Habit");
            Console.WriteLine("8) Remove Habit");
            Console.WriteLine("9) Save & Exit");
            Console.WriteLine("--------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter your choice (1 to 9): ");

            int choice = Validator.GetNumber();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            switch (choice)
            {
                case 1:
                    AddHabit();
                    break;
                case 2:
                    ViewHabits();
                    break;
                case 3:
                    CompleteToday();
                    break;
                case 4:
                    CompleteAnotherDate();
                    break;
                case 5:
                    UnmarkHabit();
                    break;
                case 6:
                    ViewStatistics();
                    break;
                case 7:
                    EditHabit();
                    break;
                case 8:
                    RemoveHabit();
                    break;
                case 9:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("See you later! Exiting program...");

                    Thread.Sleep(1000);
                    Console.ResetColor();
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid menu choice. Enter a number between 1 to 9.");
                    ReturnToMenu();


                    break;
            }

            Console.Clear();
        }
    }

    // Load habits from file, if there is no file create one and assign empty list.
    private static List<Habit> LoadHabits()
    {
        string path = "habits.json";

        if (!File.Exists(path))
        {
            var emptyList = new List<Habit>();

            // Create new file if there is none.
            string emptyJson = JsonSerializer.Serialize(emptyList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, emptyJson);

            return emptyList;
        }
            
        string json = File.ReadAllText(path);
        var list = JsonSerializer.Deserialize<List<Habit>>(json) ?? new List<Habit>();

        // If completed dates list is null, assign an empty list to it.
        foreach (var h in list)
            h.CompletedDates ??= new List<string>();

        return list;
    }

    // Save habits to file, used after making changes in habits.
    private void SaveHabits(List<Habit> habits)
    {
        string json = JsonSerializer.Serialize(habits, new JsonSerializerOptions { WriteIndented = true }); 
        File.WriteAllText("habits.json", json);
    }

    // Add a new habit.
    private void AddHabit()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= ADD NEW HABIT =======");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter a name for your new habit: ");
        string habitName = Validator.GetString();

        // Check empty name.
        if (habitName.Trim() == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Habit name can't be empty.");
            ReturnToMenu();
            return;
        }

        // Check for already existed habit name.
        foreach (var habit in Habits)
        {
            if (habit.Name.Trim().ToLower() == habitName.Trim().ToLower())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A habit with this name already exists.");
                ReturnToMenu();
                return;
            }
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter description (leave blank to pass): ");
        string description = Validator.GetString();

        Habits.Add(new Habit(habitName, description));
        SaveHabits(Habits);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Habit added successfully!");

        ReturnToMenu();
        return;
    }

    // Display all habits.
    private void ViewHabits()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= ALL HABITS =======");

        // Check no habit condition.
        if (Habits.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You haven't added any item yet.");
            ReturnToMenu();
            return;
        }

        for (int i = 0; i < Habits.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Habits[i].Name} - {Habits[i].Description}");
        }

        ReturnToMenu();
        return;
    }

    // Mark a habit completed for today.
    private void CompleteToday()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= MARK HABIT COMPLETED (TODAY) =======");

        var habit = ChooseHabit("mark as done");

        if (habit == null)
            return;

        string date = DateTime.Now.ToString("dd/MM/yyyy");

        if (habit.IsCompletedToday)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You already marked as done today.");
            ReturnToMenu();
            return;
        }

        habit.CompletedDates.Add(date);
        SaveHabits(Habits);

        Console.ForegroundColor= ConsoleColor.Green;
        Console.WriteLine($"Marked '{habit.Name}' as completed for {date}.");

        ReturnToMenu();
        return;
    }

    // Mark a habit completed for chosen date.
    private void CompleteAnotherDate()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= MARK HABIT COMPLETED (ANOTHER DATE) =======");

        var habit = ChooseHabit("mark as done");

        if (habit == null)
            return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter date to complete as marked: ");
        DateTime dateInput = Validator.GetDate();

        string date = dateInput.ToString("dd/MM/yyyy");
        string today = DateTime.Now.ToString("dd/MM/yyyy");

        // Check if date is in the future.
        if (dateInput > DateTime.Now)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't complete for a day in the future.");
            ReturnToMenu();
            return;
        }

        // Check if date is today.
        if (date == today)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Choose mark today from menu to do that.");
            ReturnToMenu();
            return;
        }

        // Check if habit already marked as completed.
        if (habit.CompletedDates.Contains(date))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You already marked as done for {date}.");
            ReturnToMenu();
            return;
        }

        habit.CompletedDates.Add(date);
        SaveHabits(Habits);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Marked '{habit.Name}' as completed for {date}.");

        ReturnToMenu();
        return;
    }

    // Unmark a habit completed for chosen date.
    private void UnmarkHabit()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= UNMARK HABIT =======");

        var habit = ChooseHabit("unmark as completed");

        if (habit == null)
            return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter date to unmark as completed: ");
        DateTime dateInput = Validator.GetDate();

        string date = dateInput.ToString("dd/MM/yyyy");
        string today = DateTime.Now.ToString("dd/MM/yyyy");

        // Check if date is in the future.
        if (dateInput > DateTime.Now)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't unmark for a day in the future.");
            ReturnToMenu();
            return;
        }

        // Check if habit haven't marked as completed.
        if (!habit.CompletedDates.Contains(date))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You haven't marked as completed for {date}.");
            ReturnToMenu();
            return;
        }

        habit.CompletedDates.Remove(date);
        SaveHabits(Habits);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Unmarked '{habit.Name}' as completed for {date}.");

        ReturnToMenu();
        return;
    }

    // Display habit statistics.
    private void ViewStatistics()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= VIEW HABIT STATISTICS =======");

        var habit = ChooseHabit("view statistics");

        if (habit == null)
            return;

        // Turn string dates to datetime and sort them.
        List<DateTime> dates = habit.CompletedDates
            .Select(date => DateTime.Parse(date))
            .ToList();
        dates.Sort();

        int totalDays = 0;

        if (dates.Count > 0)
            totalDays = (DateTime.Now - dates[0]).Days + 1;

        int completedDays = dates.Count;
        int missedDays = totalDays - completedDays;
        double completionRate = 0;
        
        if (totalDays > 0)
        {
            completionRate = (double)completedDays * 100 / totalDays;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Total Days Tracked: {totalDays}");
        Console.WriteLine($"Days Completed: {completedDays}");
        Console.WriteLine($"Days Missed: {missedDays}");
        Console.WriteLine($"Completion Rate: {completionRate:F1}%");

        // Calculate current streak.
        int currentStreak = 0;

        if (dates.Count > 0)
        {
            DateTime day = DateTime.Today;

            for (int i = dates.Count - 1; i >= 0; i--)
            {
                if (dates[i].Date == day)
                {
                    currentStreak++;
                    day = day.AddDays(-1);
                }
                else
                    break;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Current Streak: {currentStreak} days");

        // Calculate longest streak.
        int longestStreak = dates.Count > 0 ? 1 : 0;
        int streak = 1;

        for (int i = 1; i < dates.Count; i++)
        {
            if ((dates[i] - dates[i - 1]).Days == 1)
            {
                streak++;

                if (streak > longestStreak)
                    longestStreak = streak;
            }
            else
                streak = 1;
        }

        Console.WriteLine($"Longest Streak: {longestStreak} days");

        if (dates.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine($"First completion Date: {dates[0]:dd/MM/yyyy}");
            Console.WriteLine($"Last Completion Date: {dates[completedDays - 1]:dd/MM/yyyy}");
        }

        ReturnToMenu();
        return;
    }

    // Edit habit's name and description.
    private void EditHabit()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= EDIT HABIT =======");

        var habit = ChooseHabit("edit");

        if (habit == null)
            return;

        // Change habit's name if user enters.
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter new name (leave blank to keep current): ");
        string? newName = Validator.GetString();

        if (newName != "")
        {
            habit.Name = newName;
        }

        // Change habit's description if user enters.
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter new description (leave blank to keep current): ");
        string? newDescription = Validator.GetString();

        if (newDescription != "")
        {
            habit.Description = newDescription;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Habit updated successfully!");
        SaveHabits(Habits);

        ReturnToMenu();
        return;
    }

    // Remove a habit.
    private void RemoveHabit()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("======= DELETE HABIT =======");

        var habit = ChooseHabit("delete");

        if (habit == null)
            return;

        Habits.Remove(habit);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Habit deleted successfully!");
        SaveHabits(Habits);

        ReturnToMenu();
        return;
    }

    // Helper method to reuse in other methods.
    private Habit? ChooseHabit(string action)
    {
        // Check no habit condition.
        if (Habits.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You haven't added any habits yet.");
            ReturnToMenu();
            return null;
        }

        Console.WriteLine($"Select habit number to {action}:");

        for (int i = 0; i < Habits.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Habits[i].Name} - {Habits[i].Description}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Your choice: ");
        int choice = Validator.GetNumber();

        // Check valid index.
        if (choice < 1 || choice > Habits.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid choice.");
            ReturnToMenu();
            return null;
        }

        return Habits[choice - 1];
    }

    // Helper method to end action.
    private void ReturnToMenu()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey(true);
    }
}
