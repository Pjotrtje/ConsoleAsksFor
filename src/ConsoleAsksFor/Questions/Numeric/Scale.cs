using System;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    /// <summary>
    /// The scale used in <see cref="DecimalQuestion"/>.
    /// </summary>
    public sealed class Scale
    {
        /// <summary>
        /// Creates <see cref="Scale"/> with <see cref="DigitsAfterDecimalPoint"/>=0.
        /// </summary>
        public static Scale Zero => new(0);

        /// <summary>
        /// Creates <see cref="Scale"/> with <see cref="DigitsAfterDecimalPoint"/>=1.
        /// </summary>
        public static Scale One => new(1);

        /// <summary>
        /// Creates <see cref="Scale"/> with <see cref="DigitsAfterDecimalPoint"/>=2.
        /// </summary>
        public static Scale Two => new(2);

        /// <summary>
        /// Amount of digits after decimal point.
        /// </summary>
        public int DigitsAfterDecimalPoint { get; }

        private static readonly Range<int> DigitsAfterDecimalPointRange = new(0, 20);

        /// <summary>
        /// Creates new <see cref="Scale"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="digitsAfterDecimalPoint">Allowed between 0 and 20.</param>
        public static Scale Of(int digitsAfterDecimalPoint)
            => new(digitsAfterDecimalPoint);

        private Scale(int digitsAfterDecimalPoint)
        {
            if (!DigitsAfterDecimalPointRange.Contains(digitsAfterDecimalPoint))
            {
                throw new ArgumentOutOfRangeException(nameof(digitsAfterDecimalPoint), $"Allowed [{DigitsAfterDecimalPointRange.Min}..{DigitsAfterDecimalPointRange.Max}]");
            }

            DigitsAfterDecimalPoint = digitsAfterDecimalPoint;
        }
    }
}