using APBD2.Exceptions;

namespace APBD2.Models;

public class LiquidContainer : ContainerBase, IHazardNotifier
{
    private bool IsDangerous { get; set; }
    public LiquidContainer(double height, double containerMass, double depth, double maxLoadMass, bool isDangerous) : base(height, containerMass, depth, maxLoadMass)
    {
        IsDangerous = isDangerous;
    }

    public override void LoadContainer(double loadMass)
    {
        double maxAllowedLoadMass = IsDangerous ? MaxLoadMass * 0.5 : MaxLoadMass * 0.9;
        if (CurrentLoadMass + loadMass > maxAllowedLoadMass)
        {
            NotifyDanger();
            throw new OverfillException("_currentLoadMass exceeds MaxAllowedLoadMass of this container");
        }
        CurrentLoadMass += loadMass;
    }

    public override void EmptyContainer()
    {
        CurrentLoadMass = 0;
    }

    public override string ToString()
    {
        return base.ToString() + $"| IsDangerous: {IsDangerous}";
    }

    public void NotifyDanger()
    {
        Console.WriteLine($"Detected dangerous operation on container with serial number {_serialNumber}");
    }
}