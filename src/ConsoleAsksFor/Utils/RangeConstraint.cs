using System;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

/// <summary>
/// Represents "Empty" RangeConstraint. Also contains helper methods for creating readable <see cref="RangeConstraint{T}"/>
/// </summary>
public sealed class RangeConstraint
{
    private RangeConstraint()
    {
    }

    /// <summary>
    /// Create <see cref="RangeConstraint{T}"/> with both min and max.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <exception cref="InvalidRangeException"></exception>
    /// <exception cref="CannotValidateRangeException"></exception>
    /// <returns></returns>
    public static RangeConstraint<T> Between<T>(T min, T max)
        where T : struct
    {
        return new(min, max);
    }

    /// <summary>
    /// Create <see cref="RangeConstraint{T}"/> where min/max is <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static RangeConstraint<T> Exactly<T>(T amount)
        where T : struct
    {
        return new(amount, amount);
    }

    /// <summary>
    /// Create <see cref="RangeConstraint{T}"/> with only min.
    /// </summary>
    /// <param name="min"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static RangeConstraint<T> AtLeast<T>(T min)
        where T : struct
    {
        return new(min, null);
    }

    /// <summary>
    /// Create <see cref="RangeConstraint{T}"/> with only max.
    /// </summary>
    /// <param name="max"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static RangeConstraint<T> AtMost<T>(T max)
        where T : struct
    {
        return new(null, max);
    }

    /// <summary>
    /// Create <see cref="RangeConstraint"/> which has an implicit operator to convert to <see cref="RangeConstraint{T}"/> without min and without max.
    /// </summary>
    /// <returns></returns>
    public static RangeConstraint None
        => new();
}

/// <summary>
/// Constraint for range.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class RangeConstraint<T>
    where T : struct
{
    /// <summary>
    /// Min value of range.
    /// </summary>
    public T? Min { get; }

    /// <summary>
    /// Max value of range.
    /// </summary>
    public T? Max { get; }

    /// <summary>
    /// Constructor for RangeConstraint.
    /// </summary>
    /// <remarks>
    /// Easy-er is not to call this method directly but call <br/>
    /// -<see cref="RangeConstraint.Between{T}"/><br/>
    /// -<see cref="RangeConstraint.AtLeast{T}"/><br/>
    /// -<see cref="RangeConstraint.AtMost{T}"/><br/>
    /// -<see cref="RangeConstraint.Exactly{T}"/><br/>
    /// -<see cref="RangeConstraint.None"/><br/>
    /// i.c.w. <code>using static ConsoleAsksFor.RangeConstraint;</code>
    /// </remarks>
    /// <exception cref="InvalidRangeException"></exception>
    /// <exception cref="CannotValidateRangeException"></exception>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public RangeConstraint(T? min, T? max)
    {
        ThrowIfMinLargerThanMax(min, max);

        Min = min;
        Max = max;
    }

    private static void ThrowIfMinLargerThanMax(T? min, T? max)
    {
        if (!min.HasValue || !max.HasValue)
        {
            return;
        }

        if (min.Value is IComparable<T> comparable)
        {
            if (comparable.CompareTo(max.Value) > 0)
            {
                throw InvalidRangeException.Create(min.Value, max.Value);
            }
        }
        else if (RangeConstraintComparers.TryGetComparer<T>(out var comparer))
        {
            if (comparer.Compare(min.Value, max.Value) > 0)
            {
                throw InvalidRangeException.Create(min.Value, max.Value);
            }
        }
        else
        {
            throw CannotValidateRangeException.Create<T>();
        }
    }

    /// <summary>
    /// Implicit operator for converting <see cref="RangeConstraint"/> to <see cref="RangeConstraint{T}"></see> without min and without max.
    /// </summary>
    /// <param name="_"></param>
    /// <returns></returns>
    public static implicit operator RangeConstraint<T>(RangeConstraint _) => new(null, null);
}