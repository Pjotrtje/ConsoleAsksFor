namespace ConsoleAsksFor.NodaTime.ISO;

/// <summary>
/// Extension methods for <see cref="IConsole"/> related to NodaTime.ISO.
/// </summary>
public static partial class AskForAppender
{
    private static ClusteredRange<LocalDateTime> ToLocalDateTimeClusteredRange<T>(
        this RangeConstraint<T>? rangeConstraint,
        Range<T> fullRange,
        Func<T, LocalDateTime> converter)
        where T : struct, IComparable<T>
    {
        var isCircular = rangeConstraint is not null &&
                         rangeConstraint.Min.HasValue &&
                         rangeConstraint.Max.HasValue &&
                         rangeConstraint.Min.Value.CompareTo(rangeConstraint.Max.Value) > 0;

        if (isCircular)
        {
            var range1 = new Range<LocalDateTime>(
                converter(rangeConstraint!.Min!.Value),
                converter(fullRange.Max));

            var range2 = new Range<LocalDateTime>(
                converter(fullRange.Min),
                converter(rangeConstraint.Max!.Value));

            return new(new[] { range1, range2 });
        }

        var range = new Range<LocalDateTime>(
            converter(rangeConstraint?.Min ?? fullRange.Min),
            converter(rangeConstraint?.Max ?? fullRange.Max));

        return new(new[] { range });
    }
}