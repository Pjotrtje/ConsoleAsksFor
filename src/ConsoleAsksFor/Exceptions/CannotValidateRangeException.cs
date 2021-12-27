using System;
using System.Collections;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

/// <summary>
/// An exception for when validity of range can not be determined.
/// </summary>
public sealed class CannotValidateRangeException : Exception
{
    internal static CannotValidateRangeException Create<T>()
        where T : notnull
        => new(typeof(T));

    private CannotValidateRangeException(Type type)
        : base($"{type} does not implement {nameof(IComparable)}<T> and no custom {nameof(IComparer)}<T> is registered at {nameof(RangeConstraintComparers)}.")
    {
    }
}