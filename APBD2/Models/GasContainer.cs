using APBD2.Exceptions;

namespace APBD2.Models;

public class GasContainer : ContainerBase, IHazardNotifier
{
    protected double Pressure { get; set; }

    public GasContainer(double height, double containerMass, double depth, double maxLoadMass, double pressure) 
        : base(height, containerMass, depth, maxLoadMass)
    {
        Pressure = pressure;
    }
    
    
    public override void LoadContainer(double loadMass)
    {
        if (loadMass > MaxLoadMass)
            throw new OverfillException("LoadMass exceeds _maxLoadMass for this container");

        if (loadMass + CurrentLoadMass > MaxLoadMass)
        {
            NotifyDanger();
            return;
        }

        CurrentLoadMass += loadMass;
    }

    public override void EmptyContainer()
    {
        CurrentLoadMass *= 0.05;
    }

    public void NotifyDanger()
    {
        Console.WriteLine($"Detected dangerous operation on container with serial number {_serialNumber}, total LoadMass would be greater that _maxLoadMass.");
    }

    public override string ToString()
    {
        return base.ToString() + $"| Pressure: {Pressure} atm";
    }
}