using System.Text.RegularExpressions;

namespace OchronaDanychAPI.domain
{
    public class PasswordChecker
    {
        public static bool IsStrongEnough(string password)
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
