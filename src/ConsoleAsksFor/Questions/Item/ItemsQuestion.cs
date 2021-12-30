namespace ConsoleAsksFor;

internal sealed class ItemsQuestion : IQuestion<IReadOnlyCollection<string>>
{
    private readonly Range<int> _amountOfItemsToSelect;
    public string? SubType => null;

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense => new ItemsQuestionIntellisense(_items);

    public string Text { get; }

    public string PrefilledValue => _defaultValue ?? "";

    private readonly QuestionItems _items;
    private readonly string? _defaultValue;

    public ItemsQuestion(
        string text,
        IEnumerable<string> items,
        RangeConstraint<int> amountOfItemsToSelect,
        IEnumerable<string>? defaultValue)
    {
        var questionItems = QuestionItems.CreateWithEscapedSplitter(items);
        if (!questionItems.Any())
        {
            throw new MissingItemsException();
        }

        _defaultValue = defaultValue
            .EmptyWhenNull()
            .Select(x => questionItems.TryParse(x, out var answer)
                ? questionItems.FormatAnswer(answer)
                : null)
            .WhereNotNull()
            .JoinStrings(Splitter.DisplayValue);

        var allowedAmountOfItemsToSelect = new Range<int>(0, questionItems.Count);
        var inputAmountOfItemsToSelect = GetInputAmountOfItemsToSelect(amountOfItemsToSelect, allowedAmountOfItemsToSelect);

        _amountOfItemsToSelect = allowedAmountOfItemsToSelect.HasOverlap(inputAmountOfItemsToSelect, out var overlappingRange)
            ? overlappingRange
            : throw InvalidRangeException.Create(inputAmountOfItemsToSelect, allowedAmountOfItemsToSelect);

        _items = questionItems;
        Text = text;
    }

    private static Range<int> GetInputAmountOfItemsToSelect(RangeConstraint<int> range, Range<int> allowedRange)
    {
        var min = range.Min ?? allowedRange.Min;
        var max = range.Max ?? allowedRange.Max;
        return new Range<int>(min, max);
    }

    public IEnumerable<string> GetHints()
    {
        var casingHint = _items.IsCaseSensitive
            ? "case sensitive"
            : "case insensitive";

        return _items.Hints()
            .Concat(_items.Warnings())
            .Prepend($"Select {AmountOfItemsToSelectAsString()} of the following ({casingHint}; use {Splitter.Value} to separate items):");
    }

    private string AmountOfItemsToSelectAsString()
    {
        if (_amountOfItemsToSelect.Min == _amountOfItemsToSelect.Max)
        {
            return $"{_amountOfItemsToSelect.Min}";
        }

        if (_amountOfItemsToSelect.Min == 0 && _amountOfItemsToSelect.Max == _items.Count)
        {
            return "0 or more";
        }

        if (_amountOfItemsToSelect.Min > 0 && _amountOfItemsToSelect.Max < _items.Count)
        {
            return $"between {_amountOfItemsToSelect.Min} and {_amountOfItemsToSelect.Max}";
        }

        if (_amountOfItemsToSelect.Min > 0)
        {
            return $"at least {_amountOfItemsToSelect.Min}";
        }

        return $"at most {_amountOfItemsToSelect.Max}";
    }

    public bool TryParse(
        string answerAsString,
        out IEnumerable<string> errors,
        [MaybeNullWhen(false)] out IReadOnlyCollection<string> answer)
    {
        if (answerAsString.Trim() == "")
        {
            var noItemsAllowed = _amountOfItemsToSelect.Contains(0);
            answer = noItemsAllowed
                ? Array.Empty<string>()
                : null;

            errors = noItemsAllowed
                ? Enumerable.Empty<string>()
                : new[] { $"Select {AmountOfItemsToSelectAsString()} items." };

            return noItemsAllowed;
        }

        var answerAsStrings = answerAsString.Split(Splitter.Value);
        if (!_items.TryParse(answerAsStrings, out var possibleAnswer))
        {
            errors = Enumerable.Empty<string>();
            answer = null;
            return false;
        }

        var itemCountAllowed = _amountOfItemsToSelect.Contains(possibleAnswer.Count);
        answer = itemCountAllowed
            ? possibleAnswer
            : null;

        errors = itemCountAllowed
            ? Enumerable.Empty<string>()
            : new[] { $"Select {AmountOfItemsToSelectAsString()} items." };

        return itemCountAllowed;
    }

    public string FormatAnswer(IReadOnlyCollection<string> answer)
        => answer
            .Select(a => _items.FormatAnswer(a))
            .JoinStrings(Splitter.DisplayValue);
}