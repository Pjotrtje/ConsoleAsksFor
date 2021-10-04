using System;

namespace ConsoleAsksFor
{
    internal static class DecimalExtensions
    {
        public static decimal TruncateMinValue(this decimal dec, int digitsAfterDecimalPoint)
            => Math.Round(dec, digitsAfterDecimalPoint, MidpointRounding.ToPositiveInfinity);

        public static decimal TruncateMaxValue(this decimal dec, int digitsAfterDecimalPoint)
            => Math.Round(dec, digitsAfterDecimalPoint, MidpointRounding.ToNegativeInfinity);

        public static decimal RoundToZero(this decimal dec)
            => Math.Round(dec, MidpointRounding.ToZero);

        // https://gist.github.com/asbjornu/c1f43647c9c2e3723a7a
        public static int GetAmountOfDigitsAfterDecimalPoint(this decimal value)
            => BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
    }
}