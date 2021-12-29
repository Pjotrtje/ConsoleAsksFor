﻿namespace ConsoleAsksFor;

internal sealed class DateTimeOffsetQuestionParser
{
    private readonly TimeZoneInfo _timeZone;
    private readonly DateTimeOffsetFormat _format;

    public string TimeZoneInfoDescription { get; }

    public ClusteredRange<DateTimeOffset> Range { get; }

    public DateTimeOffsetQuestionParser(
        DateTimeOffsetFormat format,
        TimeZoneInfo? timeZone,
        ClusteredRange<DateTimeOffset> range)
    {
        _format = format;
        _timeZone = timeZone ?? TimeZoneInfo.Utc;
        Range = TruncateMinMax(range, format);
        TimeZoneInfoDescription = timeZone?.Id ?? "Local";
    }

    private static ClusteredRange<DateTimeOffset> TruncateMinMax(ClusteredRange<DateTimeOffset> range, DateTimeOffsetFormat dateTimeOffsetFormat)
    {
        var fixedRanges = range.SubRanges
            .Select(r => new Range<DateTimeOffset>(
                r.Min.TruncateMinValue(dateTimeOffsetFormat.SmallestIncrementInTicks),
                r.Max.TruncateMaxValue(dateTimeOffsetFormat.SmallestIncrementInTicks)))
            .ToList();

        return new(fixedRanges);
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out DateTimeOffset answer)
    {
        errors = Enumerable.Empty<string>();
        if (!TryParseExact(answerAsString, out var dateTime))
        {
            answer = default;
            return false;
        }

        var isCorrect = Range.Contains(dateTime);
        answer = isCorrect
            ? dateTime
            : default;

        return isCorrect;
    }

    public bool TryParseExact(string answerAsString, out DateTimeOffset answer)
    {
        var fixedAnswerAsString = $"{_format.Prefix}{answerAsString.Trim()}";
        var isCorrect = DateTime.TryParseExact(fixedAnswerAsString, _format.PatternIncludingPrefix, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

        try
        {
            answer = isCorrect
                ? dateTime.ToDateTimeOffset(_timeZone)
                : default;
            return isCorrect;
        }
        catch
        {
            // When converting to ToDateTimeOffset a valid datetime (i.e. Min/Max) can be converted to a DateTimeOffset which is out of range due to offset.
            // Or the value does not exist in time zone due to summer/winter time
            answer = default;
            return false;
        }
    }
}