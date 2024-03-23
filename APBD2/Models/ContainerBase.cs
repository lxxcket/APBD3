namespace APBD2.Models;

public abstract class ContainerBase
{
    public string _serialNumber { get; set; }

    public string SerialNumber
    {
        get { return _serialNumber; }
    }
    private double _currentLoadMass { get; set; }

    public double CurrentLoadMass
    {
        get { return _currentLoadMass; }
        protected set { _currentLoadMass = value; }
    }
    private double _height { get; set; }

    public double Height
    {
        get { return _height; }
    }
    
    private double _containerMass { get; set; }

    public double ContainerMass
    {
        get { return _containerMass; }
    }
    private double _depth { get; set; }

    public double Depth
    {
        get { return _depth; }
    }
    private double _maxLoadMass { get; set; }

    public double MaxLoadMass
    {
        get { return _maxLoadMass; }
    }

    private static Dictionary<string, int> serialNumberMap;

    static ContainerBase()
    {
        serialNumberMap = new Dictionary<string, int>();
        serialNumberMap.Add("LiquidContainer",1);
        serialNumberMap.Add("GasContainer",1);
        serialNumberMap.Add("RefrigeratedContainer",1);
    }

    public ContainerBase(double height, double containerMass, double depth, double maxLoadMass)
    {
        _height = height;
        _containerMass = containerMass;
        _depth = depth;
        _maxLoadMass = maxLoadMass;
        GetSerialNumber();
    }

    private void GetSerialNumber()
    {
        switch (GetType().Name)
        {
            case "LiquidContainer":
                _serialNumber = $"KON-L-{serialNumberMap[GetType().Name]++}";
                break;
            case "GasContainer":
                _serialNumber = $"KON-G-{serialNumberMap[GetType().Name]++}";
                break;
            case "RefrigeratedContainer":
                _serialNumber = $"KON-C-{serialNumberMap[GetType().Name]++}";
                break;
        }
        
    }
    
    
    public abstract void LoadContainer(double LoadMass);

    public abstract void EmptyContainer();

    public override string ToString()
    {
        return $"Serial Number: {_serialNumber} | Current load mass: {_currentLoadMass} | Height: {_height} | ContainerMass: {_containerMass} | Depth: {_depth} " ;
    }
    
}