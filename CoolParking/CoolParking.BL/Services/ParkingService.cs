// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.


using CoolParking.BL.Helpers;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Collections.ObjectModel;
using System.Timers;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ITimerService _withdrawTimer;
        private readonly ITimerService _logTimer;
        private readonly ILogService _logService;
        private Parking _Parking { get; set; }
        private TransactionInfo[] TransactionInfos { get; set; }


        public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
        {
            _Parking = Parking.GetInstance();
            _Parking.Vehicles = new List<Vehicle>(Settings.parkingCapacity);
            this._withdrawTimer = withdrawTimer;
            this._logTimer = logTimer;
            this._logService = logService;
            this._withdrawTimer.Elapsed += OnWithdraw_Funds;
            this._logTimer.Elapsed += OnLog_Record;
        }

        public void AddVehicle(Vehicle vehicle)
        {

            if (_Parking.Vehicles.Count == Settings.parkingCapacity)
            {
                throw new InvalidOperationException("There are no spaces in the parking lot");
            }

            if (_Parking.Vehicles.Count != 0 && Validation.CompareStrings(vehicle.Id, _Parking.Vehicles))
            {
                throw new ArgumentException("Invalid identifier entered");
            }

            int key = (int)vehicle.VehicleType;

            if (vehicle.Balance >= Settings.tariffs[key])
            {
                _Parking.Vehicles.Add(vehicle);

                StartOrStopTimer(_Parking.Vehicles); 
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Not enough funds in the account"); 
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Dispose()
        {
            _Parking.DisposeInstance();
        }

        public decimal GetBalance()
        {
            return _Parking.Balance;
        }

        public int GetCapacity()
        {
            return _Parking.Vehicles.Capacity;
        }

        public int GetFreePlaces()
        {
            return _Parking.Vehicles.Capacity - _Parking.Vehicles.Count;
        }

        public TransactionInfo[] GetLastParkingTransactions()
        {
            return TransactionInfos;
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            var collection = _Parking.Vehicles.AsReadOnly();
            return collection;
        }

        public string ReadFromLog()
        {
            return _logService.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            var vehicle = _Parking.Vehicles.Find(tr => tr.Id == vehicleId);

            if (vehicle != null)
            {
                if (vehicle.Balance < Settings.initialBalanceParking)
                {
                    throw new InvalidOperationException("Your balance is negative");
                }
                else
                {
                    _Parking.Vehicles.Remove(vehicle);

                    StartOrStopTimer(_Parking.Vehicles);
                }
            }
            else
            {
                throw new ArgumentException("This number does not exist");
            }
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            var vehicle = _Parking.Vehicles.Find(tr => tr.Id == vehicleId);

            if (vehicle != null && sum > Settings.initialBalanceParking)
            {
                vehicle.Balance += sum;
            }
            else
            {
                throw new ArgumentException("This number does not exist");
            }
        }

        private void OnWithdraw_Funds(object? sender, ElapsedEventArgs e) 
        {
            if (_Parking.Vehicles.Count != 0)
            {
                int count = 0;

                if (TransactionInfos == null)
                {
                    TransactionInfos = new TransactionInfo[_Parking.Vehicles.Count];
                }
                else
                {
                    Resize(TransactionInfos.Length + _Parking.Vehicles.Count);
                    count = TransactionInfos.Length - _Parking.Vehicles.Count;
                }

                foreach (var vehicles in _Parking.Vehicles)
                {
                    decimal tariff = Settings.tariffs[(int)vehicles.VehicleType];
                    decimal sumFine = 0;

                    if(vehicles.Balance < 0)
                    {
                        sumFine = tariff * Settings.penaltyCoefficient;

                        vehicles.Balance -= sumFine;
                        _Parking.Balance += sumFine;
                    }
                    else if(vehicles.Balance < tariff)
                    {
                        sumFine = vehicles.Balance + ((tariff - vehicles.Balance) * Settings.penaltyCoefficient);

                        vehicles.Balance -= sumFine;
                        _Parking.Balance += sumFine;
                    }
                    else if(vehicles.Balance >= tariff)
                    {
                        sumFine = tariff;
                        vehicles.Balance -= sumFine;
                        _Parking.Balance += sumFine;
                    }


                    TransactionInfos[count] = new TransactionInfo
                    {
                        VehicleId = vehicles.Id,
                        TransactionTime = DateTime.Now.ToString("hh:mm:ss"),
                        Sum = sumFine
                    };

                    count++;
                }
            }
        }

        private void Resize(int size)
        {
            TransactionInfo[] newArray = new TransactionInfo[size];

            for (int i = 0; i < TransactionInfos.Length; i++)
            {
                newArray[i] = TransactionInfos[i];
            }

            TransactionInfos = newArray;
        }

        private void OnLog_Record(object? sender, ElapsedEventArgs e)
        {
            string? transactions = string.Empty;

            if (TransactionInfos != null)
            {
                foreach (var transaction in TransactionInfos)
                {
                    if(transaction != null)
                    {
                        transactions += $"Id:{transaction.VehicleId} Date:{transaction.TransactionTime} Sum:{transaction.Sum}\r";
                    }
                }
            }

            _logService.Write(transactions);

            TransactionInfos = null;
        }

        private void StartOrStopTimer(IEnumerable<Vehicle> vehicles)
        {
            if (vehicles.Count() == 1)
            {
                _Parking.StartTime = DateTime.Now;
                _withdrawTimer.Interval = Settings.paymentWriteOffPeriod * Settings.coefficient;
                _withdrawTimer.Start();
                _logTimer.Interval = Settings.loggingPeriod * Settings.coefficient;
                _logTimer.Start();
            }
            else if(vehicles.Count() == 0)
            {
                _Parking.StartTime = null;
                _withdrawTimer.Stop();
                _logTimer.Stop();
            }
        }

    }
}
