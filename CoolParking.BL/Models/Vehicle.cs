// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.

// TODO: реализовать класс Vehicle.
// Свойства: Id (строка), VehicleType (ТипТранспорта), Balance (десятичный).
// Формат идентификатора поясняется в описании домашнего задания.
// Id и VehicleType не должны изменяться.
// Баланс должен иметь возможность изменяться только в проекте CoolParking.BL.
// Тип конструктора показывается в тестах и ​​конструктор должен иметь валидацию, что тоже видно из тестов.
// Статический метод GenerateRandomRegistrationPlateNumber должен возвращать случайно сгенерированный уникальный идентификатор.

using CoolParking.BL.Helpers;
using System;
using System.Linq;

namespace CoolParking.BL
{
    public class Vehicle
    {
        public string Id { get; private set; }
        public VehicleType VehicleType { get; private set; }
        public decimal Balance { get; set; }  //internal

        public Vehicle(string id, VehicleType vehicleType, decimal balance)
        {
            if (!IsValidId(id))
            {
                this.Id = GenerateRandomRegistrationPlateNumber();
            }

            this.Id = id;
            this.VehicleType = vehicleType;

            int key = (int)vehicleType;
            this.Balance = balance < Settings.tariffs[key]
            ? Settings.tariffs[key] :
            balance;

            if (balance >= Settings.tariffs[key]){}
            else{}
        }

        #region ---helpers---

        private bool IsValidId(string id)
        {
            return Validation.IsValidRegistrationPlateNumber(id);
        }


        private static string GenerateRandomRegistrationPlateNumber()
        {
            const string Array_Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //var res = DateTime.Now.ToString("ddmmyyhhmmss");
            Random _random = new Random();
            string firstLetters = new string(Enumerable.Repeat(Array_Letters, 2).Select(s => s[_random.Next(s.Length)]).ToArray());
            string numbers = _random.Next(0, 9999).ToString("D4");
            string secondLetters = new string(Enumerable.Repeat(Array_Letters, 2).Select(s => s[_random.Next(s.Length)]).ToArray());

            return $"{firstLetters}-{numbers}-{secondLetters}";
        }

        //Проверка на существование такого метода

        private bool IsUniqueId()
        {
            //TO DO
            return default(bool);
        }

        #endregion
    }
}