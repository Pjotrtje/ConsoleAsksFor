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
            range.ToDateTimeOffsetRangeConstraint().ToClusteredRange(TimeZoneInfo.Utc, DateTimeOffsetFormat.Date),
            defaultValue.ToDateTimeOffset());

        var result = await console.Ask(question, cancellationToken);
        return DateOnly.FromDateTime(result.Date);
    }

    private static RangeConstraint<DateTimeOffset> ToDateTimeOffsetRangeConstraint(this RangeConstraint<DateOnly>? range)
        => new(
            range?.Min.ToDateTimeOffset(),
            range?.Max.ToDateTimeOffset());

    private static DateTimeOffset? ToDateTimeOffset(this DateOnly? date)
        => date?.ToDateTime(new TimeOnly(00, 00, 00), DateTimeKind.Utc);

    internal static ClusteredRange<DateTimeOffset> ToClusteredRange(
        this RangeConstraint<DateTimeOffset> rangeConstraint,
        TimeZoneInfo timeZone,
        DateTimeOffsetFormat format)
    {
        var allowedRange = timeZone.GetAllowedRange(format.SmallestIncrementInTicks);

        //Todo uitwerken
        //3 corrects:
        //-1 missende values invullen: niet relevant voor TimeOnlt/DateOnly
        //-2 fixen voor tijdzone: niet relevant voor TimeOnlt/DateOnly

        DateTimeOffset? Corrected(DateTimeOffset? value)
            => value?.UtcDateTime < allowedRange.Min.UtcDateTime
                ? allowedRange.Min
                : value?.UtcDateTime > allowedRange.Max.UtcDateTime
                    ? allowedRange.Max
                    : value?.ToTimeZone(timeZone);

        var min = Corrected(rangeConstraint.Min) ?? allowedRange.Min;
        var max = Corrected(rangeConstraint.Max) ?? allowedRange.Max;

        var range = new Range<DateTimeOffset>(min, max);

        return new(new[] { range });
    }
}