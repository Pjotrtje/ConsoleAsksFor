namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class OnStartupHook : IOnStartupHook
{
    public void Initialize()
    {
        RangeConstraintComparers.RegisterComparer(ZonedDateTime.Comparer.Instant);
        CircularRanges.RegisterCircularRange<LocalTime>();
        CircularRanges.RegisterCircularRange<AnnualDate>();
    }
}