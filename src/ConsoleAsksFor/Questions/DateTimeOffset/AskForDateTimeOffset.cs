namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="timeZone"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<DateTimeOffset> AskForDateTimeOffset(
        this IConsole console,
        string questionText,
        TimeZoneInfo timeZone,
        RangeConstraint<DateTimeOffset>? range = null,
        DateTimeOffset? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new DateTimeOffsetQuestion(
            questionText,
            DateTimeOffsetFormat.DateTime,
            timeZone,
            (range ?? RangeConstraint.None).ToRange(timeZone).ToClusteredRange(),
            defaultValue?.ToTimeZone(timeZone));

        return await console.Ask(question, cancellationToken);
    }

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
        return new(new[] { range });
    }
}