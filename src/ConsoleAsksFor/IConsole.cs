using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    /// <summary>
    /// Main interface for using ConsoleAsksFor.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Ask a question an re-ask while answer is invalid.
        /// </summary>
        /// <typeparam name="TAnswer"></typeparam>
        /// <param name="question"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TAnswer> Ask<TAnswer>(IQuestion<TAnswer> question, CancellationToken cancellationToken)
            where TAnswer : notnull;

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.Success" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteSuccessLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.Warning" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteWarningLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.Error" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteErrorLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.Info" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteInfoLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.Question" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteQuestionLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.QuestionHint" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteQuestionHintLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.Answer" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteAnswerLine(string value);

        /// <summary>
        /// Write line in the color <see cref="ConsoleColors.InvalidAnswer" />.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteInvalidAnswerLine(string value);

        /// <summary>
        /// Write all help texts so user can read all shortcuts etc.
        /// </summary>
        void WriteHelpTextLines();
    }
}