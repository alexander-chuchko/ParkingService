// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.

// TODO: реализовать класс ParkingService из интерфейса IParkingService.
// При попытке добавить машину на полную парковку должно быть выброшено исключение InvalidOperationException.
// При попытке удаления автомобиля с отрицательным балансом (долгом) должно быть выброшено InvalidOperationException.
// Другие правила проверки и формат конструктора пошли из тестов.
// Другие детали реализации на ваше усмотрение, они просто должны соответствовать требованиям интерфейса
// и тесты, например, в ParkingServiceTests можно найти нужный формат конструктора и правила проверки.

using CoolParking.BL.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace CoolParking.BL
{
    public class ParkingService : IParkingService
    {
        private readonly ITimerService _withdrawTimer;
        private readonly ITimerService _logTimer;
        private readonly ILogService _logService;
        private Parking Parking { get; set; }
        private TransactionInfo[] TransactionInfos { get; set; }
        public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
        {
            Parking = Parking.GetInstance();
            this._withdrawTimer = withdrawTimer;
            this._logTimer = logTimer;
            this._logService = logService;
        }
        public void AddVehicle(Vehicle vehicle)
        {
            try
            {
                if (Parking.Vehicles.Count == Settings.parkingCapacity)
                {
                    throw new InvalidOperationException();
                }

                int key = (int)vehicle.VehicleType;

                if (vehicle.Balance >= Settings.tariffs[key])
                {
                    Parking.Vehicles.Add(vehicle);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Not enough funds in the account"); //
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch(InvalidOperationException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There are no empty spaces in the parking lot"); //
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Dispose()
        {
            Console.WriteLine($"{Parking} has been deleted");
        }

        public decimal GetBalance()
        {
            return Parking.Balance;
        }

        public int GetCapacity()
        {
            return Parking.Vehicles.Capacity;
        }

        public int GetFreePlaces()
        {
            return Parking.Vehicles.Capacity - Parking.Vehicles.Count;
        }

        public TransactionInfo[] GetLastParkingTransactions()
        {
            throw new System.NotImplementedException();
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return new ReadOnlyCollection<Vehicle>(Parking.Vehicles);
        }

        public string ReadFromLog()
        {
            return _logService.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            //Проверить id на корректность
            var vehicle = Parking.Vehicles.Find(tr => tr.Id == vehicleId);

            if (vehicle != null)
            {
                if (vehicle.Balance < Settings.initialBalanceParking)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    Parking.Vehicles.Remove(vehicle);
                }
            } 
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            if(sum > Settings.initialBalanceParking)
            {
                var vehicle = Parking.Vehicles.Find(tr => tr.Id == vehicleId);

                if (vehicle != null)
                {
                    vehicle.Balance += sum;
                }
            }
            else
            {
                Console.WriteLine("sum must be > 0!!!");
            }
        }
    }
}
