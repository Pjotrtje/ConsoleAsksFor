using System;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class DateTimeOffsetQuestionIntellisense : IIntellisense
    {
        private readonly DateTimeOffsetQuestionParser _parser;
        private readonly DateTimeOffsetFormat _format;

        public DateTimeOffsetQuestionIntellisense(
            DateTimeOffsetQuestionParser parser,
            DateTimeOffsetFormat format)
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
            var trimmedHint = hint.Trim();
            if (!_parser.TryParse(value, out _, out var answer))
            {
                return TryCompleteValue(trimmedHint, direction);
            }

            var overlap = GetRangeOverlappingWithIntellisense(trimmedHint);
            if (overlap is null)
            {
                return null;
            }

            var newAnswer = SafeMove(answer, overlap, direction);
            return _format.FormatAnswer(newAnswer);
        }

        private string? TryCompleteValue(string value, IntellisenseDirection direction)
        {
            var overlap = GetRangeOverlappingWithIntellisense(value);
            if (overlap is null)
            {
                return null;
            }

            return direction == IntellisenseDirection.Previous
                ? _format.FormatAnswer(overlap.Max)
                : _format.FormatAnswer(overlap.Min);
        }

        private Range<DateTimeOffset>? GetRangeOverlappingWithIntellisense(string value)
        {
            var minValue = _format.IntellisenseMinPatterns
                .Append(_format.FormatAnswer(_parser.Range.Min))
                .Select(p => TryCompleteByTemplate(value, p))
                .Min(d => d);

            var maxValue = _format.IntellisenseMaxPatterns
                .Append(_format.FormatAnswer(_parser.Range.Max))
                .Select(p => TryCompleteByTemplate(value, p))
                .Max(d => d);

            var intellisenseRange = minValue.HasValue && maxValue.HasValue
                ? new Range<DateTimeOffset>(minValue.Value, maxValue.Value)
                : null;

            return intellisenseRange is not null && intellisenseRange.HasOverlap(_parser.Range, out var overlap)
                ? overlap
                : null;
        }

        private DateTimeOffset? TryCompleteByTemplate(string value, string template)
        {
            if (template.Length <= value.Length)
            {
                return null;
            }

            var answerAsString = $"{value}{template[value.Length..]}";
            return _parser.TryParseExact(answerAsString, out var answer)
                ? answer
                : null;
        }

        private DateTimeOffset SafeMove(DateTimeOffset answer, Range<DateTimeOffset> overlap, IntellisenseDirection direction)
        {
            try
            {
                var delta = GetDelta(direction);
                var newAnswer = answer.AddTicks(delta);
                return newAnswer switch
                {
                    _ when newAnswer < overlap.Min && overlap.Contains(answer) => overlap.Max,
                    _ when newAnswer > overlap.Max && overlap.Contains(answer) => overlap.Min,
                    _ => newAnswer,
                };
            }
            catch (ArgumentOutOfRangeException)
            {
                return direction == IntellisenseDirection.Previous
                    ? overlap.Max
                    : overlap.Min;
            }
        }

        private long GetDelta(IntellisenseDirection direction)
            => direction switch
            {
                IntellisenseDirection.Previous => -_format.SmallestIncrementInTicks,
                IntellisenseDirection.Next => _format.SmallestIncrementInTicks,
                _ => 0,
            };
    }
}