using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StatistinesAtaskaitos
{
    public static class HashExtensions
    {
        public static string GetHashedString(this HashAlgorithm algorithm, string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            var sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}