﻿namespace ConsoleAsksFor;

internal sealed class Console : IConsole, IDisposable
{
    private readonly AsyncLocker _userFocusLocker = new();

    private readonly IConsoleLineWriter _consoleLineWriter;
    private readonly IQuestionerFactory _questionerFactory;
    private readonly IWindowWidthProvider _windowWidthProvider;

    public Console(
        IConsoleLineWriter consoleLineWriter,
        IQuestionerFactory questionerFactory,
        IWindowWidthProvider windowWidthProvider)
    {
        _consoleLineWriter = consoleLineWriter;
        _questionerFactory = questionerFactory;
        _windowWidthProvider = windowWidthProvider;
    }

    public void WriteSuccessLine(string value)
        => _consoleLineWriter.WriteSuccessLine(value);

    public void WriteWarningLine(string value)
        => _consoleLineWriter.WriteWarningLine(value);

    public void WriteErrorLine(string value)
        => _consoleLineWriter.WriteErrorLine(value);

    public void WriteInfoLine(string value)
        => _consoleLineWriter.WriteInfoLine(value);

    public void WriteQuestionLine(string value)
        => _consoleLineWriter.WriteQuestionLine(value);

    public void WriteQuestionHintLine(string value)
        => _consoleLineWriter.WriteQuestionHintLine(value);

    public void WriteAnswerLine(string value)
        => _consoleLineWriter.WriteAnswerLine(value);

    public void WriteInvalidAnswerLine(string value)
        => _consoleLineWriter.WriteInvalidAnswerLine(value);

    public void WriteHelpTextLines()
        => _consoleLineWriter.WriteHelpTextLines(HelpTexts.Lines);

    public void WriteCustomLine(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        => _consoleLineWriter.WriteCustomLine(value, new LineColor(foregroundColor, backgroundColor));

    public void WriteSplitter(ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black, char splitter = '-')
    {
        var value = new string(splitter, _windowWidthProvider.WindowWidth - 1);
        _consoleLineWriter.WriteCustomLine(value, new LineColor(foregroundColor, backgroundColor));
    }

    public async Task<TAnswer> Ask<TAnswer>(IQuestion<TAnswer> question, CancellationToken cancellationToken)
        where TAnswer : notnull
    {
        return await _userFocusLocker.LockAsync(async () =>
        {
            var questioner = await _questionerFactory.Create(question);
            return await questioner.GetAnswer(cancellationToken);
        });
    }

    public void Dispose()
    {
        _userFocusLocker.Dispose();
    }
}
