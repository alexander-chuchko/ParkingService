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

using CoolParking.BL.Helpers;
using CoolParking.BL.Interfaces;
using System.Collections.ObjectModel;
using System.Timers;

namespace CoolParking.BL
{
    public class ParkingService : IParkingService
    {
        private readonly ITimerService _withdrawTimer;
        private readonly ITimerService _logTimer;
        private readonly ILogService _logService;
        private Parking _Parking { get; set; }
        private TransactionInfo[]? TransactionInfos { get; set; }


        public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
        {
            _Parking = Parking.GetInstance();
            _Parking.Vehicles = new List<Vehicle>(Settings.parkingCapacity);
            this._withdrawTimer = withdrawTimer;
            this._logTimer = logTimer;
            this._logService = logService;
            this._withdrawTimer.Elapsed += OnTransactionsCompleted;
            this._logTimer.Elapsed += OnLogPaymentRecorded;
        }

        public void AddVehicle(Vehicle vehicle)
        {

            if (_Parking.Vehicles.Count == Settings.parkingCapacity)
            {
                throw new InvalidOperationException();
            }

            if (_Parking.Vehicles.Count != 0 && Validation.CompareStrings(vehicle.Id, _Parking.Vehicles))
            {
                throw new ArgumentException();
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
                    throw new InvalidOperationException();
                }
                else
                {
                    _Parking.Vehicles.Remove(vehicle);

                    StartOrStopTimer(_Parking.Vehicles);
                }
            }
            else
            {
                throw new ArgumentException();
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
                throw new ArgumentException();
            }
        }

        private void OnTransactionsCompleted(object? sender, ElapsedEventArgs e) 
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
                    var newArray = new TransactionInfo[TransactionInfos.Length + _Parking.Vehicles.Count];
                    Array.Copy(TransactionInfos, newArray, TransactionInfos.Length);
                    TransactionInfos = newArray;
                    count = TransactionInfos.Length - _Parking.Vehicles.Count;
                }

                foreach (var vehicles in _Parking.Vehicles)
                {
                    decimal sum = Settings.tariffs[(int)vehicles.VehicleType];

                    if(vehicles.Balance < 0)
                    {
                        var sumFine = sum * Settings.penaltyCoefficient;

                        vehicles.Balance -= sumFine;
                        _Parking.Balance += sumFine;
                    }
                    else if(vehicles.Balance < sum)
                    {
                        var sumFine = vehicles.Balance - (vehicles.Balance + ((sum - vehicles.Balance) * Settings.penaltyCoefficient));

                        vehicles.Balance -= sumFine;
                        _Parking.Balance += sumFine;
                    }
                    else if(vehicles.Balance >= sum)
                    {
                        vehicles.Balance -= sum;
                        _Parking.Balance += sum;
                    }


                    TransactionInfos[count] = new TransactionInfo
                    {
                        VehicleId = vehicles.Id,
                        TransactionTime = DateTime.Now.ToString(),
                        Sum = sum
                    };

                    count++;
                }
            }

        }

        private void OnLogPaymentRecorded(object? sender, ElapsedEventArgs e)
        {
            string transactions=string.Empty;

            if (_Parking.StartTime!= null)
            {
                if (TransactionInfos != null && TransactionInfos.Length > 0)
                {
                    foreach (var transaction in TransactionInfos)
                    {
                        transactions += $"Id:{transaction.VehicleId} Date:{transaction.TransactionTime} Sum:{transaction.Sum}";
                    }
                }

                TransactionInfos = null;
            }

            _logService.Write(transactions);
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

        private void ReSizeArray(ref TransactionInfo[] array, int size)
        {
            var newArray = new TransactionInfo[size];
            Array.Copy(array, newArray, array.Length);
            TransactionInfos = newArray;
        }

    }
}
