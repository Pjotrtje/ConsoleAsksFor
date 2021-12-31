namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="DateTime"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="kind">When <see cref="DateTimeKind.Unspecified"/> then <see cref="DateTimeKind.Local"/> is assumed.</param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<DateTime> AskForDateTime(
        this IConsole console,
        string questionText,
        DateTimeKind kind = DateTimeKind.Local,
        RangeConstraint<DateTime>? range = null,
        DateTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var timeZoneInfo = kind == DateTimeKind.Utc
            ? TimeZoneInfo.Utc
            : TimeZoneInfo.Local;

        var question = GetDateTimeQuestion(
            questionText,
            timeZoneInfo,
            range,
            defaultValue);

        var result = await console.Ask(question, cancellationToken);
        return DateTime.SpecifyKind(result.DateTime, kind);
    }

    internal static DateTimeOffsetQuestion GetDateTimeQuestion(
        string questionText,
        TimeZoneInfo timeZoneInfo,
        RangeConstraint<DateTime>? range,
        DateTime? defaultValue)
    {
        var kind = timeZoneInfo.Id == TimeZoneInfo.Utc.Id
            ? DateTimeKind.Utc
            : DateTimeKind.Local;

        return new(
            questionText,
            DateTimeOffsetFormat.DateTime,
            timeZoneInfo,
            range.ToDateTimeOffsetRangeConstraint(kind).ToRange(timeZoneInfo).ToClusteredRange(),
            defaultValue?.ToKind(kind));
    }

    private static RangeConstraint<DateTimeOffset> ToDateTimeOffsetRangeConstraint(this RangeConstraint<DateTime>? range, DateTimeKind kind)
        => new(
            range?.Min?.ToKind(kind),
            range?.Max?.ToKind(kind));
}