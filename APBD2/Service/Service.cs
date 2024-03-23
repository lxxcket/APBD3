using APBD2.Exceptions;
using APBD2.Models;

namespace APBD2.Service;

public class Service
{
    static List<Ship> ships = new List<Ship>();
    static List<ContainerBase> containers = new List<ContainerBase>();

    public static void Run()
    {
        bool exit = false;

        do
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Stworzenie statka");
            Console.WriteLine("2. Stworzenie kontenera");
            Console.WriteLine("3. Załadowanie ładunku do kontenera");
            Console.WriteLine("4. Załadowanie kontenera na statek");
            Console.WriteLine("5. Załadowanie listy kontenerów na statek");
            Console.WriteLine("6. Usunięcie kontenera ze statku");
            Console.WriteLine("7. Rozładowanie kontenera");
            Console.WriteLine("8. Zastąpienie kontenera na statku");
            Console.WriteLine("9. Przeniesienie kontenera między dwoma statkami");
            Console.WriteLine("10. Wypisanie informacji o kontenerze");
            Console.WriteLine("11. Wypisanie informacji o statku i jego ładunku");
            Console.WriteLine("12. Wyjście");

            Console.Write("Wybierz opcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateShip();
                    break;
                case "2":
                    CreateContainer();
                    break;
                case "3":
                    LoadCargoToContainer();
                    break;
                case "4":
                    LoadContainerToShip();
                    break;
                case "5":
                    LoadContainerListToShip();
                    break;
                case "6":
                    RemoveContainerFromShip();
                    break;
                case "7":
                    UnloadCargoFromContainer();
                    break;
                case "8":
                    ReplaceContainerOnShip();
                    break;
                case "9":
                    MoveContainerBetweenShips();
                    break;
                case "10":
                    PrintContainerInfo();
                    break;
                case "11":
                    PrintShipInfo();
                    break;
                case "12":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Niepoprawny wybór. Wybierz ponownie.");
                    break;
            }

            Console.WriteLine();
        }
        while (!exit);
    }

    private static void CreateShip()
    {
        Console.WriteLine("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();

        Console.WriteLine("Podaj maksymalną prędkość statku: ");
        int maxSpeed = int.Parse(Console.ReadLine());

        Console.WriteLine("Podaj maksymalną ilość kontenerów statku: ");
        int maxContainerQuantity = int.Parse(Console.ReadLine());

        Console.WriteLine("Podaj maksymalną wagę statku: ");
        int maxWeight = int.Parse(Console.ReadLine());

        Ship ship = new Ship(shipName, maxSpeed, maxContainerQuantity, maxWeight);
        ships.Add(ship);

        Console.WriteLine("Statek został utworzony.");
    }

    static void CreateContainer()
    {
        Console.WriteLine("Podaj rodzaj kontenera (Liquid, Gas, Refrigerated): ");
        string containerType = Console.ReadLine();

        Console.WriteLine("Podaj wysokość kontenera");
        double height = double.Parse(Console.ReadLine()); 

        Console.WriteLine("Podaj wagę kontenera: ");
        double containerMass = double.Parse(Console.ReadLine());
        
        Console.WriteLine("Podaj głebokość kontenera: ");
        double depth = double.Parse(Console.ReadLine());
        
        Console.WriteLine("Podaj maksymalną masę ładunku");
        double maxLoadMass = double.Parse(Console.ReadLine());
        

        ContainerBase container = null;
        switch (containerType.ToLower())
        {
            case "liquid":
                Console.WriteLine("Czy jest to niebiezpieczny ładunek? Y/N");
                string choice = Console.ReadLine();
                bool isDangerous = false;
                switch (choice)
                {
                    case "Y":
                        isDangerous = true;
                        break;
                    case "N":
                        isDangerous = false;
                        break;
                }

                container = new LiquidContainer(height, containerMass, depth, maxLoadMass, isDangerous);
                break;
            case "gas":
                Console.WriteLine("Podaj ciśnienie w kontenerze (w atmosferach): ");
                double pressure = double.Parse(Console.ReadLine());
                container = new GasContainer(height, containerMass, depth, maxLoadMass, pressure);
                break;
            case "refrigerated":
                Console.WriteLine("Podaj rodzaj produktu: ");
                string productType = Console.ReadLine();
                Console.WriteLine("Podaj temperaturę kontenera: ");
                double temperature = double.Parse(Console.ReadLine());
                try
                {
                    container = new RefrigeratedContainer(height, containerMass, depth, maxLoadMass, productType, temperature);
                }
                catch (NotAllowedTemperatureOfContainerException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Niepoprawny kontener, stwórz ponownie");
                    CreateContainer(); 
                    return; 
                }

                break;
            default:
                Console.WriteLine("Niepoprawny rodzaj kontenera.");
                return;
        }

        containers.Add(container);
        Console.WriteLine("Kontener został utworzony.");
    }

    static void LoadCargoToContainer()
    {
        Console.WriteLine("Wybierz kontener: ");
        var container = ChooseContainer();

        Console.WriteLine("Podaj wagę ładunku: ");
        double cargoWeight = double.Parse(Console.ReadLine());

        
        if (container != null)
        {
            try
            {
                container.LoadContainer(cargoWeight);
                Console.WriteLine("Ładunek został załadowany do kontenera.");
            }
            catch (OverfillException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono kontenera o podanym numerze seryjnym.");
        }
    }

    static void LoadContainerToShip()
    {
        Console.WriteLine("Wybierz statek: ");

        var ship = ChooseShip();
        if (ship != null)
        {
            Console.WriteLine("Wybierz kontener: ");


            var container = ChooseContainer();
            if (container != null)
            {
                try
                {
                    ship.AddContainer(container);
                    Console.WriteLine("Kontener został załadowany na statek.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono kontenera o podanym numerze seryjnym.");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono statku o podanej nazwie.");
        }
    }
    static void LoadContainerListToShip()
    {
        Console.WriteLine("Wybierz statek: ");

        var ship = ChooseShip();
        if (ship != null)
        {
            Console.WriteLine("Podaj ilość kontenerów do załadowania: ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Dostępne kontenery: ");
                foreach (var container in containers)
                {
                    Console.WriteLine($"Numer seryjny: {container._serialNumber}");
                }
                Console.WriteLine($"Podaj numer seryjny {i + 1} kontenera: ");
                string serialNumber = Console.ReadLine();

                var containerChoosed = containers.FirstOrDefault(c => c._serialNumber == serialNumber);
                if (containerChoosed != null)
                {
                    try
                    {
                        ship.AddContainer(containerChoosed);
                        Console.WriteLine($"Kontener {serialNumber} został załadowany na statek.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Nie znaleziono kontenera o podanym numerze seryjnym: {serialNumber}");
                }
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono statku o podanej nazwie.");
        }
    }

    static void RemoveContainerFromShip()
    {
        Console.WriteLine("Wybierz statek: ");
        string shipName = Console.ReadLine();

        var ship = ChooseShip();
        if (ship != null)
        {
            Console.WriteLine("Wybierz kontener do usunięcia: ");

            var container = ChooseContainer();
            if (container != null)
            {
                ship.RemoveContainer(container);
                Console.WriteLine("Kontener został usunięty ze statku.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono kontenera na statku o podanym numerze seryjnym.");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono statku o podanej nazwie.");
        }
    }
    static void UnloadCargoFromContainer()
    {
        Console.WriteLine("Wybierz kontener: ");

        var container = ChooseContainer();
        if (container != null)
        {
            container.EmptyContainer();
            Console.WriteLine("Ładunek został rozładowany z kontenera.");
        }
        else
        {
            Console.WriteLine("Nie znaleziono kontenera o podanym numerze seryjnym.");
        }
    }
    static void ReplaceContainerOnShip()
    {
        Console.WriteLine("Wybierz statek: ");
        
        var ship = ChooseShip();
        if (ship != null)
        {
            Console.WriteLine("Dostępne kontenery do zastąpienia:");
            foreach (var container in ship.Containers)
            {
                Console.WriteLine($"Numer seryjny: {container._serialNumber}");
            }
            Console.WriteLine("Podaj numer seryjny kontenera do zastąpienia: ");
            string containerToRemoveSerial = Console.ReadLine();

            Console.WriteLine("Dostępne kontenery: ");
            foreach (var container in containers)
            {
                Console.WriteLine($"Numer seryjny: {container._serialNumber}");
            }
            Console.WriteLine("Podaj numer seryjny nowego kontenera: ");
            string newContainerSerial = Console.ReadLine();

            var containerToRemove = ship.Containers.FirstOrDefault(c => c._serialNumber == containerToRemoveSerial);
            var newContainer = containers.FirstOrDefault(c => c._serialNumber == newContainerSerial);
            if (containerToRemove != null && newContainer != null)
            {
                Console.WriteLine("Kontener został zastąpiony na statku.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono kontenera do zastąpienia lub nowego kontenera.");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono statku o podanej nazwie.");
        }
    }
    static void MoveContainerBetweenShips()
    {
        Ship sourceShip = ChooseShip();
        Ship destinationShip = ChooseShip();

        if (sourceShip != null && destinationShip != null)
        {
            Console.WriteLine("Dostępne kontenery do przeniesienia:");
            foreach (var container in sourceShip.Containers)
            {
                Console.WriteLine($"Numer seryjny: {container._serialNumber}");
            }

            Console.Write("Podaj numer seryjny kontenera do przeniesienia: ");
            string containerSerial = Console.ReadLine();

            var containerChoosed = sourceShip.Containers.FirstOrDefault(c => c._serialNumber == containerSerial);
            if (containerChoosed != null)
            {
                try
                {
                    sourceShip.RemoveContainer(containerChoosed);
                    destinationShip.AddContainer(containerChoosed);
                    Console.WriteLine($"Kontener {containerSerial} został przeniesiony z {sourceShip.ShipName} do {destinationShip.ShipName}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Nie znaleziono kontenera o numerze seryjnym {containerSerial} na statku {sourceShip.ShipName}.");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono jednego z podanych statków.");
        }
    }
    static void PrintContainerInfo()
    {
        var container = ChooseContainer();
        if (container != null)
        {
            Console.WriteLine(container);
        }
        else
        {
            Console.WriteLine("Nie znaleziono kontenera o podanym numerze seryjnym.");
        }
    }
    static void PrintShipInfo()
    {

        var ship = ChooseShip();
        if (ship != null)
        {
            Console.WriteLine(ship);
            Console.WriteLine($"Aktualna waga wszystkich kontenerów: {ship.CalculateTotalWeight()}");
        }
        else
        {
            Console.WriteLine("Nie znaleziono statku o podanej nazwie.");
        }
    }
    static ContainerBase ChooseContainer()
    {
        Console.WriteLine("Dostępne kontenery:");
        for (int i = 0; i < containers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Numer seryjny: {containers[i]._serialNumber}");
        }

        Console.Write("Wybierz numer kontenera: ");
        int choice = int.Parse(Console.ReadLine()) - 1;

        if (choice >= 0 && choice < containers.Count)
        {
            return containers[choice];
        }
        else
        {
            Console.WriteLine("Niepoprawny numer kontenera.");
            return null;
        }
    }
    static Ship ChooseShip()
    {
        Console.WriteLine("Dostępne statki:");
        for (int i = 0; i < ships.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ships[i].ShipName}");
        }

        Console.Write("Wybierz numer statku: ");
        int choice = int.Parse(Console.ReadLine()) - 1;

        if (choice >= 0 && choice < ships.Count)
        {
            return ships[choice];
        }
        else
        {
            Console.WriteLine("Niepoprawny numer statku.");
            return null;
        }
    }
    static Ship ChooseShipWithContainers()
    {
        Console.WriteLine("Dostępne statki:");
        for (int i = 0; i < ships.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ships[i].ShipName}");
        }

        Console.Write("Wybierz numer statku: ");
        int choice = int.Parse(Console.ReadLine()) - 1;

        if (choice >= 0 && choice < ships.Count)
        {
            Ship selectedShip = ships[choice];

            Console.WriteLine($"Dostępne kontenery na statku {selectedShip.ShipName}:");
            foreach (var container in selectedShip.Containers)
            {
                Console.WriteLine($"Numer seryjny: {container._serialNumber}");
            }

            Console.Write("Wybierz numer kontenera: ");
            string containerSerial = Console.ReadLine();

            var selectedContainer = selectedShip.Containers.FirstOrDefault(c => c._serialNumber == containerSerial);
            if (selectedContainer != null)
            {
                return selectedShip;
            }
            else
            {
                Console.WriteLine($"Nie znaleziono kontenera o numerze seryjnym {containerSerial} na wybranym statku.");
                return null;
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny numer statku.");
            return null;
        }
    }
}