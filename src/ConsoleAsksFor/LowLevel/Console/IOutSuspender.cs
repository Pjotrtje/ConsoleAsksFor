namespace ConsoleAsksFor;

internal interface IOutSuspender
{
    IDisposable Suspend();
}