using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ILoveBaku.Infrastructure.Helpers
{
    public static class Hashing
    {
        public static byte[] GenerateSalt()
        {
            using RandomNumberGenerator generator = RandomNumberGenerator.Create();
            byte[] salt = new byte[128 / 8];
            generator.GetBytes(salt);
            return salt;
        }

        public static byte[] Hash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 10000, 512 / 8);
        }

        public static bool VerifyPassword(byte[] hashedPassword, byte[] salt, string password)
        {
            return hashedPassword.SequenceEqual(Hash(password, salt));
        }
    }
}
