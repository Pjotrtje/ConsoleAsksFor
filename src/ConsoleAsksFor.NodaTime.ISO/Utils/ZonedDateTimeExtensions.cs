using NodaTime;

namespace ConsoleAsksFor.NodaTime.ISO;

internal static class ZonedDateTimeExtensions
{
    public static ZonedDateTime InZone(this ZonedDateTime zonedDateTime, DateTimeZone dateTimeZone)
        => zonedDateTime.ToInstant().InZone(dateTimeZone);
}