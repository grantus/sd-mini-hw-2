namespace sd_mini_hw_2;

public class Zoo(IVeterinaryClinic clinic)
{
    private readonly List<Animal> _animals = [];
    private double _totalFood;
    private double _numOfAnimals;

    public double GetNumOfAnimals()
    {
        return _numOfAnimals;
    }
    public void AddAnimal(Animal animal)
    {
        if (clinic.CheckHealth(animal))
        {
            _animals.Add(animal);
            if (animal is Herbo herboAnimal)
            {
                if (herboAnimal.IsKind())
                {
                    _animalsForContactZoo.Add(herboAnimal);
                }
            }
            _totalFood += animal.Food;
            
            animal.OnTotalFoodChanged += OnAnimalFoodChanged;
            
            Console.WriteLine($"{animal.Name} is added to the zoo!");
            ++_numOfAnimals;
        }
        else
        {
            Console.WriteLine(
                $"{animal.Name} is not added to the zoo due to health condition. It needs medical attention");
        }
    }
    
    public void RemoveAnimal(Animal animal)
    {
        if(_animals.Contains(animal))
        {
            _animals.Remove(animal);
            if (animal is Herbo herboAnimal)
            {
                _animalsForContactZoo.Remove(herboAnimal);
            }
            _totalFood -= animal.Food;
            
            animal.OnTotalFoodChanged += OnAnimalFoodChanged;
            
            Console.WriteLine($"{animal.Name} is removed from the zoo.");
            --_numOfAnimals;
        }
        else
        {
            Console.WriteLine(
                $"{animal.Name} is not deleted from the zoo due since it wasn't there.");
        }
    }

    public void GetAnimals()
    {
        Console.WriteLine("\nThe animals:");

        _animals.Sort((a, b) => string.Compare(a.Species, b.Species, StringComparison.Ordinal));
        var prev = "";
        foreach (var animal in _animals)
        {
            if (animal.Species != prev)
            {
                Console.WriteLine($"{animal.Species}s:");
                prev = animal.Species;
            }

            Console.WriteLine($"    {animal.Name}, food per day: {animal.Food} kg");
        }
        Console.WriteLine();
    }
    
    public void UpdateAnimalFood(Animal animal, double newFoodAmount)
    {
        Console.WriteLine($"{animal.Name} is now requiring {newFoodAmount} kg of food per day (formerly {animal.Food} kg).");
        animal.Food = newFoodAmount;
    }

    public double GetTotalFood()
    {
        Console.WriteLine($"{_totalFood} is the total food");
        return _totalFood;
    }

    public List<Herbo> GetAnimalsForContactZoo()
    {
        Console.WriteLine("Animals in contact zoo:");
        foreach (var animal in _animalsForContactZoo)
        {
            Console.WriteLine($"    {animal.Name}, kindness: {animal.Kindness}");
        }
        Console.WriteLine();
        return _animalsForContactZoo;
    }
    
    private void OnAnimalFoodChanged(double correction)
    {
        _totalFood += correction;
    }
    
    private readonly List<Herbo> _animalsForContactZoo = [];
}