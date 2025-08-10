namespace TypeSpeed;

public class TypeResult(string userText, TimeSpan span)
{
    private readonly string UserText = userText;
    private readonly TimeSpan Span = span;

    // Display total time elapsed while writing text.
    public void DisplayTime() =>
        Console.WriteLine($"Time taken: {Span.Minutes} minutes {Span.Seconds} seconds");

    // Display word per minute ratio.
    public void DisplayWPM()
    {
        // Calculate total words and minutes spent.
        int wordCount = UserText.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;
        double time = Span.TotalSeconds / 60;

        // Prevent division by 0 error.
        if (time < 0.01) time = 0.01;

        double wpm = wordCount / time;
        Console.WriteLine($"Typing speed: {wpm:F2} words per minute (WPM)");
    }

    // Displacy accuracy of user's input with comparing it to original text.
    public void DisplayAccuracy(string originalText)
    {
        // Calculate word counts for both texts.
        string[] userWords = UserText.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] originalWords = originalText.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        // Keep track of correct words;
        int correctCounter = 0;

        // Determine which array is shorter in order to use that for bound in loop.
        int shortTextLength = Math.Min(userWords.Length, originalWords.Length);

        for (int i = 0; i < shortTextLength; i++)
        {
            if (userWords[i] == originalWords[i])
                correctCounter++;
        }

        // Calculate total words to use in formule because user may type more words than the original one.
        int totalWords = Math.Max(userWords.Length, originalWords.Length);

        double accuracy = (double)correctCounter / totalWords * 100;
        Console.WriteLine($"Accuracy: %{accuracy:F2}");
    }

    // Display user's text with green for correct words and red for incorrects.
    public void DisplayColoredText(string originalText)
    {
        string[] userWords = UserText.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] originalWords = originalText.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        // Determine which array is shorter in order to use that for bound in loop.
        int shortTextLength = Math.Min(userWords.Length, originalWords.Length);

        Console.WriteLine("Your typed text with accuracy highlights:");

        for (int i = 0; i < shortTextLength; i++)
        {
            if (userWords[i] == originalWords[i])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(userWords[i] + " ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(userWords[i] + " ");
            }
        }

        // If user types a text longer than original one, display red for all the rest.
        if (userWords.Length > originalWords.Length)
        {
            for (int i = originalWords.Length; i < userWords.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(userWords[i] + " ");
            }
        }

        Console.ResetColor();
    }
}