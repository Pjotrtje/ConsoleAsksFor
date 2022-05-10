namespace ConsoleAsksFor.Sdk;

/// <summary>
/// Helper class to generate consistent hints for <see cref="IQuestion{TAnswer}"/>.
/// </summary>
public static class Hint
{
    /// <summary>
    /// Creates range hint.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="range"></param>
    /// <param name="formatter"></param>
    /// <returns></returns>
    public static string ForRange<T>(Range<T> range, Func<T, string> formatter) where T : struct, IComparable<T>
        => $"Range: [{formatter(range.Min)} ... {formatter(range.Max)}]";

    /// <summary>
    /// Creates range hint.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="range"></param>
    /// <param name="formatter"></param>
    /// <returns></returns>
    public static string ForRange<T>(ClusteredRange<T> range, Func<T, string> formatter) where T : struct, IComparable<T>
        => $"Range: [{formatter(range.Min())} ... {formatter(range.Max())}]";

    /// <summary>
    /// Creates format hint.
    /// </summary>
    /// <param name="formatDescription"></param>
    /// <returns></returns>
    public static string ForFormat(string formatDescription)
        => $"Format: {formatDescription}";

    /// <summary>
    /// Creates unit hint.
    /// </summary>
    /// <param name="unitDescription"></param>
    /// <returns></returns>
    public static string ForUnit(string unitDescription)
        => $"Unit: {unitDescription}";
}
