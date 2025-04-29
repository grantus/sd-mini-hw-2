namespace sd_mini_hw_2;

public class FeedingSchedule(Animal animal, DateTime time, FeedingSchedule.FoodTypes foodType)
{
    public Animal Animal { get; } = animal;
    public DateTime Time { get; set; } = time;
    public FoodTypes FoodType { get; set; } = foodType;
    public bool Done { get; set; }

    public void MarkDone()
    {
        Done = true;
    }
    
    public class FoodTypes(string name)
    {
        public string Name { get; set; } = name;
    }
    
    public class Feeding(FoodTypes foodType, TimeOnly time)
    {
        public TimeOnly Time { get; set; } = time;
        public FoodTypes FoodType { get; set; } = foodType;
        public bool Done { get; set; }
    }
}