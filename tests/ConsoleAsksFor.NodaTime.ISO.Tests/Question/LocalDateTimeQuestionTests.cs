using FluentAssertions;

using NodaTime;

using Xunit;

namespace ConsoleAsksFor.NodaTime.ISO.Tests
{
    public class LocalDateTimeQuestionTests
    {
        private static readonly DateTimeZone WestEuropeStandardTime = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Europe/Amsterdam")!;

        private const string QuestionText = "Some Question";

        [Fact]
        public void Has_Correct_Guidance_When_No_Min_Or_Max()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.None,
                null);

            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo(
                "Range: [-9998-01-01 ... 9999-12-31]",
                "Format: 'uuuu-MM-dd' (Europe/Amsterdam)");
        }

        [Fact]
        public void Has_Correct_Guidance_When_Min()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.AtLeast(new LocalDateTime(2020, 01, 03, 00, 00)),
                null);

            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo(
                "Range: [2020-01-03 ... 9999-12-31]",
                "Format: 'uuuu-MM-dd' (Europe/Amsterdam)");
        }

        [Fact]
        public void Has_Correct_Guidance_When_Max()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.AtMost(new LocalDateTime(2020, 01, 03, 00, 00)),
                null);

            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo(
                "Range: [-9998-01-01 ... 2020-01-03]",
                "Format: 'uuuu-MM-dd' (Europe/Amsterdam)");
        }

        [Fact]
        public void When_No_Default_Value_Has_No_PrefilledValue()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.None,
                null);

            question.PrefilledValue.Should().BeEmpty();
        }

        [Fact]
        public void When_Default_Value_Has_Correct_PrefilledValue()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.None,
                new LocalDateTime(2020, 01, 03, 00, 00));

            question.PrefilledValue.Should().Be("2020-01-03");
        }

        [Theory]
        [InlineData("2020-01-03", "Regular")]
        [InlineData(" 2020-01-03 ", "With whitespace")]
        public void Parses_When_Correct_Value(string answerAsString, string useCase)
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.None,
                null);

            var isParsed = question.TryParse(answerAsString, out _, out var answer);
            isParsed.Should().BeTrue(useCase);

            answer.Should().Be(new LocalDateTime(2020, 01, 03, 00, 00));
        }

        [Fact]
        public void Does_Not_Parse_When_Smaller_Than_Min()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.AtLeast(new LocalDateTime(2020, 01, 04, 00, 00)),
                null);

            var isParsed = question.TryParse("2020-01-03", out var errors, out _);
            isParsed.Should().BeFalse();
            errors.Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Parse_When_Greater_Than_Max()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.AtMost(new LocalDateTime(2020, 01, 02, 00, 00)),
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
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.Date,
                WestEuropeStandardTime,
                RangeConstraint.None,
                null);

            var isParsed = question.TryParse(answerAsString, out var errors, out _);
            isParsed.Should().BeFalse(useCase);
            errors.Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Parse_When_DateTime_Does_Not_Exist_In_TimeZone()
        {
            var question = new LocalDateTimeQuestion(
                QuestionText,
                LocalDateTimeFormat.DateTime,
                WestEuropeStandardTime,
                RangeConstraint.None,
                null);

            var isParsed = question.TryParse("2021-03-28 02:30:00", out var errors, out _);
            isParsed.Should().BeFalse();
            errors.Should().BeEquivalentTo("This DateTime never occurs due to summer/winter time.");
        }
    }
}