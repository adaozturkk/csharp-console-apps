public class Habit
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> CompletedDates { get; set; } = [];

    public bool IsCompletedToday
    {
        get
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            return CompletedDates.Contains(today);
        }
    }

    public Habit(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
