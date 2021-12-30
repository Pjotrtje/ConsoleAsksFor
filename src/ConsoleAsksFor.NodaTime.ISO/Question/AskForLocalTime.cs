namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    private static readonly Range<LocalTime> FullLocalTimeRange = new(
        LocalTime.Midnight,
        LocalTime.Midnight.PlusSeconds(-1));

    /// <summary>
    /// Ask for <see cref="LocalTime"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<LocalTime> AskForLocalTime(
        this IConsole console,
        string questionText,
        RangeConstraint<LocalTime>? range = null,
        LocalTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.Time,
            null,
            range.ToLocalDateTimeClusteredRange(),
            defaultValue?.ToLocalDateTime());

        var localDateTime = await console.Ask(question, cancellationToken);
        return localDateTime.TimeOfDay;
    }

    internal static ClusteredRange<LocalDateTime> ToLocalDateTimeClusteredRange(this RangeConstraint<LocalTime>? range)
        => range.ToLocalDateTimeClusteredRange(FullLocalTimeRange, ToLocalDateTime);

    private static LocalDateTime ToLocalDateTime(this LocalTime localTime)
        => new LocalDate(2000, 1, 1).At(localTime).WithoutMilliseconds();
}