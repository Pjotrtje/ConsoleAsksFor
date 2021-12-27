using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor.TestUtils;

/// <summary>
/// List of known <see cref="KeyInputs" /> and method <see cref="FromChar" /> for converting <see cref="char" /> to <see cref="KeyInputs" />.
/// </summary>
public static class KeyInputs
{
    private const char NullChar = '\0';

    private static readonly Encoding Encoding = CodePagesEncodingProvider.Instance.GetEncoding("ISO-8859-8")!;

    public static KeyInput Backspace { get; } = new(KeyModifier.None, ConsoleKey.Backspace, NullChar);
    public static KeyInput CtrlBackspace { get; } = Backspace with { Modifier = KeyModifier.Ctrl };
    public static KeyInput Delete { get; } = new(KeyModifier.None, ConsoleKey.Delete, NullChar);
    public static KeyInput CtrlDelete { get; } = Delete with { Modifier = KeyModifier.Ctrl };
    public static KeyInput Escape { get; } = new(KeyModifier.None, ConsoleKey.Escape, '\u001b');

    public static KeyInput F1 { get; } = new(KeyModifier.None, ConsoleKey.F1, NullChar);
    public static KeyInput F2 { get; } = new(KeyModifier.None, ConsoleKey.F2, NullChar);
    public static KeyInput F3 { get; } = new(KeyModifier.None, ConsoleKey.F3, NullChar);
    public static KeyInput F4 { get; } = new(KeyModifier.None, ConsoleKey.F4, NullChar);
    public static KeyInput F12 { get; } = new(KeyModifier.None, ConsoleKey.F12, NullChar);
    public static KeyInput Home { get; } = new(KeyModifier.None, ConsoleKey.Home, NullChar);
    public static KeyInput End { get; } = new(KeyModifier.None, ConsoleKey.End, NullChar);
    public static KeyInput LeftArrow { get; } = new(KeyModifier.None, ConsoleKey.LeftArrow, NullChar);
    public static KeyInput CtrlLeftArrow { get; } = LeftArrow with { Modifier = KeyModifier.Ctrl };
    public static KeyInput RightArrow { get; } = new(KeyModifier.None, ConsoleKey.RightArrow, NullChar);
    public static KeyInput CtrlRightArrow { get; } = RightArrow with { Modifier = KeyModifier.Ctrl };
    public static KeyInput UpArrow { get; } = new(KeyModifier.None, ConsoleKey.UpArrow, NullChar);
    public static KeyInput DownArrow { get; } = new(KeyModifier.None, ConsoleKey.DownArrow, NullChar);

    public static KeyInput Enter { get; } = new(KeyModifier.None, ConsoleKey.Enter, '\r');
    public static KeyInput OemMinus { get; } = new(KeyModifier.None, ConsoleKey.OemMinus, '-');
    public static KeyInput OemPeriod { get; } = new(KeyModifier.None, ConsoleKey.OemPeriod, '.');
    public static KeyInput OemComma { get; } = new(KeyModifier.None, ConsoleKey.OemComma, ',');

    public static KeyInput Space { get; } = new(KeyModifier.None, ConsoleKey.Spacebar, ' ');
    public static KeyInput CtrlSpace { get; } = new(KeyModifier.Ctrl, ConsoleKey.Spacebar, ' ');
    public static KeyInput Tab { get; } = new(KeyModifier.None, ConsoleKey.Tab, '\t');
    public static KeyInput CtrlTab { get; } = new(KeyModifier.Ctrl, ConsoleKey.Tab, '\t');
    public static KeyInput CtrlShiftTab { get; } = new(KeyModifier.CtrlShift, ConsoleKey.Tab, '\t');
    public static KeyInput ShiftTab { get; } = new(KeyModifier.Shift, ConsoleKey.Tab, '\t');

    private static readonly IReadOnlyDictionary<char, KeyInput> CharToKeyInputMap = typeof(KeyInputs)
        .GetProperties()
        .Select(p => p.GetValue(null, null))
        .OfType<KeyInput>()
        .Where(k => k.KeyChar is not NullChar)
        .Where(k => k.Modifier == KeyModifier.None)
        .ToDictionary(k => k.KeyChar);

    /// <summary>
    /// Converts <see cref="char" /> to <see cref="KeyInputs" />. Implementation is naive and not all chars can be converted.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    /// <param name="keyChar"></param>
    /// <returns></returns>
    public static KeyInput FromChar(char keyChar)
        => TryGetFromChar(keyChar, out var value)
            ? value
            : throw new ArgumentException($"Not sure how to map {keyChar} from {nameof(Char)} to {nameof(KeyInput)}.", nameof(keyChar));

    private static bool TryGetFromChar(char keyChar, [NotNullWhen(true)] out KeyInput? value)
    {
        var withoutDiacritics = GetWithoutDiacritics(keyChar);

        if (CharToKeyInputMap.TryGetValue(keyChar, out var keyInput))
        {
            value = keyInput;
            return true;
        }

        if (Enum.TryParse<ConsoleKey>(withoutDiacritics, true, out var consoleKey1))
        {
            var modifier = withoutDiacritics.ToLowerInvariant() == withoutDiacritics
                ? KeyModifier.None
                : KeyModifier.Shift;

            value = new KeyInput(modifier, consoleKey1, keyChar);
            return true;
        }

        if (Enum.TryParse<ConsoleKey>($"D{keyChar}", true, out var consoleKey2))
        {
            value = new KeyInput(KeyModifier.None, consoleKey2, keyChar);
            return true;
        }

        value = null;
        return false;
    }

    private static string GetWithoutDiacritics(char keyChar)
    {
        var bytes = Encoding.GetBytes(keyChar.ToString());
        return Encoding.UTF8.GetString(bytes);
    }
}