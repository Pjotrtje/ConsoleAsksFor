namespace ConsoleAsksFor
{
    internal static class CharExtensions
    {
        public static bool CanBeTypedInConsoleAsksFor(this char keyChar)
            => char.IsLetterOrDigit(keyChar) ||
               char.IsPunctuation(keyChar) ||
               char.IsSymbol(keyChar) ||
               keyChar == ' ';
    }
}