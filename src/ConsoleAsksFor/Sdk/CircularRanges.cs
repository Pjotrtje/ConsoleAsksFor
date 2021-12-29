namespace ConsoleAsksFor.Sdk;

/// <summary>
/// ToDo
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