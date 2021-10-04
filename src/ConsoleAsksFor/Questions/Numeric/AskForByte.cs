using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="byte"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="range"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<byte> AskForByte(
            this IConsole console,
            string questionText,
            RangeConstraint<byte>? range = null,
            byte? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            return (byte)await console.AskForDecimal(
                questionText,
                Scale.Zero,
                ToDecimalRangeConstraint(range),
                defaultValue,
                cancellationToken);
        }

        private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<byte>? range)
            => RangeConstraint.Between<decimal>(
                range?.Min ?? byte.MinValue,
                range?.Max ?? byte.MaxValue);
    }
}