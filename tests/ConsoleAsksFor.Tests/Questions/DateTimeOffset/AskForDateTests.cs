using System;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Extensions;

using ConsoleAsksFor.TestUtils;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class AskForDateTests
    {
        private const string Question = "What is your favorite date?";

        private readonly TestConsole _console = TestConsole.Create();

        [Fact]
        public async Task ValidInputFlow()
        {
            _console.AddKeyInput(new()
            {
                KeyInputs.Enter,
            });

            var defaultValue = 20.September(2021).AsUtc();

            var answer = await _console.AskForDate(
                Question,
                DateTimeKind.Utc,
                RangeConstraint.Between(
                    2.January(2020).AsUtc(),
                    31.December(2022).AsUtc()),
                defaultValue);

            answer.Should().Be(defaultValue).And.BeIn(DateTimeKind.Utc);
            _console.Output.Should().Equal(
                new(LineTypeId.Question, Question),
                new(LineTypeId.QuestionHint, "Range: [2020-01-02 ... 2022-12-31]"),
                new(LineTypeId.QuestionHint, "Format: 'yyyy-MM-dd' (UTC)"),
                new(LineTypeId.Answer, "2021-09-20"));
        }
    }
}