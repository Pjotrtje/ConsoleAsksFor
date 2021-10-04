using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="Enum"/> which has <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="InvalidRangeException"></exception>
        /// <returns></returns>
        public static async Task<T> AskForFlaggedEnum<T>(
            this IConsole console,
            string questionText,
            T? defaultValue = null,
            CancellationToken cancellationToken = default)
            where T : struct, Enum
        {
            if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
            {
                throw new InvalidEnumArgumentException($"{typeof(T).Name} has no {nameof(FlagsAttribute)}");
            }

            var toAskEnums = Enum
                .GetValues<T>()
                .Where(v => IsPowerOfTwo(EnumAsInt(v)))
                .GroupBy(EnumAsInt)
                .Select(x => x.First())
                .ToList();

            var enumDefaultValues = defaultValue is null
                ? null
                : toAskEnums
                    .Where(k => HasFlag(defaultValue.Value, k))
                    .Select(x => x.ToString());

            var namedItems = toAskEnums
                .ToDictionary(
                    x => x.ToString(),
                    EnumAsInt);

            var result = await console
                .AskForItems(questionText, namedItems, null, enumDefaultValues, cancellationToken);

            return IntAsEnum<T>(result.Sum());
        }

        private static int EnumAsInt<T>(T value)
            where T : struct, Enum
            => (int)(object)value;

        private static T IntAsEnum<T>(int value)
            where T : struct, Enum
            => (T)(object)value;

        private static bool IsPowerOfTwo(int value)
            => value > 0 && (value & (value - 1)) == 0;

        // To separate method because compiler otherwise returns CA2248 error
        private static bool HasFlag(Enum defaultValue, Enum toAskItem)
            => defaultValue.HasFlag(toAskItem);
    }
}