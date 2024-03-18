namespace APBD2.Models;

public abstract class ContainerBase
{
    protected abstract void LoadContainer(double LoadMass);

    protected abstract void EmptyContainer();
}