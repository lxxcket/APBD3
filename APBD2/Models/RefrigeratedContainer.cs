using APBD2.Exceptions;

namespace APBD2.Models;

public class RefrigeratedContainer
{
    private static int _serialNumberCounter = 0;
    private string SerialNumber { get; set; }
    private double LoadMass { get; set; }
    private double Height { get; set; }
    private double ContainerMass { get; set; }
    private double Depth { get; set; }
    private readonly double MaxLoadMass = 1000.0;

    public RefrigeratedContainer(double loadMass, double height, double containerMass, double depth)
    {
        LoadMass = loadMass;
        Height = height;
        ContainerMass = containerMass;
        Depth = depth;

        _serialNumberCounter++;
        SerialNumber = $"CON-C-{_serialNumberCounter}";

        try
        {
            if (LoadMass > MaxLoadMass)
                throw new OverfillException();
        }
        catch (OverfillException e)
        {
            Console.WriteLine("LOAD MASS CAN'T BE BIGGER THAN MAXLOADMASS WHICH IS: " + MaxLoadMass);
        }
    }

    public override string ToString()
    {
        return "Container type: " + this.GetType().Name + ", SerialNumber:  " + SerialNumber + ", LoadMass: " +  LoadMass+ ", Height: " + Height + ", ContainerMass: " + ContainerMass + ", Depth: " + Depth;
    }
}