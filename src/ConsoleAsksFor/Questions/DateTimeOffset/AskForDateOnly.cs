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
        var question = GetDateOnlyQuestion(questionText, range, defaultValue);
        var result = await console.Ask(question, cancellationToken);
        return DateOnly.FromDateTime(result.Date);
    }

    internal static DateTimeOffsetQuestion GetDateOnlyQuestion(string questionText, RangeConstraint<DateOnly>? range, DateOnly? defaultValue)
        => new(
            questionText,
            DateTimeOffsetFormat.Date,
            null,
            range.ToDateTimeOffsetRangeConstraint().ToRange(TimeZoneInfo.Utc).ToClusteredRange(),
            defaultValue?.ToDateTimeOffset());

    private static RangeConstraint<DateTimeOffset> ToDateTimeOffsetRangeConstraint(this RangeConstraint<DateOnly>? range)
        => new(
            range?.Min?.ToDateTimeOffset(),
            range?.Max?.ToDateTimeOffset());

    private static DateTimeOffset ToDateTimeOffset(this DateOnly date)
        => date.ToDateTime(new TimeOnly(00, 00, 00), DateTimeKind.Utc);

    private static Range<DateTimeOffset> ToRange(
        this RangeConstraint<DateTimeOffset> rangeConstraint,
        TimeZoneInfo timeZone)
    {
        var allowedRange = timeZone.GetAllowedRange();
        DateTimeOffset? Corrected(DateTimeOffset? value)
            => value?.UtcDateTime < allowedRange.Min.UtcDateTime
                ? allowedRange.Min
                : value?.UtcDateTime > allowedRange.Max.UtcDateTime
                    ? allowedRange.Max
                    : value?.ToTimeZone(timeZone).WithoutMilliseconds();

        var min = Corrected(rangeConstraint.Min) ?? allowedRange.Min;
        var max = Corrected(rangeConstraint.Max) ?? allowedRange.Max;

        return new Range<DateTimeOffset>(min, max);
    }

    private static ClusteredRange<DateTimeOffset> ToClusteredRange(
        this Range<DateTimeOffset> range)
    {
        return new([range]);
    }
}