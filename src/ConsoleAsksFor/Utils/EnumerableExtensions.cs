using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAsksFor
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<IGrouping<T, IndexedItem<T>>> GetDuplicateItems<T>(this IEnumerable<T> items)
            => items
                .GetIndexedItems()
                .GroupBy(x => x.Item)
                .Where(x => x.Count() > 1);

        public static IEnumerable<IndexedItem<T>> GetIndexedItems<T>(this IEnumerable<T> items)
            => items
                .Select((item, index) => new IndexedItem<T>(item, index));

        public static string JoinAsStrings<T>(this IEnumerable<T> items, Func<T, string> toStringFunc, string separator)
            => items.Select(toStringFunc).JoinStrings(separator);

        public static string JoinAsStrings<T>(this IEnumerable<T> items, string separator)
            where T : notnull
            => items.JoinAsStrings(i => i.ToString()!, separator);

        public static string JoinStrings(this IEnumerable<string> strings, string separator)
            => string.Join(separator, strings);

        public static IEnumerable<T> ConditionalAppend<T>(this IEnumerable<T> items, bool condition, T toAppend)
            => condition
                ? items.Append(toAppend)
                : items;

        public static IEnumerable<T> EmptyWhenNull<T>(this IEnumerable<T>? items)
            where T : class
                => items ?? Enumerable.Empty<T>();

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
            where T : class
            => items
                .Where(i => i is not null)
                .Select(i => i!);
    }
}