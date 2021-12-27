using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ConsoleAsksFor.Sdk;

/// <summary>
/// Closed range; thus <see cref="Min"/>/<see cref="Max"/> are valid values of range
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record Range<T>
    where T : struct, IComparable<T>
{
    /// <summary>
    /// Min value of range.
    /// </summary>
    public T Min { get; }

    /// <summary>
    /// Max value of range.
    /// </summary>
    public T Max { get; }

    /// <summary>
    /// Constructs <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="min">Min valid value of range.</param>
    /// <param name="max">Max valid value of range.</param>
    /// <exception cref="InvalidRangeException"></exception>
    public Range(T min, T max)
    {
        if (min.CompareTo(max) > 0)
        {
            throw InvalidRangeException.Create(min, max);
        }

        Min = min;
        Max = max;
    }

    /// <summary>
    /// Determine whether range has overlap with other range.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="overlap">When overlap; the range of overlap.</param>
    /// <returns></returns>
    public bool HasOverlap(Range<T> other, [NotNullWhen(true)] out Range<T>? overlap)
    {
        var hasOverlap = Min.CompareTo(other.Max) <= 0 &&
                         other.Min.CompareTo(Max) <= 0;

        overlap = hasOverlap
            ? new Range<T>(
                new[] { other.Min, Min }.Max(),
                new[] { other.Max, Max }.Min())
            : default;

        return hasOverlap;
    }

    /// <summary>
    /// Checks whether <paramref name="value"/> is in range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(T value)
        => value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
}