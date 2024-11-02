namespace ConsoleAsksFor;

internal sealed class History
{
    private readonly int _maxSize;
    private readonly LinkedList<HistoryItem> _items;

    public IEnumerable<HistoryItem> Items => _items;

    public History(IEnumerable<HistoryItem> items, int maxSize)
    {
        _maxSize = maxSize;
        _items = new LinkedList<HistoryItem>(items);
    }

    public void Add(HistoryItem item)
    {
        _items.Remove(item);
        _items.AddLast(item);

        while (_items.Count > _maxSize)
        {
            _items.RemoveFirst();
        }
    }

    public ScopedHistory GetScopedHistory(HistoryType type, string questionType, string questionText)
    {
        var items = type switch
        {
            HistoryType.ByQuestionTextAndType => _items.Where(i => i.QuestionType == questionType && i.QuestionText == questionText),
            HistoryType.ByQuestionType => _items.Where(i => i.QuestionType == questionType),
            _ => _items,
        };

        var history = items
            .Select(i => i.Answer)
            .Reverse()
            .Distinct()
            .Reverse();

        return new ScopedHistory(history);
    }
}
