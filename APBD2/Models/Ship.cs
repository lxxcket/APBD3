using APBD2.Exceptions;
using APBD2.Models;

namespace APBD2;

public class Ship
{
    private List<ContainerBase> _containers { get; set; }
    public List<ContainerBase> Containers => _containers;
    
    private string _shipName { get; set; }
    public string ShipName => _shipName;

    private int _maxSpeed { get; set; }
    public int MaxSpeed => _maxSpeed;
    
    private int _maxContainerQuantity { get; set; }

    public int MaxContainerQuantity => _maxContainerQuantity;
    
    private int _maxWeight { get; set; }
    public int MaxWeight => _maxWeight;

    public Ship(string name, int maxSpeed, int maxContainerQuantity, int maxWeight)
    {
        _containers = new List<ContainerBase>();
        _shipName = name;
        _maxSpeed = maxSpeed;
        _maxContainerQuantity = maxContainerQuantity;
        _maxWeight = maxWeight;
    }

    public void AddContainer(ContainerBase container)
    {
        if (_containers.Count < _maxContainerQuantity &&
            (CalculateTotalWeight() + container.ContainerMass + container.CurrentLoadMass) <= MaxWeight)
            _containers.Add(container);
        else
            throw new ShipCapacityException("Cannot add container, ship capacity exceeded");
    }

    public void RemoveContainer(ContainerBase container)
    {
        _containers.Remove(container);
    }

    public double CalculateTotalWeight()
    {
        double totalWeight = 0;
        foreach (var container in _containers)
        {
            totalWeight += container.CurrentLoadMass;
            totalWeight += container.ContainerMass;
        }

        return totalWeight;
    }

    public override string ToString()
    {
        return $"Ship name: {ShipName} | MaxSpeed: {MaxSpeed} | MaxContainerQuantity: {MaxContainerQuantity} | MaxWeight: {MaxWeight}";
    }
    public void ReplaceContainer(string containerToRemoveSerial, ContainerBase newContainer)
    {
        var containerToRemove = Containers.FirstOrDefault(c => c.SerialNumber == containerToRemoveSerial);
        if (containerToRemove != null)
        {
            int index = Containers.IndexOf(containerToRemove);
            Containers[index] = newContainer;
            Console.WriteLine($"Kontener o numerze seryjnym {containerToRemoveSerial} został zastąpiony nowym kontenerem.");
        }
        else
        {
            Console.WriteLine($"Nie znaleziono kontenera o numerze seryjnym {containerToRemoveSerial} na statku.");
        }
    }
}