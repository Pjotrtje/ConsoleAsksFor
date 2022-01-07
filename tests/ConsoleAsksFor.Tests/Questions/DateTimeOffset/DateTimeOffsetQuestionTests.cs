#if NET6_0_OR_GREATER

namespace ConsoleAsksFor.Tests;

public class DateTimeOffsetQuestionTests
{
    private static readonly TimeZoneInfo WestEuropeStandardTime = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

    private const string QuestionText = "Some Question";

    [Fact]
    public void Has_Correct_Guidance_When_No_Min_Or_Max()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            null);

        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Range: [0001-01-01 ... 9999-12-31]",
            "Format: 'yyyy-MM-dd' (Local)");
    }

    [Fact]
    public void Has_Correct_Guidance_When_Min()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.AtLeast(new DateOnly(2020, 01, 03)),
            null);

        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Range: [2020-01-03 ... 9999-12-31]",
            "Format: 'yyyy-MM-dd' (Local)");
    }

    [Fact]
    public void Has_Correct_Guidance_When_Max()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.AtMost(new DateOnly(2020, 01, 03)),
            null);

        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEquivalentTo(
            "Range: [0001-01-01 ... 2020-01-03]",
            "Format: 'yyyy-MM-dd' (Local)");
    }

    [Fact]
    public void When_No_Default_Value_Has_No_PrefilledValue()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            null);

        question.PrefilledValue.Should().BeEmpty();
    }

    [Fact]
    public void When_Default_Value_Has_Correct_PrefilledValue()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            new DateOnly(2020, 01, 03));

        question.PrefilledValue.Should().Be("2020-01-03");
    }

    [Fact]
    public void Parses_When_InZone_And_Correct_Value()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            null);

        var isParsed = question.TryParse("2020-01-03", out _, out var answer);
        isParsed.Should().BeTrue();
        answer.Should().Be(3.January(2020).WithOffset(TimeSpan.Zero));
    }

    [Fact]
    public void Parses_When_Correct_Value_With_Whitespace()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            null);

        var isParsed = question.TryParse(" 2020-01-03 ", out _, out var answer);
        isParsed.Should().BeTrue();
        answer.Should().Be(3.January(2020).WithOffset(TimeSpan.Zero));
    }

    [Fact]
    public void Parses_When_InUtc_And_Correct_Value()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            null);

        var isParsed = question.TryParse("2020-01-03", out _, out var answer);
        isParsed.Should().BeTrue();
        answer.Should().Be(3.January(2020).WithOffset(TimeSpan.Zero));
    }

    [Fact]
    public void Does_Not_Parse_When_Smaller_Than_Min()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.AtLeast(new DateOnly(2020, 01, 04)),
            null);

        var isParsed = question.TryParse("2020-01-03", out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Does_Not_Parse_When_Greater_Than_Max()
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.AtMost(new DateOnly(2020, 01, 02)),
            null);

        var isParsed = question.TryParse("2020-01-03", out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "Missing")]
    [InlineData("1234", "Some integer")]
    [InlineData("200-01-01", "Year too short")]
    [InlineData("20000-01-01", "Year too long")]
    [InlineData("2000-1-01", "Month too short")]
    [InlineData("2000-001-01", "Month too long")]
    [InlineData("2000-01-1", "Day too short")]
    [InlineData("2000-01-001", "Day too long")]
    public void Does_Not_Parse_When_Incorrect_Value(string answerAsString, string useCase)
    {
        var question = AskForAppender.GetDateOnlyQuestion(
            QuestionText,
            RangeConstraint.None,
            null);

        var isParsed = question.TryParse(answerAsString, out var errors, out _);
        isParsed.Should().BeFalse(useCase);
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Does_Not_Parse_When_DateTime_Does_Not_Exist_In_TimeZone()
    {
        var question = AskForAppender.GetDateTimeQuestion(
            QuestionText,
            WestEuropeStandardTime,
            RangeConstraint.None,
            null);

        var isParsed = question.TryParse("2021-03-28 02:30:00", out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().BeEmpty();
    }
}

#endif