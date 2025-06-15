// Opening of program.
Console.Title = "English Vocabulary Quiz";

Console.WriteLine("Welcome to English Vocabulary Quiz!");
Console.WriteLine("Test your vocabulary skills by choosing a level below.");
Console.WriteLine("Each level has 5 multiple choice questions.");
Console.WriteLine();

// Option names for each question.
string[] options = { "A", "B", "C", "D" };

int totalScore = 0;

// Ask user which quiz they will enter.
int preferredLevel = GetLevel();

// Questions and correct answers for all levels.
string[,] a1Questions = {{ "The flesh (muscle tissue) of an animal used as food", "meat" },
    {"A motor vehicle for transporting large numbers of people along roads", "bus"}, 
    {"With no or few possessions or money, particularly in relation to contemporaries who do have them", "poor"},
    {"A large body of salt water", "sea" },
    {"A piece of furniture, usually flat and soft, on which to rest or sleep", "bed" } };

string[,] a2Questions = { { "The dimensions or magnitude of a thing; how big something is", "size" },
    {"To hold the status of something", "keep" }, 
    {"A small case, often flat and often made of leather, for keeping money (especially paper money), credit cards, etc", "wallet" },
    {"One who belongs to a group", "member" }, {"In or at any location", "anywhere" }};

string[,] b1Questions = { {"To stop (an outcome); to keep from (doing something)", "prevent" },
    {"The lowest limit", "minimum" }, {"Entertaining", "amusing" }, {"Inside, into, or within a building", "indoors" },
    { "To overcome in battle or contest", "defeat" }};

string[,] b2Questions = { {"An institution of higher education and its ambiance", "campus" },
    {"A prohibition", "ban" }, {"A hold or way of holding, particularly with the hand", "grip"},
    {"Once every year without fail, yearly", "annually"},
    {"To violate rules in order to gain, or attempt to gain, advantage from a situation", "cheat" } };

string[,] c1Questions = { { "To make by gathering pieces from various sources", "compile" }, {"Only", "sole" },
    {"A person who is intentionally physically or emotionally cruel to others, especially to those whom they perceive as being vulnerable or of less power or privilege", "bully" },
    {"Something that impedes, stands in the way of, or holds up progress, either physically or figuratively", "obstacle" },
    {"Discouraging; causing annoyance or anger by excessive difficulty", "frustrating" }};

string[,] c2Questions = { { "To determine which disease is causing a sick person's signs and symptoms", "diagnose" },
    {"To thrive or grow well", "flourish" }, {"Causing a desire to know more; mysterious", "intriguing" },
    {"Open to multiple interpretations", "ambiguous" }, {"To refuse to obey", "defy" } };

// Question options for all levels.
string[,] a1Options = { {"milk", "fruit", "meat", "breakfast" }, {"taxi", "bus", "car", "bicycle" },
    {"rich", "poor", "ready", "happy" }, {"sea", "lake", "sky", "ground" }, {"table", "chair", "television", "bed" } };

string[,] a2Options = { { "size", "color", "sound", "name" }, { "come", "hide", "give", "keep" },
    { "bag", "suitcase", "wallet", "sunglasses" }, { "friend", "guest", "member", "nobody" },
    { "anywhere", "somewhere", "everywhere", "nowhere" } };

string[,] b1Options = { { "catch", "start", "allow", "prevent" }, { "maximum", "middle", "minimum", "average" },
    { "amusing", "boring", "easy", "tiring" }, { "next-door", "indoors", "outdoors", "nearby" },
    { "fight", "defeat", "argument", "lose" } };

string[,] b2Options = { { "campus", "garden", "classrom", "school" }, { "allow", "freedom", "ban", "sign" },
    { "drop", "grab", "burst", "grip" } , { "monthly", "regular", "daily", "annually" },
    { "failure", "cheat", "compete", "try" } };

string[,] c1Options = { { "run", "compile", "edit", "collect" }, { "some", "sole", "every", "unique" },
    { "leader", "ruler", "bully", "generous" }, { "obstacle", "reinforcement", "support", "burden" },
    { "encouraging", "heartening", "entertainment", "frustrating" }, };

string[,] c2Options = { { "infectious", "prescribe", "heal", "diagnose" }, { "deteriorate", "expand", "widen", "flourish" },
    { "tedious", "intriguing", "complex", "dull" }, { "precise", "ambiguous", "determinate", "coherence" },
    { "defy", "obedience", "eminent", "underestimate" } };

