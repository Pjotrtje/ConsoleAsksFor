namespace ConsoleAsksFor;

internal sealed class DateTimeOffsetQuestionParser
{
    private readonly TimeZoneInfo? _timeZone;
    private readonly DateTimeOffsetFormat _format;

    public string TimeZoneInfoDescription => _timeZone?.Id ?? "Local";

    public Range<DateTimeOffset> Range { get; }

    public DateTimeOffsetQuestionParser(
        DateTimeOffsetFormat format,
        TimeZoneInfo? timeZone,
        RangeConstraint<DateTimeOffset> range)
    {
        _format = format;
        _timeZone = timeZone;

        Range = GetInputRange(range, timeZone ?? TimeZoneInfo.Utc, format);
    }

    private static Range<DateTimeOffset> GetInputRange(
        RangeConstraint<DateTimeOffset> range,
        TimeZoneInfo timeZone,
        DateTimeOffsetFormat format)
    {
        var allowedRange = timeZone.GetAllowedRange(format.SmallestIncrementInTicks);

        DateTimeOffset? Corrected(DateTimeOffset? value)
            => value?.UtcDateTime < allowedRange.Min.UtcDateTime
                ? allowedRange.Min
                : value?.UtcDateTime > allowedRange.Max.UtcDateTime
                    ? allowedRange.Max
                    : value?.ToTimeZone(timeZone).TruncateMinValue(format.SmallestIncrementInTicks);

        var min = Corrected(range.Min) ?? allowedRange.Min;
        var max = Corrected(range.Max) ?? allowedRange.Max;

        return new(min, max);
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
        var isCorrect = DateTime.TryParseExact(answerAsString.Trim(), _format.Pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

        if (_timeZone is null)
        {
            answer = dateTime;
            return isCorrect;
        }

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