using System;

namespace ConsoleAsksFor;

internal static class DateTimeOffsetExtensions
{
    public static DateTimeOffset ToTimeZone(this DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone)
    {
        var utcDateTime = dateTimeOffset.UtcDateTime;
        var offset = timeZone.GetUtcOffset(utcDateTime);
        return new DateTimeOffset(utcDateTime).ToOffset(offset);
    }

    public static DateTimeOffset TruncateMinValue(this DateTimeOffset date, long resolution)
    {
        try
        {
            var truncated = date.TruncateMaxValue(resolution);
            return truncated < date
                ? truncated.AddTicks(resolution)
                : truncated;
        }
        catch (ArgumentException)
        {
            // When Offset and when date=DateTimeOffset.MinValue TruncateMaxValue can result in value below minimum, which throws errors. If so up the minimal resolution
            return date.AddTicks(resolution).TruncateMaxValue(resolution);
        }
    }

    public static DateTimeOffset TruncateMaxValue(this DateTimeOffset date, long resolution)
        => new(date.Ticks - (date.Ticks % resolution), date.Offset);
}