using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="decimal"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="scale"></param>
        /// <param name="range"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>
        /// The allowed min/max of decimal is lowered by factor 10^(2+scale) so Intellisense will work. In theory you can provide a <paramref name="range"/> which is fully out of range of the allowed range. <br/>
        /// If so <see cref="InvalidRangeException"/> is thrown.
        /// </remarks>
        /// <exception cref="InvalidRangeException"></exception>
        /// <returns></returns>
        public static async Task<decimal> AskForDecimal(
            this IConsole console,
            string questionText,
            Scale scale,
            RangeConstraint<decimal>? range = null,
            decimal? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var question = new DecimalQuestion(
                questionText,
                scale,
                range ?? RangeConstraint.None,
                defaultValue);

            return await console.Ask(question, cancellationToken);
        }
    }
}