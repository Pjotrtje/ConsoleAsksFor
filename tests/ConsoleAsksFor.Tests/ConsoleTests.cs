namespace ConsoleAsksFor.Tests;

public class ConsoleTests
{
    private readonly IConsole _sut;
    private readonly InMemoryInternalConsole _internalConsole = new();

    public ConsoleTests()
    {
        _sut = new Console(
            _internalConsole,
            new QuestionerFactory(
                _internalConsole,
                _internalConsole,
                new KeyInputHandler(),
                new HistoryRepositoryStub(int.MaxValue)));
    }

    [Fact]
    public void WriteErrorLine_Writes_MessageInCorrectColor()
    {
        const string value = "Some error";

        _sut.WriteErrorLine(value);

        _internalConsole.Output.Should().Equal(
            new ConsoleLine(LineTypeId.Error, value));
    }

    [Fact]
    public void WriteSuccessLine_Writes_MessageInCorrectColor()
    {
        const string value = "Some success";

        _sut.WriteSuccessLine(value);

        _internalConsole.Output.Should().Equal(
            new ConsoleLine(LineTypeId.Success, value));
    }

    [Fact]
    public void WriteWarningLine_Writes_MessageInCorrectColor()
    {
        const string value = "Some warning";

        _sut.WriteWarningLine(value);

        _internalConsole.Output.Should().Equal(
            new ConsoleLine(LineTypeId.Warning, value));
    }

    [Fact]
    public void WriteInfoLine_Writes_MessageInCorrectColor()
    {
        const string value = "Some info";

        _sut.WriteInfoLine(value);

        _internalConsole.Output.Should().Equal(
            new ConsoleLine(LineTypeId.Info, value));
    }

    [Fact]
    public void WriteQuestionLine_Writes_MessageInCorrectColor()
    {
        const string value = "Some question";

        _sut.WriteQuestionLine(value);

        _internalConsole.Output.Should().Equal(
            new ConsoleLine(LineTypeId.Question, value));
    }

    [Fact]
    public async Task Ask_Return_Answer()
    {
        var question = new TestQuestion("OK");

        _internalConsole.AddKeyInput(new()
        {
            "OK",
            Enter,
        });

        var result = await _sut.Ask(question, CancellationToken.None);
        result.Should().Be("OK");
    }
}