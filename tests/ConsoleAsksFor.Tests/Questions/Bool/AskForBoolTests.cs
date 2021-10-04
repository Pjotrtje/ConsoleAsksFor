using System.Threading.Tasks;

using FluentAssertions;

using ConsoleAsksFor.TestUtils;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class AskForBoolTests
    {
        private const string Question = "Continue?";

        private readonly TestConsole _console = TestConsole.Create();

        [Fact]
        public async Task ValidInputFlow()
        {
            _console.AddKeyInput(new()
            {
                KeyInputs.Enter,
            });

            const bool defaultValue = false;

            var answer = await _console.AskForBool(Question, defaultValue);

            answer.Should().Be(defaultValue);
            _console.Output.Should().Equal(new ConsoleLine[]
            {
                new(LineTypeId.Question, Question),
                new(LineTypeId.QuestionHint, "Select y/n."),
                new(LineTypeId.Answer, "n"),
            });
        }
    }
}