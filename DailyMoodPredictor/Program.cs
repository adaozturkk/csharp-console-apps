// Introduction of program.
Console.Title = "Daily Mood Predictor";
Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Welcome to Daily Mood Predictor!");
Console.WriteLine("This program predicts how your day can go based on the weather and your activities.");
Console.WriteLine("If you are ready, press any key to start...");
Console.ReadKey(true);

int totalScore = 0;

// Get and handle wake up time input.
Console.ForegroundColor = ConsoleColor.Yellow;

Console.Write("\nWhat time did you wake up? (0-23): ");

int time = GetTime();
CalculateTimeScore(time);

// Get and handle weather input.
Console.ForegroundColor = ConsoleColor.Cyan;

Console.WriteLine("\nHow is the weather today? Please select the one suits most.");
Console.WriteLine("1. Sunny");
Console.WriteLine("2. Cloudy");
Console.WriteLine("3. Rainy");
Console.WriteLine("4. Snowy");

HandleWeatherInput();

// Get and handle sleep input.
Console.ForegroundColor = ConsoleColor.Magenta;

Console.Write("\nHow many hours did you sleep? (HH): ");

int sleep = GetSleep();
CalculateSleepScore(sleep);

// Get and handle plan input.
Console.ForegroundColor = ConsoleColor.Green;

Console.Write("\nLast question, do you have a plan for today? (yes/no): ");

string hasPlan = GetPlan();
CalculatePlanScore(hasPlan);

// Display result.
DisplayResult(totalScore);

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey(true);

Console.ResetColor();

// ----------------- METHODS -----------------

// Get time from user and check validity.
int GetTime()
{
    string? inputHour = Console.ReadLine();
    bool validInput = int.TryParse(inputHour, out int hour);

    while (!validInput || (hour < 0 || hour > 23))
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("Invalid hour. Please enter a value between 0 and 23.");
        inputHour = Console.ReadLine();
        validInput = int.TryParse(inputHour, out hour);
    }

    Console.ForegroundColor = ConsoleColor.White;
    return hour;
}

// Get sleep hours from user and check validity.
int GetSleep()
{
    string? inputSleep = Console.ReadLine();
    bool validInput = int.TryParse(inputSleep, out int sleep);

    while (!validInput || (sleep < 0))
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("Invalid input. Please enter a valid number.");
        inputSleep = Console.ReadLine();
        validInput = int.TryParse(inputSleep, out sleep);
    }

    Console.ForegroundColor = ConsoleColor.White;
    return sleep;
}

// Get if they have a plan or not from user and check validity.
string GetPlan()
{
    string? inputPlan = Console.ReadLine();

    while (inputPlan == null || (inputPlan.ToLower() != "yes" && inputPlan.ToLower() != "no"))
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("Invalid input. Please enter yes or no.");
        inputPlan = Console.ReadLine();
    }

    Console.ForegroundColor = ConsoleColor.White;
    return inputPlan.ToLower();
}

// Get weather input from user and check validity. Then, add score to total score according to that.
void HandleWeatherInput()
{
    string? inputOption = Console.ReadLine();
    bool validInput = int.TryParse(inputOption, out int weatherOption);

    while (!validInput || (weatherOption < 1 || weatherOption > 4))
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("Invalid number. Please enter a value between 1 and 4.");
        inputOption = Console.ReadLine();
        validInput = int.TryParse(inputOption, out weatherOption);
    }

    switch (weatherOption)
    {
        case 1:
            AddToScore(10);
            break;
        case 2:
            AddToScore(4);
            break;
        case 3:
            AddToScore(2);
            break;
        case 4:
            AddToScore(6);
            break;
    }

    Console.ForegroundColor = ConsoleColor.White;
}

// Add points to total score according to when did user wake up.
void CalculateTimeScore(int hour)
{
    if (hour >= 18)
        AddToScore(1);
    else if (hour >= 15)
        AddToScore(2);
    else if (hour >= 12)
        AddToScore(3);
    else if (hour >= 9)
        AddToScore(4);
    else if (hour >= 5)
        AddToScore(5);
    else
        AddToScore(1);
}

// Add points to total score according to how many hours user slept.
void CalculateSleepScore(int sleep)
{
    if (sleep <= 4)
        AddToScore(2);
    else if (sleep <= 6)
        AddToScore(6);
    else if (sleep <= 8)
        AddToScore(10);
    else if (sleep <= 10)
        AddToScore(6);
    else
        AddToScore(2);
}

// Add points to total score according to users plan.
void CalculatePlanScore(string hasPlan)
{
    if (hasPlan == "yes")
        AddToScore(5);
    else
        AddToScore(0);
}

// According to total score, display result to user with unique messages.
void DisplayResult(int totalScore)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;

    Console.WriteLine($"Your daily score: {totalScore}");

    Console.WriteLine();

    if (totalScore > 25)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Prediction: An amazing day waits for you, enjoy your time!");
    }
    else if (totalScore > 20)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Prediction: Today looks pretty good, keep shining!");
    }
    else if (totalScore > 10)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Prediction: An average day waits for you, stay positive.");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Prediction: Today can be rough, be kind to yourself.");
    }
}

// Helper method for add points to total score.
void AddToScore(int point)
{
    totalScore += point;
}