using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal sealed class ItemQuestion : IQuestion<string>
{
    public string? SubType => null;

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense => new ItemQuestionIntellisense(_items);

    public string Text { get; }

    public string PrefilledValue => _defaultValue ?? "";

    private readonly string? _defaultValue;
    private readonly QuestionItems _items;

    public ItemQuestion(
        string text,
        IEnumerable<string> items,
        string? defaultValue)
    {
        var questionItems = QuestionItems.CreateWithoutEscapedSplitter(items);
        if (!questionItems.Any())
        {
            throw new MissingItemsException();
        }

        _defaultValue = questionItems.TryParse(defaultValue ?? "", out var answer)
            ? questionItems.FormatAnswer(answer)
            : "";

        Text = text;
        _items = questionItems;
    }

    public IEnumerable<string> GetHints()
    {
        var casingHint = _items.IsCaseSensitive
            ? "case sensitive"
            : "case insensitive";

        return _items.Hints()
            .Concat(_items.Warnings())
            .Prepend($"Select one of the following ({casingHint}):");
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, [MaybeNullWhen(false)] out string answer)
    {
        errors = Enumerable.Empty<string>();
        return _items.TryParse(answerAsString.Trim(), out answer);
    }

    public string FormatAnswer(string answer)
        => _items.FormatAnswer(answer);
}