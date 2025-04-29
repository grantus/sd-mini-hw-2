namespace sd_mini_hw_2;

public static class DomainEvents
{
    public static event Action<AnimalMovedEvent>? AnimalMoved;
    public static event Action<FeedingTimeEvent>? FeedingTime;

    internal static void Raise(AnimalMovedEvent ev) => AnimalMoved?.Invoke(ev);
    internal static void Raise(FeedingTimeEvent ev) => FeedingTime?.Invoke(ev);
}

public class AnimalMovedEvent(Animal animal, Enclosure oldEnc, Enclosure newEnc)
{
    public Animal Animal { get; } = animal;
    public Enclosure OldEnclosure { get; } = oldEnc;
    public Enclosure NewEnclosure { get; } = newEnc;
    public DateTime MovedAt { get; } = DateTime.Now;
}

public sealed class AnimalTransferService
{
    public void Move(Animal a, Enclosure to)
    {
        var from = a.AnimalsCage;
        if (to.CurrentNumOfAnimals >= to.Capacity)
            throw new InvalidOperationException("Enclosure is full");

        a.AnimalsCage = to;

        DomainEvents.Raise(new AnimalMovedEvent(a, from!, to));
    }
}


public class FeedingTimeEvent(Animal animal, FeedingSchedule schedule)
{
    public Animal Animal { get; } = animal;
    public FeedingSchedule Schedule { get; } = schedule;
    public DateTime FiredAt { get; } = DateTime.Now;
}

public sealed class FeedingOrganizationService
{
    public static void AddSchedule(
        InMemoryZooRepository repo,
        Animal a,
        DateTime when,
        FeedingSchedule.FoodTypes food)
    {
        if (repo.FeedingSchedules.Any(f => f.Animal == a && f.Time == when))
            throw new InvalidOperationException("Duplicate schedule");

        repo.FeedingSchedules.Add(new FeedingSchedule(a, when, food));
    }

    public static void Tick(DateTime now, InMemoryZooRepository repo)
    {
        foreach (var s in repo.FeedingSchedules.Where(s => !s.Done && s.Time <= now))
        {
            s.MarkDone();
            DomainEvents.Raise(new FeedingTimeEvent(s.Animal, s));
        }
    }
}

public class ZooStatisticsService
{
    public static int CountAnimals(InMemoryZooRepository repo)
    {
        return repo.Animals.Count;
    }

    public static int CountFreeEnclosures(InMemoryZooRepository repo)
    {
        return repo.Enclosures.Count(enc => enc.IsFree);
    }

    public static int CountFeedingSchedules(InMemoryZooRepository repo)
    {
        return repo.FeedingSchedules.Count;
    }
}

public class InMemoryZooRepository
{
    public readonly List<Animal> Animals = [];
    public readonly List<Enclosure> Enclosures = [];
    public readonly List<FeedingSchedule> FeedingSchedules = [];
}
