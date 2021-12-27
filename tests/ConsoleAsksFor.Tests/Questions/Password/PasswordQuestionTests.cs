using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests;

public class PasswordQuestionTests
{
    private const string QuestionText = "Some Question";

    [Fact]
    public void Has_Correct_Guidance()
    {
        var question = new PasswordQuestion(QuestionText);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEmpty();
    }

    [Fact]
    public void MustObfuscateAnswer()
    {
        var question = new PasswordQuestion(QuestionText);
        question.MustObfuscateAnswer.Should().BeTrue();
    }

    [Theory]
    [InlineData(" ", "Space")]
    [InlineData("A", "Digit")]
    [InlineData("Skjhasfjkhsdfhjksdf", "Random shizzle")]
    public void Parses_When_Correct_A_Value(string answerAsString, string useCase)
    {
        var question = new PasswordQuestion(QuestionText);
        var isParsed = question.TryParse(answerAsString, out _, out var answer);
        isParsed.Should().BeTrue(useCase);
        answer.Should().Be(answerAsString);
    }

    [Fact]
    public void Does_Not_Parse_When_Incorrect_No_Value()
    {
        var question = new PasswordQuestion(QuestionText);
        var isParsed = question.TryParse("", out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().BeEquivalentTo("Password missing.");
    }
}