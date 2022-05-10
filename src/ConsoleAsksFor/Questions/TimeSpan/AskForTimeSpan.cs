namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static async Task<TimeSpan> AskForTimeSpan(
        this IConsole console,
        string questionText,
        TimeSpanType type,
        RangeConstraint<TimeSpan>? range = null,
        TimeSpan? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var defaultDecimalValue = GetDecimal(type, defaultValue);
        var min = GetDecimal(type, range?.Min ?? TimeSpan.MinValue);
        var max = GetDecimal(type, range?.Max ?? TimeSpan.MaxValue);
        var decimalRange = new RangeConstraint<decimal>(min, max);

        var question = new DecimalQuestion(
            questionText,
            Scale.Zero,
            decimalRange,
            Hint.ForUnit($"{type.ToString().Replace("From", "")}"),
            defaultDecimalValue);

        var decimalValue = await console.Ask(question, cancellationToken);
        return GetTimeSpanValue(type, (long)decimalValue);
    }

    private static decimal? GetDecimal(TimeSpanType type, TimeSpan? value)
        => !value.HasValue
            ? null
            : type switch
            {
                TimeSpanType.FromDays => (long)value.Value.TotalDays,
                TimeSpanType.FromHours => (long)value.Value.TotalHours,
                TimeSpanType.FromMinutes => (long)value.Value.TotalMinutes,
                TimeSpanType.FromSeconds => (long)value.Value.TotalSeconds,
                TimeSpanType.FromMilliseconds => (long)value.Value.TotalMilliseconds,
                TimeSpanType.FromTicks => value.Value.Ticks,
                _ => null,
            };

    private static TimeSpan GetTimeSpanValue(TimeSpanType type, long value)
        => type switch
        {
            TimeSpanType.FromDays => TimeSpan.FromDays(value),
            TimeSpanType.FromHours => TimeSpan.FromHours(value),
            TimeSpanType.FromMinutes => TimeSpan.FromMinutes(value),
            TimeSpanType.FromSeconds => TimeSpan.FromSeconds(value),
            TimeSpanType.FromMilliseconds => TimeSpan.FromMilliseconds(value),
            TimeSpanType.FromTicks => TimeSpan.FromTicks(value),
            _ => default,
        };
}

/// <summary>
/// ToDo
/// </summary>
public enum TimeSpanType
{
    /// <summary>
    ///ToDo
    /// </summary>
    FromDays,

    /// <summary>
    ///ToDo
    /// </summary>
    FromHours,

    /// <summary>
    ///ToDo
    /// </summary>
    FromMilliseconds,

    /// <summary>
    ///ToDo
    /// </summary>
    FromMinutes,

    /// <summary>
    ///ToDo
    /// </summary>
    FromSeconds,

    /// <summary>
    ///ToDo
    /// </summary>
    FromTicks,
}
