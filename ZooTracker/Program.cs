// Create program.
ZooTracker zoo = new ZooTracker();
zoo.Create();

public class ZooTracker
{
    // List to track animals.
    private List<Animal> animals = new List<Animal>();

    public void Create()
    {
        Console.Title = "Zoo Tracker";

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("================ Zoo Tracker =================");
        Console.WriteLine("Welcome! You can now track animals in the zoo.");

        while (true)
        {
            // Menu.
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nSelect a number to perform action.");
            Console.WriteLine("1 - Add a new animal");
            Console.WriteLine("2 - List all animals");
            Console.WriteLine("3 - Filter animals by category");
            Console.WriteLine("4 - View animal behaviors");
            Console.WriteLine("5 - Edit an animal");
            Console.WriteLine("6 - Delete an animal");
            Console.WriteLine("7 - Exit");

            // Get user's choice and validates.
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nEnter your choice (1 to 7): ");
            int choice = Validator.GetNumber();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;

            // Apply action based on user choice.
            switch (choice)
            {
                case 1:
                    AddAnimal();
                    break;
                case 2:
                    DisplayAnimals();
                    break;
                case 3:
                    FilterAnimals();
                    break;
                case 4:
                    DisplayBehavior();
                    break;
                case 5:
                    EditAnimal();
                    break;
                case 6:
                    DeleteAnimal();
                    break;
                case 7:
                    Console.ResetColor();
                    Console.WriteLine("Exiting Zoo Tracker...");
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    // Add a new animal to animals list.
    private void AddAnimal()
    {
        Console.WriteLine("------ Add New Animal ------\n");

        // Get a valid name from user.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Enter name: ");
        string name = Validator.GetString();

        // Get a valid species from user.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Enter species (Lion/Giraffe/Elephant/Snake/Crocodile/Penguin/Owl): ");
        string species = Validator.GetString().ToLower();

        AnimalSpecies category = species switch
        {
            "lion" => AnimalSpecies.Lion,
            "giraffe" => AnimalSpecies.Giraffe,
            "elephant" => AnimalSpecies.Elephant,
            "snake" => AnimalSpecies.Snake,
            "crocodile" => AnimalSpecies.Crocodile,
            "penguin" => AnimalSpecies.Penguin,
            "owl" => AnimalSpecies.Owl,
            _ => AnimalSpecies.Empty
        };

        if (category == AnimalSpecies.Empty)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid species.");
            return;
        }

        // Get a valid age from user.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Enter age: ");
        int age = Validator.GetNumber();

        switch (category)
        {
            case AnimalSpecies.Lion:
                animals.Add(new Lion(name, age));
                break;
            case AnimalSpecies.Giraffe:
                animals.Add(new Giraffe(name, age));
                break;
            case AnimalSpecies.Elephant:
                animals.Add(new Elephant(name, age));
                break;
            case AnimalSpecies.Snake:
                animals.Add(new Snake(name, age));
                break;
            case AnimalSpecies.Crocodile:
                animals.Add(new Crocodile(name, age));
                break;
            case AnimalSpecies.Penguin:
                animals.Add(new Penguin(name, age));
                break;
            case AnimalSpecies.Owl:
                animals.Add(new Owl(name, age));
                break;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{name} successfully added.");
    }

    // Check if animals list empty or not.
    private bool isEmpty()
    {
        if (animals.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no animal in the zoo.");
            return true;
        }

        return false;
    }

    // Display all animals in the list.
    private void DisplayAnimals()
    {
        if (isEmpty()) return;

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("------ All Animals ------\n");
        Console.WriteLine($"{"ID",-3} | {"Name",-8} | {"Species",-10} | {"Age",-4} | {"Category",-8}");
        Console.WriteLine("----------------------------------------------");

        foreach (var animal in animals)
            Console.WriteLine($"{animal.ID,-3} | {animal.Name,-8} | {animal.GetType(),-10} | {animal.Age,-4} | {animal.Category,-8}");
    }

    // Filter animals according to category.
    private void FilterAnimals()
    {
        if (isEmpty()) return;

        Console.WriteLine("---- Filter by Category ----\n");

        // Get a valid category from user.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Enter category (mammal/bird/reptile): ");
        string category = Validator.GetString();

        AnimalCategory animalCategory = category switch
        {
            "mammal" => AnimalCategory.Mammal,
            "bird" => AnimalCategory.Bird,
            "reptile" => AnimalCategory.Reptile,
            _ => AnimalCategory.Empty
        };

        if (animalCategory == AnimalCategory.Empty)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid category.");
            return;
        }

        // Created for checking if there is an animal in that category.
        int sameCategory = 0;

        foreach (var animal in animals)
        {
            if (animal.Category == animalCategory)
                sameCategory++;
        }

        if (sameCategory == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No animal found in this category.");
            return;
        }

        // Display animals based on category.
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\n{"ID",-3} | {"Name",-8} | {"Species",-10} | {"Age",-4}");
        Console.WriteLine("----------------------------------");

        foreach (var animal in animals)
        {
            if (animal.Category == animalCategory)
                Console.WriteLine($"{animal.ID,-3} | {animal.Name,-8} | {animal.GetType(),-10} | {animal.Age,-4}");
        }
    }

    // Display selected animal's behavior.
    private void DisplayBehavior()
    {
        if (isEmpty()) return;

        Console.WriteLine("------ Animal Behavior ------\n");
        DisplayAnimals();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nEnter animal ID: ");
        int id = Validator.GetNumber();

        bool isMatchingID = false;

        foreach (var animal in animals)
        {
            if (id == animal.ID)
            {
                isMatchingID = true;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nAnimal: {animal.Name} the {animal.GetType()}");
                Console.WriteLine($"{animal.Name} says: {animal.MakeSound()}");
                Console.WriteLine($"{animal.Name} eats: {animal.Feed()}");

                if (animal is ICanSwim swimmer)
                    swimmer.Swim();

                if (animal is ICanFly flyer)
                    flyer.Fly();
            }
        }

        if (!isMatchingID)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No animal found with this ID.");
        }
    }

    // Edit selected animal's name or age.
    private void EditAnimal()
    {
        if (isEmpty()) return;

        Console.WriteLine("------ Edit Animal ------\n");
        DisplayAnimals();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nEnter animal ID: ");
        int id = Validator.GetNumber();

        bool isMatchingID = false;

        foreach (var animal in animals)
        {
            if (id == animal.ID)
                isMatchingID = true;

        }

        if (!isMatchingID)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No animal found with this ID.");
            return;
        }

        bool isEdited = false;

        // Get a valid new name from user if they want to change it.
        Console.Write("New name (leave blank to keep): ");
        string newName = Validator.GetString();

        if (newName != "")
        {
            for (int animalCount = 0; animalCount < animals.Count; animalCount++)
            {
                if (id == animals[animalCount].ID)
                {
                    animals[animalCount].Name = newName;
                    isEdited = true;
                }
            }
        }

        // Get a valid new age from user if they want to change it.
        Console.Write("New age (enter -1 to keep): ");
        int newAge = Validator.GetNumber();

        if (newAge != -1)
        {
            for (int animalCount = 0; animalCount < animals.Count; animalCount++)
            {
                if (id == animals[animalCount].ID)
                {
                    animals[animalCount].Age = newAge;
                    isEdited |= true;
                }
            }
        }

        if (isEdited)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Animal updated successfully.");
        }
    }

    // Delete selected animal from list.
    private void DeleteAnimal()
    {
        if (isEmpty()) return;

        Console.WriteLine("------ Delete Animal ------\n");
        DisplayAnimals();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nEnter animal ID: ");
        int id = Validator.GetNumber();

        // Created for keep track of animal's index to remove it.
        int animalIndex = -1;

        for (int animalCount = 0; animalCount < animals.Count; animalCount++)
        {
            if (id == animals[animalCount].ID)
                animalIndex = animalCount;
        }

        if (animalIndex != -1)
        {
            animals.RemoveAt(animalIndex);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Animal deleted successfully.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No animal found with this ID.");
        }
    }
}

