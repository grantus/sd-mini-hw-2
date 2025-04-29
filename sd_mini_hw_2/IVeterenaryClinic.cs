namespace sd_mini_hw_2;

public interface IVeterinaryClinic
{
    public string Name { get; set; }
    public string Address { get; set; }

    public bool CheckHealth(Animal creature);

    public class VeterinaryClinic(string name, string address) : IVeterinaryClinic
    {
        public string Name { get; set; } = name;
        public string Address { get; set; } = address;

        public bool CheckHealth(Animal creature)
        {
            Console.WriteLine($"Checking health of {creature.Name}...");
            return creature.IsHealthy;
        }

        public void Cure(ref Animal creature)
        {
            creature.Cure();
            Console.WriteLine($"{creature.Name} is successfully cured!");
        }
    }

    public void Cure(Animal creature)
    {
        creature.IsHealthy = true;
        Console.WriteLine($"{creature.Name} is successfully cured!");
    }
}