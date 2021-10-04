using System;

namespace ConsoleAsksFor
{
    internal static class StringExtensions
    {
        public static bool StartsWith(this string s, string compareValue, StringComparer stringComparer)
        {
            if (s.Length < compareValue.Length)
            {
                return false;
            }

            var firstPart = s.Substring(0, compareValue.Length);
            return stringComparer.Equals(firstPart, compareValue);
        }
    }
}