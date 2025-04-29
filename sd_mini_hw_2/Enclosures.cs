namespace sd_mini_hw_2;

public class Enclosures
{
    private readonly List<Enclosure> _cages = [];

    public void AddCage()
    {
        var cage = new Enclosure();
        _cages.Add(cage);
        Console.WriteLine("Cage added");
    }

    public void RemoveCage(Enclosure enclosure)
    {
        if (_cages.Contains(enclosure))
        {
            _cages.Remove(enclosure);
            Console.WriteLine($"Cage №{enclosure.Id} is removed from the zoo.");
        }
        else
        {
            Console.WriteLine(
                $"Cage №{enclosure.Id} was not deleted from the zoo due since it wasn't there.");
        }
    }

    public int GetNumOfFreeCages()
    {
        return _cages.Count(cage => cage.IsFree);
    }
}