using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class DecimalQuestionIntellisense : IIntellisense
    {
        private readonly DecimalQuestionParser _parser;
        private readonly DecimalFormat _format;

        public DecimalQuestionIntellisense(
            DecimalQuestionParser parser,
            DecimalFormat format)
        {
            _parser = parser;
            _format = format;
        }

        public string? CompleteValue(string value)
            => Handle(value, value, IntellisenseDirection.None);

        public string? PreviousValue(string value, string hint)
            => Handle(value, hint, IntellisenseDirection.Previous);

        public string? NextValue(string value, string hint)
            => Handle(value, hint, IntellisenseDirection.Next);

        private string? Handle(string value, string hint, IntellisenseDirection direction)
        {
            if (!_parser.TryParse(value, out _, out var answer))
            {
                return TryCompleteValue(value.Trim(), direction);
            }

            var ranges = GetIntellisenseRanges(hint.Trim());
            var rangesWithOverlap = GetRangesWithOverlap(ranges);
            var delta = GetDelta(direction);
            var newMarkerValue = answer + delta;

            var toSelect = delta > 0
                ? GetNextValue(rangesWithOverlap, newMarkerValue)
                : GetPreviousValue(rangesWithOverlap, newMarkerValue);

            var newValue = _format.FormatAnswer(toSelect);
            return newValue != value
                ? newValue
                : null;
        }

        private decimal GetDelta(IntellisenseDirection direction)
            => direction switch
            {
                IntellisenseDirection.Previous => -_format.SmallestIncrement,
                IntellisenseDirection.Next => _format.SmallestIncrement,
                _ => 0,
            };

        private IEnumerable<Range<decimal>> GetIntellisenseRanges(string hint)
        {
            if (!_parser.TryParseExact(hint, out var answerFromIntellisense))
            {
                return new[] { _parser.Range };
            }

            if (answerFromIntellisense == 0)
            {
                return GetRangesFor0();
            }

            var initialRange = GetInitialIntellisenseRange(answerFromIntellisense);
            return answerFromIntellisense.GetAmountOfDigitsAfterDecimalPoint() > 0
                ? new[] { initialRange }
                : GetIntellisenseRanges(initialRange);
        }

        private Range<decimal> GetInitialIntellisenseRange(decimal answerFromIntellisense)
        {
            var currentDigits = answerFromIntellisense.GetAmountOfDigitsAfterDecimalPoint();

            var x = Enumerable
               .Range(0, currentDigits)
               .Aggregate(1m, (current, _) => current / 10);

            var post = x - _format.SmallestIncrement;

            return answerFromIntellisense >= 0
                ? new Range<decimal>(answerFromIntellisense, answerFromIntellisense + post)
                : new Range<decimal>(answerFromIntellisense - post, answerFromIntellisense);
        }

        private static decimal GetPreviousValue(IReadOnlyCollection<Range<decimal>> rangesWithOverlap, decimal newMarkerValue)
            => rangesWithOverlap
                .Where(r => r.Contains(newMarkerValue) || r.Max <= newMarkerValue)
                .Select(r => (decimal?)Math.Min(r.Max, newMarkerValue))
                .LastOrDefault() ?? rangesWithOverlap.Last().Max;

        private static decimal GetNextValue(IReadOnlyCollection<Range<decimal>> rangesWithOverlap, decimal newMarkerValue)
            => rangesWithOverlap
                .Where(r => r.Contains(newMarkerValue) || r.Min >= newMarkerValue)
                .Select(r => (decimal?)Math.Max(r.Min, newMarkerValue))
                .FirstOrDefault() ?? rangesWithOverlap.First().Min;

        private string? TryCompleteValue(string value, IntellisenseDirection direction)
        {
            if (value == "")
            {
                return direction == IntellisenseDirection.Previous
                    ? _format.FormatAnswer(_parser.Range.Max)
                    : _format.FormatAnswer(_parser.Range.Min);
            }

            if (!_parser.TryParseExact(value, out var answer))
            {
                return null;
            }

            var ranges = answer == 0
                ? GetRangesFor0()
                : GetIntellisenseRanges(GetInitialIntellisenseRange(answer));

            var rangesWithOverlap = GetRangesWithOverlap(ranges);

            if (!rangesWithOverlap.Any())
            {
                return null;
            }

            return direction == IntellisenseDirection.Previous
                ? _format.FormatAnswer(rangesWithOverlap.Last().Max)
                : _format.FormatAnswer(rangesWithOverlap.First().Min);
        }

        private IEnumerable<Range<decimal>> GetRangesFor0()
            => new[]
            {
                new Range<decimal>(0, 1 - _format.SmallestIncrement),
            };

        private IReadOnlyCollection<Range<decimal>> GetRangesWithOverlap(IEnumerable<Range<decimal>> ranges)
            => ranges
                .Select(
                    r => new
                    {
                        HasOverlap = r.HasOverlap(_parser.Range, out var overlap),
                        Overlap = overlap,
                    })
                .Where(x => x.HasOverlap)
                .Select(x => x.Overlap!)
                .ToList();

        private IEnumerable<Range<decimal>> GetIntellisenseRanges(Range<decimal> initialRange)
        {
            var isPositiveNumber = initialRange.Min >= 0;
            var additionalNines = 10 - _format.SmallestIncrement;
            var minValueAdditionalNines = isPositiveNumber ? 0 : -additionalNines;
            var maxValueAdditionalNines = isPositiveNumber ? additionalNines : 0;

            bool hasNewMin;
            bool hasNewMax;
            var newMin = initialRange.Min;
            var newMax = initialRange.Max;

            do
            {
                var range = new Range<decimal>(newMin, newMax);
                yield return range;

                hasNewMin = Function.FromLambda(() => (range.Min.RoundToZero() * 10) + minValueAdditionalNines).TryExecute(out newMin);
                hasNewMax = Function.FromLambda(() => (range.Max.RoundToZero() * 10) + maxValueAdditionalNines).TryExecute(out newMax);

                if (!hasNewMin && !isPositiveNumber)
                {
                    newMin = decimal.MinValue;
                    hasNewMin = true;
                }

                if (!hasNewMax && isPositiveNumber)
                {
                    newMax = decimal.MaxValue;
                    hasNewMax = true;
                }
            } while (hasNewMin && hasNewMax && (newMin <= newMax));
        }
    }
}