// ------------ CLASSES ------------

// Base class for all animals.
public abstract class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }
    public int ID { get; private set; }
    public AnimalCategory Category { get; set; }

    // Counter for ID property.
    private static int idCounter = 1;

    public Animal(string name, int age)
    {
        Name = name;
        Age = age;
        ID = idCounter++;
        Category = AnimalCategory.Empty;
    }

    public abstract string MakeSound();
    public abstract string Feed();
}

public class Lion : Animal
{
    public Lion(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Mammal;
    }

    public override string MakeSound() => "Roar!";
    public override string Feed() => "Meat";
}

public class Giraffe : Animal
{
    public Giraffe(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Mammal;
    }

    public override string MakeSound() => "Grunt!";
    public override string Feed() => "Leaves";
}

public class Elephant : Animal, ICanSwim
{
    public Elephant(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Mammal;
    }

    public override string MakeSound() => "Trumpet!";
    public override string Feed() => "Grass and fruits";
    public void Swim() => Console.WriteLine($"{Name} swims");
}

public class Snake : Animal, ICanSwim
{
    public Snake(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Reptile;
    }

    public override string MakeSound() => "Hiss!";
    public override string Feed() => "Warm-blooded prey";
    public void Swim() => Console.WriteLine($"{Name} swims");
}

public class Crocodile : Animal, ICanSwim
{
    public Crocodile(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Reptile;
    }

    public override string MakeSound() => "Growl!";
    public override string Feed() => "Fish and mammals";
    public void Swim() => Console.WriteLine($"{Name} swims");
}

public class Penguin : Animal, ICanSwim
{
    public Penguin(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Bird;
    }

    public override string MakeSound() => "Honk!";
    public override string Feed() => "Fish";
    public void Swim() => Console.WriteLine($"{Name} swims");
}

public class Owl : Animal, ICanFly
{
    public Owl(string name, int age) : base(name, age)
    {
        Category = AnimalCategory.Bird;
    }

    public override string MakeSound() => "Hoot!";
    public override string Feed() => "Small mammals and insects";
    public void Fly() => Console.WriteLine($"{Name} flies");
}

// Validator class to get proper input from user based on input types.
public class Validator
{
    public static int GetNumber()
    {
        string? input = Console.ReadLine()?.Trim();
        bool validInput = int.TryParse(input, out int number);

        while (input == null || !validInput)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Enter a valid number: ");
            input = Console.ReadLine()?.Trim();
            validInput = int.TryParse(input, out number);
        }

        return number;
    }

    public static string GetString()
    {
        string? input = Console.ReadLine()?.Trim();

        while (input == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Try again: ");
            input = Console.ReadLine()?.Trim();
        }

        return input;
    }
}

// -------------- INTERFACES --------------

// Created for animals who can fly.
public interface ICanFly
{
    void Fly();
}

// Created for animals who can swim.
public interface ICanSwim
{
    void Swim();
}

// ---------------- ENUMS ----------------

public enum AnimalSpecies
{
    Lion,
    Giraffe,
    Elephant,
    Snake,
    Crocodile,
    Penguin,
    Owl,
    Empty
}

public enum AnimalCategory
{
    Mammal,
    Reptile,
    Bird,
    Empty
}