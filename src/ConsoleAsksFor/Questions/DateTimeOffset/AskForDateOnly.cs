namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<DateOnly> AskForDateOnly(
        this IConsole console,
        string questionText,
        RangeConstraint<DateOnly>? range = null,
        DateOnly? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new DateTimeOffsetQuestion(
            questionText,
            DateTimeOffsetFormat.Date,
            null,
            range.ToDateTimeOffsetRangeConstraint(),
            defaultValue.ToDateTimeOffset());

        var result = await console.Ask(question, cancellationToken);
        return DateOnly.FromDateTime(result.Date);
    }

    private static RangeConstraint<DateTimeOffset> ToDateTimeOffsetRangeConstraint(this RangeConstraint<DateOnly>? range)
        => new(
            range?.Min.ToDateTimeOffset(),
            range?.Max.ToDateTimeOffset());

    private static DateTimeOffset? ToDateTimeOffset(this DateOnly? date)
        => date?.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
}