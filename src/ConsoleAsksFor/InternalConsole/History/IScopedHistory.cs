namespace ConsoleAsksFor
{
    internal interface IScopedHistory
    {
        string? MoveToNextAndGet();
        string? MoveToPreviousAndGet();
    }
}