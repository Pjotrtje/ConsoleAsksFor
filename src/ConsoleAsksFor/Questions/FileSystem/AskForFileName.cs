using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<FileInfo> AskForFileName(
            this IConsole console,
            string questionText,
            FileInfo? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var fileSystemExistence = FileSystemExistence.NewOrExisting;
            var question = new FileNameQuestion(
                questionText,
                fileSystemExistence,
                defaultValue);

            return await console.Ask(question, cancellationToken);
        }
    }
}