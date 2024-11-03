namespace ConsoleAsksFor.Tests;

public class AskForDateOnlyTests
{
    private const string Question = "What is your favorite date?";

    private readonly TestConsole _console = TestConsole.Create();

    [Fact]
    public async Task ValidInputFlow()
    {
        _console.AddKeyInput(
        [
            Enter,
        ]);

        var defaultValue = new DateOnly(2021, 09, 20); ;

        var answer = await _console.AskForDateOnly(
            Question,
            RangeConstraint.Between(
                new DateOnly(2020, 01, 02),
                new DateOnly(2022, 12, 31)),
            defaultValue);

        answer.Should().Be(defaultValue);
        _console.Output.Should().Equal(
            new(LineTypeId.Question, Question),
            new(LineTypeId.QuestionHint, "Format: 'yyyy-MM-dd' (Local)"),
            new(LineTypeId.QuestionHint, "Range: [2020-01-02 ... 2022-12-31]"),
            new(LineTypeId.Answer, "2021-09-20"));
    }
}
