// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.

using CoolParking.BL.Helpers;

namespace CoolParking.BL
{
    public class Vehicle
    {
        public string Id { get; private set; }
        public VehicleType VehicleType { get; private set; }
        public  decimal Balance { get; set; }  //internal

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
            const string Array_Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 

            Random _random = new Random();
            string firstLetters = new string(Enumerable.Repeat(Array_Letters, 2).Select(s => s[_random.Next(s.Length)]).ToArray());
            string numbers = _random.Next(0, 9999).ToString("D4");
            string secondLetters = new string(Enumerable.Repeat(Array_Letters, 2).Select(s => s[_random.Next(s.Length)]).ToArray());

            return $"{firstLetters}-{numbers}-{secondLetters}";
        }

        #endregion
    }
}