using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal sealed class ItemQuestionIntellisense : IIntellisense
{
    private readonly QuestionItems _items;

    public ItemQuestionIntellisense(QuestionItems items)
    {
        _items = items;
    }

    public string? CompleteValue(string value)
        => Handle(value, value, IntellisenseDirection.None);

    public string? PreviousValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Previous);

    public string? NextValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Next);

    private string? Handle(string value, string hint, IntellisenseDirection direction)
    {
        var subItems = _items.GetWhereDisplaysStartsWith(hint);
        if (!subItems.Any())
        {
            return null;
        }

        var toSelectIndex = direction switch
        {
            IntellisenseDirection.Previous => subItems.GetIndexOfDisplayValue(value) - 1,
            IntellisenseDirection.Next => subItems.GetIndexOfDisplayValue(value) + 1,
            _ => 0,
        };

        string? GetIfChanged(QuestionItem newValue)
            => newValue.Display == value
                ? null
                : newValue.Display;

        return toSelectIndex switch
        {
            < 0 => GetIfChanged(subItems[^1]),
            _ when toSelectIndex >= subItems.Count => GetIfChanged(subItems[0]),
            _ => GetIfChanged(subItems[toSelectIndex]),
        };
    }
}