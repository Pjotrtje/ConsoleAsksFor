namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="YearMonth"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<YearMonth> AskForYearMonth(
        this IConsole console,
        string questionText,
        RangeConstraint<YearMonth>? range = null,
        YearMonth? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.YearMonth,
            null,
            ToLocalDateTimeConstraint(range),
            defaultValue?.ToLocalDateTime());

        var localDateTime = await console.Ask(question, cancellationToken);
        return new YearMonth(localDateTime.Year, localDateTime.Month);
    }

    private static RangeConstraint<LocalDateTime> ToLocalDateTimeConstraint(RangeConstraint<YearMonth>? range)
        => new(
            range?.Min?.ToLocalDateTime(),
            range?.Max?.ToLocalDateTime());

    private static LocalDateTime ToLocalDateTime(this YearMonth yearMonth)
        => new LocalDate(yearMonth.Year, yearMonth.Month, 1) + LocalTime.Midnight;
}