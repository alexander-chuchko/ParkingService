using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;

namespace CoolParking.BL
{
    public class UserInterface
    {
        private readonly IParkingService _parkingService;
        private readonly int numberMenuItems = 8;
        private string? key;

        public UserInterface(IParkingService parkingService)
        {
            this._parkingService = parkingService;  
        }

        #region ---helpers---
        private void ShowCurrentBalance()
        {
            Console.WriteLine($"\t\tParking balance: {_parkingService.GetBalance()}");
        }

        private void ShowAmountMoneyEarned()
        {
            var transactionsLog = _parkingService.GetLastParkingTransactions();

            if (transactionsLog != null)
            {
                Console.WriteLine($"\t\tAmount for the current period: {transactionsLog.Sum(tr => tr.Sum)}");
            }
            else
            {
                Console.WriteLine($"\t\tAmount for the current period: 0");
            }
        }

        private void ShowNumberFreeAndOccupiedSpaces()
        {
            Console.WriteLine($"\t\tNumber of free - " +
                $"{_parkingService.GetFreePlaces()} / employed -" +
                $" {_parkingService.GetCapacity() - _parkingService.GetFreePlaces()}");
        }

        private void ShowListTrFundsLocated()
        {
            if (_parkingService.GetFreePlaces() < Settings.parkingCapacity)
            {
                int count = default(int);
                Console.WriteLine($"\t\tVehicle list:\n");

                foreach (var item in _parkingService.GetVehicles())
                {
                    Console.WriteLine($"\t\t{++count} - Id:{item.Id} VehicleType:{item.VehicleType} Balance:{item.Balance}");
                }
            }
            else
            {
                Console.WriteLine("\t\tThere are no cars in the parking lot");
            }
        }

        private void ShowTransactionHistory()
        {
            string arrayTransaction = _parkingService.ReadFromLog();

            var transactions = arrayTransaction.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in transactions)
            {
                Console.WriteLine($"{item}");
            }
        }

        private void TopUpBalanceCar()
        {
            Console.WriteLine("\t\tSpecify the index of the vehicle");
            ShowListTrFundsLocated();
            string? id = Console.ReadLine();
            Console.WriteLine("\t\tEnter replenishment amount");
            string? topUpAmount = Console.ReadLine();
            var vehicleses = _parkingService.GetVehicles();

            if (id != null && int.TryParse(id, out int convertIndex) && int.TryParse(topUpAmount, out int convertTopUpAmount) && convertIndex > 0 && convertIndex <= vehicleses.Count)
            {
                _parkingService.TopUpVehicle(vehicleses[convertIndex - 1].Id, convertTopUpAmount);
            }
        }

        private void PickUpVehicle()
        {
            Console.WriteLine("\t\tSpecify the index of the vehicle");
            ShowListTrFundsLocated();
            string? id = Console.ReadLine();

            var vehicleses = _parkingService.GetVehicles();

            if (id != null && int.TryParse(id, out int convertId) && convertId > 0 && convertId <= vehicleses.Count)
            {
                _parkingService.RemoveVehicle(vehicleses[convertId - 1].Id);
            }

        }

        private void PutTrAidForParking()
        {
            var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), VehicleType.Truck, 100);
            _parkingService.AddVehicle(vehicle);
            Console.WriteLine($"\t\tAdded to the parking car - Id:{vehicle.Id} VehicleType:{vehicle.VehicleType} Balance:{vehicle.Balance}");
        }

        private void ClearConsole()
        {
            Console.Clear();
        }

        private void ChangedColor(ConsoleColor consoleColor)
        {
            Console.ForegroundColor=consoleColor;
        }

        private void ShowInfo()
        {
            Console.Clear();
            ChangedColor(ConsoleColor.Red);

            Console.WriteLine("\n\t\t\t\t\tCOOL PARKING");
            ChangedColor(ConsoleColor.Yellow);

            Console.WriteLine("\n\t\tMENU");

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n\t" +
                "1 - Display the current balance of the Parking Lot\n\t" +
                "2 - Display the amount of money earned for the current period (before logging)\n\t" +
                "3 - Display the number of free/occupied parking spaces on the screen\n\t" +
                "4 - Display the list of Tr. funds located in the Parking lot\n\t" +
                "5 - Put Tr. aid for parking\n\t" +
                "6 - Pick up the vehicle from the Parking lot\n\t" +
                "7 - Top up the balance of a specific Tr. funds\n\t" +
                "8 - Display transaction history\n\t");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\t\tSelect the desired item:\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void StartApplication()
        {
            ShowInfo();

            do
            {

                key = Console.ReadLine();

                if (int.TryParse(key, out int number) && number > 0 && number <= numberMenuItems)
                {
                    switch (number)
                    {
                        case 1:
                            ClearConsole();
                            ShowInfo();
                            ShowCurrentBalance();
                            break;
                        case 2:
                            ClearConsole();
                            ShowInfo();
                            ShowAmountMoneyEarned();
                            break;

                        case 3:
                            ClearConsole();
                            ShowInfo();
                            ShowNumberFreeAndOccupiedSpaces();
                            break;

                        case 4:
                            ClearConsole();
                            ShowInfo();
                            ShowListTrFundsLocated();
                            break;

                        case 5:
                            ClearConsole();
                            ShowInfo();
                            PutTrAidForParking();
                            break;

                        case 6:
                            ClearConsole();
                            ShowInfo();
                            PickUpVehicle();
                            break;

                        case 7:
                            ClearConsole();
                            ShowInfo();
                            PutTrAidForParking();
                            TopUpBalanceCar();
                            break;

                        case 8:
                            ClearConsole();
                            ShowInfo();
                            ShowTransactionHistory();
                            break;

                        default:
                            Console.WriteLine("Invalid value specified!");
                            break;
                    }
                }
                else if (key != "e")
                {
                    Console.WriteLine("Invalid value specified!");
                }

                ChangedColor(ConsoleColor.Red);
                Console.WriteLine("\n\tEXIT THE APPLICATION - 'e'\n");
                ChangedColor(ConsoleColor.White);

            } while (key != "e");
        }

        #endregion
    }
}
