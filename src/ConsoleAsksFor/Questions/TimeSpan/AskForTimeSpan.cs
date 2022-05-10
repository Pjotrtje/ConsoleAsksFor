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
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static async Task<TimeSpan> AskForTimeSpan(
        this IConsole console,
        string questionText,
        TimeSpanUnitType unitType,
        RangeConstraint<TimeSpan>? range = null,
        TimeSpan? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var defaultDecimalValue = GetDecimal(unitType, defaultValue);
        var min = GetDecimal(unitType, range?.Min ?? TimeSpan.MinValue);
        var max = GetDecimal(unitType, range?.Max ?? TimeSpan.MaxValue);
        var decimalRange = new RangeConstraint<decimal>(min, max);

        var question = new DecimalQuestion(
            questionText,
            Scale.Zero,
            decimalRange,
            Hint.ForUnit($"{unitType.ToString().Replace("From", "")}"),
            defaultDecimalValue);

        var decimalValue = await console.Ask(question, cancellationToken);
        return GetTimeSpanValue(unitType, (long)decimalValue);
    }

    private static decimal? GetDecimal(TimeSpanUnitType unitType, TimeSpan? value)
        => !value.HasValue
            ? null
            : unitType switch
            {
                TimeSpanUnitType.FromDays => (long)value.Value.TotalDays,
                TimeSpanUnitType.FromHours => (long)value.Value.TotalHours,
                TimeSpanUnitType.FromMinutes => (long)value.Value.TotalMinutes,
                TimeSpanUnitType.FromSeconds => (long)value.Value.TotalSeconds,
                TimeSpanUnitType.FromMilliseconds => (long)value.Value.TotalMilliseconds,
                TimeSpanUnitType.FromTicks => value.Value.Ticks,
                _ => null,
            };

    private static TimeSpan GetTimeSpanValue(TimeSpanUnitType unitType, long value)
        => unitType switch
        {
            TimeSpanUnitType.FromDays => TimeSpan.FromDays(value),
            TimeSpanUnitType.FromHours => TimeSpan.FromHours(value),
            TimeSpanUnitType.FromMinutes => TimeSpan.FromMinutes(value),
            TimeSpanUnitType.FromSeconds => TimeSpan.FromSeconds(value),
            TimeSpanUnitType.FromMilliseconds => TimeSpan.FromMilliseconds(value),
            TimeSpanUnitType.FromTicks => TimeSpan.FromTicks(value),
            _ => default,
        };
}
