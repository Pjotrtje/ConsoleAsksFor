using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests;

public class DecimalExtensionsTests
{
    public static TheoryData<decimal, int, decimal, string> TruncateMinValueUseCases()
        => new()
        {
            {
                1.129m,
                2,
                1.13m,
                "Truncate"
            },
            {
                1.121m,
                2,
                1.13m,
                "Truncate, not round"
            },
        };

    [Theory]
    [MemberData(nameof(TruncateMinValueUseCases))]
    public void TruncateMinValue(decimal input, int digitsAfterDecimalPoint, decimal expectedResult, string useCase)
    {
        var dateTimeOffset = input.TruncateMinValue(digitsAfterDecimalPoint);
        dateTimeOffset.Should().Be(expectedResult, useCase);
    }

    public static TheoryData<decimal, int, decimal, string> TruncateMaxValueUseCases()
        => new()
        {
            {
                1.129m,
                2,
                1.12m,
                "Truncate not round"
            },
            {
                1.121m,
                2,
                1.12m,
                "Truncate"
            },
        };

    [Theory]
    [MemberData(nameof(TruncateMaxValueUseCases))]
    public void TruncateMaxValue(decimal input, int digitsAfterDecimalPoint, decimal expectedResult, string useCase)
    {
        var dateTimeOffset = input.TruncateMaxValue(digitsAfterDecimalPoint);
        dateTimeOffset.Should().Be(expectedResult, useCase);
    }
}