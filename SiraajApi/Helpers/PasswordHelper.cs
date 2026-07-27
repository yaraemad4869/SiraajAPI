using System.Security.Cryptography;

namespace SiraajApi.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static string GenerateReferralCode(string name)
        {
            var suffix = new string(name.Replace(" ", "").Take(3).ToArray()).ToUpper();
            var random = new Random();
            var num = random.Next(100, 999);
            return $"SIRAJ-{suffix}{num}";
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
