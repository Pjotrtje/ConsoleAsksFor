namespace ConsoleAsksFor;

internal sealed class OnStartupHook : IOnStartupHook
{
    public void Initialize()
    {
        CircularRanges.RegisterCircularRange<TimeOnly>();
    }
}