// See https://aka.ms/new-console-template for more information



using CoolParking.BL;
using System.Reflection;
using System.Text;


Console.WriteLine("Hello word");
ParkingService parkingService = new ParkingService(new TimerService(), new TimerService(), new LogService($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\transactions.log"));
var vehicle2 = new Vehicle("AA-0002-AA", VehicleType.Bus, 100);
parkingService.AddVehicle(vehicle2);

Console.ReadLine();
//string? transactions = null;
//var newArray = new TransactionInfo[]
//{
//                new TransactionInfo {VehicleId="AA-2554-HG", TransactionTime="09:02:55", Sum=5 },
//                new TransactionInfo {VehicleId="AD-2024-HG", TransactionTime="10:02:55", Sum=3 },
//                new TransactionInfo {VehicleId="AV-2984-HG", TransactionTime="11:15:55", Sum=4 },
//                new TransactionInfo {VehicleId="AW-2784-HG", TransactionTime="12:26:55", Sum=4 },
//                new TransactionInfo {VehicleId="AK-2444-HG", TransactionTime="01:02:55", Sum=5 },
//};

//foreach (var transaction in newArray)
//{
//    transactions += $"Id:{transaction.VehicleId}\nDate:{transaction.TransactionTime}\nSum:{transaction.Sum}\n\n";
//}
//string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";

//File.WriteAllText(_logFilePath, transactions);

//File.AppendAllText(_logFilePath, logInfo);

//var res1 = File.ReadAllText(_logFilePath, Encoding.Default);
//string[] allTransactions = res1.Split(new string[] { "\n" }, 3, StringSplitOptions.RemoveEmptyEntries);

//var res = File.ReadLines(_logFilePath);

//foreach (var item in allTransactions)
//{
//    Console.WriteLine(item);
//}

//Console.WriteLine("Hello, World!");


//class MyClass
//{
//    public int MyProperty { get; set; }
//    public int MyProperty1 { get; set; }
//}