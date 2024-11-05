using System;
using System.Security.Cryptography;
using System.Text;

namespace TestingPlatform.Infrastructure.Cryptography
{
    internal class Cryptography
    {

        public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }

    }
}
