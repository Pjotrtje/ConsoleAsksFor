namespace ConsoleAsksFor.Tests;

public class AskForIntTests
{
    private const string Question = "Size?";

    private readonly TestConsole _console = TestConsole.Create();

    [Fact]
    public async Task ValidInputFlow()
    {
        _console.AddKeyInput(new()
        {
            Enter,
        });

        const int defaultValue = 2;

        var answer = await _console.AskForInt(Question, RangeConstraint.Between(1, 3), defaultValue);

        answer.Should().Be(defaultValue);
        _console.Output.Should().Equal(new ConsoleLine[]
        {
            new(LineTypeId.Question, Question),
            new(LineTypeId.QuestionHint, "Range: [1 ... 3]"),
            new(LineTypeId.Answer, "2"),
        });
    }
}