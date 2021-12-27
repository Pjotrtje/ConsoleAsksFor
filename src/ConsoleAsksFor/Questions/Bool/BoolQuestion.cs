using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal sealed class BoolQuestion : IQuestion<bool>
{
    public string? SubType => null;

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; } = new BoolQuestionIntellisense();

    public string Text { get; }

    public string PrefilledValue => _defaultValue is null
        ? ""
        : FormatAnswer(_defaultValue.Value);

    private readonly bool? _defaultValue;

    public BoolQuestion(
        string text,
        bool? defaultValue)
    {
        Text = text;
        _defaultValue = defaultValue;
    }

    public IEnumerable<string> GetHints()
    {
        yield return "Select y/n.";
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out bool answer)
    {
        var trimmedAnswerAsString = answerAsString.Trim();
        errors = Enumerable.Empty<string>();
        if (trimmedAnswerAsString.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            answer = true;
            return true;
        }

        if (trimmedAnswerAsString.Equals("n", StringComparison.OrdinalIgnoreCase))
        {
            answer = false;
            return true;
        }

        answer = false;
        return false;
    }

    public string FormatAnswer(bool answer)
        => answer switch
        {
            false => "n",
            true => "y",
        };
}