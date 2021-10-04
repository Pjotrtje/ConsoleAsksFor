using System;

namespace ConsoleAsksFor
{
    /// <summary>
    /// Foreground and background colors for line.
    /// </summary>
    public sealed record LineColor
    {
        private const string UndoForegroundColor = "\x1B[39m";
        private const string UndoBackgroundColor = "\x1B[49m";

        /// <summary>
        /// The foreground color of the line.
        /// </summary>
        public ConsoleColor Foreground { get; }

        /// <summary>
        /// The background color of the line.
        /// </summary>
        public ConsoleColor Background { get; }

        private readonly string _backgroundColorCode;
        private readonly string _foregroundColorCode;

        /// <summary>
        /// Creates a new LineColor.
        /// </summary>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        public LineColor(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;

            _foregroundColorCode = GetForegroundColorEscapeCode(foreground);
            _backgroundColorCode = GetBackgroundColorEscapeCode(background);
        }

        internal string Colorize(string? value)
            => $"{_foregroundColorCode}{_backgroundColorCode}{value}{UndoForegroundColor}{UndoBackgroundColor}";

        private static string GetForegroundColorEscapeCode(ConsoleColor color)
            => color switch
            {
                ConsoleColor.Black => "\x1B[30m",
                ConsoleColor.DarkRed => "\x1B[31m",
                ConsoleColor.DarkGreen => "\x1B[32m",
                ConsoleColor.DarkYellow => "\x1B[33m",
                ConsoleColor.DarkBlue => "\x1B[34m",
                ConsoleColor.DarkMagenta => "\x1B[35m",
                ConsoleColor.DarkCyan => "\x1B[36m",
                ConsoleColor.Gray => "\x1B[37m",
                ConsoleColor.DarkGray => "\x1B[90m",
                ConsoleColor.Red => "\x1B[91m",
                ConsoleColor.Green => "\x1B[92m",
                ConsoleColor.Yellow => "\x1B[93m",
                ConsoleColor.Blue => "\x1B[94m",
                ConsoleColor.Magenta => "\x1B[95m",
                ConsoleColor.Cyan => "\x1B[96m",
                ConsoleColor.White => "\x1B[97m",
                _ => UndoForegroundColor,
            };

        private static string GetBackgroundColorEscapeCode(ConsoleColor color)
            => color switch
            {
                ConsoleColor.Black => "\x1B[40m",
                ConsoleColor.DarkRed => "\x1B[41m",
                ConsoleColor.DarkGreen => "\x1B[42m",
                ConsoleColor.DarkYellow => "\x1B[43m",
                ConsoleColor.DarkBlue => "\x1B[44m",
                ConsoleColor.DarkMagenta => "\x1B[45m",
                ConsoleColor.DarkCyan => "\x1B[46m",
                ConsoleColor.Gray => "\x1B[47m",
                ConsoleColor.DarkGray => "\x1B[100m",
                ConsoleColor.Red => "\x1B[101m",
                ConsoleColor.Green => "\x1B[102m",
                ConsoleColor.Yellow => "\x1B[103m",
                ConsoleColor.Blue => "\x1B[104m",
                ConsoleColor.Magenta => "\x1B[105m",
                ConsoleColor.Cyan => "\x1B[106m",
                ConsoleColor.White => "\x1B[107m",
                _ => UndoBackgroundColor,
            };
    }
}