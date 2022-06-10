using System.Text.RegularExpressions;

namespace CoolParking.BL.Helpers
{
    public class Validation
    {
        private static Regex patternRegistrationPlateNumber;

        static Validation()
        {
            patternRegistrationPlateNumber = new Regex(@"^[A-Z]{2}-[0-9]{4}-[A-Z]{2}$");
        }

        public static bool IsValidRegistrationPlateNumber(string id)
        {
            var validationResult = false;

            if (patternRegistrationPlateNumber.IsMatch(id))
            {
                validationResult = true;
            }

            return validationResult;
        }
    }
}