// Game logic.
EvaluateInput(preferredLevel);

DisplayResult(totalScore);

AskPlayAgain();

// ---------------- METHODS ----------------

// Decide which quiz will be done according to preferred level.
void EvaluateInput(int level)
{
    switch (level)
    {
        case 1:
            DisplayQuestions(a1Questions, a1Options);
            break;
        case 2:
            DisplayQuestions(a2Questions, a2Options);
            break;
        case 3:
            DisplayQuestions(b1Questions, b1Options);
            break;
        case 4:
            DisplayQuestions(b2Questions, b2Options);
            break;
        case 5:
            DisplayQuestions(c1Questions, c1Options);
            break;
        case 6:
            DisplayQuestions(c2Questions, c2Options);
            break;

    }
}

// Ask user quiz number and check validity.
int GetLevel()
{
    Console.WriteLine("Please choose your difficulty level:");
    Console.WriteLine("1. A1 (Elemantary)");
    Console.WriteLine("2. A2 (Pre-intermediate)");
    Console.WriteLine("3. B1 (Intermediate)");
    Console.WriteLine("4. B2 (Upper Intermediate)");
    Console.WriteLine("5. C1 (Advance)");
    Console.WriteLine("6. C2 (Proficiency)");
    Console.Write("Plese select level (1-6): ");
    
    string input = Console.ReadLine();
    bool validInput = int.TryParse(input, out int level);

    while (!validInput || (level < 1 || level > 6))
    {
        Console.WriteLine("Invalid level. Please enter a number between 1 to 6.");

        input = Console.ReadLine();
        validInput = int.TryParse(input, out level);
    }

    return level;
}

// Display questions and check for answers.
void DisplayQuestions(string[,] questionsArray, string[,] optionsArray)
{
    Console.Clear();

    for (int i = 0; i < questionsArray.GetLength(0); i++)
    {
        Console.WriteLine($"{i + 1}- {questionsArray[i, 0]}:");

        for (int j = 0; j < 4; j++)
            Console.WriteLine($"{options[j]}) {optionsArray[i, j]}");

        string answerInput = GetAnswerFromUser();

        EvaluateAnswer(answerInput, questionsArray[i, 1], optionsArray, i);
        
        Console.WriteLine();
    }
}

// Get user's answer to the question and check validity.
string GetAnswerFromUser ()
{
    while (true)
    {
        Console.Write("Enter your answer (A/B/C/D): ");
        string? answerInput = Console.ReadLine()?.Trim().ToUpper();

        foreach (string option in options)
        {
            if (option == answerInput)
                return answerInput;
        }

        Console.WriteLine("Invalid input. Please enter A, B, C, or D.");
    }
}

// Decide whether user's answer is correct or not.
void EvaluateAnswer(string userAnswer, string correctAnswer, string[,] optionsArray, int questionIndex)
{
    int selectedIndex = 0;

    for (int i = 0; i < options.Length; i++)
    {
        if (options[i] == userAnswer)
        {
            selectedIndex = i;
            break;
        }
    }

    string selectedAnswer = optionsArray[questionIndex, selectedIndex];

    if (selectedAnswer == correctAnswer)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Your answer is correct!");

        totalScore++;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Wrong answer! Correct answer was {correctAnswer}.");
    }

    Console.ResetColor();
}

// Display result of quiz.
void DisplayResult(int score)
{
    Console.Clear();

    Console.WriteLine("Quiz completed!");
    Console.WriteLine($"Your Score: {score}/5");

    if (score == 5)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Excellent work!");
    }
    else if (score >= 3)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Good job! Keep practicing.");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Don't worry! Try again to improve your vocabulary.");
    }

    Console.ResetColor();
}

// Ask if user wants to retry and restart quiz based on answer.
void AskPlayAgain()
{
    Console.Write("\nWould you like to try another level? (yes/no): ");

    string? againInput = Console.ReadLine()?.Trim().ToLower();

    if (againInput == "yes")
    {
        Console.Clear();
        totalScore = 0;

        preferredLevel = GetLevel();
        EvaluateInput(preferredLevel);

        DisplayResult(totalScore);
        AskPlayAgain();
    }
}