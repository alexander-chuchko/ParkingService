// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
// TODO: реализовать класс Parking.
// Детали реализации зависят от вас, они просто должны соответствовать требованиям
// домашнего задания и согласовываться с другими занятиями и тестами.

using System.Collections.Generic;

namespace CoolParking.BL
{
    public class Parking : IDisposable
    {
        private static Parking? instance;
        public List<Vehicle>? Vehicles { get; set; }
        public decimal Balance { get; set; }

        public DateTime? StartTime { get; set; }  
        private Parking()
        {
        }

        public static Parking GetInstance()
        {
            if (instance == null)
            {
                instance = new Parking();
            }

            return instance;
        }

        public void Dispose()
        {
        }

        public void DisposeInstance()
        {
            if (instance != null)
            {
                instance.Dispose();
                instance = null;
            }
        }
    }
}