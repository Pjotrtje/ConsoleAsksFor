using System;
using System.Globalization;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class DecimalFormat
    {
        public decimal SmallestIncrement { get; }

        public int DigitsAfterDecimalPoint { get; }

        private readonly string _pattern;

        public DecimalFormat(Scale scale)
        {
            SmallestIncrement = Enumerable
                .Range(0, scale.DigitsAfterDecimalPoint)
                .Aggregate(1m, (current, _) => current / 10);

            _pattern = scale.DigitsAfterDecimalPoint > 0
                ? "#,##0." + new string('0', scale.DigitsAfterDecimalPoint)
                : "#,##0";

            DigitsAfterDecimalPoint = scale.DigitsAfterDecimalPoint;
        }

        // Decimal scale and precision are not 1 to 1 matched. To ensure no issues; lower maxes by factor 100
        // Also for every scale decrease by factor 10 (=smallestIncrement)
        /// Nobody wants the real min/max and edge cases are hard to fix. So for now fix it by being dumb
        internal Range<decimal> GetAllowedRange()
        {
            var factor = SmallestIncrement / 100;
            var min = Math.Round(decimal.MinValue * factor).TruncateMinValue(DigitsAfterDecimalPoint);
            var max = Math.Round(decimal.MaxValue * factor).TruncateMaxValue(DigitsAfterDecimalPoint);
            return new Range<decimal>(min, max);
        }

        public string FormatAnswer(decimal value)
            => value.ToString(_pattern, CultureInfo.InvariantCulture);
    }
}