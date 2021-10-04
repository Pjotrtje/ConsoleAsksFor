using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for single item from <paramref name="items"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="items"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <returns></returns>
        public static async Task<string> AskForItem(
            this IConsole console,
            string questionText,
            IEnumerable<string> items,
            string? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var question = new ItemQuestion(
                questionText,
                items,
                defaultValue);

            return await console.Ask(question, cancellationToken);
        }

        /// <summary>
        /// Ask for single item from <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="items"></param>
        /// <param name="itemNameGetter"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <returns></returns>
        public static async Task<T> AskForItem<T>(
            this IConsole console,
            string questionText,
            IEnumerable<T> items,
            Func<T, string> itemNameGetter,
            T? defaultValue = null,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var materializedItems = items.ToList();

            var names = materializedItems
                .Select(itemNameGetter); // Not to dictionary, because maybe name is not unique (later a nice exception is thrown)

            var stringDefaultValue = defaultValue is not null
                ? itemNameGetter(defaultValue)
                : null;

            var itemName = await console.AskForItem(
                questionText,
                names,
                stringDefaultValue,
                cancellationToken);

            return materializedItems.Single(x => itemNameGetter(x) == itemName);
        }

        /// <summary>
        /// Ask for single item from <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="items"></param>
        /// <param name="itemNameGetter"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <returns></returns>
        public static async Task<T> AskForItem<T>(
            this IConsole console,
            string questionText,
            IEnumerable<T> items,
            Func<T, string> itemNameGetter,
            T? defaultValue = null,
            CancellationToken cancellationToken = default)
            where T : struct
        {
            var materializedItems = items.ToList();

            var names = materializedItems
                .Select(itemNameGetter); // Not to dictionary, because maybe name is not unique (later a nice exception is thrown)

            var stringDefaultValue = defaultValue.HasValue
                ? itemNameGetter(defaultValue.Value)
                : null;

            var itemName = await console.AskForItem(
                questionText,
                names,
                stringDefaultValue,
                cancellationToken);

            return materializedItems.Single(x => itemNameGetter(x) == itemName);
        }

        /// <summary>
        /// Ask for single item from <paramref name="namedItems"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="namedItems"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <returns></returns>
        public static async Task<T> AskForItem<T>(
            this IConsole console,
            string questionText,
            IReadOnlyDictionary<string, T> namedItems,
            string? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var itemName = await console.AskForItem(
                questionText,
                namedItems.Keys,
                defaultValue,
                cancellationToken);

            return namedItems[itemName];
        }
    }
}