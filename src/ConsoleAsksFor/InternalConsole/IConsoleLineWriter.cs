namespace ConsoleAsksFor;

internal interface IConsoleLineWriter
{
    void WriteSuccessLine(string value);

    void WriteWarningLine(string value);

    void WriteErrorLine(string value);

    void WriteInfoLine(string value);

    void WriteQuestionLine(string value);

    void WriteQuestionHintLine(string value);

    void WriteAnswerLine(string value);

    void WriteInvalidAnswerLine(string value);

    void WriteHelpTextLines(IEnumerable<string> values);

    void WriteCustomLine(string value, LineColor color);
}
