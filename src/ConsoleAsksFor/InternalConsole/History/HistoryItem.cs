namespace ConsoleAsksFor;

internal sealed record HistoryItem(
    string QuestionType,
    string QuestionText,
    string Answer);