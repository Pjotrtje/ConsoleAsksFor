using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="string"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<string> AskForString(
        this IConsole console,
        string questionText,
        string? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new StringQuestion(
            questionText,
            null,
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }

    /// <summary>
    /// Ask for <see cref="string"/> and validate answer with a <see cref="Regex"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="regex"></param>
    /// <param name="hint"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<string> AskForString(
        this IConsole console,
        string questionText,
        Regex regex,
        string? hint = null,
        string? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new StringQuestion(
            questionText,
            (regex, hint),
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }
}