namespace sd_mini_hw_2;

public interface IInventory
{
    public string Name { get; set; }
    public int Id { get; set; }
}
public abstract class Thing(string name) : IInventory
{
    public string Name { get; set; } = name;
    public int Id { get; set; }

    protected Thing() : this("Unnamed") { }
}


public class Table : Thing
{
    private static int _counter;

    public Table(string name) : base(name)
    {
        Id = _counter++;
    }
}

public class Computer : Thing
{
    private static int _counter;

    public Computer(string name) : base(name)
    {
        Id = _counter++;
    }
}

public class Enclosure : Thing
{
    private static int _counter;

    public bool IsFree;
    public int Capacity { get; init; }
    public int CurrentNumOfAnimals = 0;

    public Enclosure() : base("Unnamed Enclosure")
    {
        Id = _counter++;
        IsFree = true;
    }
    public Enclosure(string name) : base(name)
    {
        Id = _counter++;
        IsFree = true;
    }
}