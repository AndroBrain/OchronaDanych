using ShopManagmentAPI.domain.model.user;
using System.Text.RegularExpressions;

namespace OchronaDanychAPI.domain
{
    public class PasswordChecker
    {
        public static string? IsValid(string password)
        {
            if (!ContainsAllRequiredCharacters(password))
            {
                return "Password must contain 1 lowercase character, 1 uppercase character, 1 number, 1 special character and be at least 8 letters long";
            }
            if (IsTooLong(password))
            {
                return "Password too long";
            }
            if (!IsEntropyBigEnough(password))
            {
                return "Password entropy is too low";
            }
            return null;
        }
        public static bool ContainsAllRequiredCharacters(string password)
        {
            // min 1 lowercase, 1 uppercase, 1 number, 1 special character, 8 letters long
            var regex = new Regex("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.{8,})");
            return regex.IsMatch(password);
        }

        public static bool IsTooLong(string password)
        {
            return password.Length > 32;
        }

        public static bool IsEntropyBigEnough(string password)
        {
            return ShannonEntropy(password) * 16 > 40;
        }

        private static double ShannonEntropy(string password)
        {
            Dictionary<char, int> K = password.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            double entropy = 0;
            foreach (var character in K)
            {
                double PR = character.Value / (double)password.Length;
                entropy -= PR * Math.Log(PR, 2);
            }
            return entropy;
        }
    }
}
