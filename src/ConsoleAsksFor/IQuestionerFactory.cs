using System.Threading.Tasks;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal interface IQuestionerFactory
{
    Task<Questioner<TAnswer>> Create<TAnswer>(IQuestion<TAnswer> question)
        where TAnswer : notnull;
}