namespace ConsoleAsksFor.Tests;

public class ItemsQuestionTests
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

        Action ctor = () => _ = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);

        ctor.Should().Throw<NotUniqueDisplayNamesException>()
            .Which.NotUniqueDisplayNames.Should().BeEquivalentTo(new[]
            {
                new NotUniqueDisplayNamesException.NotUniqueDisplayName("A", new[] {0, 4, 6}),
                new NotUniqueDisplayNamesException.NotUniqueDisplayName("C", new[] {2, 5}),
            });
    }

    [Fact]
    public void Ctor_Throws_When_No_Items()
    {
        Action ctor = () => _ = new ItemsQuestion(QuestionText, Enumerable.Empty<string>(), RangeConstraint.None, null);

        ctor.Should().Throw<MissingItemsException>();
    }

    [Fact]
    public void Given_CaseInSensitive_Has_Correct_Guidance()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Select 0 or more of the following (case insensitive; use | to separate items):",
            "-Item1",
            "-Item2");
    }

    [Fact]
    public void Given_CaseSensitive_Has_Correct_Guidance()
    {
        var items = new[] { "item2", "ITEM2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Select 0 or more of the following (case sensitive; use | to separate items):",
            "-item2",
            "-ITEM2");
    }

    [Fact]
    public void Given_Warning_In_QuestionItems_Has_Correct_Guidance()
    {
        var items = new[] { "Item1 ", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Select 0 or more of the following (case insensitive; use | to separate items):",
            "One or more items contained trailing/leading spaces, those items have been trimmed.",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...",
            "-Item1",
            "-Item2");
    }

    [Fact]
    public void When_No_Default_Value_Has_No_PrefilledValue()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        question.PrefilledValue.Should().BeEmpty();
    }

    [Fact]
    public void When_Valid_Default_Value_Has_Correct_PrefilledValue()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, new[] { "Item1" });
        question.PrefilledValue.Should().Be("Item1");
    }

    [Fact]
    public void When_Valid_Default_Value_Has_Correct_PrefilledValues()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, items);
        question.PrefilledValue.Should().Be("Item1 | Item2");
    }

    [Fact]
    public void When_Invalid_Default_Value_Has_No_PrefilledValue()
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, new[] { "Item3" });
        question.PrefilledValue.Should().Be("");
    }

    public static TheoryData<string, IReadOnlyCollection<string>, string> CorrectParseUseCases()
    {
        return new()
        {
            { "Item2", new[] { "Item2" }, "SingleItem - Regular" },
            { "item2", new[] { "Item2" }, "SingleItem - Lower case" },
            { "ItEm2", new[] { "Item2" }, "SingleItem - Random case" },
            { "Item2 ", new[] { "Item2" }, "SingleItem - Trailing space" },
            { " Item2 ", new[] { "Item2" }, "SingleItem - Leading space" },
            { "Item1|Item2", new[] { "Item1", "Item2" }, "Multiple items - without spaces" },
            { "Item1 | Item2", new[] { "Item1", "Item2" }, "Multiple items - with spaces" },
        };
    }

    [Theory]
    [MemberData(nameof(CorrectParseUseCases))]
    public void Given_CaseInSensitive_Parses_When_Correct_Value(string answerAsString, IReadOnlyCollection<string> expectedAnswer, string useCase)
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        var isParsed = question.TryParse(answerAsString, out _, out var answer);
        isParsed.Should().BeTrue(useCase);
        answer.Should().BeEquivalentTo(expectedAnswer);
    }

    [Theory]
    [InlineData("Item3", "Not existing item")]
    [InlineData("Item1|", "Ending with splitter")]
    [InlineData("Item1|Item1", "Duplicate item")]
    public void Given_CaseInSensitive_Does_NotParse_When_Invalid_Item(string answerAsString, string useCase)
    {
        var items = new[] { "Item1", "Item2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        var isParsed = question.TryParse(answerAsString, out var errors, out _);
        isParsed.Should().BeFalse(useCase);
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Given_CaseSensitive_Parses_When_Correct_Value()
    {
        var items = new[] { "item2", "ITEM2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        var isParsed = question.TryParse("ITEM2", out _, out var answer);
        isParsed.Should().BeTrue();
        answer.Should().BeEquivalentTo("ITEM2");
    }

    [Theory]
    [InlineData("item2", "Lower case")]
    [InlineData("ItEm2", "Random case")]
    public void Given_CaseSensitive_Does_NotParse_When_Invalid_Casing(string answerAsString, string useCase)
    {
        var items = new[] { "Item2", "ITEM2" };
        var question = new ItemsQuestion(QuestionText, items, RangeConstraint.None, null);
        var isParsed = question.TryParse(answerAsString, out var errors, out _);
        isParsed.Should().BeFalse(useCase);
        errors.Should().BeEmpty();
    }
}