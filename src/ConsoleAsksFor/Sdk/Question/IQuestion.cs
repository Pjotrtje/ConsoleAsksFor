using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleAsksFor.Sdk;

/// <summary>
/// Interface for asking question with <see cref="IConsole"/>.
/// </summary>
/// <typeparam name="TAnswer"></typeparam>
public interface IQuestion<TAnswer>
    where TAnswer : notnull
{
    /// <summary>
    /// SubType Identifier for grouping answers in history.
    /// </summary>
    string? SubType { get; }

    /// <summary>
    /// Whether or not answer should be obfuscated. When obfuscated answers are not stored in history.
    /// </summary>
    bool MustObfuscateAnswer { get; }

    /// <summary>
    /// Tab (with/without Ctrl/Shift) and Ctrl+Space behaviour.
    /// </summary>
    IIntellisense Intellisense { get; }

    /// <summary>
    /// Question text which is asked before getting answer for question.
    /// </summary>
    string Text { get; }

    /// <summary>
    /// Default value for question as string.
    /// </summary>
    string PrefilledValue { get; }

    /// <summary>
    /// Hints/constraints to display before getting answer for question.
    /// </summary>
    /// <returns></returns>
    IEnumerable<string> GetHints();

    /// <summary>
    /// Converts <see cref="string"/> representation of <paramref name="answer"/> to <paramref name="answer"/>.
    /// </summary>
    /// <param name="answerAsString">The input as entered by user.</param>
    /// <param name="errors">When <paramref name="answerAsString"/> cannot be parsed (can be empty).</param>
    /// <param name="answer">The parsed answer.</param>
    /// <returns></returns>
    bool TryParse(
        string answerAsString,
        [MaybeNullWhen(true)] out IEnumerable<string> errors,
        [MaybeNullWhen(false)] out TAnswer answer);

    /// <summary>
    /// Gets <see cref="string"/> representation of  <paramref name="answer"/>.
    /// </summary>
    /// <param name="answer"></param>
    /// <returns></returns>
    string FormatAnswer(TAnswer answer);
}