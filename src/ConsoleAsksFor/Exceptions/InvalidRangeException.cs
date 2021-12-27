namespace ConsoleAsksFor;

/// <summary>
/// An exception for when range is invalid.
/// </summary>
public sealed class InvalidRangeException : Exception
{
    internal static InvalidRangeException Create<T>(T min, T max)
        where T : notnull
        => new(min, max);

    internal static InvalidRangeException Create<T>(Range<T> input, Range<T> allowedRange)
        where T : struct, IComparable<T>
        => new(input.Min, input.Max, allowedRange.Min, allowedRange.Max);

    private InvalidRangeException(object min, object max)
        : base($"Min: {min}, max: {max} is not a valid range.")
    {
    }

    private InvalidRangeException(object inputMin, object inputMax, object allowedRangeMin, object allowedRangeMax)
        : base($"No overlap in input range (min: {inputMin}, max: {inputMax}) and allowedRange (min: {allowedRangeMin}, max: {allowedRangeMax}).")
    {
    }
}