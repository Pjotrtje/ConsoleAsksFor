using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class BoolQuestionIntellisense : IIntellisense
    {
        public string? CompleteValue(string value)
            => string.IsNullOrWhiteSpace(value)
                ? "y"
                : null;

        public string? PreviousValue(string value, string hint)
            => string.IsNullOrWhiteSpace(hint)
                ? TryToggleValue(value, "n")
                : null;

        public string? NextValue(string value, string hint)
            => string.IsNullOrWhiteSpace(hint)
                ? TryToggleValue(value, "y")
                : null;

        private static string? TryToggleValue(string value, string valueWhenMissing)
            => value.Trim() switch
            {
                "" => valueWhenMissing,
                "y" => "n",
                "n" => "y",
                _ => null,
            };
    }
}