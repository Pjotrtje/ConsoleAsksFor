namespace ConsoleAsksFor.Tests;

public class AskForDateTimeTests
{
    private const string Question = "What is your favorite date?";

    private readonly TestConsole _console = TestConsole.Create();

    [Fact]
    public async Task ValidInputFlow()
    {
        _console.AddKeyInput(new()
        {
            Enter,
        });

        var defaultValue = 20.September(2021).At(22, 00).AsUtc();

        var answer = await _console.AskForDateTime(
            Question,
            DateTimeKind.Utc,
            RangeConstraint.Between(
                2.January(2020).At(9, 30).AsUtc(),
                31.December(2022).At(13, 00).AsUtc()),
            defaultValue);

        answer.Should().Be(defaultValue).And.BeIn(DateTimeKind.Utc);
        _console.Output.Should().Equal(
            new(LineTypeId.Question, Question),
            new(LineTypeId.QuestionHint, "Format: 'yyyy-MM-dd HH:mm:ss' (UTC)"),
            new(LineTypeId.QuestionHint, "Range: [2020-01-02 09:30:00 ... 2022-12-31 13:00:00]"),
            new(LineTypeId.Answer, "2021-09-20 22:00:00"));
    }
}
