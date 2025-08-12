namespace TypeSpeed;

public class SpeedTracker
{
    public async Task CreateProgram()
    {
        // Paragraph for testing.
        string sentence =
    "The gentle breeze whispered through the towering trees, carrying the scent of blooming flowers across the peaceful meadow. " +
    "As the golden sun dipped below the horizon, casting vibrant hues of orange and pink, the world seemed to slow down, inviting quiet reflection. " +
    "In the distance, birds sang their final songs of the day, creating a soothing symphony that echoed softly in the twilight.";

        // Describe program to user.
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Welcome to Type Speed Tracker!");
        Console.WriteLine("You'll be given a paragraph to type exactly as it appears.");
        Console.WriteLine("Pay attention to uppercase letters and punctuation marks.");
        Console.WriteLine("Paragraph is below:");
        Console.WriteLine();
        Console.WriteLine(sentence);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Press 'Y' when you are ready to start: ");

        string? choice = Console.ReadLine()?.Trim().ToLower();

        if (choice == null || choice != "y")
            return;

        await CountToStart();

        // Get time before and after user types text to get total time elapsed.
        DateTime before = DateTime.Now;

        Console.ForegroundColor = ConsoleColor.Cyan;
        string? textInput = Console.ReadLine();
        Console.WriteLine();

        if (textInput == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid text");
            return;
        }

        DateTime after = DateTime.Now;
        TimeSpan timeSpan = after - before;
        TypeResult result = new(textInput, timeSpan);

        Console.ForegroundColor = ConsoleColor.White;
        result.DisplayTime();
        result.DisplayWPM();
        result.DisplayAccuracy(sentence);
        result.DisplayColoredText(sentence);
    }

    // Display a countdown message from 3 to 1.
    private Task CountToStart()
    {
        return Task.Run(async () =>
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"{i}...");
                await Task.Delay(1000);
            }

            Console.WriteLine("Start!");
        });
    }
}
