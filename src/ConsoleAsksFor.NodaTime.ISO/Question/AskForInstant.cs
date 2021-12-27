namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="Instant"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Instant> AskForInstant(
        this IConsole console,
        string questionText,
        RangeConstraint<Instant>? range = null,
        Instant? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var zonedDateTime = await console.AskForZonedDateTime(
            questionText,
            DateTimeZone.Utc,
            ToZonedDateTimeConstraint(range),
            defaultValue?.InUtc(),
            cancellationToken);

        return zonedDateTime.ToInstant();
    }

    private static RangeConstraint<ZonedDateTime> ToZonedDateTimeConstraint(RangeConstraint<Instant>? range)
        => new(
            range?.Min?.InUtc(),
            range?.Max?.InUtc());
}