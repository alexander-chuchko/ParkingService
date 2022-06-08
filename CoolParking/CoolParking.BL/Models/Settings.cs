﻿// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.

// TODO: реализовать настройки класса.
// Детали реализации на ваше усмотрение, они просто должны соответствовать требованиям домашнего задания.

using System.Collections.Generic;

namespace CoolParking.BL
{
    public static class Settings
    {
        public static decimal initialBalanceParking = 0;
        public static int parkingCapacity = 10;
        public static int paymentWriteOffPeriod = 5;
        public static int loggingPeriod = 60;

        public static Dictionary<int, decimal> tariffs = new Dictionary<int, decimal>()
        {
            { (int)VehicleType.PassengerCar, 2m },
            { (int)VehicleType.Truck, 5m },
            { (int)VehicleType.Bus, 3.5m },
            { (int)VehicleType.Motorcycle, 1m },
        };
        public struct TariffsDepending
        {
            public const double Car = 2;
            public const double Bus = 3.5;
            public const double Motorcycle = 1;
        }

        public static double penaltyCoefficient = 2.5;
    }
}