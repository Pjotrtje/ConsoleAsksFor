namespace ConsoleAsksFor.TestUtils;

internal sealed class InMemoryInternalConsole : IConsoleLineWriter, IConsoleInputGetter
{
    private readonly LineTypes _lineTypes = new(ConsoleColors.Default);
    private readonly List<KeyInput> _readKeyInputs = new();
    private readonly List<ConsoleLine> _output = new();
    private int _readKeyIndex;

    public IEnumerable<ConsoleLine> Output => _output;

    public void WriteSuccessLine(string value)
        => WriteLine(_lineTypes.Success, value);

    public void WriteWarningLine(string value)
        => WriteLine(_lineTypes.Warning, value);

    public void WriteErrorLine(string value)
        => WriteLine(_lineTypes.Error, value);

    public void WriteInfoLine(string value)
        => WriteLine(_lineTypes.Type, value);

    public void WriteQuestionLine(string value)
        => WriteLine(_lineTypes.Question, value);

    public void WriteQuestionHintLine(string value)
        => WriteLine(_lineTypes.QuestionHint, value);

    public void WriteAnswerLine(string value)
        => WriteLine(_lineTypes.Answer, value);

    public void WriteInvalidAnswerLine(string value)
        => WriteLine(_lineTypes.InvalidAnswer, value);

    public void WriteHelpTextLines(IEnumerable<string> values)
    {
        foreach (var value in values)
        {
            WriteLine(_lineTypes.HelpText, value);
        }
    }

    public Task<KeyInput> ReadKeyWhileBlinkLine(
        InProgressLine line,
        bool currentLineIsValid,
        CancellationToken cancellationToken)
        => _readKeyIndex < _readKeyInputs.Count
            ? Task.FromResult(_readKeyInputs[_readKeyIndex++])
            : throw new InvalidOperationException($"{nameof(ReadKeyWhileBlinkLine)} is called more times than it has setups");

    public void AddKeyInput(KeyInputItems keyInputItems)
        => _readKeyInputs.AddRange(keyInputItems);

    private void WriteLine(LineType lineType, string value)
        => _output.Add(new(lineType.Id, value));
}