namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    private static readonly Range<AnnualDate> FullAnnualDateRange = new(
        new AnnualDate(01, 01),
        new AnnualDate(12, 31));

    /// <summary>
    /// Ask for <see cref="AnnualDate"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<AnnualDate> AskForAnnualDate(
        this IConsole console,
        string questionText,
        RangeConstraint<AnnualDate>? range = null,
        AnnualDate? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.AnnualDate,
            null,
            range.ToLocalDateTimeClusteredRange(),
            defaultValue?.ToLocalDateTime());

        var localDateTime = await console.Ask(question, cancellationToken);
        return new AnnualDate(localDateTime.Month, localDateTime.Day);
    }

    private static ClusteredRange<LocalDateTime> ToLocalDateTimeClusteredRange(this RangeConstraint<AnnualDate>? range)
        => range.ToLocalDateTimeClusteredRange(FullAnnualDateRange, ToLocalDateTime);

    private static LocalDateTime ToLocalDateTime(this AnnualDate annualDate)
        => new LocalDate(2000, annualDate.Month, annualDate.Day) + LocalTime.Midnight;
}