using System;
using System.Text;

namespace Sidekick.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Encode a string in Base64
        /// </summary>
        public static string EncodeBase64(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Decodes a Base64 string
        /// </summary>
        public static string DecodeBase64(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
    }
}
