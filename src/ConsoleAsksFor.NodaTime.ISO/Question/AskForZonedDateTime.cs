namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="ZonedDateTime"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="dateTimeZone"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<ZonedDateTime> AskForZonedDateTime(
        this IConsole console,
        string questionText,
        DateTimeZone dateTimeZone,
        RangeConstraint<ZonedDateTime>? range = null,
        ZonedDateTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.DateTime,
            dateTimeZone,
            range.ToLocalDateTimeRangeConstraint(dateTimeZone).ToClusteredRange(),
            defaultValue?.InZone(dateTimeZone).LocalDateTime);

        var localDateTime = await console.Ask(question, cancellationToken);
        var zonedDateTimes = dateTimeZone.MapLocal(localDateTime);
        return zonedDateTimes.Count switch
        {
            1 => zonedDateTimes.Single(),
            2 => await console.AskForCorrectZone(zonedDateTimes, defaultValue, cancellationToken),
            _ => throw new InvalidOperationException("Mapping has count outside range 1-2; should not happen."),
        };
    }

    private static async Task<ZonedDateTime> AskForCorrectZone(
        this IConsole console,
        ZoneLocalMapping zoneLocalMapping,
        ZonedDateTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var mappingItems = new Dictionary<string, ZonedDateTime>
        {
            { "First", zoneLocalMapping.First() },
            { "Last", zoneLocalMapping.Last() },
        };

        var mappingDefaultValue = defaultValue switch
        {
            { } when zoneLocalMapping.First() == defaultValue => "First",
            { } when zoneLocalMapping.Last() == defaultValue => "Last",
            _ => null,
        };

        return await console.AskForItem(
            "This ZonedDateTime occurs twice, which to choose?",
            mappingItems,
            mappingDefaultValue,
            cancellationToken);
    }

    private static RangeConstraint<LocalDateTime> ToLocalDateTimeRangeConstraint(this RangeConstraint<ZonedDateTime>? range, DateTimeZone dateTimeZone)
        => new(
            range?.Min?.InZone(dateTimeZone).LocalDateTime,
            range?.Max?.InZone(dateTimeZone).LocalDateTime);
}