using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace captcha_integration.Core
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(x => GenerateRandomKey(5));
        }

        public static string GenerateRandomKey(int bytes)
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[bytes];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }
    }
}