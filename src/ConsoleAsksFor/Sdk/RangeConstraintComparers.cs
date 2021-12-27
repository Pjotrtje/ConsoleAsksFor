using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleAsksFor.Sdk;

/// <summary>
/// In NodaTime ZonedDateTime does not implement <see cref="IComparable{T}"/> because it unclear how to compare. <br/>
/// It is comparable though, but how to compare depends on the use case. Here use case is clear. So this class we can register comparer.
/// (Maybe similar issues in other libraries?)
/// </summary>
public static class RangeConstraintComparers
{
    private static readonly ConcurrentDictionary<Type, object> Comparers = new();

    /// <summary>
    /// Register <see cref="IComparable{T}"/>.
    /// </summary>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    public static void RegisterComparer<T>(IComparer<T> comparer)
    {
        Comparers.AddOrUpdate(typeof(T), comparer, (_, _) => comparer);
    }

    internal static bool TryGetComparer<T>([MaybeNullWhen(false)] out IComparer<T> comparer)
    {
        var isFound = Comparers.TryGetValue(typeof(T), out var comparerObj);
        comparer = isFound
            ? (IComparer<T>)comparerObj!
            : null;

        return isFound;
    }
}