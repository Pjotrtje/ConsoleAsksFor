namespace ConsoleAsksFor.Sdk;

/// <summary>
/// Key Modifier for ReadKey.
/// </summary>
public enum KeyModifier
{
    /// <summary>
    /// No <see cref="ConsoleModifiers"/>.
    /// </summary>
    None = 0,

    /// <summary>
    /// <inheritdoc cref="ConsoleModifiers.Shift"/>
    /// </summary>
    Shift = 1,

    /// <summary>
    /// <inheritdoc cref="ConsoleModifiers.Control"/>
    /// </summary>
    Ctrl = 2,

    /// <summary>
    /// Combination of: <br/>
    /// <inheritdoc cref="ConsoleModifiers.Control"/><br/>
    /// <inheritdoc cref="ConsoleModifiers.Shift"/><br/>
    /// </summary>
    CtrlShift = 3,

    /// <summary>
    /// Combination of <see cref="ConsoleModifiers"/> not listed above which are ignored.
    /// </summary>
    IrrelevantCombination = 4,
}