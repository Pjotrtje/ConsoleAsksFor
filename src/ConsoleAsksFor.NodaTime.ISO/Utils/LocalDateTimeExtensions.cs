namespace ConsoleAsksFor.NodaTime.ISO;

internal static class LocalDateTimeExtensions
{
    public static LocalDateTime WithoutMilliseconds(this LocalDateTime dateTimeOffset)
        => new(
            dateTimeOffset.Year,
            dateTimeOffset.Month,
            dateTimeOffset.Day,
            dateTimeOffset.Hour,
            dateTimeOffset.Minute,
            dateTimeOffset.Second,
            dateTimeOffset.Calendar);
}