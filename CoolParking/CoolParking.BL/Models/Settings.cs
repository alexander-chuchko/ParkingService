// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.


namespace CoolParking.BL.Models
{
    public static class Settings
    {
        public static decimal initialBalanceParking = 0;
        public static int parkingCapacity = 10;
        public static int paymentWriteOffPeriod = 5;
        public static int loggingPeriod = 60;
        public static int coefficient = 1000;

        public static Dictionary<int, decimal> tariffs = new Dictionary<int, decimal>()
        {
            { (int)VehicleType.PassengerCar, 2m },
            { (int)VehicleType.Truck, 5m },
            { (int)VehicleType.Bus, 3.5m },
            { (int)VehicleType.Motorcycle, 1m },
        };

        public static decimal penaltyCoefficient = 2.5m;
    }
}