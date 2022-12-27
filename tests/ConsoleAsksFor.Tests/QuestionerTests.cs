namespace ConsoleAsksFor.Tests;

public class QuestionerTests
{
    private const string CorrectAnswer = "OK";
    private readonly InMemoryInternalConsole _internalConsole = new();

    private Questioner<string> GetQuestioner(TestQuestion question)
        => new(
            _internalConsole,
            _internalConsole,
            new KeyInputHandler(),
            new HistoryRepositoryStub(int.MaxValue),
            new History(Enumerable.Empty<HistoryItem>(), int.MaxValue),
            question);

    [Fact]
    public async Task Pressing_F1_Shows_HelpTexts()
    {
        var question = new TestQuestion(CorrectAnswer);
        _internalConsole.AddKeyInput(new()
        {
            "O",
            F1,
            "K",
            Enter,
        });

        var sut = GetQuestioner(question);
        await sut.GetAnswer(CancellationToken.None);

        _internalConsole.Output.Should().Equal(
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.HelpText, "Shortcuts:"),
            new(LineTypeId.HelpText, "  F1: Help"),
            new(LineTypeId.HelpText, "  F2: Toggle history type for current question; ByQuestionTextAndType/ByQuestionType/NotFiltered"),
            new(LineTypeId.HelpText, "  F3: Toggle obfuscate answer for current question (if applicable)"),
            new(LineTypeId.HelpText, "  F4: Repeat question"),
            new(LineTypeId.HelpText, "  F12: Cancel input by throwing TaskCanceledByF12Exception (derived from TaskCanceledException)"),
            new(LineTypeId.HelpText, "  Up: Previous history"),
            new(LineTypeId.HelpText, "  Down: Next History"),
            new(LineTypeId.HelpText, "  Left: Move cursor left"),
            new(LineTypeId.HelpText, "  Ctrl+Left: Move cursor to begin previous word"),
            new(LineTypeId.HelpText, "  Right: Move cursor right"),
            new(LineTypeId.HelpText, "  Ctrl+Right: Move cursor to begin next word"),
            new(LineTypeId.HelpText, "  Home: Move cursor to beginning of line"),
            new(LineTypeId.HelpText, "  End: Move cursor to end of line"),
            new(LineTypeId.HelpText, "  Backspace: Delete previous character"),
            new(LineTypeId.HelpText, "  Ctrl+Backspace: Delete to begin previous word"),
            new(LineTypeId.HelpText, "  Delete: Delete next character"),
            new(LineTypeId.HelpText, "  Ctrl+Delete: Delete to begin next word"),
            new(LineTypeId.HelpText, "  Escape: Clear line"),
            new(LineTypeId.HelpText, "  Tab: Try complete answer; Try select 'next/greater' relevant answer (respecting your typed characters)"),
            new(LineTypeId.HelpText, "  Ctrl+Tab: Try complete answer; Try select 'next/greater' relevant answer (ignoring your typed characters)"),
            new(LineTypeId.HelpText, "  Shift+Tab: Try complete answer; Try select 'previous/smaller' relevant answer (respecting your typed characters)"),
            new(LineTypeId.HelpText, "  Ctrl+Shift+Tab: Try complete answer; Try select 'previous/smaller' relevant answer (ignoring your typed characters)"),
            new(LineTypeId.HelpText, "  Ctrl+Space: Try complete answer"),
            new(LineTypeId.Answer, CorrectAnswer));
    }

    [Fact]
    public async Task Pressing_F4_Repeats_Question()
    {
        var question = new TestQuestion(CorrectAnswer);
        _internalConsole.AddKeyInput(new()
        {
            "O",
            F4,
            "K",
            Enter,
        });

        var sut = GetQuestioner(question);
        await sut.GetAnswer(CancellationToken.None);

        _internalConsole.Output.Should().Equal(new ConsoleLine[]
        {
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.Answer, CorrectAnswer),
        });
    }

    [Fact]
    public async Task Pressing_F12_Throws_TaskCanceledByF12Exception()
    {
        var question = new TestQuestion(CorrectAnswer);
        _internalConsole.AddKeyInput(new()
        {
            "O",
            F12,
        });

        var sut = GetQuestioner(question);
        Func<Task> getAnswer = () => sut.GetAnswer(CancellationToken.None);
        await getAnswer.Should().ThrowAsync<TaskCanceledByF12Exception>();

        _internalConsole.Output.Should().Equal(new ConsoleLine[]
        {
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.InvalidAnswer, "O"),
            new(LineTypeId.Warning, "Answering question interupted by F12!"),
        });
    }

    [Fact]
    public async Task When_Asking_Shows_PrefilledValue()
    {
        var question = new TestQuestion(CorrectAnswer)
        {
            PrefilledValue = CorrectAnswer,
        };

        _internalConsole.AddKeyInput(new()
        {
            Enter,
        });

        var sut = GetQuestioner(question);
        await sut.GetAnswer(CancellationToken.None);

        _internalConsole.Output.Should().Equal(new ConsoleLine[]
        {
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.Answer, CorrectAnswer),
        });
    }

    [Fact]
    public async Task When_Asking_And_Invalid_Answer_Shows_Errors_And_Re_Asks()
    {
        var question = new TestQuestion(CorrectAnswer)
        {
            ParseErrorsWhenIncorrectValue = new[] { "Error1", "Error2" },
        };

        _internalConsole.AddKeyInput(new()
        {
            "NOK",
            Enter,
            Escape,
            CorrectAnswer,
            Enter,
        });

        var sut = GetQuestioner(question);
        await sut.GetAnswer(CancellationToken.None);

        _internalConsole.Output.Should().Equal(
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.InvalidAnswer, "NOK"),
            new(LineTypeId.Error, "Error1"),
            new(LineTypeId.Error, "Error2"),
            new(LineTypeId.Error, "Please correct/retry."),
            new(LineTypeId.Answer, CorrectAnswer));
    }

    [Fact]
    public async Task When_Asking_Shows_Hints()
    {
        var question = new TestQuestion(CorrectAnswer)
        {
            Hints = new[] { "Hint1", "Hint2" },
        };

        _internalConsole.AddKeyInput(new()
        {
            CorrectAnswer,
            Enter,
        });

        var sut = GetQuestioner(question);
        await sut.GetAnswer(CancellationToken.None);

        _internalConsole.Output.Should().Equal(
            new(LineTypeId.Question, question.Text),
            new(LineTypeId.QuestionHint, "Hint1"),
            new(LineTypeId.QuestionHint, "Hint2"),
            new(LineTypeId.Answer, CorrectAnswer));
    }
}
