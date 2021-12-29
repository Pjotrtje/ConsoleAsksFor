namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="LocalDateTime"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<LocalDateTime> AskForLocalDateTime(
        this IConsole console,
        string questionText,
        RangeConstraint<LocalDateTime>? range = null,
        LocalDateTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.DateTime,
            null,
            (range ?? RangeConstraint.None).ToClusteredRange(),
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }

    internal static ClusteredRange<LocalDateTime> ToClusteredRange(this RangeConstraint<LocalDateTime> rangeConstraint)
    {
        // ToDo truncate?
        var range = new Range<LocalDateTime>(
            rangeConstraint.Min ?? LocalDate.MinIsoValue.At(new LocalTime(00, 00, 00)),
            rangeConstraint.Max ?? LocalDate.MaxIsoValue.At(new LocalTime(23, 59, 59)));

        return new(new[] { range });
    }
}