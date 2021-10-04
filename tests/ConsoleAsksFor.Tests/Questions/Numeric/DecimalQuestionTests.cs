using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class DecimalQuestionTests
    {
        private const string QuestionText = "Some Question";

        [Fact]
        public void Has_Correct_Guidance_When_No_Range()
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Two,
                RangeConstraint.None,
                null);

            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo(
                "Range: [-7,922,816,251,426,433,759,354,395.00 ... 7,922,816,251,426,433,759,354,395.00]");
        }

        [Theory]
        [InlineData(0, "Range: [1 ... 2]")]
        [InlineData(1, "Range: [1.0 ... 2.0]")]
        [InlineData(2, "Range: [1.00 ... 2.00]")]
        public void Has_Correct_Format_Guidance_When_Range(int digitsAfterDecimalPoint, string formatGuideText)
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Of(digitsAfterDecimalPoint),
                RangeConstraint.Between(1m, 2m),
                null);

            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo(formatGuideText);
        }

        [Fact]
        public void When_No_Default_Value_Has_No_PrefilledValue()
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Two,
                RangeConstraint.None,
                null);

            question.PrefilledValue.Should().BeEmpty();
        }

        [Fact]
        public void When_Default_Value_Has_Correct_PrefilledValue()
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.One,
                RangeConstraint.None,
                10.1m);

            question.PrefilledValue.Should().Be("10.1");
        }

        public static TheoryData<string, decimal, string> CorrectParseUseCases =>
            new()
            {
                { "1", 1m, "Whole number" },
                { " 1", 1m, "Whole number with leading whitespace" },
                { "1 ", 1m, "Whole number with trailing whitespace" },
                { "-1", -1m, "Negative whole number" },
                { "1.01", 1.01m, "No whole number" },
                { "1,001", 1001m, "With separator" },
                { "1001", 1001m, "Without separator" },
            };

        [Theory]
        [MemberData(nameof(CorrectParseUseCases))]
        public void Parses_When_Correct_Value(string answerAsString, decimal expectedAnswer, string useCase)
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Two,
                RangeConstraint.None,
                null);

            var isParsed = question.TryParse(answerAsString, out _, out var answer);
            isParsed.Should().BeTrue(useCase);
            answer.Should().Be(expectedAnswer);
        }

        [Fact]
        public void Does_Not_Parse_When_Smaller_Than_Min()
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Two,
                RangeConstraint.Between(9.01m, 10m),
                null);

            var isParsed = question.TryParse("9", out var errors, out _);
            isParsed.Should().BeFalse();
            errors.Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Parse_When_Greater_Than_Max()
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Two,
                RangeConstraint.Between(8m, 8.99m),
                null);

            var isParsed = question.TryParse("9", out var errors, out _);
            isParsed.Should().BeFalse();
            errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData("", "Missing")]
        [InlineData("e", "Not a number")]
        [InlineData("1.001", "To many decimals")]
        public void Does_Not_Parse_When_Incorrect_Value(string answerAsString, string useCase)
        {
            var question = new DecimalQuestion(
                QuestionText,
                Scale.Two,
                RangeConstraint.None,
                null);

            var isParsed = question.TryParse(answerAsString, out var errors, out _);
            isParsed.Should().BeFalse(useCase);
            errors.Should().BeEmpty();
        }
    }
}