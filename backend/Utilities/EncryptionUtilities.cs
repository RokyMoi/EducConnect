using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using backend.DTOs;
using EduConnect.DTOs;
using EduConnect.Entities.Person;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;

namespace EduConnect.Utilities
{
    public class EncryptionUtilities
    {
        public static string GenerateSalt(int size = 16)
        {



            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }




            return Convert.ToBase64String(salt);
        }

        public static string GenerateRandomString(int stringSize = 6)
        {
            var bytes = new byte[6];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        public static string HashPassword(string password)
        {
            // Generate a salt
            var salt = GenerateSalt();
            // Hash the password with the salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                // Combine the salt and hash for storage
                var hashBytes = new byte[36];
                Array.Copy(Convert.FromBase64String(salt), 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the stored hash with the computed hash
            return hashBytes.Skip(16).Take(20).SequenceEqual(hash);

        }
    }
}