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

        public static bool CompareStrings(string password, string confirmPassword)
        {
            var comparisonResult = false;

            var returnedResult = string.Compare(password, confirmPassword, false);

            if (returnedResult == 0)
            {
                comparisonResult = true;
            }
            return comparisonResult;
        }

        public static bool IsUnique(string id)
        {
            var validationResult = false;

            MatchCollection myMatches = Regex.Matches(id, "asdasdasdasdasd", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            if (myMatches.Count == 0)
            {
                validationResult = true;
            }
            
            return validationResult;
        }
    }
}
