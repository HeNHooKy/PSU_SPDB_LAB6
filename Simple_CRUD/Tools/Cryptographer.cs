using System;
using System.Security.Cryptography;

namespace CRUD.Tools
{
    public static class Cryptographer
    {
        public static bool Equals(string str, string hashStr)
        {
            byte[] hashBytes = Convert.FromBase64String(hashStr);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(str, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);

            return Contains(hashBytes, hash, 16);
        }

        public static string Encrypt(string str)
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(str, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        private static bool Contains(byte[] source, byte[] destination, int startSourceIndex)
        {
            int endIndex = Math.Min(source.Length - startSourceIndex, destination.Length);
            for (int i = 0; i < endIndex; i++)
            {
                if (source[i + startSourceIndex] != destination[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
