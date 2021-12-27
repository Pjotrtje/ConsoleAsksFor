namespace ConsoleAsksFor.TestUtils;

/// <summary>
/// Class for adding multiple input for <see cref="IConsoleInputGetter.ReadKeyWhileBlinkLine" />.<br/>
/// Wrapper around <see cref="IEnumerable" />&lt;<see cref="KeyInput" />&gt; which accepts both <see cref="KeyInput" /> and <see cref="string" />.
/// </summary>
public class KeyInputItems : IEnumerable<KeyInput>
{
    private readonly List<KeyInput> _keyInputs = new();

    public void Add(KeyInput keyInput)
        => _keyInputs.Add(keyInput);

    public void Add(string text)
    {
        var keyInputs = text.Select(KeyInputs.FromChar);
        _keyInputs.AddRange(keyInputs);
    }

    public IEnumerator<KeyInput> GetEnumerator()
        => _keyInputs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}