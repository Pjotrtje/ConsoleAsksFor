namespace ConsoleAsksFor;

internal static class DateTimeOffsetExtensions
{
    public static DateTimeOffset ToTimeZone(this DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone)
    {
        var utcDateTime = dateTimeOffset.UtcDateTime;
        var offset = timeZone.GetUtcOffset(utcDateTime);
        return new DateTimeOffset(utcDateTime).ToOffset(offset);
    }

    public static DateTimeOffset WithoutMilliseconds(this DateTimeOffset dateTimeOffset)
        => new(
            dateTimeOffset.Year,
            dateTimeOffset.Month,
            dateTimeOffset.Day,
            dateTimeOffset.Hour,
            dateTimeOffset.Minute,
            dateTimeOffset.Second,
            dateTimeOffset.Offset);
}