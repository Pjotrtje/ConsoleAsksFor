namespace ConsoleAsksFor;

internal static class TimeZoneInfoExtensions
{
    public static Range<DateTimeOffset> GetAllowedRange(this TimeZoneInfo timeZone)
    {
        var min = timeZone.GetMinDateTimeOffsetForTimeZone();
        var max = timeZone.GetMaxDateTimeOffsetForTimeZone();

        return new(min, max);
    }

    private static DateTimeOffset GetMinDateTimeOffsetForTimeZone(this TimeZoneInfo timeZone)
    {
        var unspecifiedMinValue = DateTimeOffset.MinValue.DateTime;
        var minOffset = timeZone.GetUtcOffset(unspecifiedMinValue);
        var correction = minOffset < TimeSpan.Zero
            ? TimeSpan.Zero
            : minOffset;

        return new DateTimeOffset(unspecifiedMinValue.Add(correction), minOffset);
    }

    private static DateTimeOffset GetMaxDateTimeOffsetForTimeZone(this TimeZoneInfo timeZone)
    {
        var unspecifiedMaxValue = DateTimeOffset.MaxValue.DateTime;
        var maxOffset = timeZone.GetUtcOffset(unspecifiedMaxValue);
        var correction = maxOffset > TimeSpan.Zero
            ? TimeSpan.Zero
            : maxOffset;

        return new DateTimeOffset(unspecifiedMaxValue.Add(correction), maxOffset);
    }
}