using System;
using System.Collections.Generic;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor.Tests;

public record TestQuestion : IQuestion<string>
{
    public string SubType => "SomeSubType";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; } = new NoIntellisense();

    public string Text { get; } = "SomeText";

    private readonly string _correctAnswer;

    public string PrefilledValue { get; init; } = "";

    public IReadOnlyCollection<string> Hints { get; init; } = Array.Empty<string>();

    public IReadOnlyCollection<string> ParseErrorsWhenIncorrectValue { get; init; } = Array.Empty<string>();

    public TestQuestion(string correctAnswer)
        => _correctAnswer = correctAnswer;

    public IEnumerable<string> GetHints()
        => Hints;

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out string answer)
    {
        errors = ParseErrorsWhenIncorrectValue;
        answer = _correctAnswer;
        return _correctAnswer == answerAsString;
    }

    public string FormatAnswer(string answer)
        => answer;
}