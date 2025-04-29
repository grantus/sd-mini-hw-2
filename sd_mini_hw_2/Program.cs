namespace sd_mini_hw_2;

public static class Program
{
    public static void Main()
    {
        var repo = new InMemoryZooRepository();

        var feedingService = new FeedingOrganizationService();
        var statsService = new ZooStatisticsService();

        var animals = new AnimalsController(repo);
        var encl = new EnclosuresController(repo);
        var feed = new FeedingController(repo, feedingService);
        var stat = new StatisticsController(repo, statsService);

        var enclosure1 = new Enclosure("Cage #1") { Capacity = 2 };
        var enclosure2 = new Enclosure("Cage #2") { Capacity = 3 };
        encl.AddEnclosure(enclosure1);
        encl.AddEnclosure(enclosure2);

        var tiger = new Tiger("Tiger", 5.0, true, 1, enclosure1, "9AM & 6PM");
        var rabbit = new Rabbit("Bunny", 1.2, true, 2, 8, enclosure1, "8AM");
        var monkey = new Monkey("Monkey", 2.5, true, 3, 7, enclosure2, "10AM");

        animals.AddAnimal(tiger);
        animals.AddAnimal(rabbit);
        animals.AddAnimal(monkey);

        stat.ShowStats();
        encl.ListEnclosures();

        feed.AddFeeding(tiger, DateTime.Now.AddSeconds(2), new FeedingSchedule.FoodTypes("Meat"));
        feed.AddFeeding(rabbit, DateTime.Now.AddSeconds(5), new FeedingSchedule.FoodTypes("Carrots"));
        feed.ListFeedings();
        
        DomainEvents.AnimalMoved += ev =>
            Console.WriteLine($"[EVENT] {ev.Animal.Name} moved to {ev.NewEnclosure.Name} ({ev.MovedAt:t})");

        DomainEvents.FeedingTime += ev =>
            Console.WriteLine($"[EVENT] Feeding time for {ev.Animal.Name} ({ev.FiredAt:t})");
        
        encl.ListEnclosures();

        Thread.Sleep(3000);
        feed.ProcessDueFeedings();
        feed.ListFeedings();

        stat.ShowStats();
    }
}