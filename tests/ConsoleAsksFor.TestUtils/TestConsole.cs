namespace ConsoleAsksFor.TestUtils;

public class TestConsole : IConsole
{
    public static TestConsole Create()
    {
        var internalConsole = new InMemoryInternalConsole();

        var console = new Console(
            internalConsole,
            new QuestionerFactory(
                internalConsole,
                internalConsole,
                new KeyInputHandler(),
                new HistoryRepositoryStub(int.MaxValue)),
            internalConsole);

        return new TestConsole(console, internalConsole);
    }

    private readonly Console _console;
    private readonly InMemoryInternalConsole _internalConsole;

    private TestConsole(
        Console console,
        InMemoryInternalConsole internalConsole)
    {
        _console = console;
        _internalConsole = internalConsole;
    }

    public async Task<TAnswer> Ask<TAnswer>(IQuestion<TAnswer> question, CancellationToken cancellationToken = default)
        where TAnswer : notnull
        => await _console.Ask(question, cancellationToken);

    public void WriteSuccessLine(string value)
        => _console.WriteSuccessLine(value);

    public void WriteWarningLine(string value)
        => _console.WriteWarningLine(value);

    public void WriteErrorLine(string value)
        => _console.WriteErrorLine(value);

    public void WriteInfoLine(string value)
        => _console.WriteInfoLine(value);

    public void WriteQuestionLine(string value)
        => _console.WriteQuestionLine(value);

    public void WriteQuestionHintLine(string value)
        => _console.WriteQuestionHintLine(value);

    public void WriteAnswerLine(string value)
        => _console.WriteAnswerLine(value);

    public void WriteInvalidAnswerLine(string value)
        => _console.WriteInvalidAnswerLine(value);

    public void WriteHelpTextLines()
        => _console.WriteHelpTextLines();

    public void WriteCustomLine(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        => _console.WriteCustomLine(value, foregroundColor, backgroundColor);

    public void WriteSplitter(ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black, char splitter = '-')
        => _console.WriteSplitter(foregroundColor, backgroundColor, splitter);

    /// <summary>
    /// The input <see cref="TestConsole" /> uses for answering questions.
    /// </summary>
    /// <param name="keyInputItems"></param>
    public void AddKeyInput(KeyInputItems keyInputItems)
        => _internalConsole.AddKeyInput(keyInputItems);

    /// <summary>
    /// The lines outputted by <see cref="TestConsole" />.
    /// </summary>
    public IEnumerable<ConsoleLine> Output
        => _internalConsole.Output;
}
