using ConsoleAsksFor.Sdk;

using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests.Sdk;

public class RangeTests
{
    public static TheoryData<Range<int>, Range<int>, Range<int>, string> OverlapUseCases()
    {
        return new()
        {
            { Range(0, 4), Range(2, 3), Range(2, 3), "Range 1 superset of range 2" },
            { Range(2, 3), Range(0, 4), Range(2, 3), "Range 2 superset of range 1" },
            { Range(0, 4), Range(2, 7), Range(2, 4), "Range 1 starts first" },
            { Range(2, 7), Range(0, 4), Range(2, 4), "Range 2 starts first" },
        };
    }

    [Theory]
    [MemberData(nameof(OverlapUseCases))]
    public void HasOverlap_When_Overlap(Range<int> range1, Range<int> range2, Range<int> expectedOverlap, string useCase)
    {
        var hasOverlap = range1.HasOverlap(range2, out var overlap);

        hasOverlap.Should().BeTrue(useCase);
        overlap.Should().Be(expectedOverlap, useCase);
    }

    public static TheoryData<Range<int>, Range<int>, string> NoOverlapUseCases()
    {
        return new()
        {
            { Range(0, 1), Range(2, 3), "Range 1 starts first" },
            { Range(2, 3), Range(0, 1), "Range 2 starts first" },
        };
    }

    [Theory]
    [MemberData(nameof(NoOverlapUseCases))]
    public void HasOverlap_When_NoOverlap(Range<int> range1, Range<int> range2, string useCase)
    {
        var hasOverlap = range1.HasOverlap(range2, out _);

        hasOverlap.Should().BeFalse(useCase);
    }

    private static Range<int> Range(int min, int max)
        => new(min, max);
}