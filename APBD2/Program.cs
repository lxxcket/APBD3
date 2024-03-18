// See https://aka.ms/new-console-template for more information

using System.Xml;
using APBD2.Models;

class Program
{
    static void Main(string[] args)
    {
        FluidContainer fluidContainer = new FluidContainer(650, 50.0, 20.0, 30.0);
        FluidContainer fluidContainer2 = new FluidContainer(540, 50, 30, 30);
        GasContainer gas = new GasContainer(50, 20, 30, 30);
        GasContainer ga1s = new GasContainer();
        
    }
}