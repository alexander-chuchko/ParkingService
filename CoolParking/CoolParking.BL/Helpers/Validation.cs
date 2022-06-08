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

        public static bool CompareStrings(string password, IEnumerable<Vehicle> vehicles) //типизированым сделать, написать с помощью LINQ
        {
            var comparisonResult = false;

            foreach (var vehicle in vehicles)
            {
                var returnedResult = string.Compare(password, vehicle.Id, false);
                
                if (returnedResult == 0)
                {
                    comparisonResult = true;
                    break;
                }
            }

            return comparisonResult;
        }

        public static bool IsUnique(string id, IEnumerable<Vehicle> vehicles)
        {
            var validationResult = false;
            MatchCollection myMatches;
            foreach (var vehicle in vehicles)
            {

            }

            myMatches = Regex.Matches(id, "asdasdasdasdasd", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            if (myMatches.Count == 0)
            {
                validationResult = true;
            }
            
            return validationResult;
        }
    }
}
