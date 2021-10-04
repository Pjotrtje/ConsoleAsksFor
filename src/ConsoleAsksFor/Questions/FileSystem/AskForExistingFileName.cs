using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for existing <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<FileInfo> AskForExistingFileName(
            this IConsole console,
            string questionText,
            FileInfo? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var question = new FileNameQuestion(
                questionText,
                FileSystemExistence.Existing,
                defaultValue);

            return await console.Ask(question, cancellationToken);
        }
    }
}