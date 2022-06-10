// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.

using CoolParking.BL.Helpers;

namespace CoolParking.BL.Models
{
    public class Vehicle
    {
        public string Id { get; }
        public VehicleType VehicleType { get; }
        public  decimal Balance { get; internal set; } 

        public Vehicle(string id, VehicleType vehicleType, decimal balance)
        {
            int key = (int)vehicleType;

            if (IsValidId(id) && balance >= Settings.tariffs[key])
            {
                this.Id = id;
                this.VehicleType = vehicleType;
                this.Balance = balance;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        #region ---helpers---

        private bool IsValidId(string id)
        {
            return Validation.IsValidRegistrationPlateNumber(id);
        }


        public static string GenerateRandomRegistrationPlateNumber()
        {
            string plateNumber = $"{GenerateTwoNumbers(Settings.TWO_LETERS)}-{GenerateTwoNumbers(Settings.FOUR_DIGITS)}-{GenerateTwoNumbers(Settings.TWO_LETERS)}";

            return plateNumber;
        }

        private static string GenerateTwoNumbers(string key)
        {
            string result = string.Empty;
            Random _random = new Random();

            if (key == Settings.TWO_LETERS)
            {
                result = new string(Enumerable.Repeat(Settings.ARRAY_LETERS, 2).Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            else if(key == Settings.FOUR_DIGITS)
            {
                result = _random.Next(0, 9999).ToString("D4");
            }

            return result;
        }

        #endregion
    }
}