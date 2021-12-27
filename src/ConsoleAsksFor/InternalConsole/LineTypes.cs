namespace ConsoleAsksFor;

internal sealed record LineTypes(
    LineType Error,
    LineType Warning,
    LineType Type,
    LineType Success,
    LineType Question,
    LineType QuestionHint,
    LineType Answer,
    LineType InvalidAnswer,
    LineType HelpText)
{
    public LineTypes(ConsoleColors colors)
        : this(
            Error: new LineType(LineTypeId.Error, colors.Error),
            Warning: new LineType(LineTypeId.Warning, colors.Warning),
            Type: new LineType(LineTypeId.Info, colors.Info),
            Success: new LineType(LineTypeId.Success, colors.Success),
            Question: new LineType(LineTypeId.Question, colors.Question),
            QuestionHint: new LineType(LineTypeId.QuestionHint, colors.QuestionHint),
            Answer: new LineType(LineTypeId.Answer, colors.Answer),
            InvalidAnswer: new LineType(LineTypeId.InvalidAnswer, colors.InvalidAnswer),
            HelpText: new LineType(LineTypeId.HelpText, colors.HelpText))
    {
    }
}