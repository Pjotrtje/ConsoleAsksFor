using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAsksFor;

internal static class EnumerableOfStringExtensions
{
    public static bool WhenIgnoringCaseItemsAreStillUnique(this IEnumerable<string> items)
        => !items
            .GroupBy(
                x => x,
                StringComparer.InvariantCultureIgnoreCase)
            .Any(x => x.Count() > 1);
}