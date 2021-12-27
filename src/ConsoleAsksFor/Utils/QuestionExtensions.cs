using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal static class QuestionExtensions
{
    public static string GetHistoryType<T>(this IQuestion<T> question)
        where T : notnull
    {
        var type = question.GetType().Name;

        return question.SubType is null
            ? type
            : $"{type}: {question.SubType}";
    }
}