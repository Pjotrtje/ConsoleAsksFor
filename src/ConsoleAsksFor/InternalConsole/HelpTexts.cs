namespace ConsoleAsksFor;

internal static class HelpTexts
{
    public static readonly IReadOnlyCollection<string> Lines =
    [
        "Shortcuts:",
        "  F1: Help",
        $"  F2: Toggle history type for current question; {HistoryType.ByQuestionTextAndType}/{HistoryType.ByQuestionType}/{HistoryType.NotFiltered}",
        "  F3: Toggle obfuscate answer for current question (if applicable)",
        "  F4: Repeat question",
        $"  F12: Cancel input by throwing {nameof(TaskCanceledByF12Exception)} (derived from {nameof(TaskCanceledException)})",
        "  Up: Previous history",
        "  Down: Next History",
        "  Left: Move cursor left",
        "  Ctrl+Left: Move cursor to begin previous word",
        "  Right: Move cursor right",
        "  Ctrl+Right: Move cursor to begin next word",
        "  Home: Move cursor to beginning of line",
        "  End: Move cursor to end of line",
        "  Backspace: Delete previous character",
        "  Ctrl+Backspace: Delete to begin previous word",
        "  Delete: Delete next character",
        "  Ctrl+Delete: Delete to begin next word",
        "  Escape: Clear line",
        "  Tab: Try complete answer; Try select 'next/greater' relevant answer (respecting your typed characters)",
        "  Ctrl+Tab: Try complete answer; Try select 'next/greater' relevant answer (ignoring your typed characters)",
        "  Shift+Tab: Try complete answer; Try select 'previous/smaller' relevant answer (respecting your typed characters)",
        "  Ctrl+Shift+Tab: Try complete answer; Try select 'previous/smaller' relevant answer (ignoring your typed characters)",
        "  Ctrl+Space: Try complete answer",
    ];
}