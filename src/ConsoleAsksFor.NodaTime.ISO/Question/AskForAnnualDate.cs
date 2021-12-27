namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
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
            ToLocalDateTimeRangeConstraint(range),
            defaultValue?.ToLocalDateTime());

        var localDateTime = await console.Ask(question, cancellationToken);
        return new AnnualDate(localDateTime.Month, localDateTime.Day);
    }

    private static RangeConstraint<LocalDateTime> ToLocalDateTimeRangeConstraint(RangeConstraint<AnnualDate>? rangeConstraint)
        => RangeConstraint.Between(
            (rangeConstraint?.Min ?? new AnnualDate(01, 01)).ToLocalDateTime(),
            (rangeConstraint?.Max ?? new AnnualDate(12, 31)).ToLocalDateTime());

    private static LocalDateTime ToLocalDateTime(this AnnualDate annualDate)
        => new LocalDate(2000, annualDate.Month, annualDate.Day) + LocalTime.Midnight;
}