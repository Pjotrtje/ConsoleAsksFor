namespace ConsoleAsksFor.Tests;

public class AskForStringTests
{
    private const string Question = "What is your middle name?";

    private readonly TestConsole _console = TestConsole.Create();

    [Fact]
    public async Task ValidInputFlow_WithoutRegex()
    {
        const string defaultValue = "some value";
        _console.AddKeyInput(
        [
            Enter,
        ]);

        var answer = await _console.AskForString(Question, defaultValue);

        answer.Should().Be(defaultValue);
        _console.Output.Should().Equal(
        [
            new(LineTypeId.Question, Question),
            new(LineTypeId.Answer, defaultValue),
        ]);
    }

    [Fact]
    public async Task ValidInputFlow_WithRegex()
    {
        const string defaultValue = "someValue";
        _console.AddKeyInput(
        [
            Enter,
        ]);

        var answer = await _console.AskForString(Question, new Regex("[a-z]+"), "SomeHint", defaultValue);

        answer.Should().Be(defaultValue);
        _console.Output.Should().Equal(
        [
            new(LineTypeId.Question, Question),
            new(LineTypeId.QuestionHint, "SomeHint"),
            new(LineTypeId.Answer, defaultValue),
        ]);
    }
}