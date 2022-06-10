using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;

namespace CoolParking.BL
{
    public class Navigation
    {
        private readonly IParkingService _parkingService;
        private int numberMenuItems = 8;
        private string? key;
        public Navigation(IParkingService parkingService)
        {
            this._parkingService = parkingService;  
        }

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
                    Console.WriteLine($"\t\t{++count} - Id:{item.Id}; VehicleType:{item.VehicleType}; Balance:{item.Balance}");
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
                Console.WriteLine($"\t\t{item}");
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
            var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), VehicleType.Truck, 23);
            _parkingService.AddVehicle(vehicle);
        }

        private void ShowInfo()
        {
            Console.WriteLine("\n\t\t\t\t\tCoolParking");
            Console.WriteLine("\n\t\tTo work with text, the application provides the following methods:");
            Console.WriteLine("\n\t" +
                "1 - Display the current balance of the Parking Lot\n\t" +
                "2 - Display the amount of money earned for the current period (before logging).\n\t" +
                "3 - Display the number of free/occupied parking spaces on the screen.\n\t" +
                "4 - Display the list of Tr. funds located in the Parking lot.\n\t" +
                "5 - Put Tr. aid for parking\n\t" +
                "6 - Pick up the vehicle from the Parking lot.\n\t" +
                "7 - Top up the balance of a specific Tr. funds.\n\t" +
                "8 - Display transaction history (by reading data from the Transactions.log file).\n\t");

            Console.WriteLine("\n\t\tTo start the application, specify the method index:");
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
                            ShowCurrentBalance();
                            break;
                        case 2:
                            ShowAmountMoneyEarned();
                            break;

                        case 3:
                            ShowNumberFreeAndOccupiedSpaces();
                            break;

                        case 4:
                            ShowListTrFundsLocated();
                            break;

                        case 5:
                            PutTrAidForParking();
                            break;

                        case 6:
                            PickUpVehicle();
                            break;

                        case 7:
                            PutTrAidForParking();
                            TopUpBalanceCar();
                            break;

                        case 8:
                            ShowTransactionHistory();
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
        }
    }
}
