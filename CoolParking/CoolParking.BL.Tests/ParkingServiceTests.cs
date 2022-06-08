using CoolParking.BL.Interfaces;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoolParking.BL.Tests
{
    public class ParkingServiceTests : IDisposable
    {
        readonly ParkingService _parkingService;
        readonly FakeTimerService _withdrawTimer;
        readonly FakeTimerService _logTimer;
        readonly ILogService _logService;

        public ParkingServiceTests()
        {
            _withdrawTimer = new FakeTimerService();
            _logTimer = new FakeTimerService();
            _logService = A.Fake<ILogService>();
            _parkingService = new ParkingService(_withdrawTimer, _logTimer, _logService);
        }
        public void Dispose()
        {
            _parkingService.Dispose();
        }

       
        [Fact]
        public void Parking_IsSingelton()
        {
            var newParkingService = new ParkingService(_withdrawTimer, _logTimer, _logService);
            var vehicle = new Vehicle("AA-0001-AA", VehicleType.Truck, 100);
            _parkingService.AddVehicle(vehicle);

            Assert.Single(newParkingService.GetVehicles());
            Assert.Single(_parkingService.GetVehicles());
            Assert.Same(_parkingService.GetVehicles()[0], newParkingService.GetVehicles()[0]);
        }

        [Fact]
        public void GetCapacity_WhenEmpty_Then10()
        {
            Assert.Equal(10, _parkingService.GetCapacity());
        }

        [Fact]
        public void GetFreePlaces_WhenEmpty_Then10()
        {
            Assert.Equal(10, _parkingService.GetFreePlaces());
        }

        [Fact]
        public void AddVehicle_WhenNewVehicle_ThenVehiclesPlusOne()
        {

            var vehicle = new Vehicle("AA-0001-JA", VehicleType.Bus, 120);

            _parkingService.AddVehicle(vehicle);
            Assert.Single(_parkingService.GetVehicles());
        }

        [Fact]
        public void AddVehicle_WhenExistingVehicleId_ThenThrowArgumentException()
        {
            var vehicle1 = new Vehicle("AA-0001-AA", VehicleType.Bus, 100);
            var vehicle2 = new Vehicle(vehicle1.Id, VehicleType.Motorcycle, 200);
            _parkingService.AddVehicle(vehicle1);

            Assert.Throws<ArgumentException>(() => _parkingService.AddVehicle(vehicle2));
        }

        [Theory]
        [InlineData("AA 0001", VehicleType.Bus, 100)]
        [InlineData("AA-0001-AA", VehicleType.Bus, -100)]
        public void NewVehicle_WhenWrongArguments_ThenThrowArgumentException(string id, VehicleType vehicleType, decimal balance)
        {
            Assert.Throws<ArgumentException>(() => new Vehicle(id, vehicleType, balance));
        }


    }
}
