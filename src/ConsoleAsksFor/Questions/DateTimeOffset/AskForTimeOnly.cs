namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    private static readonly Range<TimeOnly> FullTimeOnlyRange = new(
        new TimeOnly(00, 00, 00),
        new TimeOnly(23, 59, 59));

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
        var question = GetTimeOnlyQuestion(questionText, range, defaultValue);

        var result = await console.Ask(question, cancellationToken);
        return TimeOnly.FromDateTime(result.Date);
    }

    private static DateTimeOffsetQuestion GetTimeOnlyQuestion(string questionText, RangeConstraint<TimeOnly>? range, TimeOnly? defaultValue)
        => new(
            questionText,
            DateTimeOffsetFormat.Time,
            null,
            range.ToLocalDateTimeClusteredRange(FullTimeOnlyRange, ToDateTimeOffset),
            defaultValue?.ToDateTimeOffset());

    private static DateTimeOffset ToDateTimeOffset(this TimeOnly time)
        => FakeDate
            .ToDateTime(time, DateTimeKind.Unspecified)
            .ToDateTimeOffset(TimeZoneInfo.Utc)
            .WithoutMilliseconds();

    private static ClusteredRange<DateTimeOffset> ToLocalDateTimeClusteredRange<T>(
        this RangeConstraint<T>? rangeConstraint,
        Range<T> fullRange,
        Func<T, DateTimeOffset> converter)
        where T : struct, IComparable<T>
    {
        var isCircular = rangeConstraint is not null &&
                         rangeConstraint.Min.HasValue &&
                         rangeConstraint.Max.HasValue &&
                         rangeConstraint.Min.Value.CompareTo(rangeConstraint.Max.Value) > 0;

        if (isCircular)
        {
            var range1 = new Range<DateTimeOffset>(
                converter(rangeConstraint!.Min!.Value),
                converter(fullRange.Max));

            var range2 = new Range<DateTimeOffset>(
                converter(fullRange.Min),
                converter(rangeConstraint.Max!.Value));

            return new(new[] { range1, range2 });
        }

        var range = new Range<DateTimeOffset>(
            converter(rangeConstraint?.Min ?? fullRange.Min),
            converter(rangeConstraint?.Max ?? fullRange.Max));

        return new(new[] { range });
    }
}