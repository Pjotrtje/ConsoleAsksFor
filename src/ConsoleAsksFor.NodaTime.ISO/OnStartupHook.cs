using ConsoleAsksFor.Sdk;

using NodaTime;

namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class OnStartupHook : IOnStartupHook
{
    public void Initialize()
    {
        RangeConstraintComparers.RegisterComparer(ZonedDateTime.Comparer.Instant);
    }
}