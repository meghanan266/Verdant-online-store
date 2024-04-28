using System.Security.Cryptography;

namespace E_commerce_API.Helper
{
    public class PasswordHasher
    {
        private static RSACryptoServiceProvider rng = new RSACryptoServiceProvider();
        public static readonly int saltsize = 16;
        public static readonly int iterations = 10000;
        public static readonly int hashSize = 20;

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[saltsize];
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = key.GetBytes(hashSize);

            var hashBytes = new byte[hashSize + saltsize];
            Array.Copy(salt, 0, hashBytes, 0, saltsize);
            Array.Copy(hash, 0, hashBytes, saltsize, hashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);
            var salt = new byte[saltsize];
            Array.Copy(hashBytes, 0, salt, 0, saltsize);
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = key.GetBytes(hashSize);

            for (var i = 0; i < hashSize; i++)
            {
                if (hashBytes[i + saltsize] != hash[i])
                    return false;

            }
            return true;
        }
    }
}
