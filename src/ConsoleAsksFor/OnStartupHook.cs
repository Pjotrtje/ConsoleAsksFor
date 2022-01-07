namespace ConsoleAsksFor;

internal sealed class OnStartupHook : IOnStartupHook
{
    public void Initialize()
    {
#if NET6_0_OR_GREATER
        CircularRanges.RegisterCircularRange<TimeOnly>();
#endif
    }
}