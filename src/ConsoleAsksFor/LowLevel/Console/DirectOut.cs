using System.IO;
using System.Text;

namespace ConsoleAsksFor;

internal sealed class DirectOut : IDirectOut, IDirectOutWriter
{
    private readonly TextWriter _textWriter;

    public DirectOut(TextWriter textWriter)
        => _textWriter = textWriter;

    public void Write(char value)
        => _textWriter.Write(value);

    public void Write(string? value)
        => _textWriter.Write(value);

    public void WriteLine(string? value)
        => _textWriter.WriteLine(value);

    public Encoding Encoding
        => _textWriter.Encoding;
}