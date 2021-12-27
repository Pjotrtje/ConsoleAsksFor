using System.Collections.Generic;
using System.Linq;

namespace ConsoleAsksFor;

internal static class EnumerableCharExtensions
{
    public static string JoinToString(this IEnumerable<char> items)
        => new(items.ToArray());
}