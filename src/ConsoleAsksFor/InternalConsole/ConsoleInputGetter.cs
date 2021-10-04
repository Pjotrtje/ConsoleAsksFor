using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class ConsoleInputGetter : IConsoleInputGetter
    {
        private readonly IOutSuspender _outSuspender;
        private readonly ISystemConsole _systemConsole;
        private readonly IDirectOutWriter _directOutWriter;
        private readonly TimeSpan _flushDelay;
        private readonly LineTypes _lineTypes;

        private static readonly LineColor NoColor = new(ConsoleColor.Black, ConsoleColor.Black);

        public ConsoleInputGetter(
            IOutSuspender outSuspender,
            ISystemConsole systemConsole,
            IDirectOutWriter directOutWriter,
            LineTypes lineTypes,
            int onIdleKeyPressFlushOutEverySeconds)
        {
            _outSuspender = outSuspender;
            _systemConsole = systemConsole;
            _directOutWriter = directOutWriter;
            _lineTypes = lineTypes;
            _flushDelay = TimeSpan.FromSeconds(onIdleKeyPressFlushOutEverySeconds);
        }

        public async Task<KeyInput> ReadKeyWhileBlinkLine(
            InProgressLine line,
            bool currentLineIsValid,
            CancellationToken cancellationToken)
        {
            var lineType = currentLineIsValid
                ? _lineTypes.Answer
                : _lineTypes.InvalidAnswer;

            while (true)
            {
                using (_outSuspender.Suspend())
                {
                    Write(lineType.Color, line);

                    try
                    {
                        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        linkedCts.CancelAfter(_flushDelay);

                        var cki = await _systemConsole.ReadKey(linkedCts.Token);
                        return ToKeyInput(cki);
                    }
                    catch (TaskCanceledException)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        UndoWrite(line);
                    }
                }
            }
        }

        private void Write(LineColor color, InProgressLine line)
            => RefreshLine(color, line.DisplayValue, 0, line.CursorIndex);

        private void UndoWrite(InProgressLine line)
        {
            var value = new string(' ', line.Length);
            RefreshLine(NoColor, value, line.CursorIndex, 0);
        }

        private void RefreshLine(LineColor color, string value, int cursorIndex, int newCursorIndex)
        {
            var lineStartPosition = GetLineStartPosition(_systemConsole.CursorPosition, cursorIndex);
            var newCursorPosition = GetNewCursorPosition(lineStartPosition, newCursorIndex);

            _systemConsole.CursorVisible = false;
            _systemConsole.CursorPosition = lineStartPosition;

            _directOutWriter.Write(color.Colorize(value));

            _systemConsole.CursorPosition = newCursorPosition;
            _systemConsole.CursorVisible = true;
        }

        private Position GetLineStartPosition(
            Position originalCursorPosition,
            int cursorIndex)
        {
            var quotient = Math.DivRem(cursorIndex, _systemConsole.WindowWidth, out var remainder);
            return new(
                originalCursorPosition.Left - remainder,
                originalCursorPosition.Top - quotient);
        }

        private Position GetNewCursorPosition(
            Position lineStartPosition,
            int newCursorIndex)
        {
            var quotient = Math.DivRem(newCursorIndex, _systemConsole.WindowWidth, out var remainder);
            return new(
                lineStartPosition.Left + remainder,
                lineStartPosition.Top + quotient);
        }

        private static KeyInput ToKeyInput(ConsoleKeyInfo cki)
        {
            var modifier = ToKeyModifier(cki);
            return new(modifier, cki.Key, cki.KeyChar);
        }

        private static KeyModifier ToKeyModifier(ConsoleKeyInfo cki)
            => cki.Modifiers switch
            {
                0 => KeyModifier.None,
                ConsoleModifiers.Control | ConsoleModifiers.Shift => KeyModifier.CtrlShift,
                ConsoleModifiers.Shift => KeyModifier.Shift,
                ConsoleModifiers.Control => KeyModifier.Ctrl,
                _ => KeyModifier.IrrelevantCombination,
            };
    }
}