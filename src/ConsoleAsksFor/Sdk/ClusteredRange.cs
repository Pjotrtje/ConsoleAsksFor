namespace ConsoleAsksFor.Sdk;

/// <summary>
/// Closed range; thus <see cref="Min"/>/<see cref="Max"/> are valid values of range.
/// Can be used for logic related to <see cref="CircularRanges"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ClusteredRange<T>
    where T : struct, IComparable<T>
{
    /// <summary>
    /// Min value of range.
    /// </summary>
    public T Min() => SubRanges.First().Min;

    /// <summary>
    /// Max value of range.
    /// </summary>
    public T Max() => SubRanges.Last().Max;

    /// <summary>
    /// The sub ranges.
    /// </summary>
    public IReadOnlyCollection<Range<T>> SubRanges { get; }

    /// <summary>
    /// Constructs <see cref="ClusteredRange{T}"/>.
    /// </summary>
    /// <param name="subRanges"></param>
    public ClusteredRange(IReadOnlyCollection<Range<T>> subRanges)
    {
        SubRanges = subRanges;
    }

    /// <summary>
    /// Checks whether <paramref name="value"/> is in range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(T value)
        => SubRanges.Any(r => r.Contains(value));

    /// <summary>
    /// Determine whether range has overlap with other range.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="overlap">When overlap; the range of overlap.</param>
    /// <returns></returns>
    public bool HasOverlap(Range<T> other, [NotNullWhen(true)] out ClusteredRange<T>? overlap)
    {
        var newSubRanges = new List<Range<T>>();
        foreach (var subRange in SubRanges)
        {
            if (subRange.HasOverlap(other, out var range))
            {
                newSubRanges.Add(range);
            }
        }

        var result = newSubRanges.Any();
        overlap = result
            ? new ClusteredRange<T>(newSubRanges)
            : null;

        return result;
    }
}