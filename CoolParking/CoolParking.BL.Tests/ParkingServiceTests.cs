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
    }
}
