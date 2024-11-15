namespace ConsoleAsksFor;

internal static class EnumExtensions
{
    public static T Next<T>(this T current)
        where T : struct, Enum
        => Enum
            .GetValues<T>()
            .GetValueAfter(current);

    private static T GetValueAfter<T>(this IReadOnlyCollection<T> values, T current)
        where T : struct, Enum
        => values
            .Append(values.First())
            .SkipWhile(e => !current.Equals(e))
            .Skip(1)
            .First();
}
