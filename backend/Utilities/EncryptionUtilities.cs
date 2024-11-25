using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace backend.Utilities
{
    public class EncryptionUtilities
    {
        public static string GenerateSalt(int size = 16)
        {



            var salt = new byte[size];
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
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


    }
}