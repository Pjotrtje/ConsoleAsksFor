namespace ConsoleAsksFor.Tests;

public class AskForBoolTests
{
    private const string Question = "Continue?";

    private readonly TestConsole _console = TestConsole.Create();

    [Fact]
    public async Task ValidInputFlow()
    {
        _console.AddKeyInput(
        [
            Enter,
        ]);

        const bool defaultValue = false;

        var answer = await _console.AskForBool(Question, defaultValue);

        answer.Should().Be(defaultValue);
        _console.Output.Should().Equal(
        [
            new(LineTypeId.Question, Question),
            new(LineTypeId.QuestionHint, "Select y/n."),
            new(LineTypeId.Answer, "n"),
        ]);
    }
}