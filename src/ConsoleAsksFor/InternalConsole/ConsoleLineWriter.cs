namespace ConsoleAsksFor;

internal sealed class ConsoleLineWriter : IConsoleLineWriter
{
    private readonly ISuspendableOutWriter _suspendableOutWriter;
    private readonly IWriteLineLogger _writeLineLogger;
    private readonly LineTypes _lineTypes;

    public ConsoleLineWriter(
        ISuspendableOutWriter suspendableOutWriter,
        IWriteLineLogger writeLineLogger,
        LineTypes lineTypes)
    {
        _suspendableOutWriter = suspendableOutWriter;
        _writeLineLogger = writeLineLogger;
        _lineTypes = lineTypes;
    }

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

    private void WriteLine(LineType lineType, string value)
    {
        _suspendableOutWriter.WriteLine(lineType.Color.Colorize(value));
        _writeLineLogger.LogToFile(lineType.Id, value);
    }
}