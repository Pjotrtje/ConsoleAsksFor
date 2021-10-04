using System;

namespace ConsoleAsksFor
{
    internal static class ExceptionExtensions
    {
        public static string ToActionExceptionMessage(this Exception exception, string actionName)
        {
            var message = exception.Message.Replace(Environment.NewLine, "; ");
            return $"{actionName} failed: {message}.";
        }
    }
}