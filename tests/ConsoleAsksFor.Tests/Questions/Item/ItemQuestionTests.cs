namespace ConsoleAsksFor.Tests;

public class ItemQuestionTests
{
    private const string QuestionText = "Some Question";

    [Fact]
    public void Ctor_Throws_When_Not_All_Items_Are_Unique()
    {
        var items = new[]
        {
            "A",
            "B",
            "C",
            "D",
            "A",
            "C",
            "A",
        };

        Action ctor = () => _ = new ItemQuestion(QuestionText, items, null);

        ctor.Should().Throw<NotUniqueDisplayNamesException>()
            .Which.NotUniqueDisplayNames.Should().BeEquivalentTo(
            [
                new NotUniqueDisplayNamesException.NotUniqueDisplayName("A", [0, 4, 6]),
                new NotUniqueDisplayNamesException.NotUniqueDisplayName("C", [2, 5]),
            ]);
    }

    [Fact]
    public void Ctor_Throws_When_No_Items()
    {
        Action ctor = () => _ = new ItemQuestion(QuestionText, [], null);

        ctor.Should().Throw<MissingItemsException>();
    }

    [Fact]
    public void Given_CaseInSensitive_Has_Correct_Guidance()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemQuestion(QuestionText, items, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Select one of the following (case insensitive):",
            "-Item1",
            "-Item2");
    }

    [Fact]
    public void Given_CaseSensitive_Has_Correct_Guidance()
    {
        var items = new[] { "item2", "ITEM2" };
        var question = new ItemQuestion(QuestionText, items, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Select one of the following (case sensitive):",
            "-item2",
            "-ITEM2");
    }

    [Fact]
    public void Given_Warning_In_QuestionItems_Has_Correct_Guidance()
    {
        var items = new[] { "Item1 ", "Item2" };
        var question = new ItemQuestion(QuestionText, items, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Select one of the following (case insensitive):",
            "One or more items contained trailing/leading spaces, those items have been trimmed.",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...",
            "-Item1",
            "-Item2");
    }

    [Fact]
    public void When_No_Default_Value_Has_No_PrefilledValue()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemQuestion(QuestionText, items, null);
        question.PrefilledValue.Should().BeEmpty();
    }

    [Fact]
    public void When_Valid_Default_Value_Has_Correct_PrefilledValue()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemQuestion(QuestionText, items, "Item2");
        question.PrefilledValue.Should().Be("Item2");
    }

    [Fact]
    public void When_Invalid_Default_Value_Has_No_PrefilledValue()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemQuestion(QuestionText, items, "Item3");
        question.PrefilledValue.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Item2", "Item2", "Regular")]
    [InlineData("item2", "Item2", "Lower case")]
    [InlineData("ItEm2", "Item2", "Random case")]
    [InlineData("Item2 ", "Item2", "Trailing space")]
    [InlineData(" Item2", "Item2", "Leading space")]
    public void Given_CaseInSensitive_Parses_When_Correct_Value(string answerAsString, string expectedAnswer, string useCase)
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemQuestion(QuestionText, items, null);
        var isParsed = question.TryParse(answerAsString, out _, out var answer);
        isParsed.Should().BeTrue(useCase);
        answer.Should().Be(expectedAnswer);
    }

    [Fact]
    public void Given_CaseInSensitive_Does_NotParse_When_Not_Existing_Item()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemQuestion(QuestionText, items, null);
        var isParsed = question.TryParse("Item3", out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Given_CaseSensitive_Parses_When_Correct_Value()
    {
        var items = new[] { "item2", "ITEM2" };
        var question = new ItemQuestion(QuestionText, items, null);
        var isParsed = question.TryParse("ITEM2", out _, out var answer);
        isParsed.Should().BeTrue();
        answer.Should().Be("ITEM2");
    }

    [Theory]
    [InlineData("item2", "Lower case")]
    [InlineData("ItEm2", "Random case")]
    public void Given_CaseSensitive_Does_NotParse_When_Invalid_Casing(string answerAsString, string useCase)
    {
        var items = new[] { "Item2", "ITEM2" };
        var question = new ItemQuestion(QuestionText, items, null);
        var isParsed = question.TryParse(answerAsString, out var errors, out _);
        isParsed.Should().BeFalse(useCase);
        errors.Should().BeEmpty();
    }
}
