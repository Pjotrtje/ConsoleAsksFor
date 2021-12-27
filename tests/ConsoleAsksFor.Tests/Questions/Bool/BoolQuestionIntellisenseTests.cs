namespace ConsoleAsksFor.Tests;

public class BoolQuestionIntellisenseTests
{
    private readonly BoolQuestionIntellisense _sut = new();

    private static readonly IntellisenseUseCases UseCases = new()
    {
        FromUser = new List<IntellisenseUseCases.FromUserInput>
        {
            new() { Input = "",     Previous = "n",  Next = "y",  UseCase = "No value" },
            new() { Input = " ",    Previous = "n",  Next = "y",  UseCase = "Whitespace" },
            new() { Input = "y",    Previous = null, Next = null, UseCase = "Valid value" },
            new() { Input = " y ",  Previous = null, Next = null, UseCase = "Valid value with whitespace" },
            new() { Input = "lala", Previous = null, Next = null, UseCase = "Invalid value" },
        },
        FromIntellisense = new List<IntellisenseUseCases.FromIntellisenseInput>
        {
            new() { Input = "n",   Hint = "",  Previous = "y", Next = "y", UseCase = "Valid false value" },
            new() { Input = "y",   Hint = "",  Previous = "n", Next = "n", UseCase = "Valid true value" },
            new() { Input = "y",   Hint = " ", Previous = "n", Next = "n", UseCase = "Valid true value with whitespace around hint " },
            new() { Input = " y ", Hint = "",  Previous = "n", Next = "n", UseCase = "Valid true value with whitespace around input " },
        },
    };

    public static TheoryData<string, string, string?> CompleteValueUseCases => UseCases.CompleteValueUseCases;

    public static TheoryData<string, string, string, string?> PreviousValueUseCases => UseCases.PreviousValueUseCases;

    public static TheoryData<string, string, string, string?> NextValueUseCases => UseCases.NextValueUseCases;

    [Theory]
    [MemberData(nameof(CompleteValueUseCases))]
    public void CompleteValue_Returns_CorrectValue(string useCase, string value, string? newValue)
    {
        _sut.CompleteValue(value).Should().Be(newValue, useCase);
    }

    [Theory]
    [MemberData(nameof(PreviousValueUseCases))]
    public void PreviousValue_Returns_CorrectValue(string useCase, string value, string hint, string? newValue)
    {
        _sut.PreviousValue(value, hint).Should().Be(newValue, useCase);
    }

    [Theory]
    [MemberData(nameof(NextValueUseCases))]
    public void NextValue_Returns_CorrectValue(string useCase, string value, string hint, string? newValue)
    {
        _sut.NextValue(value, hint).Should().Be(newValue, useCase);
    }
}