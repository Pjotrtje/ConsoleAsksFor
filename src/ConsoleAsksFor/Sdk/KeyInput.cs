using System;

namespace ConsoleAsksFor.Sdk
{
    /// <summary>
    /// Output of <see cref="IConsoleInputGetter.ReadKeyWhileBlinkLine" />.
    /// </summary>
    public sealed record KeyInput(
        KeyModifier Modifier,
        ConsoleKey Key,
        char KeyChar)
    {
        ///<inheritdoc cref="object.ToString"/>
        public override string ToString()
            => $"{Key} (Char={KeyChar}, Modifier={Modifier})]";
    }
}
