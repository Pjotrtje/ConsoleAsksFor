using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="string"/>, but obfuscate answer.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string> AskForPassword(
            this IConsole console,
            string questionText,
            CancellationToken cancellationToken = default)
        {
            var question = new PasswordQuestion(questionText);
            return await console.Ask(question, cancellationToken);
        }
    }
}