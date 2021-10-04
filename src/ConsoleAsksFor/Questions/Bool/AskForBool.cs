using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="bool"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> AskForBool(
            this IConsole console,
            string questionText,
            bool? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var question = new BoolQuestion(
                questionText,
                defaultValue);

            return await console.Ask(question, cancellationToken);
        }
    }
}