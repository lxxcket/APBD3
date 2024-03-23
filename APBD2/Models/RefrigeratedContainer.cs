using APBD2.Exceptions;

namespace APBD2.Models;

public class RefrigeratedContainer : ContainerBase
{
    protected string ProductType {get; set; }
    protected double ContainerTemperature { get; set; }
    private static Dictionary<string, double> ProductAllowedTemperature;

    static RefrigeratedContainer()
    {
        ProductAllowedTemperature = new Dictionary<string, double>();
        ProductAllowedTemperature.Add("Bananas", 13.3);
        ProductAllowedTemperature.Add("Chocolate", 18);
        ProductAllowedTemperature.Add("Fish", 2);
        ProductAllowedTemperature.Add("Meat", -15);
        ProductAllowedTemperature.Add("Ice cream", -18);
        ProductAllowedTemperature.Add("Frozen pizza", -30);
        ProductAllowedTemperature.Add("Cheese", 7.2);
        ProductAllowedTemperature.Add("Sausages", 5);
        ProductAllowedTemperature.Add("Butter", 20.5);
        ProductAllowedTemperature.Add("Eggs", 19);
        
    }
    public RefrigeratedContainer(double height, double containerMass, double depth, double maxLoadMass, string productType, double containerTemperature) : base(height, containerMass, depth, maxLoadMass)
    {
        if (ProductAllowedTemperature[productType] > containerTemperature)
        {
            throw new NotAllowedTemperatureOfContainerException(
                "Container temperature can't be lower than temperature that needed to store provided product type");
        }

        ProductType = productType;
        ContainerTemperature = containerTemperature;
    }

    public void LoadContainerWithProducts(double loadMass, string productType)
    {
        if (productType.ToLower() != ProductType.ToLower())
            throw new NotAllowedProductException(
                $"This product could not be placed in this container because this container can only be used to store: {ProductType}");
        LoadContainer(loadMass);
    }

    public override void LoadContainer(double loadMass)
    {
        if (CurrentLoadMass + loadMass > MaxLoadMass)
            throw new OverfillException("LoadMass exceeds _maxLoadMass for this container");

        CurrentLoadMass += loadMass;
    }

    public override void EmptyContainer()
    {
        CurrentLoadMass = 0;
    }
    
    public override string ToString()
    {
        return base.ToString() + $"| Product type: {ProductType} | Temperature of container: {ContainerTemperature}";
    }
}