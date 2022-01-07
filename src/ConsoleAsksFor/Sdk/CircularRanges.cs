namespace ConsoleAsksFor.Sdk;

/// <summary>
/// Some constructs like DateOnly implement <see cref="IComparable{T}"/>, but are really circular.
/// A range of [23:00 ... 01:00] is also a valid range...
/// Here we can register such constructs so when creating ranges with <see cref="RangeConstraint"/> no exceptions are thrown.
/// </summary>
public static class CircularRanges
{
    private static readonly HashSet<Type> Types = new();

    /// <summary>
    /// Register <see cref="IComparable{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void RegisterCircularRange<T>()
        where T : IComparable<T>
    {
        Types.Add(typeof(T));
    }

    internal static bool IsCircular<T>()
        => Types.Contains(typeof(T));
}