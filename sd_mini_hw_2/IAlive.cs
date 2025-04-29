namespace sd_mini_hw_2;

public delegate void TotalFoodChanged(double correction);

public interface IAlive
{
    double Food { get; set; }
    bool IsHealthy { get; set; }
    string Name { get; }
    int Id { get; }
    string? FeedingSchedule { get; set; }
}

public abstract class Animal : IAlive
{
    public event TotalFoodChanged? OnTotalFoodChanged;

    private double _food;
    private const double Tolerance = 0.0001;

    public double Food
    {
        get => _food;
        set
        {
            if (Math.Abs(_food - value) > Tolerance)
            {
                OnTotalFoodChanged?.Invoke(value - _food);
                _food = value;
            }
        }
    }

    public bool IsHealthy { get; set; }
    public string Name { get; }
    public int Id { get; }
    public string? FeedingSchedule { get; set; }
    public string Species { get; }

    public DateOnly Birthday { get; init; }
    public bool Gender { get; init; }
    public string FavouriteFood { get; init; }

    public void Feed(string food) =>
        Console.WriteLine($"{Name} eats {food} (fav = {FavouriteFood})");
    
    private Enclosure? _animalsCage;

    public Enclosure? AnimalsCage
    {
        get => _animalsCage;
        set
        {
            if (_animalsCage != null)
            {
                _animalsCage.IsFree = true;
                --_animalsCage.CurrentNumOfAnimals;
            }

            _animalsCage = value;
            if (_animalsCage != null)
            {
                _animalsCage.IsFree = false;
                ++_animalsCage.CurrentNumOfAnimals;
            }
        }
    }

    public void Cure() => IsHealthy = true;

    protected Animal(
        string name,
        double food,
        bool isHealthy,
        int id,
        Enclosure? animalsCage,
        string? feedingSchedule = null,
        DateOnly? birthday = null,
        bool gender = true,
        string favouriteFood = "")
    {
        Name = name;
        Food = food;
        IsHealthy = isHealthy;
        Id = id;
        FeedingSchedule = feedingSchedule;
        Species = GetType().Name;
        AnimalsCage = animalsCage;

        Birthday = birthday ?? DateOnly.FromDateTime(DateTime.Now);
        Gender = gender;
        FavouriteFood = favouriteFood;
    }
}

public abstract class Herbo(
    string      name,
    double      food,
    bool        isHealthy,
    int         id,
    int         kindness,
    Enclosure?  cage,
    string?     feedingSchedule = null,
    DateOnly?   birthday        = null,
    bool        gender          = true,
    string      favouriteFood   = "")
    : Animal(name, food, isHealthy, id, cage,
        feedingSchedule, birthday, gender, favouriteFood)
{
    public int Kindness { get; private set; } = 
        kindness is >= 0 and <= 10
            ? kindness
            : throw new ArgumentOutOfRangeException(nameof(kindness),
                "Kindness must be between 0 and 10");

    public bool IsKind() => Kindness > 5;
}


public abstract class Predator(
    string name,
    double food,
    bool isHealthy,
    int id,
    Enclosure? cage,
    string? feedingSchedule = null,
    DateOnly? birthday = null,
    bool gender = true,
    string favouriteFood = "")
    : Animal(name, food, isHealthy, id, cage,
        feedingSchedule, birthday, gender, favouriteFood);

public class Monkey(
    string name,
    double food,
    bool isHealthy,
    int id,
    int kindness,
    Enclosure? cage,
    string? feedingSchedule = null,
    DateOnly? birthday = null,
    bool gender = true,
    string favouriteFood = "") : Herbo(name, food, isHealthy, id, kindness, cage, feedingSchedule, birthday, gender,
    favouriteFood);

public class Rabbit(
    string name,
    double food,
    bool isHealthy,
    int id,
    int kindness,
    Enclosure? cage,
    string? feedingSchedule = null,
    DateOnly? birthday = null,
    bool gender = true,
    string favouriteFood = "") : Herbo(name, food, isHealthy, id, kindness, cage, feedingSchedule, birthday, gender,
    favouriteFood);

public class Tiger(
    string name,
    double food,
    bool isHealthy,
    int id,
    Enclosure? cage,
    string? feedingSchedule = null,
    DateOnly? birthday = null,
    bool gender = true,
    string favouriteFood = "")
    : Predator(name, food, isHealthy, id, cage, feedingSchedule, birthday, gender, favouriteFood);

public class Wolf(string name, double food, bool isHealthy, int id, Enclosure? cage, string? feedingSchedule = null, DateOnly? birthday = null, bool gender = true, string favouriteFood = "") : Predator(name, food, isHealthy, id, cage, feedingSchedule, birthday, gender, favouriteFood);