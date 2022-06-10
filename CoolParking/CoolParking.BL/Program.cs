// See https://aka.ms/new-console-template for more information




using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.BL.Services;
using System.Reflection;



int numberMenuItems = 8;
Console.WriteLine("\n\t\t\t\t\tCoolParking");
Console.WriteLine("\n\tTo work with text, the application provides the following methods:");
Console.WriteLine("\n\t" +
    "1 - Display the current balance of the Parking Lot\n\t" +
    "2 - Display the amount of money earned for the current period (before logging).\n\t" +
    "3 - Display the number of free/occupied parking spaces on the screen.\n\t" +
    "4 - Display the list of Tr. funds located in the Parking lot.\n\t" +
    "5 - Put Tr. aid for parking\n\t" +
    "6 - Pick up the vehicle from the Parking lot.\n\t" +
    "7 - Top up the balance of a specific Tr. funds.\n\t" +
    "8 - Display transaction history (by reading data from the Transactions.log file).\n\t");

Console.WriteLine("\n\tTo start the application, specify the method index:");

string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\transactions.log";
TimerService withdrawTimer = new TimerService();
TimerService logTimer = new TimerService();
LogService logService = new LogService(_logFilePath);


ParkingService parkingService = new ParkingService(withdrawTimer, logTimer, logService);

void ShowCurrentBalance(IParkingService parkingService)
{
    Console.WriteLine($"\t\tParking balance: {parkingService.GetBalance()}");
}

void ShowAmountMoneyEarned(IParkingService parkingService)
{
    var transactionsLog = parkingService.GetLastParkingTransactions();

    if (transactionsLog != null)
    {
        Console.WriteLine($"\t\tAmount for the current period: {transactionsLog.Sum(tr => tr.Sum)}");
    }
    else
    {
        Console.WriteLine($"Amount for the current period: 0");
    }
}

void ShowNumberFreeAndOccupiedSpaces(IParkingService parkingService)
{
    Console.WriteLine($"\t\tNumber of free - " +
        $"{parkingService.GetFreePlaces()} / employed -" +
        $" {parkingService.GetCapacity() - parkingService.GetFreePlaces()}");
}

void ShowListTrFundsLocated(IParkingService parkingService)
{
    if (parkingService.GetFreePlaces() < Settings.parkingCapacity)
    {
        int count = default(int);
        Console.WriteLine($"\t\tVehicle list:\n");

        foreach (var item in parkingService.GetVehicles())
        {
            Console.WriteLine($"\t\t{++count} - Id:{item.Id}; VehicleType:{item.VehicleType}; Balance:{item.Balance}");
        }
    }
    else
    {
        Console.WriteLine("There are no cars in the parking lot");
    }
}

void ShowTransactionHistory(IParkingService parkingService)
{
    string arrayTransaction = parkingService.ReadFromLog();

    var transactions = arrayTransaction.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);

    foreach (var item in transactions)
    {
        Console.WriteLine($"\t{item}");
    }
}

void TopUpBalanceCar(IParkingService parkingService)
{
    Console.WriteLine("Specify the index of the vehicle");
    ShowListTrFundsLocated(parkingService);
    string? id = Console.ReadLine();
    Console.WriteLine("Enter replenishment amount");
    string? topUpAmount = Console.ReadLine();
    var vehicleses = parkingService.GetVehicles();

    if (id != null && int.TryParse(id, out int convertIndex) && int.TryParse(topUpAmount, out int convertTopUpAmount) && convertIndex > 0 && convertIndex <= vehicleses.Count)
    {
        parkingService.TopUpVehicle(vehicleses[convertIndex - 1].Id, convertTopUpAmount);
    }
}

void PickUpVehicle(IParkingService parkingService)
{
    Console.WriteLine("Specify the index of the vehicle");
    ShowListTrFundsLocated(parkingService);
    string? id = Console.ReadLine();

    var vehicleses = parkingService.GetVehicles();

    if (id != null && int.TryParse(id, out int convertId) && convertId > 0 && convertId <= vehicleses.Count)
    {
        parkingService.RemoveVehicle(vehicleses[convertId - 1].Id);
    }

}

void PutTrAidForParking(IParkingService parkingService)
{
    var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), VehicleType.Truck, 23);
    parkingService.AddVehicle(vehicle);
}


String? key;

do
{
    key = Console.ReadLine();

    if (int.TryParse(key, out int number) && number > 0 && number <= numberMenuItems)
    {
        switch (number)
        {
            case 1:
                ShowCurrentBalance(parkingService);

                break;
            case 2:

                ShowAmountMoneyEarned(parkingService);

                break;
            case 3:

                ShowNumberFreeAndOccupiedSpaces(parkingService);

                break;
            case 4:

                ShowListTrFundsLocated(parkingService);

                break;

            case 5:

                PutTrAidForParking(parkingService);

                break;
            case 6:

                PickUpVehicle(parkingService);

                break;
            case 7:

                PutTrAidForParking(parkingService);
                TopUpBalanceCar(parkingService);

                break;

            case 8:

                ShowTransactionHistory(parkingService);

                break;

            default:
                Console.WriteLine("Invalid value specified!");
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid value specified!");
    }
    Console.WriteLine("\n\tEnter item number\n\tExit the application - 'e'\n");


} while (key != "e");

Console.ReadKey(true);



