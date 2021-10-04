using System.Text;

namespace ConsoleAsksFor
{
    internal interface IDirectOut
    {
        void Write(char value);

        void Write(string? value);

        void WriteLine(string? value);

        public Encoding Encoding { get; }
    }
}