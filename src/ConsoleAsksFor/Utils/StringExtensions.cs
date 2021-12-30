namespace ConsoleAsksFor;

internal static class StringExtensions
{
    public static bool StartsWith(this string s, string compareValue, StringComparer stringComparer)
    {
        if (s.Length < compareValue.Length)
        {
            return false;
        }

        var firstPart = s[..compareValue.Length];
        return stringComparer.Equals(firstPart, compareValue);
    }
}