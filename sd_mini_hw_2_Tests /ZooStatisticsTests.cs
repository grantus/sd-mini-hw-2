using sd_mini_hw_2;
using Xunit;

namespace sd_mini_hw_2_Tests;

public class ZooStatisticsTests
{
    [Fact]
    public void StatsCountsAreCorrect()
    {
        var repo = new InMemoryZooRepository();

        var enc = new Enclosure("Test") { Capacity = 2 };
        repo.Enclosures.Add(enc);

        var a = new Rabbit("R", 1, true, 1, 6, enc);
        repo.Animals.Add(a);

        Assert.Equal(1, ZooStatisticsService.CountAnimals(repo));
        Assert.Equal(0, ZooStatisticsService.CountFreeEnclosures(repo));
    }

    [Fact]
    public void CountsAreCorrect_ForInitialState()
    {
        var repo = new InMemoryZooRepository();

        Assert.Equal(0, ZooStatisticsService.CountAnimals(repo));
        Assert.Equal(0, ZooStatisticsService.CountFreeEnclosures(repo));
        Assert.Equal(0, ZooStatisticsService.CountFeedingSchedules(repo));
    }

    [Fact]
    public void CountsReflectAddedAnimals_AndEnclosureOccupancy()
    {
        var repo = new InMemoryZooRepository();

        var enc1 = new Enclosure("Savanna") { Capacity = 1 };
        var enc2 = new Enclosure("Forest") { Capacity = 2 };
        repo.Enclosures.AddRange([enc1, enc2]);

        Assert.Equal(2, ZooStatisticsService.CountFreeEnclosures(repo));

        var tiger = new Tiger("T", 5, true, 1, enc1);
        repo.Animals.Add(tiger);

        Assert.Equal(1, ZooStatisticsService.CountAnimals(repo));
        Assert.Equal(1, ZooStatisticsService.CountFreeEnclosures(repo));
    }

    [Fact]
    public void FeedingScheduleCountIncrements_WhenScheduleAdded()
    {
        var repo = new InMemoryZooRepository();

        var enc = new Enclosure("Cage") { Capacity = 3 };
        repo.Enclosures.Add(enc);
        var rabbit = new Rabbit("R", 1, true, 7, 6, enc);
        repo.Animals.Add(rabbit);

        FeedingOrganizationService.AddSchedule(repo, rabbit, DateTime.Now.AddHours(1),
            new FeedingSchedule.FoodTypes("Carrot"));

        Assert.Equal(1, ZooStatisticsService.CountFeedingSchedules(repo));

        FeedingOrganizationService.AddSchedule(repo, rabbit, DateTime.Now.AddHours(2),
            new FeedingSchedule.FoodTypes("Apple"));
        Assert.Equal(2, ZooStatisticsService.CountFeedingSchedules(repo));
    }
}

public class ExtendedDomainTests
{
    [Fact]
    public void Cure_SetsIsHealthyTrue()
    {
        var cage = new Enclosure("Tmp") { Capacity = 1 };
        var tiger = new Tiger("BadCat", 5, false, 1, cage);
        Assert.False(tiger.IsHealthy);

        tiger.Cure();
        Assert.True(tiger.IsHealthy);
    }

    [Fact]
    public void ZooApi_PostsAnimalAndEnclosure()
    {
        var repo = new InMemoryZooRepository();
        var api = new ZooApi(repo);

        var enc = new Enclosure("E1") { Capacity = 2 };
        api.PostEnclosure(enc);
        Assert.Single(repo.Enclosures);

        var rabbit = new Rabbit("R", 1.1, true, 1, 7, enc);
        api.PostAnimal(rabbit);
        Assert.Single(repo.Animals);
        Assert.Equal(enc, rabbit.AnimalsCage);
    }
    [Fact]
    public void Animal_Birthday_And_Gender_DefaultsAreReasonable()
    {
        var cage = new Enclosure("Tmp") { Capacity = 1 };
        var birthday = new DateOnly(2015, 2, 1);
        var monkey = new Monkey("M", 2, true, 1, 6, cage, birthday: birthday, gender: false);
        Assert.Equal(birthday, monkey.Birthday);
        Assert.False(monkey.Gender);
    }

    [Fact]
    public void Zoo_AddAnimal_UpdatesTotalFood()
    {
        var clinic = new IVeterinaryClinic.VeterinaryClinic("VC", "here");
        var zoo = new Zoo(clinic);
        var enc = new Enclosure("C") { Capacity = 2 };
        var tiger = new Tiger("T", 4, true, 1, enc);

        zoo.AddAnimal(tiger);
        Assert.Equal(4, zoo.GetTotalFood());
    }

    public class ZooAdditionalTests
    {
        private sealed class StubClinic : IVeterinaryClinic
        {
            public string Name { get; set; } = "Stub";
            public string Address { get; set; } = "Nowhere";
            public bool CheckHealth(Animal _) => true;
        }

        [Fact]
        public void Zoo_GetNumOfAnimals_Returns_Correct_Count()
        {
            var zoo = new Zoo(new StubClinic());
            var enc  = new Enclosure("Test") { Capacity = 5 };

            var r1 = new Rabbit("R1", 1, true, 1, 6, enc);
            var r2 = new Rabbit("R2", 1, true, 2, 6, enc);

            zoo.AddAnimal(r1);
            zoo.AddAnimal(r2);
            var count = zoo.GetNumOfAnimals();

            Assert.Equal(2, count);
        }

        [Fact]
        public void Animal_Cure_Sets_IsHealthy_To_True()
        {
            var enc  = new Enclosure("Tmp") { Capacity = 1 };
            var tiger = new Tiger("T", 5, false, 99, enc);

            Assert.False(tiger.IsHealthy);
            tiger.Cure();
            Assert.True(tiger.IsHealthy);
        }

        [Fact]
        public void Enclosures_GetNumOfFreeCages_Works()
        {
            var enclosures = new Enclosures();

            enclosures.AddCage();
            enclosures.AddCage();
            enclosures.AddCage();

            Assert.Equal(3, enclosures.GetNumOfFreeCages());
        }
    }
}