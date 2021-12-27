namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    private static readonly DateOnly FakeDate = GetFakeDate();

    private static DateOnly GetFakeDate()
    {
        var length = DateTimeOffsetFormat.Time.Pattern.Length;

        return DateOnly.ParseExact(
            DateTimeOffsetFormat.Time.Prefix,
            DateTimeOffsetFormat.Time.PatternIncludingPrefix[..^length]);
    }

    /// <summary>
    /// Ask for <see cref="TimeOnly"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<TimeOnly> AskForTimeOnly(
        this IConsole console,
        string questionText,
        RangeConstraint<TimeOnly>? range = null,
        TimeOnly? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new DateTimeOffsetQuestion(
            questionText,
            DateTimeOffsetFormat.Time,
            null,
            range.ToDateTimeOffsetRangeConstraint(),
            defaultValue.ToDateTimeOffset());

        var result = await console.Ask(question, cancellationToken);
        return TimeOnly.FromDateTime(result.Date);
    }

    private static RangeConstraint<DateTimeOffset> ToDateTimeOffsetRangeConstraint(this RangeConstraint<TimeOnly>? range)
        => new(
            range?.Min.ToDateTimeOffset(),
            range?.Max.ToDateTimeOffset());

    private static DateTimeOffset? ToDateTimeOffset(this TimeOnly? time)
        => time is null
            ? null
            : FakeDate.ToDateTime(time.Value, DateTimeKind.Utc);
}