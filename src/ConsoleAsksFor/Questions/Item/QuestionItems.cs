using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ConsoleAsksFor;

internal sealed class QuestionItems : IReadOnlyList<QuestionItem>
{
    private readonly IReadOnlyList<QuestionItem> _questionItems;
    private readonly StringComparer _comparer;

    public bool IsCaseSensitive { get; }

    private QuestionItems(
        IReadOnlyList<QuestionItem> questionItems,
        StringComparer comparer)
    {
        _questionItems = questionItems;
        _comparer = comparer;
    }

    public static QuestionItems CreateWithoutEscapedSplitter(IEnumerable<string> items)
        => new(items, false);

    public static QuestionItems CreateWithEscapedSplitter(IEnumerable<string> items)
        => new(items, true);

    private QuestionItems(IEnumerable<string> items, bool escapeSplitter)
    {
        var questionItems = items
            .Select(i => new QuestionItem(i, escapeSplitter))
            .ToList();

        var displayItems = questionItems
            .Select(x => x.Display)
            .ToList();

        ThrowWhenNotUniqueDisplayNames(displayItems);

        var isCaseSensitive = !displayItems.WhenIgnoringCaseItemsAreStillUnique();
        _questionItems = questionItems;

        IsCaseSensitive = isCaseSensitive;
        _comparer = isCaseSensitive
            ? StringComparer.InvariantCulture
            : StringComparer.InvariantCultureIgnoreCase;
    }

    public IEnumerable<string> Hints()
        => _questionItems
            .Select(x => x.Display)
            .Select(i => $"-{i}");

    public IEnumerable<string> Warnings()
    {
        var hasTrimmed = _questionItems.Any(x => x.IsTrimmed);
        var hasEscapedWhitespace = _questionItems.Any(x => x.HasEscapedWhitespace);
        var hasEscapedSplitter = _questionItems.Any(x => x.HasEscapedSplitter);
        var hasRemovedNotPrintableChars = _questionItems.Any(x => x.HasRemovedNotPrintableChars);
        var hasReplacedStringEmpty = _questionItems.Any(x => x.IsReplacedStringEmpty);
        var hasAlteredItems = hasTrimmed ||
                              hasEscapedWhitespace ||
                              hasEscapedSplitter ||
                              hasRemovedNotPrintableChars ||
                              hasReplacedStringEmpty;

        return Enumerable.Empty<string>()
            .ConditionalAppend(hasEscapedSplitter, $"One or more items contained splitter char {Splitter.Value}, those chars are replaced with {Splitter.EscapedValue}.")
            .ConditionalAppend(hasTrimmed, "One or more items contained trailing/leading spaces, those items have been trimmed.")
            .ConditionalAppend(hasEscapedWhitespace, "One or more items contained non-visible whitespace, which has been made visible.")
            .ConditionalAppend(hasRemovedNotPrintableChars, "One or more items contained not printable chars, those chars are removed.")
            .ConditionalAppend(hasReplacedStringEmpty, $"One item was empty and replaced with {QuestionItem.StringEmptyReplacement}.")
            .ConditionalAppend(hasAlteredItems, "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...");
    }

    private static void ThrowWhenNotUniqueDisplayNames(IEnumerable<string> items)
    {
        var notUniqueDisplayNames = items
            .GetDuplicateItems()
            .Select(x => new NotUniqueDisplayNamesException.NotUniqueDisplayName(x.Key, x.Select(i => i.Index).ToList()))
            .ToList();

        if (notUniqueDisplayNames.Any())
        {
            throw new NotUniqueDisplayNamesException(notUniqueDisplayNames);
        }
    }

    public bool TryParse(
        string answerAsString,
        [MaybeNullWhen(false)] out string answer)
    {
        var item = _questionItems.SingleOrDefault(x => _comparer.Equals(x.Display, answerAsString.Trim()));
        answer = item?.RealValue;
        return item is not null;
    }

    public bool TryParse(
        IEnumerable<string> answerAsStrings,
        [MaybeNullWhen(false)] out IReadOnlyCollection<string> answer)
    {
        var possibleAnswer = answerAsStrings
            .Select(i => i.Trim())
            .Select(i => _questionItems.SingleOrDefault(x => _comparer.Equals(x.Display, i)))
            .ToList();

        var isParsed = possibleAnswer.All(i => i is not null) &&
                       !possibleAnswer.GetDuplicateItems().Any();

        answer = isParsed
            ? possibleAnswer
                .WhereNotNull()
                .Select(i => i.RealValue)
                .ToList()
            : null;

        return isParsed;
    }

    public QuestionItems ExceptAnswers(IReadOnlyCollection<string> answer)
    {
        var newQuestionItems = _questionItems
            .Where(x => !answer.Contains(x.RealValue))
            .ToList();

        return new QuestionItems(newQuestionItems, _comparer);
    }

    public QuestionItems GetWhereDisplaysStartsWith(string hint)
    {
        var newQuestionItems = _questionItems
            .Where(e => e.Display.StartsWith(hint.Trim(), _comparer))
            .ToList();

        return new QuestionItems(newQuestionItems, _comparer);
    }

    public string FormatAnswer(string answer)
        => _questionItems.Single(kvp => kvp.RealValue == answer).Display;

    public int GetIndexOfDisplayValue(string displayValue)
    {
        var trimmedDisplayValue = displayValue.Trim();
        var indexedItem = _questionItems
            .GetIndexedItems()
            .SingleOrDefault(i => _comparer.Equals(i.Item.Display, trimmedDisplayValue));

        return indexedItem?.Index ?? -1;
    }

    public IEnumerator<QuestionItem> GetEnumerator()
        => _questionItems.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public int Count
        => _questionItems.Count;

    public QuestionItem this[int index]
        => _questionItems[index];
}