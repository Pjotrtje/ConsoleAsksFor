using System;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class KeyInputHandler : IKeyInputHandler
    {
        public InProgressLine HandleKeyInput(
            InProgressLine line,
            KeyInput keyInput,
            IScopedHistory scopedHistory,
            IIntellisense intellisense)
        {
            return IsRegularInput(keyInput)
                ? HandleRegularInput(line, keyInput)
                : HandleSpecialInput(line, keyInput, scopedHistory, intellisense);
        }

        private static InProgressLine HandleRegularInput(InProgressLine line, KeyInput keyInput)
        {
            var value = line.Value.Insert(line.CursorIndex, keyInput.KeyChar.ToString());
            return line with
            {
                Value = value,
                IntellisenseHint = value,
                CursorIndex = line.CursorIndex + 1,
            };
        }

        private static InProgressLine HandleIntellisense(InProgressLine line, Func<string, string, string?> getNewValue, string intellisenseHint)
        {
            string? newValue;
            try
            {
                newValue = getNewValue(line.Value, intellisenseHint);
            }
            catch
            {
                return line;
            }

            return newValue is null
                ? line
                : line with
                {
                    Value = newValue,
                    CursorIndex = newValue.Length,
                };
        }

        private static InProgressLine HandleSpecialInput(InProgressLine line, KeyInput keyInput, IScopedHistory scopedHistory, IIntellisense intellisense)
        {
            return (keyInput.Key, keyInput.Modifier) switch
            {
                (ConsoleKey.Spacebar, KeyModifier.Ctrl) when !line.MustObfuscate => HandleIntellisense(line, (v, _) => intellisense.CompleteValue(v), line.IntellisenseHint),
                (ConsoleKey.Tab, KeyModifier.None) when !line.MustObfuscate => HandleIntellisense(line, intellisense.NextValue, line.IntellisenseHint),
                (ConsoleKey.Tab, KeyModifier.Ctrl) when !line.MustObfuscate => HandleIntellisense(line, intellisense.NextValue, ""),
                (ConsoleKey.Tab, KeyModifier.Shift) when !line.MustObfuscate => HandleIntellisense(line, intellisense.PreviousValue, line.IntellisenseHint),
                (ConsoleKey.Tab, KeyModifier.CtrlShift) when !line.MustObfuscate => HandleIntellisense(line, intellisense.PreviousValue, ""),
                (ConsoleKey.UpArrow, KeyModifier.None) when !line.MustObfuscate => HandleHistory(line, scopedHistory.MoveToPreviousAndGet),
                (ConsoleKey.DownArrow, KeyModifier.None) when !line.MustObfuscate => HandleHistory(line, scopedHistory.MoveToNextAndGet),
                (ConsoleKey.Home, KeyModifier.None) => line with { CursorIndex = 0 },
                (ConsoleKey.LeftArrow, KeyModifier.None) => TryMoveCursor(line, -1),
                (ConsoleKey.LeftArrow, KeyModifier.Ctrl) => TryMoveCursor(line, GetCtrlLeftArrowDelta(line)),
                (ConsoleKey.RightArrow, KeyModifier.None) => TryMoveCursor(line, 1),
                (ConsoleKey.RightArrow, KeyModifier.Ctrl) => TryMoveCursor(line, GetCtrlRightArrowDelta(line)),
                (ConsoleKey.End, KeyModifier.None) => line with { CursorIndex = line.Length },
                (ConsoleKey.Backspace, KeyModifier.None) => TryBackspace(line, 1),
                (ConsoleKey.Backspace, KeyModifier.Ctrl) => TryBackspace(line, -GetCtrlLeftArrowDelta(line)),
                (ConsoleKey.Delete, KeyModifier.None) => TryDelete(line, 1),
                (ConsoleKey.Delete, KeyModifier.Ctrl) => TryDelete(line, GetCtrlRightArrowDelta(line)),
                (ConsoleKey.Escape, KeyModifier.None) => line with { Value = "", IntellisenseHint = "", CursorIndex = 0 },
                _ => line,
            };
        }

        private static InProgressLine TryMoveCursor(InProgressLine line, int delta)
        {
            var cursorIndex = line.CursorIndex + delta;
            return cursorIndex < 0 || cursorIndex > line.Length
                ? line
                : line with
                {
                    CursorIndex = cursorIndex,
                };
        }

        private static InProgressLine TryDelete(InProgressLine line, int amountToRemove)
        {
            if (line.Length < line.CursorIndex + amountToRemove)
            {
                return line;
            }

            var value = line.Value.Remove(line.CursorIndex, amountToRemove);
            return line with
            {
                Value = value,
                IntellisenseHint = value,
            };
        }

        private static InProgressLine TryBackspace(InProgressLine line, int amountToRemove)
        {
            var cursorIndex = line.CursorIndex - amountToRemove;
            if (cursorIndex < 0)
            {
                return line;
            }

            var value = line.Value.Remove(cursorIndex, amountToRemove);
            return line with
            {
                Value = value,
                IntellisenseHint = value,
                CursorIndex = cursorIndex,
            };
        }

        private static int GetCtrlLeftArrowDelta(InProgressLine line)
        {
            var displayValue = line.DisplayValue;
            var toInspectIndex = line.CursorIndex - 1;
            var hasFoundRelevantSpace = false;
            var hasFoundDigit = false;
            var delta = 0;

            while (toInspectIndex >= 0)
            {
                var toInspectIsSpace = displayValue[toInspectIndex] == ' ';
                hasFoundRelevantSpace = hasFoundRelevantSpace || (hasFoundDigit && toInspectIsSpace);
                hasFoundDigit = hasFoundDigit || !toInspectIsSpace;
                if (hasFoundRelevantSpace && hasFoundDigit)
                {
                    return delta;
                }

                delta--;
                toInspectIndex--;
            }

            return delta;
        }

        private static int GetCtrlRightArrowDelta(InProgressLine line)
        {
            var toInspectIndex = line.CursorIndex;
            var hasFoundSpace = false;
            var hasFoundRelevantDigit = false;
            var delta = 0;

            while (toInspectIndex < line.Length)
            {
                var toInspectIsSpace = line.DisplayValue[toInspectIndex] == ' ';
                hasFoundSpace = hasFoundSpace || toInspectIsSpace;
                hasFoundRelevantDigit = hasFoundRelevantDigit || (hasFoundSpace && !toInspectIsSpace);
                if (hasFoundSpace && hasFoundRelevantDigit)
                {
                    return delta;
                }

                toInspectIndex++;
                delta++;
            }

            return delta;
        }

        private static InProgressLine HandleHistory(InProgressLine line, Func<string?> fetcher)
        {
            var valueFromHistory = fetcher();
            return valueFromHistory is null
                ? line
                : line with
                {
                    Value = valueFromHistory,
                    IntellisenseHint = valueFromHistory,
                    CursorIndex = valueFromHistory.Length,
                };
        }

        private static bool IsRegularInput(KeyInput keyInput)
            => keyInput.KeyChar.CanBeTypedInConsoleAsksFor() &&
               keyInput.Modifier is KeyModifier.None or KeyModifier.Shift;
    }
}