using System;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class ItemsQuestionIntellisense : IIntellisense
    {
        private readonly QuestionItems _items;

        public ItemsQuestionIntellisense(QuestionItems items)
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
            var subAnswers = value
                .Split(Splitter.Value)
                .ToList();

            var allPotentiallyCompletedSubAnswers = subAnswers.Count == 1
                ? Array.Empty<string>()
                : subAnswers.SkipLast(1).ToArray();

            if (!_items.TryParse(allPotentiallyCompletedSubAnswers, out var allCompletedSubAnswers))
            {
                return null;
            }

            var lastItemQuestionItems = _items.ExceptAnswers(allCompletedSubAnswers);
            var lastItemIntellisense = new ItemQuestionIntellisense(lastItemQuestionItems);
            var lastItemHint = hint
                .Split(Splitter.Value)
                .Last();

            var lastItemValue = subAnswers.Last();
            var addition = direction switch
            {
                IntellisenseDirection.Next => lastItemIntellisense.NextValue(lastItemValue, lastItemHint),
                IntellisenseDirection.Previous => lastItemIntellisense.PreviousValue(lastItemValue, lastItemHint),
                _ => lastItemIntellisense.CompleteValue(lastItemValue),
            };

            var newValue = allCompletedSubAnswers
                .Select(_items.FormatAnswer)
                .Append(addition ?? lastItemValue.Trim())
                .JoinStrings(Splitter.DisplayValue);

            return newValue != value
                ? newValue
                : null;
        }
    }
}