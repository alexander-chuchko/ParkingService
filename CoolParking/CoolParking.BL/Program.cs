// See https://aka.ms/new-console-template for more information



using CoolParking.BL;
using System.Reflection;
using System.Text;


//Количество методов предоставленных пользователю
int numberOfMethods = 8;
Console.WriteLine("\n\t\t\t\t\tApplication for working with CoolParking");
Console.WriteLine("\n\tTo work with text, the application provides the following methods:");
Console.WriteLine("\n\t" +
    "1 - Вывести на экран текущий баланс Парковки.\n\t" +
    "2 - Вывести на экран сумму заработанных денег за текущий период (до записи в лог);\n\t" +
    "3 - Вывести на экран количество свободных/занятых мест на парковке.\n\t" +
    "4 - Вывести на экран список Тр. средств находящихся на Паркинге.\n\t" +
    "5 - Поставить Тр. средство на Паркинг.\n\t" +
    "6 - Забрать транспортное средство с Паркинга.\n\t" +
    "7 - Пополнить баланс конкретного Тр. средства.\n\t" +
    "8 - Вывести историю транзакций (считав данные из файла Transactions.log).\n\t");

Console.WriteLine("\n\tTo start the application, specify the method index:");

string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\transactions.log";
TimerService withdrawTimer = new TimerService();
TimerService logTimer = new TimerService();
LogService logService = new LogService(_logFilePath);


ParkingService parkingService = new ParkingService(withdrawTimer, logTimer, logService);


ConsoleKeyInfo? keypress;
do
{
    keypress = null;

    string? str = Console.ReadLine();

    if (int.TryParse(str, out int number) && number > 0 && number <= numberOfMethods)
    {
        switch (number)
        {
            case 1:
                Console.WriteLine($"\n\nParking balance: {parkingService.GetBalance()}"); 
                break;
            case 2:
                var sums = parkingService.GetLastParkingTransactions();

                if(sums!=null)
                {
                    Console.WriteLine($"Amount for the current period: {sums.Sum(tr => tr.Sum)}");
                }
                else
                {
                    Console.WriteLine($"Amount for the current period: 0");
                }
                break;
            case 3:
                Console.WriteLine($"Number of free: {parkingService.GetFreePlaces()}/employed:{parkingService.GetCapacity()- parkingService.GetFreePlaces()}");
                break;
            case 4:
                Console.WriteLine($"Vehicle list:\n\n");

                if (parkingService.GetFreePlaces() < Settings.parkingCapacity)
                {
                    foreach (var item in parkingService.GetVehicles())
                    {
                        Console.WriteLine($"Id: {item.Id} {item.VehicleType}\n");
                    }
                }
                else
                {
                    Console.WriteLine("0");
                }

                break;

            case 5:

                var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), VehicleType.Truck, 100);
                parkingService.AddVehicle(vehicle);
                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

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
    Console.WriteLine("\n\tTo call the next method, press 'Enter', to exit the application press the key 'e'\n");

    keypress = Console.ReadKey();

} while (keypress?.KeyChar != 'e');

Console.ReadKey(true);



