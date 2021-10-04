using System.Text.RegularExpressions;

using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class StringQuestionTests
    {
        private const string QuestionText = "Some Question";

        [Fact]
        public void Has_Correct_Guidance_When_No_Regex()
        {
            var question = new StringQuestion(QuestionText, null, null);
            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEmpty();
        }

        [Fact]
        public void Has_Correct_Guidance_When_Regex_Without_Hint()
        {
            var question = new StringQuestion(QuestionText, (new Regex("^(.*)$"), null), null);
            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo("With regex pattern: '^(.*)$'.");
        }

        [Fact]
        public void Has_Correct_Guidance_When_Regex_With_Hint()
        {
            var question = new StringQuestion(QuestionText, (new Regex("^(.*)$"), "Some helpt text!"), null);
            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEquivalentTo("Some helpt text!");
        }

        [Fact]
        public void When_No_Default_Value_Has_No_PrefilledValue()
        {
            var question = new StringQuestion(QuestionText, null, null);
            question.PrefilledValue.Should().BeEmpty();
        }

        [Fact]
        public void When_Default_Value_Has_Correct_PrefilledValue()
        {
            var question = new StringQuestion(QuestionText, null, "1234");
            question.PrefilledValue.Should().Be("1234");
        }

        [Fact]
        public void Parses_When_Correct_Value_When_Regex()
        {
            var question = new StringQuestion(QuestionText, (new Regex("^A(.*)C$"), "Some helpt text!"), null);
            var isParsed = question.TryParse("ABC", out _, out var answer);
            isParsed.Should().BeTrue();
            answer.Should().Be("ABC");
        }

        [Fact]
        public void Does_Not_Parse_When_Incorrect_Value_When_Regex()
        {
            var question = new StringQuestion(QuestionText, (new Regex("^A(.*)C$"), "Some helpt text!"), null);
            var isParsed = question.TryParse("ACB", out var errors, out _);
            isParsed.Should().BeFalse();
            errors.Should().BeEmpty();
        }
    }
}