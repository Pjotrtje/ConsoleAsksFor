namespace ConsoleAsksFor;

internal sealed class Questioner<TAnswer>
    where TAnswer : notnull
{
    private readonly IConsoleLineWriter _consoleLineWriter;
    private readonly IConsoleInputGetter _consoleInputGetter;
    private readonly IKeyInputHandler _keyInputHandler;
    private readonly IHistoryRepository _historyRepository;
    private readonly History _history;
    private readonly IQuestion<TAnswer> _question;

    private const HistoryType DefaultHistoryType = HistoryType.ByQuestionTextAndType;

    public Questioner(
        IConsoleLineWriter consoleLineWriter,
        IConsoleInputGetter consoleInputGetter,
        IKeyInputHandler keyInputHandler,
        IHistoryRepository historyRepository,
        History history,
        IQuestion<TAnswer> question)
    {
        _consoleLineWriter = consoleLineWriter;
        _consoleInputGetter = consoleInputGetter;
        _keyInputHandler = keyInputHandler;
        _historyRepository = historyRepository;
        _history = history;
        _question = question;
    }

    public async Task<TAnswer> GetAnswer(CancellationToken cancellationToken)
    {
        var historyType = DefaultHistoryType;
        var scopedHistory = GetScopedHistory(historyType);
        var line = new InProgressLine(_question.PrefilledValue, _question.MustObfuscateAnswer);
        WriteQuestion();
        do
        {
            var keyInput = await _consoleInputGetter.ReadKeyWhileBlinkLine(
                line,
                IsValid(line),
                cancellationToken);

            switch (keyInput.Key)
            {
                case ConsoleKey.Enter:
                    if (!TryParse(line.Value, out var errors, out var answer))
                    {
                        WriteInvalidAnswerLine(line);
                        WriteErrors(errors);
                        break;
                    }

                    var formattedLine = GetFormattedLine(line, answer);
                    WriteAnswerLine(formattedLine);
                    AddToHistory(formattedLine);
                    return answer;

                case ConsoleKey.F1:
                    WriteHelpTextLines();
                    break;

                case ConsoleKey.F2:
                    historyType = historyType.Next();
                    scopedHistory = GetScopedHistory(historyType);
                    WriteHistoryType(historyType);
                    break;

                case ConsoleKey.F3:
                    if (_question.MustObfuscateAnswer)
                    {
                        line = line with { MustObfuscate = !line.MustObfuscate };
                    }
                    break;

                case ConsoleKey.F4:
                    WriteQuestion();
                    break;

                case ConsoleKey.F12:
                    WriteInvalidAnswerLine(line);
                    _consoleLineWriter.WriteWarningLine("Answering question interupted by F12!");
                    throw new TaskCanceledByF12Exception();

                default:
                    line = _keyInputHandler.HandleKeyInput(line, keyInput, scopedHistory, _question.Intellisense);
                    break;
            }
        } while (true);
    }

    private bool IsValid(InProgressLine line)
        => TryParse(line.Value, out _, out _);

    private InProgressLine GetFormattedLine(InProgressLine line, TAnswer answer)
        => line with
        {
            Value = FormatAnswer(answer, line.Value),
            MustObfuscate = _question.MustObfuscateAnswer,
        };

    private string FormatAnswer(TAnswer answer, string valueAsString)
    {
        try
        {
            return _question.FormatAnswer(answer);
        }
        catch
        {
            return valueAsString;
        }
    }

    private bool TryParse(
        string value,
        [MaybeNullWhen(true)] out IEnumerable<string> errors,
        [MaybeNullWhen(false)] out TAnswer answer)
    {
        try
        {
            return _question.TryParse(value, out errors, out answer);
        }
        catch (Exception e)
        {
            errors = [e.Message];
            answer = default;
            return false;
        }
    }

    private ScopedHistory GetScopedHistory(HistoryType historyType)
        => _history.GetScopedHistory(historyType, _question.GetHistoryType(), _question.Text);

    private void WriteQuestion()
    {
        _consoleLineWriter.WriteQuestionLine(_question.Text);
        foreach (var hint in _question.GetHints())
        {
            _consoleLineWriter.WriteQuestionHintLine(hint);
        }

        if (_question.MustObfuscateAnswer)
        {
            _consoleLineWriter.WriteQuestionHintLine("Display will be obfuscated but can be shown by pressing F3.");
            _consoleLineWriter.WriteQuestionHintLine("Answer will not be saved to history.");
        }
    }

    private void WriteInvalidAnswerLine(InProgressLine line)
        => _consoleLineWriter.WriteInvalidAnswerLine(line.DisplayValue);

    private void WriteAnswerLine(InProgressLine formattedLine)
        => _consoleLineWriter.WriteAnswerLine(formattedLine.DisplayValue);

    private void WriteHelpTextLines()
        => _consoleLineWriter.WriteHelpTextLines(HelpTexts.Lines);

    private void WriteErrors(IEnumerable<string> errors)
    {
        var materializedErrors = errors.ToList();
        if (materializedErrors.Any())
        {
            foreach (var errorHint in materializedErrors)
            {
                _consoleLineWriter.WriteErrorLine(errorHint);
            }
        }
        else
        {
            _consoleLineWriter.WriteErrorLine("Invalid.");
        }

        _consoleLineWriter.WriteErrorLine("Please correct/retry.");
    }

    private void WriteHistoryType(HistoryType historyType)
        => _consoleLineWriter.WriteInfoLine($"HistoryType set to {historyType} (and will be reset to default {DefaultHistoryType} after this question).");

    private void AddToHistory(InProgressLine line)
    {
        if (!_question.MustObfuscateAnswer)
        {
            var historyItem = new HistoryItem(
                _question.GetHistoryType(),
                _question.Text,
                line.DisplayValue);

            _history.Add(historyItem);
            _historyRepository.PersistHistory(_history);
        }
    }
}