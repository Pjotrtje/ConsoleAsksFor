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
        /// Ask for zero or more item from <paramref name="items"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="items"></param>
        /// <param name="amountOfItemsToSelect"></param>
        /// <param name="defaultValues"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <exception cref="InvalidRangeException"></exception>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<string>> AskForItems(
            this IConsole console,
            string questionText,
            IEnumerable<string> items,
            RangeConstraint<int>? amountOfItemsToSelect = null,
            IEnumerable<string>? defaultValues = null,
            CancellationToken cancellationToken = default)
        {
            var question = new ItemsQuestion(
                questionText,
                items,
                amountOfItemsToSelect ?? RangeConstraint.None,
                defaultValues);

            return await console.Ask(question, cancellationToken);
        }

        /// <summary>
        /// Ask for zero or more item from <paramref name="items"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="items"></param>
        /// <param name="itemNameGetter"></param>
        /// <param name="amountOfItemsToSelect"></param>
        /// <param name="defaultValues"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <exception cref="InvalidRangeException"></exception>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<T>> AskForItems<T>(
            this IConsole console,
            string questionText,
            IEnumerable<T> items,
            Func<T, string> itemNameGetter,
            RangeConstraint<int>? amountOfItemsToSelect = null,
            IEnumerable<T>? defaultValues = null,
            CancellationToken cancellationToken = default)
        {
            var materializedItems = items.ToList();

            var names = materializedItems
                .Select(itemNameGetter); // Not to dictionary, because maybe name is not unique (later a nice exception is thrown)

            var stringDefaultValues = defaultValues?.Select(itemNameGetter);

            var itemNames = await console.AskForItems(
                questionText,
                names,
                amountOfItemsToSelect,
                stringDefaultValues,
                cancellationToken);

            return materializedItems
                .Where(x => itemNames.Contains(itemNameGetter(x)))
                .ToList();
        }

        /// <summary>
        /// Ask for zero or more item from <paramref name="namedItems"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="namedItems"></param>
        /// <param name="amountOfItemsToSelect"></param>
        /// <param name="defaultValues"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotUniqueDisplayNamesException"></exception>
        /// <exception cref="MissingItemsException"></exception>
        /// <exception cref="InvalidRangeException"></exception>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<T>> AskForItems<T>(
            this IConsole console,
            string questionText,
            IReadOnlyDictionary<string, T> namedItems,
            RangeConstraint<int>? amountOfItemsToSelect = null,
            IEnumerable<string>? defaultValues = null,
            CancellationToken cancellationToken = default)
        {
            var keys = await console.AskForItems(
                questionText,
                namedItems.Keys,
                amountOfItemsToSelect,
                defaultValues,
                cancellationToken);

            return keys
                .Select(k => namedItems[k])
                .ToList();
        }
    }
}