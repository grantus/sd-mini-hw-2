namespace sd_mini_hw_2;

public sealed class ZooApi
{
    private readonly AnimalsController      _animals;
    private readonly EnclosuresController   _enclosures;
    private readonly FeedingController      _feeding;
    private readonly StatisticsController   _stats;

    public ZooApi(InMemoryZooRepository repo)
    {
        var feedingSvc  = new FeedingOrganizationService();
        _animals    = new AnimalsController(repo);
        _enclosures = new EnclosuresController(repo);
        _feeding    = new FeedingController(repo, feedingSvc);
        _stats      = new StatisticsController(repo, new ZooStatisticsService());
    }

   public void PostAnimal(Animal a)              => _animals.AddAnimal(a);
    public void DeleteAnimal(Animal a)            => _animals.RemoveAnimal(a);

    public void PostEnclosure(Enclosure e)        => _enclosures.AddEnclosure(e);
    public void DeleteEnclosure(Enclosure e)      => _enclosures.RemoveEnclosure(e);

    public void PatchMoveAnimal(Animal a, Enclosure dst) =>
        new AnimalTransferService().Move(a, dst);

    public void GetFeeding()                      => _feeding.ListFeedings();
    public void PostFeeding(Animal a, DateTime t, string food) =>
        _feeding.AddFeeding(a, t, new FeedingSchedule.FoodTypes(food));

    public void GetStats()                        => _stats.ShowStats();
}

public class AnimalsController(InMemoryZooRepository repo)
{
    public void AddAnimal(Animal animal)
    {
        repo.Animals.Add(animal);
        Console.WriteLine($"{animal.Name} added to zoo. Enclosure: {animal.AnimalsCage?.Name}");
    }

    public void RemoveAnimal(Animal animal)
    {
        if (repo.Animals.Remove(animal))
        {
            animal.AnimalsCage = null; 
            Console.WriteLine($"{animal.Name} removed from zoo");
        }
        else
        {
            Console.WriteLine($"{animal.Name} not found in zoo");
        }
    }

    public void ListAnimals()
    {
        Console.WriteLine("Animals in zoo:");
        foreach (var a in repo.Animals)
            Console.WriteLine($"  {a.Species} {a.Name}, in enclosure: {a.AnimalsCage?.Name}, food: {a.Food}");
    }
}

public class EnclosuresController(InMemoryZooRepository repo)
{
    public void AddEnclosure(Enclosure enc)
    {
        repo.Enclosures.Add(enc);
        Console.WriteLine($"Enclosure {enc.Name} added (Id = {enc.Id})");
    }

    public void RemoveEnclosure(Enclosure enc)
    {
        Console.WriteLine(
            repo.Enclosures.Remove(enc) ? $"Enclosure {enc.Name} removed" : "Enclosure not found in repo");
    }

    public void ListEnclosures()
    {
        Console.WriteLine("Enclosures in zoo:");
        foreach (var e in repo.Enclosures)
        {
            Console.WriteLine($"  [#{e.Id}] {e.Name}, free? {e.IsFree}, currentNum={e.CurrentNumOfAnimals}");
        }
    }
}

public class FeedingController(
    InMemoryZooRepository repo,
    FeedingOrganizationService service)
{
    public void AddFeeding(Animal animal,
        DateTime feedTime,
        FeedingSchedule.FoodTypes food)
    {
        FeedingOrganizationService.AddSchedule(repo, animal, feedTime, food);
        Console.WriteLine($"[Schedule] {animal.Name} — {food.Name} @ {feedTime}");
    }

    public void ProcessDueFeedings()
    {
        FeedingOrganizationService.Tick(DateTime.Now, repo);
    }

    public void ListFeedings()
    {
        Console.WriteLine("Feeding schedules:");
        foreach (var f in repo.FeedingSchedules)
            Console.WriteLine(
                $" • {f.Animal.Name,-10} : {f.Time} [{f.FoodType.Name}]  Done = {f.Done}");
    }
}

public class StatisticsController(InMemoryZooRepository repo, ZooStatisticsService stats)
{
    public void ShowStats()
    {
        Console.WriteLine($"Number of animals: {ZooStatisticsService.CountAnimals(repo)}");
        Console.WriteLine($"Free Enclosures: {ZooStatisticsService.CountFreeEnclosures(repo)}");
        Console.WriteLine($"Feeding Schedules total: {ZooStatisticsService.CountFeedingSchedules(repo)}");
    }
}
