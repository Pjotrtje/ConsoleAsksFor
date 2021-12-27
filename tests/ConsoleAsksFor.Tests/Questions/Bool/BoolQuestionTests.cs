using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests;

public class BoolQuestionTests
{
    private const string QuestionText = "Some Question";

    [Fact]
    public void Has_Correct_Guidance()
    {
        var question = new BoolQuestion(QuestionText, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo("Select y/n.");
    }

    [Fact]
    public void When_No_Default_Value_Has_No_PrefilledValue()
    {
        var question = new BoolQuestion(QuestionText, null);
        question.PrefilledValue.Should().BeEmpty();
    }

    [Theory]
    [InlineData(true, "y")]
    [InlineData(false, "n")]
    public void When_Default_Value_Has_Correct_PrefilledValue(bool defaultValue, string prefilledValue)
    {
        var question = new BoolQuestion(QuestionText, defaultValue);
        question.PrefilledValue.Should().Be(prefilledValue);
    }

    [Theory]
    [InlineData("y", true, "Lower y")]
    [InlineData("Y", true, "Upper y")]
    [InlineData("n", false, "Lower n")]
    [InlineData("N", false, "Upper n")]
    [InlineData(" N ", false, "N with whitespace")]
    [InlineData(" Y ", true, "Y with whitespace")]
    public void Parses_When_Correct_Value(string answerAsString, bool expectedAnswer, string useCase)
    {
        var question = new BoolQuestion(QuestionText, null);
        var isParsed = question.TryParse(answerAsString, out _, out var answer);
        isParsed.Should().BeTrue(useCase);
        answer.Should().Be(expectedAnswer);
    }

    [Theory]
    [InlineData("", "Missing")]
    [InlineData("j", "Dutch")]
    [InlineData("Yes", "Full true word")]
    [InlineData("1", "True bit")]
    [InlineData("No", "Full false word")]
    [InlineData("0", "False bit")]
    public void Does_Not_Parse_When_Incorrect_Value(string answerAsString, string useCase)
    {
        var question = new BoolQuestion(QuestionText, null);
        var isParsed = question.TryParse(answerAsString, out var errors, out _);
        isParsed.Should().BeFalse(useCase);
        errors.Should().BeEmpty();
    }
}