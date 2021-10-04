using System;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleAsksFor
{
    internal static class Function
    {
        public static Func<T> FromLambda<T>(Func<T> f) => f;

        public static bool TryExecute<T>(this Func<T> calc, [NotNullWhen(true)] out T? result)
            where T : notnull
        {
            try
            {
                result = calc();
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}